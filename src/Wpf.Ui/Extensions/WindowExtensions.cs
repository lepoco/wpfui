// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using Wpf.Ui.Appearance;
using Wpf.Ui.Interop;

namespace Wpf.Ui.Extensions;

/// <summary>
/// Set of extensions for <see cref="Window"/>.
/// </summary>
public static class WindowExtensions
{
    #region Styles

    //public static Window GandalfDoMagic(this Window window)
    //{
    //    window.Loaded += (sender, args) =>
    //    {
    //        var windowHandle = new WindowInteropHelper(window).Handle;

    //        var styles = (long)(Interop.User32.WS.CAPTION);
    //        //var styles = (long)(User32.WINDOW_STYLE.WS_BORDER | User32.WINDOW_STYLE.WS_SIZEFRAME);
    //        var exStyles = (long)(Interop.User32.WS_EX.CLIENTEDGE | Interop.User32.WS_EX.LAYERED | Interop.User32.WS_EX.TRANSPARENT);

    //        //User32.SetWindowLong(windowHandle, User32.WINDOWLONGFLAG.GWL_STYLE, styles);
    //        Interop.User32.SetWindowLong(windowHandle, Interop.User32.GWL.GWL_EXSTYLE, exStyles);
    //    };

    //    return window;
    //}

    /// <summary>
    /// Tries to aplly backdrop effect to selected <see cref="Window"/>.
    /// </summary>
    /// <param name="window"></param>
    /// <param name="backgroundType"></param>
    /// <returns></returns>
    public static Window ApplyBackdrop(this Window window, BackgroundType backgroundType)
    {
        Appearance.Background.Apply(window, backgroundType);

        return window;
    }

    public static Window RemoveAllStyles(this Window window)
    {
        if (window.IsLoaded)
            RemoveWindowStyles(window);
        else
            window.Loaded += WindowRemoveStylesOnLoaded;

        return window;
    }

    private static void WindowRemoveStylesOnLoaded(object sender, RoutedEventArgs e)
    {
        RemoveWindowStyles(sender as Window);
    }

    private static void RemoveWindowStyles(Window window)
    {
        var windowHandle = new WindowInteropHelper(window).Handle;

        // Default WPF window style NONE is WS_CAPTION
        Interop.User32.SetWindowLong(windowHandle, Interop.User32.GWL.GWL_STYLE, (long)Interop.User32.WS.BORDER);
    }

    #endregion

    #region Titlebar

    /// <summary>
    /// Tries to remove the default titlebar.
    /// </summary>
    /// <param name="window">Selected window.</param>
    public static Window RemoveTitlebar(this Window window)
    {
        if (window.IsLoaded)
            UnsafeNativeMethods.RemoveWindowTitlebar(window);
        else
            window.Loaded += (sender, args) => UnsafeNativeMethods.RemoveWindowTitlebar(window);

        return window;
    }

    #endregion

    #region Background

    /// <summary>
    /// Tries to set the default window background color for the selected theme.
    /// </summary>
    /// <param name="window">Selected window.</param>
    public static Window ApplyDefaultBackground(this Window window)
    {
        var applicationBackgroundRaw = Application.Current.Resources["ApplicationBackgroundColor"];

        if (applicationBackgroundRaw is not Color backgroundColor)
            return window;

        window.Background = new SolidColorBrush { Color = backgroundColor };

        return window;
    }

    #endregion

    #region Mica

    public static void ApplyBackgroundEffect(this Window window)
    {
    }

    #endregion

    #region Corners

    /// <summary>
    /// Tries to round the <see cref="Window"/> corners.
    /// </summary>
    /// <param name="window">Selected window.</param>
    /// <param name="cornerPreference">Window corner preference.</param>
    public static Window ApplyCorners(this Window window, WindowCornerPreference cornerPreference)
    {
        if (window.IsLoaded)
            UnsafeNativeMethods.ApplyWindowCornerPreference(window, cornerPreference);
        else
            window.Loaded += (sender, args) => UnsafeNativeMethods.ApplyWindowCornerPreference(window, cornerPreference);

        return window;
    }

    #endregion
}
