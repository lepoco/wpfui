// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Specialized;   // NameValueCollection
using System.Configuration;             // ConfigurationManager
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;

// using MS.Win32;
// using MS.Internal;

namespace WPFUI.Win32;

// Implementation note:
//
// The theme state is kept in three variables:  a bool _isActive and two
// strings _themeName and _themeColor (the latter two are treated equally
// in this discussion, so we'll mention only _themeName for conciseness).
// There are three interesting states:
//  I (Inactive): _isActive=false, _themeName=null
//  AU (Active/Uninitialized): _isActive=true, _themeName=null
//  AI (Active/Initialized): _isActive=true, _themeName=non-null
//
// The original implementation handled OS theme-change notifications by
// transitioning to state I or AU, depending on the result of calling
// the OS function IsThemeActive (and the HighContrast state).
// In the AU case, the next query to ThemeName moved to state AI
// using the result of another OS function GetCurrentThemeName.
//
// A previous change exposed a race condition that occurs when there are two
// windows running in different threads.  An OS change to an "active"
// theme sends notifications to both threads, and they both try to change
// state:  AI -> AU -> AI.  If thread B's first transition occurs in the
// middle of thread A's second transition, thread A's get_ThemeName can
// return null, which crashes the app.
//
// [The opportunities for this race to end badly increased as of Win8.
// Reconnecting to an RDP session now sends WM_THEMECHANGED to all windows.]
//
// The current implementation avoids the race condition without using locks
// (thus dodging questions of deadlock or re-entrancy), while keeping the
// same pattern of OS calls (thus dodging questions of performance or use
// of OS services).  The key points are:
//  1. Update the state atomically.  The state is stored as a read-only
//      class with three members (rather than as three variables), and
//      updated by installing a new instance of the class.
//  2. Use the state atomically.  Set a local variable to refer to the
//      state, and use that reference (rather than re-fetching the static
//      member, which another thread might change).
//  3. Avoid the pattern AI -> AU -> AI.  If we had initialized the name
//      of the old theme, there will almost certainly be a request for
//      the name of the new theme, so obtaining the name during the
//      theme change (rather than waiting for the ThemeName request)
//      entails no wasted effort.  This avoids the problematic intermediate
//      state where _themeName is null, except when switching between
//      active and inactive themes.
//  4. When two threads try to install a new state simultaneously, ensure
//      that only one actually does so.  Usually this happens when both
//      threads are reacting to the same OS theme change and thus are trying
//      to make the same transition.  In that case the losing thread can
//      simply use the state installed by the winning thread.
//  5. In theory, there's a more complicated race condition that might occur,
//      as illustrated by this (hypothetical) timeline:
//          OS                  Thread A                Thread B
//      change to theme X
//      broadcast WM_THEMECHANGE(1)
//                              OnThemeChanged(1)
//                              get data from OS
//                              install new state
//      change to theme Y
//      broadcast WM_THEMECHANGE(2)
//                              OnThemeChanged(2)
//                              get data from OS
//                              install new state
//                                                      OnThemeChanged(1)
//                                                      get data from OS
//      change to theme Z
//      broadcast WM_THEMECHANGE(3)
//                              OnThemeChanged(3)
//                              get data from OS
//                                                      install new state
//                              try install new state
//
//      Thread A knows that thread B won the race, but it doesn't know whether
//      the installed state is valid.   In this picture it's not - thread
//      B installed a state referring to theme Y.  But in the same picture
//      with the last two lines reversed, thread B is the loser but the
//      state installed by the winner is valid - thread A's state refers to
//      theme Z.
//
//      This situation is unlikely, perhaps even impossible (limited attempts
//      failed to cause it).  And the original implementation has the same
//      problem.  Yet ignoring it is pretty bad - in the above picture thread
//      B will eventually get the two pending WM_THEMECHANGEs and change the
//      state to "theme Z", but until it does thread A will render everything
//      according to theme Y, and will never have any reason to change that.
//
//      The current implementation handles this situation by restarting
//      thread A's update process from the beginning, as if it got a fresh
//      OnThemeChanged.  This guarantees that thread A always uses the latest
//      theme data, but it does raise the possibility of starvation - thread
//      A can get stuck in the update loop if other threads always sneak
//      their own updates into the window between A's attempts to install
//      its state.  This would only happen if the OS were raising a continuous
//      stream of WM_THEMECHANGEs (real ones, that actually change the theme),
//      in which case starvation of thread A is the least of your worries.
//
//      Note that no amount of locking would help this situation.  You'd need
//      to lock around the OS actions of changing the theme data and broadcasting
//      WM_THEMECHANGE.   The OS does not provide that kind of locking.

/// <summary>
///     Wrapper class for loading UxTheme system theme data
/// </summary>
internal static class UxThemeWrapper
{
    static UxThemeWrapper()
    {
        // When in high contrast we want to force the WPF theme to be Classic
        // because that is the only WPF theme that has full fidelity support for the
        // high contrast color scheme. Prior to Win8 the OS automatically switched
        // to Classic theme when in high contrast, but Win8 onwards apps that claim
        // Win8 OS support via the app.manifest will receive the AeroLite as the theme
        // when in high contrast. The reason being the OS team wants to give high
        // contrast a face lift starting Win8. However, WPF isnt setup to support this
        // currently. Thus we fallback to Classic in this situation.

        _themeState = new ThemeState(!SystemParameters.HighContrast && SafeNativeMethods.IsUxThemeActive(), null, null);
    }

    internal static bool IsActive
    {
        get
        {
            return IsActiveCompatWrapper;
        }
    }

    internal static string ThemeName
    {
        get
        {
            return ThemeNameCompatWrapper;
        }
    }

    internal static string ThemeColor
    {
        get
        {
            return ThemeColorCompatWrapper;
        }
    }

    internal static string ThemedResourceName
    {
        get
        {
            return ThemedResourceNameCompatWrapper;
        }
    }

    private static ThemeState EnsureThemeState(bool themeChanged)
    {
        ThemeState themeState = _themeState;    // capture latest state
        bool needName = !themeChanged;
        string themeName, themeColor;

        bool needUpdate = true;
        while (needUpdate)
        {
            // compute the desired new state
            ThemeState newState;

            if (themeChanged)
            {
                // when called in response to a ThemeChange, reevaluate
                // the full state

                // Please refer to elaborate comment about high
                // contrast in the static constructor
                bool isActive = !SystemParameters.HighContrast && SafeNativeMethods.IsUxThemeActive();

                if (isActive && (needName || themeState.ThemeName != null))
                {
                    // avoid AI -> AU -> AI transition;   get theme name now
                    // (see implementation note 3)
                    GetThemeNameAndColor(out themeName, out themeColor);
                }
                else
                {
                    themeName = themeColor = null;
                }

                newState = new ThemeState(isActive, themeName, themeColor);
            }
            else
            {
                // when called outside of ThemeChange, only update if we
                // need the name of an active theme
                if (themeState.IsActive && themeState.ThemeName == null)
                {
                    GetThemeNameAndColor(out themeName, out themeColor);
                    newState = new ThemeState(themeState.IsActive, themeName, themeColor);
                }
                else
                {
                    newState = themeState;
                    needUpdate = false;
                }
            }

            if (needUpdate)
            {
                // try to update the state
                ThemeState currentState = System.Threading.Interlocked.CompareExchange(ref _themeState, newState, themeState);

                if (currentState == themeState)
                {
                    // the update worked, we're done
                    themeState = newState;
                    needUpdate = false;
                }
                else if (currentState.IsActive == newState.IsActive &&
                            (!newState.IsActive ||
                                newState.ThemeName == null ||
                                currentState.ThemeName != null)
                        )
                {
                    // another thread updated the state while we were in this
                    // method, but installed a state that's as good or better
                    // than the one we wanted.   Accept the other update.
                    themeState = currentState;
                    needUpdate = false;
                }
                else
                {
                    // another thread updated the state to something different
                    // from what we wanted.  This implies the other thread
                    // was responding to a ThemeChange, but it might have been
                    // an older ThemeChange, in which case the state isn't
                    // valid.  To be safe, start over as if responding
                    // to a ThemeChange.
                    themeChanged = true;
                    themeState = currentState;
                }
            }
        }

        return themeState;
    }

    private static void GetThemeNameAndColor(out string themeName, out string themeColor)
    {
        StringBuilder themeNameSB = new StringBuilder(Win32Constant.MAX_PATH);
        StringBuilder themeColorSB = new StringBuilder(Win32Constant.MAX_PATH);

        if (UnsafeNativeMethods.GetCurrentThemeName(themeNameSB, themeNameSB.Capacity,
                                                    themeColorSB, themeColorSB.Capacity,
                                                    null, 0) == 0)
        {
            // Success
            themeName = themeNameSB.ToString();
            themeName = Path.GetFileNameWithoutExtension(themeName);

            if (String.Compare(themeName, "aero", StringComparison.OrdinalIgnoreCase) == 0 && Utilities.IsOSWindows8OrNewer)
            {
                themeName = "Aero2";
            }

#if DEBUG
#if NET6_0_OR_GREATER
            // for debugging, config file can override the theme name
            NameValueCollection appSettings = null;
            try
            {
                appSettings = ConfigurationManager.AppSettings;
            }
            catch (ConfigurationErrorsException)
            {
            }

            if (appSettings != null)
            {
                string s = appSettings["ThemeNameOverride"];
                if (!String.IsNullOrEmpty(s))
                {
                    themeName = s;
                }
            }
#endif
#endif

            themeColor = themeColorSB.ToString();
        }
        else
        {
            // Failed to retrieve the name
            themeName = themeColor = String.Empty;
        }
    }

    internal static void OnThemeChanged()
    {
        RestoreSupportedState();
        EnsureThemeState(themeChanged: true);
    }

    private static ThemeState _themeState;

    private class ThemeState
    {
        public ThemeState(bool isActive, string name, string color)
        {
            _isActive = isActive;
            _themeName = name;
            _themeColor = color;
        }

        public bool IsActive { get { return _isActive; } }
        public string ThemeName { get { return _themeName; } }
        public string ThemeColor { get { return _themeColor; } }

        private bool _isActive;
        private string _themeName;
        private string _themeColor;
    }

    #region Compatibility

    // There are apps that override the system theme with one of their own
    // choosing, and intercept (and discard) WM_THEMECHANGED messages to
    // keep their theme in place even when the end-user selects a different
    // theme.   They do this using private reflection to assign values to
    // the three state variables.
    //
    // This state (i.e. where the three variables have values that differ from
    // the ones we choose) is unsupported.  So is the technique for getting
    // into that state (i.e. private reflection).  Nevertheless, .Net wants
    // to preserve some level of compatibility - at the very least, avoid
    // crashing.  [The apps use the result of GetField("_isActive") without
    // checking for null.]
    //
    // We do this in three steps:
    // 1) preserve the three fields;  this fixes the crashes.
    // 2) if the app overrides the values, use the overridden values
    //      in preference to ours.
    // 3) during WM_THEMECHANGED, restore the preference for our values.
    //      If the app overrides them again, step (2) will kick in.
    // Note that step (3) will never happen if the app is intercepting
    // WM_THEMECHANGED.

    private static bool _isActive;
    private static string _themeName;
    private static string _themeColor;

    private static bool IsAppSupported
    {
        // we never set _themeName to non-null, so it can only be non-null
        // if the app has set it by private reflection => unsupported
        get { return (_themeName == null); }
    }

    private static bool IsActiveCompatWrapper
    {
        get { return IsAppSupported ? _themeState.IsActive : _isActive; }
    }

    private static string ThemeNameCompatWrapper
    {
        get
        {
            if (IsAppSupported)
            {
                ThemeState themeState = EnsureThemeState(themeChanged: false);

                if (themeState.IsActive)
                {
                    return themeState.ThemeName;
                }
                else
                {
                    return "classic";
                }
            }
            else
            {
                return _themeName;
            }
        }
    }

    private static string ThemeColorCompatWrapper
    {
        get
        {
            if (IsAppSupported)
            {
                ThemeState themeState = EnsureThemeState(themeChanged: false);
                Debug.Assert(themeState.IsActive, "Queried ThemeColor while UxTheme is not active.");

                return themeState.ThemeColor;
            }
            else
            {
                return _themeColor;
            }
        }
    }

    private static string ThemedResourceNameCompatWrapper
    {
        get
        {
            if (IsAppSupported)
            {
                ThemeState themeState = EnsureThemeState(themeChanged: false);
                if (themeState.IsActive)
                {
                    return "themes/" + themeState.ThemeName.ToLowerInvariant() + "." + themeState.ThemeColor.ToLowerInvariant();
                }
                else
                {
                    return SystemResources.ClassicResourceName;
                }
            }
            else
            {
                if (_isActive)
                {
                    return "themes/" + _themeName.ToLowerInvariant() + "." + _themeColor.ToLowerInvariant();
                }
                else
                {
                    return SystemResources.ClassicResourceName;
                }
            }
        }
    }

    private static void RestoreSupportedState()
    {
        _isActive = false;
        _themeName = null;
        _themeColor = null;
    }

    #endregion Compatibility
}
