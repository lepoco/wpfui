// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Demo.Models.Data;

namespace Wpf.Ui.Demo.ViewModels;

public class DataViewModel : ObservableObject, INavigationAware
{
    private bool _dataInitialized = false;

    private IEnumerable<string> _listBoxItemCollection = new string[] { };

    private IEnumerable<Customer> _dataGridItemCollection = new Customer[] { };

    private IEnumerable<Brush> _brushCollection = new Brush[] { };

    public IEnumerable<string> ListBoxItemCollection
    {
        get => _listBoxItemCollection;
        set => SetProperty(ref _listBoxItemCollection, value);
    }

    public IEnumerable<Customer> DataGridItemCollection
    {
        get => _dataGridItemCollection;
        set => SetProperty(ref _dataGridItemCollection, value);
    }

    public IEnumerable<Brush> BrushCollection
    {
        get => _brushCollection;
        set => SetProperty(ref _brushCollection, value);
    }

    public void OnNavigatedTo()
    {
        if (!_dataInitialized)
            InitializeData();
    }

    public void OnNavigatedFrom()
    {
    }

    private void InitializeData()
    {
        ListBoxItemCollection = new List<string>()
        {
            "Somewhere over the rainbow",
            "Way up high",
            "And the dreams that you dream of",
            "Once in a lullaby, oh"
        };

        DataGridItemCollection = new List<Customer>()
        {
            new()
            {
                Email = "john.doe@example.com",
                MailTo = "mailto:john.doe@example.com",
                FirstName = "John",
                LastName = "Doe",
                IsMember = true,
                Status = OrderStatus.Processing
            },
            new()
            {
                Email = "chloe.clarkson@example.com",
                MailTo = "mailto:chloe.clarkson@example.com",
                FirstName = "Chloe",
                LastName = "Clarkson",
                IsMember = true,
                Status = OrderStatus.Processing
            },
            new()
            {
                Email = "eric.brown@example.com",
                MailTo = "mailto:eric.brown@example.com",
                FirstName = "Eric",
                LastName = "Brown",
                IsMember = false,
                Status = OrderStatus.New
            },
            new()
            {
                Email = "john.doe@example.com",
                MailTo = "mailto:john.doe@example.com",
                FirstName = "John",
                LastName = "Doe",
                IsMember = true,
                Status = OrderStatus.Processing
            },
            new()
            {
                Email = "chloe.clarkson@example.com",
                MailTo = "mailto:chloe.clarkson@example.com",
                FirstName = "Chloe",
                LastName = "Clarkson",
                IsMember = true,
                Status = OrderStatus.Shipped
            },
            new()
            {
                Email = "eric.brown@example.com",
                MailTo = "mailto:eric.brown@example.com",
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

        BrushCollection = brushList;

        _dataInitialized = true;
    }
}

