// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Controls;
using Wpf.Ui.Gallery.ControlsLookup;
using Wpf.Ui.Gallery.ViewModels.Pages.OpSystem;

namespace Wpf.Ui.Gallery.Views.Pages.OpSystem;

[GalleryPage("System clipboard.", SymbolRegular.Desktop24)]
public partial class ClipboardPage : INavigableView<ClipboardViewModel>
{
    public ClipboardViewModel ViewModel { get; }

    public ClipboardPage(ClipboardViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }
}
