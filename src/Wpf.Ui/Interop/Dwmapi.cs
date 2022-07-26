// This Source Code is partially based on reverse engineering of the Windows Operating System,
// and is intended for use on Windows systems only.
// This Source Code is partially based on the source code provided by the .NET Foundation.
// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski.
// All Rights Reserved.

// NOTE
// I split unmanaged code stuff into the NativeMethods library.
// If you have suggestions for the code below, please submit your changes there.
// https://github.com/lepoco/nativemethods

using System;
using System.Runtime.InteropServices;

namespace Wpf.Ui.Interop;

// Windows Kits\10\Include\10.0.22000.0\um\dwmapi.h

/// <summary>
/// Desktop Window Manager (DWM).
/// </summary>
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
internal static class Dwmapi
{
    /// <summary>
    /// Cloaked flags describing why a window is cloaked.
    /// </summary>
    public enum DWM_CLOAKED
    {
        DWM_CLOAKED_APP = 0x00000001,
        DWM_CLOAKED_SHELL = 0x00000002,
        DWM_CLOAKED_INHERITED = 0x00000004
    }

    /// <summary>
    /// GT_*
    /// </summary>
    public enum GESTURE_TYPE
    {
        GT_PEN_TAP = 0,
        GT_PEN_DOUBLETAP = 1,
        GT_PEN_RIGHTTAP = 2,
        GT_PEN_PRESSANDHOLD = 3,
        GT_PEN_PRESSANDHOLDABORT = 4,
        GT_TOUCH_TAP = 5,
        GT_TOUCH_DOUBLETAP = 6,
        GT_TOUCH_RIGHTTAP = 7,
        GT_TOUCH_PRESSANDHOLD = 8,
        GT_TOUCH_PRESSANDHOLDABORT = 9,
        GT_TOUCH_PRESSANDTAP = 10,
    }

    /// <summary>
    /// DWMTWR_* Tab window requirements.
    /// </summary>
    public enum DWM_TAB_WINDOW_REQUIREMENTS
    {
        /// <summary>
        /// This result means the window meets all requirements requested.
        /// </summary>
        DWMTWR_NONE = 0x0000,

        /// <summary>
        /// In some configurations, admin/user setting or mode of the system means that windows won't be tabbed
        /// This requirement says that the system/mode must implement tabbing and if it does not
        /// nothing can be done to change this.
        /// </summary>
        DWMTWR_IMPLEMENTED_BY_SYSTEM = 0x0001,

        /// <summary>
        /// The window has an owner or parent so is ineligible for tabbing.
        /// </summary>
        DWMTWR_WINDOW_RELATIONSHIP = 0x0002,

        /// <summary>
        /// The window has styles that make it ineligible for tabbing.
        /// <para>To be eligible windows must:</para>
        /// <para>Have the WS_OVERLAPPEDWINDOW (WS_CAPTION, WS_THICKFRAME, etc.) styles set.</para>
        /// <para>Not have WS_POPUP, WS_CHILD or WS_DLGFRAME set.</para>
        /// <para>Not have WS_EX_TOPMOST or WS_EX_TOOLWINDOW set.</para>
        /// </summary>
        DWMTWR_WINDOW_STYLES = 0x0004,

        // The window has a region (set using SetWindowRgn) making it ineligible.
        DWMTWR_WINDOW_REGION = 0x0008,

        /// <summary>
        /// The window is ineligible due to its Dwm configuration.
        /// It must not extended its client area into the title bar using DwmExtendFrameIntoClientArea
        /// It must not have DWMWA_NCRENDERING_POLICY set to DWMNCRP_ENABLED
        /// </summary>
        DWMTWR_WINDOW_DWM_ATTRIBUTES = 0x0010,

        /// <summary>
        /// The window is ineligible due to it's margins, most likely due to custom handling in WM_NCCALCSIZE.
        /// The window must use the default window margins for the non-client area.
        /// </summary>
        DWMTWR_WINDOW_MARGINS = 0x0020,

        /// <summary>
        /// The window has been explicitly opted out by setting DWMWA_TABBING_ENABLED to FALSE.
        /// </summary>
        DWMTWR_TABBING_ENABLED = 0x0040,

        /// <summary>
        /// The user has configured this application to not participate in tabbing.
        /// </summary>
        DWMTWR_USER_POLICY = 0x0080,

        /// <summary>
        /// The group policy has configured this application to not participate in tabbing.
        /// </summary>
        DWMTWR_GROUP_POLICY = 0x0100,

        /// <summary>
        /// This is set if app compat has blocked tabs for this window. Can be overridden per window by setting
        /// DWMWA_TABBING_ENABLED to TRUE. That does not override any other tabbing requirements.
        /// </summary>
        DWMTWR_APP_COMPAT = 0x0200
    }

    /// <summary>
    /// Flags used by the DwmSetWindowAttribute function to specify the rounded corner preference for a window.
    /// </summary>
    [Flags]
    public enum DWM_WINDOW_CORNER_PREFERENCE
    {
        DEFAULT = 0,
        DONOTROUND = 1,
        ROUND = 2,
        ROUNDSMALL = 3
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
        DWMSBT_TABBEDWINDOW = 4
    }

    /// <summary>
    /// Non-client rendering policy attribute values
    /// </summary>
    public enum DWMNCRENDERINGPOLICY
    {
        /// <summary>
        /// Enable/disable non-client rendering based on window style
        /// </summary>
        DWMNCRP_USEWINDOWSTYLE,

        /// <summary>
        /// Disabled non-client rendering; window style is ignored
        /// </summary>
        DWMNCRP_DISABLED,

        /// <summary>
        /// Enabled non-client rendering; window style is ignored
        /// </summary>
        DWMNCRP_ENABLED,

        /// <summary>
        /// Sentinel value.
        /// </summary>
        DWMNCRP_LAST
    }

    /// <summary>
    /// Values designating how Flip3D treats a given window.
    /// </summary>
    public enum DWMFLIP3DWINDOWPOLICY
    {
        /// <summary>
        /// Hide or include the window in Flip3D based on window style and visibility.
        /// </summary>
        DWMFLIP3D_DEFAULT,

        /// <summary>
        /// Display the window under Flip3D and disabled.
        /// </summary>
        DWMFLIP3D_EXCLUDEBELOW,

        /// <summary>
        /// Display the window above Flip3D and enabled.
        /// </summary>
        DWMFLIP3D_EXCLUDEABOVE,

        /// <summary>
        /// Sentinel value.
        /// </summary>
        DWMFLIP3D_LAST
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
        DWMWA_MICA_EFFECT = 1029
    }

    /// <summary>
    /// Represents the current DWM color accent settings.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DWMCOLORIZATIONPARAMS
    {
        /// <summary>
        /// ColorizationColor
        /// </summary>
        public uint clrColor;

        /// <summary>
        /// ColorizationAfterglow.
        /// </summary>
        public uint clrAfterGlow;

        /// <summary>
        /// ColorizationColorBalance.
        /// </summary>
        public uint nIntensity;

        /// <summary>
        /// ColorizationAfterglowBalance.
        /// </summary>
        public uint clrAfterGlowBalance;

        /// <summary>
        /// ColorizationBlurBalance.
        /// </summary>
        public uint clrBlurBalance;

        /// <summary>
        /// ColorizationGlassReflectionIntensity.
        /// </summary>
        public uint clrGlassReflectionIntensity;

        /// <summary>
        /// ColorizationOpaqueBlend.
        /// </summary>
        public bool fOpaque;
    }

    /// <summary>
    /// Defines a data type used by the Desktop Window Manager (DWM) APIs. It represents a generic ratio and is used for different purposes and units even within a single API.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct UNSIGNED_RATIO
    {
        /// <summary>
        /// The ratio numerator.
        /// </summary>
        public uint uiNumerator;

        /// <summary>
        /// The ratio denominator.
        /// </summary>
        public uint uiDenominator;
    }

    /// <summary>
    /// Specifies the input operations for which visual feedback should be provided. This enumeration is used by the DwmShowContact function.
    /// </summary>
    public enum DWM_SHOWCONTACT
    {
        DWMSC_DOWN,
        DWMSC_UP,
        DWMSC_DRAG,
        DWMSC_HOLD,
        DWMSC_PENBARREL,
        DWMSC_NONE,
        DWMSC_ALL
    }

    /// <summary>
    /// Flags used by the DwmSetPresentParameters function to specify the frame sampling type.
    /// </summary>
    public enum DWM_SOURCE_FRAME_SAMPLING
    {
        /// <summary>
        /// Use the first source frame that includes the first refresh of the output frame
        /// </summary>
        DWM_SOURCE_FRAME_SAMPLING_POINT,

        /// <summary>
        /// Use the source frame that includes the most refreshes of out the output frame
        /// in case of multiple source frames with the same coverage the last will be used
        /// </summary>
        DWM_SOURCE_FRAME_SAMPLING_COVERAGE,

        /// <summary>
        /// Sentinel value.
        /// </summary>
        DWM_SOURCE_FRAME_SAMPLING_LAST
    }

    /// <summary>
    /// Specifies Desktop Window Manager (DWM) composition timing information. Used by the <see cref="DwmGetCompositionTimingInfo"/> function.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DWM_TIMING_INFO
    {
        public int cbSize;
        public UNSIGNED_RATIO rateRefresh;
        public ulong qpcRefreshPeriod;
        public UNSIGNED_RATIO rateCompose;
        public ulong qpcVBlank;
        public ulong cRefresh;
        public uint cDXRefresh;
        public ulong qpcCompose;
        public ulong cFrame;
        public uint cDXPresent;
        public ulong cRefreshFrame;
        public ulong cFrameSubmitted;
        public uint cDXPresentSubmitted;
        public ulong cFrameConfirmed;
        public uint cDXPresentConfirmed;
        public ulong cRefreshConfirmed;
        public uint cDXRefreshConfirmed;
        public ulong cFramesLate;
        public uint cFramesOutstanding;
        public ulong cFrameDisplayed;
        public ulong qpcFrameDisplayed;
        public ulong cRefreshFrameDisplayed;
        public ulong cFrameComplete;
        public ulong qpcFrameComplete;
        public ulong cFramePending;
        public ulong qpcFramePending;
        public ulong cFramesDisplayed;
        public ulong cFramesComplete;
        public ulong cFramesPending;
        public ulong cFramesAvailable;
        public ulong cFramesDropped;
        public ulong cFramesMissed;
        public ulong cRefreshNextDisplayed;
        public ulong cRefreshNextPresented;
        public ulong cRefreshesDisplayed;
        public ulong cRefreshesPresented;
        public ulong cRefreshStarted;
        public ulong cPixelsReceived;
        public ulong cPixelsDrawn;
        public ulong cBuffersEmpty;
    }

    /// <summary>
    /// SIT flags.
    /// </summary>
    public enum DWM_SIT
    {
        /// <summary>
        /// None.
        /// </summary>
        NONE,

        /// <summary>
        /// Displays a frame around the provided bitmap.
        /// </summary>
        DISPLAYFRAME = 1,
    }

    /// <summary>
    /// Obtains a value that indicates whether Desktop Window Manager (DWM) composition is enabled.
    /// </summary>
    /// <param name="pfEnabled">A pointer to a value that, when this function returns successfully, receives TRUE if DWM composition is enabled; otherwise, FALSE.</param>
    /// <returns>If this function succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
    [DllImport(Libraries.Dwmapi, BestFitMapping = false)]
    public static extern int DwmIsCompositionEnabled([Out] out int pfEnabled);

    /// <summary>
    /// Extends the window frame into the client area.
    /// </summary>
    /// <param name="hWnd">The handle to the window in which the frame will be extended into the client area.</param>
    /// <param name="pMarInset">A pointer to a MARGINS structure that describes the margins to use when extending the frame into the client area.</param>
    [DllImport(Libraries.Dwmapi, PreserveSig = false)]
    public static extern void DwmExtendFrameIntoClientArea([In] IntPtr hWnd, [In] ref UxTheme.MARGINS pMarInset);

    /// <summary>
    /// Retrieves the current composition timing information for a specified window.
    /// </summary>
    /// <param name="hWnd">The handle to the window for which the composition timing information should be retrieved.</param>
    /// <param name="pTimingInfo">A pointer to a <see cref="DWM_TIMING_INFO"/> structure that, when this function returns successfully, receives the current composition timing information for the window.</param>
    [DllImport(Libraries.Dwmapi)]
    public static extern void DwmGetCompositionTimingInfo([In] IntPtr hWnd, [In] ref DWM_TIMING_INFO pTimingInfo);

    /// <summary>
    /// Called by an application to indicate that all previously provided iconic bitmaps from a window, both thumbnails and peek representations, should be refreshed.
    /// </summary>
    /// <param name="hWnd">A handle to the window or tab whose bitmaps are being invalidated through this call. This window must belong to the calling process.</param>
    [DllImport(Libraries.Dwmapi, PreserveSig = false)]
    public static extern void DwmInvalidateIconicBitmaps([In] IntPtr hWnd);

    /// <summary>
    /// Sets a static, iconic bitmap on a window or tab to use as a thumbnail representation. The taskbar can use this bitmap as a thumbnail switch target for the window or tab.
    /// </summary>
    /// <param name="hWnd">A handle to the window or tab. This window must belong to the calling process.</param>
    /// <param name="hbmp">A handle to the bitmap to represent the window that hwnd specifies.</param>
    /// <param name="dwSITFlags">The display options for the thumbnail.</param>
    [DllImport(Libraries.Dwmapi, PreserveSig = false)]
    public static extern void DwmSetIconicThumbnail([In] IntPtr hWnd, [In] IntPtr hbmp, [In] DWM_SIT dwSITFlags);

    /// <summary>
    /// Sets a static, iconic bitmap to display a live preview (also known as a Peek preview) of a window or tab. The taskbar can use this bitmap to show a full-sized preview of a window or tab.
    /// </summary>
    /// <param name="hWnd">A handle to the window. This window must belong to the calling process.</param>
    /// <param name="hbmp">A handle to the bitmap to represent the window that hwnd specifies.</param>
    /// <param name="pptClient">The offset of a tab window's client region (the content area inside the client window frame) from the host window's frame. This offset enables the tab window's contents to be drawn correctly in a live preview when it is drawn without its frame.</param>
    /// <param name="dwSITFlags">The display options for the live preview.</param>
    [DllImport(Libraries.Dwmapi, PreserveSig = false)]
    public static extern int DwmSetIconicLivePreviewBitmap([In] IntPtr hWnd, [In] IntPtr hbmp,
        [In, Optional] WinDef.POINT pptClient, [In] DWM_SIT dwSITFlags);

    /// <summary>
    /// Sets the value of Desktop Window Manager (DWM) non-client rendering attributes for a window.
    /// </summary>
    /// <param name="hWnd">The handle to the window for which the attribute value is to be set.</param>
    /// <param name="dwAttribute">A flag describing which value to set, specified as a value of the DWMWINDOWATTRIBUTE enumeration.</param>
    /// <param name="pvAttribute">A pointer to an object containing the attribute value to set.</param>
    /// <param name="cbAttribute">The size, in bytes, of the attribute value being set via the <c>pvAttribute</c> parameter.</param>
    /// <returns>If the function succeeds, it returns <c>S_OK</c>. Otherwise, it returns an <c>HRESULT</c> error code.</returns>
    [DllImport(Libraries.Dwmapi)]
    public static extern int DwmSetWindowAttribute([In] IntPtr hWnd, [In] int dwAttribute,
        [In] ref int pvAttribute,
        [In] int cbAttribute);

    /// <summary>
    /// Sets the value of Desktop Window Manager (DWM) non-client rendering attributes for a window.
    /// </summary>
    /// <param name="hWnd">The handle to the window for which the attribute value is to be set.</param>
    /// <param name="dwAttribute">A flag describing which value to set, specified as a value of the DWMWINDOWATTRIBUTE enumeration.</param>
    /// <param name="pvAttribute">A pointer to an object containing the attribute value to set.</param>
    /// <param name="cbAttribute">The size, in bytes, of the attribute value being set via the <c>pvAttribute</c> parameter.</param>
    /// <returns>If the function succeeds, it returns <c>S_OK</c>. Otherwise, it returns an <c>HRESULT</c> error code.</returns>
    [DllImport(Libraries.Dwmapi)]
    public static extern int DwmSetWindowAttribute([In] IntPtr hWnd, [In] DWMWINDOWATTRIBUTE dwAttribute,
        [In] ref int pvAttribute,
        [In] int cbAttribute);

    /// <summary>
    /// Retrieves the current value of a specified Desktop Window Manager (DWM) attribute applied to a window. For programming guidance, and code examples, see Controlling non-client region rendering.
    /// </summary>
    /// <param name="hWnd">The handle to the window from which the attribute value is to be retrieved.</param>
    /// <param name="dwAttributeToGet">A flag describing which value to retrieve, specified as a value of the <see cref="DWMWINDOWATTRIBUTE"/> enumeration.</param>
    /// <param name="pvAttributeValue">A pointer to a value which, when this function returns successfully, receives the current value of the attribute. The type of the retrieved value depends on the value of the dwAttribute parameter.</param>
    /// <param name="cbAttribute">The size, in bytes, of the attribute value being received via the pvAttribute parameter.</param>
    /// <returns>If the function succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
    [DllImport(Libraries.Dwmapi)]
    public static extern int DwmGetWindowAttribute([In] IntPtr hWnd, [In] DWMWINDOWATTRIBUTE dwAttributeToGet,
           [In] ref int pvAttributeValue,
           [In] int cbAttribute);

    /// <summary>
    /// Retrieves the current value of a specified Desktop Window Manager (DWM) attribute applied to a window. For programming guidance, and code examples, see Controlling non-client region rendering.
    /// </summary>
    /// <param name="hWnd">The handle to the window from which the attribute value is to be retrieved.</param>
    /// <param name="dwAttributeToGet">A flag describing which value to retrieve, specified as a value of the <see cref="DWMWINDOWATTRIBUTE"/> enumeration.</param>
    /// <param name="pvAttributeValue">A pointer to a value which, when this function returns successfully, receives the current value of the attribute. The type of the retrieved value depends on the value of the dwAttribute parameter.</param>
    /// <param name="cbAttribute">The size, in bytes, of the attribute value being received via the pvAttribute parameter.</param>
    /// <returns>If the function succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
    [DllImport(Libraries.Dwmapi)]
    public static extern int DwmGetWindowAttribute([In] IntPtr hWnd, [In] int dwAttributeToGet,
            [In] ref int pvAttributeValue,
            [In] int cbAttribute);

    /// <summary>
    /// The feature is not included in the Microsoft documentation. Reads Desktop Window Manager (DWM) color information.
    /// </summary>
    /// <param name="dwParameters">A pointer to a reference value that will hold the color information.</param>
    [DllImport(Libraries.Dwmapi, EntryPoint = "#127", PreserveSig = false, CharSet = CharSet.Unicode)]
    public static extern void DwmGetColorizationParameters([Out] out DWMCOLORIZATIONPARAMS dwParameters);
}
