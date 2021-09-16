// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.ObjectModel;
using System.Windows;
using WPFUI.Common;

namespace WPFUI.Demo.Views.Windows
{
    /// <summary>
    /// Interaction logic for Bubble.xaml
    /// </summary>
    public partial class Bubble : Window
    {
        public Bubble()
        {
            InitializeComponent();
            this.InitializeNavigation();
        }

        private void InitializeNavigation()
        {
            RootNavigation.Frame = RootFrame;
            RootNavigation.Items = new ObservableCollection<NavItem>
            {
                new NavItem { Icon = MiconIcon.Accounts, Name = "Dashboard", Tag = "dashboard", Type = typeof(Pages.Dashboard)},
                new NavItem { Icon = MiconIcon.Accounts, Name = "Controls", Tag = "controls", Type = typeof(Pages.Controls)},
                new NavItem { Icon = MiconIcon.Accounts, Name = "Icons", Tag = "icons", Type = typeof(Pages.Icons)},
                new NavItem { Icon = MiconIcon.Accounts, Name = "Colors", Tag = "colors", Type = typeof(Pages.Colors)}
            };

            RootNavigation.Navigate("dashboard");
        }
    }
}
