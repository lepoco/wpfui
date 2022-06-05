// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using WPFUI.Common.Interfaces;
using WPFUI.Mvvm.Contracts;

namespace WPFUI.Demo.ViewModels;

public class ExperimentalViewModel : WPFUI.Mvvm.ViewModelBase, INavigationAware
{
    private bool _dataInitialized = false;

    public int GeneralId
    {
        get => GetStructOrDefault(0);
        set => SetValue(value);
    }

    public string GeneralText
    {
        get => GetValueOrDefault(String.Empty);
        set => SetValue(value);
    }

    public INavigationWindow ParentWindow
    {
        get => GetValue<INavigationWindow>();
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

        GeneralText = "Hello World";

        _dataInitialized = true;
    }
}
