// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Media;

namespace Wpf.Ui.Controls;

/// <summary>
/// Ala Pa**one color card.
/// </summary>
public class CardColor : System.Windows.Controls.Control
{
    /// <summary>
    /// Property for <see cref="Title"/>.
    /// </summary>
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title),
        typeof(string), typeof(CardColor), new PropertyMetadata(String.Empty));

    /// <summary>
    /// Property for <see cref="Subtitle"/>.
    /// </summary>
    public static readonly DependencyProperty SubtitleProperty = DependencyProperty.Register(nameof(Subtitle),
        typeof(string), typeof(CardColor), new PropertyMetadata(String.Empty, OnSubtitlePropertyChanged));

    /// <summary>
    /// Property for <see cref="SubtitleFontSize"/>.
    /// </summary>
    public static readonly DependencyProperty SubtitleFontSizeProperty = DependencyProperty.Register(nameof(SubtitleFontSize),
        typeof(double), typeof(CardColor), new PropertyMetadata(11.0d));

    /// <summary>
    /// Property for <see cref="Color"/>.
    /// </summary>
    public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color),
        typeof(Color), typeof(CardColor), new PropertyMetadata(Color.FromArgb(0, 0, 0, 0), OnColorPropertyChanged));

    /// <summary>
    /// Property for <see cref="Brush"/>.
    /// </summary>
    public static readonly DependencyProperty BrushProperty = DependencyProperty.Register(nameof(Brush),
        typeof(Brush), typeof(CardColor),
        new PropertyMetadata(new SolidColorBrush { Color = Color.FromArgb(0, 0, 0, 0) }, OnBrushPropertyChanged));

    /// <summary>
    /// Property for <see cref="CardBrush"/>.
    /// </summary>
    public static readonly DependencyProperty CardBrushProperty = DependencyProperty.Register(nameof(CardBrush),
        typeof(Brush), typeof(CardColor),
        new PropertyMetadata(new SolidColorBrush { Color = Color.FromArgb(0, 0, 0, 0) }));

    /// <summary>
    /// Gets or sets the main text displayed below the color.
    /// </summary>
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    /// <summary>
    /// Gets or sets text displayed under main <see cref="Title"/>.
    /// </summary>
    public string Subtitle
    {
        get => (string)GetValue(SubtitleProperty);
        set => SetValue(SubtitleProperty, value);
    }

    /// <summary>
    /// Gets or sets the font size of <see cref="Subtitle"/>.
    /// </summary>
    public double SubtitleFontSize
    {
        get => (double)GetValue(SubtitleFontSizeProperty);
        set => SetValue(SubtitleFontSizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the displayed <see cref="CardBrush"/>.
    /// </summary>
    public Color Color
    {
        get => (Color)GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }

    /// <summary>
    /// Gets or sets the displayed <see cref="CardBrush"/>.
    /// </summary>
    public Brush Brush
    {
        get => (Brush)GetValue(BrushProperty);
        set => SetValue(BrushProperty, value);
    }

    /// <summary>
    /// Gets the <see cref="System.Windows.Media.Brush"/> displayed in <see cref="CardColor"/>.
    /// </summary>
    public Brush CardBrush
    {
        get => (Brush)GetValue(CardBrushProperty);
        internal set => SetValue(CardBrushProperty, value);
    }

    /// <summary>
    /// Virtual method triggered when <see cref="Subtitle"/> is changed.
    /// </summary>
    protected virtual void OnSubtitlePropertyChanged()
    {
    }

    /// <summary>
    /// Virtual method triggered when <see cref="Color"/> is changed.
    /// </summary>
    protected virtual void OnColorPropertyChanged()
    {
        CardBrush = new SolidColorBrush(Color);
    }

    /// <summary>
    /// Virtual method triggered when <see cref="Brush"/> is changed.
    /// </summary>
    protected virtual void OnBrushPropertyChanged()
    {
        CardBrush = Brush;
    }

    private static void OnSubtitlePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not CardColor cardColor)
            return;

        cardColor.OnSubtitlePropertyChanged();
    }

    private static void OnColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not CardColor cardColor)
            return;

        cardColor.OnColorPropertyChanged();
    }

    private static void OnBrushPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not CardColor cardColor)
            return;

        cardColor.OnBrushPropertyChanged();
    }
}
