using System;
using Wpf.Ui.Common;

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
