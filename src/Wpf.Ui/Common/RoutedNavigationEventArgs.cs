// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

#nullable enable
using System.Windows;
using Wpf.Ui.Controls.Interfaces;

namespace Wpf.Ui.Common;

/// <summary>
/// <see cref="RoutedEventArgs"/>
/// </summary>
public class RoutedNavigationEventArgs : RoutedEventArgs
{
    public readonly INavigationItem? NavigatedFrom;
    public readonly INavigationItem NavigatedTo;

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="routedEvent"></param>
    /// <param name="source"></param>
    /// <param name="navigatedFrom"></param>
    /// <param name="navigatedTo"></param>
    public RoutedNavigationEventArgs(RoutedEvent routedEvent, INavigation source, INavigationItem? navigatedFrom, INavigationItem navigatedTo) : base(routedEvent, source)
    {
        NavigatedFrom = navigatedFrom;
        NavigatedTo = navigatedTo;
    }
}
