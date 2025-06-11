// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using Wpf.Ui.Demo.SetResources.Simple.Views.Pages;
using Wpf.Ui.Markup;

namespace Wpf.Ui.Demo.SetResources.Simple;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow()
    {
        DataContext = this;

        App.ApplyTheme(this);

        InitializeComponent();

        Loaded += (_, _) => RootNavigation.Navigate(typeof(DashboardPage));
    }
}
