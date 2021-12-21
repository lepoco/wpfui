// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using WPFUI.Common;
using WPFUI.Controls;

namespace WPFUI.Demo.Views
{
    /// <summary>
    /// Interaction logic for Container.xaml
    /// </summary>
    public partial class Container : Window
    {
        private const string AssetsPath = "pack://application:,,,/Assets/";

        public ObservableCollection<NavItem> NavigationItems { get; set; }

        public Container()
        {
            WPFUI.Background.Manager.Apply(this);

            InitializeComponent();

            NavigationItems = new ObservableCollection<NavItem>
            {
                new() { ImageUri = AssetsPath + "microsoft-shell-desktop.ico", Name = "Dashboard", Tag = "dashboard", Type = typeof(Pages.Dashboard)},
                new() { ImageUri = AssetsPath + "microsoft-shell-accessibility.ico", Name = "Forms", Tag = "forms", Type = typeof(Pages.Forms)},
                new() { ImageUri = AssetsPath + "microsoft-shell-settings.ico", Name = "Controls", Tag = "controls", Type = typeof(Pages.Controls)},
                new() { ImageUri = AssetsPath + "microsoft-shell-workspace.ico", Name = "Actions", Tag = "actions", Type = typeof(Pages.Actions)},
                new() { ImageUri = AssetsPath + "microsoft-shell-colors.ico", Name = "Colors", Tag = "colors", Type = typeof(Pages.Colors)},
                new() { ImageUri = AssetsPath + "microsoft-shell-gallery.ico", Name = "Icons", Tag = "icons", Type = typeof(Pages.Icons)},
                new() { ImageUri = AssetsPath + "microsoft-shell-monitor.ico", Name = "Windows", Tag = "windows", Type = typeof(Pages.WindowsPage)}
            };

            DataContext = this;

            RootTitleBar.CloseActionOverride = CloseActionOverride;
        }

        private void CloseActionOverride(TitleBar titleBar, Window window)
        {
            Application.Current.Shutdown();
        }

        private void RootNavigation_OnLoaded(object sender, RoutedEventArgs e)
        {
            RootNavigation.Navigate("dashboard");
        }

        private void RootDialog_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Root dialog action button was clicked!");
        }

        private void RootDialog_RightButtonClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Root dialog custom right button was clicked!");

            RootDialog.Show = false;
        }

        private void RootNavigation_OnNavigated(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Page now is: " + (sender as NavigationFluent)?.PageNow);
        }

        private void TitleBar_OnMinimizeClicked(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Minimize button clicked");
        }

        private void TrayMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is not MenuItem menuItem) return;

            string tag = menuItem.Tag as string ?? String.Empty;

            System.Diagnostics.Debug.WriteLine("Menu item clicked: " + tag);
        }

        private void RootTitleBar_OnNotifyIconClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Notify Icon clicked");
        }
    }
}
