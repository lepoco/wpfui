// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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

            // Update drag drop setup when CanReorderTabs changes
            if (Behaviors.TryGetValue(tabControl, out TabControlBehavior? behavior))
            {
                behavior.SetupDragDrop();
            }
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

    /// <summary>Identifies the <see cref="DragOverTabBackgroundBrushProperty"/> attached property.</summary>
    /// <remarks>
    /// Gets or sets the background brush applied to a tab when another tab is dragged over it during drag-and-drop reordering.
    /// </remarks>
    public static readonly DependencyProperty DragOverTabBackgroundBrushProperty = DependencyProperty.RegisterAttached(
        "DragOverTabBackgroundBrush",
        typeof(Brush),
        typeof(TabControlExtensions),
        new PropertyMetadata(null)
    );

    /// <summary>Helper for getting <see cref="DragOverTabBackgroundBrushProperty"/> from <paramref name="target"/>.</summary>
    /// <param name="target"><see cref="TabControl"/> to read <see cref="DragOverTabBackgroundBrushProperty"/> from.</param>
    /// <returns>DragOverTabBackgroundBrush property value.</returns>
    [AttachedPropertyBrowsableForType(typeof(TabControl))]
    public static Brush? GetDragOverTabBackgroundBrush(TabControl target) => (Brush?)target.GetValue(DragOverTabBackgroundBrushProperty);

    /// <summary>Sets the value of the <see cref="DragOverTabBackgroundBrushProperty"/> attached property.</summary>
    /// <param name="target"><see cref="TabControl"/> to set <see cref="DragOverTabBackgroundBrushProperty"/> on.</param>
    /// <param name="value">The value to set for the <see cref="DragOverTabBackgroundBrushProperty"/> attached property.</param>
    public static void SetDragOverTabBackgroundBrush(TabControl target, Brush? value) => target.SetValue(DragOverTabBackgroundBrushProperty, value);

    /// <summary>Identifies the <see cref="DraggedTabOpacityProperty"/> attached property.</summary>
    /// <remarks>
    /// Gets or sets the opacity of a tab while it is being dragged during drag-and-drop reordering.
    /// The default value is 0.5.
    /// </remarks>
    public static readonly DependencyProperty DraggedTabOpacityProperty = DependencyProperty.RegisterAttached(
        "DraggedTabOpacity",
        typeof(double),
        typeof(TabControlExtensions),
        new PropertyMetadata(0.5)
    );

    /// <summary>Helper for getting <see cref="DraggedTabOpacityProperty"/> from <paramref name="target"/>.</summary>
    /// <param name="target"><see cref="TabControl"/> to read <see cref="DraggedTabOpacityProperty"/> from.</param>
    /// <returns>DraggedTabOpacity property value.</returns>
    [AttachedPropertyBrowsableForType(typeof(TabControl))]
    public static double GetDraggedTabOpacity(TabControl target) => (double)target.GetValue(DraggedTabOpacityProperty);

    /// <summary>Sets the value of the <see cref="DraggedTabOpacityProperty"/> attached property.</summary>
    /// <param name="target"><see cref="TabControl"/> to set <see cref="DraggedTabOpacityProperty"/> on.</param>
    /// <param name="value">The value to set for the <see cref="DraggedTabOpacityProperty"/> attached property.</param>
    public static void SetDraggedTabOpacity(TabControl target, double value) => target.SetValue(DraggedTabOpacityProperty, value);

    /// <summary>Identifies the <see cref="DragOverTabIconProperty"/> attached property.</summary>
    /// <remarks>
    /// Gets or sets the icon symbol displayed on a tab when another tab is dragged over it during drag-and-drop reordering.
    /// If set to <see cref="SymbolRegular.Empty"/> or <see langword="null"/>, no icon is displayed.
    /// </remarks>
    public static readonly DependencyProperty DragOverTabIconProperty = DependencyProperty.RegisterAttached(
        "DragOverTabIcon",
        typeof(SymbolRegular?),
        typeof(TabControlExtensions),
        new PropertyMetadata(null)
    );

    /// <summary>Helper for getting <see cref="DragOverTabIconProperty"/> from <paramref name="target"/>.</summary>
    /// <param name="target"><see cref="TabControl"/> to read <see cref="DragOverTabIconProperty"/> from.</param>
    /// <returns>DragOverTabIcon property value.</returns>
    [AttachedPropertyBrowsableForType(typeof(TabControl))]
    public static SymbolRegular? GetDragOverTabIcon(TabControl target) => (SymbolRegular?)target.GetValue(DragOverTabIconProperty);

    /// <summary>Sets the value of the <see cref="DragOverTabIconProperty"/> attached property.</summary>
    /// <param name="target"><see cref="TabControl"/> to set <see cref="DragOverTabIconProperty"/> on.</param>
    /// <param name="value">The value to set for the <see cref="DragOverTabIconProperty"/> attached property.</param>
    public static void SetDragOverTabIcon(TabControl target, SymbolRegular? value) => target.SetValue(DragOverTabIconProperty, value);

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

    /// <summary>
    /// Identifies the <see cref="TabClosingProperty"/> attached property.
    /// This property allows you to set an event handler for the <see cref="TabClosingEvent"/> routed event in XAML.
    /// The event is raised when a user attempts to close a tab.
    /// </summary>
    /// <remarks>
    /// Use this attached property in XAML to handle the tab closing event:
    /// <code>
    /// &lt;TabControl uiControls:TabControlExtensions.TabClosing="OnTabClosing" /&gt;
    /// </code>
    /// </remarks>
    public static readonly DependencyProperty TabClosingProperty = DependencyProperty.RegisterAttached(
        "TabClosing",
        typeof(TabClosingEventHandler),
        typeof(TabControlExtensions),
        new PropertyMetadata(null, OnTabClosingChanged)
    );

    /// <summary>Helper for getting <see cref="TabClosingProperty"/> from <paramref name="target"/>.</summary>
    /// <param name="target"><see cref="TabControl"/> to read <see cref="TabClosingProperty"/> from.</param>
    /// <returns>TabClosing property value.</returns>
    [AttachedPropertyBrowsableForType(typeof(TabControl))]
    public static TabClosingEventHandler? GetTabClosing(TabControl target) => (TabClosingEventHandler?)target.GetValue(TabClosingProperty);

    /// <summary>Sets the value of the <see cref="TabClosingProperty"/> attached property.</summary>
    /// <param name="target"><see cref="TabControl"/> to set <see cref="TabClosingProperty"/> on.</param>
    /// <param name="value">The value to set for the <see cref="TabClosingProperty"/> attached property.</param>
    public static void SetTabClosing(TabControl target, TabClosingEventHandler? value) => target.SetValue(TabClosingProperty, value);

    /// <summary>
    /// Called when the <see cref="TabClosingProperty"/> attached property changes.
    /// Automatically registers or unregisters the event handler with the <see cref="TabClosingEvent"/> routed event.
    /// </summary>
    /// <param name="d">The dependency object on which the property changed.</param>
    /// <param name="e">The event data that contains the old and new values.</param>
    private static void OnTabClosingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is TabControl tabControl)
        {
            // Remove old handler if it exists
            if (e.OldValue is TabClosingEventHandler oldHandler)
            {
                RemoveTabClosingHandler(tabControl, oldHandler);
            }

            // Add new handler if it exists
            if (e.NewValue is TabClosingEventHandler newHandler)
            {
                AddTabClosingHandler(tabControl, newHandler);
            }
        }
    }

    /// <summary>
    /// Identifies the <see cref="TabAddingProperty"/> attached property.
    /// This property allows you to set an event handler for the <see cref="TabAddingEvent"/> routed event in XAML.
    /// The event is raised when a user clicks the add button to create a new tab.
    /// </summary>
    /// <remarks>
    /// Use this attached property in XAML to handle the tab adding event:
    /// <code>
    /// &lt;TabControl uiControls:TabControlExtensions.TabAdding="OnTabAdding" /&gt;
    /// </code>
    /// </remarks>
    public static readonly DependencyProperty TabAddingProperty = DependencyProperty.RegisterAttached(
        "TabAdding",
        typeof(TabAddingEventHandler),
        typeof(TabControlExtensions),
        new PropertyMetadata(null, OnTabAddingChanged)
    );

    /// <summary>Helper for getting <see cref="TabAddingProperty"/> from <paramref name="target"/>.</summary>
    /// <param name="target"><see cref="TabControl"/> to read <see cref="TabAddingProperty"/> from.</param>
    /// <returns>TabAdding property value.</returns>
    [AttachedPropertyBrowsableForType(typeof(TabControl))]
    public static TabAddingEventHandler? GetTabAdding(TabControl target) => (TabAddingEventHandler?)target.GetValue(TabAddingProperty);

    /// <summary>Sets the value of the <see cref="TabAddingProperty"/> attached property.</summary>
    /// <param name="target"><see cref="TabControl"/> to set <see cref="TabAddingProperty"/> on.</param>
    /// <param name="value">The value to set for the <see cref="TabAddingProperty"/> attached property.</param>
    public static void SetTabAdding(TabControl target, TabAddingEventHandler? value) => target.SetValue(TabAddingProperty, value);

    /// <summary>
    /// Called when the <see cref="TabAddingProperty"/> attached property changes.
    /// Automatically registers or unregisters the event handler with the <see cref="TabAddingEvent"/> routed event.
    /// </summary>
    /// <param name="d">The dependency object on which the property changed.</param>
    /// <param name="e">The event data that contains the old and new values.</param>
    private static void OnTabAddingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is TabControl tabControl)
        {
            // Remove old handler if it exists
            if (e.OldValue is TabAddingEventHandler oldHandler)
            {
                RemoveTabAddingHandler(tabControl, oldHandler);
            }

            // Add new handler if it exists
            if (e.NewValue is TabAddingEventHandler newHandler)
            {
                AddTabAddingHandler(tabControl, newHandler);
            }
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
        private const double PositionTolerance = 2.0; // 2 pixels tolerance to reduce flickering
        private const double DragLeaveMargin = 10.0; // 10 pixels margin for DragLeave bounds check to prevent flickering
        private readonly TabControl _tabControl;
        private TabItem? _draggedTab;
        private int _draggedTabIndex = -1;
        private System.Windows.Controls.Button? _addButton;
        private Point _dragStartPoint;
        private bool _isDragging;
        private TabItem? _hoveredTab;
        private SymbolIcon? _hoverIcon;
        private TabItem? _lastDetectedTabItem;
        private Point _lastDragPosition;

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
            SetupDragDrop();
            if (_tabControl.Items is INotifyCollectionChanged notifyCollection)
            {
                notifyCollection.CollectionChanged += OnItemsCollectionChanged;
            }
        }

        internal void SetupDragDrop()
        {
            if (GetCanReorderTabs(_tabControl))
            {
                _tabControl.SetCurrentValue(UIElement.AllowDropProperty, true);
                _tabControl.DragOver -= OnTabControlDragOver;
                _tabControl.DragLeave -= OnTabControlDragLeave;
                _tabControl.Drop -= OnTabControlDrop;
                _tabControl.DragOver += OnTabControlDragOver;
                _tabControl.DragLeave += OnTabControlDragLeave;
                _tabControl.Drop += OnTabControlDrop;
            }
            else
            {
                _tabControl.SetCurrentValue(UIElement.AllowDropProperty, false);
                _tabControl.DragOver -= OnTabControlDragOver;
                _tabControl.DragLeave -= OnTabControlDragLeave;
                _tabControl.Drop -= OnTabControlDrop;
            }
        }

        internal void SetupAddButton()
        {
            // Skip if already set up
            if (_addButton != null)
            {
                UpdateAddButtonVisibility();
                return;
            }

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
                _addButton.SetCurrentValue(UIElement.VisibilityProperty, canAddTabs ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed);
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

                    // Start the drag operation using WPF's DragDrop
                    if (_draggedTab != null)
                    {
                        // Apply visual feedback to dragged tab using custom opacity property
                        double draggedOpacity = GetDraggedTabOpacity(_tabControl);
                        _draggedTab.SetCurrentValue(UIElement.OpacityProperty, draggedOpacity);

                        DragDrop.DoDragDrop(_draggedTab, _draggedTab, DragDropEffects.Move);

                        // Restore visual feedback
                        if (_draggedTab != null)
                        {
                            _draggedTab.SetCurrentValue(UIElement.OpacityProperty, 1.0);
                        }

                        // Reset dragging state after drag operation completes
                        _isDragging = false;
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

                // Restore visual feedback if drag was cancelled
                if (_draggedTab != null)
                {
                    _draggedTab.SetCurrentValue(UIElement.OpacityProperty, 1.0);
                }

                _draggedTab = null;
                _draggedTabIndex = -1;
                _isDragging = false;
            }

            // Clear hover effect
            ClearHoverEffect();
        }

        private void OnTabControlDragOver(object sender, DragEventArgs e)
        {
            if (!GetCanReorderTabs(_tabControl))
            {
                e.Effects = DragDropEffects.None;
                return;
            }

            // Get the dragged tab from data or use cached value
            TabItem? draggedTab = _draggedTab;
            if (draggedTab == null && e.Data.GetData(typeof(TabItem)) is TabItem dataTab)
            {
                draggedTab = dataTab;
            }

            if (draggedTab == null)
            {
                e.Effects = DragDropEffects.None;
                return;
            }

            e.Effects = DragDropEffects.Move;
            e.Handled = true;

            // Find the tab item at the current position
            Point position = e.GetPosition(_tabControl);

            // Early return if we're still over the same tab item
            // This is critical to prevent flickering when moving over child elements (TextBlock, Icon, etc.)
            // We check bounds first to avoid calling GetTabItemAtPosition which can be expensive and may return null
            if (_hoveredTab != null && _hoveredTab != draggedTab)
            {
                // Always check if we're still within the hovered tab's bounds first
                // This is more reliable than checking distance or calling GetTabItemAtPosition
                // especially when GetTabItemAtPosition returns null for child elements
                // Use a margin to make the bounds check more lenient for child elements
                TabPanel? tabPanel = FindTabPanel(_tabControl);
                if (tabPanel != null)
                {
                    try
                    {
                        GeneralTransform? transform = _tabControl.TransformToDescendant(tabPanel);
                        if (transform != null)
                        {
                            Point panelPosition = transform.Transform(position);
                            GeneralTransform? itemTransform = _hoveredTab.TransformToAncestor(tabPanel);
                            if (itemTransform != null)
                            {
                                Point tabItemPosition = itemTransform.Transform(new Point(0, 0));

                                // Expand bounds by margin to make the check more lenient
                                // This prevents flickering when hovering over child elements (TextBlock, Icon, etc.)
                                Rect bounds = new Rect(
                                    tabItemPosition.X - DragLeaveMargin,
                                    tabItemPosition.Y - DragLeaveMargin,
                                    _hoveredTab.ActualWidth + (DragLeaveMargin * 2),
                                    _hoveredTab.ActualHeight + (DragLeaveMargin * 2));

                                // If we're still within the expanded hovered tab's bounds, always return early
                                // This prevents GetTabItemAtPosition from being called, which may return null
                                // even when we're still over the same tab (e.g., hovering over child elements)
                                if (bounds.Contains(panelPosition))
                                {
                                    // Calculate distance only if needed for position tolerance check
                                    if (!double.IsNaN(_lastDragPosition.X) && !double.IsNaN(_lastDragPosition.Y))
                                    {
                                        double deltaX = position.X - _lastDragPosition.X;
                                        double deltaY = position.Y - _lastDragPosition.Y;
                                        double distanceSquared = (deltaX * deltaX) + (deltaY * deltaY);

                                        // Use squared distance comparison to avoid expensive sqrt calculation
                                        if (distanceSquared < (PositionTolerance * PositionTolerance))
                                        {
                                            // Position hasn't changed much, return early
                                            _lastDragPosition = position;
                                            return;
                                        }
                                    }

                                    // _lastDragPosition is NaN or distance >= PositionTolerance, but still within bounds
                                    _lastDragPosition = position;
                                    return;
                                }
                            }
                        }
                    }
                    catch
                    {
                        // Transform failed, fall through to normal processing
                    }
                }
            }

            // Get the tab item at the current position
            TabItem? tabItem = GetTabItemAtPosition(position);

            // Validate that tabItem is actually a TabItem, not StackPanel or other element
            if (tabItem != null && tabItem.GetType() != typeof(TabItem))
            {
                // Try to find TabItem in visual tree
                if (tabItem is DependencyObject depObj)
                {
                    TabItem? tabItemFromTree = FindTabItemInVisualTree(depObj);
                    if (tabItemFromTree != null)
                    {
                        tabItem = tabItemFromTree;
                    }
                    else
                    {
                        tabItem = null;
                    }
                }
                else
                {
                    tabItem = null;
                }
            }

            // Early return if we're still over the same tab item
            // This must be checked BEFORE updating _lastDetectedTabItem to prevent flickering
            // This is critical to prevent clearing/re-applying when moving over child elements
            if (tabItem == _hoveredTab && tabItem != null && tabItem != draggedTab)
            {
                _lastDragPosition = position;
                _lastDetectedTabItem = tabItem;
                return;
            }

            // Update position tracking BEFORE processing tabItem changes
            // This ensures _lastDragPosition is always current for the next DragOver event
            _lastDragPosition = position;
            _lastDetectedTabItem = tabItem;

            // Update hover effect only if the tab item changed
            // This prevents flickering when moving over child elements (text, icons, etc.)
            if (tabItem != null && tabItem != draggedTab)
            {
                // Only update if we're hovering over a different tab
                // This check prevents re-applying when moving over child elements of the same tab
                if (tabItem != _hoveredTab)
                {
                    // Clear previous hover effect
                    ClearHoverEffect();

                    // Apply hover effect to the new tab (set _hoveredTab after calling ApplyHoverEffect)
                    ApplyHoverEffect(tabItem);
                    _hoveredTab = tabItem;
                }
            }
            else if (tabItem == null && _hoveredTab != null && _hoveredTab != draggedTab)
            {
                // If tabItem is null, check if we're still within the bounds of the hovered tab
                // This prevents clearing the hover effect when GetTabItemAtPosition fails to detect the tab
                // (e.g., when hovering over child elements like TextBlock, Icon, etc.)
                // Use expanded bounds with margin to be more lenient
                TabPanel? tabPanel = FindTabPanel(_tabControl);
                if (tabPanel != null)
                {
                    try
                    {
                        GeneralTransform? transform = _tabControl.TransformToDescendant(tabPanel);
                        if (transform != null)
                        {
                            Point panelPosition = transform.Transform(position);

                            // Check if position is within the expanded hovered tab's bounds
                            GeneralTransform? itemTransform = _hoveredTab.TransformToAncestor(tabPanel);
                            if (itemTransform != null)
                            {
                                Point tabItemPosition = itemTransform.Transform(new Point(0, 0));

                                // Expand bounds by margin to make the check more lenient
                                // This prevents flickering when hovering over child elements
                                Rect bounds = new Rect(
                                    tabItemPosition.X - DragLeaveMargin,
                                    tabItemPosition.Y - DragLeaveMargin,
                                    _hoveredTab.ActualWidth + (DragLeaveMargin * 2),
                                    _hoveredTab.ActualHeight + (DragLeaveMargin * 2));

                                // If still within expanded bounds, don't clear the hover effect
                                // This is critical to prevent flickering when GetTabItemAtPosition returns null
                                // but we're still over the same tab (e.g., hovering over child elements)
                                if (bounds.Contains(panelPosition))
                                {
                                    // Still within the tab bounds, keep the hover effect
                                    // Don't update _lastDragPosition here as it's already updated above
                                    return;
                                }
                            }
                        }
                    }
                    catch
                    {
                        // Transform failed, fall through to clear hover effect
                    }
                }

                // Clear hover effect only if we're truly outside the hovered tab bounds (even with margin)
                ClearHoverEffect();
            }
            else if (tabItem == null)
            {
                // No hovered tab and no tab item found, clear any lingering hover effect
                if (_hoveredTab != null)
                {
                    ClearHoverEffect();
                }
            }
        }

        private void OnTabControlDragLeave(object sender, DragEventArgs e)
        {
            Point position = e.GetPosition(_tabControl);

            // Check if we're actually outside the hovered tab's bounds
            // DragLeave can fire even when hovering over child elements (TextBlock, Icon, etc.)
            // if the mouse briefly leaves the tab's visual bounds
            // We use a margin to make the bounds check more lenient and prevent flickering
            if (_hoveredTab != null && _hoveredTab != _draggedTab)
            {
                TabPanel? tabPanel = FindTabPanel(_tabControl);
                if (tabPanel != null)
                {
                    try
                    {
                        GeneralTransform? transform = _tabControl.TransformToDescendant(tabPanel);
                        if (transform != null)
                        {
                            Point panelPosition = transform.Transform(position);
                            GeneralTransform? itemTransform = _hoveredTab.TransformToAncestor(tabPanel);
                            if (itemTransform != null)
                            {
                                Point tabItemPosition = itemTransform.Transform(new Point(0, 0));

                                // Expand bounds by margin to make the check more lenient
                                // This prevents flickering when the mouse is near the edge of the tab
                                Rect bounds = new Rect(
                                    tabItemPosition.X - DragLeaveMargin,
                                    tabItemPosition.Y - DragLeaveMargin,
                                    _hoveredTab.ActualWidth + (DragLeaveMargin * 2),
                                    _hoveredTab.ActualHeight + (DragLeaveMargin * 2));

                                // If still within expanded bounds, ignore this DragLeave event
                                // This prevents flickering when moving over child elements or near tab edges
                                if (bounds.Contains(panelPosition))
                                {
                                    return;
                                }
                            }
                        }
                    }
                    catch
                    {
                        // Transform failed, fall through to clear hover effect
                    }
                }
            }

            // Actually outside the tab bounds (even with margin), clear the hover effect
            ClearHoverEffect();
            _lastDetectedTabItem = null;
            _lastDragPosition = new Point(double.NaN, double.NaN);
        }

        private void OnTabControlDrop(object sender, DragEventArgs e)
        {
            if (!GetCanReorderTabs(_tabControl))
            {
                return;
            }

            // Get the dragged tab from data or use cached value
            TabItem? draggedTab = _draggedTab;
            if (draggedTab == null && e.Data.GetData(typeof(TabItem)) is TabItem dataTab)
            {
                draggedTab = dataTab;
            }

            if (draggedTab == null)
            {
                return;
            }

            e.Handled = true;

            // Clear hover effect
            ClearHoverEffect();

            // Find the target tab item
            Point position = e.GetPosition(_tabControl);
            TabItem? targetTabItem = GetTabItemAtPosition(position);

            // Get the dragged tab index
            int draggedIndex = _draggedTabIndex;
            if (draggedIndex < 0)
            {
                draggedIndex = _tabControl.Items.IndexOf(draggedTab);
            }

            if (targetTabItem != null && targetTabItem != draggedTab && draggedIndex >= 0)
            {
                int targetIndex = _tabControl.Items.IndexOf(targetTabItem);
                if (targetIndex >= 0 && targetIndex != draggedIndex)
                {
                    ReorderTabItem(draggedIndex, targetIndex);
                    _tabControl.SetCurrentValue(System.Windows.Controls.Primitives.Selector.SelectedItemProperty, draggedTab);
                }
            }

            // Clean up
            if (draggedTab != null)
            {
                draggedTab.SetCurrentValue(UIElement.OpacityProperty, 1.0);
            }

            _draggedTab = null;
            _draggedTabIndex = -1;
            _isDragging = false;
            _hoveredTab = null; // Explicitly reset hovered tab to prevent any lingering state
            _lastDetectedTabItem = null;
            _lastDragPosition = new Point(double.NaN, double.NaN);
        }

        private void ApplyHoverEffect(TabItem tabItem)
        {
            if (tabItem == null)
            {
                return;
            }

            // Skip if already applied to this tab to prevent flickering
            // This is critical to prevent re-applying when moving over child elements
            if (tabItem == _hoveredTab)
            {
                return;
            }

            // Apply hover effect using background color change
            // Try to find the Border element in the template
            tabItem.ApplyTemplate();
            Border? border = tabItem.Template?.FindName("Border", tabItem) as Border;

            if (border != null)
            {
                // Get configured hover brush once to avoid multiple calls
                Brush? configuredHoverBrush = GetDragOverTabBackgroundBrush(_tabControl);

                // Check if hover effect is already applied by checking if background is a hover brush
                // This prevents re-applying when moving over child elements of the same tab
                Brush? currentBackground = border.Background;
                if (currentBackground != null)
                {
                    // Check if the current background matches the hover brush
                    if (configuredHoverBrush != null && currentBackground == configuredHoverBrush)
                    {
                        // Hover effect is already applied, skip to prevent flickering
                        return;
                    }

                    // Check if it's the default hover color (semi-transparent blue)
                    if (currentBackground is SolidColorBrush solidBrush)
                    {
                        Color color = solidBrush.Color;

                        // Default hover color: ARGB(100, 0, 120, 215) or with opacity 0.5
                        if ((color.A == 100 && color.R == 0 && color.G == 120 && color.B == 215) ||
                            (solidBrush.Opacity >= 0.4 && color.R == 0 && color.G == 120 && color.B == 215))
                        {
                            // Hover effect is already applied, skip to prevent flickering
                            return;
                        }
                    }
                }

                // Store original background if not already stored
                string? tag = tabItem.Tag?.ToString();
                if (tag == null || !tag.StartsWith("OriginalBackgroundStored:"))
                {
                    // Store a flag that we've stored the original
                    tabItem.SetCurrentValue(FrameworkElement.TagProperty, "OriginalBackgroundStored:true");
                }

                // Apply hover background color - use custom brush if provided, otherwise use default
                Brush? hoverBrush = configuredHoverBrush;

                if (hoverBrush == null)
                {
                    // Try to get a hover color from resources
                    try
                    {
                        hoverBrush = _tabControl.TryFindResource("SystemControlHighlightListAccentLowBrush") as Brush;
                    }
                    catch
                    {
                        // Resource not found, use default
                    }

                    if (hoverBrush == null)
                    {
                        // Fallback: create a light blue overlay
                        hoverBrush = new SolidColorBrush(Color.FromArgb(100, 0, 120, 215));
                    }
                    else
                    {
                        // Clone the brush to avoid modifying the resource
                        if (hoverBrush is SolidColorBrush solidBrush)
                        {
                            hoverBrush = new SolidColorBrush(solidBrush.Color)
                            {
                                Opacity = 0.5
                            };
                        }
                    }
                }

                border.Background = hoverBrush;
            }

            // Apply icon if specified
            SymbolRegular? iconSymbol = GetDragOverTabIcon(_tabControl);
            if (iconSymbol.HasValue && iconSymbol.Value != SymbolRegular.Empty && border != null)
            {
                // Find the Grid inside the Border
                Grid? grid = null;
                int childCount = VisualTreeHelper.GetChildrenCount(border);
                for (int i = 0; i < childCount; i++)
                {
                    if (VisualTreeHelper.GetChild(border, i) is Grid g)
                    {
                        grid = g;
                        break;
                    }
                }

                if (grid != null)
                {
                    if (_hoverIcon == null)
                    {
                        // Create and add the icon
                        _hoverIcon = new SymbolIcon
                        {
                            Symbol = iconSymbol.Value,
                            FontSize = 16,
                            VerticalAlignment = VerticalAlignment.Center,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            Margin = new Thickness(4, 0, 0, 0)
                        };
                        Grid.SetColumn(_hoverIcon, 0);

                        // Find ContentPresenter using template name or by type
                        ContentPresenter? contentPresenter = tabItem.Template?.FindName("ContentSite", tabItem) as ContentPresenter;
                        if (contentPresenter == null)
                        {
                            // Fallback: find first ContentPresenter in the grid
                            for (int i = 0; i < grid.Children.Count; i++)
                            {
                                if (grid.Children[i] is ContentPresenter cp)
                                {
                                    contentPresenter = cp;
                                    break;
                                }
                            }
                        }

                        if (contentPresenter != null && contentPresenter.Parent == grid)
                        {
                            int insertIndex = grid.Children.IndexOf(contentPresenter);
                            grid.Children.Insert(insertIndex, _hoverIcon);
                        }
                        else
                        {
                            // Fallback: insert at the beginning
                            grid.Children.Insert(0, _hoverIcon);
                        }
                    }
                    else if (_hoverIcon.Symbol != iconSymbol.Value)
                    {
                        // Update the icon symbol if it changed
                        _hoverIcon.SetCurrentValue(SymbolIcon.SymbolProperty, iconSymbol.Value);
                    }
                }
            }
        }

        private void ClearHoverEffect()
        {
            if (_hoveredTab != null)
            {
                // Restore original background by clearing the local value
                if (_hoveredTab.Template?.FindName("Border", _hoveredTab) is Border border)
                {
                    // Clear the local value to restore the template binding
                    border.ClearValue(Border.BackgroundProperty);
                }

                // Remove the icon if it exists
                if (_hoverIcon != null)
                {
                    // Find the parent Grid and remove the icon
                    if (_hoverIcon.Parent is Grid grid)
                    {
                        grid.Children.Remove(_hoverIcon);
                    }

                    _hoverIcon = null;
                }

                _hoveredTab = null;
            }
        }

        private TabItem? GetTabItemAtPosition(Point position)
        {
            // Use TabPanel to directly check which TabItem contains the position
            // This is more reliable than hit testing child elements (text blocks, icons, etc.)
            TabPanel? tabPanel = FindTabPanel(_tabControl);
            if (tabPanel != null && tabPanel.Children.Count > 0)
            {
                // Get the position relative to the TabPanel
                try
                {
                    GeneralTransform? transform = _tabControl.TransformToDescendant(tabPanel);
                    if (transform != null)
                    {
                        Point panelPosition = transform.Transform(position);

                        // Check each TabItem's bounds to find which one contains the point
                        // Iterate in reverse order to check topmost tabs first (for overlapping tabs)
                        // This method is more reliable than hit testing because it checks bounds directly
                        TabItem? foundTabItem = null;

                        // Use ItemContainerGenerator to get TabItems when ItemsSource is set
                        // Otherwise, use Children directly
                        int itemCount = _tabControl.Items.Count;
                        for (int i = itemCount - 1; i >= 0; i--)
                        {
                            TabItem? tabItem = null;

                            // Try to get TabItem from ItemContainerGenerator first
                            DependencyObject? container = _tabControl.ItemContainerGenerator.ContainerFromIndex(i);

                            // Use GetType() for strict type checking, not 'is' operator
                            if (container != null && container.GetType() == typeof(TabItem))
                            {
                                tabItem = (TabItem)container;
                            }
                            else if (container != null)
                            {
                                // Container is not a TabItem (might be StackPanel or other element)
                                // Traverse up the visual tree to find the TabItem
                                tabItem = FindTabItemInVisualTree(container);
                            }

                            // Fallback: check Children directly
                            if (tabItem == null && i < tabPanel.Children.Count)
                            {
                                UIElement? child = tabPanel.Children[i];

                                // Use GetType() for strict type checking
                                if (child != null && child.GetType() == typeof(TabItem))
                                {
                                    tabItem = (TabItem)child;
                                }
                                else if (child is DependencyObject childObj)
                                {
                                    // Child is not a TabItem, traverse up to find it
                                    tabItem = FindTabItemInVisualTree(childObj);
                                }
                            }

                            if (tabItem == null)
                            {
                                continue;
                            }

                            try
                            {
                                // Skip if tab item is not visible or has zero size
                                if (tabItem.Visibility != Visibility.Visible ||
                                    tabItem.ActualWidth <= 0 ||
                                    tabItem.ActualHeight <= 0)
                                {
                                    continue;
                                }

                                // Get TabItem's position relative to TabPanel
                                GeneralTransform? itemTransform = tabItem.TransformToAncestor(tabPanel);
                                if (itemTransform != null)
                                {
                                    Point tabItemPosition = itemTransform.Transform(new Point(0, 0));
                                    Rect bounds = new Rect(tabItemPosition, new Size(tabItem.ActualWidth, tabItem.ActualHeight));

                                    // Use Contains with proper bounds checking
                                    // This ensures we always return the same TabItem even when hovering over child elements
                                    if (bounds.Contains(panelPosition))
                                    {
                                        // Ensure tabItem is actually a TabItem, not StackPanel or other element
                                        // Use GetType() for strict type checking
                                        if (tabItem.GetType() == typeof(TabItem))
                                        {
                                            foundTabItem = tabItem;

                                            // Don't break here - continue to check if there's a tab on top (for overlapping tabs)
                                            // But we'll return the topmost one
                                        }
                                        else
                                        {
                                            // Try to find TabItem in visual tree
                                            TabItem? tabItemFromTree = FindTabItemInVisualTree(tabItem);
                                            if (tabItemFromTree != null)
                                            {
                                                foundTabItem = tabItemFromTree;
                                            }
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                // Transform failed for this tab, continue to next
                            }
                        }

                        // Return the found tab item (topmost one if overlapping)
                        // Ensure we only return TabItem, not StackPanel or other elements
                        if (foundTabItem != null && foundTabItem.GetType() == typeof(TabItem))
                        {
                            return foundTabItem;
                        }
                        else if (foundTabItem != null)
                        {
                            // Try to find TabItem in visual tree from the found element
                            TabItem? tabItemFromTree = FindTabItemInVisualTree(foundTabItem);
                            if (tabItemFromTree != null)
                            {
                                return tabItemFromTree;
                            }
                        }
                    }
                }
                catch
                {
                    // Transform failed, fall back to hit test
                }
            }

            // Fallback: use hit test method, but always traverse up to find TabItem
            // This ensures we get the TabItem even when hovering over child elements
            HitTestResult? hitTestResult = VisualTreeHelper.HitTest(_tabControl, position);
            if (hitTestResult?.VisualHit != null)
            {
                DependencyObject? current = hitTestResult.VisualHit;

                // Always traverse up the visual tree to find the TabItem
                // This ensures we get the TabItem even when hovering over TextBlock, Icon, etc.
                while (current != null)
                {
                    if (current is TabItem tabItem)
                    {
                        // Verify it's part of this TabControl by checking parent chain
                        DependencyObject? parent = VisualTreeHelper.GetParent(tabItem);
                        while (parent != null)
                        {
                            if (parent == _tabControl || parent is TabPanel)
                            {
                                return tabItem;
                            }

                            parent = VisualTreeHelper.GetParent(parent);
                        }
                    }

                    // Stop if we've reached the TabControl or TabPanel
                    if (current == _tabControl || current is TabPanel)
                    {
                        break;
                    }

                    current = VisualTreeHelper.GetParent(current);
                }
            }

            return null;
        }

        private static TabItem? FindTabItemInVisualTree(DependencyObject element)
        {
            // Traverse up the visual tree to find the TabItem
            DependencyObject? current = element;
            int depth = 0;
            while (current != null && depth < 20) // Limit depth to prevent infinite loops
            {
                // Use GetType() for strict type checking
                if (current.GetType() == typeof(TabItem))
                {
                    return (TabItem)current;
                }

                // Stop if we've reached the TabPanel or TabControl
                if (current.GetType() == typeof(TabPanel) || current.GetType() == typeof(TabControl))
                {
                    break;
                }

                current = VisualTreeHelper.GetParent(current);
                depth++;
            }

            return null;
        }

        private TabPanel? FindTabPanel(TabControl tabControl)
        {
            // Try to find TabPanel by name first
            if (tabControl.Template?.FindName("HeaderPanel", tabControl) is TabPanel panel)
            {
                return panel;
            }

            // Fallback: search in visual tree
            return FindVisualChild<TabPanel>(tabControl as DependencyObject);
        }

        private static T? FindVisualChild<T>(DependencyObject? parent)
            where T : DependencyObject
        {
            if (parent == null)
            {
                return null;
            }

            int childCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childCount; i++)
            {
                DependencyObject? child = VisualTreeHelper.GetChild(parent, i);
                if (child is T result)
                {
                    return result;
                }

                T? childOfChild = FindVisualChild<T>(child);
                if (childOfChild != null)
                {
                    return childOfChild;
                }
            }

            return null;
        }

        public void Dispose()
        {
            // Clean up event handlers
            _addButton?.Click -= OnAddButtonClick;
            _tabControl.DragOver -= OnTabControlDragOver;
            _tabControl.DragLeave -= OnTabControlDragLeave;
            _tabControl.Drop -= OnTabControlDrop;

            if (_tabControl.Items is INotifyCollectionChanged notifyCollection)
            {
                notifyCollection.CollectionChanged -= OnItemsCollectionChanged;
            }

            // Clean up dragged tab reference
            _draggedTab?.ReleaseMouseCapture();
            if (_draggedTab != null)
            {
                _draggedTab.SetCurrentValue(UIElement.OpacityProperty, 1.0);
            }

            _draggedTab = null;

            // Clear hover effect
            ClearHoverEffect();
        }
    }
}

