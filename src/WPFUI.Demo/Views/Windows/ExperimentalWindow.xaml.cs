// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Controls;
using WPFUI.Appearance;
using WPFUI.Controls.Interfaces;
using WPFUI.Demo.ViewModels;
using WPFUI.Mvvm.Contracts;

namespace WPFUI.Demo.Views.Windows;

/// <summary>
/// Interaction logic for ExperimentalWindow.xaml
/// </summary>
public partial class ExperimentalWindow : WPFUI.Controls.UiWindow, INavigationWindow
{
    private readonly IThemeService _themeService;

    public ExperimentalWindow(ExperimentalViewModel viewModel, IThemeService themeService)
    {
        _themeService = themeService;

        viewModel.ParentWindow = this;
        DataContext = viewModel;
        InitializeComponent();

        WPFUI.Appearance.Background.Apply(this, WPFUI.Appearance.BackgroundType.Mica);

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
        //var newTheme = WPFUI.Appearance.Theme.GetAppTheme() == WPFUI.Appearance.ThemeType.Dark
        //    ? WPFUI.Appearance.ThemeType.Light
        //    : WPFUI.Appearance.ThemeType.Dark;

        // We apply the theme to the entire application.
        //WPFUI.Appearance.Theme.Apply(
        //    themeType: newTheme,
        //    backgroundEffect: WPFUI.Appearance.BackgroundType.Mica,
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
