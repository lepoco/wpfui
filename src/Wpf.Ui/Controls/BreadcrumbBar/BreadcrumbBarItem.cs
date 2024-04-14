// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.
//
// Based on Windows UI Library

using Wpf.Ui.Converters;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Represents an item in a <see cref="BreadcrumbBar"/> control.
/// </summary>
public class BreadcrumbBarItem : System.Windows.Controls.ContentControl
{
    /// <summary>Identifies the <see cref="Icon"/> dependency property.</summary>
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        nameof(Icon),
        typeof(IconElement),
        typeof(BreadcrumbBarItem),
        new PropertyMetadata(null, null, IconElement.Coerce)
    );

    /// <summary>Identifies the <see cref="IconMargin"/> dependency property.</summary>
    public static readonly DependencyProperty IconMarginProperty = DependencyProperty.Register(
        nameof(IconMargin),
        typeof(Thickness),
        typeof(BreadcrumbBarItem),
        new PropertyMetadata(new Thickness(0))
    );

    /// <summary>Identifies the <see cref="IsLast"/> dependency property.</summary>
    public static readonly DependencyProperty IsLastProperty = DependencyProperty.Register(
        nameof(IsLast),
        typeof(bool),
        typeof(BreadcrumbBarItem),
        new PropertyMetadata(false)
    );

    /// <summary>
    /// Gets or sets displayed <see cref="IconElement"/>.
    /// </summary>
    public IconElement? Icon
    {
        get => (IconElement?)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <summary>
    /// Gets or sets get or sets margin for the <see cref="Icon"/>
    /// </summary>
    public Thickness IconMargin
    {
        get => (Thickness)GetValue(IconMarginProperty);
        set => SetValue(IconMarginProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the current item is the last one.
    /// </summary>
    public bool IsLast
    {
        get => (bool)GetValue(IsLastProperty);
        set => SetValue(IsLastProperty, value);
    }
}
