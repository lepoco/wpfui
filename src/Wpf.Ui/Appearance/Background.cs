// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using Wpf.Ui.Interop;

namespace Wpf.Ui.Appearance;

/// <summary>
/// Lets you apply background effects to <see cref="Window"/> or <c>hWnd</c> by its <see cref="IntPtr"/>.
/// </summary>
public static class Background
{
    /// <summary>
    /// Checks if the current <see cref="Windows"/> supports selected <see cref="BackgroundType"/>.
    /// </summary>
    /// <param name="type">Background type to check.</param>
    /// <returns><see langword="true"/> if <see cref="BackgroundType"/> is supported.</returns>
    public static bool IsSupported(BackgroundType type)
    {
        return type switch
        {
            BackgroundType.Auto => Win32.Utilities.IsOSWindows11Insider1OrNewer, // Insider with new API
            BackgroundType.Tabbed => Win32.Utilities.IsOSWindows11Insider1OrNewer,
            BackgroundType.Mica => Win32.Utilities.IsOSWindows11OrNewer,
            BackgroundType.Acrylic => Win32.Utilities.IsOSWindows7OrNewer,
            BackgroundType.Unknown => true,
            _ => false
        };
    }

    /// <summary>
    /// Applies selected background effect to <see cref="Window"/> when is rendered.
    /// </summary>
    /// <param name="window">Window to apply effect.</param>
    /// <param name="type">Background type.</param>
    public static bool Apply(Window window, BackgroundType type)
        => Apply(window, type, false);

    /// <summary>
    /// Applies selected background effect to <see cref="Window"/> when is rendered.
    /// </summary>
    /// <param name="window">Window to apply effect.</param>
    /// <param name="type">Background type.</param>
    /// <param name="force">Skip the compatibility check.</param>
    public static bool Apply(Window window, BackgroundType type, bool force)
    {
        if (!force && !IsSupported(type))
            return false;

        if (window.IsLoaded)
        {
            var windowHandle = new WindowInteropHelper(window).Handle;

            if (windowHandle == IntPtr.Zero)
                return false;

            // Remove currently set background of the window and it's composition area
            RemoveContentBackground(window);

            return Apply(windowHandle, type, force);
        }

        window.Loaded += (sender, _) =>
        {
            var windowHandle = new WindowInteropHelper(sender as Window).Handle;

            if (windowHandle == IntPtr.Zero)
                return;

            // Remove currently set background of the window and it's composition area
            RemoveContentBackground(sender as Window);

            Apply(windowHandle, type, force);
        };

        return true;
    }

    /// <summary>
    /// Applies selected background effect to <c>hWnd</c> by it's pointer.
    /// </summary>
    /// <param name="handle">Pointer to the window handle.</param>
    /// <param name="type">Background type.</param>
    public static bool Apply(IntPtr handle, BackgroundType type)
        => Apply(handle, type, false);

    /// <summary>
    /// Applies selected background effect to <c>hWnd</c> by it's pointer.
    /// </summary>
    /// <param name="handle">Pointer to the window handle.</param>
    /// <param name="type">Background type.</param>
    /// <param name="force">Skip the compatibility check.</param>
    public static bool Apply(IntPtr handle, BackgroundType type, bool force)
    {
        if (!force && !IsSupported(type))
            return false;

        if (!force && !UnsafeNativeMethods.IsCompositionEnabled())
            return false;

        if (handle == IntPtr.Zero)
            return false;

        if (type == BackgroundType.Unknown)
        {
            Remove(handle);

            return true;
        }

        //if (!UnsafeNativeMethods.RemoveWindowTitlebar(handle))
        //    return false;

        if (Theme.GetAppTheme() == ThemeType.Dark)
            UnsafeNativeMethods.ApplyWindowDarkMode(handle);
        else
            UnsafeNativeMethods.RemoveWindowDarkMode(handle);


        // Caption of the window should be removed, does not respect dark theme
        UnsafeNativeMethods.RemoveWindowCaption(handle);

        AppearanceData.AddHandle(handle);

        // First release of Windows 11
        if (!Win32.Utilities.IsOSWindows11Insider1OrNewer)
        {
            if (!(type == BackgroundType.Mica || type == BackgroundType.Auto))
                return false;

            // TODO: Apply legacy Acrylic
            //if (type == BackgroundType.Acrylic)
            //    return UnsafeNativeMethods.ApplyWindowLegacyAcrylicEffect(handle, type);

            return UnsafeNativeMethods.ApplyWindowLegacyMicaEffect(handle);
        }

        // Newer Windows 11 versions
        return UnsafeNativeMethods.ApplyWindowBackdrop(handle, type);
    }

    /// <summary>
    /// Tries to remove background effects if they have been applied to the <see cref="Window"/>.
    /// </summary>
    /// <param name="window">The window from which the effect should be removed.</param>
    public static bool Remove(Window window)
    {
        if (window == null)
            return false;

        var windowHandle = new WindowInteropHelper(window).Handle;

        RestoreContentBackground(window);

        if (windowHandle == IntPtr.Zero)
            return false;

        UnsafeNativeMethods.RemoveWindowBackdrop(windowHandle);

        if (AppearanceData.HasHandle(windowHandle))
            AppearanceData.RemoveHandle(windowHandle);

        return true;
    }

    /// <summary>
    /// Tries to remove all effects if they have been applied to the <c>hWnd</c>.
    /// </summary>
    /// <param name="handle">Pointer to the window handle.</param>
    public static bool Remove(IntPtr handle)
    {
        if (handle == IntPtr.Zero)
            return false;

        RestoreContentBackground(handle);

        UnsafeNativeMethods.RemoveWindowBackdrop(handle);

        if (AppearanceData.HasHandle(handle))
            AppearanceData.RemoveHandle(handle);

        return true;
    }

    /// <summary>
    /// Tries to remove background from <see cref="Window"/> and it's composition area.
    /// </summary>
    /// <param name="window">Window to manipulate.</param>
    /// <returns><see langword="true"/> if operation was successful.</returns>
    public static bool RemoveContentBackground(Window window)
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

    /// <summary>
    /// Tries to restore default background for <see cref="Window"/> and it's composition target.
    /// </summary>
    /// <param name="window">Window to manipulate.</param>
    /// <returns><see langword="true"/> if operation was successful.</returns>
    public static bool RestoreContentBackground(Window window)
    {
        if (window == null)
            return false;

        // Global resources
        var backgroundBrush = Application.Current.Resources["ApplicationBackgroundBrush"];

        // Local resources
        if (backgroundBrush is not SolidColorBrush)
            backgroundBrush = window.Resources["ApplicationBackgroundBrush"];

        // Manual fallback
        if (backgroundBrush is not SolidColorBrush)
            backgroundBrush = Theme.GetAppTheme() == ThemeType.Dark
                ? new SolidColorBrush(Color.FromArgb(0xFF, 0x20, 0x20, 0x20))
                : new SolidColorBrush(Color.FromArgb(0xFF, 0xFA, 0xFA, 0xFA));

        window.Background = (SolidColorBrush)backgroundBrush;

        var windowHandle = new WindowInteropHelper(window).Handle;

        if (windowHandle == IntPtr.Zero)
            return false;

        var windowSource = HwndSource.FromHwnd(windowHandle);

        Appearance.Background.Remove(windowHandle);

        // Restore client area
        if (windowSource?.Handle != IntPtr.Zero && windowSource?.CompositionTarget != null)
            windowSource.CompositionTarget.BackgroundColor = SystemColors.WindowColor;

        return true;
    }

    /// <summary>
    /// Tries to restore default background for <see cref="Window"/> composition target, based on it's handle.
    /// </summary>
    /// <param name="hWnd">Window handle.</param>
    /// <returns><see langword="true"/> if operation was successful.</returns>
    public static bool RestoreContentBackground(IntPtr hWnd)
    {
        if (hWnd == IntPtr.Zero)
            return false;

        if (!UnsafeNativeMethods.IsValidWindow(hWnd))
            return false;

        var windowSource = HwndSource.FromHwnd(hWnd);

        // Restore client area
        if (windowSource?.Handle != IntPtr.Zero && windowSource?.CompositionTarget != null)
            windowSource.CompositionTarget.BackgroundColor = SystemColors.WindowColor;

        if (windowSource?.RootVisual is Window window)
        {
            var backgroundBrush = window.Resources["ApplicationBackgroundBrush"];

            // Manual fallback
            if (backgroundBrush is not SolidColorBrush)
                backgroundBrush = Theme.GetAppTheme() == ThemeType.Dark
                    ? new SolidColorBrush(Color.FromArgb(0xFF, 0x20, 0x20, 0x20))
                    : new SolidColorBrush(Color.FromArgb(0xFF, 0xFA, 0xFA, 0xFA));

            window.Background = (SolidColorBrush)backgroundBrush;
        }

        return true;
    }

    internal static void RemoveAll()
    {
        var handles = AppearanceData.ModifiedBackgroundHandles;

        foreach (var singleHandle in handles)
        {
            if (!UnsafeNativeMethods.IsValidWindow(singleHandle))
                continue;

            Remove(singleHandle);

            AppearanceData.RemoveHandle(singleHandle);
        }
    }

    internal static void UpdateAll(ThemeType themeType,
        BackgroundType backdropType = BackgroundType.Unknown)
    {
        var handles = AppearanceData.ModifiedBackgroundHandles;

        foreach (var singleHandle in handles)
        {
            if (!UnsafeNativeMethods.IsValidWindow(singleHandle))
                continue;

            if (themeType == ThemeType.Dark)
                UnsafeNativeMethods.ApplyWindowDarkMode(singleHandle);
            else
                UnsafeNativeMethods.RemoveWindowDarkMode(singleHandle);

            if (Win32.Utilities.IsOSWindows11Insider1OrNewer)
            {
                if (!UnsafeNativeMethods.IsWindowHasBackdrop(singleHandle, backdropType))
                    UnsafeNativeMethods.ApplyWindowBackdrop(singleHandle, backdropType);

                continue;
            }

            if (backdropType == BackgroundType.Mica)
            {
                if (!UnsafeNativeMethods.IsWindowHasLegacyMica(singleHandle))
                    UnsafeNativeMethods.ApplyWindowLegacyMicaEffect(singleHandle);
            }

            // TODO: Legacy acrylic effect

            //if (backdropType == BackgroundType.Acrylic)
            //{
            //    if (!UnsafeNativeMethods.IsWindowHasLegacyAcrylic(singleHandle))
            //        UnsafeNativeMethods.ApplyWindowLegacyAcrylicEffect(singleHandle);
            //}
        }
    }
}
