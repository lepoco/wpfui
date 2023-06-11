// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Documents;
using Wpf.Ui.Common;
using Wpf.Ui.Extensions;

namespace Wpf.Ui.Controls;

/// <summary>
/// Extended <see cref="System.Windows.Controls.TextBlock"/> with additional parameters like <see cref="FontTypography"/>.
/// </summary>
public class TextBlock : System.Windows.Controls.TextBlock
{
    static TextBlock()
    {
        TextElement.FontSizeProperty.OverrideMetadata(typeof(System.Windows.Controls.TextBlock), new FrameworkPropertyMetadata(14.0));
    }

    /// <summary>
    /// Property for <see cref="FontTypography"/>.
    /// </summary>
    public static readonly DependencyProperty FontTypographyProperty = DependencyProperty.Register(
        nameof(FontTypography),
        typeof(FontTypography), typeof(TextBlock), new PropertyMetadata(FontTypography.Body,
            static (o, args) => ((TextBlock)o).OnFontTypographyChanged((FontTypography)args.NewValue)));

    /// <summary>
    /// Property for <see cref="Appearance"/>.
    /// </summary>
    public static readonly DependencyProperty AppearanceProperty = DependencyProperty.Register(
        nameof(Appearance),
        typeof(TextColor), typeof(TextBlock), new PropertyMetadata(TextColor.Primary,
            static (o, args) => ((TextBlock)o).OnAppearanceChanged((TextColor)args.NewValue)));

    /// <summary>
    /// TODO
    /// </summary>
    public FontTypography FontTypography
    {
        get => (FontTypography)GetValue(FontTypographyProperty);
        set => SetValue(FontTypographyProperty, value);
    }

    /// <summary>
    /// TODO
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
