// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Windows.Controls;
using WPFUI.Controls.Interfaces;

namespace WPFUI.Services;

/// <summary>
/// Internal class responsible for the management of navigation.
/// </summary>
internal class NavigationService
{
    protected IEnumerable<INavigationItem> NavigationItems { get; set; }

    protected IDictionary<Type, object> Cache { get; set; }

    protected Frame Frame { get; set; }

    public NavigationService()
    {
        InitializeService();
    }

    protected virtual void InitializeService()
    {

    }
}
