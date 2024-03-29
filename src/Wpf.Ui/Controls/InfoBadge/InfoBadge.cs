// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Converters;

namespace Wpf.Ui.Controls;

public class InfoBadge : System.Windows.Controls.Control
{
    /// <summary>
    /// Property for <see cref="Icon"/>.
    /// </summary>
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        nameof(Icon),
        typeof(IconElement),
        typeof(InfoBadge),
        new PropertyMetadata(null, null, IconElement.Coerce)
    );

    /// <summary>
    /// Property for <see cref="Severity"/>.
    /// </summary>
    public static readonly DependencyProperty SeverityProperty = DependencyProperty.Register(
        nameof(Severity),
        typeof(InfoBadgeSeverity),
        typeof(InfoBadge),
        new PropertyMetadata(InfoBadgeSeverity.Informational)
    );

    /// <summary>
    /// Property for <see cref="Value"/>.
    /// </summary>
    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        nameof(Value),
        typeof(string),
        typeof(InfoBadge),
        new PropertyMetadata(string.Empty)
    );

    /// <summary>
    /// Property for <see cref="CornerRadius"/>.
    /// </summary>
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        nameof(CornerRadius),
        typeof(CornerRadius),
        typeof(InfoBadge),
        (PropertyMetadata)
            new FrameworkPropertyMetadata(
                (object)new CornerRadius(8),
                FrameworkPropertyMetadataOptions.AffectsMeasure
                    | FrameworkPropertyMetadataOptions.AffectsRender
            )
    );

    /// <summary>
    /// Gets or sets the title of the <see cref="Severity" />.
    /// </summary>
    public InfoBadgeSeverity Severity
    {
        get => (InfoBadgeSeverity)GetValue(SeverityProperty);
        set => SetValue(SeverityProperty, value);
    }

    /// <summary>
    /// Gets or sets the title of the <see cref="Value" />.
    /// </summary>
    public string Value
    {
        get => (string)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    /// <summary>
    /// Gets or sets the title of the <see cref="CornerRadius" />.
    /// </summary>
    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    /// <summary>
    /// Gets or sets displayed <see cref="IconElement"/>.
    /// </summary>
    [Bindable(true), Category("Appearance")]
    public IconElement? Icon
    {
        get => (IconElement)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }
}
