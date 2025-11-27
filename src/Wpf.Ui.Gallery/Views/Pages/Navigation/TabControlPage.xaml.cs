// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;
using Wpf.Ui.Controls;
using Wpf.Ui.Gallery.ControlsLookup;
using Wpf.Ui.Gallery.ViewModels.Pages.Navigation;

namespace Wpf.Ui.Gallery.Views.Pages.Navigation;

[GalleryPage("Tab control like in browser.", SymbolRegular.TabDesktopBottom24)]
public partial class TabControlPage : INavigableView<TabControlViewModel>
{
    public TabControlViewModel ViewModel { get; }

    public TabControlPage(TabControlViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = viewModel;

        InitializeComponent();

        // Ensure the first tab is selected and its content is displayed
        if (ViewModel.StandardTabs.Count > 0)
        {
            ViewModel.SelectedTab = ViewModel.StandardTabs[0];
        }
    }

    private void OnTabClosing(object sender, TabClosingEventArgs e)
    {
        // The tab will be automatically removed from ItemsSource by OnTabCloseRequested
        // ViewModel's CloseTabCommand is not needed here as the removal is handled automatically
    }

    private void OnTabAdding(object sender, TabAddingEventArgs e)
    {
        // Handle tab adding if needed
        // You can customize the new tab here
        // e.Header = "New Tab";
        // e.Content = new TextBlock { Text = "New Content" };
    }
}
