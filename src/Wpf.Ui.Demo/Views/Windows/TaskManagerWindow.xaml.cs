// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Demo.ViewModels;

namespace Wpf.Ui.Demo.Views.Windows;

/// <summary>
/// Interaction logic for TaskManagerWindow.xaml
/// </summary>
public partial class TaskManagerWindow
{
    public TaskManagerViewModel ViewModel
    {
        get;
    }

    public TaskManagerWindow(TaskManagerViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }
}
