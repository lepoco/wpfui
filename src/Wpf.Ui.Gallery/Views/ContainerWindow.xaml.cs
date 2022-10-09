// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using Wpf.Ui.Gallery.Services.Contracts;
using Wpf.Ui.Gallery.ViewModels.Windows;
using Wpf.Ui.Gallery.Views.Pages;
using Wpf.Ui.Mvvm.Contracts;

namespace Wpf.Ui.Gallery.Views;

/// <summary>
/// Interaction logic for ContainerWindow.xaml
/// </summary>
public partial class ContainerWindow : IWindow
{
    public ContainerViewModel ViewModel { get; }

    public ContainerWindow(ContainerViewModel viewModel, INavigationService navigationService,
        IServiceProvider serviceProvider, ISnackbarService snackbarService,
        IDialogService dialogService)
    {
        Appearance.Watcher.Watch(this);

        DataContext = this;
        ViewModel = viewModel;

        InitializeComponent();

        snackbarService.SetSnackbarControl(RootSnackbar);
        dialogService.SetDialogControl(RootDialog);
        navigationService.SetNavigationControl(NavigationView);

        NavigationView.SetServiceProvider(serviceProvider);
        NavigationView.Loaded += (_, _) => NavigationView.Navigate(typeof(DashboardPage));
    }

    private void OnNavigationSelectionChanged(object sender, RoutedEventArgs e)
    {
        if (sender is not Wpf.Ui.Controls.Navigation.NavigationView navigationView)
            return;

        NavigationView.HeaderVisibility = navigationView.SelectedItem.TargetPageType != typeof(DashboardPage)
            ? Visibility.Visible
            : Visibility.Collapsed;
    }
}
