// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.ComponentModel;
using System.Windows;

namespace WPFUI.Controls;

/// <summary>
/// Extended <see cref="System.Windows.Controls.Page"/> with WPF UI features. 
/// </summary>
public class UiPage : System.Windows.Controls.Page
{
    /// <summary>
    /// Property for <see cref="Scrollable"/>.
    /// </summary>
    public static readonly DependencyProperty ScrollableProperty = DependencyProperty.Register(nameof(Scrollable),
        typeof(bool), typeof(UiPage), new PropertyMetadata(false));

    /// <summary>
    /// Gets or sets a value determining whether the content should be scrollable.
    /// <para>If set, <see cref="WPFUI.Controls.DynamicScrollViewer"/> will be added to the <see cref="System.Windows.Controls.Control.Template"/></para>
    /// </summary>
    [Bindable(true), Category("Appearance")]
    public bool Scrollable
    {
        get => (bool)GetValue(ScrollableProperty);
        set => SetValue(ScrollableProperty, value);
    }

    public UiPage()
    {
        SetResourceReference(StyleProperty, typeof(UiPage));
    }

    static UiPage()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(UiPage), new FrameworkPropertyMetadata(typeof(UiPage)));
    }
}
