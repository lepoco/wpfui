// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Controls;
using Wpf.Ui.Gallery.ViewModels.Pages.Icons;

namespace Wpf.Ui.Gallery.Views.Pages.Icons;

public partial class FontIconPage : INavigableView<FontIconViewModel>
{
    public FontIconViewModel ViewModel { get; }

    public FontIconPage(FontIconViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }
}
