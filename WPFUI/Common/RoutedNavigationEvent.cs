// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using WPFUI.Controls.Interfaces;

namespace WPFUI.Common
{
    /// <summary>
    /// Event triggered on successful navigation.
    /// </summary>
    /// <param name="navigation">Current navigation instance.</param>
    /// <param name="current">Current item.</param>
    public delegate void RoutedNavigationEvent(INavigation navigation, INavigationItem current);
}