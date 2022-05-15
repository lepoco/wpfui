// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace WPFUI.Controls.Interfaces;

/// <summary>
/// Navigation element.
/// </summary>
public interface INavigationItem
{
    /// <summary>
    /// Represents a text page identifier that can be navigated with <see cref="INavigation.Navigate(string, bool, object)"/>.
    /// </summary>
    string PageTag { get; set; }

    /// <summary>
    /// Content is the data used to generate the child elements of this control.
    /// </summary>
    object Content { get; }

    /// <summary>
    /// Gets information whether the page has a tag and type.
    /// </summary>
    bool IsValid { get; }

    /// <summary>
    /// Gets information whether the current element is active.
    /// </summary>
    bool IsActive { get; set; }

    /// <summary>
    /// Determines whether an <see cref="Page"/> should be cached.
    /// </summary>
    bool Cache { get; set; }

    /// <summary>
    /// Instance of <see cref="System.Windows.Controls.Page"/>.
    /// </summary>
    Page Instance { get; set; }

    /// <summary>
    /// <see cref="System.Windows.Controls.Page"/> type.
    /// </summary>
    Type Page { get; set; }

    /// <summary>
    /// Add / Remove ClickEvent handler
    /// </summary>
    [Category("Behavior")]
    event RoutedEventHandler Click;

    /// <summary>
    /// Tires to set the DataContext for the selected page.
    /// </summary>
    /// <param name="dataContext">Data context to be set.</param>
    void SetContext(object dataContext);
}
