// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Provides data for the <see cref="TabControlExtensions.TabAddingEvent"/> event.
/// </summary>
public class TabAddingEventArgs : RoutedEventArgs
{
    /// <summary>
    /// Gets or sets the tab item to be added. If null, a new TabItem will be created.
    /// </summary>
    public TabItem? TabItem { get; set; }

    /// <summary>
    /// Gets or sets the content for the new tab.
    /// </summary>
    public object? Content { get; set; }

    /// <summary>
    /// Gets or sets the header for the new tab.
    /// </summary>
    public object? Header { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the add operation should be canceled.
    /// </summary>
    public bool Cancel { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TabAddingEventArgs"/> class.
    /// </summary>
    public TabAddingEventArgs(RoutedEvent routedEvent)
        : base(routedEvent)
    {
    }
}

