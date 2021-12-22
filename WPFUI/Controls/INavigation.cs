// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.ObjectModel;
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
        /// Gets or sets the list of <see cref="NavigationItem"/> that will be displayed on the menu.
        /// </summary>
        public ObservableCollection<NavigationItem> Items { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="NavigationItem"/> which will be displayed at the bottom of the navigation and will not be scrolled.
        /// </summary>
        public ObservableCollection<NavigationItem> Footer { get; set; }

        /// <summary>
        /// Clears all navigation items.
        /// </summary>
        public void Flush();

        /// <summary>
        /// Loads a <see cref="System.Windows.Controls.Page"/> instance into <see cref="Frame"/> based on the <see cref="NavigationItem.Tag"/>.
        /// </summary>
        /// <param name="pageName">Name of the page to be loaded.</param>
        /// <param name="reload"><see langword="true"/> if the page object is to be recreated.</param>
        public void Navigate(string pageName, bool reload = false);
    }
}
