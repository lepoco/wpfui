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

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// The TabView control is a way to display a set of tabs and their respective content.
/// Tab controls are useful for displaying several pages (or documents) of content while
/// giving a user the capability to rearrange, open, or close new tabs.
/// </summary>
[TemplatePart(Name = "PART_AddButton", Type = typeof(System.Windows.Controls.Button))]
public class TabView : System.Windows.Controls.TabControl
{
    private TabViewItem? _draggedTab;
    private int _draggedTabIndex = -1;

    /// <summary>Identifies the <see cref="CanReorderTabs"/> dependency property.</summary>
    public static readonly DependencyProperty CanReorderTabsProperty = DependencyProperty.Register(
        nameof(CanReorderTabs),
        typeof(bool),
        typeof(TabView),
        new PropertyMetadata(true)
    );

    /// <summary>Identifies the <see cref="CanAddTabs"/> dependency property.</summary>
    public static readonly DependencyProperty CanAddTabsProperty = DependencyProperty.Register(
        nameof(CanAddTabs),
        typeof(bool),
        typeof(TabView),
        new PropertyMetadata(true)
    );

    /// <summary>
    /// Gets or sets a value indicating whether tabs can be reordered by dragging.
    /// </summary>
    public bool CanReorderTabs
    {
        get => (bool)GetValue(CanReorderTabsProperty);
        set => SetValue(CanReorderTabsProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether new tabs can be added.
    /// </summary>
    public bool CanAddTabs
    {
        get => (bool)GetValue(CanAddTabsProperty);
        set => SetValue(CanAddTabsProperty, value);
    }

    /// <summary>
    /// Occurs when a tab is requested to be closed.
    /// </summary>
    public event EventHandler<TabViewItemClosingEventArgs>? TabClosing;

    /// <summary>
    /// Occurs when a new tab is requested to be added.
    /// </summary>
    public event EventHandler<TabViewItemAddingEventArgs>? TabAdding;

    static TabView()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(TabView), new FrameworkPropertyMetadata(typeof(TabView)));
    }

    public TabView()
    {
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        Loaded -= OnLoaded;
        UpdateTabItems();
        if (Items is INotifyCollectionChanged notifyCollection)
        {
            notifyCollection.CollectionChanged += OnItemsCollectionChanged;
        }
    }

    private void OnItemsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        UpdateTabItems();
    }

    private void UpdateTabItems()
    {
        foreach (object? item in Items)
        {
            if (item is TabViewItem tabItem)
            {
                SetupTabItem(tabItem);
            }
            else if (item is FrameworkElement element && element.Parent is TabViewItem tabItem2)
            {
                SetupTabItem(tabItem2);
            }
        }
    }

    private void SetupTabItem(TabViewItem tabItem)
    {
        tabItem.CloseRequested -= OnTabCloseRequested;
        tabItem.CloseRequested += OnTabCloseRequested;

        if (CanReorderTabs)
        {
            tabItem.PreviewMouseLeftButtonDown -= OnTabItemPreviewMouseLeftButtonDown;
            tabItem.PreviewMouseMove -= OnTabItemPreviewMouseMove;
            tabItem.PreviewMouseLeftButtonUp -= OnTabItemPreviewMouseLeftButtonUp;
            tabItem.PreviewMouseLeftButtonDown += OnTabItemPreviewMouseLeftButtonDown;
            tabItem.PreviewMouseMove += OnTabItemPreviewMouseMove;
            tabItem.PreviewMouseLeftButtonUp += OnTabItemPreviewMouseLeftButtonUp;
        }
    }

    private void OnTabCloseRequested(object sender, RoutedEventArgs e)
    {
        if (sender is TabViewItem tabItem)
        {
            TabViewItemClosingEventArgs args = new TabViewItemClosingEventArgs(tabItem);
            TabClosing?.Invoke(this, args);

            if (!args.Cancel && tabItem.IsClosable)
            {
                Items.Remove(tabItem);
            }
        }
    }

    private void OnTabItemPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is TabViewItem tabItem && CanReorderTabs)
        {
            _draggedTab = tabItem;
            _draggedTabIndex = Items.IndexOf(tabItem);
            tabItem.CaptureMouse();
        }
    }

    private void OnTabItemPreviewMouseMove(object sender, MouseEventArgs e)
    {
        if (_draggedTab != null && e.LeftButton == MouseButtonState.Pressed && CanReorderTabs)
        {
            Point currentPosition = e.GetPosition(this);
            TabViewItem? tabItem = GetTabItemAtPosition(currentPosition);

            if (tabItem != null && tabItem != _draggedTab)
            {
                int newIndex = Items.IndexOf(tabItem);
                if (newIndex >= 0 && newIndex != _draggedTabIndex)
                {
                    Items.RemoveAt(_draggedTabIndex);
                    Items.Insert(newIndex, _draggedTab);
                    _draggedTabIndex = newIndex;
                    SetCurrentValue(SelectedItemProperty, _draggedTab);
                }
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
        }
    }

    private TabViewItem? GetTabItemAtPosition(Point position)
    {
        HitTestResult hitTestResult = VisualTreeHelper.HitTest(this, position);
        if (hitTestResult?.VisualHit != null)
        {
            DependencyObject? current = hitTestResult.VisualHit;
            while (current != null)
            {
                if (current is TabViewItem tabItem)
                {
                    return tabItem;
                }

                current = VisualTreeHelper.GetParent(current) as Visual;
            }
        }

        return null;
    }

    /// <summary>
    /// Adds a new tab to the TabView.
    /// </summary>
    public void AddTab(object? content = null, string? header = null)
    {
        TabViewItemAddingEventArgs args = new TabViewItemAddingEventArgs();
        TabAdding?.Invoke(this, args);

        if (args.Cancel)
        {
            return;
        }

        TabViewItem newTab = args.TabItem ?? new TabViewItem();
        if (content != null)
        {
            newTab.Content = content;
        }

        if (header != null)
        {
            newTab.Header = header;
        }

        Items.Add(newTab);
        SetCurrentValue(SelectedItemProperty, newTab);
        SetupTabItem(newTab);
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if (GetTemplateChild("PART_AddButton") is System.Windows.Controls.Button addButton)
        {
            addButton.Click -= OnAddButtonClick;
            addButton.Click += OnAddButtonClick;
        }
    }

    private void OnAddButtonClick(object sender, RoutedEventArgs e)
    {
        AddTab();
    }
}

