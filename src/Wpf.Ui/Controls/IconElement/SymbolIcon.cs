// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Extensions;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Represents a text element containing an icon glyph.
/// </summary>
public class SymbolIcon : FontIcon
{
    /// <summary>Identifies the <see cref="Symbol"/> dependency property.</summary>
    public static readonly DependencyProperty SymbolProperty = DependencyProperty.Register(
        nameof(Symbol),
        typeof(SymbolRegular),
        typeof(SymbolIcon),
        new PropertyMetadata(SymbolRegular.Empty, static (o, _) => ((SymbolIcon)o).OnGlyphChanged())
    );

    /// <summary>Identifies the <see cref="Filled"/> dependency property.</summary>
    public static readonly DependencyProperty FilledProperty = DependencyProperty.Register(
        nameof(Filled),
        typeof(bool),
        typeof(SymbolIcon),
        new PropertyMetadata(false, OnFilledChanged)
    );

    /// <summary>
    /// Gets or sets displayed <see cref="SymbolRegular"/>.
    /// </summary>
    public SymbolRegular Symbol
    {
        get => (SymbolRegular)GetValue(SymbolProperty);
        set => SetValue(SymbolProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether or not we should use the <see cref="SymbolFilled"/>.
    /// </summary>
    public bool Filled
    {
        get => (bool)GetValue(FilledProperty);
        set => SetValue(FilledProperty, value);
    }

    public SymbolIcon() { }

    public SymbolIcon(SymbolRegular symbol, double fontSize = 14, bool filled = false)
    {
        Symbol = symbol;
        Filled = filled;
        FontSize = fontSize;
    }

    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);

        SetFontReference();
    }

    private void OnGlyphChanged()
    {
        if (Filled)
        {
            SetCurrentValue(GlyphProperty, Symbol.Swap().GetString());
        }
        else
        {
            SetCurrentValue(GlyphProperty, Symbol.GetString());
        }
    }

    private void SetFontReference()
    {
        SetResourceReference(FontFamilyProperty, Filled ? "FluentSystemIconsFilled" : "FluentSystemIcons");
    }

    private static void OnFilledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var self = (SymbolIcon)d;
        self.SetFontReference();
        self.OnGlyphChanged();
    }
}
