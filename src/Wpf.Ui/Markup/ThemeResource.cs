// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Markup;

/// <summary>
/// Collection of theme resources.
/// </summary>
#pragma warning disable CS1591
public enum ThemeResource
{
    /// <summary>
    /// Unspecified theme resource.
    /// </summary>
    Unknown,

    // Accents
    SystemAccentColor,
    SystemAccentColorPrimary,
    SystemAccentColorSecondary,
    SystemAccentColorTertiary,
    SystemAccentColorPrimaryBrush,
    SystemAccentColorSecondaryBrush,
    SystemAccentColorTertiaryBrush,

    // Background
    ApplicationBackgroundColor,
    ApplicationBackgroundBrush,

    // Focus
    KeyboardFocusBorderColor,
    KeyboardFocusBorderColorBrush,

    // Text
    TextFillColorPrimary,
    TextFillColorSecondary,
    TextFillColorTertiary,
    TextFillColorDisabled,
    TextFillColorInverse,

    // Text on accent
    AccentTextFillColorDisabled,
    TextOnAccentFillColorSelectedText,
    TextOnAccentFillColorPrimary,
    TextOnAccentFillColorSecondary,
    TextOnAccentFillColorDisabled
}

