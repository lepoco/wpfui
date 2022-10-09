// Based on Windows UI Library
// Copyright(c) Microsoft Corporation.All rights reserved.

// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;

namespace Wpf.Ui.Controls.Navigation;

public partial class NavigationView
{
    /// <summary>
    /// Property for <see cref="PaneOpened"/>.
    /// </summary>
    public static readonly RoutedEvent PaneOpenedEvent = EventManager.RegisterRoutedEvent(nameof(PaneOpened),
        RoutingStrategy.Bubble, typeof(NavigationViewEvent), typeof(NavigationView));

    /// <summary>
    /// Property for <see cref="PaneClosed"/>.
    /// </summary>
    public static readonly RoutedEvent PaneClosedEvent = EventManager.RegisterRoutedEvent(nameof(PaneClosed),
        RoutingStrategy.Bubble, typeof(NavigationViewEvent), typeof(NavigationView));

    /// <summary>
    /// Property for <see cref="SelectionChanged"/>.
    /// </summary>
    public static readonly RoutedEvent SelectionChangedEvent = EventManager.RegisterRoutedEvent(nameof(SelectionChanged),
        RoutingStrategy.Bubble, typeof(NavigationViewEvent), typeof(NavigationView));

    /// <summary>
    /// Property for <see cref="ItemInvoked"/>.
    /// </summary>
    public static readonly RoutedEvent ItemInvokedEvent = EventManager.RegisterRoutedEvent(nameof(ItemInvoked),
        RoutingStrategy.Bubble, typeof(NavigationViewEvent), typeof(NavigationView));

    /// <summary>
    /// Property for <see cref="BackRequested"/>.
    /// </summary>
    public static readonly RoutedEvent BackRequestedEvent = EventManager.RegisterRoutedEvent(nameof(BackRequested),
        RoutingStrategy.Bubble, typeof(NavigationViewEvent), typeof(NavigationView));

    /// <inheritdoc/>
    public event NavigationViewEvent PaneOpened
    {
        add => AddHandler(PaneOpenedEvent, value);
        remove => RemoveHandler(PaneOpenedEvent, value);
    }

    /// <inheritdoc/>
    public event NavigationViewEvent PaneClosed
    {
        add => AddHandler(PaneClosedEvent, value);
        remove => RemoveHandler(PaneClosedEvent, value);
    }

    /// <inheritdoc/>
    public event NavigationViewEvent SelectionChanged
    {
        add => AddHandler(SelectionChangedEvent, value);
        remove => RemoveHandler(SelectionChangedEvent, value);
    }

    /// <inheritdoc/>
    public event NavigationViewEvent ItemInvoked
    {
        add => AddHandler(ItemInvokedEvent, value);
        remove => RemoveHandler(ItemInvokedEvent, value);
    }

    /// <inheritdoc/>
    public event NavigationViewEvent BackRequested
    {
        add => AddHandler(BackRequestedEvent, value);
        remove => RemoveHandler(BackRequestedEvent, value);
    }

    /// <summary>
    /// Raises the pane opened event.
    /// </summary>
    protected virtual void OnPaneOpened()
    {
        RaiseEvent(new RoutedEventArgs(PaneOpenedEvent));
    }

    /// <summary>
    /// Raises the pane closed event.
    /// </summary>
    protected virtual void OnPaneClosed()
    {
        RaiseEvent(new RoutedEventArgs(PaneClosedEvent));
    }

    /// <summary>
    /// Raises the selection changed event.
    /// </summary>
    protected virtual void OnSelectionChanged()
    {
        RaiseEvent(new RoutedEventArgs(SelectionChangedEvent));
    }

    /// <summary>
    /// Raises the item invoked event.
    /// </summary>
    protected virtual void OnItemInvoked()
    {
        RaiseEvent(new RoutedEventArgs(ItemInvokedEvent));
    }

    /// <summary>
    /// Raises the back requested event.
    /// </summary>
    protected virtual void OnBackRequested()
    {
        RaiseEvent(new RoutedEventArgs(BackRequestedEvent));
    }
}
