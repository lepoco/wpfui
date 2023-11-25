// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using Wpf.Ui.Appearance;

namespace Wpf.Ui.Demo.Console.Views.Pages;

/// <summary>
/// Interaction logic for SettingsPage.xaml
/// </summary>
public partial class SettingsPage
{
    public SettingsPage()
    {
        InitializeComponent();

        AppVersionTextBlock.Text = $"WPF UI - Console Demo - {GetAssemblyVersion()}";

        if (Appearance.ApplicationThemeManager.GetAppTheme() == ApplicationTheme.Dark)
        {
            DarkThemeRadioButton.IsChecked = true;
        }
        else
        {
            LightThemeRadioButton.IsChecked = true;
        }

        MainView.Apply(this);
        Appearance.ApplicationThemeManager.Changed += (s, e) =>
        {
            MainView.Apply(this);
        };
    }

    private void OnLightThemeRadioButtonChecked(object sender, RoutedEventArgs e)
    {
        Appearance.ApplicationThemeManager.Apply(ApplicationTheme.Light, updateAccent: false);
    }

    private void OnDarkThemeRadioButtonChecked(object sender, RoutedEventArgs e)
    {
        Appearance.ApplicationThemeManager.Apply(ApplicationTheme.Dark, updateAccent: false);
    }

    private string GetAssemblyVersion()
    {
        return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString()
            ?? String.Empty;
    }
}
