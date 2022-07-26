// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using Wpf.Ui.Controls.Interfaces;

namespace Wpf.Ui.Common;

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
