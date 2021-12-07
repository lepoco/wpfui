// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace WPFUI.Demo.Views.Pages
{
    /// <summary>
    /// Interaction logic for Controls.xaml
    /// </summary>
    public partial class Controls : Page
    {
        public ObservableCollection<string> ListBoxItemCollection { get; set; }

        public Controls()
        {
            InitializeComponent();

            ListBoxItemCollection = new ObservableCollection<string>()
            {
                "Somewhere over the rainbow",
                "Way up high",
                "And the dreams that you dream of",
                "Once in a lullaby, oh"
            };

            DataContext = this;
        }

        private void Button_ShowDialog_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            (((Container)System.Windows.Application.Current.MainWindow)!).RootDialog.Show = true;
        }

        private void Button_ShowSnackbar_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            (((Container)System.Windows.Application.Current.MainWindow)!).RootSnackbar.Expand();
        }

        private void Button_ShowBox_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // TODO: Custom window as messagebox
            MessageBox.Show("Hello, world!", "This is a caption", MessageBoxButton.YesNo, MessageBoxImage.None);
        }
    }
}