// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Wpf.Ui.Common;
using Wpf.Ui.Controls.Navigation;

namespace Wpf.Ui.Demo.ViewModels;

public class TaskManagerViewModel : ObservableObject
{
    private bool _dataInitialized = false;

    private ObservableCollection<object> _navigationItems = new();

    private ObservableCollection<object> _navigationFooter = new();

    public ObservableCollection<object> NavigationItems
    {
        get => _navigationItems;
        set => SetProperty(ref _navigationItems, value);
    }

    public ObservableCollection<object> NavigationFooter
    {
        get => _navigationFooter;
        set => SetProperty(ref _navigationFooter, value);
    }

    public TaskManagerViewModel()
    {
        if (!_dataInitialized)
            InitializeViewModel();

        // Navigate to first Item
    }

    private void InitializeViewModel()
    {
        NavigationItems = new ObservableCollection<object>
        {
            new NavigationViewItem
            {
                Content = "Processes",
                TargetPageTag = "processes",
                Icon = SymbolRegular.Apps24,
                TargetPageType = typeof(Views.Pages.TMPage)
            },
            new NavigationViewItem
            {
                Content = "Performance",
                TargetPageTag = "performance",
                Icon = SymbolRegular.DesktopPulse24,
                TargetPageType = typeof(Views.Pages.TMPage)
            },
            new NavigationViewItem
            {
                Content = "App history",
                TargetPageTag = "app_history",
                Icon = SymbolRegular.History24,
                TargetPageType = typeof(Views.Pages.TMPage)
            },
            new NavigationViewItem
            {
                Content = "Startup",
                TargetPageTag = "startup",
                Icon = SymbolRegular.Home24,
                TargetPageType = typeof(Views.Pages.TMPage)
            },
            new NavigationViewItem
            {
                Content = "Users",
                TargetPageTag = "users",
                Icon = SymbolRegular.People24,
                TargetPageType = typeof(Views.Pages.TMPage)
            },
            new NavigationViewItem
            {
                Content = "Details",
                TargetPageTag = "details",
                Icon = SymbolRegular.TextBulletListLtr24,
                TargetPageType = typeof(Views.Pages.TMPage)
            },
            new NavigationViewItem
            {
                Content = "Services",
                TargetPageTag = "services",
                Icon = SymbolRegular.PuzzlePiece24,
                TargetPageType = typeof(Views.Pages.TMPage)
            }
        };

        NavigationFooter = new ObservableCollection<object>
        {
            new NavigationViewItem
            {
                Content = "Settings",
                TargetPageTag = "settings",
                Icon = SymbolRegular.Settings24,
                TargetPageType = typeof(Views.Pages.TMPage)
            }
        };

        _dataInitialized = true;
    }
}
