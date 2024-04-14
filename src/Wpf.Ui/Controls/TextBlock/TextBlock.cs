// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Extensions;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Extended <see cref="System.Windows.Controls.TextBlock"/> with additional parameters like <see cref="FontTypography"/>.
/// </summary>
public class TextBlock : System.Windows.Controls.TextBlock
{
    /// <summary>Identifies the <see cref="FontTypography"/> dependency property.</summary>
    public static readonly DependencyProperty FontTypographyProperty = DependencyProperty.Register(
        nameof(FontTypography),
        typeof(FontTypography),
        typeof(TextBlock),
        new PropertyMetadata(
            FontTypography.Body,
            static (o, args) =>
            {
                ((TextBlock)o).OnFontTypographyChanged((FontTypography)args.NewValue);
            }
        )
    );

    /// <summary>Identifies the <see cref="Appearance"/> dependency property.</summary>
    public static readonly DependencyProperty AppearanceProperty = DependencyProperty.Register(
        nameof(Appearance),
        typeof(TextColor),
        typeof(TextBlock),
        new PropertyMetadata(
            TextColor.Primary,
            static (o, args) =>
            {
                ((TextBlock)o).OnAppearanceChanged((TextColor)args.NewValue);
            }
        )
    );

    /// <summary>
    /// Gets or sets the <see cref="FontTypography"/> of the text.
    /// </summary>
    public FontTypography FontTypography
    {
        get => (FontTypography)GetValue(FontTypographyProperty);
        set => SetValue(FontTypographyProperty, value);
    }

    /// <summary>
    /// Gets or sets the color of the text.
    /// </summary>
    public TextColor Appearance
    {
        get => (TextColor)GetValue(AppearanceProperty);
        set => SetValue(AppearanceProperty, value);
    }

    private void OnFontTypographyChanged(FontTypography newTypography)
    {
        SetResourceReference(StyleProperty, newTypography.ToResourceValue());
    }

    private void OnAppearanceChanged(TextColor textColor)
    {
        SetResourceReference(ForegroundProperty, textColor.ToResourceValue());
    }
}
