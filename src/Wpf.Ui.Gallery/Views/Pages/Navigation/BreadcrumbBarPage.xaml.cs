// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Controls;
using Wpf.Ui.Gallery.ControlsLookup;
using Wpf.Ui.Gallery.ViewModels.Pages.Navigation;

namespace Wpf.Ui.Gallery.Views.Pages.Navigation;

[GalleryPage("Shows the trail of navigation taken to the current location.", SymbolRegular.Navigation24)]
public partial class BreadcrumbBarPage : INavigableView<BreadcrumbBarViewModel>
{
    public BreadcrumbBarViewModel ViewModel { get; }

    public BreadcrumbBarPage(BreadcrumbBarViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }
}
