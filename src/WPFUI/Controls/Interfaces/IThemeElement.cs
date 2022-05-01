// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace WPFUI.Controls.Interfaces;

internal interface IThemeElement
{
    /// <summary>
    /// Indicates whether the application has a Mica effect applied at the moment.
    /// </summary>
    public int IsMica { get; }

    /// <summary>
    /// Indicates whether the application is in dark mode.
    /// </summary>
    public int IsDarkTheme { get; }
}
