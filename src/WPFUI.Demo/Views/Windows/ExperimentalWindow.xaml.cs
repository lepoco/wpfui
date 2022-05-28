// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using WPFUI.Common;
using WPFUI.Controls.Interfaces;

namespace WPFUI.Demo.Views.Windows;

public class ExperimentalViewData : ViewData
{
    public INavigation Navigation { get; set; } = (INavigation)null;

    private int _generalId = 0;
    public int GeneralId
    {
        get => _generalId;
        set => UpdateProperty(ref _generalId, value, nameof(GeneralId));
    }

    private string _generalText = "Hello world";
    public string GeneralText
    {
        get => _generalText;
        set => UpdateProperty(ref _generalText, value, nameof(GeneralText));
    }
}

/// <summary>
/// Interaction logic for ExperimentalWindow.xaml
/// </summary>
public partial class ExperimentalWindow : WPFUI.Controls.UiWindow
{
    private ExperimentalViewData _viewData;

    public ExperimentalWindow()
    {
        InitializeComponent();

        WPFUI.Appearance.Background.Apply(this, WPFUI.Appearance.BackgroundType.Mica);

        _viewData = new ExperimentalViewData();
        _viewData.GeneralId = 2;
        _viewData.Navigation = RootNavigation;

        RootNavigation.Loaded += RootNavigationOnLoaded;
    }

    private void RootNavigationOnLoaded(object sender, RoutedEventArgs e)
    {
        RootNavigation.Navigate(0, _viewData);
    }

    private void NavigationButtonTheme_OnClick(object sender, RoutedEventArgs e)
    {
        // We check what theme is currently
        // active and choose its opposite.
        var newTheme = WPFUI.Appearance.Theme.GetAppTheme() == WPFUI.Appearance.ThemeType.Dark
            ? WPFUI.Appearance.ThemeType.Light
            : WPFUI.Appearance.ThemeType.Dark;

        // We apply the theme to the entire application.
        WPFUI.Appearance.Theme.Apply(
            themeType: newTheme,
            backgroundEffect: WPFUI.Appearance.BackgroundType.Mica,
            updateAccent: true,
            forceBackground: false);
    }
}
