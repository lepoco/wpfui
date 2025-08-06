// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Ui.Controls;
using Wpf.Ui.Gallery.ControlsLookup;
using Wpf.Ui.Gallery.ViewModels.Pages.Collections;

namespace Wpf.Ui.Gallery.Views.Pages.Collections;
/// <summary>
/// ListViewScrollPage.xaml 的交互逻辑
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
