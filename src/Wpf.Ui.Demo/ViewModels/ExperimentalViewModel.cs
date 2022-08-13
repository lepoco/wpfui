// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace Wpf.Ui.Demo.ViewModels;

public class ExperimentalViewModel : ObservableObject, INavigationAware
{
    private bool _dataInitialized = false;

    private int _generalId = 0;

    private string _generalText = String.Empty;

    private INavigationWindow _parentWindow = null;

    public int GeneralId
    {
        get => _generalId;
        set => SetProperty(ref _generalId, value);
    }

    public string GeneralText
    {
        get => _generalText;
        set => SetProperty(ref _generalText, value);
    }

    public INavigationWindow ParentWindow
    {
        get => _parentWindow;
        set => SetProperty(ref _parentWindow, value);
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
        GeneralText = "Hello World";

        _dataInitialized = true;
    }
}
