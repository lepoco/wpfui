// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Provides data for the <see cref="TabControlExtensions.TabClosingEvent"/> event.
/// </summary>
public class TabClosingEventArgs : RoutedEventArgs
{
    /// <summary>
    /// Gets the tab item that is being closed.
    /// </summary>
    public TabItem TabItem { get; }

    /// <summary>
    /// Gets or sets a value indicating whether the close operation should be canceled.
    /// </summary>
    public bool Cancel { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TabClosingEventArgs"/> class.
    /// </summary>
    public TabClosingEventArgs(RoutedEvent routedEvent, TabItem tabItem)
        : base(routedEvent)
    {
        TabItem = tabItem;
    }
}

