// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using Wpf.Ui.Common;
using Wpf.Ui.Extensions;

namespace Wpf.Ui.Controls.IconElements;

/// <summary>
/// Represents a text element containing an icon glyph.
/// </summary>
[ToolboxItem(true)]
[ToolboxBitmap(typeof(SymbolIcon), "SymbolIcon.bmp")]
public class SymbolIcon : FontIcon
{
    /// <summary>
    /// Property for <see cref="Symbol"/>.
    /// </summary>
    public static readonly DependencyProperty SymbolProperty = DependencyProperty.Register(nameof(Symbol),
        typeof(SymbolRegular), typeof(SymbolIcon),
        new PropertyMetadata(SymbolRegular.Empty, static (o, _) => ((SymbolIcon)o).OnGlyphChanged()));

    /// <summary>
    /// Property for <see cref="Filled"/>.
    /// </summary>
    public static readonly DependencyProperty FilledProperty = DependencyProperty.Register(nameof(Filled),
        typeof(bool), typeof(SymbolIcon), new PropertyMetadata(false, OnFilledChanged));

    /// <summary>
    /// Gets or sets displayed <see cref="SymbolRegular"/>.
    /// </summary>
    public SymbolRegular Symbol
    {
        get => (SymbolRegular)GetValue(SymbolProperty);
        set => SetValue(SymbolProperty, value);
    }

    /// <summary>
    /// Defines whether or not we should use the <see cref="SymbolFilled"/>.
    /// </summary>
    public bool Filled
    {
        get => (bool)GetValue(FilledProperty);
        set => SetValue(FilledProperty, value);
    }

    public SymbolIcon() {}

    public SymbolIcon(SymbolRegular symbol, bool filled  = false)
    {
        Symbol = symbol;
        Filled = filled;
    }

    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);

        SetFontReference();
    }

    private void OnGlyphChanged()
    {
        if (Filled)
            Glyph = Symbol.Swap().GetString();
        else
            Glyph = Symbol.GetString();
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
