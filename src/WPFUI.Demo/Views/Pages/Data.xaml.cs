// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using WPFUI.Common;

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

public class DataViewData : ViewData
{
    private IEnumerable<string> _listBoxItemCollection = new string[] { };
    public IEnumerable<string> ListBoxItemCollection
    {
        get => _listBoxItemCollection;
        set => UpdateProperty(ref _listBoxItemCollection, value, nameof(ListBoxItemCollection));
    }

    private IEnumerable<Customer> _dataGridItemCollection = new Customer[] { };
    public IEnumerable<Customer> DataGridItemCollection
    {
        get => _dataGridItemCollection;
        set => UpdateProperty(ref _dataGridItemCollection, value, nameof(DataGridItemCollection));
    }

    private IEnumerable<Brush> _brushCollection = new Brush[] { };
    public IEnumerable<Brush> BrushCollection
    {
        get => _brushCollection;
        set => UpdateProperty(ref _brushCollection, value, nameof(BrushCollection));
    }
}

/// <summary>
/// Interaction logic for Data.xaml
/// </summary>
public partial class Data
{
    internal DataViewData _data;


    public Data()
    {
        InitializeComponent();
        InitializeContent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        RootPanel.ScrollOwner = ScrollHost;
    }

    private void InitializeContent()
    {
        _data = new DataViewData();
        DataContext = _data;

        _data.ListBoxItemCollection = new List<string>()
        {
            "Somewhere over the rainbow",
            "Way up high",
            "And the dreams that you dream of",
            "Once in a lullaby, oh"
        };

        _data.DataGridItemCollection = new List<Customer>()
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

        var random = new Random();
        var brushList = new List<Brush>();

        for (int i = 0; i < 4096; i++)
        {
            brushList.Add(new SolidColorBrush
            {
                Color = Color.FromArgb(
                    (byte)200,
                    (byte)random.Next(0, 250),
                    (byte)random.Next(0, 250),
                    (byte)random.Next(0, 250))
            });
        }

        _data.BrushCollection = brushList;
    }
}

