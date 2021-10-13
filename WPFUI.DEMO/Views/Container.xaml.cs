// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.ObjectModel;
using System.Windows;
using WPFUI.Common;

namespace WPFUI.Demo.Views
{
    /// <summary>
    /// Interaction logic for Container.xaml
    /// </summary>
    public partial class Container : Window
    {
        private const string AssetsPath = "pack://application:,,,/Assets/";

        public Container()
        {
            InitializeComponent();
            this.InitializeNavigation();
        }

        private void InitializeNavigation()
        {
            RootNavigation.Frame = RootFrame;
            RootNavigation.Items = new ObservableCollection<NavItem>
            {
                new NavItem { ImageUri = AssetsPath + "microsoft-shell-desktop.ico", Name = "Dashboard", Tag = "dashboard", Type = typeof(Pages.Dashboard)},
                new NavItem { ImageUri = AssetsPath + "microsoft-shell-workspace.ico", Name = "Controls", Tag = "controls", Type = typeof(Pages.Controls)},
                new NavItem { ImageUri = AssetsPath + "microsoft-shell-monitor.ico", Name = "Behavior", Tag = "behavior", Type = typeof(Pages.Behavior)},
                new NavItem { ImageUri = AssetsPath + "microsoft-shell-monitor.ico", Name = "Windows", Tag = "windows", Type = typeof(Pages.WindowsPage)},
                new NavItem { ImageUri = AssetsPath + "microsoft-shell-star.ico", Name = "Icons", Tag = "icons", Type = typeof(Pages.Icons)},
                new NavItem { ImageUri = AssetsPath + "microsoft-shell-colors.ico", Name = "Colors", Tag = "colors", Type = typeof(Pages.Colors)},
                new NavItem { ImageUri = AssetsPath + "microsoft-shell-settings.ico", Name = "Settings", Tag = "settings", Type = typeof(Pages.Settings)}
            };

            //RootNavigation.Footer = new ObservableCollection<NavItem>
            //{
            //    new NavItem { Icon = MUIcon.GridView, Name = "Settings", Tag = "settings", Type = typeof(Pages.Settings)}
            //};

            //rootNavigation.OnNavigate = OnNavigate;
            RootNavigation.Navigate("dashboard");
        }
    }
}
