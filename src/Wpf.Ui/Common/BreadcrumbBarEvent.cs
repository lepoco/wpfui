using System.Windows;
using Wpf.Ui.Controls;
using System.Diagnostics.CodeAnalysis;


namespace Wpf.Ui.Common;

/// <summary>
/// Event triggered on via <see cref="BreadcrumbBar"/>.
/// </summary>
/// <param name="sender">Current <see cref="BreadcrumbBar"/> instance.</param>
public delegate void BreadcrumbBarItemClickedEvent(BreadcrumbBar sender, BreadcrumbBarItemClickedEventArgs e);

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
