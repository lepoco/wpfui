// Based on Windows UI Library
// Copyright(c) Microsoft Corporation.All rights reserved.

// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Wpf.Ui.Controls.Navigation;

/// <summary>
/// Represents the container for an item in a NavigationView control.
/// </summary>
public interface INavigationViewItem
{
    /// <summary>
    /// Get or sets content
    /// </summary>
    object Content { get; }

    /// <summary>
    /// Unique identifier that allows the item to be located in the navigation.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Gets or sets the icon displayed in the MenuItem object.
    /// <para>If it's a <see cref="SymbolIcon"/>, additional effects will be applied.</para>
    /// </summary>
    object? Icon { get; set; }

    /// <summary>
    /// Gets the collection of menu items displayed in the NavigationView.
    /// </summary>
    IList? MenuItems { get; set; }

    /// <summary>
    /// Gets or sets an object source used to generate the content of the NavigationView menu.
    /// </summary>
    object? MenuItemsSource { get; set; }

    /// <summary>
    /// Gets information whether the current element is active.
    /// </summary>
    bool IsActive { get; set; }

    /// <summary>
    /// Gets information whether the sub-<see cref="MenuItems"/> are expanded.
    /// </summary>
    bool IsExpanded { get; }

    /// <summary>
    /// A unique tag used by the parent navigation system for the purpose of searching and navigating.
    /// </summary>
    public string TargetPageTag { get; set; }

    /// <summary>
    /// The type of the page to be navigated. (Should be derived from <see cref="FrameworkElement"/>).
    /// </summary>
    public Type? TargetPageType { get; set; }

    /// <summary>
    /// Template Property
    /// </summary>
    public ControlTemplate? Template { get; set; }

    /// <summary>
    /// Add / Remove ClickEvent handler.
    /// </summary>
    [Category("Behavior")]
    public event RoutedEventHandler Click;
}

