// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Controls.Navigation;
using Wpf.Ui.Gallery.ViewModels.Pages.Icons;

namespace Wpf.Ui.Gallery.Views.Pages.Icons;

public partial class SymbolIconPage : INavigableView<SymbolIconViewModel>
{
    public SymbolIconViewModel ViewModel { get; }

    public SymbolIconPage(SymbolIconViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();
    }
}
