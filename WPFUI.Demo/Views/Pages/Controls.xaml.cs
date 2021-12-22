// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.ObjectModel;
using System.Windows.Controls;
using WPFUI.Controls;

namespace WPFUI.Demo.Views.Pages
{
    public enum OrderStatus
    {
        None,
        New,
        Processing,
        Shipped,
        Received
    };

    /// <summary>
    /// Interaction logic for Controls.xaml
    /// </summary>
    public partial class Controls : Page
    {
        public class Customer
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public bool IsMember { get; set; }
            public OrderStatus Status { get; set; }
        }

        public ObservableCollection<string> ListBoxItemCollection { get; set; }

        public ObservableCollection<Customer> DataGridItemCollection { get; set; }

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

            DataGridItemCollection = new ObservableCollection<Customer>()
            {
                new()
                {
                    Email = "john.doe@example.com", FirstName = "John", LastName = "Doe", IsMember = true,
                    Status = OrderStatus.Processing
                },
                new()
                {
                    Email = "chloe.clarkson@example.com", FirstName = "Chloe", LastName = "Clarkson", IsMember = true,
                    Status = OrderStatus.Processing
                },
                new()
                {
                    Email = "eric.brown@example.com", FirstName = "Eric", LastName = "Brown", IsMember = false,
                    Status = OrderStatus.New
                }
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
            MessageBox messageBox = new WPFUI.Controls.MessageBox();

            messageBox.LeftButtonName = "Hello World";
            messageBox.RightButtonName = "Just close me";

            messageBox.LeftButtonClick += MessageBox_LeftButtonClick;
            messageBox.RightButtonClick += MessageBox_RightButtonClick;

            messageBox.Show("Something weird", "May happen");
        }

        private void MessageBox_LeftButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            (sender as MessageBox)?.Close();
        }

        private void MessageBox_RightButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            (sender as MessageBox)?.Close();
        }
    }
}