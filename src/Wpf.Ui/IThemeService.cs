// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Appearance;

namespace Wpf.Ui;

/// <summary>
/// Represents a contract with a service that provides tools for manipulating the theme.
/// </summary>
public interface IThemeService
{
    /// <summary>
    /// Gets current application theme.
    /// </summary>
    /// <returns>Currently set application theme.</returns>
    ApplicationTheme GetTheme();

    /// <summary>
    /// Gets current system theme.
    /// </summary>
    /// <returns>Currently set Windows theme.</returns>
    ApplicationTheme GetSystemTheme();

    /// <summary>
    /// Gets current system theme.
    /// </summary>
    /// <returns>Currently set Windows theme using system enumeration.</returns>
    SystemTheme GetNativeSystemTheme();

    /// <summary>
    /// Sets current application theme.
    /// </summary>
    /// <param name="applicationTheme">Theme type to set.</param>
    /// <returns><see langword="true"/> if the operation succeeds. <see langword="false"/> otherwise.</returns>
    bool SetTheme(ApplicationTheme applicationTheme);

    /// <summary>
    /// Sets currently used Windows OS accent.
    /// </summary>
    /// <returns><see langword="true"/> if the operation succeeds. <see langword="false"/> otherwise.</returns>
    bool SetSystemAccent();

    /// <summary>
    /// Sets current application accent.
    /// </summary>
    /// <returns><see langword="true"/> if the operation succeeds. <see langword="false"/> otherwise.</returns>
    bool SetAccent(Color accentColor);

    /// <summary>
    /// Sets current application accent.
    /// </summary>
    /// <returns><see langword="true"/> if the operation succeeds. <see langword="false"/> otherwise.</returns>
    bool SetAccent(SolidColorBrush accentSolidBrush);
}
