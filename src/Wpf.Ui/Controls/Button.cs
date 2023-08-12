// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Wpf.Ui.Common;
using Wpf.Ui.Controls.Interfaces;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Inherited from the <see cref="System.Windows.Controls.Button"/>, adding <see cref="SymbolRegular"/>.
/// </summary>
/// <example>
/// <code lang="xml">
/// &lt;ui:Button
///     Appearance="Primary"
///     Content="WPF UI button with font icon"
///     Icon="Fluent24" /&gt;
/// </code>
/// </example>
/// <remarks>
/// The <see cref="Button"/> class inherits from the base <see cref="System.Windows.Controls.Button"/> class.
/// </remarks>
public class Button : System.Windows.Controls.Button, IAppearanceControl, IIconControl
{
    /// <summary>
    /// Property for <see cref="Icon"/>.
    /// </summary>
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon),
        typeof(Common.SymbolRegular), typeof(Button),
        new PropertyMetadata(Common.SymbolRegular.Empty));

    /// <summary>
    /// Property for <see cref="IconFilled"/>.
    /// </summary>
    public static readonly DependencyProperty IconFilledProperty = DependencyProperty.Register(nameof(IconFilled),
        typeof(bool), typeof(Button), new PropertyMetadata(false));

    /// <summary>
    /// Property for <see cref="Appearance"/>.
    /// </summary>
    public static readonly DependencyProperty AppearanceProperty = DependencyProperty.Register(
        nameof(Appearance),
        typeof(ControlAppearance),
        typeof(Button),
        new PropertyMetadata(ControlAppearance.Primary)
    );

    /// <summary>
    /// Property for <see cref="MouseOverBackground"/>.
    /// </summary>
    public static readonly DependencyProperty MouseOverBackgroundProperty =
        DependencyProperty.Register(
            nameof(MouseOverBackground),
            typeof(Brush),
            typeof(Button),
            new PropertyMetadata(Border.BackgroundProperty.DefaultMetadata.DefaultValue)
        );

    /// <summary>
    /// Property for <see cref="MouseOverBorderBrush"/>.
    /// </summary>
    public static readonly DependencyProperty MouseOverBorderBrushProperty =
        DependencyProperty.Register(
            nameof(MouseOverBorderBrush),
            typeof(Brush),
            typeof(Button),
            new PropertyMetadata(Border.BorderBrushProperty.DefaultMetadata.DefaultValue)
        );

    /// <summary>
    /// Property for <see cref="PressedForeground"/>.
    /// </summary>
    public static readonly DependencyProperty PressedForegroundProperty =
        DependencyProperty.Register(
            nameof(PressedForeground),
            typeof(Brush),
            typeof(Button),
            new FrameworkPropertyMetadata(
                SystemColors.ControlTextBrush,
                FrameworkPropertyMetadataOptions.Inherits
            )
        );

    /// <summary>
    /// Property for <see cref="PressedBackground"/>.
    /// </summary>
    public static readonly DependencyProperty PressedBackgroundProperty =
        DependencyProperty.Register(
            nameof(PressedBackground),
            typeof(Brush),
            typeof(Button),
            new PropertyMetadata(Border.BackgroundProperty.DefaultMetadata.DefaultValue)
        );

    /// <summary>
    /// Property for <see cref="PressedBorderBrush"/>.
    /// </summary>
    public static readonly DependencyProperty PressedBorderBrushProperty =
        DependencyProperty.Register(
            nameof(PressedBorderBrush),
            typeof(Brush),
            typeof(Button),
            new PropertyMetadata(Border.BorderBrushProperty.DefaultMetadata.DefaultValue)
        );

    /// <summary>
    /// Property for <see cref="CornerRadius"/>.
    /// </summary>
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(Button), (PropertyMetadata)new FrameworkPropertyMetadata((object)new CornerRadius(), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

    /// <summary>
    /// Gets or sets displayed <see cref="SymbolRegular"/>.
    /// </summary>
    [Bindable(true), Category("Appearance")]
    public Common.SymbolRegular Icon
    {
        get => (Common.SymbolRegular)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <summary>
    /// Gets or sets a value deciding whether the <see cref="Icon"/> should be filled.
    /// </summary>
    public bool IconFilled
    {
        get => (bool)GetValue(IconFilledProperty);
        set => SetValue(IconFilledProperty, value);
    }

    /// <inheritdoc />
    [Bindable(true), Category("Appearance")]
    public ControlAppearance Appearance
    {
        get => (ControlAppearance)GetValue(AppearanceProperty);
        set => SetValue(AppearanceProperty, value);
    }

    /// <summary>
    /// Background <see cref="Brush"/> when the user interacts with an element with a pointing device.
    /// </summary>
    [Bindable(true), Category("Appearance")]
    public Brush MouseOverBackground
    {
        get => (Brush)GetValue(MouseOverBackgroundProperty);
        set => SetValue(MouseOverBackgroundProperty, value);
    }

    /// <summary>
    /// Border <see cref="Brush"/> when the user interacts with an element with a pointing device.
    /// </summary>
    [Bindable(true), Category("Appearance")]
    public Brush MouseOverBorderBrush
    {
        get => (Brush)GetValue(MouseOverBorderBrushProperty);
        set => SetValue(MouseOverBorderBrushProperty, value);
    }

    /// <summary>
    /// Foreground when pressed.
    /// </summary>
    [Bindable(true), Category("Appearance")]
    public Brush PressedForeground
    {
        get => (Brush)GetValue(PressedForegroundProperty);
        set => SetValue(PressedForegroundProperty, value);
    }

    /// <summary>
    /// Background <see cref="Brush"/> when the user clicks the button.
    /// </summary>
    [Bindable(true), Category("Appearance")]
    public Brush PressedBackground
    {
        get => (Brush)GetValue(PressedBackgroundProperty);
        set => SetValue(PressedBackgroundProperty, value);
    }

    /// <summary>
    /// Border <see cref="Brush"/> when the user clicks the button.
    /// </summary>
    [Bindable(true), Category("Appearance")]
    public Brush PressedBorderBrush
    {
        get => (Brush)GetValue(PressedBorderBrushProperty);
        set => SetValue(PressedBorderBrushProperty, value);
    }

    /// <summary>
    /// Gets or sets a value that represents the degree to which the corners of a <see cref="T:System.Windows.Controls.Border" /> are rounded.
    /// </summary>
    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, (object)value);
    }
}
