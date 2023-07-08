// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using Wpf.Ui.Appearance;

namespace Wpf.Ui.Demo.Simple.Views.Pages;

/// <summary>
/// Interaction logic for SettingsPage.xaml
/// </summary>
public partial class SettingsPage
{
    public SettingsPage()
    {
        InitializeComponent();

        AppVersionTextBlock.Text = $"WPF UI - Simple Demo - {GetAssemblyVersion()}";

        if (Appearance.Theme.GetAppTheme() == ThemeType.Dark)
            DarkThemeRadioButton.IsChecked = true;
        else
            LightThemeRadioButton.IsChecked = true;
    }

    private void OnLightThemeRadioButtonChecked(object sender, RoutedEventArgs e)
    {
        Appearance.Theme.Apply(ThemeType.Light);
    }

    private void OnDarkThemeRadioButtonChecked(object sender, RoutedEventArgs e)
    {
        Appearance.Theme.Apply(ThemeType.Dark);
    }

    private string GetAssemblyVersion()
    {
        return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString()
            ?? String.Empty;
    }
}
