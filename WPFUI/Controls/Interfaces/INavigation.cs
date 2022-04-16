// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.ObjectModel;
using System.Windows.Controls;
using WPFUI.Common;

namespace WPFUI.Controls.Interfaces
{
    /// <summary>
    /// Represents navigation class.
    /// </summary>
    public interface INavigation
    {
        /// <summary>
        /// Menu item ID of the current page.
        /// <para>If set to a value less than <see langword="0"/>, no <see cref="Page"/> will be loaded during <see cref="INavigation"/> initialization.</para>
        /// </summary>
        public int SelectedPageIndex { get; set; }

        /// <summary>
        /// Menu item ID of the previous page.
        /// </summary>
        public int PreviousPageIndex { get; set; }

        /// <summary>
        /// Currently used item like <see cref="INavigationItem"/>.
        /// </summary>
        public INavigationItem Current { get; }

        /// <summary>
        /// Gets or sets the <see cref="System.Windows.Controls.Frame"/> in which the <see cref="System.Windows.Controls.Page"/> will be loaded after navigation.
        /// </summary>
        public Frame Frame { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="INavigationItem"/> that will be displayed on the menu.
        /// </summary>
        public ObservableCollection<INavigationItem> Items { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="INavigationItem"/> which will be displayed at the bottom of the navigation and will not be scrolled.
        /// </summary>
        public ObservableCollection<INavigationItem> Footer { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="RoutedNavigationEvent"/> that will be triggered during navigation.
        /// </summary>
        public event RoutedNavigationEvent Navigated;

        /// <summary>
        /// Gets or sets the <see cref="RoutedNavigationEvent"/> that will be triggered during forward navigation.
        /// </summary>
        public event RoutedNavigationEvent NavigatedForward;

        /// <summary>
        /// Gets or sets the <see cref="RoutedNavigationEvent"/> that will be triggered during backward navigation.
        /// </summary>
        public event RoutedNavigationEvent NavigatedBackward;

        /// <summary>
        /// Clears all navigation items.
        /// </summary>
        public void Flush();

        /// <summary>
        /// Clears all initialized instances of the pages.
        /// </summary>
        public void FlushPages();

        /// <summary>
        /// Loads a <see cref="System.Windows.Controls.Page"/> instance into <see cref="Frame"/> based on the tag of <see cref="INavigationItem"/>.
        /// </summary>
        /// <param name="pageIndex">ID of the page to be loaded.</param>
        /// <param name="reload"><see langword="true"/> if the page object is to be recreated.</param>
        /// <param name="dataContext">When an <see cref="System.Windows.Controls.Page"/> DataContext changes, all data-bound properties (on this element or any other element) whose Bindings use this DataContext will change to reflect the new value.</param>
        public bool Navigate(int pageIndex, bool reload = false, object dataContext = null);

        /// <summary>
        /// Loads a <see cref="System.Windows.Controls.Page"/> instance into <see cref="Frame"/> based on the tag of <see cref="INavigationItem"/>.
        /// </summary>
        /// <param name="pageName">Name of the page to be loaded.</param>
        /// <param name="reload"><see langword="true"/> if the page object is to be recreated.</param>
        /// <param name="dataContext">When an <see cref="System.Windows.Controls.Page"/> DataContext changes, all data-bound properties (on this element or any other element) whose Bindings use this DataContext will change to reflect the new value.</param>
        public bool Navigate(string pageName, bool reload = false, object dataContext = null);
    }
}
