// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Gallery.ViewModels.Pages.Collections;

namespace Wpf.Ui.Gallery.Views.Pages.Collections;

public partial class TreeViewPage : INavigableView<TreeViewViewModel>
{
    public TreeViewViewModel ViewModel { get; }

    public TreeViewPage(TreeViewViewModel viewModel)
    {
        ViewModel = viewModel;

        InitializeComponent();
    }
}
