// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using Wpf.Ui.Appearance;
using Wpf.Ui.Common;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Controls.Navigation;
using Wpf.Ui.Demo.ViewModels;
using Wpf.Ui.Mvvm.Contracts;

namespace Wpf.Ui.Demo.Views.Windows;

/// <summary>
/// Interaction logic for ExperimentalWindow.xaml
/// </summary>
public partial class ExperimentalWindow : Wpf.Ui.Controls.UiWindow, INavigationWindow
{
    private readonly IThemeService _themeService;

    public ExperimentalWindow(ExperimentalViewModel viewModel, IThemeService themeService)
    {
        _themeService = themeService;

        viewModel.ParentWindow = this;
        DataContext = viewModel;
        InitializeComponent();

        var navigationItems = new ObservableCollection<object>
        {
            new NavigationViewItem
            {
                Content = "Home",
                TargetPageTag = "dashboard",
                Icon = SymbolRegular.Home24,
                TargetPageType = typeof(Views.Pages.ExperimentalDashboard),
            },
            new NavigationViewItem
            {
                Content = "Debug",
                TargetPageTag = "debug",
                Icon = SymbolRegular.Bug24,
                IconForeground = Brushes.BlueViolet,
                TargetPageType = typeof(Views.Pages.Debug),
            },
            new NavigationViewItemHeader()
            {
                Text = "Precache"
            },
            new NavigationViewItem
            {
                Content = "Controls",
                TargetPageTag = "controls1",
                Icon = SymbolRegular.Fluent24,
                TargetPageType = typeof(Views.Pages.Controls),
            },
            new NavigationViewItem
            {
                Content = "Controls",
                TargetPageTag = "controls2",
                Icon = SymbolRegular.Fluent24,
                TargetPageType = typeof(Views.Pages.Controls),
            },
            new NavigationViewItem
            {
                Content = "Controls",
                TargetPageTag = "controls3",
                Icon = SymbolRegular.Fluent24,
                TargetPageType = typeof(Views.Pages.Controls),
            },
            new NavigationViewItem
            {
                Content = "Controls",
                TargetPageTag = "controls4",
                Icon = SymbolRegular.Fluent24,
                TargetPageType = typeof(Views.Pages.Controls),
            },
            new NavigationViewItem
            {
                Content = "Controls",
                TargetPageTag = "controls5",
                Icon = SymbolRegular.Fluent24,
                TargetPageType = typeof(Views.Pages.Controls),
            }
        };

        RootNavigation.MenuItemsSource = navigationItems;


        Wpf.Ui.Appearance.Background.Apply(this, Wpf.Ui.Appearance.BackgroundType.Mica);

        RootNavigation.Loaded += RootNavigationOnLoaded;
    }

    private void RootNavigationOnLoaded(object sender, RoutedEventArgs e)
    {
        RootNavigation.Navigate("controls1", DataContext);
    }

    private void NavigationButtonTheme_OnClick(object sender, RoutedEventArgs e)
    {
        // Classic way
        // We check what theme is currently
        // active and choose its opposite.
        //var newTheme = Wpf.Ui.Appearance.Theme.GetAppTheme() == Wpf.Ui.Appearance.ThemeType.Dark
        //    ? Wpf.Ui.Appearance.ThemeType.Light
        //    : Wpf.Ui.Appearance.ThemeType.Dark;

        // We apply the theme to the entire application.
        //Wpf.Ui.Appearance.Theme.Apply(
        //    themeType: newTheme,
        //    backgroundEffect: Wpf.Ui.Appearance.BackgroundType.Mica,
        //    updateAccent: true,
        //    forceBackground: false);

        // MVVM way
        _themeService.SetTheme(_themeService.GetTheme() == ThemeType.Dark ? ThemeType.Light : ThemeType.Dark);
    }

    public INavigationView GetNavigation()
        => RootNavigation;

    public bool Navigate(Type pageType)
        => RootNavigation.Navigate(pageType);

    public void SetServiceProvider(IServiceProvider serviceProvider)
        => RootNavigation.SetServiceProvider(serviceProvider);

    public void SetPageService(IPageService pageService)
        => RootNavigation.SetPageService(pageService);

    public void ShowWindow()
        => Show();

    public void CloseWindow()
        => Close();
}
