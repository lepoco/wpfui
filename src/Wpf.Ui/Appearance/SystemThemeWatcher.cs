// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Microsoft.Win32;
using Windows.Win32;
using Wpf.Ui.Controls;
using Wpf.Ui.Interop;

namespace Wpf.Ui.Appearance;

/// <summary>
/// Automatically updates the application background if the system theme or color is changed.
/// <para><see cref="SystemThemeWatcher"/> settings work globally and cannot be changed for each <see cref="System.Windows.Window"/>.</para>
/// </summary>
/// <example>
/// <code lang="csharp">
/// SystemThemeWatcher.Watch(this as System.Windows.Window);
/// SystemThemeWatcher.UnWatch(this as System.Windows.Window);
/// </code>
/// <code lang="csharp">
/// SystemThemeWatcher.Watch(
///     _serviceProvider.GetRequiredService&lt;MainWindow&gt;()
/// );
/// </code>
/// </example>
public static class SystemThemeWatcher
{
    private static readonly List<ObservedWindow> _observedWindows = [];
    private static readonly TimeSpan _unlockIgnoreDuration = TimeSpan.FromSeconds(5);
    private static ApplicationTheme? _lastAppliedTheme;
    private static Color? _lastAppliedAccentColor;
    private static bool _sessionSwitchHandlerRegistered;
    private static DateTime _lastUnlockTime = DateTime.MinValue;
    private static bool _isRestoringTheme = false;

    /// <summary>
    /// Watches the <see cref="Window"/> and applies the background effect and theme according to the system theme.
    /// </summary>
    /// <param name="window">The window that will be updated.</param>
    /// <param name="backdrop">Background effect to be applied when changing the theme.</param>
    /// <param name="updateAccents">If <see langword="true"/>, the accents will be updated when the change is detected.</param>
    public static void Watch(
        Window? window,
        WindowBackdropType backdrop = WindowBackdropType.Mica,
        bool updateAccents = true
    )
    {
        if (window is null)
        {
            return;
        }

        if (window.IsLoaded)
        {
            ObserveLoadedWindow(window, backdrop, updateAccents);
        }
        else
        {
            ObserveWindowWhenLoaded(window, backdrop, updateAccents);
        }

        if (_observedWindows.Count == 0)
        {
            ApplicationThemeManager.ApplySystemTheme(updateAccents);
            StoreCurrentThemeAndAccent();
        }

        if (!_sessionSwitchHandlerRegistered)
        {
            SystemEvents.SessionSwitch += OnSessionSwitch;
            ApplicationThemeManager.Changed += OnThemeChanged;
            _sessionSwitchHandlerRegistered = true;
        }
    }

    private static void ObserveLoadedWindow(Window window, WindowBackdropType backdrop, bool updateAccents)
    {
        IntPtr hWnd =
            (hWnd = new WindowInteropHelper(window).Handle) == IntPtr.Zero
                ? throw new InvalidOperationException("Could not get window handle.")
                : hWnd;

        if (hWnd == IntPtr.Zero)
        {
            throw new InvalidOperationException("Window handle cannot be empty");
        }

        ObserveLoadedHandle(new ObservedWindow(hWnd, backdrop, updateAccents));
    }

    private static void ObserveWindowWhenLoaded(
        Window window,
        WindowBackdropType backdrop,
        bool updateAccents
    )
    {
        window.Loaded += (_, _) =>
        {
            IntPtr hWnd =
                (hWnd = new WindowInteropHelper(window).Handle) == IntPtr.Zero
                    ? throw new InvalidOperationException("Could not get window handle.")
                    : hWnd;

            if (hWnd == IntPtr.Zero)
            {
                throw new InvalidOperationException("Window handle cannot be empty");
            }

            ObserveLoadedHandle(new ObservedWindow(hWnd, backdrop, updateAccents));
        };
    }

    private static void ObserveLoadedHandle(ObservedWindow observedWindow)
    {
        if (!observedWindow.HasHook)
        {
            observedWindow.AddHook(WndProc);
            _observedWindows.Add(observedWindow);
        }
    }

    /// <summary>
    /// Unwatches the window and removes the hook to receive messages from the system.
    /// </summary>
    public static void UnWatch(Window? window)
    {
        if (window is null)
        {
            return;
        }

        if (!window.IsLoaded)
        {
            throw new InvalidOperationException("You cannot unwatch a window that is not yet loaded.");
        }

        IntPtr hWnd =
            (hWnd = new WindowInteropHelper(window).Handle) == IntPtr.Zero
                ? throw new InvalidOperationException("Could not get window handle.")
                : hWnd;

        ObservedWindow? observedWindow = _observedWindows.FirstOrDefault(x => x.Handle == hWnd);

        if (observedWindow is null)
        {
            return;
        }

        observedWindow.RemoveHook(WndProc);

        _ = _observedWindows.Remove(observedWindow);
    }

    /// <summary>
    /// Listens to system messages on the application windows.
    /// </summary>
    private static IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        if (msg == (int)PInvoke.WM_WININICHANGE && !_isRestoringTheme)
        {
            DateTime now = DateTime.Now;
            if (now - _lastUnlockTime > _unlockIgnoreDuration)
            {
                UpdateObservedWindow(hWnd);
            }
        }

        return IntPtr.Zero;
    }

    private static void UpdateObservedWindow(nint hWnd)
    {
        if (!UnsafeNativeMethods.IsValidWindow(hWnd))
        {
            return;
        }

        ObservedWindow? observedWindow = _observedWindows.FirstOrDefault(x => x.Handle == hWnd);
        if (observedWindow is null)
        {
            return;
        }

        SystemTheme currentSystemTheme = ApplicationThemeManager.GetSystemTheme();
        ApplicationTheme systemThemeAsAppTheme = ConvertSystemThemeToApplicationTheme(currentSystemTheme);

        // Check if stored theme is different from what system theme would be
        if (_lastAppliedTheme.HasValue && 
            _lastAppliedTheme.Value != ApplicationTheme.Unknown &&
            _lastAppliedTheme.Value != systemThemeAsAppTheme)
        {
            // Preserve stored theme
            WindowBackdropType backdrop = _observedWindows.Count > 0
                ? _observedWindows[0].Backdrop
                : WindowBackdropType.Mica;

            ApplicationThemeManager.Apply(_lastAppliedTheme.Value, backdrop, false);

            if (_lastAppliedAccentColor.HasValue && _lastAppliedAccentColor.Value != Colors.Transparent)
            {
                ApplicationAccentColorManager.Apply(_lastAppliedAccentColor.Value, _lastAppliedTheme.Value, false);
            }

            if (observedWindow.RootVisual is { } window)
            {
                WindowBackgroundManager.UpdateBackground(window, _lastAppliedTheme.Value, observedWindow.Backdrop);
            }
        }
        else
        {
            // Normal system theme update
            ApplicationThemeManager.ApplySystemTheme(observedWindow.UpdateAccents);
            StoreCurrentThemeAndAccent();

            ApplicationTheme newAppTheme = ApplicationThemeManager.GetAppTheme();
            if (observedWindow.RootVisual is { } window)
            {
                WindowBackgroundManager.UpdateBackground(window, newAppTheme, observedWindow.Backdrop);
            }
        }
    }

    /// <summary>
    /// Converts a system theme to an application theme.
    /// </summary>
    private static ApplicationTheme ConvertSystemThemeToApplicationTheme(SystemTheme systemTheme)
    {
        return systemTheme switch
        {
            SystemTheme.Dark or SystemTheme.CapturedMotion or SystemTheme.Glow => ApplicationTheme.Dark,
            SystemTheme.HC1 or SystemTheme.HC2 or SystemTheme.HCBlack or SystemTheme.HCWhite => ApplicationTheme.HighContrast,
            _ => ApplicationTheme.Light
        };
    }

    /// <summary>
    /// Stores the current theme and accent color for restoration after session unlock.
    /// </summary>
    private static void StoreCurrentThemeAndAccent()
    {
        _lastAppliedTheme = ApplicationThemeManager.GetAppTheme();
        _lastAppliedAccentColor = ApplicationAccentColorManager.SystemAccent;
    }

    /// <summary>
    /// Handles session switch events (lock/unlock) to restore theme and accent color.
    /// </summary>
    private static void OnSessionSwitch(object sender, SessionSwitchEventArgs e)
    {
        if (e.Reason == SessionSwitchReason.SessionUnlock)
        {
            _lastUnlockTime = DateTime.Now;

            // Restore theme if we have a stored value
            if (_lastAppliedTheme.HasValue && _lastAppliedTheme.Value != ApplicationTheme.Unknown)
            {
                _isRestoringTheme = true;

                // Use the backdrop from the first observed window, or default to Mica
                WindowBackdropType backdrop = _observedWindows.Count > 0
                    ? _observedWindows[0].Backdrop
                    : WindowBackdropType.Mica;

                ApplicationThemeManager.Apply(
                    _lastAppliedTheme.Value,
                    backdrop,
                    false // Don't update accent yet, we'll do it separately
                );

                // Restore accent color if we have a stored value
                if (_lastAppliedAccentColor.HasValue && _lastAppliedAccentColor.Value != Colors.Transparent)
                {
                    ApplicationAccentColorManager.Apply(
                        _lastAppliedAccentColor.Value,
                        _lastAppliedTheme.Value,
                        false
                    );
                }
                else
                {
                    // Fallback to system accent color
                    ApplicationAccentColorManager.ApplySystemAccent();
                }

                // Update all observed windows
                foreach (ObservedWindow observedWindow in _observedWindows)
                {
                    if (observedWindow.RootVisual is { } window)
                    {
                        WindowBackgroundManager.UpdateBackground(window, _lastAppliedTheme.Value, observedWindow.Backdrop);
                    }
                }

                // Store the restored theme to prevent it from being overwritten
                StoreCurrentThemeAndAccent();

                // Give the UI time to update before allowing system theme changes
                _ = System.Threading.Tasks.Task.Run(async () =>
                {
                    await System.Threading.Tasks.Task.Delay(100);
                    _isRestoringTheme = false;
                });
            }
            else
            {
                // If we don't have a stored theme, apply system theme
                ApplicationThemeManager.ApplySystemTheme(true);
                StoreCurrentThemeAndAccent();
            }
        }
        else if (e.Reason == SessionSwitchReason.SessionLock)
        {
            // Store current theme and accent before lock
            StoreCurrentThemeAndAccent();
        }
    }

    /// <summary>
    /// Handles theme change events to keep the stored theme up to date.
    /// </summary>
    private static void OnThemeChanged(ApplicationTheme theme, Color accentColor)
    {
        // Update stored theme if we're restoring or if we're past the unlock ignore period
        if (_isRestoringTheme || DateTime.Now - _lastUnlockTime > _unlockIgnoreDuration)
        {
            _lastAppliedTheme = theme;
            _lastAppliedAccentColor = accentColor;
        }
    }
}
