// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Controls;
using Wpf.Ui.Interop;

namespace Wpf.Ui.Appearance;

/// <summary>
/// Facilitates the management of the window background.
/// </summary>
public static class WindowBackgroundManager
{
    /// <summary>
    /// Tries to apply dark theme to <see cref="Window"/>.
    /// </summary>
    public static void ApplyDarkThemeToWindow(Window? window)
    {
        if (window is null)
        {
            return;
        }

        if (window.IsLoaded)
        {
            _ = UnsafeNativeMethods.ApplyWindowDarkMode(window);
        }

        window.Loaded += (sender, _) => UnsafeNativeMethods.ApplyWindowDarkMode(sender as Window);
    }

    /// <summary>
    /// Tries to remove dark theme from <see cref="Window"/>.
    /// </summary>
    public static void RemoveDarkThemeFromWindow(Window? window)
    {
        if (window is null)
        {
            return;
        }

        if (window.IsLoaded)
        {
            _ = UnsafeNativeMethods.RemoveWindowDarkMode(window);
        }

        window.Loaded += (sender, _) => UnsafeNativeMethods.RemoveWindowDarkMode(sender as Window);
    }

    /// <summary>
    /// Forces change to application background. Required if custom background effect was previously applied.
    /// </summary>
    public static void UpdateBackground(
        Window? window,
        ApplicationTheme applicationTheme,
        WindowBackdropType backdrop,
        bool forceBackground
    )
    {
        if (window is null)
        {
            return;
        }

        _ = WindowBackdrop.RemoveBackdrop(window);

        if (applicationTheme == ApplicationTheme.HighContrast)
        {
            backdrop = WindowBackdropType.None;
        }
        else
        {
            _ = WindowBackdrop.RemoveBackground(window);
        }

        _ = WindowBackdrop.ApplyBackdrop(window, backdrop);
        if (applicationTheme is ApplicationTheme.Dark)
        {
            ApplyDarkThemeToWindow(window);
        }
        else
        {
            RemoveDarkThemeFromWindow(window);
        }

        foreach (var subWindow in window.OwnedWindows)
        {
            if (subWindow is Window windowSubWindow)
            {
                _ = WindowBackdrop.ApplyBackdrop(windowSubWindow, backdrop);

                if (applicationTheme is ApplicationTheme.Dark)
                {
                    ApplyDarkThemeToWindow(windowSubWindow);
                }
                else
                {
                    RemoveDarkThemeFromWindow(windowSubWindow);
                }
            }
        }

        // Do we really neeed this?
        //if (!Win32.Utilities.IsOSWindows11OrNewer)
        //{
        //    var mainWindow = Application.Current.MainWindow;

        //    if (mainWindow == null)
        //        return;

        //    var backgroundColor = Application.Current.Resources["ApplicationBackgroundColor"];
        //    if (backgroundColor is Color color)
        //        mainWindow.Background = new SolidColorBrush(color);
        //}


        //        var mainWindow = Application.Current.MainWindow;

        //        if (mainWindow == null)
        //            return;

        //        // TODO: Do not refresh window presenter background if already applied
        //        var backgroundColor = Application.Current.Resources["ApplicationBackgroundColor"];
        //        if (backgroundColor is Color color)
        //            mainWindow.Background = new SolidColorBrush(color);

        //#if DEBUG
        //        System.Diagnostics.Debug.WriteLine($"INFO | Current background color: {backgroundColor}", "Wpf.Ui.Theme");
        //#endif

        //        var windowHandle = new WindowInteropHelper(mainWindow).Handle;

        //        if (windowHandle == IntPtr.Zero)
        //            return;

        //        Background.Remove(windowHandle);

        //        //if (!IsAppMatchesSystem() || backgroundEffect == BackgroundType.Unknown)
        //        //    return;

        //        if (backgroundEffect == BackgroundType.Unknown)
        //            return;

        //        // TODO: Improve
        //        if (Background.Apply(windowHandle, backgroundEffect, forceBackground))
        //            mainWindow.Background = Brushes.Transparent;
    }
}
