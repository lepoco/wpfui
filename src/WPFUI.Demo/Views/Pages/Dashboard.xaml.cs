// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;
using WPFUI.Controls.Interfaces;

namespace WPFUI.Demo.Views.Pages
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page, INavigable
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void ActionCardIcons_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as Container)?.RootNavigation.Navigate("icons");
        }

        private void ActionCardColors_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as Container)?.RootNavigation.Navigate("colors");
        }

        private void ActionCardControls_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as Container)?.RootNavigation.Navigate("controls");
        }

        public void OnNavigationRequest(INavigation sender, object current)
        {
            System.Diagnostics.Debug.WriteLine("DEBUG | Navigated to dashboard", "WPFUI.Demo");
        }
    }
}
