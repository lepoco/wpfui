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
        // Method 1: Set tab name using TabAddingEventArgs Header property
        // Get the tab number (get current tab count from ViewModel)
        int tabNumber = ViewModel.StandardTabs.Count + 1;
        e.Header = $"New Tab {tabNumber}";
        e.Content = new System.Windows.Controls.TextBlock
        {
            Text = $"New Tab {tabNumber} content",
            Margin = new System.Windows.Thickness(12)
        };
        
        // Alternatively, you can create a custom TabItem
        // e.TabItem = new TabItem
        // {
        //     Header = $"New Tab {tabNumber}",
        //     Content = new TextBlock { Text = $"New Tab {tabNumber} content" }
        // };
    }
}
