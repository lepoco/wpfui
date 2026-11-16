// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Provides data for the <see cref="TabView.TabAdding"/> event.
/// </summary>
public class TabViewItemAddingEventArgs : EventArgs
{
    /// <summary>
    /// Gets or sets the tab item to be added. With unbound Items, a new TabViewItem is
    /// created when neither this property nor Item is supplied.
    /// </summary>
    public TabViewItem? TabItem { get; set; }

    /// <summary>
    /// Gets or sets the data item to add to ItemsSource. A bound model collection requires
    /// an item of its element type; the control does not create models automatically.
    /// </summary>
    public object? Item { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the default add operation should be canceled.
    /// Set this to true when the handler updates the collection itself.
    /// </summary>
    public bool Cancel { get; set; }
}
