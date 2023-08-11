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
public static class TextBlockFontTypographyExtensions
{
    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="typography"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static string ToResourceValue(this FontTypography typography) =>
        typography switch
        {
            FontTypography.Caption => "CaptionTextBlockStyle",
            FontTypography.Body => "BodyTextBlockStyle",
            FontTypography.BodyStrong => "BodyStrongTextBlockStyle",
            FontTypography.Subtitle => "SubtitleTextBlockStyle",
            FontTypography.Title => "TitleTextBlockStyle",
            FontTypography.TitleLarge => "TitleLargeTextBlockStyle",
            FontTypography.Display => "DisplayTextBlockStyle",
            _ => throw new ArgumentOutOfRangeException(nameof(typography), typography, null)
        };
}
