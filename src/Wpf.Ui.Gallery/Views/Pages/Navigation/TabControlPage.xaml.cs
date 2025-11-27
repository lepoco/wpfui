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
    /// Handles the mouse left button down event on a tab item.
    /// Selects the clicked tab and prepares for potential drag operation.
    /// </summary>
    private void StandardTabItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        // Don't process if the close button was clicked
        if (IsCloseButton(e.OriginalSource) || sender is not TabItem tabItem)
        {
            return;
        }

        // Use ViewModel to select the tab and prepare for drag
        ViewModel.SetDragStartPoint(e.GetPosition(null));
        ViewModel.SelectTabForDragCommand.Execute(tabItem);
    }

    /// <summary>
    /// Handles the mouse move event during drag operation.
    /// Initiates drag-and-drop when the mouse has moved beyond the minimum drag distance.
    /// </summary>
    private void StandardTabItem_PreviewMouseMove(object sender, MouseEventArgs e)
    {
        // Don't process if the close button is being hovered or mouse button is not pressed
        if (IsCloseButton(e.OriginalSource) || e.LeftButton != MouseButtonState.Pressed)
        {
            return;
        }

        Point currentPoint = e.GetPosition(null);

        // Use ViewModel to check if drag should start
        if (ViewModel.TryStartDrag(currentPoint))
        {
            var draggedTab = ViewModel.GetDraggedTab();
            if (draggedTab != null)
            {
                DragDrop.DoDragDrop(draggedTab, draggedTab, DragDropEffects.Move);
                ViewModel.EndDrag();
            }
        }
    }

    /// <summary>
    /// Handles the mouse left button up event.
    /// Clears the dragged tab reference when the drag operation ends.
    /// </summary>
    private void StandardTabItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        // Use ViewModel to end drag operation
        ViewModel.EndDrag();
    }

    /// <summary>
    /// Handles the drop event when a tab is dropped onto the TabControl.
    /// Reorders the tabs in the collection based on the drop position.
    /// </summary>
    private void StandardTabControl_Drop(object sender, DragEventArgs e)
    {
        var draggedTab = ViewModel.GetDraggedTab();
        if (draggedTab == null || sender is not TabControl tabControl)
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
        if (targetTabItem == null)
        {
            return;
        }

        // Use ViewModel to reorder tabs
        ViewModel.ReorderTabs(draggedTab, targetTabItem);
    }

    private static T? FindParent<T>(DependencyObject child) where T : DependencyObject
    {
        DependencyObject? current = child;
        while (current != null)
        {
            current = VisualTreeHelper.GetParent(current);
            if (current is T parent)
            {
                return parent;
            }
        }
        return null;
    }

    /// <summary>
    /// Determines if the specified element is a close button.
    /// </summary>
    private static bool IsCloseButton(object? source)
    {
        if (source is not DependencyObject depObj)
        {
            return false;
        }

        DependencyObject? current = depObj;
        const string closeButtonTag = "CloseButton";
        
        while (current != null)
        {
            if (current is System.Windows.Controls.Button button && 
                button.Tag is string tag && 
                tag == closeButtonTag)
            {
                return true;
            }
            current = VisualTreeHelper.GetParent(current);
        }
        return false;
    }
}
