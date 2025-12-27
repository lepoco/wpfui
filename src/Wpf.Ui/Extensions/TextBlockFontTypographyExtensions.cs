// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Controls;

namespace Wpf.Ui.Extensions;

/// <summary>
/// Extension that converts the typography type enumeration to the name of the resource that represents it.
/// </summary>
public static class TextBlockFontTypographyExtensions
{
    /// <summary>
    ///  Converts the typography type enumeration to the name of the resource that represents it.
    /// </summary>
    /// <returns>Name of the resource matching the <see cref="FontTypography"/>. <see cref="ArgumentOutOfRangeException"/> otherwise.</returns>
    public static string ToResourceKey(this FontTypography typography)
    {
        return typography switch
        {
            FontTypography.Caption => "CaptionFontTypography",
            FontTypography.Body => "BodyFontTypography",
            FontTypography.BodyStrong => "BodyStrongFontTypography",
            FontTypography.Subtitle => "SubtitleFontTypography",
            FontTypography.Title => "TitleFontTypography",
            FontTypography.TitleLarge => "TitleLargeFontTypography",
            FontTypography.Display => "DisplayFontTypography",
            _ => throw new ArgumentOutOfRangeException(nameof(typography), typography, null),
        };
    }
}
