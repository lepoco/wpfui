// Based on Windows UI Library
// Copyright(c) Microsoft Corporation.All rights reserved.

// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using Wpf.Ui.Controls.IconElements;

namespace Wpf.Ui.Controls.Navigation;

// https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.navigationviewitemheader?view=winrt-22621

/// <summary>
/// Represents a header for a group of menu items in a NavigationMenu.
/// </summary>
[ToolboxItem(true)]
[System.Drawing.ToolboxBitmap(typeof(NavigationViewItemHeader), "NavigationViewItemHeader.bmp")]
public class NavigationViewItemHeader : System.Windows.Controls.Control
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
        typeof(IconElement), typeof(NavigationViewItemHeader),
        new PropertyMetadata(null));

    /// <summary>
    /// Text presented in the header element.
    /// </summary>
    [Bindable(true)]
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    [Bindable(true), Category("Appearance")]
    public IconElement? Icon
    {
        get => (IconElement)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }
}
