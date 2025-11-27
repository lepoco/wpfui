// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Provides attached properties for enhancing TabControl with reordering, adding, and closing capabilities.
/// </summary>
public static class TabControlExtensions
{
    private static readonly Dictionary<TabControl, TabControlBehavior> Behaviors = new Dictionary<TabControl, TabControlBehavior>();

    /// <summary>Identifies the <see cref="CanReorderTabsProperty"/> attached property.</summary>
    public static readonly DependencyProperty CanReorderTabsProperty = DependencyProperty.RegisterAttached(
        "CanReorderTabs",
        typeof(bool),
        typeof(TabControlExtensions),
        new PropertyMetadata(false, OnCanReorderTabsChanged)
    );

    /// <summary>Helper for getting <see cref="CanReorderTabsProperty"/> from <paramref name="target"/>.</summary>
    /// <returns>CanReorderTabs property value.</returns>
    [AttachedPropertyBrowsableForType(typeof(TabControl))]
    public static bool GetCanReorderTabs(TabControl target) => (bool)target.GetValue(CanReorderTabsProperty);

    /// <summary>Sets the value of the <see cref="CanReorderTabsProperty"/> attached property.</summary>
    public static void SetCanReorderTabs(TabControl target, bool value) => target.SetValue(CanReorderTabsProperty, value);

    private static void OnCanReorderTabsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is TabControl tabControl)
        {
            EnsureBehavior(tabControl);
        }
    }

    /// <summary>Identifies the <see cref="CanAddTabsProperty"/> attached property.</summary>
    public static readonly DependencyProperty CanAddTabsProperty = DependencyProperty.RegisterAttached(
        "CanAddTabs",
        typeof(bool),
        typeof(TabControlExtensions),
        new PropertyMetadata(false, OnCanAddTabsChanged)
    );

    /// <summary>Helper for getting <see cref="CanAddTabsProperty"/> from <paramref name="target"/>.</summary>
    /// <returns>CanAddTabs property value.</returns>
    [AttachedPropertyBrowsableForType(typeof(TabControl))]
    public static bool GetCanAddTabs(TabControl target) => (bool)target.GetValue(CanAddTabsProperty);

    /// <summary>Sets the value of the <see cref="CanAddTabsProperty"/> attached property.</summary>
    public static void SetCanAddTabs(TabControl target, bool value) => target.SetValue(CanAddTabsProperty, value);

    private static void OnCanAddTabsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is TabControl tabControl)
        {
            EnsureBehavior(tabControl);
        }
    }

    /// <summary>Identifies the <see cref="IsClosableProperty"/> attached property.</summary>
    public static readonly DependencyProperty IsClosableProperty = DependencyProperty.RegisterAttached(
        "IsClosable",
        typeof(bool),
        typeof(TabControlExtensions),
        new PropertyMetadata(true)
    );

    /// <summary>Helper for getting <see cref="IsClosableProperty"/> from <paramref name="target"/>.</summary>
    /// <param name="target"><see cref="TabItem"/> to read <see cref="IsClosableProperty"/> from.</param>
    /// <returns>IsClosable property value.</returns>
    [AttachedPropertyBrowsableForType(typeof(TabItem))]
    public static bool GetIsClosable(TabItem target) => (bool)target.GetValue(IsClosableProperty);

    /// <summary>
    /// Sets the value of the <see cref="IsClosableProperty"/> attached property.
    /// </summary>
    /// <param name="target"><see cref="TabItem"/> to set <see cref="IsClosableProperty"/> on.</param>
    /// <param name="value">The value to set for the <see cref="IsClosableProperty"/> attached property.</param>
    public static void SetIsClosable(TabItem target, bool value) => target.SetValue(IsClosableProperty, value);

    /// <summary>
    /// Identifies the <see cref="TabClosingEvent"/> routed event.
    /// </summary>
    public static readonly RoutedEvent TabClosingEvent = EventManager.RegisterRoutedEvent(
        "TabClosing",
        RoutingStrategy.Bubble,
        typeof(TabClosingEventHandler),
        typeof(TabControlExtensions)
    );

    /// <summary>
    /// Adds a handler for the <see cref="TabClosingEvent"/> event.
    /// </summary>
    public static void AddTabClosingHandler(DependencyObject d, TabClosingEventHandler handler)
    {
        if (d is UIElement element)
        {
            element.AddHandler(TabClosingEvent, handler);
        }
    }

    /// <summary>
    /// Removes a handler for the <see cref="TabClosingEvent"/> event.
    /// </summary>
    public static void RemoveTabClosingHandler(DependencyObject d, TabClosingEventHandler handler)
    {
        if (d is UIElement element)
        {
            element.RemoveHandler(TabClosingEvent, handler);
        }
    }

    /// <summary>
    /// Identifies the <see cref="TabAddingEvent"/> routed event.
    /// </summary>
    public static readonly RoutedEvent TabAddingEvent = EventManager.RegisterRoutedEvent(
        "TabAdding",
        RoutingStrategy.Bubble,
        typeof(TabAddingEventHandler),
        typeof(TabControlExtensions)
    );

    /// <summary>
    /// Adds a handler for the <see cref="TabAddingEvent"/> event.
    /// </summary>
    public static void AddTabAddingHandler(DependencyObject d, TabAddingEventHandler handler)
    {
        if (d is UIElement element)
        {
            element.AddHandler(TabAddingEvent, handler);
        }
    }

    /// <summary>
    /// Removes a handler for the <see cref="TabAddingEvent"/> event.
    /// </summary>
    public static void RemoveTabAddingHandler(DependencyObject d, TabAddingEventHandler handler)
    {
        if (d is UIElement element)
        {
            element.RemoveHandler(TabAddingEvent, handler);
        }
    }

    private static void EnsureBehavior(TabControl tabControl)
    {
        if (!Behaviors.ContainsKey(tabControl))
        {
            var behavior = new TabControlBehavior(tabControl);
            Behaviors[tabControl] = behavior;
            tabControl.Unloaded += (s, e) =>
            {
                if (Behaviors.TryGetValue(tabControl, out TabControlBehavior? b))
                {
                    b.Dispose();
                    Behaviors.Remove(tabControl);
                }
            };
        }
    }

    internal static void OnTabCloseRequested(TabControl tabControl, TabItem tabItem)
    {
        TabClosingEventArgs args = new TabClosingEventArgs(TabClosingEvent, tabItem);
        tabControl.RaiseEvent(args);

        if (!args.Cancel && GetIsClosable(tabItem))
        {
            if (tabControl.ItemsSource is IList itemsSource && !itemsSource.IsReadOnly)
            {
                // When ItemsSource is set, remove from the bound collection
                // Find the index of the TabItem container first
                int containerIndex = -1;
                for (int i = 0; i < tabControl.Items.Count; i++)
                {
                    DependencyObject? container = tabControl.ItemContainerGenerator.ContainerFromIndex(i);
                    if (container == tabItem)
                    {
                        containerIndex = i;
                        break;
                    }
                }

                // If container index found, remove from ItemsSource by index
                if (containerIndex >= 0 && containerIndex < itemsSource.Count)
                {
                    itemsSource.RemoveAt(containerIndex);
                }
                else
                {
                    // Fallback: try to get the item from ItemContainerGenerator
                    object? item = tabControl.ItemContainerGenerator.ItemFromContainer(tabItem);
                    if (item == null || item == DependencyProperty.UnsetValue)
                    {
                        // Fallback to DataContext
                        item = tabItem.DataContext;
                    }

                    // If still null, try to find the TabItem itself in the collection
                    if (item == null)
                    {
                        // For ItemsSource bound to TabItem collection, the TabItem itself might be in the collection
                        if (itemsSource.Contains(tabItem))
                        {
                            item = tabItem;
                        }
                    }

                    if (item != null)
                    {
                        // Try direct remove first (works for reference types)
                        if (itemsSource.Contains(item))
                        {
                            itemsSource.Remove(item);
                        }
                        else
                        {
                            // Fallback: try to find by index
                            int index = itemsSource.IndexOf(item);
                            if (index >= 0)
                            {
                                itemsSource.RemoveAt(index);
                            }
                        }
                    }
                }
            }
            else if (tabControl.ItemsSource == null)
            {
                // When ItemsSource is not set, remove from Items collection
                tabControl.Items.Remove(tabItem);
            }
        }
    }

    internal static void OnTabAddRequested(TabControl tabControl)
    {
        TabAddingEventArgs args = new TabAddingEventArgs(TabAddingEvent);
        tabControl.RaiseEvent(args);

        if (!args.Cancel)
        {
            TabItem newTab = args.TabItem ?? new TabItem();
            if (args.Content != null)
            {
                newTab.Content = args.Content;
            }

            if (args.Header != null)
            {
                newTab.Header = args.Header;
            }

            if (tabControl.ItemsSource is IList itemsSource)
            {
                // When ItemsSource is set, add to the bound collection
                object? itemToAdd = args.Content ?? args.Header ?? newTab;
                itemsSource.Add(itemToAdd);
                tabControl.SetCurrentValue(System.Windows.Controls.Primitives.Selector.SelectedItemProperty, itemToAdd);
            }
            else
            {
                // When ItemsSource is not set, add to Items collection
                tabControl.Items.Add(newTab);
                tabControl.SetCurrentValue(System.Windows.Controls.Primitives.Selector.SelectedItemProperty, newTab);
            }
        }
    }

    private sealed class TabControlBehavior : IDisposable
    {
        private readonly TabControl _tabControl;
        private TabItem? _draggedTab;
        private int _draggedTabIndex = -1;
        private Button? _addButton;
        private Point _dragStartPoint;
        private bool _isDragging;

        public TabControlBehavior(TabControl tabControl)
        {
            _tabControl = tabControl;
            _tabControl.Loaded += OnLoaded;
            if (_tabControl.IsLoaded)
            {
                OnLoaded(_tabControl, new RoutedEventArgs());
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            UpdateTabItems();
            SetupAddButton();
            if (_tabControl.Items is INotifyCollectionChanged notifyCollection)
            {
                notifyCollection.CollectionChanged += OnItemsCollectionChanged;
            }
        }

        private void SetupAddButton()
        {
            _tabControl.ApplyTemplate();
            if (_tabControl.Template?.FindName("AddButton", _tabControl) is Button addButton)
            {
                _addButton = addButton;
                addButton.Click -= OnAddButtonClick;
                addButton.Click += OnAddButtonClick;
            }
        }

        private void OnAddButtonClick(object sender, RoutedEventArgs e)
        {
            OnTabAddRequested(_tabControl);
        }

        private void OnItemsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateTabItems();
        }

        private void UpdateTabItems()
        {
            // When ItemsSource is set, Items collection contains TabItem containers
            // We need to get the TabItem containers from the ItemContainerGenerator
            if (_tabControl.ItemsSource != null)
            {
                for (int i = 0; i < _tabControl.Items.Count; i++)
                {
                    if (_tabControl.ItemContainerGenerator.ContainerFromIndex(i) is TabItem tabItem)
                    {
                        SetupTabItem(tabItem);
                    }
                }
            }
            else
            {
                foreach (object? item in _tabControl.Items)
                {
                    if (item is TabItem tabItem)
                    {
                        SetupTabItem(tabItem);
                    }
                }
            }
        }

        private void SetupTabItem(TabItem tabItem)
        {
            if (GetCanReorderTabs(_tabControl))
            {
                tabItem.PreviewMouseLeftButtonDown -= OnTabItemPreviewMouseLeftButtonDown;
                tabItem.PreviewMouseMove -= OnTabItemPreviewMouseMove;
                tabItem.PreviewMouseLeftButtonUp -= OnTabItemPreviewMouseLeftButtonUp;
                tabItem.PreviewMouseLeftButtonDown += OnTabItemPreviewMouseLeftButtonDown;
                tabItem.PreviewMouseMove += OnTabItemPreviewMouseMove;
                tabItem.PreviewMouseLeftButtonUp += OnTabItemPreviewMouseLeftButtonUp;
            }

            // Setup close button
            tabItem.Loaded += OnTabItemLoaded;
            if (tabItem.IsLoaded)
            {
                OnTabItemLoaded(tabItem, new RoutedEventArgs());
            }
        }

        private void OnTabItemLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is TabItem tabItem)
            {
                tabItem.ApplyTemplate();
                if (tabItem.Template?.FindName("CloseButton", tabItem) is Button closeButton)
                {
                    // Remove previous handlers
                    closeButton.Click -= OnCloseButtonClick;
                    closeButton.PreviewMouseLeftButtonDown -= OnCloseButtonPreviewMouseLeftButtonDown;
                    
                    // Add handlers
                    closeButton.Click += OnCloseButtonClick;
                    closeButton.PreviewMouseLeftButtonDown += OnCloseButtonPreviewMouseLeftButtonDown;
                }
            }
        }

        private void OnCloseButtonPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Mark the event as handled to prevent it from bubbling up to the TabItem
            e.Handled = true;
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            // Mark the event as handled to prevent it from bubbling up to the TabItem
            e.Handled = true;
            
            if (sender is Button button)
            {
                // Try to get TabItem from TemplatedParent first
                TabItem? tabItem = button.TemplatedParent as TabItem;
                
                if (tabItem == null)
                {
                    // If TemplatedParent is not available, find the TabItem in the visual tree
                    DependencyObject? current = button;
                    while (current != null)
                    {
                        current = VisualTreeHelper.GetParent(current);
                        if (current is TabItem item)
                        {
                            tabItem = item;
                            break;
                        }
                    }
                }

                if (tabItem != null)
                {
                    OnTabCloseRequested(_tabControl, tabItem);
                }
            }
        }

        private void OnTabItemPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TabItem tabItem && GetCanReorderTabs(_tabControl))
            {
                // Check if the click is on the close button or any of its children
                if (IsCloseButton(e.OriginalSource))
                {
                    // Don't start drag operation if clicking on close button
                    e.Handled = false; // Let the close button handle the event
                    return;
                }

                _draggedTab = tabItem;
                _draggedTabIndex = _tabControl.Items.IndexOf(tabItem);
                _dragStartPoint = e.GetPosition(null);
                _isDragging = false;
                tabItem.CaptureMouse();
            }
        }

        private static bool IsCloseButton(object? source)
        {
            if (source is not DependencyObject depObj)
            {
                return false;
            }

            DependencyObject? current = depObj;
            while (current != null)
            {
                if (current is Button button && button.Name == "CloseButton")
                {
                    return true;
                }

                current = VisualTreeHelper.GetParent(current);
            }

            return false;
        }

        private void OnTabItemPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (_draggedTab != null && e.LeftButton == MouseButtonState.Pressed && GetCanReorderTabs(_tabControl))
            {
                Point currentPosition = e.GetPosition(null);

                // Check if the mouse has moved far enough to start dragging
                if (!_isDragging)
                {
                    double deltaX = currentPosition.X - _dragStartPoint.X;
                    double deltaY = currentPosition.Y - _dragStartPoint.Y;
                    double minDistance = SystemParameters.MinimumHorizontalDragDistance;

                    if (Math.Abs(deltaX) < minDistance && Math.Abs(deltaY) < minDistance)
                    {
                        // Not enough movement to start dragging
                        return;
                    }

                    _isDragging = true;
                }

                Point tabControlPosition = e.GetPosition(_tabControl);
                TabItem? tabItem = GetTabItemAtPosition(tabControlPosition);

                if (tabItem != null && tabItem != _draggedTab)
                {
                    int newIndex = _tabControl.Items.IndexOf(tabItem);
                    if (newIndex >= 0 && newIndex != _draggedTabIndex)
                    {
                        ReorderTabItem(_draggedTabIndex, newIndex);
                        _draggedTabIndex = newIndex;
                        _tabControl.SetCurrentValue(System.Windows.Controls.Primitives.Selector.SelectedItemProperty, _draggedTab);
                    }
                }
            }
        }

        private void ReorderTabItem(int oldIndex, int newIndex)
        {
            if (_tabControl.ItemsSource is IList itemsSource && !itemsSource.IsReadOnly)
            {
                // When ItemsSource is set, operate on the bound collection
                object? item = itemsSource[oldIndex];
                itemsSource.RemoveAt(oldIndex);
                itemsSource.Insert(newIndex, item);
            }
            else if (_tabControl.ItemsSource == null)
            {
                // When ItemsSource is not set, operate on Items collection
                object? item = _tabControl.Items[oldIndex];
                _tabControl.Items.RemoveAt(oldIndex);
                _tabControl.Items.Insert(newIndex, item);
            }
        }

        private void OnTabItemPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_draggedTab != null)
            {
                _draggedTab.ReleaseMouseCapture();
                _draggedTab = null;
                _draggedTabIndex = -1;
                _isDragging = false;
            }
        }

        private TabItem? GetTabItemAtPosition(Point position)
        {
            HitTestResult? hitTestResult = VisualTreeHelper.HitTest(_tabControl, position);
            if (hitTestResult?.VisualHit != null)
            {
                DependencyObject? current = hitTestResult.VisualHit;
                while (current != null)
                {
                    if (current is TabItem tabItem)
                    {
                        return tabItem;
                    }

                    current = VisualTreeHelper.GetParent(current) as Visual;
                }
            }

            return null;
        }

        public void Dispose()
        {
            if (_addButton != null)
            {
                _addButton.Click -= OnAddButtonClick;
            }

            if (_tabControl.Items is INotifyCollectionChanged notifyCollection)
            {
                notifyCollection.CollectionChanged -= OnItemsCollectionChanged;
            }
        }
    }
}

