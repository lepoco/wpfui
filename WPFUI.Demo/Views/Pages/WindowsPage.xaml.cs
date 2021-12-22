// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;
using WPFUI.Controls;

namespace WPFUI.Demo.Views.Pages
{
    /// <summary>
    /// Interaction logic for WindowsPage.xaml
    /// </summary>
    public partial class WindowsPage : Page, INavigable
    {
        public WindowsPage()
        {
            InitializeComponent();
        }

        private void CardStore_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            new Windows.Store().Show();
        }

        private void CardXbox_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            new Windows.Xbox().Show();
        }

        private void CardEditor_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            new Windows.Editor().Show();
        }

        private void CardBackdrop_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            new Windows.Backdrop().Show();
        }

        public void OnNavigationRequest(INavigation sender, object current)
        {
            System.Diagnostics.Debug.WriteLine("Page with window selectors loaded.");
        }
    }
}
