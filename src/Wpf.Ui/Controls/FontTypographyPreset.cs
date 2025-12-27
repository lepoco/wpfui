// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Markup;

namespace Wpf.Ui.Controls;

/// <summary>
/// Represents a named typography preset containing font metrics used by controls
/// (for example, font size and weight). Presets are intended to be stored as
/// resources and referenced by the control's <c>FontTypography</c> mapping.
/// </summary>
/// <remarks>
/// Example:
/// <code lang="xml">
/// &lt;ResourceDictionary
///     xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
///     xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
///     xmlns:controls="clr-namespace:Wpf.Ui.Controls"&gt;
///
///   &lt;controls:FontTypographyPreset x:Key="BodyTextBlockStyle" FontSize="14" FontWeight="Regular" /&gt;
///
/// &lt;/ResourceDictionary&gt;
///
/// &lt;!-- TextBlock resolves the preset by resource key produced from the FontTypography enum --&gt;
/// &lt;ui:TextBlock FontTypography="Body" /&gt;
/// </code>
/// </remarks>
[MarkupExtensionReturnType(typeof(FontTypographyPreset))]
public class FontTypographyPreset : MarkupExtension
{
    /// <summary>
    /// Gets or sets the font size for this typography style, measured in device-independent units (1/96 inch).
    /// If this property is <c>null</c>, no font size override will be applied from this style.
    /// </summary>
    public double? FontSize { get; set; }

    /// <summary>
    /// Gets or sets the font weight defined by this typography preset.
    /// A <c>null</c> value indicates that no specific font weight should be applied.
    /// </summary>
    public FontWeight? FontWeight { get; set; }

    /*
       Note: Excluding LineHeight intentionally. WPF and WinUI have fundamentally
       different text rendering engines - identical FontSize/LineHeight pairs would
       break vertical text alignment and create maintenance nightmares.
    */

    /// <summary>
    /// Returns this instance when used in XAML so the preset can be declared as a resource.
    /// </summary>
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }
}
