// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

// This Source Code is partially based on reverse engineering of the Windows Operating System,
// and is intended for use on Windows systems only.
// This Source Code is partially based on the source code provided by the .NET Foundation.
//
// NOTE:
// I split unmanaged code stuff into the NativeMethods library.
// If you have suggestions for the code below, please submit your changes there.
// https://github.com/lepoco/nativemethods
//
// Windows Kits\10\Include\10.0.22000.0\um\dwmapi.h

// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
#pragma warning disable SA1307 // Accessible fields should begin with upper-case letter
#pragma warning disable CA1060 // Move pinvokes to native methods class

using System.Runtime.InteropServices;

namespace Wpf.Ui.Interop;

/// <summary>
/// Desktop Window Manager (DWM).
/// </summary>
internal static class Dwmapi
{
    /// <summary>
    /// Flags used by the DwmSetWindowAttribute function to specify the rounded corner preference for a window.
    /// </summary>
    [Flags]
    public enum DWM_WINDOW_CORNER_PREFERENCE
    {
        DEFAULT = 0,
        DONOTROUND = 1,
        ROUND = 2,
        ROUNDSMALL = 3,
    }

    /// <summary>
    /// Backdrop types.
    /// </summary>
    [Flags]
    public enum DWMSBT : uint
    {
        /// <summary>
        /// Automatically selects backdrop effect.
        /// </summary>
        DWMSBT_AUTO = 0,

        /// <summary>
        /// Turns off the backdrop effect.
        /// </summary>
        DWMSBT_DISABLE = 1,

        /// <summary>
        /// Sets Mica effect with generated wallpaper tint.
        /// </summary>
        DWMSBT_MAINWINDOW = 2,

        /// <summary>
        /// Sets acrlic effect.
        /// </summary>
        DWMSBT_TRANSIENTWINDOW = 3,

        /// <summary>
        /// Sets blurred wallpaper effect, like Mica without tint.
        /// </summary>
        DWMSBT_TABBEDWINDOW = 4,
    }

    /// <summary>
    /// Options used by the DwmGetWindowAttribute and DwmSetWindowAttribute functions.
    /// <para><see href="https://github.com/electron/electron/issues/29937"/></para>
    /// </summary>
    [Flags]
    public enum DWMWINDOWATTRIBUTE
    {
        /// <summary>
        /// Is non-client rendering enabled/disabled
        /// </summary>
        DWMWA_NCRENDERING_ENABLED = 1,

        /// <summary>
        /// DWMNCRENDERINGPOLICY - Non-client rendering policy
        /// </summary>
        DWMWA_NCRENDERING_POLICY = 2,

        /// <summary>
        /// Potentially enable/forcibly disable transitions
        /// </summary>
        DWMWA_TRANSITIONS_FORCEDISABLED = 3,

        /// <summary>
        /// Enables content rendered in the non-client area to be visible on the frame drawn by DWM.
        /// </summary>
        DWMWA_ALLOW_NCPAINT = 4,

        /// <summary>
        /// Retrieves the bounds of the caption button area in the window-relative space.
        /// </summary>
        DWMWA_CAPTION_BUTTON_BOUNDS = 5,

        /// <summary>
        /// Is non-client content RTL mirrored
        /// </summary>
        DWMWA_NONCLIENT_RTL_LAYOUT = 6,

        /// <summary>
        /// Forces the window to display an iconic thumbnail or peek representation (a static bitmap), even if a live or snapshot representation of the window is available.
        /// </summary>
        DWMWA_FORCE_ICONIC_REPRESENTATION = 7,

        /// <summary>
        /// Designates how Flip3D will treat the window.
        /// </summary>
        DWMWA_FLIP3D_POLICY = 8,

        /// <summary>
        /// Gets the extended frame bounds rectangle in screen space
        /// </summary>
        DWMWA_EXTENDED_FRAME_BOUNDS = 9,

        /// <summary>
        /// Indicates an available bitmap when there is no better thumbnail representation.
        /// </summary>
        DWMWA_HAS_ICONIC_BITMAP = 10,

        /// <summary>
        /// Don't invoke Peek on the window.
        /// </summary>
        DWMWA_DISALLOW_PEEK = 11,

        /// <summary>
        /// LivePreview exclusion information
        /// </summary>
        DWMWA_EXCLUDED_FROM_PEEK = 12,

        /// <summary>
        /// Cloaks the window such that it is not visible to the user.
        /// </summary>
        DWMWA_CLOAK = 13,

        /// <summary>
        /// If the window is cloaked, provides one of the following values explaining why.
        /// </summary>
        DWMWA_CLOAKED = 14,

        /// <summary>
        /// Freeze the window's thumbnail image with its current visuals. Do no further live updates on the thumbnail image to match the window's contents.
        /// </summary>
        DWMWA_FREEZE_REPRESENTATION = 15,

        /// <summary>
        /// BOOL, Updates the window only when desktop composition runs for other reasons
        /// </summary>
        DWMWA_PASSIVE_UPDATE_MODE = 16,

        /// <summary>
        /// BOOL, Allows the use of host backdrop brushes for the window.
        /// </summary>
        DWMWA_USE_HOSTBACKDROPBRUSH = 17,

        /// <summary>
        /// Allows a window to either use the accent color, or dark, according to the user Color Mode preferences.
        /// </summary>
        DMWA_USE_IMMERSIVE_DARK_MODE_OLD = 19,

        /// <summary>
        /// Allows a window to either use the accent color, or dark, according to the user Color Mode preferences.
        /// </summary>
        DWMWA_USE_IMMERSIVE_DARK_MODE = 20,

        /// <summary>
        /// Controls the policy that rounds top-level window corners.
        /// <para>Windows 11 and above.</para>
        /// </summary>
        DWMWA_WINDOW_CORNER_PREFERENCE = 33,

        /// <summary>
        /// The color of the thin border around a top-level window.
        /// </summary>
        DWMWA_BORDER_COLOR = 34,

        /// <summary>
        /// The color of the caption.
        /// <para>Windows 11 and above.</para>
        /// </summary>
        DWMWA_CAPTION_COLOR = 35,

        /// <summary>
        /// The color of the caption text.
        /// <para>Windows 11 and above.</para>
        /// </summary>
        DWMWA_TEXT_COLOR = 36,

        /// <summary>
        /// Width of the visible border around a thick frame window.
        /// <para>Windows 11 and above.</para>
        /// </summary>
        DWMWA_VISIBLE_FRAME_BORDER_THICKNESS = 37,

        /// <summary>
        /// Allows to enter a value from 0 to 4 deciding on the imposed backdrop effect.
        /// </summary>
        DWMWA_SYSTEMBACKDROP_TYPE = 38,

        /// <summary>
        /// Indicates whether the window should use the Mica effect.
        /// <para>Windows 11 and above.</para>
        /// </summary>
        DWMWA_MICA_EFFECT = 1029,
    }

    /// <summary>
    /// Sets the value of Desktop Window Manager (DWM) non-client rendering attributes for a window.
    /// </summary>
    /// <param name="hWnd">The handle to the window for which the attribute value is to be set.</param>
    /// <param name="dwAttribute">A flag describing which value to set, specified as a value of the DWMWINDOWATTRIBUTE enumeration.</param>
    /// <param name="pvAttribute">A pointer to an object containing the attribute value to set.</param>
    /// <param name="cbAttribute">The size, in bytes, of the attribute value being set via the <c>pvAttribute</c> parameter.</param>
    /// <returns>If the function succeeds, it returns <c>S_OK</c>. Otherwise, it returns an <c>HRESULT</c> error code.</returns>
    [DllImport(Libraries.Dwmapi)]
    public static extern int DwmSetWindowAttribute(
        [In] IntPtr hWnd,
        [In] DWMWINDOWATTRIBUTE dwAttribute,
        [In] ref int pvAttribute,
        [In] int cbAttribute
    );
}

#pragma warning restore SA1307 // Accessible fields should begin with upper-case letter
#pragma warning restore CA1060 // Move pinvokes to native methods class
