// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using Wpf.Ui.Appearance;
using Wpf.Ui.Interop;

namespace Wpf.Ui.Controls.Window;

/// <summary>
/// Applies the chosen backdrop effect to the selected window.
/// </summary>
public static class WindowBackdrop
{
    /// <summary>
    /// Checks whether the selected backdrop type is supported on current platform.
    /// </summary>
    /// <returns><see langword="true"/> if the selected backdrop type is supported on current platform.</returns>
    public static bool IsSupported(WindowBackdropType backdropType)
    {
        return backdropType switch
        {
            WindowBackdropType.Auto => Win32.Utilities.IsOSWindows11Insider1OrNewer,
            WindowBackdropType.Tabbed => Win32.Utilities.IsOSWindows11Insider1OrNewer,
            WindowBackdropType.Mica => Win32.Utilities.IsOSWindows11OrNewer,
            WindowBackdropType.Acrylic => Win32.Utilities.IsOSWindows7OrNewer,
            WindowBackdropType.None => true,
            _ => false
        };
    }

    /// <summary>
    /// Applies backdrop effect to the selected <see cref="System.Windows.Window"/>.
    /// </summary>
    /// <param name="window">Selected window.</param>
    /// <returns><see langword="true"/> if the operation was successfull, otherwise <see langword="false"/>.</returns>
    public static bool ApplyBackdrop(System.Windows.Window window, WindowBackdropType backdropType)
    {
        if (window is null)
            return false;

        if (window.IsLoaded)
        {
            var windowHandle = new WindowInteropHelper(window).Handle;

            if (windowHandle == IntPtr.Zero)
                return false;

            return ApplyBackdrop(windowHandle, backdropType);
        }

        window.Loaded += (sender, _) =>
        {
            var windowHandle = new WindowInteropHelper(sender as System.Windows.Window ?? null)?.Handle ?? IntPtr.Zero;

            if (windowHandle == IntPtr.Zero)
                return;

            ApplyBackdrop(windowHandle, backdropType);
        };

        return true;
    }

    /// <summary>
    /// Applies backdrop effect to the selected handle.
    /// </summary>
    /// <param name="hWnd">Window handle.</param>
    /// <returns><see langword="true"/> if the operation was successfull, otherwise <see langword="false"/>.</returns>
    public static bool ApplyBackdrop(IntPtr hWnd, WindowBackdropType backdropType)
    {
        if (hWnd == IntPtr.Zero)
            return false;

        if (!User32.IsWindow(hWnd))
            return false;

        if (Theme.GetAppTheme() == ThemeType.Dark)
            UnsafeNativeMethods.ApplyWindowDarkMode(hWnd);
        else
            UnsafeNativeMethods.RemoveWindowDarkMode(hWnd);

        UnsafeNativeMethods.RemoveWindowCaption(hWnd);

        // 22H1
        if (!Win32.Utilities.IsOSWindows11Insider1OrNewer)
        {
            if (backdropType == WindowBackdropType.Mica || backdropType == WindowBackdropType.Auto)
                return ApplyLegacyMicaBackdrop(hWnd);

            if (backdropType == WindowBackdropType.Acrylic)
                return ApplyLegacyAcrylicBackdrop(hWnd);

            return false;
        }

        switch (backdropType)
        {
            case WindowBackdropType.Auto:
                return ApplyDwmwWindowAttrubute(hWnd, Dwmapi.DWMSBT.DWMSBT_AUTO);

            case WindowBackdropType.Mica:
                return ApplyDwmwWindowAttrubute(hWnd, Dwmapi.DWMSBT.DWMSBT_MAINWINDOW);

            case WindowBackdropType.Acrylic:
                return ApplyDwmwWindowAttrubute(hWnd, Dwmapi.DWMSBT.DWMSBT_TRANSIENTWINDOW);

            case WindowBackdropType.Tabbed:
                return ApplyDwmwWindowAttrubute(hWnd, Dwmapi.DWMSBT.DWMSBT_TABBEDWINDOW);
        }

        return ApplyDwmwWindowAttrubute(hWnd, Dwmapi.DWMSBT.DWMSBT_DISABLE);
    }

    /// <summary>
    /// Tries to remove backdrop effects if they have been applied to the <see cref="Window"/>.
    /// </summary>
    /// <param name="window">The window from which the effect should be removed.</param>
    public static bool RemoveBackdrop(System.Windows.Window window)
    {
        if (window == null)
            return false;

        var windowHandle = new WindowInteropHelper(window).Handle;

        return RemoveBackdrop(windowHandle);
    }

    /// <summary>
    /// Tries to remove all effects if they have been applied to the <c>hWnd</c>.
    /// </summary>
    /// <param name="hWnd">Pointer to the window handle.</param>
    public static bool RemoveBackdrop(IntPtr hWnd)
    {
        if (hWnd == IntPtr.Zero)
            return false;

        RestoreContentBackground(hWnd);

        if (hWnd == IntPtr.Zero)
            return false;

        if (!User32.IsWindow(hWnd))
            return false;

        var pvAttribute = 0; // Disable
        var backdropPvAttribute = (int)Dwmapi.DWMSBT.DWMSBT_DISABLE;

        Dwmapi.DwmSetWindowAttribute(
            hWnd,
            Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_MICA_EFFECT,
            ref pvAttribute,
            Marshal.SizeOf(typeof(int)));

        Dwmapi.DwmSetWindowAttribute(
            hWnd,
            Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE,
            ref backdropPvAttribute,
            Marshal.SizeOf(typeof(int)));

        return true;
    }

    /// <summary>
    /// Tries to remove background from <see cref="Window"/> and it's composition area.
    /// </summary>
    /// <param name="window">Window to manipulate.</param>
    /// <returns><see langword="true"/> if operation was successful.</returns>
    public static bool RemoveBackground(System.Windows.Window window)
    {
        if (window == null)
            return false;

        // Remove background from visual root
        window.Background = Brushes.Transparent;

        var windowHandle = new WindowInteropHelper(window).Handle;

        if (windowHandle == IntPtr.Zero)
            return false;

        var windowSource = HwndSource.FromHwnd(windowHandle);

        // Remove background from client area
        if (windowSource?.Handle != IntPtr.Zero && windowSource?.CompositionTarget != null)
            windowSource.CompositionTarget.BackgroundColor = Colors.Transparent;

        return true;
    }

    private static bool ApplyDwmwWindowAttrubute(IntPtr hWnd, Dwmapi.DWMSBT dwmSbt)
    {
        if (hWnd == IntPtr.Zero)
            return false;

        if (!User32.IsWindow(hWnd))
            return false;

        var backdropPvAttribute = (int)dwmSbt;

        var dwmApiResult = Dwmapi.DwmSetWindowAttribute(
            hWnd,
            Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE,
            ref backdropPvAttribute,
            Marshal.SizeOf(typeof(int)));

        return dwmApiResult == HRESULT.S_OK;
    }

    private static bool ApplyLegacyMicaBackdrop(IntPtr hWnd)
    {
        var backdropPvAttribute = 1; //Enable

        // TODO: Validate HRESULT
        var dwmApiResult = Dwmapi.DwmSetWindowAttribute(
            hWnd,
            Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_MICA_EFFECT,
            ref backdropPvAttribute,
            Marshal.SizeOf(typeof(int)));

        return dwmApiResult == HRESULT.S_OK;
    }

    private static bool ApplyLegacyAcrylicBackdrop(IntPtr hWnd)
    {
        throw new NotImplementedException();
    }

    private static bool RestoreContentBackground(IntPtr hWnd)
    {
        if (hWnd == IntPtr.Zero)
            return false;

        if (!User32.IsWindow(hWnd))
            return false;

        var windowSource = HwndSource.FromHwnd(hWnd);

        // Restore client area background
        if (windowSource?.Handle != IntPtr.Zero && windowSource?.CompositionTarget != null)
            windowSource.CompositionTarget.BackgroundColor = SystemColors.WindowColor;

        if (windowSource?.RootVisual is System.Windows.Window window)
        {
            var backgroundBrush = window.Resources["ApplicationBackgroundBrush"];

            // Manual fallback
            if (backgroundBrush is not SolidColorBrush)
                backgroundBrush = GetFallbackBackgroundBrush();

            window.Background = (SolidColorBrush)backgroundBrush;
        }

        return true;
    }

    private static Brush GetFallbackBackgroundBrush()
    {
        return Theme.GetAppTheme() == ThemeType.Dark
                    ? new SolidColorBrush(Color.FromArgb(0xFF, 0x20, 0x20, 0x20))
                    : new SolidColorBrush(Color.FromArgb(0xFF, 0xFA, 0xFA, 0xFA));
    }
}
