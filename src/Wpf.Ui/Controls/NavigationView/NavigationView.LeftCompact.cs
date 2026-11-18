// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections;

namespace Wpf.Ui.Controls;

/// <content>
/// Coordinates hierarchical flyouts in the LeftCompact pane display mode.
/// </content>
public partial class NavigationView
{
    private NavigationViewItem? _compactFlyoutOwner;

    internal void ToggleCompactFlyout(NavigationViewItem item)
    {
        bool wasOpen = _compactFlyoutOwner == item && item.IsExpanded;
        CloseCompactFlyout();
        if (!wasOpen)
        {
            _compactFlyoutOwner = item;
            item.SetCurrentValue(NavigationViewItem.IsExpandedProperty, true);
        }
    }

    internal void CloseCompactFlyout(bool restoreFocus = false)
    {
        NavigationViewItem? owner = _compactFlyoutOwner;
        _compactFlyoutOwner = null;
        owner?.SetCurrentValue(NavigationViewItem.IsExpandedProperty, false);
        if (restoreFocus && owner is { IsVisible: true })
        {
            _ = owner.Focus();
        }
    }

    internal void RegisterCompactFlyout(NavigationViewItem item)
    {
        if (_compactFlyoutOwner != item)
        {
            CloseCompactFlyout();
            _compactFlyoutOwner = item;
        }
    }

    internal void OnCompactFlyoutClosed(NavigationViewItem item, bool restoreFocus)
    {
        if (_compactFlyoutOwner == item)
        {
            CloseCompactFlyout(restoreFocus);
        }
    }

    internal void MoveCompactItemFocus(NavigationViewItem item, int offset)
    {
        var items = new List<NavigationViewItem>();
        if (_compactFlyoutOwner is { } owner && item.NavigationViewItemParent is not null)
        {
            AddVisibleItems(owner.MenuItems, items);
        }
        else
        {
            AddVisibleItems(MenuItems, items);
            AddVisibleItems(FooterMenuItems, items);
        }

        int index = items.IndexOf(item);
        int next = index + offset;
        if (index >= 0 && next >= 0 && next < items.Count)
        {
            _ = items[next].Focus();
        }
    }

    private static void AddVisibleItems(IEnumerable source, List<NavigationViewItem> items)
    {
        foreach (NavigationViewItem item in source.OfType<NavigationViewItem>())
        {
            if (!item.IsVisible || !item.IsEnabled)
            {
                continue;
            }

            items.Add(item);
            if (item.IsPaneOpen && item.IsExpanded)
            {
                AddVisibleItems(item.MenuItems, items);
            }
        }
    }

    private static void UpdateCompactItemPresentation(IEnumerable items)
    {
        foreach (NavigationViewItem item in items.OfType<NavigationViewItem>())
        {
            item.UpdatePanePresentation();
            UpdateCompactItemPresentation(item.MenuItems);
        }
    }
}
