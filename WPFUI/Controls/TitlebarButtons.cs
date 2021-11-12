// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;

namespace WPFUI.Controls
{
    /// <summary>
    /// Custom navigation buttons for the window.
    /// </summary>
    public class TitlebarButtons : UserControl
    {
        /// <summary>
        /// Property for <see cref="ApplicationNavigation"/>.
        /// </summary>
        public static readonly DependencyProperty ApplicationNavigationProperty =
            DependencyProperty.Register("ApplicationNavigation", typeof(bool), typeof(TitlebarButtons),
                new PropertyMetadata(false));

        /// <summary>
        /// Property for <see cref="ShowMaximize"/>.
        /// </summary>
        public static readonly DependencyProperty ShowMaximizeProperty = DependencyProperty.Register("ShowMaximize",
            typeof(bool), typeof(TitlebarButtons), new PropertyMetadata(true));

        /// <summary>
        /// Property for <see cref="ShowMinimize"/>.
        /// </summary>
        public static readonly DependencyProperty ShowMinimizeProperty = DependencyProperty.Register("ShowMinimize",
            typeof(bool), typeof(TitlebarButtons), new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets information whether the controls affect main application window.
        /// </summary>
        public bool ApplicationNavigation
        {
            get => (bool)((bool?)GetValue(ApplicationNavigationProperty));
            set => SetValue(ApplicationNavigationProperty, value);
        }

        /// <summary>
        /// Gets or sets information whether to show maximize button.
        /// </summary>
        public bool ShowMaximize
        {
            get => (bool)((bool?)GetValue(ShowMaximizeProperty));
            set => SetValue(ShowMaximizeProperty, value);
        }

        /// <summary>
        /// Gets or sets information whether to show minimize button.
        /// </summary>
        public bool ShowMinimize
        {
            get => (bool)((bool?)GetValue(ShowMinimizeProperty));
            set => SetValue(ShowMinimizeProperty, value);
        }

        private Window _parent;

        private Window ParentWindow
        {
            get
            {
                if (this._parent == null)
                    this._parent = Window.GetWindow(this);

                return this._parent;
            }
        }
    }
}