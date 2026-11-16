// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections;
using System.Collections.Specialized;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Displays document tabs that can be added, closed and reordered within this control.
/// Bind model collections using ItemsSource and provide new models through TabAdding.Item.
/// Handlers that perform their own collection updates must set Cancel to true.
/// </summary>
[TemplatePart(Name = "PART_AddButton", Type = typeof(System.Windows.Controls.Button))]
public class TabView : System.Windows.Controls.TabControl
{
    private readonly HashSet<TabViewItem> _containers = new();
    private System.Windows.Controls.Button? _addButton;
    private TabItem? _draggedTab;
    private Point _dragStartPoint;
    private bool _isDragging;
    private bool _isUpdating;

    /// <summary>Identifies the <see cref="CanReorderTabs"/> dependency property.</summary>
    public static readonly DependencyProperty CanReorderTabsProperty = DependencyProperty.Register(
        nameof(CanReorderTabs),
        typeof(bool),
        typeof(TabView),
        new PropertyMetadata(true, OnCanReorderTabsChanged)
    );

    /// <summary>Identifies the <see cref="CanAddTabs"/> dependency property.</summary>
    public static readonly DependencyProperty CanAddTabsProperty = DependencyProperty.Register(
        nameof(CanAddTabs),
        typeof(bool),
        typeof(TabView),
        new PropertyMetadata(true)
    );

    /// <summary>Gets or sets a value indicating whether tabs can be reordered by dragging.</summary>
    public bool CanReorderTabs
    {
        get => (bool)GetValue(CanReorderTabsProperty);
        set => SetValue(CanReorderTabsProperty, value);
    }

    /// <summary>Gets or sets a value indicating whether the add button is displayed.</summary>
    public bool CanAddTabs
    {
        get => (bool)GetValue(CanAddTabsProperty);
        set => SetValue(CanAddTabsProperty, value);
    }

    /// <summary>Occurs before the default close operation. Cancel to handle removal yourself.</summary>
    public event EventHandler<TabViewItemClosingEventArgs>? TabClosing;

    /// <summary>Occurs before adding a tab. Supply Item for a model ItemsSource.</summary>
    public event EventHandler<TabViewItemAddingEventArgs>? TabAdding;

    static TabView()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(TabView),
            new FrameworkPropertyMetadata(typeof(TabView))
        );
    }

    /// <summary>Initializes a new instance of the <see cref="TabView"/> class.</summary>
    public TabView()
    {
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;

        // Enable dropping by default; explicitly assigned AllowDrop values remain authoritative.
        SetCurrentValue(AllowDropProperty, true);
    }

    /// <inheritdoc />
    protected override DependencyObject GetContainerForItemOverride() => new TabViewItem();

    /// <inheritdoc />
    protected override bool IsItemItsOwnContainerOverride(object item) => item is TabItem;

    /// <inheritdoc />
    protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
    {
        base.PrepareContainerForItemOverride(element, item);
        if (element is TabViewItem container)
        {
            _containers.Add(container);
            AttachContainer(container);
        }
    }

    /// <inheritdoc />
    protected override void ClearContainerForItemOverride(DependencyObject element, object item)
    {
        if (element is TabViewItem container)
        {
            container.CloseRequested -= OnTabCloseRequested;
            _containers.Remove(container);
        }

        if (element == _draggedTab)
        {
            EndDrag();
        }

        base.ClearContainerForItemOverride(element, item);
    }

    /// <inheritdoc />
    protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
    {
        EndDrag();
        base.OnItemsSourceChanged(oldValue, newValue);
    }

    /// <inheritdoc />
    public override void OnApplyTemplate()
    {
        EndDrag();
        DetachAddButton();
        base.OnApplyTemplate();
        _addButton = GetTemplateChild("PART_AddButton") as System.Windows.Controls.Button;
        AttachAddButton();
    }

    private void AttachAddButton()
    {
        if (_addButton != null)
        {
            _addButton.Click -= OnAddButtonClick;
            _addButton.Click += OnAddButtonClick;
        }
    }

    private void DetachAddButton()
    {
        if (_addButton != null)
        {
            _addButton.Click -= OnAddButtonClick;
        }
    }

    private void AttachContainer(TabViewItem container)
    {
        container.CloseRequested -= OnTabCloseRequested;
        container.CloseRequested += OnTabCloseRequested;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        AttachAddButton();
        foreach (TabViewItem container in _containers)
        {
            AttachContainer(container);
        }
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        EndDrag();
        DetachAddButton();
        foreach (TabViewItem container in _containers)
        {
            container.CloseRequested -= OnTabCloseRequested;
        }
    }

    private static void OnCanReorderTabsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (!(bool)e.NewValue)
        {
            ((TabView)d).EndDrag();
        }
    }

    private void OnAddButtonClick(object sender, RoutedEventArgs e)
    {
        e.Handled = true;
        if (CanAddTabs)
        {
            AddTab();
        }
    }

    /// <summary>
    /// Requests a new tab. With ItemsSource, supply a compatible Item or TabItem in TabAdding.
    /// Unsupported collections are left unchanged. Content and header apply to TabViewItem entries.
    /// </summary>
    public void AddTab(object? content = null, string? header = null)
    {
        if (_isUpdating)
        {
            return;
        }

        _isUpdating = true;
        try
        {
            IEnumerable? source = ItemsSource;
            int count = Items.Count;
            TabViewItemAddingEventArgs args = new();
            TabAdding?.Invoke(this, args);
            if (args.Cancel || source != ItemsSource || count != Items.Count)
            {
                return;
            }

            IList? collection = GetMutableCollection();
            if (collection == null)
            {
                return;
            }

            object? item = args.Item ?? args.TabItem;
            if (item == null && source == null)
            {
                item = new TabViewItem();
            }

            if (item == null || !TabViewCollection.CanAdd(collection, item))
            {
                return;
            }

            if (item is TabViewItem tab)
            {
                if (content != null)
                {
                    tab.Content = content;
                }

                if (header != null)
                {
                    tab.Header = header;
                }
            }

            collection.Add(item);
            RefreshCollection(collection);
            SetCurrentValue(SelectedItemProperty, item);
        }
        finally
        {
            _isUpdating = false;
        }
    }

    private void OnTabCloseRequested(object sender, RoutedEventArgs e)
    {
        if (_isUpdating || sender is not TabViewItem tab || !tab.IsClosable)
        {
            return;
        }

        int index = ItemContainerGenerator.IndexFromContainer(tab);
        if (index < 0)
        {
            return;
        }

        _isUpdating = true;
        try
        {
            object item = Items[index];
            IEnumerable? source = ItemsSource;
            TabViewItemClosingEventArgs args = new(tab) { Item = item };
            TabClosing?.Invoke(this, args);
            IList? collection = GetMutableCollection();
            if (
                args.Cancel
                || !tab.IsClosable
                || source != ItemsSource
                || collection == null
                || ItemContainerGenerator.IndexFromContainer(tab) != index
                || index >= Items.Count
                || !IsSameItem(Items[index], item)
            )
            {
                return;
            }

            bool wasSelected = IsSameItem(SelectedItem, item);
            collection.RemoveAt(index);
            RefreshCollection(collection);
            if (wasSelected)
            {
                SetCurrentValue(
                    SelectedItemProperty,
                    Items.Count == 0 ? null : Items[Math.Min(index, Items.Count - 1)]
                );
            }
        }
        finally
        {
            _isUpdating = false;
        }
    }

    // ItemCollection exposes the view order. Never use that order to mutate a sorted/filtered source.
    private IList? GetMutableCollection()
    {
        if (Items.SortDescriptions.Count > 0 || Items.Filter != null || Items.GroupDescriptions?.Count > 0)
        {
            return null;
        }

        object? source = ItemsSource;
        if (source is ICollectionView view)
        {
            if (view.SortDescriptions.Count > 0 || view.Filter != null || view.GroupDescriptions?.Count > 0)
            {
                return null;
            }

            source = view.SourceCollection;
        }

        IList? collection = source == null ? Items : source as IList;
        if (
            collection == null
            || collection.IsReadOnly
            || collection.IsFixedSize
            || collection.Count != Items.Count
        )
        {
            return null;
        }

        for (int i = 0; i < collection.Count; i++)
        {
            if (!IsSameItem(collection[i], Items[i]))
            {
                return null;
            }
        }

        return collection;
    }

    private void RefreshCollection(IList collection)
    {
        if (ItemsSource != null && collection is not INotifyCollectionChanged)
        {
            Items.Refresh();
        }
    }

    private void ReorderTabItem(int oldIndex, int newIndex)
    {
        IList? collection = GetMutableCollection();
        if (
            _isUpdating
            || !CanReorderTabs
            || collection == null
            || oldIndex < 0
            || newIndex < 0
            || oldIndex >= collection.Count
            || newIndex >= collection.Count
            || oldIndex == newIndex
        )
        {
            return;
        }

        _isUpdating = true;
        object? selected = SelectedItem;
        try
        {
            TabViewCollection.Move(collection, oldIndex, newIndex);
            RefreshCollection(collection);
        }
        finally
        {
            try
            {
                SetCurrentValue(SelectedItemProperty, selected);
            }
            finally
            {
                _isUpdating = false;
            }
        }
    }

    /// <inheritdoc />
    protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnPreviewMouseLeftButtonDown(e);
        EndDrag();
        if (
            !CanReorderTabs
            || GetMutableCollection() == null
            || e.OriginalSource is not DependencyObject source
            || ItemsControl.ContainerFromElement(this, source) is not TabItem tab
        )
        {
            return;
        }

        // Buttons and editable header content keep their normal input behavior.
        for (
            DependencyObject? current = source;
            current != null && current != tab;
            current = current is Visual or System.Windows.Media.Media3D.Visual3D
                    ? VisualTreeHelper.GetParent(current)
                    : LogicalTreeHelper.GetParent(current)
        )
        {
            if (current is ButtonBase or TextBoxBase or Thumb)
            {
                return;
            }
        }

        _draggedTab = tab;
        _dragStartPoint = e.GetPosition(this);
    }

    /// <inheritdoc />
    protected override void OnPreviewMouseMove(MouseEventArgs e)
    {
        base.OnPreviewMouseMove(e);
        if (_isDragging || _draggedTab == null)
        {
            return;
        }

        if (!CanReorderTabs || e.LeftButton != MouseButtonState.Pressed)
        {
            EndDrag();
            return;
        }

        Point point = e.GetPosition(this);
        if (
            Math.Abs(point.X - _dragStartPoint.X) < SystemParameters.MinimumHorizontalDragDistance
            && Math.Abs(point.Y - _dragStartPoint.Y) < SystemParameters.MinimumVerticalDragDistance
        )
        {
            return;
        }

        int index = ItemContainerGenerator.IndexFromContainer(_draggedTab);
        if (index < 0)
        {
            EndDrag();
            return;
        }

        _isDragging = true;
        try
        {
            DragDrop.DoDragDrop(_draggedTab, new TabDragData(this, Items[index]), DragDropEffects.Move);
        }
        finally
        {
            EndDrag();
        }
    }

    /// <inheritdoc />
    protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
    {
        base.OnPreviewMouseLeftButtonUp(e);
        if (!_isDragging)
        {
            EndDrag();
        }
    }

    /// <inheritdoc />
    protected override void OnDragOver(DragEventArgs e)
    {
        base.OnDragOver(e);
        if (e.Data.GetData(typeof(TabDragData)) is TabDragData data && data.Owner == this)
        {
            e.Effects =
                _isDragging && CanReorderTabs && GetMutableCollection() != null && GetDropTarget(e) != null
                    ? DragDropEffects.Move
                    : DragDropEffects.None;
            e.Handled = true;
        }
    }

    /// <inheritdoc />
    protected override void OnDrop(DragEventArgs e)
    {
        base.OnDrop(e);
        if (e.Data.GetData(typeof(TabDragData)) is not TabDragData data || data.Owner != this)
        {
            return;
        }

        e.Handled = true;
        try
        {
            if (!_isDragging)
            {
                e.Effects = DragDropEffects.None;
                return;
            }

            TabItem? target = GetDropTarget(e);
            if (target != null)
            {
                ReorderTabItem(GetItemIndex(data.Item), ItemContainerGenerator.IndexFromContainer(target));
            }
        }
        finally
        {
            EndDrag();
        }
    }

    private TabItem? GetDropTarget(DragEventArgs e)
    {
        if (
            e.OriginalSource is DependencyObject source
            && ItemsControl.ContainerFromElement(this, source) is TabItem tab
        )
        {
            return tab;
        }

        DependencyObject? hit = InputHitTest(e.GetPosition(this)) as DependencyObject;
        return hit == null ? null : ItemsControl.ContainerFromElement(this, hit) as TabItem;
    }

    private void EndDrag()
    {
        _draggedTab = null;
        _isDragging = false;
    }

    private static bool IsSameItem(object? left, object? right) =>
        ReferenceEquals(left, right) || (left is ValueType && Equals(left, right));

    private int GetItemIndex(object item)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (IsSameItem(Items[i], item))
            {
                return i;
            }
        }

        return -1;
    }

    private sealed class TabDragData(TabView owner, object item)
    {
        public TabView Owner { get; } = owner;

        public object Item { get; } = item;
    }
}
