// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using Wpf.Ui.Controls.Window;

namespace Wpf.Ui.Interop;

/// <summary>
/// A set of dangerous methods to modify the appearance.
/// </summary>
public static class UnsafeNativeMethods
{
    /// <summary>
    /// Tries to set the <see cref="Window"/> corner preference.
    /// </summary>
    /// <param name="window">Selected window.</param>
    /// <param name="cornerPreference">Window corner preference.</param>
    /// <returns><see langword="true"/> if invocation of native Windows function succeeds.</returns>
    public static bool ApplyWindowCornerPreference(Window window, WindowCornerPreference cornerPreference)
        => GetHandle(window, out IntPtr windowHandle) && ApplyWindowCornerPreference(windowHandle, cornerPreference);

    /// <summary>
    /// Tries to set the corner preference of the selected window.
    /// </summary>
    /// <param name="handle">Selected window handle.</param>
    /// <param name="cornerPreference">Window corner preference.</param>
    /// <returns><see langword="true"/> if invocation of native Windows function succeeds.</returns>
    public static bool ApplyWindowCornerPreference(IntPtr handle, WindowCornerPreference cornerPreference)
    {
        if (handle == IntPtr.Zero)
            return false;

        if (!User32.IsWindow(handle))
            return false;

        int pvAttribute = (int)UnsafeReflection.Cast(cornerPreference);

        // TODO: Validate HRESULT
        Dwmapi.DwmSetWindowAttribute(
            handle,
            Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE,
            ref pvAttribute,
            Marshal.SizeOf(typeof(int)));

        return true;
    }

    /// <summary>
    /// Tries to remove ImmersiveDarkMode effect from the <see cref="Window"/>.
    /// </summary>
    /// <param name="window">The window to which the effect is to be applied.</param>
    /// <returns><see langword="true"/> if invocation of native Windows function succeeds.</returns>
    public static bool RemoveWindowDarkMode(Window? window)
        => GetHandle(window, out IntPtr windowHandle) && RemoveWindowDarkMode(windowHandle);

    /// <summary>
    /// Tries to remove ImmersiveDarkMode effect from the window handle.
    /// </summary>
    /// <param name="handle">Window handle.</param>
    /// <returns><see langword="true"/> if invocation of native Windows function succeeds.</returns>
    public static bool RemoveWindowDarkMode(IntPtr handle)
    {
        if (handle == IntPtr.Zero)
            return false;

        if (!User32.IsWindow(handle))
            return false;

        var pvAttribute = 0x0; // Disable
        var dwAttribute = Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE;

        if (!Win32.Utilities.IsOSWindows11Insider1OrNewer)
            dwAttribute = Dwmapi.DWMWINDOWATTRIBUTE.DMWA_USE_IMMERSIVE_DARK_MODE_OLD;

        // TODO: Validate HRESULT
        Dwmapi.DwmSetWindowAttribute(
            handle,
            dwAttribute,
            ref pvAttribute,
            Marshal.SizeOf(typeof(int)));

        return true;
    }

    /// <summary>
    /// Tries to apply ImmersiveDarkMode effect for the <see cref="Window"/>.
    /// </summary>
    /// <param name="window">The window to which the effect is to be applied.</param>
    /// <returns><see langword="true"/> if invocation of native Windows function succeeds.</returns>
    public static bool ApplyWindowDarkMode(Window? window)
        => GetHandle(window, out IntPtr windowHandle) && ApplyWindowDarkMode(windowHandle);

    /// <summary>
    /// Tries to apply ImmersiveDarkMode effect for the window handle.
    /// </summary>
    /// <param name="handle">Window handle.</param>
    /// <returns><see langword="true"/> if invocation of native Windows function succeeds.</returns>
    public static bool ApplyWindowDarkMode(IntPtr handle)
    {
        if (handle == IntPtr.Zero)
            return false;

        if (!User32.IsWindow(handle))
            return false;

        var pvAttribute = 0x1; // Enable
        var dwAttribute = Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE;

        if (!Win32.Utilities.IsOSWindows11Insider1OrNewer)
            dwAttribute = Dwmapi.DWMWINDOWATTRIBUTE.DMWA_USE_IMMERSIVE_DARK_MODE_OLD;

        // TODO: Validate HRESULT
        Dwmapi.DwmSetWindowAttribute(
            handle,
            dwAttribute,
            ref pvAttribute,
            Marshal.SizeOf(typeof(int)));

        return true;
    }

    /// <summary>
    /// Tries to remove titlebar from selected <see cref="Window"/>.
    /// </summary>
    /// <param name="window">The window to which the effect is to be applied.</param>
    /// <returns><see langword="true"/> if invocation of native Windows function succeeds.</returns>
    public static bool RemoveWindowTitlebarContents(Window? window)
    {
        if (window == null)
            return false;

        if (window.IsLoaded)
            return GetHandle(window, out IntPtr windowHandle) && RemoveWindowTitlebarContents(windowHandle);

        window.Loaded += (sender, _) =>
        {
            GetHandle(sender as Window, out IntPtr windowHandle);
            RemoveWindowTitlebarContents(windowHandle);
        };

        return true;
    }

    /// <summary>
    /// Tries to remove titlebar from selected window handle.
    /// </summary>
    /// <param name="handle">Window handle.</param>
    /// <returns><see langword="true"/> if invocation of native Windows function succeeds.</returns>
    public static bool RemoveWindowTitlebarContents(IntPtr handle)
    {
        if (handle == IntPtr.Zero)
            return false;

        if (!User32.IsWindow(handle))
            return false;

        var windowStyleLong = User32.GetWindowLong(handle, User32.GWL.GWL_STYLE);
        windowStyleLong &= ~(int)User32.WS.SYSMENU;

        var result = User32.SetWindowLong(handle, User32.GWL.GWL_STYLE, windowStyleLong);

        return result > 0x0;
    }

    /// <summary>
    /// Tries to apply selected backdrop type for window handle.
    /// </summary>
    /// <param name="handle">Selected window handle.</param>
    /// <param name="backgroundType">Backdrop type.</param>
    /// <returns><see langword="true"/> if invocation of native Windows function succeeds.</returns>
    public static bool ApplyWindowBackdrop(IntPtr handle, WindowBackdropType backgroundType)
    {
        if (handle == IntPtr.Zero)
            return false;

        if (!User32.IsWindow(handle))
            return false;

        var backdropPvAttribute = (int)UnsafeReflection.Cast(backgroundType);

        if (backdropPvAttribute == (int)Dwmapi.DWMSBT.DWMSBT_DISABLE)
            return false;

        // TODO: Validate HRESULT
        Dwmapi.DwmSetWindowAttribute(
            handle,
            Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE,
            ref backdropPvAttribute,
            Marshal.SizeOf(typeof(int)));

        return true;
    }

    /// <summary>
    /// Tries to determine whether the provided <see cref="Window"/> has applied legacy backdrop effect.
    /// </summary>
    /// <param name="handle">Window handle.</param>
    /// <param name="backdropType">Background backdrop type.</param>
    public static bool IsWindowHasBackdrop(IntPtr handle, WindowBackdropType backdropType)
    {
        if (!User32.IsWindow(handle))
            return false;

        var pvAttribute = 0x0;

        Dwmapi.DwmGetWindowAttribute(handle, Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE, ref pvAttribute,
            Marshal.SizeOf(typeof(int)));

        return pvAttribute == (int)UnsafeReflection.Cast(backdropType);
    }

    /// <summary>
    /// Tries to determine whether the provided <see cref="Window"/> has applied legacy Mica effect.
    /// </summary>
    /// <param name="window">Window to check.</param>
    public static bool IsWindowHasLegacyMica(Window? window)
        => GetHandle(window, out IntPtr windowHandle) && IsWindowHasLegacyMica(windowHandle);

    /// <summary>
    /// Tries to determine whether the provided handle has applied legacy Mica effect.
    /// </summary>
    /// <param name="handle">Window handle.</param>
    public static bool IsWindowHasLegacyMica(IntPtr handle)
    {
        if (!User32.IsWindow(handle))
            return false;

        var pvAttribute = 0x0;

        Dwmapi.DwmGetWindowAttribute(handle, Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_MICA_EFFECT, ref pvAttribute,
            Marshal.SizeOf(typeof(int)));

        return pvAttribute == 0x1;
    }

    /// <summary>
    /// Tries to apply legacy Mica effect for the selected <see cref="Window"/>.
    /// </summary>
    /// <param name="window">The window to which the effect is to be applied.</param>
    /// <returns><see langword="true"/> if invocation of native Windows function succeeds.</returns>
    public static bool ApplyWindowLegacyMicaEffect(Window? window)
        => GetHandle(window, out IntPtr windowHandle) && ApplyWindowLegacyMicaEffect(windowHandle);

    /// <summary>
    /// Tries to apply legacy Mica effect for the selected <see cref="Window"/>.
    /// </summary>
    /// <param name="handle">Window handle.</param>
    /// <returns><see langword="true"/> if invocation of native Windows function succeeds.</returns>
    public static bool ApplyWindowLegacyMicaEffect(IntPtr handle)
    {
        var backdropPvAttribute = 0x1; //Enable

        // TODO: Validate HRESULT
        Dwmapi.DwmSetWindowAttribute(
            handle,
            Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_MICA_EFFECT,
            ref backdropPvAttribute,
            Marshal.SizeOf(typeof(int)));

        return true;
    }

    /// <summary>
    /// Tries to apply legacy Acrylic effect for the selected <see cref="Window"/>.
    /// </summary>
    /// <param name="window">The window to which the effect is to be applied.</param>
    /// <returns><see langword="true"/> if invocation of native Windows function succeeds.</returns>
    public static bool ApplyWindowLegacyAcrylicEffect(Window? window)
        => GetHandle(window, out IntPtr windowHandle) && ApplyWindowLegacyAcrylicEffect(windowHandle);

    /// <summary>
    /// Tries to apply legacy Acrylic effect for the selected <see cref="Window"/>.
    /// </summary>
    /// <param name="handle">Window handle</param>
    /// <returns><see langword="true"/> if invocation of native Windows function succeeds.</returns>
    public static bool ApplyWindowLegacyAcrylicEffect(IntPtr handle)
    {
        var accentPolicy = new Interop.User32.ACCENT_POLICY
        {
            nAccentState = User32.ACCENT_STATE.ACCENT_ENABLE_ACRYLICBLURBEHIND,
            nColor = 0x990000 & 0xFFFFFF
        };

        var accentStructSize = Marshal.SizeOf(accentPolicy);
        var accentPtr = Marshal.AllocHGlobal(accentStructSize);

        Marshal.StructureToPtr(accentPolicy, accentPtr, false);

        var data = new User32.WINCOMPATTRDATA
        {
            Attribute = User32.WCA.WCA_ACCENT_POLICY,
            SizeOfData = accentStructSize,
            Data = accentPtr
        };

        User32.SetWindowCompositionAttribute(handle, ref data);

        Marshal.FreeHGlobal(accentPtr);

        return true;
    }

    /// <summary>
    /// Tries to get currently selected Window accent color.
    /// </summary>
    public static Color GetDwmColor()
    {
        Dwmapi.DwmGetColorizationParameters(out var dwmParams);

        var values = BitConverter.GetBytes(dwmParams.clrColor);

        return Color.FromArgb(
            255,
            values[2],
            values[1],
            values[0]
        );
    }

    /// <summary>
    /// Tries to set taskbar state for the selected window handle.
    /// </summary>
    /// <param name="hWnd">Window handle.</param>
    /// <param name="taskbarFlag">Taskbar flag.</param>
    internal static bool SetTaskbarState(IntPtr hWnd, ShObjIdl.TBPFLAG taskbarFlag)
    {
        if (hWnd == IntPtr.Zero)
            return false;

        if (!User32.IsWindow(hWnd))
            return false;

        var taskbarList = new ShObjIdl.CTaskbarList() as ShObjIdl.ITaskbarList4;

        if (taskbarList == null)
            return false;

        taskbarList.HrInit();
        taskbarList.SetProgressState(hWnd, taskbarFlag);

        return true;
    }

    /// <summary>
    /// Tries to set taskbar value for the selected window handle.
    /// </summary>
    /// <param name="hWnd">Window handle.</param>
    /// <param name="current">Current value.</param>
    /// <param name="total">Total value to divide.</param>
    internal static bool SetTaskbarValue(IntPtr hWnd, ShObjIdl.TBPFLAG taskbarFlag, int current, int total)
    {
        if (hWnd == IntPtr.Zero)
            return false;

        if (!User32.IsWindow(hWnd))
            return false;

        // TODO: Get existing taskbar class

        var taskbarList = new ShObjIdl.CTaskbarList() as ShObjIdl.ITaskbarList4;

        if (taskbarList == null)
            return false;

        taskbarList.HrInit();
        taskbarList.SetProgressState(hWnd, taskbarFlag);

        if (taskbarFlag != ShObjIdl.TBPFLAG.TBPF_INDETERMINATE && taskbarFlag != ShObjIdl.TBPFLAG.TBPF_NOPROGRESS)
            taskbarList.SetProgressValue(
                hWnd,
                Convert.ToUInt64(current),
                Convert.ToUInt64(total));

        return true;
    }

    public static bool RemoveWindowCaption(Window window)
    {
        if (window == null)
            return false;

        var windowHandle = new WindowInteropHelper(window).Handle;

        return RemoveWindowCaption(windowHandle);
    }

    public static bool RemoveWindowCaption(IntPtr hWnd)
    {
        if (hWnd == IntPtr.Zero)
            return false;

        if (!User32.IsWindow(hWnd))
            return false;

        var wtaOptions = new UxTheme.WTA_OPTIONS()
        {
            dwFlags = UxTheme.WTNCA.NODRAWCAPTION,
            dwMask = UxTheme.WTNCA.VALIDBITS
        };

        UxTheme.SetWindowThemeAttribute(
            hWnd,
            UxTheme.WINDOWTHEMEATTRIBUTETYPE.WTA_NONCLIENT,
            ref wtaOptions,
            (uint)Marshal.SizeOf(typeof(UxTheme.WTA_OPTIONS)));

        return true;
    }

    public static bool ExtendClientAreaIntoTitleBar(Window window)
    {
        if (window == null)
            return false;

        var windowHandle = new WindowInteropHelper(window).Handle;

        return ExtendClientAreaIntoTitleBar(windowHandle);
    }

    public static bool ExtendClientAreaIntoTitleBar(IntPtr hWnd)
    {
        // !! EXPERIMENTAl

        // NOTE:
        // WinRt has ExtendContentIntoTitlebar, but it needs some digging

        if (hWnd == IntPtr.Zero)
            return false;

        if (!User32.IsWindow(hWnd))
            return false;

        // #1 Remove titlebar elements
        var wtaOptions = new UxTheme.WTA_OPTIONS()
        {
            dwFlags = (UxTheme.WTNCA.NODRAWCAPTION | UxTheme.WTNCA.NODRAWICON | UxTheme.WTNCA.NOSYSMENU),
            dwMask = UxTheme.WTNCA.VALIDBITS
        };

        Interop.UxTheme.SetWindowThemeAttribute(
            hWnd,
            UxTheme.WINDOWTHEMEATTRIBUTETYPE.WTA_NONCLIENT,
            ref wtaOptions,
            (uint)Marshal.SizeOf(typeof(UxTheme.WTA_OPTIONS)));

        var windowDpi = Ui.Dpi.DpiHelper.GetWindowDpi(hWnd);

        // #2 Extend glass frame
        var deviceGlassThickness = Ui.Dpi.DpiHelper.LogicalThicknessToDevice(
            new Thickness(-1, -1, -1, -1),
            windowDpi.DpiScaleX,
            windowDpi.DpiScaleY);

        var dwmMargin = new UxTheme.MARGINS
        {
            // err on the side of pushing in glass an extra pixel.
            cxLeftWidth = (int)Math.Ceiling(deviceGlassThickness.Left),
            cxRightWidth = (int)Math.Ceiling(deviceGlassThickness.Right),
            cyTopHeight = (int)Math.Ceiling(deviceGlassThickness.Top),
            cyBottomHeight = (int)Math.Ceiling(deviceGlassThickness.Bottom),
        };

        // #3 Extend client area
        Interop.Dwmapi.DwmExtendFrameIntoClientArea(hWnd, ref dwmMargin);

        // #4 Clear rounding region
        Interop.User32.SetWindowRgn(hWnd, IntPtr.Zero,
            Interop.User32.IsWindowVisible(hWnd));

        return true;
    }

    /// <summary>
    /// Checks whether the DWM composition is enabled.
    /// </summary>
    /// <returns></returns>
    public static bool IsCompositionEnabled()
    {
        Dwmapi.DwmIsCompositionEnabled(out var isEnabled);

        return isEnabled == 0x1;
    }

    /// <summary>
    /// Checks if provided pointer represents existing window.
    /// </summary>
    public static bool IsValidWindow(IntPtr hWnd)
    {
        return User32.IsWindow(hWnd);
    }

    /// <summary>
    /// Tries to get the pointer to the window handle.
    /// </summary>
    /// <param name="window"></param>
    /// <param name="windowHandle"></param>
    /// <returns><see langword="true"/> if the handle is not <see cref="IntPtr.Zero"/>.</returns>
    private static bool GetHandle(Window? window, out IntPtr windowHandle)
    {
        if (window == null)
        {
            windowHandle = IntPtr.Zero;

            return false;
        }

        windowHandle = new WindowInteropHelper(window).Handle;

        return windowHandle != IntPtr.Zero;
    }
}
