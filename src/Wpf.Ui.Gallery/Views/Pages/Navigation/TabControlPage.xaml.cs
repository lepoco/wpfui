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

    /// <summary>
    /// Initializes a new instance of the <see cref="TabControlPage"/> class.
    /// The sample ensures the first tab remains selected and prevents it from being closed.
    /// </summary>
    public TabControlPage(TabControlViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = viewModel;

        InitializeComponent();

        // Ensure the first tab is selected and its content is displayed
        if (ViewModel.StandardTabs.Count > 0)
        {
            ViewModel.SelectedTab = ViewModel.StandardTabs[0];

            // Make the first tab non-closable
            TabControlExtensions.SetIsClosable(ViewModel.StandardTabs[0], false);
        }
    }

    /// <summary>
    /// Handles the TabClosing event.
    /// This event is raised when a user attempts to close a tab.
    /// You can cancel the operation by setting e.Cancel = true.
    /// </summary>
    /// <remarks>
    /// Examples of usage:
    /// <list type="bullet">
    /// <item>Prevent closing the last tab (implemented in this method).</item>
    /// <item>Show confirmation dialog before closing (commented out, can be uncommented if needed).</item>
    /// </list>
    /// The tab will be automatically removed from ItemsSource by OnTabCloseRequested.
    /// ViewModel's CloseTabCommand is not needed here as the removal is handled automatically.
    /// </remarks>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event arguments containing the tab item to close.</param>
    private void OnTabClosing(object sender, TabClosingEventArgs e)
    {
        if (ViewModel.StandardTabs.Count <= 1)
        {
            e.Cancel = true;
            return;
        }
    }

    /// <summary>
    /// Handles the TabAdding event.
    /// This event is raised when a user clicks the add button to create a new tab.
    /// You can customize the new tab by setting e.Header, e.Content, or e.TabItem.
    /// You can cancel the operation by setting e.Cancel = true.
    /// </summary>
    /// <remarks>
    /// This implementation uses Method 1: Setting tab properties using TabAddingEventArgs.
    /// <list type="number">
    /// <item>Get the tab number from the current tab count.</item>
    /// <item>Set the header with an icon using CreateTabHeader.</item>
    /// <item>Set the content to a TextBlock with the tab number.</item>
    /// </list>
    /// Alternative Method 2: You can also create a custom TabItem and assign it to e.TabItem for more control over tab creation.
    /// Example: Cancel adding if maximum tabs reached (commented out, can be uncommented if needed).
    /// </remarks>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event arguments used to customize the new tab.</param>
    private void OnTabAdding(object sender, TabAddingEventArgs e)
    {
        int tabNumber = ViewModel.StandardTabs.Count + 1;

        e.Header = CreateTabHeader($"New Tab {tabNumber}", SymbolRegular.Document24);

        e.Content = new System.Windows.Controls.TextBlock
        {
            Text = $"New Tab {tabNumber} content",
            Margin = new System.Windows.Thickness(12)
        };
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
