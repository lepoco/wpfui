// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Dwm;
using Wpf.Ui.Appearance;
using Wpf.Ui.Interop;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

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
            _ => false,
        };
    }

    /// <summary>
    /// Applies a backdrop effect to the selected <see cref="System.Windows.Window"/>.
    /// </summary>
    /// <param name="window">The window to which the backdrop effect will be applied.</param>
    /// <param name="backdropType">The type of backdrop effect to apply. Determines the visual appearance of the window's backdrop.</param>
    /// <returns><see langword="true"/> if the operation was successful; otherwise, <see langword="false"/>.</returns>
    public static bool ApplyBackdrop(Window? window, WindowBackdropType backdropType)
    {
        if (window is null)
        {
            return false;
        }

        if (window.IsLoaded)
        {
            IntPtr windowHandle = new WindowInteropHelper(window).Handle;

            if (windowHandle == IntPtr.Zero)
            {
                return false;
            }

            return ApplyBackdrop(windowHandle, backdropType);
        }

        window.Loaded += (sender, _1) =>
        {
            IntPtr windowHandle =
                new WindowInteropHelper(sender as Window)?.Handle ?? IntPtr.Zero;

            if (windowHandle == IntPtr.Zero)
            {
                return;
            }

            _ = ApplyBackdrop(windowHandle, backdropType);
        };

        return true;
    }

    /// <summary>
    /// Applies backdrop effect to the selected window handle based on the specified backdrop type.
    /// </summary>
    /// <param name="hWnd">The window handle to which the backdrop effect will be applied.</param>
    /// <param name="backdropType">The type of backdrop effect to apply. This determines the visual appearance of the window's backdrop.</param>
    /// <returns><see langword="true"/> if the operation was successful; otherwise, <see langword="false"/>.</returns>
    public static bool ApplyBackdrop(IntPtr hWnd, WindowBackdropType backdropType)
    {
        if (hWnd == IntPtr.Zero)
        {
            return false;
        }

        if (!PInvoke.IsWindow(new HWND(hWnd)))
        {
            return false;
        }

        if (ApplicationThemeManager.GetAppTheme() == ApplicationTheme.Dark)
        {
            _ = UnsafeNativeMethods.ApplyWindowDarkMode(hWnd);
        }
        else
        {
            _ = UnsafeNativeMethods.RemoveWindowDarkMode(hWnd);
        }

        _ = UnsafeNativeMethods.RemoveWindowCaption(hWnd);

        // 22H1
        if (!Win32.Utilities.IsOSWindows11Insider1OrNewer)
        {
            if (backdropType != WindowBackdropType.None)
            {
                return ApplyLegacyMicaBackdrop(hWnd);
            }

            return false;
        }

        return backdropType switch
        {
            WindowBackdropType.Auto => ApplyDwmwWindowAttribute(hWnd, DWM_SYSTEMBACKDROP_TYPE.DWMSBT_AUTO),
            WindowBackdropType.Mica => ApplyDwmwWindowAttribute(hWnd, DWM_SYSTEMBACKDROP_TYPE.DWMSBT_MAINWINDOW),
            WindowBackdropType.Acrylic => ApplyDwmwWindowAttribute(hWnd, DWM_SYSTEMBACKDROP_TYPE.DWMSBT_TRANSIENTWINDOW),
            WindowBackdropType.Tabbed => ApplyDwmwWindowAttribute(hWnd, DWM_SYSTEMBACKDROP_TYPE.DWMSBT_TABBEDWINDOW),
            _ => ApplyDwmwWindowAttribute(hWnd, DWM_SYSTEMBACKDROP_TYPE.DWMSBT_NONE),
        };
    }

    /// <summary>
    /// Tries to remove backdrop effects if they have been applied to the <see cref="Window"/>.
    /// </summary>
    /// <param name="window">The window from which the effect should be removed.</param>
    public static bool RemoveBackdrop(Window? window)
    {
        if (window is null)
        {
            return false;
        }

        IntPtr windowHandle = new WindowInteropHelper(window).Handle;

        return RemoveBackdrop(windowHandle);
    }

    /// <summary>
    /// Tries to remove all effects if they have been applied to the <c>hWnd</c>.
    /// </summary>
    /// <param name="hWnd">Pointer to the window handle.</param>
    public static unsafe bool RemoveBackdrop(IntPtr hWnd)
    {
        if (hWnd == IntPtr.Zero)
        {
            return false;
        }

        _ = RestoreContentBackground(hWnd);

        if (hWnd == IntPtr.Zero)
        {
            return false;
        }

        if (!PInvoke.IsWindow(new HWND(hWnd)))
        {
            return false;
        }

        BOOL pvAttribute = false;

        _ = PInvoke.DwmSetWindowAttribute(
            new HWND(hWnd),
            DWMWINDOWATTRIBUTE.DWMWA_MICA_EFFECT,
            &pvAttribute,
            (uint) sizeof(BOOL)
        );

        DWM_SYSTEMBACKDROP_TYPE backdropPvAttribute = DWM_SYSTEMBACKDROP_TYPE.DWMSBT_NONE;

        _ = PInvoke.DwmSetWindowAttribute(
            new HWND(hWnd),
            DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE,
            &backdropPvAttribute,
            sizeof(DWM_SYSTEMBACKDROP_TYPE)
        );

        return true;
    }

    /// <summary>
    /// Tries to remove background from <see cref="Window"/> and it's composition area.
    /// </summary>
    /// <param name="window">Window to manipulate.</param>
    /// <returns><see langword="true"/> if operation was successful.</returns>
    public static bool RemoveBackground(Window? window)
    {
        if (window is null)
        {
            return false;
        }

        // Remove background from visual root
        window.SetCurrentValue(System.Windows.Controls.Control.BackgroundProperty, Brushes.Transparent);

        IntPtr windowHandle = new WindowInteropHelper(window).Handle;

        if (windowHandle == IntPtr.Zero)
        {
            return false;
        }

        var windowSource = HwndSource.FromHwnd(windowHandle);

        // Remove background from client area
        if (windowSource?.Handle != IntPtr.Zero && windowSource?.CompositionTarget != null)
        {
            windowSource.CompositionTarget.BackgroundColor = Colors.Transparent;
        }

        return true;
    }

    public static unsafe bool RemoveTitlebarBackground(Window? window)
    {
        if (window is null)
        {
            return false;
        }

        IntPtr windowHandle = new WindowInteropHelper(window).Handle;

        if (windowHandle == IntPtr.Zero)
        {
            return false;
        }

        HwndSource? windowSource = HwndSource.FromHwnd(windowHandle);

        // Remove background from client area
        if (windowSource?.Handle != IntPtr.Zero && windowSource?.CompositionTarget != null)
        {
            // NOTE: https://learn.microsoft.com/en-us/windows/win32/api/dwmapi/ne-dwmapi-dwmwindowattribute
            // Specifying DWMWA_COLOR_DEFAULT (value 0xFFFFFFFF) for the color will reset the window back to using the system's default behavior for the caption color.
            uint titlebarPvAttribute = PInvoke.DWMWA_COLOR_NONE;

            return PInvoke.DwmSetWindowAttribute(new HWND(windowSource.Handle),
                                                 DWMWINDOWATTRIBUTE.DWMWA_CAPTION_COLOR,
                                                 &titlebarPvAttribute,
                                                 sizeof(uint)) ==
                   HRESULT.S_OK;
        }

        return true;
    }

    private static unsafe bool ApplyDwmwWindowAttribute(IntPtr hWnd, DWM_SYSTEMBACKDROP_TYPE dwmSbt)
    {
        if (hWnd == IntPtr.Zero)
        {
            return false;
        }

        if (!PInvoke.IsWindow(new HWND(hWnd)))
        {
            return false;
        }

        DWM_SYSTEMBACKDROP_TYPE backdropPvAttribute = dwmSbt;

        return PInvoke.DwmSetWindowAttribute(new HWND(hWnd),
                                             DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE,
                                             &backdropPvAttribute,
                                             sizeof(DWM_SYSTEMBACKDROP_TYPE)) ==
               HRESULT.S_OK;
    }

    private static unsafe bool ApplyLegacyMicaBackdrop(IntPtr hWnd)
    {
        BOOL backdropPvAttribute = true;

        return PInvoke.DwmSetWindowAttribute(new HWND(hWnd),
                                             DWMWINDOWATTRIBUTE.DWMWA_MICA_EFFECT,
                                             &backdropPvAttribute,
                                             (uint)sizeof(BOOL)) ==
               HRESULT.S_OK;
    }

    private static bool RestoreContentBackground(IntPtr hWnd)
    {
        if (hWnd == IntPtr.Zero)
        {
            return false;
        }

        if (!PInvoke.IsWindow(new HWND(hWnd)))
        {
            return false;
        }

        var windowSource = HwndSource.FromHwnd(hWnd);

        // Restore client area background
        if (windowSource?.Handle != IntPtr.Zero && windowSource?.CompositionTarget != null)
        {
            windowSource.CompositionTarget.BackgroundColor = SystemColors.WindowColor;
        }

        if (windowSource?.RootVisual is Window window)
        {
            var backgroundBrush = window.Resources["ApplicationBackgroundBrush"];

            // Manual fallback
            if (backgroundBrush is not SolidColorBrush)
            {
                backgroundBrush = GetFallbackBackgroundBrush();
            }

            window.Background = (SolidColorBrush)backgroundBrush;
        }

        return true;
    }

    private static SolidColorBrush GetFallbackBackgroundBrush()
    {
        return ApplicationThemeManager.GetAppTheme() switch
        {
            ApplicationTheme.HighContrast => ApplicationThemeManager.GetSystemTheme() switch
            {
                SystemTheme.HC1 => new SolidColorBrush(Color.FromArgb(0xFF, 0x2D, 0x32, 0x36)),
                SystemTheme.HC2 => new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x00, 0x00)),
                SystemTheme.HCBlack => new SolidColorBrush(Color.FromArgb(0xFF, 0x20, 0x20, 0x20)),
                SystemTheme.HCWhite => new SolidColorBrush(Color.FromArgb(0xFF, 0x20, 0x20, 0x20)),
                _ => new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFA, 0xEF)),
            },
            ApplicationTheme.Dark => new SolidColorBrush(Color.FromArgb(0xFF, 0x20, 0x20, 0x20)),
            ApplicationTheme.Light => new SolidColorBrush(Color.FromArgb(0xFF, 0xFA, 0xFA, 0xFA)),
            _ => new SolidColorBrush(Color.FromArgb(0xFF, 0xFA, 0xFA, 0xFA)),
        };
    }
}
