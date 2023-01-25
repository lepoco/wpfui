// Based on Windows UI Library
// Copyright(c) Microsoft Corporation.All rights reserved.

// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using Wpf.Ui.Contracts;

namespace Wpf.Ui.Controls.Navigation;

public partial class NavigationView
{
    private readonly List<string> _journal = new();

    private IServiceProvider? _serviceProvider;
    private IPageService? _pageService;

    private int _currentIndexInJournal = 0;

    /// <inheritdoc />
    public bool CanGoBack
        => _journal.Count > 1 && _currentIndexInJournal >= 0;

    /// <inheritdoc />
    public void SetPageService(IPageService pageService)
        => _pageService = pageService;

    /// <inheritdoc />
    public void SetServiceProvider(IServiceProvider serviceProvider)
        => _serviceProvider = serviceProvider;

    /// <inheritdoc />
    public bool Navigate(Type pageType, object? dataContext = null)
    {
        if (!PageTypeNavigationViewsDictionary.TryGetValue(pageType, out var navigationViewItem))
            return false;

        return NavigateInternal(navigationViewItem, dataContext, true, true);
    }

    /// <inheritdoc />
    public bool Navigate(string pageIdOrTargetTag, object? dataContext = null)
    {
        if (!PageIdOrTargetTagNavigationViewsDictionary.TryGetValue(pageIdOrTargetTag, out var navigationViewItem))
            return false;

        return NavigateInternal(navigationViewItem, dataContext, true, true);
    }

    /// <inheritdoc />
    public bool ReplaceContent(Type? pageTypeToEmbed)
    {
        if (pageTypeToEmbed == null)
            return false;

        if (_serviceProvider != null)
        {
            UpdateContent(_serviceProvider.GetService(pageTypeToEmbed));

            return true;
        }

        if (_pageService == null)
            return false;

        UpdateContent(_pageService.GetPage(pageTypeToEmbed));

        return true;
    }

    /// <inheritdoc />
    public bool ReplaceContent(UIElement pageInstanceToEmbed, object? dataContext = null)
    {
        UpdateContent(pageInstanceToEmbed, dataContext);

        return true;
    }

    /// <inheritdoc />
    public bool GoForward()
    {
        if (_journal.Count <= 1)
            return false;

        _currentIndexInJournal += 1;

        if (_currentIndexInJournal > _journal.Count - 1)
            return false;

        return Navigate(_journal[_currentIndexInJournal]);
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

    /// <inheritdoc />
    public void ClearJournal()
    {
        _journal.Clear();
        _currentIndexInJournal = 0;
    }

    private bool NavigateInternal(INavigationViewItem viewItem, object? dataContext, bool notifyAboutUpdate, bool bringIntoView)
    {
        if (viewItem == SelectedItem)
            return false;

        UpdateJournal(viewItem);

        IsBackEnabled = _journal.Count > 0;

        Debug.WriteLine($"DEBUG | {viewItem.Id} - {viewItem.TargetPageTag ?? "NO_TAG"} | NAVIGATED");

        RenderSelectedItemContent(viewItem, dataContext);

        if (!notifyAboutUpdate)
            return true;

        SelectedItem = viewItem;

        UpdateSelectionForMenuItems(MenuItems);
        UpdateSelectionForMenuItems(FooterMenuItems);

        OnSelectionChanged();

        if (bringIntoView && viewItem is FrameworkElement frameworkElement)
        {
            frameworkElement.BringIntoView();
            frameworkElement.Focus(); // TODO: Element or content?
        }

        return true;
    }

    private void UpdateJournal(INavigationViewItem viewItem)
    {
#if DEBUG
        Debug.WriteLine($"JOURNAL INDEX {_currentIndexInJournal}");

        if (_journal.Count > 0)
            Debug.WriteLine($"JOURNAL LAST ELEMENT {_journal[_journal.Count - 1]}");
#endif

        if (_journal.Count == 0)
        {
            _currentIndexInJournal = 0;
            _journal.Add(viewItem.Id);

            return;
        }

        // TODO: Fix at last position

        if (_currentIndexInJournal == _journal.Count - 1)
        {
            _journal.Add(viewItem.Id);

            _currentIndexInJournal++;
        }

        if (_journal.Count > 20)
            _journal.RemoveAt(0);
    }

    private void RenderSelectedItemContent(INavigationViewItem viewItem, object? dataContext)
    {
        if (_serviceProvider != null)
        {
            if (viewItem.TargetPageType == null)
                return;

            UpdateContent(_serviceProvider.GetService(viewItem.TargetPageType) ?? null!, dataContext);

            return;
        }

        if (_pageService != null)
        {
            if (viewItem.TargetPageType == null)
                return;

            UpdateContent(_pageService.GetPage(viewItem.TargetPageType) ?? null!, dataContext);

            return;
        }

        if (viewItem.TargetPageType == null)
            return;

        var pageInstance = NavigationViewActivator.CreateInstance(viewItem.TargetPageType);

        if (pageInstance == null)
            return;

        UpdateContent(pageInstance, dataContext);
    }

    private void UpdateContent(object? content, object? dataContext = null)
    {
        if (dataContext != null && content is FrameworkElement frameworkViewContent)
            frameworkViewContent.DataContext = dataContext;

        NavigationViewContentPresenter.Navigate(content);
    }
}
