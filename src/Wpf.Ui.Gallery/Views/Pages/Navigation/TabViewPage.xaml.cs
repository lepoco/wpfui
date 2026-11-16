// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Controls;
using Wpf.Ui.Gallery.ControlsLookup;
using Wpf.Ui.Gallery.ViewModels.Pages.Navigation;

namespace Wpf.Ui.Gallery.Views.Pages.Navigation;

[GalleryPage("Add, close and reorder document tabs.", SymbolRegular.TabDesktop24)]
public partial class TabViewPage : INavigableView<TabViewViewModel>
{
    private int _nextTabNumber = 3;

    public TabViewViewModel ViewModel { get; }

    public TabViewPage(TabViewViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = viewModel;
        InitializeComponent();
    }

    private void OnTabAdding(object sender, TabViewItemAddingEventArgs e)
    {
        int number = _nextTabNumber++;
        e.TabItem = new TabViewItem
        {
            Header = $"Document {number}",
            Content = new System.Windows.Controls.TextBlock
            {
                Text = $"Content of document {number}.",
                Margin = new Thickness(12),
            },
        };
    }

    private void OnModelTabAdding(object sender, TabViewItemAddingEventArgs e)
    {
        e.Item = ViewModel.CreateDocument();
    }

    private void OnModelTabClosing(object sender, TabViewItemClosingEventArgs e)
    {
        // Business rules use the model, not the generated TabViewItem.
        e.Cancel = e.Item is TabDocument { IsClosable: false };
    }
}
