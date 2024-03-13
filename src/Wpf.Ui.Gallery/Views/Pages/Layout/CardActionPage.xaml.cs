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
using Wpf.Ui.Gallery.ViewModels.Pages.Layout;

namespace Wpf.Ui.Gallery.Views.Pages.Layout;

/// <summary>
/// Interaction logic for CardActionPage.xaml
/// </summary>
[GalleryPage("Card action control.", SymbolRegular.Code24)]
public partial class CardActionPage : INavigableView<CardActionViewModel>
{
    public CardActionPage(CardActionViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
    }

    public CardActionViewModel ViewModel { get; }
}
