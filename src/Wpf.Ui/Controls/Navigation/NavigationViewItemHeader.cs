// Based on Windows UI Library
// Copyright(c) Microsoft Corporation.All rights reserved.

// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace Wpf.Ui.Controls.Navigation;

// https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.navigationviewitemheader?view=winrt-22621

/// <summary>
/// Represents a header for a group of menu items in a NavigationMenu.
/// </summary>
[ToolboxItem(true)]
[System.Drawing.ToolboxBitmap(typeof(NavigationViewItemHeader), "NavigationViewItemHeader.bmp")]
public class NavigationViewItemHeader : System.Windows.Controls.Control, IIconControl
{
    /// <summary>
    /// Property for <see cref="Text"/>.
    /// </summary>
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text),
        typeof(string), typeof(NavigationViewItemHeader),
        new PropertyMetadata(string.Empty));

    /// <summary>
    /// Property for <see cref="Icon"/>.
    /// </summary>
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon),
        typeof(Common.SymbolRegular), typeof(NavigationViewItemHeader),
        new PropertyMetadata(Common.SymbolRegular.Empty));

    /// <summary>
    /// Property for <see cref="IconFilled"/>.
    /// </summary>
    public static readonly DependencyProperty IconFilledProperty = DependencyProperty.Register(nameof(IconFilled),
        typeof(bool), typeof(NavigationViewItemHeader), new PropertyMetadata(false));

    /// <summary>
    /// Property for <see cref="IconForeground"/>.
    /// </summary>
    public static readonly DependencyProperty IconForegroundProperty = DependencyProperty.Register(nameof(IconForeground),
        typeof(Brush), typeof(NavigationViewItemHeader), new FrameworkPropertyMetadata(SystemColors.ControlTextBrush,
            FrameworkPropertyMetadataOptions.Inherits));

    /// <summary>
    /// Property for <see cref="IconSize"/>.
    /// </summary>
    public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(nameof(IconSize),
        typeof(double), typeof(NavigationViewItemHeader), new FrameworkPropertyMetadata(13d));

    /// <summary>
    /// Text presented in the header element.
    /// </summary>
    [Bindable(true)]
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <inheritdoc />
    [Bindable(true), Category("Appearance")]
    public Common.SymbolRegular Icon
    {
        get => (Common.SymbolRegular)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <inheritdoc />
    [Bindable(true), Category("Appearance")]
    public bool IconFilled
    {
        get => (bool)GetValue(IconFilledProperty);
        set => SetValue(IconFilledProperty, value);
    }

    /// <summary>
    /// Foreground of the <see cref="SymbolIcon"/>.
    /// </summary>
    [Bindable(true), Category("Appearance")]
    public Brush IconForeground
    {
        get => (Brush)GetValue(IconForegroundProperty);
        set => SetValue(IconForegroundProperty, value);
    }

    /// <summary>
    /// Font size of the <see cref="SymbolIcon"/>.
    /// </summary>
    [Bindable(true), Category("Appearance")]
    public double IconSize
    {
        get => (double)GetValue(IconSizeProperty);
        set => SetValue(IconSizeProperty, value);
    }
}
