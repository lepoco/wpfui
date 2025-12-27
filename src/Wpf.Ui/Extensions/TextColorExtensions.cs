// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Controls;

namespace Wpf.Ui.Extensions;

/// <summary>
/// Extension that converts the text color type enumeration to the name of the resource that represents it.
/// </summary>
public static class TextColorExtensions
{
    /// <summary>
    /// Converts the text color type enumeration to the name of the resource that represents it.
    /// </summary>
    /// <returns>Name of the resource matching the <see cref="TextColor"/>. <see cref="ArgumentOutOfRangeException"/> otherwise.</returns>
    public static string ToResourceKey(this TextColor textColor)
    {
        return textColor switch
        {
            TextColor.Primary => "TextFillColorPrimaryBrush",
            TextColor.Secondary => "TextFillColorSecondaryBrush",
            TextColor.Tertiary => "TextFillColorTertiaryBrush",
            TextColor.Disabled => "TextFillColorDisabledBrush",
            _ => throw new ArgumentOutOfRangeException(nameof(textColor), textColor, null),
        };
    }
}
