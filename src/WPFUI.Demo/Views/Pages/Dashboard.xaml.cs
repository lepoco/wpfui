// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Controls;

namespace WPFUI.Demo.Views.Pages;

/// <summary>
/// Interaction logic for Dashboard.xaml
/// </summary>
public partial class Dashboard : Page
{
    public Dashboard()
    {
        InitializeComponent();
    }

    private void ButtonControls_OnClick(object sender, RoutedEventArgs e)
    {
        (Application.Current.MainWindow as Container)?.RootNavigation.Navigate("controls");
    }

    private void ButtonAction_OnClick(object sender, RoutedEventArgs e)
    {
        if (sender is not WPFUI.Controls.CardAction cardAction)
            return;

        var tag = cardAction.Tag as string;
        var navTag = String.Empty;

        if (String.IsNullOrWhiteSpace(tag))
            return;

        switch (tag)
        {
            case "input":
                navTag = tag;
                break;

            case "controls":
                navTag = tag;
                break;

            case "colors":
                navTag = tag;
                break;

            case "icons":
                navTag = tag;
                break;
        }

        if (String.IsNullOrWhiteSpace(navTag))
            return;

        (Application.Current.MainWindow as Container)?.RootNavigation.Navigate(navTag);
    }
}

