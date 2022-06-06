// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using WPFUI.Appearance;

namespace WPFUI.Interop;

/// <summary>
/// A set of dangerous methods to modify the appearance.
/// </summary>
[Obsolete("This class is not depracted, but is dangerous to use.")]
public static class UnsafeNativeMethods
{
    #region Window Corners

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
        int pvAttribute = (int)UnsafeReflection.Cast(cornerPreference);

        // TODO: Validate HRESULT
        Dwmapi.DwmSetWindowAttribute(
            handle,
            Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE,
            ref pvAttribute,
            Marshal.SizeOf(typeof(int)));

        return true;
    }

    #endregion

    #region Window Immersive Dark Mode

    /// <summary>
    /// Tries to remove ImmersiveDarkMode effect from the <see cref="Window"/>.
    /// </summary>
    /// <param name="window">The window to which the effect is to be applied.</param>
    /// <returns><see langword="true"/> if invocation of native Windows function succeeds.</returns>
    public static bool RemoveWindowDarkMode(Window window)
        => GetHandle(window, out IntPtr windowHandle) && RemoveWindowDarkMode(windowHandle);

    /// <summary>
    /// Tries to remove ImmersiveDarkMode effect from the window handle.
    /// </summary>
    /// <param name="handle">Window handle.</param>
    /// <returns><see langword="true"/> if invocation of native Windows function succeeds.</returns>
    public static bool RemoveWindowDarkMode(IntPtr handle)
    {
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
    public static bool ApplyWindowDarkMode(Window window)
        => GetHandle(window, out IntPtr windowHandle) && ApplyWindowDarkMode(windowHandle);

    /// <summary>
    /// Tries to apply ImmersiveDarkMode effect for the window handle.
    /// </summary>
    /// <param name="handle">Window handle.</param>
    /// <returns><see langword="true"/> if invocation of native Windows function succeeds.</returns>
    public static bool ApplyWindowDarkMode(IntPtr handle)
    {
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

    #endregion

    #region Window Titlebar

    /// <summary>
    /// Tries to remove titlebar from selected <see cref="Window"/>.
    /// </summary>
    /// <param name="window">The window to which the effect is to be applied.</param>
    /// <returns><see langword="true"/> if invocation of native Windows function succeeds.</returns>
    public static bool RemoveWindowTitlebar(Window window)
        => GetHandle(window, out IntPtr windowHandle) && RemoveWindowTitlebar(windowHandle);

    /// <summary>
    /// Tries to remove titlebar from selected window handle.
    /// </summary>
    /// <param name="handle">Window handle.</param>
    /// <returns><see langword="true"/> if invocation of native Windows function succeeds.</returns>
    public static bool RemoveWindowTitlebar(IntPtr handle)
    {
        var windowStyleLong = User32.GetWindowLong(handle, User32.GWL.GWL_STYLE);
        windowStyleLong &= ~(int)User32.WS.SYSMENU;

        var result = User32.SetWindowLong(handle, User32.GWL.GWL_STYLE, windowStyleLong);

        return result > 0x0;
    }

    #endregion

    #region Window Backdrop Effect

    /// <summary>
    /// Tries to apply selected backdrop type for <see cref="Window"/>
    /// </summary>
    /// <param name="window">Selected window.</param>
    /// <param name="backgroundType">Backdrop type.</param>
    /// <returns><see langword="true"/> if invocation of native Windows function succeeds.</returns>
    public static bool ApplyWindowBackdrop(Window window, BackgroundType backgroundType)
        => GetHandle(window, out IntPtr windowHandle) && ApplyWindowBackdrop(windowHandle, backgroundType);

    /// <summary>
    /// Tries to apply selected backdrop type for window handle.
    /// </summary>
    /// <param name="handle">Selected window handle.</param>
    /// <param name="backgroundType">Backdrop type.</param>
    /// <returns><see langword="true"/> if invocation of native Windows function succeeds.</returns>
    public static bool ApplyWindowBackdrop(IntPtr handle, BackgroundType backgroundType)
    {
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
    /// Tries to remove backdrop effect from the <see cref="Window"/>.
    /// </summary>
    /// <param name="window">Selected Window.</param>
    /// <returns><see langword="true"/> if invocation of native Windows function succeeds.</returns>
    public static bool RemoveWindowBackdrop(Window window)
        => GetHandle(window, out IntPtr windowHandle) && RemoveWindowBackdrop(windowHandle);

    /// <summary>
    /// Tries to remove backdrop effect from the window handle.
    /// </summary>
    /// <param name="handle">Window handle.</param>
    /// <returns><see langword="true"/> if invocation of native Windows function succeeds.</returns>
    public static bool RemoveWindowBackdrop(IntPtr handle)
    {
        var pvAttribute = 0x0; // Disable
        var backdropPvAttribute = (int)Dwmapi.DWMSBT.DWMSBT_DISABLE;

        Dwmapi.DwmSetWindowAttribute(
            handle,
            Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_MICA_EFFECT,
            ref pvAttribute,
            Marshal.SizeOf(typeof(int)));

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
    /// <param name="window">Window to check.</param>
    /// <param name="backdropType">Background backdrop type.</param>
    public static bool IsWindowHasBackdrop(Window window, BackgroundType backdropType)
        => GetHandle(window, out IntPtr windowHandle) && IsWindowHasBackdrop(windowHandle, backdropType);

    /// <summary>
    /// Tries to determine whether the provided <see cref="Window"/> has applied legacy backdrop effect.
    /// </summary>
    /// <param name="handle">Window handle.</param>
    /// <param name="backdropType">Background backdrop type.</param>
    public static bool IsWindowHasBackdrop(IntPtr handle, BackgroundType backdropType)
    {
        if (!User32.IsWindow(handle))
            return false;

        var pvAttribute = 0x0;

        Dwmapi.DwmGetWindowAttribute(handle, Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE, ref pvAttribute,
            Marshal.SizeOf(typeof(int)));

        return pvAttribute == (int)UnsafeReflection.Cast(backdropType);
    }

    #endregion Window Backdrop Effect

    #region Initial Windows 11 Mica

    /// <summary>
    /// Tries to determine whether the provided <see cref="Window"/> has applied legacy Mica effect.
    /// </summary>
    /// <param name="window">Window to check.</param>
    public static bool IsWindowHasLegacyMica(Window window)
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
    public static bool ApplyWindowLegacyMicaEffect(Window window)
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

    #endregion

    #region Window Legacy Acrylic

    /// <summary>
    /// Tries to apply legacy Acrylic effect for the selected <see cref="Window"/>.
    /// </summary>
    /// <param name="window">The window to which the effect is to be applied.</param>
    /// <returns><see langword="true"/> if invocation of native Windows function succeeds.</returns>
    public static bool ApplyWindowLegacyAcrylicEffect(Window window)
        => GetHandle(window, out IntPtr windowHandle) && ApplyWindowLegacyAcrylicEffect(windowHandle);

    /// <summary>
    /// Tries to apply legacy Acrylic effect for the selected <see cref="Window"/>.
    /// </summary>
    /// <param name="handle">Window handle</param>
    /// <returns><see langword="true"/> if invocation of native Windows function succeeds.</returns>
    public static bool ApplyWindowLegacyAcrylicEffect(IntPtr handle)
    {
        // TODO
        return false;
        //if (Common.Windows.Is(WindowsRelease.Windows11Insider1))
        //{
        //    if (!UnsafeNativeMethods.RemoveWindowTitlebar(handle))
        //        return false;

        //    int backdropPvAttribute = (int)NativeMethods.Interop.Dwmapi.DWMSBT.DWMSBT_TRANSIENTWINDOW;

        //    NativeMethods.Interop.Dwmapi.DwmSetWindowAttribute(
        //        handle,
        //        NativeMethods.Interop.Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE,
        //        ref backdropPvAttribute,
        //        Marshal.SizeOf(typeof(int)));

        //    if (!AppearanceData.Handlers.Contains(handle))
        //        AppearanceData.Handlers.Add(handle);

        //    return true;
        //}

        //if (Common.Windows.Is(WindowsRelease.Windows10V20H1))
        //{
        //    //TODO: We need to set window transparency to True

        //    var accentPolicy = new NativeMethods.Interop.User32.ACCENT_POLICY
        //    {
        //        AccentState = NativeMethods.Interop.User32.ACCENT_STATE.ACCENT_ENABLE_ACRYLICBLURBEHIND,
        //        GradientColor = (0 << 24) | (0x990000 & 0xFFFFFF)
        //    };

        //    int accentStructSize = Marshal.SizeOf(accentPolicy);

        //    IntPtr accentPtr = Marshal.AllocHGlobal(accentStructSize);
        //    Marshal.StructureToPtr(accentPolicy, accentPtr, false);

        //    var data = new NativeMethods.Interop.User32.WINCOMPATTRDATA
        //    {
        //        Attribute = NativeMethods.Interop.User32.WINCOMPATTR.WCA_ACCENT_POLICY,
        //        SizeOfData = accentStructSize,
        //        Data = accentPtr
        //    };

        //    NativeMethods.Interop.User32.SetWindowCompositionAttribute(handle, ref data);

        //    Marshal.FreeHGlobal(accentPtr);

        //    return true;
        //}

        //return false;
    }

    #endregion

    #region DMWA Colorization

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

    #endregion

    #region Taskbar

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
        taskbarList.SetProgressValue(
            hWnd,
            Convert.ToUInt64(current),
            Convert.ToUInt64(total));

        return true;
    }

    #endregion

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
    private static bool GetHandle(Window window, out IntPtr windowHandle)
    {
        windowHandle = new WindowInteropHelper(window).Handle;

        return windowHandle != IntPtr.Zero;
    }
}
