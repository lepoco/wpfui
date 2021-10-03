// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;

namespace WPFUI.Demo.Views.Pages
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void ActionCardIconsClick(object sender, System.Windows.RoutedEventArgs e)
        {
            (App.Current.MainWindow as Container).RootNavigation.Navigate("icons");
        }

        private void ActionCardColorsClick(object sender, System.Windows.RoutedEventArgs e)
        {
            (App.Current.MainWindow as Container).RootNavigation.Navigate("colors");
        }

        private void ActionCardControlsClick(object sender, System.Windows.RoutedEventArgs e)
        {
            (App.Current.MainWindow as Container).RootNavigation.Navigate("controls");
        }
    }
}
