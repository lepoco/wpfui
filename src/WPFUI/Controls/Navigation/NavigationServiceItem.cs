// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using WPFUI.Controls.Interfaces;

namespace WPFUI.Controls.Navigation;

/// <summary>
/// Represents <see cref="Interfaces.INavigationItem"/> in <see cref="NavigationService"/>.
/// </summary>
internal class NavigationServiceItem
{
    public string Tag { get; set; } = String.Empty;
    public bool Cache { get; set; } = false;
    public Type Type { get; set; } = (Type)null;
    public Uri Source { get; set; } = (Uri)null;
    public object Instance { get; set; } = null;

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
