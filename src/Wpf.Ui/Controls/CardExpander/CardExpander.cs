// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Converters;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Inherited from the <see cref="System.Windows.Controls.Expander"/> control which can hide the collapsible content.
/// </summary>
public class CardExpander : System.Windows.Controls.Expander
{
    /// <summary>
    /// Property for <see cref="Icon"/>.
    /// </summary>
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        nameof(Icon),
        typeof(IconElement),
        typeof(CardExpander),
        new PropertyMetadata(null, null, IconElement.Coerce)
    );

    /// <summary>
    /// Property for <see cref="CornerRadius"/>.
    /// </summary>
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        nameof(CornerRadius),
        typeof(CornerRadius),
        typeof(CardExpander),
        new PropertyMetadata(new CornerRadius(4))
    );

    /// <summary>
    /// Property for <see cref="ContentPadding"/>.
    /// </summary>
    public static readonly DependencyProperty ContentPaddingProperty = DependencyProperty.Register(
        nameof(ContentPadding),
        typeof(Thickness),
        typeof(CardExpander),
        new FrameworkPropertyMetadata(
            default(Thickness),
            FrameworkPropertyMetadataOptions.AffectsParentMeasure
        )
    );

    /// <summary>
    /// Gets or sets displayed <see cref="IconElement"/>.
    /// </summary>
    [Bindable(true), Category("Appearance")]
    public IconElement? Icon
    {
        get => (IconElement)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <summary>
    /// Gets or sets displayed <see cref="IconElement"/>.
    /// </summary>
    [Bindable(true), Category("Appearance")]
    public CornerRadius? CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    /// <summary>
    /// Gets or sets content padding Property
    /// </summary>
    [Bindable(true), Category("Layout")]
    public Thickness ContentPadding
    {
        get { return (Thickness)GetValue(ContentPaddingProperty); }
        set { SetValue(ContentPaddingProperty, value); }
    }
}
