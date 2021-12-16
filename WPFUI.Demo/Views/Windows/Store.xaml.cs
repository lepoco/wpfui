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
    public partial class Store : Window
    {
        public ObservableCollection<NavItem> NavigationItems { get; set; }

        public ObservableCollection<NavItem> NavigationFooter { get; set; }

        public Store()
        {
            if (WPFUI.Background.Mica.IsSupported() && WPFUI.Background.Mica.IsSystemThemeCompatible())
                WPFUI.Background.Mica.Apply(this);

            InitializeComponent();

            NavigationItems = new ObservableCollection<NavItem>
            {
                new() { Icon = Common.Icon.Home20, Name = "Home", Tag = "dashboard", Type = typeof(Pages.DashboardStore)},
                new() { Icon = Common.Icon.Apps28, Name = "Forms", Tag = "forms", Type = typeof(Pages.Forms)},
                new() { Icon = Common.Icon.ResizeLarge24, Name = "Controls", Tag = "controls", Type = typeof(Pages.Controls)},
                new() { Icon = Common.Icon.Games28, Name = "Actions", Tag = "actions", Type = typeof(Pages.Actions)},
                new() { Icon = Common.Icon.Color24, Name = "Colors", Tag = "colors", Type = typeof(Pages.Colors)}
            };

            NavigationFooter = new ObservableCollection<NavItem>
            {
                new() { Icon = Common.Icon.Library20, Name = "Library", Tag = "library", Type = typeof(Pages.DashboardStore)},
                new() { Icon = Common.Icon.QuestionCircle28, Name = "Help", Tag = "help", Type = typeof(Pages.DashboardStore)}
            };

            DataContext = this;
        }

        private void RootNavigation_OnLoaded(object sender, RoutedEventArgs e)
        {
            RootNavigation.Navigate("dashboard");
        }
    }
}
