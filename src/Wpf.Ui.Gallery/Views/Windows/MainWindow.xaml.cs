// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Abstractions;
using Wpf.Ui.Controls;
using Wpf.Ui.Gallery.Services;
using Wpf.Ui.Gallery.Services.Contracts;
using Wpf.Ui.Gallery.ViewModels.Windows;
using Wpf.Ui.Gallery.Views.Pages;

namespace Wpf.Ui.Gallery.Views.Windows;

public partial class MainWindow : IWindow
{
    private readonly AppConfigService _configService;

    public MainWindow(
        MainWindowViewModel viewModel,
        INavigationService navigationService,
        IServiceProvider serviceProvider,
        ISnackbarService snackbarService,
        IContentDialogService contentDialogService
    )
    {
        Appearance.SystemThemeWatcher.Watch(this);

        ViewModel = viewModel;
        DataContext = this;

        // Initialize configuration service
        _configService = new AppConfigService();

        InitializeComponent();

        // Restore saved navigation pane width
        RestoreNavigationPaneWidth();

        NavigationView.SetPageProviderService(serviceProvider.GetRequiredService<INavigationViewPageProvider>());
        snackbarService.SetSnackbarPresenter(SnackbarPresenter);
        contentDialogService.SetDialogHost(RootContentDialog);

        NavigationView.SetCurrentValue(NavigationView.MenuItemsSourceProperty, ViewModel.MenuItems);
        NavigationView.SetCurrentValue(NavigationView.FooterMenuItemsSourceProperty, ViewModel.FooterMenuItems);
    }

    public MainWindowViewModel ViewModel { get; }

    private bool _isUserClosedPane;

    private bool _isPaneOpenedOrClosedFromCode;

    /// <summary>
    /// Handles the value change event of the navigation pane width slider
    /// </summary>
    private void PaneWidthSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (NavigationView != null && e.NewValue > 0)
        {
            NavigationView.SetCurrentValue(NavigationView.OpenPaneLengthProperty, e.NewValue);
            _configService.UpdateNavigationPaneWidth(e.NewValue);
        }
    }

    /// <summary>
    /// Restores the saved navigation pane width
    /// </summary>
    private void RestoreNavigationPaneWidth()
    {
        var savedWidth = _configService.Config.NavigationPaneWidth;
        if (savedWidth >= 200 && savedWidth <= 500)
        {
            NavigationView.SetCurrentValue(NavigationView.OpenPaneLengthProperty, savedWidth);
            PaneWidthSlider.SetCurrentValue(System.Windows.Controls.Primitives.RangeBase.ValueProperty, savedWidth);
        }
    }

    private void OnNavigationSelectionChanged(object sender, RoutedEventArgs e)
    {
        if (sender is not Wpf.Ui.Controls.NavigationView navigationView)
        {
            return;
        }

        NavigationView.SetCurrentValue(
            NavigationView.HeaderVisibilityProperty,
            navigationView.SelectedItem?.TargetPageType != typeof(DashboardPage)
                ? Visibility.Visible
                : Visibility.Collapsed
        );
    }

    private void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (_isUserClosedPane)
        {
            return;
        }

        _isPaneOpenedOrClosedFromCode = true;
        NavigationView.SetCurrentValue(NavigationView.IsPaneOpenProperty, e.NewSize.Width > 1200);
        _isPaneOpenedOrClosedFromCode = false;
    }

    private void NavigationView_OnPaneOpened(NavigationView sender, RoutedEventArgs args)
    {
        if (_isPaneOpenedOrClosedFromCode)
        {
            return;
        }

        _isUserClosedPane = false;
    }

    private void NavigationView_OnPaneClosed(NavigationView sender, RoutedEventArgs args)
    {
        if (_isPaneOpenedOrClosedFromCode)
        {
            return;
        }

        _isUserClosedPane = true;
    }
}
