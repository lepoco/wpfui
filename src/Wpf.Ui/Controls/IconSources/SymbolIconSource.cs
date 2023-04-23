// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common;
using Wpf.Ui.Controls.IconElements;

namespace Wpf.Ui.Controls.IconSources;

/// <summary>
/// Represents an icon source that uses a glyph from the specified font.
/// </summary>
public class SymbolIconSource : IconSource
{
    /// <summary>
    /// Property for <see cref="FontSize"/>.
    /// </summary>
    public static readonly DependencyProperty FontSizeProperty =
        DependencyProperty.Register(
            nameof(FontSize),
            typeof(double),
            typeof(SymbolIconSource),
            new PropertyMetadata(SystemFonts.MessageFontSize));

    /// <summary>
    /// Property for <see cref="FontStyle"/>.
    /// </summary>
    public static readonly DependencyProperty FontStyleProperty =
        DependencyProperty.Register(
            nameof(FontStyle),
            typeof(FontStyle),
            typeof(SymbolIconSource),
            new PropertyMetadata(FontStyles.Normal));

    /// <summary>
    /// Property for <see cref="FontWeight"/>.
    /// </summary>
    public static readonly DependencyProperty FontWeightProperty =
        DependencyProperty.Register(
            nameof(FontWeight),
            typeof(FontWeight),
            typeof(SymbolIconSource),
            new PropertyMetadata(FontWeights.Normal));

    /// <summary>
    /// Property for <see cref="Symbol"/>.
    /// </summary>
    public static readonly DependencyProperty SymbolProperty =
        DependencyProperty.Register(
            nameof(Symbol),
            typeof(SymbolRegular),
            typeof(SymbolIconSource),
            new PropertyMetadata(SymbolRegular.Empty));

    /// <summary>
    /// Property for <see cref="Filled"/>.
    /// </summary>
    public static readonly DependencyProperty FilledProperty = DependencyProperty.Register(nameof(Filled),
        typeof(bool), typeof(SymbolIconSource), new PropertyMetadata(false));

    /// <inheritdoc cref="Control.FontSize"/>
    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    /// <inheritdoc cref="Control.FontWeight"/>
    public FontWeight FontWeight
    {
        get => (FontWeight)GetValue(FontWeightProperty);
        set => SetValue(FontWeightProperty, value);
    }

    /// <inheritdoc cref="Control.FontStyle"/>
    public FontStyle FontStyle
    {
        get => (FontStyle)GetValue(FontStyleProperty);
        set => SetValue(FontStyleProperty, value);
    }

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

    public override IconElement CreateIconElement()
    {
        SymbolIcon symbolIcon = new(Symbol, Filled);

        if (!FontSize.Equals(SystemFonts.MessageFontSize))
        {
            symbolIcon.FontSize = FontSize;
        }

        if (FontWeight != FontWeights.Normal)
        {
            symbolIcon.FontWeight = FontWeight;
        }

        if (FontStyle != FontStyles.Normal)
        {
            symbolIcon.FontStyle = FontStyle;
        }

        if (Foreground != SystemColors.ControlTextBrush)
        {
            symbolIcon.Foreground = Foreground;
        }

        return symbolIcon;
    }
}
