// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Wpf.Ui.Appearance;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
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

        var navigationItems = new ObservableCollection<INavigationControl>
        {
            new NavigationItem
            {
                Content = "Home",
                PageTag = "dashboard",
                Icon = SymbolRegular.Home24,
                PageType = typeof(Views.Pages.ExperimentalDashboard),
            },
            new NavigationItem
            {
                Content = "Debug",
                PageTag = "debug",
                Icon = SymbolRegular.Bug24,
                IconForeground = Brushes.BlueViolet,
                PageType = typeof(Views.Pages.Debug),
            },
            new NavigationHeader()
            {
                Text = "Precache"
            },
            new NavigationItem
            {
                Content = "Controls",
                PageTag = "controls1",
                Icon = SymbolRegular.Fluent24,
                PageType = typeof(Views.Pages.Controls),
            },
            new NavigationItem
            {
                Content = "Controls",
                PageTag = "controls2",
                Icon = SymbolRegular.Fluent24,
                PageType = typeof(Views.Pages.Controls),
            },
            new NavigationItem
            {
                Content = "Controls",
                PageTag = "controls3",
                Icon = SymbolRegular.Fluent24,
                PageType = typeof(Views.Pages.Controls),
            },
            new NavigationItem
            {
                Content = "Controls",
                PageTag = "controls4",
                Icon = SymbolRegular.Fluent24,
                PageType = typeof(Views.Pages.Controls),
            },
            new NavigationItem
            {
                Content = "Controls",
                PageTag = "controls5",
                Icon = SymbolRegular.Fluent24,
                PageType = typeof(Views.Pages.Controls),
            }
        };

        RootNavigation.Items = navigationItems;


        Wpf.Ui.Appearance.Background.Apply(this, Wpf.Ui.Appearance.BackgroundType.Mica);

        RootNavigation.Loaded += RootNavigationOnLoaded;
    }

    private void RootNavigationOnLoaded(object sender, RoutedEventArgs e)
    {
        RootNavigation.Navigate(0, DataContext);
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

    public Frame GetFrame()
        => RootFrame;

    public INavigation GetNavigation()
        => RootNavigation;

    public bool Navigate(Type pageType)
        => RootNavigation.Navigate(pageType);

    public void SetPageService(IPageService pageService)
        => RootNavigation.PageService = pageService;

    public void ShowWindow()
        => Show();

    public void CloseWindow()
        => Close();
}
