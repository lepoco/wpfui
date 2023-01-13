// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Media;
using Wpf.Ui.Appearance;

namespace Wpf.Ui.Contracts;

/// <summary>
/// Represents a contract with a service that provides tools for manipulating the theme.
/// </summary>
public interface IThemeService
{
    /// <summary>
    /// Gets current application theme.
    /// </summary>
    ThemeType GetTheme();

    /// <summary>
    /// Gets current system theme.
    /// </summary>
    ThemeType GetSystemTheme();

    /// <summary>
    /// Gets current system theme.
    /// </summary>
    SystemThemeType GetNativeSystemTheme();

    /// <summary>
    /// Sets current application theme.
    /// </summary>
    /// <param name="themeType">Theme type to set.</param>
    bool SetTheme(ThemeType themeType);

    /// <summary>
    /// Sets currently used Windows OS accent.
    /// </summary>
    bool SetSystemAccent();

    /// <summary>
    /// Sets current application accent.
    /// </summary>
    bool SetAccent(Color accentColor);

    /// <summary>
    /// Sets current application accent.
    /// </summary>
    bool SetAccent(SolidColorBrush accentSolidBrush);
}

