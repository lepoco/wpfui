// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Interop;
using WPFUI.Common;

namespace WPFUI.Appearance;

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
            BackgroundType.Auto => Win32.Utilities.IsOSWindows11Insider1OrNewer // Insider with new API
            ,
            BackgroundType.Tabbed => Win32.Utilities.IsOSWindows11Insider1OrNewer
            ,
            BackgroundType.Mica => Win32.Utilities.IsOSWindows11OrNewer
            ,
            BackgroundType.Acrylic => Win32.Utilities.IsOSWindows7OrNewer
            ,
            _ => false
        };
    }

    /// <summary>
    /// Applies selected background effect to <see cref="Window"/> when is rendered.
    /// </summary>
    /// <param name="window">Window to apply effect.</param>
    /// <param name="type">Background type.</param>
    /// <param name="force">Skip the compatibility check.</param>
    public static bool Apply(Window window, BackgroundType type, bool force = false)
    {
        if (!force && !IsSupported(type))
            return false;

        if (window.IsLoaded)
        {
            var windowHandle = new WindowInteropHelper(window).Handle;

            if (windowHandle == IntPtr.Zero)
                return false;

            // Remove currently set background AND TODO: Get rid of WindowChrome and make window transparent with User32.
            window.Background = System.Windows.Media.Brushes.Transparent;

            return Apply(windowHandle, type, force);
        }

        window.Loaded += (_, _) =>
        {
            var windowHandle = new WindowInteropHelper(window).Handle;

            if (windowHandle == IntPtr.Zero)
                return;

            // Remove currently set background AND TODO: Get rid of WindowChrome and make window transparent with User32.
            window.Background = System.Windows.Media.Brushes.Transparent;

            Apply(windowHandle, type, force);
        };

        return true;
    }

    /// <summary>
    /// Applies selected background effect to <c>hWnd</c> by it's pointer.
    /// </summary>
    /// <param name="handle">Pointer to the window handle.</param>
    /// <param name="type">Background type.</param>
    /// <param name="force">Skip the compatibility check.</param>
    public static bool Apply(IntPtr handle, BackgroundType type, bool force = false)
    {
        if (!force && !IsSupported(type))
            return false;

        if (handle == IntPtr.Zero)
            return false;

        if (!UnsafeNativeMethods.RemoveWindowTitlebar(handle))
            return false;

        if (Theme.GetAppTheme() == ThemeType.Dark)
            UnsafeNativeMethods.ApplyWindowDarkMode(handle);
        else
            UnsafeNativeMethods.RemoveWindowDarkMode(handle);

        if (!AppearanceData.Handlers.Contains(handle))
            AppearanceData.Handlers.Add(handle);

        // TODO: Apply legacy Acrylic

        if (!Win32.Utilities.IsOSWindows11Insider1OrNewer)
            return UnsafeNativeMethods.ApplyWindowLegacyMicaEffect(handle);

        return UnsafeNativeMethods.ApplyWindowBackdrop(handle, type);
    }

    /// <summary>
    /// Tries to remove background effects if they have been applied to the <see cref="Window"/>.
    /// </summary>
    /// <param name="window">The window from which the effect should be removed.</param>
    public static void Remove(Window window)
        => Remove(new WindowInteropHelper(window).Handle);

    /// <summary>
    /// Tries to remove all effects if they have been applied to the <c>hWnd</c>.
    /// </summary>
    /// <param name="handle">Pointer to the window handle.</param>
    public static void Remove(IntPtr handle)
    {
        if (handle == IntPtr.Zero)
            return;

        UnsafeNativeMethods.RemoveWindowBackdrop(handle);

        for (int i = 0; i < AppearanceData.Handlers.Count; i++)
        {
            if (AppearanceData.Handlers[i] != handle)
                continue;

            AppearanceData.Handlers.RemoveAt(i);

            break;
        }
    }
}
