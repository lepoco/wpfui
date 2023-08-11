// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using Wpf.Ui.Controls;

namespace Wpf.Ui.Extensions;

/// <summary>
/// TODO
/// </summary>
public static class TextColorExtensions
{
    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="textColor"></param>
    /// <returns></returns>
    public static string ToResourceValue(this TextColor textColor) =>
        textColor switch
        {
            TextColor.Primary => "TextFillColorPrimaryBrush",
            TextColor.Secondary => "TextFillColorSecondaryBrush",
            TextColor.Tertiary => "TextFillColorTertiaryBrush",
            TextColor.Disabled => "TextFillColorDisabledBrush",
            _ => throw new ArgumentOutOfRangeException(nameof(textColor), textColor, null)
        };
}
