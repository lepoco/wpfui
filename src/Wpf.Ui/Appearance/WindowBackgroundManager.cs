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
/// <example>
/// <code lang="csharp">
/// WindowBackgroundManager.UpdateBackground(
///     observedWindow.RootVisual,
///     currentApplicationTheme,
///     observedWindow.Backdrop
/// );
/// </code>
/// </example>
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

    [Obsolete("Use UpdateBackground(Window, ApplicationTheme, WindowBackdropType) instead.")]
    public static void UpdateBackground(Window? window, ApplicationTheme applicationTheme, WindowBackdropType backdrop, bool forceBackground)
    {
        UpdateBackground(window, applicationTheme, backdrop);
    }

    /// <summary>
    /// Forces change to application background. Required if custom background effect was previously applied.
    /// </summary>
    public static void UpdateBackground(
        Window? window,
        ApplicationTheme applicationTheme,
        WindowBackdropType backdrop
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

        // This was required to update the background when moving from a HC theme to light/dark theme. However, this breaks theme proper light/dark theme changing on Windows 10.
        // else
        // {
        //    _ = WindowBackdrop.RemoveBackground(window);
        // }
        _ = WindowBackdrop.ApplyBackdrop(window, backdrop);
        if (applicationTheme is ApplicationTheme.Dark)
        {
            ApplyDarkThemeToWindow(window);
        }
        else
        {
            RemoveDarkThemeFromWindow(window);
        }

        foreach (object? subWindow in window.OwnedWindows)
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
    }
}
