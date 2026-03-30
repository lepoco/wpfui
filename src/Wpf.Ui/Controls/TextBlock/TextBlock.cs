// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Extended <see cref="System.Windows.Controls.TextBlock"/> with additional parameters like <see cref="FontTypography"/>.
/// </summary>
public class TextBlock : System.Windows.Controls.TextBlock
{
    /// <summary>Identifies the <see cref="FontTypographyStyle" /> dependency property.</summary>
    internal static readonly DependencyProperty FontTypographyStyleProperty = DependencyProperty.Register(
        nameof(FontTypographyStyle),
        typeof(Style),
        typeof(System.Windows.Controls.TextBlock),
        new PropertyMetadata(default(Style))
    );

    /// <summary>Identifies the <see cref="AppearanceForeground" /> dependency property.</summary>
    internal static readonly DependencyProperty AppearanceForegroundProperty = DependencyProperty.Register(
        nameof(AppearanceForeground),
        typeof(Brush),
        typeof(System.Windows.Controls.TextBlock),
        new PropertyMetadata(default(Brush))
    );

    /// <summary>Identifies the <see cref="FontTypography" /> dependency property.</summary>
    public static readonly DependencyProperty FontTypographyProperty = DependencyProperty.Register(
        nameof(FontTypography),
        typeof(FontTypography?),
        typeof(System.Windows.Controls.TextBlock),
        new FrameworkPropertyMetadata(
            null,
            FrameworkPropertyMetadataOptions.AffectsMeasure
                | FrameworkPropertyMetadataOptions.AffectsRender
                | FrameworkPropertyMetadataOptions.Inherits,
            OnFontTypographyChanged
        )
    );

    /// <summary>Identifies the <see cref="Appearance" /> dependency property.</summary>
    public static readonly DependencyProperty AppearanceProperty = DependencyProperty.Register(
        nameof(Appearance),
        typeof(TextColor?),
        typeof(System.Windows.Controls.TextBlock),
        new FrameworkPropertyMetadata(
            null,
            FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits,
            OnAppearanceChanged
        )
    );

    static TextBlock() => TextBlockMetadata.Initialize();

    /// <summary>
    /// Gets or sets the <see cref="AppearanceForeground" /> of the text.
    /// </summary>
    internal Brush? AppearanceForeground
    {
        get { return (Brush?)GetValue(AppearanceForegroundProperty); }
        set { SetValue(AppearanceForegroundProperty, value); }
    }

    /// <summary>
    /// Gets or sets the <see cref="FontTypographyStyle" /> of the text.
    /// </summary>
    internal Style? FontTypographyStyle
    {
        get { return (Style?)GetValue(FontTypographyStyleProperty); }
        set { SetValue(FontTypographyStyleProperty, value); }
    }

    /// <summary>
    /// Gets or sets the <see cref="FontTypography" /> of the text.
    /// </summary>
    public FontTypography? FontTypography
    {
        get => (FontTypography?)GetValue(FontTypographyProperty);
        set => SetValue(FontTypographyProperty, value);
    }

    /// <summary>
    /// Gets or sets the color of the text.
    /// </summary>
    public TextColor? Appearance
    {
        get => (TextColor?)GetValue(AppearanceProperty);
        set => SetValue(AppearanceProperty, value);
    }

    private static void OnFontTypographyChanged(DependencyObject o, DependencyPropertyChangedEventArgs args)
    {
        if (o is System.Windows.Controls.TextBlock tb)
        {
            if (args.NewValue is FontTypography fontTypography)
            {
                tb.SetCurrentValue(
                    FontTypographyStyleProperty,
                    tb.TryFindResource(fontTypography.ToResourceValue())
                );
            }
            else
            {
                tb.ClearValue(FontTypographyStyleProperty);
            }

            tb.CoerceValue(FontSizeProperty);
            tb.CoerceValue(FontWeightProperty);
        }
    }

    private static void OnAppearanceChanged(DependencyObject o, DependencyPropertyChangedEventArgs args)
    {
        if (o is System.Windows.Controls.TextBlock tb)
        {
            if (args.NewValue is TextColor textColor)
            {
                tb.SetCurrentValue(
                    AppearanceForegroundProperty,
                    tb.TryFindResource(textColor.ToResourceValue())
                );
            }
            else
            {
                tb.ClearValue(AppearanceForegroundProperty);
            }

            tb.CoerceValue(ForegroundProperty);
        }
    }
}
