// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.ObjectModel;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace Wpf.Ui.Gallery.ViewModels.Pages.Navigation;

public partial class TabControlViewModel : ViewModel
{
    [ObservableProperty]
    private ObservableCollection<TabItem> _standardTabs = new()
    {
        new TabItem
        {
            Header = CreateTabHeader("Hello", SymbolRegular.XboxConsole24),
            Content = new System.Windows.Controls.TextBlock { Text = "World", Margin = new System.Windows.Thickness(12) },
            IsSelected = true
        },
        new TabItem
        {
            Header = CreateTabHeader("The cake", SymbolRegular.StoreMicrosoft16),
            Content = new System.Windows.Controls.TextBlock { Text = "Is a lie.", Margin = new System.Windows.Thickness(12) }
        },
        new TabItem
        {
            Header = CreateTabHeader("Document", SymbolRegular.Document24),
            Content = new System.Windows.Controls.TextBlock { Text = "Document content", Margin = new System.Windows.Thickness(12) }
        },
        new TabItem
        {
            Header = CreateTabHeader("Settings", SymbolRegular.Settings24),
            Content = new System.Windows.Controls.TextBlock { Text = "Settings content", Margin = new System.Windows.Thickness(12) }
        },
        new TabItem
        {
            Header = CreateTabHeader("Mail", SymbolRegular.Mail24),
            Content = new System.Windows.Controls.TextBlock { Text = "Mail content", Margin = new System.Windows.Thickness(12) }
        },
        new TabItem
        {
            Header = CreateTabHeader("Calendar", SymbolRegular.CalendarLtr24),
            Content = new System.Windows.Controls.TextBlock { Text = "Calendar content", Margin = new System.Windows.Thickness(12) }
        },
        new TabItem
        {
            Header = CreateTabHeader("Image", SymbolRegular.Image24),
            Content = new System.Windows.Controls.TextBlock { Text = "Image content", Margin = new System.Windows.Thickness(12) }
        },
        new TabItem
        {
            Header = CreateTabHeader("Music", SymbolRegular.MusicNote124),
            Content = new System.Windows.Controls.TextBlock { Text = "Music content", Margin = new System.Windows.Thickness(12) }
        },
        new TabItem
        {
            Header = CreateTabHeader("Video", SymbolRegular.Video24),
            Content = new System.Windows.Controls.TextBlock { Text = "Video content", Margin = new System.Windows.Thickness(12) }
        },
        new TabItem
        {
            Header = CreateTabHeader("Folder", SymbolRegular.Folder24),
            Content = new System.Windows.Controls.TextBlock { Text = "Folder content", Margin = new System.Windows.Thickness(12) }
        },
        new TabItem
        {
            Header = CreateTabHeader("tab 1", SymbolRegular.Folder24),
            Content = new System.Windows.Controls.TextBlock { Text = "tab 1 content", Margin = new System.Windows.Thickness(12) }
        },
        new TabItem
        {
            Header = CreateTabHeader("tab 2", SymbolRegular.Folder24),
            Content = new System.Windows.Controls.TextBlock { Text = "tab 2 content", Margin = new System.Windows.Thickness(12) }
        },
    };

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
