// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Media;
using WPFUI.Controls.Interfaces;

// TODO: Add optional X icon to clear stuff in input

namespace WPFUI.Controls;

/// <summary>
/// Extended <see cref="System.Windows.Controls.TextBox"/> with additional parameters like <see cref="Placeholder"/>.
/// </summary>
public class TextBox : System.Windows.Controls.TextBox, IIconControl
{
    /// <summary>
    /// Property for <see cref="Icon"/>.
    /// </summary>
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon),
        typeof(Common.SymbolRegular), typeof(TextBox),
        new PropertyMetadata(Common.SymbolRegular.Empty));

    /// <summary>
    /// Property for <see cref="IconPosition"/>.
    /// </summary>
    public static readonly DependencyProperty IconPositionProperty = DependencyProperty.Register(
        nameof(IconPosition),
        typeof(Common.ElementPosition), typeof(TextBox),
        new PropertyMetadata(Common.ElementPosition.Left));

    /// <summary>
    /// Property for <see cref="IconFilled"/>.
    /// </summary>
    public static readonly DependencyProperty IconFilledProperty = DependencyProperty.Register(nameof(IconFilled),
        typeof(bool), typeof(TextBox), new PropertyMetadata(false));

    /// <summary>
    /// DependencyProperty for <see cref="IconForeground" /> property.
    /// </summary>
    public static readonly DependencyProperty IconForegroundProperty =
        DependencyProperty.RegisterAttached(
            nameof(IconForeground),
            typeof(Brush),
            typeof(TextBox),
            new FrameworkPropertyMetadata(
                SystemColors.ControlTextBrush,
                FrameworkPropertyMetadataOptions.Inherits));

    /// <summary>
    /// Property for <see cref="Placeholder"/>.
    /// </summary>
    public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(nameof(Placeholder),
        typeof(string), typeof(TextBox), new PropertyMetadata(String.Empty));

    /// <summary>
    /// Property for <see cref="PlaceholderVisible"/>.
    /// </summary>
    public static readonly DependencyProperty PlaceholderVisibleProperty = DependencyProperty.Register(
        nameof(PlaceholderVisible),
        typeof(bool), typeof(TextBox), new PropertyMetadata(true));

    /// <inheritdoc />
    public Common.SymbolRegular Icon
    {
        get => (Common.SymbolRegular)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <summary>
    /// Defines which side the icon should be placed on.
    /// </summary>
    public Common.ElementPosition IconPosition
    {
        get => (Common.ElementPosition)GetValue(IconPositionProperty);
        set => SetValue(IconPositionProperty, value);
    }

    /// <inheritdoc />
    public bool IconFilled
    {
        get => (bool)GetValue(IconFilledProperty);
        set => SetValue(IconFilledProperty, value);
    }

    /// <summary>
    /// The Foreground property specifies the foreground brush of an element's <see cref="Icon"/>.
    /// </summary>
    public Brush IconForeground
    {
        get => (Brush)GetValue(IconForegroundProperty);
        set => SetValue(IconForegroundProperty, value);
    }

    /// <summary>
    /// Gets or sets numbers pattern.
    /// </summary>
    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    /// <summary>
    /// Gets or sets value determining whether to display the placeholder.
    /// </summary>
    public bool PlaceholderVisible
    {
        get => (bool)GetValue(PlaceholderVisibleProperty);
        set => SetValue(PlaceholderVisibleProperty, value);
    }

    /// <summary>
    /// Creates a new instance and assigns default events.
    /// </summary>
    public TextBox()
    {
        TextChanged += TextBox_TextChanged;
    }

    private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        if (sender is not TextBox control)
            return;

        if (PlaceholderVisible && control.Text.Length > 0)
            PlaceholderVisible = false;

        if (!PlaceholderVisible && control.Text.Length < 1)
            PlaceholderVisible = true;
    }
}
