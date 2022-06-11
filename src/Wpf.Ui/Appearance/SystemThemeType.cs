// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Appearance;

/// <summary>
/// Collection of Windows 11 themes.
/// </summary>
public enum SystemThemeType
{
    /// <summary>
    /// Unknown Windows theme.
    /// </summary>
    Unknown,

    /// <summary>
    /// Custom Windows theme.
    /// </summary>
    Custom,

    /// <summary>
    /// Default light theme.
    /// </summary>
    Light,

    /// <summary>
    /// Default dark theme.
    /// </summary>
    Dark,

    /// <summary>
    /// First custom, kinda purple Windows 11 theme.
    /// </summary>
    Glow,

    /// <summary>
    /// Second custom, kinda red Windows 11 theme.
    /// </summary>
    CapturedMotion,

    /// <summary>
    /// Third custom, kinda washed off cyan Windows 11 theme.
    /// </summary>
    Sunrise,

    /// <summary>
    /// Fourth custom, kinda gray Windows 11 theme.
    /// </summary>
    Flow
}
