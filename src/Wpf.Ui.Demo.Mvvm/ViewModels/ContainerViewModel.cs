// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Navigation;
using Wpf.Ui.Mvvm.Contracts;

namespace Wpf.Ui.Demo.Mvvm.ViewModels;

public partial class ContainerViewModel : ObservableObject
{
    private bool _isInitialized = false;

    [ObservableProperty]
    private string _applicationTitle = String.Empty;

    [ObservableProperty]
    private ObservableCollection<object> _navigationItems = new();

    [ObservableProperty]
    private ObservableCollection<object> _navigationFooter = new();

    [ObservableProperty]
    private ObservableCollection<MenuItem> _trayMenuItems = new();

    public ContainerViewModel(INavigationService navigationService)
    {
        if (!_isInitialized)
            InitializeViewModel();
    }

    private void InitializeViewModel()
    {
        ApplicationTitle = "WPF UI - MVVM Demo";

        NavigationItems = new ObservableCollection<object>
            {
                new NavigationViewItem()
                {
                    Content = "Home",
                    Icon = SymbolRegular.Home24,
                    TargetPageType = typeof(Views.Pages.DashboardPage)
                },
                new NavigationViewItem()
                {
                    Content = "Data",
                    Icon = SymbolRegular.DataHistogram24,
                    TargetPageType = typeof(Views.Pages.DataPage)
                }
            };

        NavigationFooter = new ObservableCollection<object>
            {
                new NavigationViewItem()
                {
                    Content = "Settings",
                    Icon = SymbolRegular.Settings24,
                    TargetPageType = typeof(Views.Pages.SettingsPage)
                }
            };

        TrayMenuItems = new ObservableCollection<MenuItem>
            {
                new MenuItem
                {
                    Header = "Home",
                    Tag = "tray_home"
                }
            };

        _isInitialized = true;
    }
}
