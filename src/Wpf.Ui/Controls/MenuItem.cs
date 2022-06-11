// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.ComponentModel;
using System.Windows;

namespace Wpf.Ui.Controls;

/// <summary>
/// Extended <see cref="System.Windows.Controls.MenuItem"/> with <see cref="Wpf.Ui.Common.SymbolRegular"/> properties.
/// </summary>
public class MenuItem : System.Windows.Controls.MenuItem
{
    /// <summary>
    /// Property for <see cref="SymbolIcon"/>.
    /// </summary>
    public static readonly DependencyProperty SymbolIconProperty = DependencyProperty.Register(nameof(SymbolIcon),
        typeof(Common.SymbolRegular), typeof(Wpf.Ui.Controls.MenuItem),
        new PropertyMetadata(Common.SymbolRegular.Empty));

    /// <summary>
    /// Property for <see cref="SymbolIconFilled"/>.
    /// </summary>
    public static readonly DependencyProperty SymbolIconFilledProperty = DependencyProperty.Register(
        nameof(SymbolIconFilled),
        typeof(bool), typeof(Wpf.Ui.Controls.MenuItem), new PropertyMetadata(false));

    ///// <summary>
    ///// Property for <see cref="SymbolIconForeground"/>.
    ///// </summary>
    //public static readonly DependencyProperty SymbolIconForegroundProperty = DependencyProperty.Register(
    //    nameof(SymbolIconForeground),
    //    typeof(Brush), typeof(Wpf.Ui.Controls.MenuItem), new FrameworkPropertyMetadata(
    //        System.Windows.SystemColors.ControlTextBrush,
    //        FrameworkPropertyMetadataOptions.Inherits));

    /// <summary>
    /// Gets or sets displayed <see cref="Common.SymbolRegular"/>.
    /// </summary>
    [Bindable(true), Category("Appearance")]
    public Common.SymbolRegular SymbolIcon
    {
        get => (Common.SymbolRegular)GetValue(SymbolIconProperty);
        set => SetValue(SymbolIconProperty, value);
    }

    /// <summary>
    /// Defines whether or not we should use the <see cref="Common.SymbolFilled"/>.
    /// </summary>
    [Bindable(true), Category("Appearance")]
    public bool SymbolIconFilled
    {
        get => (bool)GetValue(SymbolIconFilledProperty);
        set => SetValue(SymbolIconFilledProperty, value);
    }

    ///// <summary>
    ///// Foreground of the <see cref="Wpf.Ui.Common.SymbolRegular"/>.
    ///// </summary>
    //[Bindable(true), Category("Appearance")]
    //public Brush SymbolIconForeground
    //{
    //    get => (Brush)GetValue(SymbolIconForegroundProperty);
    //    set => SetValue(SymbolIconForegroundProperty, value);
    //}
}
