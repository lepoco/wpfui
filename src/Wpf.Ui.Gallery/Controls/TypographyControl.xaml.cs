// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace Wpf.Ui.Gallery.Controls;

public class TypographyControl : Control
{
    /// <summary>Identifies the <see cref="Example"/> dependency property.</summary>
    public static readonly DependencyProperty ExampleProperty = DependencyProperty.Register(
        nameof(Example),
        typeof(string),
        typeof(TypographyControl),
        new PropertyMetadata(string.Empty)
    );

    /// <summary>Identifies the <see cref="ExampleFontTypography"/> dependency property.</summary>
    public static readonly DependencyProperty ExampleFontTypographyProperty = DependencyProperty.Register(
        nameof(ExampleFontTypography),
        typeof(FontTypography),
        typeof(TypographyControl),
        new PropertyMetadata(
            FontTypography.Body,
            static (o, args) =>
            {
                ((TypographyControl)o).OnExampleFontTypographyChanged((FontTypography)args.NewValue);
            }
        )
    );

    /// <summary>Identifies the <see cref="VariableFont"/> dependency property.</summary>
    public static readonly DependencyProperty VariableFontProperty = DependencyProperty.Register(
        nameof(VariableFont),
        typeof(string),
        typeof(TypographyControl),
        new PropertyMetadata(string.Empty)
    );

    /// <summary>Identifies the <see cref="SizeLinHeight"/> dependency property.</summary>
    public static readonly DependencyProperty SizeLinHeightProperty = DependencyProperty.Register(
        nameof(SizeLinHeight),
        typeof(string),
        typeof(TypographyControl),
        new PropertyMetadata(string.Empty)
    );

    /// <summary>Identifies the <see cref="FontTypographyStyle"/> dependency property.</summary>
    public static readonly DependencyProperty FontTypographyStyleProperty = DependencyProperty.Register(
        nameof(FontTypographyStyle),
        typeof(string),
        typeof(TypographyControl),
        new PropertyMetadata(FontTypography.Body.ToString())
    );

    public string Example
    {
        get => (string)GetValue(ExampleProperty);
        set => SetValue(ExampleProperty, value);
    }

    public FontTypography ExampleFontTypography
    {
        get => (FontTypography)GetValue(ExampleFontTypographyProperty);
        set => SetValue(ExampleFontTypographyProperty, value);
    }

    public string VariableFont
    {
        get => (string)GetValue(VariableFontProperty);
        set => SetValue(VariableFontProperty, value);
    }

    public string SizeLinHeight
    {
        get => (string)GetValue(SizeLinHeightProperty);
        set => SetValue(SizeLinHeightProperty, value);
    }

    public string FontTypographyStyle
    {
        get => (string)GetValue(FontTypographyStyleProperty);
        set => SetValue(FontTypographyStyleProperty, value);
    }

    private void OnExampleFontTypographyChanged(FontTypography fontTypography)
    {
        SetCurrentValue(FontTypographyStyleProperty, fontTypography.ToString());
    }
}
