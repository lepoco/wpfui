// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Controls.Navigation;
using Wpf.Ui.Gallery.ViewModels.Pages.BasicInput;

namespace Wpf.Ui.Gallery.Views.Pages.BasicInput;

public partial class ToggleSwitchPage : INavigableView<ToggleSwitchViewModel>
{
    public ToggleSwitchViewModel ViewModel { get; }

    public ToggleSwitchPage(ToggleSwitchViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();
    }
}
