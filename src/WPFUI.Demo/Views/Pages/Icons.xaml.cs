// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using WPFUI.Demo.ViewModels;

namespace WPFUI.Demo.Views.Pages;

/// <summary>
/// Interaction logic for Icons.xaml
/// </summary>
public partial class Icons
{
    public Icons(IconsViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
