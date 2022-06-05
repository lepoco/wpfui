// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Windows.Media;
using WPFUI.Common.Interfaces;
using WPFUI.Demo.Models.Data;

namespace WPFUI.Demo.ViewModels;

public class DataViewModel : WPFUI.Mvvm.ViewModelBase, INavigationAware
{
    private bool _dataInitialized = false;

    public IEnumerable<string> ListBoxItemCollection
    {
        get => GetValue<IEnumerable<string>>();
        set => SetValue(value);
    }

    public IEnumerable<Customer> DataGridItemCollection
    {
        get => GetValue<IEnumerable<Customer>>();
        set => SetValue(value);
    }

    public IEnumerable<Brush> BrushCollection
    {
        get => GetValue<IEnumerable<Brush>>();
        set => SetValue(value);
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

        BrushCollection = brushList;

        _dataInitialized = true;
    }
}

