// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.


using Wpf.Ui.Controls;
using Wpf.Ui.Gallery.ControlsLookup;
using Wpf.Ui.Gallery.ViewModels.Pages.Text;

namespace Wpf.Ui.Gallery.Views.Pages.Text;

[GalleryPage("A rich editing control.", SymbolRegular.DrawText24)]
public partial class RichTextBoxPage : INavigableView<RichTextBoxViewModel>
{
    public RichTextBoxViewModel ViewModel { get; }

    public RichTextBoxPage(RichTextBoxViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }
}
