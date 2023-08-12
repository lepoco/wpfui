// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.ComponentModel;
using System.Windows;
using Wpf.Ui.Controls.Interfaces;

namespace Wpf.Ui.Controls;

/// <summary>
/// Extended <see cref="System.Windows.Controls.TreeViewItem"/> with <see cref="Wpf.Ui.Common.SymbolRegular"/> properties.
/// </summary>
public class TreeViewItem : System.Windows.Controls.TreeViewItem, IIconControl
{
    /// <summary>
    /// Property for <see cref="Icon"/>.
    /// </summary>
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        nameof(Icon),
        typeof(Common.SymbolRegular),
        typeof(TreeViewItem),
        new PropertyMetadata(Common.SymbolRegular.Empty)
    );

    /// <summary>
    /// Property for <see cref="IconFilled"/>.
    /// </summary>
    public static readonly DependencyProperty IconFilledProperty = DependencyProperty.Register(
        nameof(IconFilled),
        typeof(bool),
        typeof(TreeViewItem),
        new PropertyMetadata(false)
    );

    /// <summary>
    /// Gets or sets displayed <see cref="Common.SymbolRegular"/>.
    /// </summary>
    [Bindable(true), Category("Appearance")]
    public Common.SymbolRegular Icon
    {
        get => (Common.SymbolRegular)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <summary>
    /// Defines whether or not we should use the <see cref="Common.SymbolFilled"/>.
    /// </summary>
    [Bindable(true), Category("Appearance")]
    public bool IconFilled
    {
        get => (bool)GetValue(IconFilledProperty);
        set => SetValue(IconFilledProperty, value);
    }
}
