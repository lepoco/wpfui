using System;
using System.Windows;
using Wpf.Ui.Common;

namespace Wpf.Ui.Controls;

[StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(BreadcrumbBarItem))]
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
        RoutingStrategy.Bubble, typeof(EventHandler<BreadcrumbBarItemClickedEventArgs>), typeof(BreadcrumbBar));

    /// <summary>
    /// Occurs when an item is clicked in the <see cref="BreadcrumbBar"/>.
    /// </summary>
    public event RoutedEventHandler ItemClicked
    {
        add => AddHandler(ItemClickedRoutedEvent, value);
        remove => RemoveHandler(ItemClickedRoutedEvent, value);
    }

    public BreadcrumbBar()
    {
        SetValue(TemplateButtonCommandProperty, new RelayCommand<object>(OnTemplateButtonClick));
    }

    protected override bool IsItemItsOwnContainerOverride(object item)
    {
        return item is BreadcrumbBarItem;
    }

    protected override DependencyObject GetContainerForItemOverride()
    {
        return new BreadcrumbBarItem();
    }

    private void OnItemClicked(object item)
    {
        var args = new BreadcrumbBarItemClickedEventArgs(ItemClickedRoutedEvent, this, item);
        RaiseEvent(args);
    }

    private void OnTemplateButtonClick(object? obj)
    {
        if (obj is null)
            throw new ArgumentNullException("BreadcrumbBarItem's content is null");

        OnItemClicked(obj);
    }
}
