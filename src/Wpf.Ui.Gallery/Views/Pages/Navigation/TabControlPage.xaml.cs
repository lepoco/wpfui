// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Wpf.Ui.Controls;
using Wpf.Ui.Gallery.ControlsLookup;
using Wpf.Ui.Gallery.ViewModels.Pages.Navigation;

namespace Wpf.Ui.Gallery.Views.Pages.Navigation;

[GalleryPage("Tab control like in browser.", SymbolRegular.TabDesktopBottom24)]
public partial class TabControlPage : INavigableView<TabControlViewModel>
{
    public TabControlViewModel ViewModel { get; }

    // Stores the tab being dragged during drag-and-drop operation
    private TabItem? _standardDraggedTab;

    // Stores the starting point of the drag operation
    private Point _standardStartPoint;

    public TabControlPage(TabControlViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }

    /// <summary>
    /// Handles the mouse left button down event on a tab item.
    /// Selects the clicked tab and prepares for potential drag operation.
    /// </summary>
    private void StandardTabItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is TabItem tabItem && tabItem != null)
        {
            // Deselect all other tabs to ensure only one tab is selected at a time
            foreach (TabItem tab in ViewModel.StandardTabs)
            {
                if (tab != tabItem && tab.IsSelected)
                {
                    tab.SetCurrentValue(TabItem.IsSelectedProperty, false);
                }
            }

            // Select the clicked tab before starting drag operation
            tabItem.SetCurrentValue(TabItem.IsSelectedProperty, true);
            _standardDraggedTab = tabItem;
            _standardStartPoint = e.GetPosition(null);
        }
    }

    /// <summary>
    /// Handles the mouse move event during drag operation.
    /// Initiates drag-and-drop when the mouse has moved beyond the minimum drag distance.
    /// </summary>
    private void StandardTabItem_PreviewMouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed && _standardDraggedTab != null)
        {
            Point currentPoint = e.GetPosition(null);

            // Check if the mouse has moved far enough to initiate a drag operation
            if (Math.Abs(currentPoint.X - _standardStartPoint.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(currentPoint.Y - _standardStartPoint.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                DragDrop.DoDragDrop(_standardDraggedTab, _standardDraggedTab, DragDropEffects.Move);
            }
        }
    }

    /// <summary>
    /// Handles the mouse left button up event.
    /// Clears the dragged tab reference when the drag operation ends.
    /// </summary>
    private void StandardTabItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        _standardDraggedTab = null;
    }

    /// <summary>
    /// Handles the drop event when a tab is dropped onto the TabControl.
    /// Reorders the tabs in the collection based on the drop position.
    /// </summary>
    private void StandardTabControl_Drop(object sender, DragEventArgs e)
    {
        if (_standardDraggedTab == null || sender is not TabControl tabControl)
        {
            return;
        }

        // Find the tab item at the drop position
        HitTestResult hitTestResult = VisualTreeHelper.HitTest(tabControl, e.GetPosition(tabControl));
        if (hitTestResult?.VisualHit == null)
        {
            return;
        }

        // Get the target tab item from the visual tree
        TabItem? targetTabItem = FindParent<TabItem>(hitTestResult.VisualHit);
        if (targetTabItem == null || targetTabItem == _standardDraggedTab)
        {
            return;
        }

        // Reorder tabs: remove from original position and insert at target position
        ObservableCollection<TabItem> tabs = ViewModel.StandardTabs;
        int draggedIndex = tabs.IndexOf(_standardDraggedTab);
        int targetIndex = tabs.IndexOf(targetTabItem);

        if (draggedIndex >= 0 && targetIndex >= 0 && draggedIndex != targetIndex)
        {
            tabs.RemoveAt(draggedIndex);
            tabs.Insert(targetIndex, _standardDraggedTab);

            // Maintain selection state after drop to keep the moved tab selected
            _standardDraggedTab.SetCurrentValue(TabItem.IsSelectedProperty, true);
        }
    }

    private static T? FindParent<T>(DependencyObject child) where T : DependencyObject
    {
        DependencyObject parentObject = VisualTreeHelper.GetParent(child);
        if (parentObject == null)
        {
            return null;
        }

        if (parentObject is T parent)
        {
            return parent;
        }

        return FindParent<T>(parentObject);
    }
}
