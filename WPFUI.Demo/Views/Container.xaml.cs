// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Controls;
using WPFUI.Controls;

namespace WPFUI.Demo.Views
{
    /// <summary>
    /// Interaction logic for Container.xaml
    /// </summary>
    public partial class Container : Window
    {
        public Container()
        {
            WPFUI.Background.Manager.Apply(this);

            InitializeComponent();

            RootTitleBar.CloseActionOverride = CloseActionOverride;

            //RootTitleBar.NotifyIconMenu = new ContextMenu();
            //RootTitleBar.NotifyIconMenu.Items.Add(new MenuItem() { Header = "Test #1" });
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

        private void RootNavigation_OnNavigatedForward(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Navigated forward");
        }

        private void RootNavigation_OnNavigatedBackward(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Navigated backward");
        }
    }
}
