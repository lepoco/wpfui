// Based on Windows UI Library
// Copyright(c) Microsoft Corporation.All rights reserved.

// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.Generic;
using System.Windows;

namespace Wpf.Ui.Controls.Navigation;

public partial class NavigationView
{
    /// <summary>
    /// Property for <see cref="Header"/>.
    /// </summary>
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof(Header),
        typeof(object), typeof(NavigationView),
        new FrameworkPropertyMetadata(((object)null!)));

    /// <summary>
    /// Property for <see cref="MenuItems"/>.
    /// </summary>
    public static readonly DependencyProperty MenuItemsProperty = DependencyProperty.Register(nameof(MenuItems),
        typeof(IList<object>), typeof(NavigationView),
        new FrameworkPropertyMetadata(new List<object>()));

    /// <summary>
    /// Property for <see cref="MenuItemsSource"/>.
    /// </summary>
    public static readonly DependencyProperty MenuItemsSourceProperty = DependencyProperty.Register(nameof(MenuItemsSource),
        typeof(object), typeof(NavigationView),
        new FrameworkPropertyMetadata(((object)null!)));

    /// <summary>
    /// Property for <see cref="FooterMenuItems"/>.
    /// </summary>
    public static readonly DependencyProperty FooterMenuItemsProperty = DependencyProperty.Register(nameof(FooterMenuItemsProperty),
        typeof(IList<object>), typeof(NavigationView),
        new FrameworkPropertyMetadata(new List<object>()));

    /// <summary>
    /// Property for <see cref="FooterMenuItemsSource"/>.
    /// </summary>
    public static readonly DependencyProperty FooterMenuItemsSourceProperty = DependencyProperty.Register(nameof(FooterMenuItemsSource),
        typeof(object), typeof(NavigationView),
        new FrameworkPropertyMetadata(((object)null!)));

    /// <summary>
    /// Property for <see cref="ContentOverlay"/>.
    /// </summary>
    public static readonly DependencyProperty ContentOverlayProperty = DependencyProperty.Register(nameof(ContentOverlay),
        typeof(object), typeof(NavigationView),
        new FrameworkPropertyMetadata(((object)null!)));

    /// <summary>
    /// Property for <see cref="IsBackEnabled"/>.
    /// </summary>
    public static readonly DependencyProperty IsBackEnabledProperty = DependencyProperty.Register(nameof(IsBackEnabled),
        typeof(bool), typeof(NavigationView),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Property for <see cref="IsBackButtonVisible"/>.
    /// </summary>
    public static readonly DependencyProperty IsBackButtonVisibleProperty = DependencyProperty.Register(nameof(IsBackButtonVisible),
        typeof(NavigationViewBackButtonVisible), typeof(NavigationView),
        new FrameworkPropertyMetadata(NavigationViewBackButtonVisible.Auto));

    /// <summary>
    /// Property for <see cref="IsPaneOpen"/>.
    /// </summary>
    public static readonly DependencyProperty IsPaneOpenProperty = DependencyProperty.Register(nameof(IsPaneOpen),
        typeof(bool), typeof(NavigationView),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Property for <see cref="IsPaneVisible"/>.
    /// </summary>
    public static readonly DependencyProperty IsPaneVisibleProperty = DependencyProperty.Register(nameof(IsPaneVisible),
        typeof(bool), typeof(NavigationView),
        new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Property for <see cref="OpenPaneLength"/>.
    /// </summary>
    public static readonly DependencyProperty OpenPaneLengthProperty = DependencyProperty.Register(nameof(OpenPaneLength),
        typeof(double), typeof(NavigationView),
        new FrameworkPropertyMetadata(0D));

    /// <summary>
    /// Property for <see cref="VisualStyle"/>.
    /// </summary>
    public static readonly DependencyProperty VisualStyleProperty = DependencyProperty.Register(nameof(VisualStyle),
        typeof(NavigationViewVisualStyle), typeof(NavigationView),
        new FrameworkPropertyMetadata(NavigationViewVisualStyle.Compact));

    /// <inheritdoc/>
    public object Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    /// <inheritdoc/>
    public IList<object> MenuItems
    {
        get => (IList<object>)GetValue(MenuItemsProperty);
        set => SetValue(MenuItemsProperty, value);
    }

    /// <inheritdoc/>
    public object MenuItemsSource
    {
        get => GetValue(MenuItemsSourceProperty);
        set => SetValue(MenuItemsSourceProperty, value);
    }

    /// <inheritdoc/>
    public IList<object> FooterMenuItems
    {
        get => (IList<object>)GetValue(FooterMenuItemsProperty);
        set => SetValue(FooterMenuItemsProperty, value);
    }

    /// <inheritdoc/>
    public object FooterMenuItemsSource
    {
        get => GetValue(FooterMenuItemsSourceProperty);
        set => SetValue(FooterMenuItemsSourceProperty, value);
    }

    /// <inheritdoc/>
    public object ContentOverlay
    {
        get => GetValue(ContentOverlayProperty);
        set => SetValue(ContentOverlayProperty, value);
    }

    /// <inheritdoc/>
    public bool IsBackEnabled
    {
        get => (bool)GetValue(IsBackEnabledProperty);
        set => SetValue(IsBackEnabledProperty, value);
    }

    /// <inheritdoc/>
    public NavigationViewBackButtonVisible IsBackButtonVisible
    {
        get => (NavigationViewBackButtonVisible)GetValue(IsBackButtonVisibleProperty);
        set => SetValue(IsBackButtonVisibleProperty, value);
    }

    /// <inheritdoc/>
    public bool IsPaneOpen
    {
        get => (bool)GetValue(IsPaneOpenProperty);
        set => SetValue(IsPaneOpenProperty, value);
    }

    /// <inheritdoc/>
    public bool IsPaneVisible
    {
        get => (bool)GetValue(IsPaneVisibleProperty);
        set => SetValue(IsPaneVisibleProperty, value);
    }

    /// <inheritdoc/>
    public double OpenPaneLength
    {
        get => (double)GetValue(OpenPaneLengthProperty);
        set => SetValue(OpenPaneLengthProperty, value);
    }

    /// <inheritdoc/>
    public NavigationViewVisualStyle VisualStyle
    {
        get => (NavigationViewVisualStyle)GetValue(VisualStyleProperty);
        set => SetValue(VisualStyleProperty, value);
    }
}
