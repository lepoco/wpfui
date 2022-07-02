// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Interop;

namespace Wpf.Ui.Appearance;

/// <summary>
/// Static container for appearance data.
/// </summary>
internal static class AppearanceData
{
    /// <summary>
    /// Collection of handles that have a background effect applied.
    /// </summary>
    public static List<IntPtr> ModifiedBackgroundHandles = new();

    /// <summary>
    /// Namespace for the XAML dictionaries.
    /// </summary>
    public const string LibraryNamespace = "ui;";

    /// <summary>
    /// Main dictionary for WPF UI controls.
    /// </summary>
    public const string LibraryMainDictionary = "Wpf.Ui";

    /// <summary>
    /// Default <see cref="System.Uri"/> for the application theme dictionaries.
    /// </summary>
    public const string LibraryThemeDictionariesUri = "pack://application:,,,/Wpf.Ui;component/Styles/Theme/";

    /// <summary>
    /// Default <see cref="System.Uri"/> for the application theme dictionaries.
    /// </summary>
    public const string LibraryDictionariesUri = "pack://application:,,,/Wpf.Ui;component/Styles/";

    /// <summary>
    /// Current system theme.
    /// </summary>
    public static SystemThemeType SystemTheme = SystemThemeType.Unknown;

    /// <summary>
    /// Current application theme.
    /// </summary>
    public static ThemeType ApplicationTheme = ThemeType.Unknown;

    /// <summary>
    /// Adds given window to list of modified handles.
    /// </summary>
    public static void AddHandle(Window window)
    {
        AddHandle(new WindowInteropHelper(window).Handle);
    }

    /// <summary>
    /// Adds given handle to list of modified handles.
    /// </summary>
    public static void AddHandle(IntPtr hWnd)
    {
        if (!ModifiedBackgroundHandles.Contains(hWnd))
            ModifiedBackgroundHandles.Add(hWnd);
    }

    /// <summary>
    /// Removes given window from list of modified handles.
    /// </summary>
    public static void RemoveHandle(Window window)
    {
        RemoveHandle(new WindowInteropHelper(window).Handle);
    }

    /// <summary>
    /// Removes given handle from list of modified handles.
    /// </summary>
    public static void RemoveHandle(IntPtr hWnd)
    {
        if (!ModifiedBackgroundHandles.Contains(hWnd))
            ModifiedBackgroundHandles.Remove(hWnd);
    }

    /// <summary>
    /// Returns a value indicating whether the given window had a modified background.
    /// </summary>
    public static bool HasHandle(Window window)
    {
        return HasHandle(new WindowInteropHelper(window).Handle);
    }

    /// <summary>
    /// Returns a value indicating whether the given handle had a modified background.
    /// </summary>
    public static bool HasHandle(IntPtr hWnd)
    {
        return ModifiedBackgroundHandles.Contains(hWnd);
    }
}
