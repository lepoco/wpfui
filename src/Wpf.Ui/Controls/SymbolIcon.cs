// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.ComponentModel;
using System.Drawing;
using System.Windows;
using Wpf.Ui.Extensions;

namespace Wpf.Ui.Controls;

/// <summary>
/// Represents a text element containing an icon glyph.
/// </summary>
///https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.symbolicon?view=winrt-22000
[ToolboxItem(true)]
[ToolboxBitmap(typeof(SymbolIcon), "SymbolIcon.bmp")]
public class SymbolIcon : System.Windows.Controls.Control
{
    /// <summary>
    /// Property for <see cref="Symbol"/>.
    /// </summary>
    public static readonly DependencyProperty SymbolProperty = DependencyProperty.Register(nameof(Symbol),
        typeof(Common.SymbolRegular), typeof(SymbolIcon),
        new PropertyMetadata(Common.SymbolRegular.Empty, OnGlyphChanged));

    /// <summary>
    /// Property for <see cref="RawSymbol"/>.
    /// </summary>
    public static readonly DependencyProperty RawSymbolProperty = DependencyProperty.Register(nameof(RawSymbol),
        typeof(string), typeof(SymbolIcon), new PropertyMetadata("\uEA01"));

    /// <summary>
    /// Property for <see cref="Filled"/>.
    /// </summary>
    public static readonly DependencyProperty FilledProperty = DependencyProperty.Register(nameof(Filled),
        typeof(bool), typeof(SymbolIcon), new PropertyMetadata(false, OnGlyphChanged));

    /// <summary>
    /// Gets or sets displayed <see cref="Common.SymbolRegular"/>.
    /// </summary>
    public Common.SymbolRegular Symbol
    {
        get => (Common.SymbolRegular)GetValue(SymbolProperty);
        set => SetValue(SymbolProperty, value);
    }

    /// <summary>
    /// Gets or sets displayed <see cref="Common.SymbolRegular"/> as <see langword="string"/>.
    /// </summary>
    public string RawSymbol
    {
        get => (string)GetValue(RawSymbolProperty);
    }

    /// <summary>
    /// Defines whether or not we should use the <see cref="Common.SymbolFilled"/>.
    /// </summary>
    public bool Filled
    {
        get => (bool)GetValue(FilledProperty);
        set => SetValue(FilledProperty, value);
    }

    private static void OnGlyphChanged(DependencyObject dependency, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependency is not SymbolIcon control)
            return;

        if ((bool)control.GetValue(FilledProperty))
            control.SetValue(RawSymbolProperty, control.Symbol.Swap().GetString());
        else
            control.SetValue(RawSymbolProperty, control.Symbol.GetString());
    }
}
