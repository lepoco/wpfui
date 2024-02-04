// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Wpf.Ui.Demo.Console.Views.Pages;

namespace Wpf.Ui.Demo.Console.Views;

public partial class MainView
{
    public MainView()
    {
        DataContext = this;

        InitializeComponent();

        Loaded += (_, _) => RootNavigation.Navigate(typeof(DashboardPage));

        UiApplication.Current.MainWindow = this;

        this.ApplyTheme();
    }
}
