// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;

namespace WPFUI.Demo.Views.Pages
{
    /// <summary>
    /// Interaction logic for Controls.xaml
    /// </summary>
    public partial class Controls : Page
    {
        public Controls()
        {
            InitializeComponent();
        }

        private void ButtonDialog_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            (System.Windows.Application.Current.MainWindow as Views.Container).RootDialog.Show = true;
        }
    }
}
