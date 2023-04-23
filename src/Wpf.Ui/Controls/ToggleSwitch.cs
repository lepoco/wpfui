// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.ComponentModel;
using System.Drawing;
using System.Windows;

namespace Wpf.Ui.Controls;

/// <summary>
/// Use <see cref="ToggleSwitch"/> to present users with two mutally exclusive options (like on/off).
/// </summary>
[ToolboxItem(true)]
[ToolboxBitmap(typeof(ToggleSwitch), "ToggleSwitch.bmp")]
public class ToggleSwitch : System.Windows.Controls.Primitives.ToggleButton
{
    public static readonly DependencyProperty OffContentProperty = DependencyProperty.Register(
        "OffContent", typeof(object), typeof(ToggleSwitch), new PropertyMetadata(null));

    [Bindable(true)]
    public object OffContent
    {
        get => GetValue(OffContentProperty);
        set => SetValue(OffContentProperty, value);
    }

    public static readonly DependencyProperty OnContentProperty = DependencyProperty.Register(
        "OnContent", typeof(object), typeof(ToggleSwitch), new PropertyMetadata(null));

    [Bindable(true)]
    public object OnContent
    {
        get => GetValue(OnContentProperty);
        set => SetValue(OnContentProperty, value);
    }
}
