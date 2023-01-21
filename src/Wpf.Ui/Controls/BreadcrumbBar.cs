using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls.Primitives;
using Wpf.Ui.Common;

namespace Wpf.Ui.Controls;

[StyleTypedProperty(Property = nameof(ItemContainerStyle), StyleTargetType = typeof(BreadcrumbBarItem))]
public class BreadcrumbBar : System.Windows.Controls.ItemsControl
{
    /// <summary>
    /// Property for <see cref="TemplateButtonCommand"/>.
    /// </summary>
    public static readonly DependencyProperty TemplateButtonCommandProperty =
        DependencyProperty.Register(nameof(TemplateButtonCommand), typeof(IRelayCommand), typeof(InfoBar),
            new PropertyMetadata(null));

    /// <summary>
    /// Gets the <see cref="RelayCommand{T}"/> triggered after clicking
    /// </summary>
    public IRelayCommand TemplateButtonCommand => (IRelayCommand)GetValue(TemplateButtonCommandProperty);

    /// <summary>
    /// Property for <see cref="ItemClicked"/>.
    /// </summary>
    public static readonly RoutedEvent ItemClickedRoutedEvent = EventManager.RegisterRoutedEvent(nameof(ItemClicked),
        RoutingStrategy.Bubble, typeof(BreadcrumbBarItemClickedEvent), typeof(BreadcrumbBar));

    /// <summary>
    /// Occurs when an item is clicked in the <see cref="BreadcrumbBar"/>.
    /// </summary>
    public event BreadcrumbBarItemClickedEvent ItemClicked
    {
        add => AddHandler(ItemClickedRoutedEvent, value);
        remove => RemoveHandler(ItemClickedRoutedEvent, value);
    }

    public BreadcrumbBar()
    {
        SetValue(TemplateButtonCommandProperty, new RelayCommand<object>(OnTemplateButtonClick));

        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    protected override bool IsItemItsOwnContainerOverride(object item)
    {
        return item is BreadcrumbBarItem;
    }

    protected override DependencyObject GetContainerForItemOverride()
    {
        return new BreadcrumbBarItem();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        ItemContainerGenerator.ItemsChanged += ItemContainerGeneratorOnItemsChanged;
        ItemContainerGenerator.StatusChanged += ItemContainerGeneratorOnStatusChanged;

        UpdateLastContainer();
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        Loaded -= OnLoaded;
        Unloaded -= OnUnloaded;

        ItemContainerGenerator.ItemsChanged -= ItemContainerGeneratorOnItemsChanged;
    }

    private void ItemContainerGeneratorOnStatusChanged(object? sender, EventArgs e)
    {
        if (ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
            return;

        if (ItemContainerGenerator.Items.Count <= 1)
        {
            UpdateLastContainer();
            return;
        }

        InteractWithItemContainer(2, static item => item.IsLast = false);
        UpdateLastContainer();
    }

    private void ItemContainerGeneratorOnItemsChanged(object sender, ItemsChangedEventArgs e)
    {
        if (e.Action != NotifyCollectionChangedAction.Remove)
            return;

        UpdateLastContainer();
    }

    private void OnItemClicked(object item, int index)
    {
        var args = new BreadcrumbBarItemClickedEventArgs(ItemClickedRoutedEvent, this, item, index);
        RaiseEvent(args);
    }

    private void OnTemplateButtonClick(object? obj)
    {
        if (obj is null)
            throw new ArgumentNullException("Item content is null");

        var container = ItemContainerGenerator.ContainerFromItem(obj);
        var index = ItemContainerGenerator.IndexFromContainer(container);

        OnItemClicked(obj, index);
    }

    private void InteractWithItemContainer(int offsetFromEnd, Action<BreadcrumbBarItem> action)
    {
        if (ItemContainerGenerator.Items.Count <= 0)
            return;

        var item = ItemContainerGenerator.Items[ItemContainerGenerator.Items.Count - offsetFromEnd];
        var container = (BreadcrumbBarItem) ItemContainerGenerator.ContainerFromItem(item);

        action.Invoke(container);
    }

    private void UpdateLastContainer() => InteractWithItemContainer(1, static item => item.IsLast = true);
}
