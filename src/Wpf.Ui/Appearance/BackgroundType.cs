// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Appearance;

/// <summary>
/// Collection of fluent background types.
/// </summary>
public enum BackgroundType
{
    /// <summary>
    /// Unknown background type.
    /// </summary>
    Unknown,

    /// <summary>
    /// No backdrop effect.
    /// </summary>
    None,

    /// <summary>
    /// Sets <c>DWMWA_SYSTEMBACKDROP_TYPE</c> to <see langword="0"></see>.
    /// </summary>
    Auto,

    /// <summary>
    /// Windows 11 Mica effect.
    /// </summary>
    Mica,

    /// <summary>
    /// Windows Acrylic effect.
    /// </summary>
    Acrylic,

    /// <summary>
    /// Windows 11 wallpaper blur effect.
    /// </summary>
    Tabbed
}
