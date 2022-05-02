// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace WPFUI.Demo.Views.Pages;

public enum OrderStatus
{
    None,
    New,
    Processing,
    Shipped,
    Received
};

public class Customer
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public bool IsMember { get; set; }
    public OrderStatus Status { get; set; }
}

/// <summary>
/// Interaction logic for Data.xaml
/// </summary>
public partial class Data : Page
{
    public ObservableCollection<string> ListBoxItemCollection { get; set; }
    public ObservableCollection<Customer> DataGridItemCollection { get; set; }


    public Data()
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
                Email = "john.doe@example.com",
                FirstName = "John",
                LastName = "Doe",
                IsMember = true,
                Status = OrderStatus.Processing
            },
            new()
            {
                Email = "chloe.clarkson@example.com",
                FirstName = "Chloe",
                LastName = "Clarkson",
                IsMember = true,
                Status = OrderStatus.Processing
            },
            new()
            {
                Email = "eric.brown@example.com",
                FirstName = "Eric",
                LastName = "Brown",
                IsMember = false,
                Status = OrderStatus.New
            },
            new()
            {
                Email = "john.doe@example.com",
                FirstName = "John",
                LastName = "Doe",
                IsMember = true,
                Status = OrderStatus.Processing
            },
            new()
            {
                Email = "chloe.clarkson@example.com",
                FirstName = "Chloe",
                LastName = "Clarkson",
                IsMember = true,
                Status = OrderStatus.Shipped
            },
            new()
            {
                Email = "eric.brown@example.com",
                FirstName = "Eric",
                LastName = "Brown",
                IsMember = false,
                Status = OrderStatus.Received
            }
        };

        DataContext = this;
    }
}

