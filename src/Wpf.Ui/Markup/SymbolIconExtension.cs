// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows.Markup;
using Wpf.Ui.Common;
using Wpf.Ui.Controls.IconElements;

namespace Wpf.Ui.Markup;

/// <summary>
/// Custom <see cref="MarkupExtension"/> which can provide <see cref="SymbolIcon"/>.
/// </summary>
[ContentProperty(nameof(Symbol))]
[MarkupExtensionReturnType(typeof(SymbolIcon))]
public class SymbolIconExtension : MarkupExtension
{
    public SymbolIconExtension(SymbolRegular symbol)
    {
        Symbol = symbol;
    }

    public SymbolIconExtension(SymbolRegular symbol, bool filled) : this(symbol)
    {
        Filled = filled;
    }

    [ConstructorArgument("symbol")]
    public SymbolRegular Symbol { get; set; }

    [ConstructorArgument("filled")]
    public bool Filled { get; set; }

    public double FontSize { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var symbolIcon = new SymbolIcon()
        {
            Symbol = Symbol,
            Filled = Filled
        };

        if (FontSize > 0)
        {
            symbolIcon.FontSize = FontSize;
        }

        return symbolIcon;
    }
}
