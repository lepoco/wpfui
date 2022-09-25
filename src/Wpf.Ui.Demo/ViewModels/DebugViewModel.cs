// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using Wpf.Ui.Common.Interfaces;

namespace Wpf.Ui.Demo.ViewModels;

public class DebugViewModel : ObservableObject, INavigationAware
{
    private bool _dataInitialized = false;

    private IEnumerable<Models.Hardware> _hardwareCollection = new Models.Hardware[] { };

    public IEnumerable<Models.Hardware> HardwareCollection
    {
        get => _hardwareCollection;
        set => SetProperty(ref _hardwareCollection, value);
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
        var hardwareCollection = new List<Models.Hardware>();

        hardwareCollection.Add(new Models.Hardware()
        {
            Name = "CPU",
            Value = 89,
            Min = 29,
            Max = 92
        });

        hardwareCollection.Add(new Models.Hardware()
        {
            Name = "GPU",
            Value = 59,
            Min = 22,
            Max = 78
        });

        HardwareCollection = hardwareCollection;

        _dataInitialized = true;
    }
}
