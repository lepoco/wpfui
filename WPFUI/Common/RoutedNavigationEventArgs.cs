using System.Windows;
using WPFUI.Controls.Interfaces;

namespace WPFUI.Common;

/// <summary>
/// <see cref="RoutedEventArgs"/> with additional <see cref="CurrentPage"/>.
/// </summary>
public class RoutedNavigationEventArgs : RoutedEventArgs
{
    /// <summary>
    /// Currently displayed page.
    /// </summary>
    public INavigationItem CurrentPage { get; set; }

    /// <summary>
    /// Constructor for <see cref="RoutedEventArgs"/>.
    /// </summary>
    /// <param name="source">The new value that the SourceProperty is being set to.</param>
    /// <param name="routedEvent">The new value that the <see cref="RoutedEvent"/> Property is being set to.</param>
    /// <param name="currentPage">Currently displayed page.</param>
    public RoutedNavigationEventArgs(RoutedEvent routedEvent, object source, INavigationItem currentPage) : base(
        routedEvent, source)
    {
        CurrentPage = currentPage;
    }
}
