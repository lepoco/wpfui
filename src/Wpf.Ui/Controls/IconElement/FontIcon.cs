// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;
using System.Windows.Documents;
using FontFamily = System.Windows.Media.FontFamily;
using FontStyle = System.Windows.FontStyle;
using SystemFonts = System.Windows.SystemFonts;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Represents an icon that uses a glyph from the specified font.
/// </summary>
public class FontIcon : IconElement
{
    /// <summary>Identifies the <see cref="FontFamily"/> dependency property.</summary>
    public static readonly DependencyProperty FontFamilyProperty = DependencyProperty.Register(
        nameof(FontFamily),
        typeof(FontFamily),
        typeof(FontIcon),
        new FrameworkPropertyMetadata(SystemFonts.MessageFontFamily, OnFontFamilyChanged)
    );

    /// <summary>Identifies the <see cref="FontSize"/> dependency property.</summary>
    public static readonly DependencyProperty FontSizeProperty = TextElement.FontSizeProperty.AddOwner(
        typeof(FontIcon),
        new FrameworkPropertyMetadata(
            SystemFonts.MessageFontSize,
            FrameworkPropertyMetadataOptions.Inherits,
            OnFontSizeChanged
        )
    );

    /// <summary>Identifies the <see cref="FontStyle"/> dependency property.</summary>
    public static readonly DependencyProperty FontStyleProperty = DependencyProperty.Register(
        nameof(FontStyle),
        typeof(FontStyle),
        typeof(FontIcon),
        new FrameworkPropertyMetadata(FontStyles.Normal, OnFontStyleChanged)
    );

    /// <summary>Identifies the <see cref="FontWeight"/> dependency property.</summary>
    public static readonly DependencyProperty FontWeightProperty = DependencyProperty.Register(
        nameof(FontWeight),
        typeof(FontWeight),
        typeof(FontIcon),
        new FrameworkPropertyMetadata(FontWeights.Normal, OnFontWeightChanged)
    );

    /// <summary>Identifies the <see cref="Glyph"/> dependency property.</summary>
    public static readonly DependencyProperty GlyphProperty = DependencyProperty.Register(
        nameof(Glyph),
        typeof(string),
        typeof(FontIcon),
        new FrameworkPropertyMetadata(string.Empty, OnGlyphChanged)
    );

    /// <inheritdoc cref="Control.FontFamily"/>
    [Bindable(true)]
    [Category("Appearance")]
    [Localizability(LocalizationCategory.Font)]
    public FontFamily FontFamily
    {
        get => (FontFamily)GetValue(FontFamilyProperty);
        set => SetValue(FontFamilyProperty, value);
    }

    /// <inheritdoc cref="Control.FontSize"/>
    [TypeConverter(typeof(FontSizeConverter))]
    [Bindable(true)]
    [Category("Appearance")]
    [Localizability(LocalizationCategory.None)]
    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    /// <inheritdoc cref="Control.FontStyle"/>
    [Bindable(true)]
    [Category("Appearance")]
    public FontStyle FontStyle
    {
        get => (FontStyle)GetValue(FontStyleProperty);
        set => SetValue(FontStyleProperty, value);
    }

    /// <inheritdoc cref="Control.FontWeight"/>
    [Bindable(true)]
    [Category("Appearance")]
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

    protected TextBlock? TextBlock { get; set; }

    protected override UIElement InitializeChildren()
    {
        if (FontSize.Equals(SystemFonts.MessageFontSize))
        {
            SetResourceReference(FontSizeProperty, "DefaultIconFontSize");

            // If the FontSize is the default, set it to the parent's FontSize.
            if (VisualParent is not null)
            {
                SetCurrentValue(FontSizeProperty, TextElement.GetFontSize(VisualParent));
            }
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

        SetCurrentValue(FocusableProperty, false);

        return TextBlock;
    }

    private static void OnFontFamilyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var self = (FontIcon)d;
        if (self.TextBlock is null)
        {
            return;
        }

        self.TextBlock.SetCurrentValue(
            System.Windows.Controls.TextBlock.FontFamilyProperty,
            (FontFamily)e.NewValue
        );
    }

    private static void OnFontSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var self = (FontIcon)d;
        if (self.TextBlock is null)
        {
            return;
        }

        self.TextBlock.SetCurrentValue(
            System.Windows.Controls.TextBlock.FontSizeProperty,
            (double)e.NewValue
        );
    }

    private static void OnFontStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var self = (FontIcon)d;
        if (self.TextBlock is null)
        {
            return;
        }

        self.TextBlock.SetCurrentValue(
            System.Windows.Controls.TextBlock.FontStyleProperty,
            (FontStyle)e.NewValue
        );
    }

    private static void OnFontWeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var self = (FontIcon)d;
        if (self.TextBlock is null)
        {
            return;
        }

        self.TextBlock.SetCurrentValue(
            System.Windows.Controls.TextBlock.FontWeightProperty,
            (FontWeight)e.NewValue
        );
    }

    private static void OnGlyphChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var self = (FontIcon)d;
        if (self.TextBlock is null)
        {
            return;
        }

        self.TextBlock.SetCurrentValue(System.Windows.Controls.TextBlock.TextProperty, (string)e.NewValue);
    }
}
