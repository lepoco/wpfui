// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using Wpf.Ui.Controls.IconSources;
using Wpf.Ui.Converters;

namespace Wpf.Ui.Controls.IconElements;

public class IconSourceElement : IconElement
{
    /// <summary>
    /// Property for <see cref="IconSource"/>.
    /// </summary>
    public static readonly DependencyProperty IconSourceProperty =
        DependencyProperty.Register(
            nameof(IconSource),
            typeof(IconSource),
            typeof(IconSourceElement),
            new FrameworkPropertyMetadata(null));

    public IconSource? IconSource
    {
        get => (IconSource)GetValue(IconSourceProperty);
        set => SetValue(IconSourceProperty, value);
    }

    protected override void OnShouldInheritForegroundFromVisualParentChanged()
    {
        
    }

    protected override UIElement InitializeChildren()
    {
        throw new InvalidOperationException($"Use {nameof(IconSourceElementConverter)} class.");
    }
}
