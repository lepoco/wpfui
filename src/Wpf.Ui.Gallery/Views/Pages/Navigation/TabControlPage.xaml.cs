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

    /// <summary>
    /// Handles the TabClosing event.
    /// This event is raised when a user attempts to close a tab.
    /// You can cancel the operation by setting e.Cancel = true.
    /// </summary>
    private void OnTabClosing(object sender, TabClosingEventArgs e)
    {
        // Example: Prevent closing the last tab
        if (ViewModel.StandardTabs.Count <= 1)
        {
            e.Cancel = true;
            return;
        }

        // Example: Show confirmation dialog (optional)
        // You can uncomment this to show a confirmation dialog
        // var result = MessageBox.Show(
        //     $"Are you sure you want to close '{e.TabItem.Header}'?",
        //     "Close Tab",
        //     MessageBoxButton.YesNo,
        //     MessageBoxImage.Question);
        // if (result == MessageBoxResult.No)
        // {
        //     e.Cancel = true;
        //     return;
        // }

        // The tab will be automatically removed from ItemsSource by OnTabCloseRequested
        // ViewModel's CloseTabCommand is not needed here as the removal is handled automatically
    }

    /// <summary>
    /// Handles the TabAdding event.
    /// This event is raised when a user clicks the add button to create a new tab.
    /// You can customize the new tab by setting e.Header, e.Content, or e.TabItem.
    /// You can cancel the operation by setting e.Cancel = true.
    /// </summary>
    private void OnTabAdding(object sender, TabAddingEventArgs e)
    {
        // Method 1: Set tab properties using TabAddingEventArgs
        // Get the tab number (get current tab count from ViewModel)
        int tabNumber = ViewModel.StandardTabs.Count + 1;

        // Set the header with an icon
        e.Header = CreateTabHeader($"New Tab {tabNumber}", SymbolRegular.Document24);

        // Set the content
        e.Content = new System.Windows.Controls.TextBlock
        {
            Text = $"New Tab {tabNumber} content",
            Margin = new System.Windows.Thickness(12)
        };

        // Method 2: Alternatively, you can create a custom TabItem
        // This gives you more control over the tab creation
        // e.TabItem = new TabItem
        // {
        //     Header = CreateTabHeader($"New Tab {tabNumber}", SymbolRegular.Document24),
        //     Content = new TextBlock
        //     {
        //         Text = $"New Tab {tabNumber} content",
        //         Margin = new Thickness(12)
        //     }
        // };

        // Example: Cancel adding if maximum tabs reached (optional)
        // const int maxTabs = 10;
        // if (ViewModel.StandardTabs.Count >= maxTabs)
        // {
        //     e.Cancel = true;
        //     MessageBox.Show($"Maximum {maxTabs} tabs allowed.", "Limit Reached");
        //     return;
        // }
    }

    /// <summary>
    /// Creates a tab header with an icon and text.
    /// </summary>
    private static System.Windows.Controls.StackPanel CreateTabHeader(string text, SymbolRegular symbol)
    {
        return new System.Windows.Controls.StackPanel
        {
            Orientation = System.Windows.Controls.Orientation.Horizontal,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
            Children =
            {
                new SymbolIcon
                {
                    Symbol = symbol,
                    Margin = new System.Windows.Thickness(0, 0, 6, 0)
                },
                new System.Windows.Controls.TextBlock
                {
                    Text = text
                }
            }
        };
    }
}
