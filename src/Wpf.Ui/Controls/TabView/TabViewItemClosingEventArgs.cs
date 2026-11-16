// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Provides data for the <see cref="TabView.TabClosing"/> event.
/// </summary>
public class TabViewItemClosingEventArgs : EventArgs
{
    /// <summary>
    /// Gets the tab item that is being closed.
    /// </summary>
    public TabViewItem TabItem { get; }

    /// <summary>Gets the data item represented by <see cref="TabItem"/>.</summary>
    public object? Item { get; internal set; }

    /// <summary>
    /// Gets or sets a value indicating whether the default close operation should be canceled.
    /// Set this to true when the handler updates the collection itself.
    /// </summary>
    public bool Cancel { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TabViewItemClosingEventArgs"/> class.
    /// </summary>
    public TabViewItemClosingEventArgs(TabViewItem tabItem)
    {
        TabItem = tabItem;
        Item = tabItem;
    }
}
