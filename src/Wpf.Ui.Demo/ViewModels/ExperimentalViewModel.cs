// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace Wpf.Ui.Demo.ViewModels;

public partial class ExperimentalViewModel : ObservableObject, INavigationAware
{
    private bool _dataInitialized = false;

    [ObservableProperty]
    private int _generalId = 0;

    [ObservableProperty]
    private string _generalText = String.Empty;

    [ObservableProperty]
    private INavigationWindow _parentWindow = null;

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
        GeneralText = "Hello World";

        _dataInitialized = true;
    }
}
