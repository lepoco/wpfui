// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Controls;
using Wpf.Ui.Gallery.ViewModels.Windows;

namespace Wpf.Ui.Gallery.Views.Windows;

public partial class SandboxWindow
{
    public SandboxWindowViewModel ViewModel { get; init; }

    public SandboxWindow(SandboxWindowViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }

    private void OnAutoSuggestBoxTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        Debug.WriteLine($"OnAutoSuggestBoxTextChanged: {sender.Text} (ViewModel.AutoSuggestBoxText: {ViewModel.AutoSuggestBoxText})");
    }
}
