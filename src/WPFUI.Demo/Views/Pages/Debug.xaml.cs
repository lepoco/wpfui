// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using WPFUI.Demo.ViewModels;

namespace WPFUI.Demo.Views.Pages;

/// <summary>
/// Interaction logic for Debug.xaml
/// </summary>
public partial class Debug
{
    // CLASSIC
    public Debug()
    {
        DataContext = new DebugViewModel();
        InitializeComponent();
    }

    // MVVM
    public Debug(DebugViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }

    private void FocusSwitch_Checked(object sender, RoutedEventArgs e)
    {
        if (Window.GetWindow(this) is Container window)
        {
            window.DebuggingLayer.IsFocusIndicatorEnabled = FocusSwitch.IsChecked is true;
        }
    }
}
