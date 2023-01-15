using System.Windows;

namespace Wpf.Ui.Controls;

public sealed class BreadcrumbBarItemClickedEventArgs : RoutedEventArgs
{
    public BreadcrumbBarItemClickedEventArgs(RoutedEvent routedEvent, object source, object item, int index) : base(routedEvent, source)
    {
        Item = item;
        Index = index;
    }

    /// <summary>
    /// Gets the Content property value of the BreadcrumbBarItem that is clicked.
    /// </summary>
    public object Item { get; }

    /// <summary>
    /// Gets the index of the item that was clicked.
    /// </summary>
    public int Index { get; }
}
