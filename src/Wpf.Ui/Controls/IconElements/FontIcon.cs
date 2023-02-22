// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Based on FontIcon created by Yimeng Wu licensed under MIT license.
// https://github.com/Kinnara/ModernWpf/blob/master/ModernWpf/IconElement/FontIcon.cs
// Copyright (C) Ivan Dmitryiyev, Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace Wpf.Ui.Controls.IconElements;

/// <summary>
/// Represents an icon that uses a glyph from the specified font.
/// </summary>
public class FontIcon : IconElement
{
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
        DependencyProperty.Register(
            nameof(FontSize),
            typeof(double),
            typeof(FontIcon),
            new FrameworkPropertyMetadata(SystemFonts.MessageFontSize, OnFontSizeChanged));

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

    /// <summary>
    /// Gets or sets the font used to display the icon glyph.
    /// </summary>
    /// <returns>The font used to display the icon glyph.</returns>
    [Bindable(true), Category("Appearance")]
    [Localizability(LocalizationCategory.Font)]
    public FontFamily FontFamily
    {
        get => (FontFamily)GetValue(FontFamilyProperty);
        set => SetValue(FontFamilyProperty, value);
    }

    /// <summary>
    /// Gets or sets the size of the icon glyph.
    /// </summary>
    /// <returns>A non-negative value that specifies the font size, measured in pixels.</returns>
    [TypeConverter(typeof(FontSizeConverter))]
    [Bindable(true), Category("Appearance")]
    [Localizability(LocalizationCategory.None)]
    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the font style for the icon glyph.
    /// </summary>
    /// <returns>
    /// A named constant of the enumeration that specifies the style in which the icon
    /// glyph is rendered. The default is **Normal**.
    /// </returns>
    [Bindable(true), Category("Appearance")]
    public FontStyle FontStyle
    {
        get => (FontStyle)GetValue(FontStyleProperty);
        set => SetValue(FontStyleProperty, value);
    }

    /// <summary>
    /// Gets or sets the thickness of the icon glyph.
    /// </summary>
    /// <returns>
    /// A value that specifies the thickness of the icon glyph. The default is **Normal**.
    /// </returns>
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

    protected TextBlock? TextBlock;

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

    protected override UIElement InitializeChildren()
    {
        SetResourceReference(FontSizeProperty, "DefaultIconFontSize");

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
            Text = Glyph
        };

        return TextBlock;
    }

    protected override void OnShouldInheritForegroundFromVisualParentChanged()
    {
        if (TextBlock is null)
            return;

        if (ShouldInheritForegroundFromVisualParent)
        {
            TextBlock.SetBinding(TextBlock.ForegroundProperty,
                new Binding
                {
                    Path = new PropertyPath(TextElement.ForegroundProperty),
                    Source = VisualParent,
                });
        }
        else
        {
            TextBlock.ClearValue(TextBlock.ForegroundProperty);
        }
    }
}
