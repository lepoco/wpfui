// Based on Windows UI Library
// Copyright(c) Microsoft Corporation.All rights reserved.

// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace Wpf.Ui.Controls.Navigation;

public partial class NavigationView
{
    private IServiceProvider? _serviceProvider = null;

    private IPageService? _pageService = null;

    private readonly List<string> _journal = new();

    private int _currentIndexInJournal = 0;

    /// <summary>
    /// Gets a value that indicates whether there is at least one entry in back navigation history.
    /// </summary>
    public bool CanGoBack => _journal.Count > 1;

    /// <summary>
    /// Allows you to assign to the NavigationView a special service responsible for retrieving the page instances.
    /// </summary>
    public void SetPageService(IPageService pageService)
        => _pageService = pageService;

    /// <summary>
    /// Allows you to assign a general <see cref="IServiceProvider"/> to the NavigationView that will be used to retrieve page instances and view models.
    /// </summary>
    public void SetServiceProvider(IServiceProvider serviceProvider)
        => _serviceProvider = serviceProvider;

    /// <summary>
    /// This method synchronously navigates this Frame to the
    /// given Element.
    /// </summary>
    public bool Navigate(Type pageType)
        => Navigate(pageType, null!);

    public bool Navigate(Type pageType, object dataContext)
    {
        if (MenuItemsSource is IEnumerable enumerableItemsSource)
            foreach (var singleMenuItem in enumerableItemsSource)
                if (singleMenuItem is NavigationViewItem singleNavigationViewItem)
                    if (singleNavigationViewItem?.TargetPageType != null && singleNavigationViewItem?.TargetPageType == pageType)
                        return NavigateInternal(singleNavigationViewItem, dataContext);

        if (FooterMenuItems is IEnumerable enumerableFooterItemsSource)
            foreach (var singleMenuItem in enumerableFooterItemsSource)
                if (singleMenuItem is NavigationViewItem singleNavigationViewItem)
                    if (singleNavigationViewItem?.TargetPageType != null && singleNavigationViewItem?.TargetPageType == pageType)
                        return NavigateInternal(singleNavigationViewItem, dataContext);

        return false;
    }

    public bool Navigate(string pageIdOrTargetTag)
        => Navigate(pageIdOrTargetTag, null!);

    public bool Navigate(string pageIdOrTargetTag, object dataContext)
    {
        if (MenuItemsSource is IEnumerable enumerableItemsSource)
            foreach (var singleMenuItem in enumerableItemsSource)
                if (singleMenuItem is NavigationViewItem singleNavigationViewItem)
                    if (singleNavigationViewItem.Id == pageIdOrTargetTag || singleNavigationViewItem?.TargetPageTag == pageIdOrTargetTag)
                        return NavigateInternal(singleNavigationViewItem, dataContext);

        if (FooterMenuItems is IEnumerable enumerableFooterItemsSource)
            foreach (var singleMenuItem in enumerableFooterItemsSource)
                if (singleMenuItem is NavigationViewItem singleNavigationViewItem)
                    if (singleNavigationViewItem.Id == pageIdOrTargetTag || singleNavigationViewItem?.TargetPageTag == pageIdOrTargetTag)
                        return NavigateInternal(singleNavigationViewItem, dataContext);

        return false;
    }

    /// <summary>
    /// Clears the NavigationView history.
    /// </summary>
    public void ClearJournal()
    {
        _journal.Clear();
        _currentIndexInJournal = 0;
    }

    /// <inheritdoc />
    public bool GoForward()
    {
        if (_journal.Count <= 1)
            return false;

        _currentIndexInJournal += 1;

        if (_currentIndexInJournal > _journal.Count - 1)
            return false;

        return Navigate(_journal[_currentIndexInJournal], null!);
    }

    /// <inheritdoc />
    public bool GoBack()
    {
        if (_journal.Count <= 1)
            return false;

        _currentIndexInJournal -= 1;

        if (_currentIndexInJournal < 0)
            return false;

        return Navigate(_journal[_currentIndexInJournal], null!);
    }

    protected bool NavigateInternal(INavigationViewItem viewItem, object? dataContext)
    {
        System.Diagnostics.Debug.WriteLine($"DEBUG | {viewItem.GetHashCode()} - {viewItem.TargetPageTag ?? "NO_TAG"} | CLICKED");

        if (viewItem == SelectedItem)
            return false;

        _journal.Add(viewItem.Id);

        if (_journal.Count > 20)
            _journal.RemoveAt(0);

        if (_currentIndexInJournal + 1 == _journal.Count)
            _currentIndexInJournal++;

        System.Diagnostics.Debug.WriteLine($"DEBUG | {viewItem.GetHashCode()} - {viewItem.TargetPageTag ?? "NO_TAG"} | NAVIGATED");

        SelectedItem = viewItem;

        UpdateActiveNavigationViewItem();
        OnSelectionChanged();

        return true;
    }

    private void UpdateActiveNavigationViewItem()
    {
        if (MenuItemsSource is IEnumerable enumerableMenuItems)
        {
            foreach (var singleMenuItem in enumerableMenuItems)
            {
                if (singleMenuItem is not NavigationViewItem navigationViewItem)
                    continue;

                navigationViewItem.IsActive = navigationViewItem == SelectedItem;

                if (navigationViewItem.MenuItems is IEnumerable enumerableSubMenuItems)
                {
                    foreach (var singleSubMenuItem in enumerableSubMenuItems)
                    {
                        if (singleSubMenuItem is not NavigationViewItem navigationViewSubItem)
                            continue;

                        navigationViewSubItem.IsActive = navigationViewSubItem == SelectedItem;
                    }
                }
            }
        }

        if (FooterMenuItemsSource is IEnumerable enumerableFooterMenuItems)
        {
            foreach (var singleFooterMenuItem in enumerableFooterMenuItems)
            {
                if (singleFooterMenuItem is not NavigationViewItem navigationViewItem)
                    continue;

                navigationViewItem.IsActive = navigationViewItem == SelectedItem;

                if (navigationViewItem.MenuItems is IEnumerable enumerableSubMenuItems)
                {
                    foreach (var singleSubMenuItem in enumerableSubMenuItems)
                    {
                        if (singleSubMenuItem is not NavigationViewItem navigationViewSubItem)
                            continue;

                        navigationViewSubItem.IsActive = navigationViewSubItem == SelectedItem;
                    }
                }
            }
        }
    }
}
