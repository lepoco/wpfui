// Based on Windows UI Library
// Copyright(c) Microsoft Corporation.All rights reserved.

// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Diagnostics;
using System.Windows;
using Wpf.Ui.Common;

namespace Wpf.Ui.Controls.Navigation;

public partial class NavigationView
{
    /// <summary>
    /// Property for <see cref="PaneOpened"/>.
    /// </summary>
    public static readonly RoutedEvent PaneOpenedEvent = EventManager.RegisterRoutedEvent(nameof(PaneOpened),
        RoutingStrategy.Bubble, typeof(TypedEventHandler<NavigationView, RoutedEventArgs>), typeof(NavigationView));

    /// <summary>
    /// Property for <see cref="PaneClosed"/>.
    /// </summary>
    public static readonly RoutedEvent PaneClosedEvent = EventManager.RegisterRoutedEvent(nameof(PaneClosed),
        RoutingStrategy.Bubble, typeof(TypedEventHandler<NavigationView, RoutedEventArgs>), typeof(NavigationView));

    /// <summary>
    /// Property for <see cref="SelectionChanged"/>.
    /// </summary>
    public static readonly RoutedEvent SelectionChangedEvent = EventManager.RegisterRoutedEvent(nameof(SelectionChanged),
        RoutingStrategy.Bubble, typeof(TypedEventHandler<NavigationView, RoutedEventArgs>), typeof(NavigationView));

    /// <summary>
    /// Property for <see cref="ItemInvoked"/>.
    /// </summary>
    public static readonly RoutedEvent ItemInvokedEvent = EventManager.RegisterRoutedEvent(nameof(ItemInvoked),
        RoutingStrategy.Bubble, typeof(TypedEventHandler<NavigationView, RoutedEventArgs>), typeof(NavigationView));

    /// <summary>
    /// Property for <see cref="BackRequested"/>.
    /// </summary>
    public static readonly RoutedEvent BackRequestedEvent = EventManager.RegisterRoutedEvent(nameof(BackRequested),
        RoutingStrategy.Bubble, typeof(TypedEventHandler<NavigationView, RoutedEventArgs>), typeof(NavigationView));
    /// <summary>
    /// Property for <see cref="Navigating"/>.
    /// </summary>
    public static readonly RoutedEvent NavigatingEvent = EventManager.RegisterRoutedEvent(nameof(Navigating),
        RoutingStrategy.Bubble, typeof(TypedEventHandler<NavigationView, NavigatingCancelEventArgs>), typeof(NavigationView));

    /// <inheritdoc/>
    public event TypedEventHandler<NavigationView, RoutedEventArgs> PaneOpened
    {
        add => AddHandler(PaneOpenedEvent, value);
        remove => RemoveHandler(PaneOpenedEvent, value);
    }

    /// <inheritdoc/>
    public event TypedEventHandler<NavigationView, RoutedEventArgs> PaneClosed
    {
        add => AddHandler(PaneClosedEvent, value);
        remove => RemoveHandler(PaneClosedEvent, value);
    }

    /// <inheritdoc/>
    public event TypedEventHandler<NavigationView, RoutedEventArgs> SelectionChanged
    {
        add => AddHandler(SelectionChangedEvent, value);
        remove => RemoveHandler(SelectionChangedEvent, value);
    }

    /// <inheritdoc/>
    public event TypedEventHandler<NavigationView, RoutedEventArgs> ItemInvoked
    {
        add => AddHandler(ItemInvokedEvent, value);
        remove => RemoveHandler(ItemInvokedEvent, value);
    }

    /// <inheritdoc/>
    public event TypedEventHandler<NavigationView, RoutedEventArgs> BackRequested
    {
        add => AddHandler(BackRequestedEvent, value);
        remove => RemoveHandler(BackRequestedEvent, value);
    }

    /// <inheritdoc/>
    public event TypedEventHandler<NavigationView, NavigatingCancelEventArgs> Navigating
    {
        add => AddHandler(NavigatingEvent, value);
        remove => RemoveHandler(NavigatingEvent, value);
    }

    /// <summary>
    /// Raises the pane opened event.
    /// </summary>
    [DebuggerStepThrough]
    protected virtual void OnPaneOpened()
    {
        RaiseEvent(new RoutedEventArgs(PaneOpenedEvent, this));
    }

    /// <summary>
    /// Raises the pane closed event.
    /// </summary>
    [DebuggerStepThrough]
    protected virtual void OnPaneClosed()
    {
        RaiseEvent(new RoutedEventArgs(PaneClosedEvent, this));
    }

    /// <summary>
    /// Raises the selection changed event.
    /// </summary>
    [DebuggerStepThrough]
    protected virtual void OnSelectionChanged()
    {
        RaiseEvent(new RoutedEventArgs(SelectionChangedEvent, this));
    }

    /// <summary>
    /// Raises the item invoked event.
    /// </summary>
    [DebuggerStepThrough]
    protected virtual void OnItemInvoked()
    {
        RaiseEvent(new RoutedEventArgs(ItemInvokedEvent, this));
    }

    /// <summary>
    /// Raises the back requested event.
    /// </summary>
    [DebuggerStepThrough]
    protected virtual void OnBackRequested()
    {
        RaiseEvent(new RoutedEventArgs(BackRequestedEvent));
    }

    /// <summary>
    /// Raises the navigating requested event.
    /// </summary>
    /// <param name="sourcePageType"></param>
    /// <returns></returns>
    [DebuggerStepThrough]
    protected virtual bool OnNavigating(object sourcePageType)
    {
        var eventArgs = new NavigatingCancelEventArgs(NavigatingEvent, this, sourcePageType);

        RaiseEvent(eventArgs);

        return eventArgs.Cancel;
    }
}
