// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace Wpf.Ui.Gallery.ViewModels.Pages.Navigation;

public partial class TabControlViewModel : ViewModel
{
    [ObservableProperty]
    private TabItem? _selectedTab;

    // Stores the tab being dragged during drag-and-drop operation
    private TabItem? _draggedTab;

    // Stores the starting point of the drag operation
    private Point _dragStartPoint;

    // Indicates whether a drag operation is currently in progress
    private bool _isDragging;

    partial void OnSelectedTabChanged(TabItem? value)
    {
        // Update IsSelected property only for tabs that need to change
        foreach (TabItem tab in StandardTabs)
        {
            bool shouldBeSelected = tab == value;
            if (tab.IsSelected != shouldBeSelected)
            {
                tab.SetCurrentValue(TabItem.IsSelectedProperty, shouldBeSelected);
            }
        }
    }

    [ObservableProperty]
    private ObservableCollection<TabItem> _standardTabs =
    [
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
    ];

    /// <summary>
    /// Adds a new tab to the collection.
    /// </summary>
    [RelayCommand]
    private void AddTab()
    {
        // Create a new tab with a unique name
        int tabNumber = StandardTabs.Count + 1;
        var newTab = new TabItem
        {
            Header = CreateTabHeader($"New Tab {tabNumber}", SymbolRegular.Document24),
            Content = new System.Windows.Controls.TextBlock
            {
                Text = $"New Tab {tabNumber} content",
                Margin = new System.Windows.Thickness(12)
            }
        };

        // Add the new tab to the collection
        StandardTabs.Add(newTab);

        // Select the new tab (this will update IsSelected for all tabs via OnSelectedTabChanged)
        SelectedTab = newTab;
    }

    /// <summary>
    /// Closes the specified tab.
    /// </summary>
    /// <param name="tabItem">The tab item to close.</param>
    [RelayCommand]
    private void CloseTab(object? tabItem)
    {
        if (tabItem is not TabItem item)
        {
            return;
        }

        if (StandardTabs.Count <= 1)
        {
            // Don't remove the last tab
            return;
        }

        int tabIndex = StandardTabs.IndexOf(item);
        if (tabIndex < 0)
        {
            return;
        }

        // Remove the tab
        StandardTabs.RemoveAt(tabIndex);

        // Select another tab (preferably the one at the same index, or the last one)
        if (StandardTabs.Count > 0)
        {
            int newSelectedIndex = Math.Min(tabIndex, StandardTabs.Count - 1);
            SelectedTab = StandardTabs[newSelectedIndex];
        }
    }

    /// <summary>
    /// Selects the specified tab and prepares for potential drag operation.
    /// </summary>
    [RelayCommand]
    private void SelectTabForDrag(object? parameter)
    {
        if (parameter is not TabItem tabItem)
        {
            return;
        }

        // Select the clicked tab
        SelectedTab = tabItem;
        _draggedTab = tabItem;
    }

    /// <summary>
    /// Starts the drag operation if the mouse has moved far enough.
    /// </summary>
    /// <param name="currentPoint">The current mouse position.</param>
    /// <returns>True if drag operation was started, false otherwise.</returns>
    public bool TryStartDrag(Point currentPoint)
    {
        if (_draggedTab == null || _isDragging)
        {
            return false;
        }

        // Check if the mouse has moved far enough to initiate a drag operation
        double deltaX = currentPoint.X - _dragStartPoint.X;
        double deltaY = currentPoint.Y - _dragStartPoint.Y;
        double minDistance = SystemParameters.MinimumHorizontalDragDistance;

        if (Math.Abs(deltaX) > minDistance || Math.Abs(deltaY) > minDistance)
        {
            _isDragging = true;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Gets the tab being dragged.
    /// </summary>
    public TabItem? GetDraggedTab() => _draggedTab;

    /// <summary>
    /// Sets the starting point for drag operation.
    /// </summary>
    public void SetDragStartPoint(Point point)
    {
        _dragStartPoint = point;
    }

    /// <summary>
    /// Ends the drag operation.
    /// </summary>
    public void EndDrag()
    {
        _draggedTab = null;
        _isDragging = false;
    }

    /// <summary>
    /// Reorders tabs by moving a tab from one position to another.
    /// </summary>
    /// <param name="draggedTab">The tab being moved.</param>
    /// <param name="targetTab">The target tab position.</param>
    public void ReorderTabs(TabItem draggedTab, TabItem targetTab)
    {
        if (draggedTab == targetTab)
        {
            return;
        }

        int draggedIndex = StandardTabs.IndexOf(draggedTab);
        int targetIndex = StandardTabs.IndexOf(targetTab);

        // Early return if indices are invalid or the same
        if (draggedIndex < 0 || targetIndex < 0 || draggedIndex == targetIndex)
        {
            return;
        }

        StandardTabs.Move(draggedIndex, targetIndex);
        SelectedTab = draggedTab;
    }

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
