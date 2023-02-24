// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Wpf.Ui.Controls.IconElements;

namespace Wpf.Ui.Controls.IconSources;

/// <summary>
/// Represents the base class for an icon source.
/// </summary>
public abstract class IconSource : DependencyObject
{
    /// <summary>
    /// Property for <see cref="Foreground"/>.
    /// </summary>
    public static readonly DependencyProperty ForegroundProperty =
        DependencyProperty.Register(
            nameof(Foreground),
            typeof(Brush),
            typeof(IconSource), new FrameworkPropertyMetadata(SystemColors.ControlTextBrush));

    /// <summary>
    /// <inheritdoc cref="Control.Foreground"/>
    /// </summary>
    public Brush Foreground
    {
        get => (Brush)GetValue(ForegroundProperty);
        set => SetValue(ForegroundProperty, value);
    }

    public abstract IconElement CreateIconElement();
}
