// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows.Controls;

namespace WPFUI.Controls
{
    /// <summary>
    /// Represents navigation class.
    /// </summary>
    public interface INavigation
    {
        /// <summary>
        /// Gets or sets the action that will be triggered during navigation.
        /// </summary>
        public Action<INavigation, string> Navigated { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="System.Windows.Controls.Frame"/> in which the <see cref="System.Windows.Controls.Page"/> will be loaded after navigation.
        /// </summary>
        public Frame Frame { get; set; }

        /// <summary>
        /// Gets currently active <see cref="System.Windows.Controls.Page"/> tag.
        /// </summary>
        public string PageNow { get; }

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
