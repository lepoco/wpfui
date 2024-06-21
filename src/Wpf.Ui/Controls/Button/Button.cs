// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;

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
///     Icon="{ui:SymbolIcon Symbol=Fluent24}" /&gt;
/// </code>
/// <code lang="xml">
/// &lt;ui:Button
///     Appearance="Primary"
///     Content="WPF UI button with font icon"
///     Icon="{ui:FontIcon Glyph='&#x1F308;'}"/&gt;
/// </code>
/// </example>
/// <remarks>
/// The <see cref="Button"/> class inherits from the base <see cref="System.Windows.Controls.Button"/> class.
/// </remarks>
public class Button : System.Windows.Controls.Button, IAppearanceControl, IIconControl
{
    /// <summary>Identifies the <see cref="Icon"/> dependency property.</summary>
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        nameof(Icon),
        typeof(IconElement),
        typeof(Button),
        new PropertyMetadata(null, null, IconElement.Coerce)
    );

    /// <summary>Identifies the <see cref="Appearance"/> dependency property.</summary>
    public static readonly DependencyProperty AppearanceProperty = DependencyProperty.Register(
        nameof(Appearance),
        typeof(ControlAppearance),
        typeof(Button),
        new PropertyMetadata(ControlAppearance.Primary)
    );

    /// <summary>Identifies the <see cref="MouseOverBackground"/> dependency property.</summary>
    public static readonly DependencyProperty MouseOverBackgroundProperty = DependencyProperty.Register(
        nameof(MouseOverBackground),
        typeof(Brush),
        typeof(Button),
        new PropertyMetadata(Border.BackgroundProperty.DefaultMetadata.DefaultValue)
    );

    /// <summary>Identifies the <see cref="MouseOverBorderBrush"/> dependency property.</summary>
    public static readonly DependencyProperty MouseOverBorderBrushProperty = DependencyProperty.Register(
        nameof(MouseOverBorderBrush),
        typeof(Brush),
        typeof(Button),
        new PropertyMetadata(Border.BorderBrushProperty.DefaultMetadata.DefaultValue)
    );

    /// <summary>Identifies the <see cref="PressedForeground"/> dependency property.</summary>
    public static readonly DependencyProperty PressedForegroundProperty = DependencyProperty.Register(
        nameof(PressedForeground),
        typeof(Brush),
        typeof(Button),
        new FrameworkPropertyMetadata(
            SystemColors.ControlTextBrush,
            FrameworkPropertyMetadataOptions.Inherits
        )
    );

    /// <summary>Identifies the <see cref="PressedBackground"/> dependency property.</summary>
    public static readonly DependencyProperty PressedBackgroundProperty = DependencyProperty.Register(
        nameof(PressedBackground),
        typeof(Brush),
        typeof(Button),
        new PropertyMetadata(Border.BackgroundProperty.DefaultMetadata.DefaultValue)
    );

    /// <summary>Identifies the <see cref="PressedBorderBrush"/> dependency property.</summary>
    public static readonly DependencyProperty PressedBorderBrushProperty = DependencyProperty.Register(
        nameof(PressedBorderBrush),
        typeof(Brush),
        typeof(Button),
        new PropertyMetadata(Border.BorderBrushProperty.DefaultMetadata.DefaultValue)
    );

    /// <summary>Identifies the <see cref="CornerRadius"/> dependency property.</summary>
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        nameof(CornerRadius),
        typeof(CornerRadius),
        typeof(Button),
        new FrameworkPropertyMetadata(
            default(CornerRadius),
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender
        )
    );

    /// <summary>Identifies the <see cref="HorizontalIconAlignment"/> dependency property.</summary>
    public static readonly DependencyProperty HorizontalIconAlignmentProperty = DependencyProperty.Register(
        nameof(HorizontalIconAlignment),
        typeof(HorizontalIconAlignment),
        typeof(Button),
        new PropertyMetadata(HorizontalIconAlignment.Left));

    /// <summary>
    /// Gets or sets displayed <see cref="IconElement"/>.
    /// </summary>
    [Bindable(true)]
    [Category("Appearance")]
    public IconElement? Icon
    {
        get => (IconElement?)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <inheritdoc />
    [Bindable(true)]
    [Category("Appearance")]
    public ControlAppearance Appearance
    {
        get => (ControlAppearance)GetValue(AppearanceProperty);
        set => SetValue(AppearanceProperty, value);
    }

    /// <summary>
    /// Gets or sets background <see cref="Brush"/>.
    /// </summary>
    [Bindable(true)]
    [Category("Appearance")]
    public Brush MouseOverBackground
    {
        get => (Brush)GetValue(MouseOverBackgroundProperty);
        set => SetValue(MouseOverBackgroundProperty, value);
    }

    /// <summary>
    /// Gets or sets border <see cref="Brush"/> when the user mouses over the button.
    /// </summary>
    [Bindable(true)]
    [Category("Appearance")]
    public Brush MouseOverBorderBrush
    {
        get => (Brush)GetValue(MouseOverBorderBrushProperty);
        set => SetValue(MouseOverBorderBrushProperty, value);
    }

    /// <summary>
    /// Gets or sets the foreground <see cref="Brush"/> when the user clicks the button.
    /// </summary>
    [Bindable(true)]
    [Category("Appearance")]
    public Brush PressedForeground
    {
        get => (Brush)GetValue(PressedForegroundProperty);
        set => SetValue(PressedForegroundProperty, value);
    }

    /// <summary>
    /// Gets or sets background <see cref="Brush"/> when the user clicks the button.
    /// </summary>
    [Bindable(true)]
    [Category("Appearance")]
    public Brush PressedBackground
    {
        get => (Brush)GetValue(PressedBackgroundProperty);
        set => SetValue(PressedBackgroundProperty, value);
    }

    /// <summary>
    /// Gets or sets border <see cref="Brush"/> when the user clicks the button.
    /// </summary>
    [Bindable(true)]
    [Category("Appearance")]
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
        set => SetValue(CornerRadiusProperty, value);
    }

    /// <summary>
    /// Gets or sets whether the icon should be shown left or right from the content.
    /// </summary>
    public HorizontalIconAlignment HorizontalIconAlignment
    {
        get => (HorizontalIconAlignment)GetValue(HorizontalIconAlignmentProperty);
        set => SetValue(HorizontalIconAlignmentProperty, value);
    }
}