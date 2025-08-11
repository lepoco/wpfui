// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls;
using Wpf.Ui.Gallery.ControlsLookup;
using Wpf.Ui.Gallery.ViewModels.Pages.Collections;

namespace Wpf.Ui.Gallery.Views.Pages.Collections;
/// <summary>
/// Interaction logic for ListViewScrollPage.xaml
/// </summary>
[GalleryPage("Selectable list.", SymbolRegular.GroupList24)]
public partial class ListViewScrollPage : INavigableView<ListViewScrollViewModel>
{
    /// <inheritdoc />
    public ListViewScrollViewModel ViewModel { get; set; }

    public ListViewScrollPage(ListViewScrollViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }

}
