// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using Wpf.Ui.Controls.Interfaces;

namespace Wpf.Ui.Services.Internal;

/// <summary>
/// Represents <see cref="Interfaces.INavigationItem"/> in <see cref="INavigation"/> internal methods.
/// </summary>
internal class NavigationServiceItem
{
    /// <summary>
    /// Tags of the presented page.
    /// </summary>
    public string Tag { get; set; } = string.Empty;

    /// <summary>
    /// Whether the cache is active.
    /// </summary>
    public bool Cache { get; set; } = false;

    /// <summary>
    /// Type of the <see cref="Instance"/>.
    /// </summary>
    public Type Type { get; set; } = null;

    /// <summary>
    /// Source of the <see cref="Instance"/>.
    /// </summary>
    public Uri Source { get; set; } = null;

    /// <summary>
    /// Instantiated page content.
    /// </summary>
    public object Instance { get; set; } = null;

    /// <summary>
    /// Sets DataContext of the <see cref="Instance"/>.
    /// </summary>
    public bool SetContext(object dataContext)
    {
        if (!Cache)
            return false;

        if (Instance is not FrameworkElement)
            return false;

        ((FrameworkElement)Instance).DataContext = dataContext;

        return true;
    }

    /// <summary>
    /// Creates <see cref="NavigationServiceItem"/> from <see cref="INavigationItem"/>.
    /// </summary>
    public static NavigationServiceItem Create(INavigationItem navigationItem)
    {
        return new NavigationServiceItem
        {
            Tag = navigationItem.PageTag,
            Type = navigationItem.PageType,
            Source = navigationItem.AbsolutePageSource,
            Cache = navigationItem.Cache
        };
    }
}
