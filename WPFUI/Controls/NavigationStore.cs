// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;

namespace WPFUI.Controls
{
    /// <summary>
    /// Modern navigation styled according to the principles of Fluent Design for Windows 11.
    /// </summary>
    public class NavigationStore : Navigation
    {
        /// <summary>
        /// Property for <see cref="NavItemCommand"/>.
        /// </summary>
        public static readonly DependencyProperty NavItemCommandProperty =
            DependencyProperty.Register(nameof(NavItemCommand),
                typeof(Common.RelayCommand), typeof(NavigationStore), new PropertyMetadata(null));

        /// <summary>
        /// Command triggered after clicking the right mouse button on the control.
        /// </summary>
        public Common.RelayCommand NavItemCommand => (Common.RelayCommand)GetValue(NavItemCommandProperty);

        /// <summary>
        /// Creates a new instance of the class and sets the default <see cref="Common.RelayCommand"/> of <see cref="NavItemCommand"/>.
        /// </summary>
        public NavigationStore() => SetValue(NavItemCommandProperty, new Common.RelayCommand(o => NavItem_OnClick(o)));

        private void NavItem_OnClick(object? parameter)
        {
            string pageTag = parameter as string;

            if (String.IsNullOrEmpty(pageTag))
                return;

            Navigate(pageTag);
        }
    }
}
