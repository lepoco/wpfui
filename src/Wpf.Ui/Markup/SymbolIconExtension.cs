// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;
using System.Windows.Markup;
using Wpf.Ui.Controls;

namespace Wpf.Ui.Markup;

/// <summary>
/// Custom <see cref="MarkupExtension"/> which can provide <see cref="SymbolIcon"/>.
/// </summary>
/// <example>
/// <code lang="xml">
/// &lt;ui:Button
///     Appearance="Primary"
///     Content="WPF UI button with font icon"
///     Icon="{ui:SymbolIcon Symbol=Fluent24}" /&gt;
/// </code>
/// <code lang="xml">
/// &lt;ui:Button Icon="{ui:SymbolIcon Fluent24}" /&gt;
/// </code>
/// <code lang="xml">
/// &lt;ui:HyperlinkButton Icon="{ui:SymbolIcon Fluent24}" /&gt;
/// </code>
/// <code lang="xml">
/// &lt;ui:TitleBar Icon="{ui:SymbolIcon Fluent24}" /&gt;
/// </code>
/// </example>
[ContentProperty(nameof(Symbol))]
[MarkupExtensionReturnType(typeof(SymbolIcon))]
public class SymbolIconExtension : MarkupExtension
{
    public SymbolIconExtension() { }

    public SymbolIconExtension(SymbolRegular symbol)
    {
        Symbol = symbol;
    }

    public SymbolIconExtension(string symbol)
    {
        Symbol = (SymbolRegular)Enum.Parse(typeof(SymbolRegular), symbol);
    }

    public SymbolIconExtension(SymbolRegular symbol, bool filled)
        : this(symbol)
    {
        Filled = filled;
    }

    public SymbolIconExtension(SymbolRegular symbol, bool filled, double fontSize)
        : this(symbol, filled)
    {
        FontSize = fontSize;
    }

    [ConstructorArgument("symbol")]
    public SymbolRegular Symbol { get; set; }

    [ConstructorArgument("filled")]
    public bool Filled { get; set; }

    [ConstructorArgument("fontSize")]
    public double FontSize { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (serviceProvider.GetService(typeof(IProvideValueTarget)) is not IProvideValueTarget provideValueTarget)
        {
            return new SymbolIcon { Symbol = Symbol, Filled = Filled, FontSize = FontSize > 0 ? FontSize : SystemFonts.MessageFontSize };
        }

        if (provideValueTarget.TargetObject is Setter)
        {
            return this;
        }

        SymbolIcon symbolIcon = new ()
        {
            Symbol = Symbol,
            Filled = Filled
        };

        if (provideValueTarget.TargetObject is not FrameworkElement targetElement)
        {
            return symbolIcon;
        }

        targetElement.Loaded += (_, _) =>
        {
            UpdateFontSize(symbolIcon, targetElement);
        };

        targetElement.LayoutUpdated += (_, _) =>
        {
            UpdateFontSize(symbolIcon, targetElement);
        };

        return symbolIcon;
    }

    private void UpdateFontSize(SymbolIcon symbolIcon, FrameworkElement targetElement)
    {
        if (FontSize > 0)
        {
            symbolIcon.SetCurrentValue(FontIcon.FontSizeProperty, FontSize);
        }
        else if (targetElement is Control control)
        {
            symbolIcon.SetCurrentValue(FontIcon.FontSizeProperty, control.FontSize);
        }
    }
}