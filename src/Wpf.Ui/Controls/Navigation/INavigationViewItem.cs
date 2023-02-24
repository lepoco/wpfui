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
using Wpf.Ui.Controls.IconElements;

namespace Wpf.Ui.Controls.Navigation;

/// <summary>
/// Represents the container for an item in a NavigationView control.
/// </summary>
public interface INavigationViewItem
{
    /// <summary>
    /// Unique identifier that allows the item to be located in the navigation.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Get or sets content
    /// </summary>
    object Content { get; set; }

    /// <summary>
    /// Gets or sets the icon displayed in the MenuItem object.
    /// </summary>
    IconElement? Icon { get; set; }

    /// <summary>
    /// Gets the collection of menu items displayed in the NavigationView.
    /// </summary>
    IList MenuItems { get; set; }

    /// <summary>
    /// Gets or sets an object source used to generate the content of the NavigationView menu.
    /// </summary>
    object? MenuItemsSource { get; set; }

    /// <summary>
    /// Gets information whether the current element is active.
    /// </summary>
    bool IsActive { get; }

    /// <summary>
    /// Gets information whether the sub-<see cref="MenuItems"/> are expanded.
    /// </summary>
    bool IsExpanded { get; internal set; }

    /// <summary>
    /// A unique tag used by the parent navigation system for the purpose of searching and navigating.
    /// </summary>
    string TargetPageTag { get; set; }

    /// <summary>
    /// The type of the page to be navigated. (Should be derived from <see cref="FrameworkElement"/>).
    /// </summary>
    Type? TargetPageType { get; set; }

    /// <summary>
    /// Template Property
    /// </summary>
    ControlTemplate? Template { get; set; }

    /// <summary>
    /// Gets parent if in <see cref="MenuItems"/> collection
    /// </summary>
    INavigationViewItem? NavigationViewItemParent { get; internal set; }

    /// <summary>
    /// Add / Remove ClickEvent handler.
    /// </summary>
    [Category("Behavior")]
    event RoutedEventHandler Click;

    internal bool IsMenuElement {get; set; }

    /// <summary>
    /// Correctly activates
    /// </summary>
    void Activate(INavigationView navigationView);

    /// <summary>
    /// Correctly deactivates
    /// </summary>
    void Deactivate(INavigationView navigationView);
}

