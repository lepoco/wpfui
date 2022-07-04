// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Demo.ViewModels;
using Wpf.Ui.Mvvm.Contracts;

namespace Wpf.Ui.Demo.Views.Pages;

/// <summary>
/// Interaction logic for Buttons.xaml
/// </summary>
public partial class Buttons
{
    public ButtonsViewModel ViewModel
    {
        get;
    }

    public Buttons(ButtonsViewModel viewModel)
    {
        //ViewModel = App.GetService<ButtonsViewModel>();
        ViewModel = viewModel;
        DataContext = this;

        var testGetThemeService = App.GetService<IThemeService>();
        var currentTheme = testGetThemeService.GetSystemTheme();

        InitializeComponent();
    }
}
