// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;

namespace WPFUI.Controls
{
    /// <summary>
    /// Represents navigation class.
    /// </summary>
    public interface INavigation
    {
        /// <summary>
        /// Gets or sets the <see cref="RoutedEvent"/> that will be triggered during navigation.
        /// </summary>
        public event RoutedEventHandler Navigated;

        /// <summary>
        /// Gets or sets the <see cref="System.Windows.Controls.Frame"/> in which the <see cref="System.Windows.Controls.Page"/> will be loaded after navigation.
        /// </summary>
        public Frame Frame { get; set; }

        /// <summary>
        /// Gets currently active <see cref="System.Windows.Controls.Page"/> tag as <see langword="string"/>.
        /// </summary>
        public string PageNow { get; }

        /// <summary>
        /// Currently used item like <see cref="NavigationItem"/>.
        /// </summary>
        public object Current { get; }

        /// <summary>
        /// Clears all navigation items.
        /// </summary>
        public void Flush();

        /// <summary>
        /// Loads a <see cref="System.Windows.Controls.Page"/> instance into <see cref="Frame"/> based on the <see cref="WPFUI.Common.NavItem.Tag"/>.
        /// </summary>
        /// <param name="pageName">Name of the page to be loaded.</param>
        /// <param name="reload"><see langword="true"/> if the page object is to be recreated.</param>
        public void Navigate(string pageName, bool reload = false);
    }
}
