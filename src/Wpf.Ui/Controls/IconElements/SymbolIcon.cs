// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using Wpf.Ui.Common;
using Wpf.Ui.Extensions;

namespace Wpf.Ui.Controls.IconElements;

/// <summary>
/// Represents a text element containing an icon glyph.
/// </summary>
public sealed class SymbolIcon : FontIcon
{
    /// <summary>
    /// Property for <see cref="Symbol"/>.
    /// </summary>
    public static readonly DependencyProperty SymbolProperty = DependencyProperty.Register(nameof(Symbol),
        typeof(SymbolRegular), typeof(SymbolIcon),
        new PropertyMetadata(SymbolRegular.Empty, OnGlyphChanged));

    /// <summary>
    /// Property for <see cref="Filled"/>.
    /// </summary>
    public static readonly DependencyProperty FilledProperty = DependencyProperty.Register(nameof(Filled),
        typeof(bool), typeof(SymbolIcon), new PropertyMetadata(false, OnGlyphChanged));

    public SymbolRegular Symbol
    {
        get => (SymbolRegular)GetValue(SymbolProperty);
        set => SetValue(SymbolProperty, value);
    }

    /// <summary>
    /// Defines whether or not we should use the <see cref="Common.SymbolFilled"/>.
    /// </summary>
    public bool Filled
    {
        get => (bool)GetValue(FilledProperty);
        set => SetValue(FilledProperty, value);
    }

    protected override void InitializeChildren()
    {
        SetResourceReference(FontFamilyProperty, "FluentSystemIcons");

        base.InitializeChildren();
    }

    private static void OnGlyphChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not SymbolIcon control)
            return;

        if (control.Filled)
            control.Glyph = control.Symbol.Swap().GetString();
        else
            control.Glyph = control.Symbol.GetString();
    }
}
