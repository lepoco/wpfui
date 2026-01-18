// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Controls;
using Wpf.Ui.Gallery.ControlsLookup;
using Wpf.Ui.Gallery.ViewModels.Pages.BasicInput;

namespace Wpf.Ui.Gallery.Views.Pages.BasicInput;

[GalleryPage("NumericUpDown control", SymbolRegular.NumberSymbol16)]
public partial class NumericUpDownPage : INavigableView<NumericUpDownViewModel>
{
    public NumericUpDownViewModel ViewModel { get; }

    public NumericUpDownPage(NumericUpDownViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }
}
