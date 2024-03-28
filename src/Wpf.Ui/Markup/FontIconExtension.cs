// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Markup;
using Wpf.Ui.Controls;

namespace Wpf.Ui.Markup;

/// <summary>
/// Custom <see cref="MarkupExtension"/> which can provide <see cref="FontIcon"/>.
/// </summary>
/// <example>
/// <code lang="xml">
/// &lt;ui:Button
///     Appearance="Primary"
///     Content="WPF UI button with font icon"
///     Icon="{ui:FontIcon '&#x1F308;'}" /&gt;
/// </code>
/// <code lang="xml">
/// &lt;ui:Button Icon="{ui:FontIcon '&amp;#x1F308;'}" /&gt;
/// </code>
/// <code lang="xml">
/// &lt;ui:HyperlinkButton Icon="{ui:FontIcon '&amp;#x1F308;'}" /&gt;
/// </code>
/// <code lang="xml">
/// &lt;ui:TitleBar Icon="{ui:FontIcon '&amp;#x1F308;'}" /&gt;
/// </code>
/// </example>
[ContentProperty(nameof(Glyph))]
[MarkupExtensionReturnType(typeof(FontIcon))]
public class FontIconExtension : MarkupExtension
{
    public FontIconExtension()
    {
    }

    public FontIconExtension(string glyph)
    {
        Glyph = glyph;
    }

    [ConstructorArgument("glyph")]
    public string? Glyph { get; set; }

    [ConstructorArgument("fontFamily")]
    public FontFamily FontFamily { get; set; } = new("FluentSystemIcons");

    public double FontSize { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget { TargetObject: Setter })
        {
            return this;
        }

        FontIcon fontIcon = new() { Glyph = Glyph, FontFamily = FontFamily };

        if (FontSize > 0)
        {
            fontIcon.FontSize = FontSize;
        }

        return fontIcon;
    }
}