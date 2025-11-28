// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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
            // If behavior already exists, update add button visibility
            if (Behaviors.TryGetValue(tabControl, out TabControlBehavior? behavior))
            {
                // Always try to setup/update the add button
                behavior.SetupAddButton();
                // Also update visibility immediately if button is already set up
                behavior.UpdateAddButtonVisibility();
            }
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
        if (Behaviors.ContainsKey(tabControl))
        {
            return;
        }

        TabControlBehavior behavior = new TabControlBehavior(tabControl);
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

    internal static void OnTabCloseRequested(TabControl tabControl, TabItem tabItem)
    {
        TabClosingEventArgs args = new TabClosingEventArgs(TabClosingEvent, tabItem);
        tabControl.RaiseEvent(args);

        if (!args.Cancel && GetIsClosable(tabItem))
        {
            if (tabControl.ItemsSource is IList itemsSource && !itemsSource.IsReadOnly)
            {
                // When ItemsSource is set, remove from the bound collection
                // Get the data item from ItemContainerGenerator (for ItemsSource bound to TabItem collection, this returns the TabItem itself)
                object? item = tabControl.ItemContainerGenerator.ItemFromContainer(tabItem);
                if (item == null || item == DependencyProperty.UnsetValue)
                {
                    // Fallback: try DataContext
                    item = tabItem.DataContext;
                }

                // If still null, the TabItem itself might be in the collection (for ItemsSource bound to TabItem collection)
                if (item == null)
                {
                    item = tabItem;
                }

                // Remove the item from the collection (similar to TabControlViewModel.CloseTab)
                if (item != null)
                {
                    int index = itemsSource.IndexOf(item);
                    if (index >= 0)
                    {
                        itemsSource.RemoveAt(index);
                    }
                    else
                    {
                        // Fallback: try direct remove
                        itemsSource.Remove(item);
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
                // If TabItem is already provided, use it; otherwise use the newTab we created
                object? itemToAdd = args.TabItem ?? newTab;
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
        private System.Windows.Controls.Button? _addButton;
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

        internal void SetupAddButton()
        {
            // Use Dispatcher to ensure template is fully applied
            _tabControl.Dispatcher.BeginInvoke(
                () =>
                {
                    _tabControl.ApplyTemplate();
                    if (_tabControl.Template?.FindName("AddButton", _tabControl) is System.Windows.Controls.Button addButton)
                    {
                        _addButton = addButton;
                        addButton.Click -= OnAddButtonClick;
                        addButton.Click += OnAddButtonClick;
                        
                        // Set visibility based on CanAddTabs property
                        UpdateAddButtonVisibility();
                    }
                },
                System.Windows.Threading.DispatcherPriority.Loaded);
        }

        internal void UpdateAddButtonVisibility()
        {
            if (_addButton != null)
            {
                bool canAddTabs = GetCanAddTabs(_tabControl);
                _addButton.Visibility = canAddTabs ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            }
        }

        private void OnAddButtonClick(object sender, RoutedEventArgs e)
        {
            OnTabAddRequested(_tabControl);
        }

        private void OnItemsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            // Only update if items were actually added or removed
            if (e.Action == NotifyCollectionChangedAction.Add ||
                e.Action == NotifyCollectionChangedAction.Remove ||
                e.Action == NotifyCollectionChangedAction.Replace ||
                e.Action == NotifyCollectionChangedAction.Reset)
            {
                UpdateTabItems();
            }
        }

        private void UpdateTabItems()
        {
            // When ItemsSource is set, Items collection contains TabItem containers
            // We need to get the TabItem containers from the ItemContainerGenerator
            if (_tabControl.ItemsSource != null)
            {
                // Wait for containers to be generated
                if (_tabControl.ItemContainerGenerator.Status != System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated)
                {
                    // If containers are not ready, wait for them
                    _tabControl.ItemContainerGenerator.StatusChanged += OnItemContainerGeneratorStatusChanged;
                    return;
                }

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

        private void OnItemContainerGeneratorStatusChanged(object? sender, EventArgs e)
        {
            if (_tabControl.ItemContainerGenerator.Status == System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated)
            {
                _tabControl.ItemContainerGenerator.StatusChanged -= OnItemContainerGeneratorStatusChanged;
                UpdateTabItems();
            }
            else if (_tabControl.ItemContainerGenerator.Status == System.Windows.Controls.Primitives.GeneratorStatus.Error)
            {
                // Unsubscribe on error to avoid memory leaks
                _tabControl.ItemContainerGenerator.StatusChanged -= OnItemContainerGeneratorStatusChanged;
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

            // Setup close button - use Loaded event and also try immediately
            tabItem.Loaded -= OnTabItemLoaded;
            tabItem.Loaded += OnTabItemLoaded;

            // Try to setup immediately if already loaded
            if (tabItem.IsLoaded)
            {
                // Use Dispatcher to ensure template is fully applied
                tabItem.Dispatcher.BeginInvoke(
                    () => SetupCloseButton(tabItem),
                    System.Windows.Threading.DispatcherPriority.Loaded);
            }
        }

        private void OnTabItemLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is TabItem tabItem)
            {
                // Use Dispatcher to ensure template is fully applied and rendered
                tabItem.Dispatcher.BeginInvoke(
                    () => SetupCloseButton(tabItem),
                    System.Windows.Threading.DispatcherPriority.Loaded);
            }
        }

        private void SetupCloseButton(TabItem tabItem)
        {
            // Ensure template is applied
            tabItem.ApplyTemplate();

            // Try to find CloseButton - use multiple attempts to ensure it's found
            System.Windows.Controls.Button? closeButton = tabItem.Template?.FindName("CloseButton", tabItem) as System.Windows.Controls.Button;

            // Fallback: Visual tree search by position (Grid.Column="1") if FindName fails
            closeButton ??= FindCloseButtonInVisualTree(tabItem);

            if (closeButton != null)
            {
                // Remove previous handlers to avoid duplicate subscriptions
                closeButton.Click -= OnCloseButtonClick;
                closeButton.PreviewMouseLeftButtonDown -= OnCloseButtonPreviewMouseLeftButtonDown;

                // Add handlers
                closeButton.Click += OnCloseButtonClick;
                closeButton.PreviewMouseLeftButtonDown += OnCloseButtonPreviewMouseLeftButtonDown;
            }
        }

        private static System.Windows.Controls.Button? FindCloseButtonInVisualTree(DependencyObject parent)
        {
            int childCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childCount; i++)
            {
                DependencyObject? child = VisualTreeHelper.GetChild(parent, i);
                if (child is System.Windows.Controls.Button button)
                {
                    // Check if it's in the second column of a Grid (CloseButton is in Grid.Column="1")
                    // This is more reliable than Name property which may not be set
                    int column = Grid.GetColumn(button);
                    if (column == 1)
                    {
                        // This is likely the CloseButton based on position
                        return button;
                    }

                    // Also check Name property as fallback
                    string? name = button.Name;
                    if (name == "CloseButton")
                    {
                        return button;
                    }
                }

                if (child != null)
                {
                    System.Windows.Controls.Button? found = FindCloseButtonInVisualTree(child);
                    if (found != null)
                    {
                        return found;
                    }
                }
            }

            return null;
        }

        private static TabItem? FindTabItemFromButton(System.Windows.Controls.Button button)
        {
            // Try to get TabItem from TemplatedParent first
            if (button.TemplatedParent is TabItem tabItem)
            {
                return tabItem;
            }

            // If TemplatedParent is not available, find the TabItem in the visual tree
            DependencyObject? current = button;
            while (current != null)
            {
                current = VisualTreeHelper.GetParent(current);
                if (current is TabItem item)
                {
                    return item;
                }
            }

            return null;
        }

        private void OnCloseButtonPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Mark the event as handled to prevent it from bubbling up to the TabItem
            e.Handled = true;

            if (sender is System.Windows.Controls.Button button)
            {
                TabItem? tabItem = FindTabItemFromButton(button);
                if (tabItem != null)
                {
                    OnTabCloseRequested(_tabControl, tabItem);
                }
            }
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            // Mark the event as handled to prevent it from bubbling up to the TabItem
            e.Handled = true;

            if (sender is System.Windows.Controls.Button button)
            {
                TabItem? tabItem = FindTabItemFromButton(button);
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
                    // Let the close button handle the event
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
                if (current is System.Windows.Controls.Button button)
                {
                    // Check by Grid column position (more reliable than Name)
                    if (Grid.GetColumn(button) == 1)
                    {
                        return true;
                    }

                    // Fallback: check by Name
                    if (button.Name == "CloseButton")
                    {
                        return true;
                    }
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
            // Validate indices
            if (oldIndex < 0 || newIndex < 0 || oldIndex == newIndex)
            {
                return;
            }

            if (_tabControl.ItemsSource is IList itemsSource && !itemsSource.IsReadOnly)
            {
                // When ItemsSource is set, operate on the bound collection
                if (oldIndex < itemsSource.Count && newIndex < itemsSource.Count)
                {
                    object? item = itemsSource[oldIndex];
                    itemsSource.RemoveAt(oldIndex);
                    itemsSource.Insert(newIndex, item);
                }
            }
            else if (_tabControl.ItemsSource == null)
            {
                // When ItemsSource is not set, operate on Items collection
                if (oldIndex < _tabControl.Items.Count && newIndex < _tabControl.Items.Count)
                {
                    object? item = _tabControl.Items[oldIndex];
                    _tabControl.Items.RemoveAt(oldIndex);
                    _tabControl.Items.Insert(newIndex, item);
                }
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
            // Clean up event handlers
            _addButton?.Click -= OnAddButtonClick;

            if (_tabControl.Items is INotifyCollectionChanged notifyCollection)
            {
                notifyCollection.CollectionChanged -= OnItemsCollectionChanged;
            }

            // Clean up dragged tab reference
            _draggedTab?.ReleaseMouseCapture();
            _draggedTab = null;
        }
    }
}

