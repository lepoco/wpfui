// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Input;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Provides data for the <see cref="TabView.TabAdding"/> event.
/// </summary>
public class TabViewItemAddingEventArgs : EventArgs
{
    /// <summary>
    /// Gets or sets the tab item to be added. If null, a new TabViewItem will be created.
    /// </summary>
    public TabViewItem? TabItem { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the add operation should be canceled.
    /// </summary>
    public bool Cancel { get; set; }
}
