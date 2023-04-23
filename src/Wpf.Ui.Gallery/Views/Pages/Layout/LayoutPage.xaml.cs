// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.


using Wpf.Ui.Controls.Navigation;
using Wpf.Ui.Gallery.ViewModels.Pages.Layout;

namespace Wpf.Ui.Gallery.Views.Pages.Layout;

public partial class LayoutPage : INavigableView<LayoutViewModel>
{
    public LayoutViewModel ViewModel { get; }

    public LayoutPage(LayoutViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }
}
