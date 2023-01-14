using System.Windows;

namespace Wpf.Ui.Controls;

public sealed class BreadcrumbBarItemClickedEventArgs : RoutedEventArgs
{
    public BreadcrumbBarItemClickedEventArgs(RoutedEvent routedEvent, object source, object item) : base(routedEvent, source)
    {
        Item = item;
    }

    public object Item { get; }
}
