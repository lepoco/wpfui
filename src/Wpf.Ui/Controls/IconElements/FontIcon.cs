// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using FontStyle = System.Windows.FontStyle;
using SystemFonts = System.Windows.SystemFonts;
using FontFamily = System.Windows.Media.FontFamily;

namespace Wpf.Ui.Controls.IconElements;

/// <summary>
/// Represents an icon that uses a glyph from the specified font.
/// </summary>
[ToolboxItem(true)]
[ToolboxBitmap(typeof(FontIcon), "FontIcon.bmp")]
public class FontIcon : IconElement
{
    #region Static properties

    /// <summary>
    /// Property for <see cref="FontFamily"/>.
    /// </summary>
    public static readonly DependencyProperty FontFamilyProperty =
        DependencyProperty.Register(
            nameof(FontFamily),
            typeof(FontFamily),
            typeof(FontIcon),
            new FrameworkPropertyMetadata(
                SystemFonts.MessageFontFamily,
                OnFontFamilyChanged));

    /// <summary>
    /// Property for <see cref="FontSize"/>.
    /// </summary>
    public static readonly DependencyProperty FontSizeProperty =
        TextElement.FontSizeProperty.AddOwner(
            typeof(FontIcon),
            new FrameworkPropertyMetadata(SystemFonts.MessageFontSize, FrameworkPropertyMetadataOptions.Inherits,
                OnFontSizeChanged));

    /// <summary>
    /// Property for <see cref="FontStyle"/>.
    /// </summary>
    public static readonly DependencyProperty FontStyleProperty =
        DependencyProperty.Register(
            nameof(FontStyle),
            typeof(FontStyle),
            typeof(FontIcon),
            new FrameworkPropertyMetadata(FontStyles.Normal, OnFontStyleChanged));

    /// <summary>
    /// Property for <see cref="FontWeight"/>.
    /// </summary>
    public static readonly DependencyProperty FontWeightProperty =
        DependencyProperty.Register(
            nameof(FontWeight),
            typeof(FontWeight),
            typeof(FontIcon),
            new FrameworkPropertyMetadata(FontWeights.Normal, OnFontWeightChanged));

    /// <summary>
    /// Property for <see cref="Glyph"/>.
    /// </summary>
    public static readonly DependencyProperty GlyphProperty =
        DependencyProperty.Register(
            nameof(Glyph),
            typeof(string),
            typeof(FontIcon),
            new FrameworkPropertyMetadata(string.Empty, OnGlyphChanged));

    #endregion

    #region Properties

    /// <inheritdoc cref="Control.FontFamily"/>
    [Bindable(true), Category("Appearance")]
    [Localizability(LocalizationCategory.Font)]
    public FontFamily FontFamily
    {
        get => (FontFamily)GetValue(FontFamilyProperty);
        set => SetValue(FontFamilyProperty, value);
    }

    /// <inheritdoc cref="Control.FontSize"/>
    [TypeConverter(typeof(FontSizeConverter))]
    [Bindable(true), Category("Appearance")]
    [Localizability(LocalizationCategory.None)]
    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    /// <inheritdoc cref="Control.FontStyle"/>
    [Bindable(true), Category("Appearance")]
    public FontStyle FontStyle
    {
        get => (FontStyle)GetValue(FontStyleProperty);
        set => SetValue(FontStyleProperty, value);
    }

    /// <inheritdoc cref="Control.FontWeight"/>
    [Bindable(true), Category("Appearance")]
    public FontWeight FontWeight
    {
        get => (FontWeight)GetValue(FontWeightProperty);
        set => SetValue(FontWeightProperty, value);
    }

    /// <summary>
    /// Gets or sets the character code that identifies the icon glyph.
    /// </summary>
    /// <returns>The hexadecimal character code for the icon glyph.</returns>
    public string Glyph
    {
        get => (string)GetValue(GlyphProperty);
        set => SetValue(GlyphProperty, value);
    }

    #endregion

    protected TextBlock? TextBlock;

    protected override UIElement InitializeChildren()
    {
        if (FontSize.Equals(SystemFonts.MessageFontSize))
        {
            SetResourceReference(FontSizeProperty, "DefaultIconFontSize");   
        }

        TextBlock = new TextBlock
        {
            Style = null,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Center,
            TextAlignment = TextAlignment.Center,
            FontFamily = FontFamily,
            FontSize = FontSize,
            FontStyle = FontStyle,
            FontWeight = FontWeight,
            Text = Glyph,
            Visibility = Visibility.Visible,
            Focusable = false,
        };

        Focusable = false;

        return TextBlock;
    }

    #region Static methods

    private static void OnFontFamilyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var self = (FontIcon)d;
        if (self.TextBlock is null)
            return;

        self.TextBlock.FontFamily = (FontFamily)e.NewValue;
    }

    private static void OnFontSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var self = (FontIcon)d;
        if (self.TextBlock is null)
            return;

        self.TextBlock.FontSize = (double)e.NewValue;
    }

    private static void OnFontStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var self = (FontIcon)d;
        if (self.TextBlock is null)
            return;

        self.TextBlock.FontStyle = (FontStyle)e.NewValue;
    }

    private static void OnFontWeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var self = (FontIcon)d;
        if (self.TextBlock is null)
            return;

        self.TextBlock.FontWeight = (FontWeight)e.NewValue;
    }

    private static void OnGlyphChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var self = (FontIcon)d;
        if (self.TextBlock is null)
            return;

        self.TextBlock.Text = (string)e.NewValue;
    }

    #endregion
}
