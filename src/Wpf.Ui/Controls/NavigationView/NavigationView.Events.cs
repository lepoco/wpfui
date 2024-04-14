// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.
//
// Based on Windows UI Library
// Copyright(c) Microsoft Corporation.All rights reserved.

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <content>
/// Defines events for <see cref="NavigationView"/>.
/// </content>
public partial class NavigationView
{
    /// <summary>Identifies the <see cref="PaneOpened"/> routed event.</summary>
    public static readonly RoutedEvent PaneOpenedEvent = EventManager.RegisterRoutedEvent(
        nameof(PaneOpened),
        RoutingStrategy.Bubble,
        typeof(TypedEventHandler<NavigationView, RoutedEventArgs>),
        typeof(NavigationView)
    );

    /// <summary>Identifies the <see cref="PaneClosed"/> routed event.</summary>
    public static readonly RoutedEvent PaneClosedEvent = EventManager.RegisterRoutedEvent(
        nameof(PaneClosed),
        RoutingStrategy.Bubble,
        typeof(TypedEventHandler<NavigationView, RoutedEventArgs>),
        typeof(NavigationView)
    );

    /// <summary>Identifies the <see cref="SelectionChanged"/> routed event.</summary>
    public static readonly RoutedEvent SelectionChangedEvent = EventManager.RegisterRoutedEvent(
        nameof(SelectionChanged),
        RoutingStrategy.Bubble,
        typeof(TypedEventHandler<NavigationView, RoutedEventArgs>),
        typeof(NavigationView)
    );

    /// <summary>Identifies the <see cref="ItemInvoked"/> routed event.</summary>
    public static readonly RoutedEvent ItemInvokedEvent = EventManager.RegisterRoutedEvent(
        nameof(ItemInvoked),
        RoutingStrategy.Bubble,
        typeof(TypedEventHandler<NavigationView, RoutedEventArgs>),
        typeof(NavigationView)
    );

    /// <summary>Identifies the <see cref="BackRequested"/> routed event.</summary>
    public static readonly RoutedEvent BackRequestedEvent = EventManager.RegisterRoutedEvent(
        nameof(BackRequested),
        RoutingStrategy.Bubble,
        typeof(TypedEventHandler<NavigationView, RoutedEventArgs>),
        typeof(NavigationView)
    );

    /// <summary>Identifies the <see cref="Navigating"/> routed event.</summary>
    public static readonly RoutedEvent NavigatingEvent = EventManager.RegisterRoutedEvent(
        nameof(Navigating),
        RoutingStrategy.Bubble,
        typeof(TypedEventHandler<NavigationView, NavigatingCancelEventArgs>),
        typeof(NavigationView)
    );

    /// <summary>Identifies the <see cref="Navigated"/> routed event.</summary>
    public static readonly RoutedEvent NavigatedEvent = EventManager.RegisterRoutedEvent(
        nameof(Navigated),
        RoutingStrategy.Bubble,
        typeof(TypedEventHandler<NavigationView, NavigatedEventArgs>),
        typeof(NavigationView)
    );

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

    /// <inheritdoc/>
    public event TypedEventHandler<NavigationView, NavigatedEventArgs> Navigated
    {
        add => AddHandler(NavigatedEvent, value);
        remove => RemoveHandler(NavigatedEvent, value);
    }

    /// <summary>
    /// Raises the pane opened event.
    /// </summary>
    protected virtual void OnPaneOpened()
    {
        RaiseEvent(new RoutedEventArgs(PaneOpenedEvent, this));
    }

    /// <summary>
    /// Raises the pane closed event.
    /// </summary>
    protected virtual void OnPaneClosed()
    {
        RaiseEvent(new RoutedEventArgs(PaneClosedEvent, this));
    }

    /// <summary>
    /// Raises the selection changed event.
    /// </summary>
    protected virtual void OnSelectionChanged()
    {
        RaiseEvent(new RoutedEventArgs(SelectionChangedEvent, this));
    }

    /// <summary>
    /// Raises the item invoked event.
    /// </summary>
    protected virtual void OnItemInvoked()
    {
        RaiseEvent(new RoutedEventArgs(ItemInvokedEvent, this));
    }

    /// <summary>
    /// Raises the back requested event.
    /// </summary>
    protected virtual void OnBackRequested()
    {
        RaiseEvent(new RoutedEventArgs(BackRequestedEvent));
    }

    /// <summary>
    /// Raises the navigating requested event.
    /// </summary>
    protected virtual bool OnNavigating(object sourcePage)
    {
        var eventArgs = new NavigatingCancelEventArgs(NavigatingEvent, this) { Page = sourcePage };

        RaiseEvent(eventArgs);

        return eventArgs.Cancel;
    }

    /// <summary>
    /// Raises the navigated requested event.
    /// </summary>
    protected virtual void OnNavigated(object page)
    {
        var eventArgs = new NavigatedEventArgs(NavigatedEvent, this) { Page = page };

        RaiseEvent(eventArgs);
    }
}
