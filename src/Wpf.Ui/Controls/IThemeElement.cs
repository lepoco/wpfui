// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Controls;

/// <summary>
/// Control changing its properties or appearance depending on the theme.
/// </summary>
public interface IThemeControl
{
    /// <summary>
    /// The theme is currently set.
    /// </summary>
    public Appearance.ThemeType Theme { get; }
}
