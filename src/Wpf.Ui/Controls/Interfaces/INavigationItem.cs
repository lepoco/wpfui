// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Wpf.Ui.Controls.Interfaces;

/// <summary>
/// Navigation element.
/// </summary>
public interface INavigationItem
{
    /// <summary>
    /// Represents a text page identifier that can be navigated with <see cref="INavigation.Navigate(string, object)"/>.
    /// </summary>
    string PageTag { get; set; }

    /// <summary>
    /// Content is the data used to generate the child elements of this control.
    /// </summary>
    object Content { get; }

    /// <summary>
    /// Gets information whether the current element is active.
    /// </summary>
    bool IsActive { get; set; }

    /// <summary>
    /// Determines whether an <see cref="Page"/> should be cached.
    /// </summary>
    bool Cache { get; set; }

    /// <summary>
    /// URI of the application or content being navigated to.
    /// </summary>
    Uri PageSource { get; set; }

    /// <summary>
    /// A <see cref="Type"/> inherited from <see cref="Page"/> that defines page of the item.
    /// </summary>
    Type PageType { get; set; }

    /// <summary>
    /// Absolute path to the <see cref="Page"/> XAML template based on <see cref="PageSource"/> and <see cref="System.Windows.Markup.IUriContext.BaseUri"/>.
    /// </summary>
    Uri AbsolutePageSource { get; }

    /// <summary>
    /// Add / Remove ClickEvent handler
    /// </summary>
    [Category("Behavior")]
    event RoutedEventHandler Click;
}
