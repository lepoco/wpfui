// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;
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
    public FontIconExtension() { }

    public FontIconExtension(string glyph)
    {
        Glyph = glyph;
    }

    public FontIconExtension(string glyph, double fontSize)
        : this(glyph)
    {
        FontSize = fontSize;
    }

    public FontIconExtension(string glyph, FontFamily fontFamily, double fontSize)
        : this(glyph, fontSize)
    {
        FontFamily = fontFamily;
    }

    [ConstructorArgument("glyph")]
    public required string Glyph { get; set; }

    [ConstructorArgument("fontFamily")]
    public FontFamily FontFamily { get; set; } = new("FluentSystemIcons");

    [ConstructorArgument("fontSize")]
    public double? FontSize { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (serviceProvider.GetService(typeof(IProvideValueTarget)) is not IProvideValueTarget provideValueTarget)
        {
            return new FontIcon
            {
                FontFamily = FontFamily,
                Glyph = Glyph,
                FontSize = FontSize ?? SystemFonts.MessageFontSize
            };
        }

        if (provideValueTarget.TargetObject is Setter)
        {
            return this;
        }

        FontIcon fontIcon = new()
        {
            FontFamily = FontFamily,
            Glyph = Glyph,
        };

        if (FontSize.HasValue)
        {
            fontIcon.FontSize = FontSize.Value;
        }

        if (provideValueTarget.TargetObject is not FrameworkElement targetElement)
        {
            return fontIcon;
        }

        targetElement.Loaded += (_, _) =>
        {
            UpdateFontSize(fontIcon, targetElement);
        };

        targetElement.LayoutUpdated += (_, _) =>
        {
            UpdateFontSize(fontIcon, targetElement);
        };

        return fontIcon;
    }

    private void UpdateFontSize(FontIcon fontIcon, FrameworkElement targetElement)
    {
        if (FontSize.HasValue)
        {
            fontIcon.SetCurrentValue(FontIcon.FontSizeProperty, FontSize.Value);
        }
        else if (targetElement is Control control)
        {
            fontIcon.SetCurrentValue(FontIcon.FontSizeProperty, control.FontSize);
        }
        else
        {
            fontIcon.SetCurrentValue(FontIcon.FontSizeProperty, SystemFonts.MessageFontSize);
        }
    }
}