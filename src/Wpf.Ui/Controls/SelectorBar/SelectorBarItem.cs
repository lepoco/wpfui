// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

/* Based on Windows UI Library
   Copyright(c) Microsoft Corporation.All rights reserved. */

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Represents one item of content in a <see cref="SelectorBar"/> control.
/// </summary>
/// <example>
/// <code lang="xml">
/// &lt;ui:SelectorBarItem Text="Recent" Icon="{ui:SymbolIcon Clock24}" /&gt;
/// </code>
/// </example>
public class SelectorBarItem : System.Windows.Controls.ListBoxItem
{
    /// <summary>Identifies the <see cref="Text"/> dependency property.</summary>
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
        nameof(Text),
        typeof(string),
        typeof(SelectorBarItem),
        new PropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="Icon"/> dependency property.</summary>
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        nameof(Icon),
        typeof(IconElement),
        typeof(SelectorBarItem),
        new PropertyMetadata(null, null, IconElement.Coerce)
    );

    /// <summary>Identifies the <see cref="IconMargin"/> dependency property.</summary>
    public static readonly DependencyProperty IconMarginProperty = DependencyProperty.Register(
        nameof(IconMargin),
        typeof(Thickness),
        typeof(SelectorBarItem),
        new PropertyMetadata(new Thickness(0))
    );

    static SelectorBarItem()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(SelectorBarItem),
            new FrameworkPropertyMetadata(typeof(SelectorBarItem))
        );
    }

    /// <summary>
    /// Gets or sets the text label for this item.
    /// </summary>
    public string? Text
    {
        get => (string?)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>
    /// Gets or sets the graphical icon for this item.
    /// </summary>
    public IconElement? Icon
    {
        get => (IconElement?)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <summary>
    /// Gets or sets the margin for the <see cref="Icon"/>.
    /// </summary>
    public Thickness IconMargin
    {
        get => (Thickness)GetValue(IconMarginProperty);
        set => SetValue(IconMarginProperty, value);
    }
}
