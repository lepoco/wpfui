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
using System.Linq;
using System.Reflection;
using System.Windows;
using Wpf.Ui.Contracts;

namespace Wpf.Ui.Controls.Navigation;

public partial class NavigationView
{
    protected readonly List<string> Journal = new();
    protected readonly List<INavigationViewItem> NavigationStack = new();

    private readonly Dictionary<INavigationViewItem, INavigationViewItem[]> _complexNavigationStackHistory = new();

    private IServiceProvider? _serviceProvider;
    private IPageService? _pageService;

    private int _currentIndexInJournal;
    private bool _isBackwardsNavigated;

    /// <inheritdoc />
    public bool CanGoBack
        => Journal.Count > 1 && _currentIndexInJournal >= 0;

    /// <inheritdoc />
    public void SetPageService(IPageService pageService)
        => _pageService = pageService;

    /// <inheritdoc />
    public void SetServiceProvider(IServiceProvider serviceProvider)
        => _serviceProvider = serviceProvider;

    /// <inheritdoc />
    public virtual bool Navigate(Type pageType, object? dataContext = null)
    {
        if (!PageTypeNavigationViewsDictionary.TryGetValue(pageType, out var navigationViewItem))
            return false;

        return NavigateInternal(navigationViewItem, dataContext, true, true);
    }

    /// <inheritdoc />
    public virtual bool Navigate(string pageIdOrTargetTag, object? dataContext = null)
    {
        if (!PageIdOrTargetTagNavigationViewsDictionary.TryGetValue(pageIdOrTargetTag, out var navigationViewItem))
            return false;

        return NavigateInternal(navigationViewItem, dataContext, true, true);
    }

    /// <inheritdoc />
    public virtual bool ReplaceContent(Type? pageTypeToEmbed)
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
    public virtual bool ReplaceContent(UIElement pageInstanceToEmbed, object? dataContext = null)
    {
        UpdateContent(pageInstanceToEmbed, dataContext);

        return true;
    }

    /// <inheritdoc />
    public virtual bool GoForward()
    {
        if (Journal.Count <= 1)
            return false;

        _currentIndexInJournal += 1;

        if (_currentIndexInJournal > Journal.Count - 1)
            return false;

        return Navigate(Journal[_currentIndexInJournal]);
    }

    /// <inheritdoc />
    public virtual bool GoBack()
    {
        if (Journal.Count <= 1)
            return false;

        var itemId = Journal[Journal.Count - 2];
        _isBackwardsNavigated = true;
        return Navigate(itemId);
    }

    /// <inheritdoc />
    public virtual void ClearJournal()
    {
        Journal.Clear();
        NavigationStack.Clear();
        _complexNavigationStackHistory.Clear();
        _currentIndexInJournal = 0;
    }

    private bool NavigateInternal(INavigationViewItem viewItem, object? dataContext, bool notifyAboutUpdate, bool bringIntoView)
    {
        if (NavigationStack.Count > 0 && NavigationStack[NavigationStack.Count -1] == viewItem)
            return false;

        Debug.WriteLine($"DEBUG | {viewItem.Id} - {viewItem.TargetPageTag ?? "NO_TAG"} | NAVIGATED");

        RenderSelectedItemContent(viewItem, dataContext);

        if (!notifyAboutUpdate)
            return true;

        UpdateCurrentNavigationStackItem(viewItem);
        AddToJournal(viewItem);

        UpdateSelectionForMenuItem(viewItem);
        OnSelectionChanged();

        if (bringIntoView && viewItem is FrameworkElement frameworkElement)
        {
            frameworkElement.BringIntoView();
            frameworkElement.Focus(); // TODO: Element or content?
        }

        return true;
    }

    private void AddToJournal(INavigationViewItem viewItem)
    {
        Journal.Add(viewItem.Id);
        IsBackEnabled = CanGoBack;
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
        if (dataContext is not null && content is FrameworkElement frameworkViewContent)
            frameworkViewContent.DataContext = dataContext;

        NavigationViewContentPresenter.Navigate(content);
    }

    private void AddToNavigationStack(INavigationViewItem viewItem)
    {
        if (!NavigationStack.Contains(viewItem))
            NavigationStack.Add(viewItem);

        if (NavigationStack.Count > 1)
            AddToNavigationStackHistory(viewItem);

        SelectedItem = NavigationStack[NavigationStack.Count - 1];
    }

    private void UpdateCurrentNavigationStackItem(INavigationViewItem viewItem)
    {
        if (NavigationStack.Contains(viewItem))
            return;

        if (NavigationStack.Count == 0)
        {
            NavigationStack.Add(viewItem);
        }
        else
        {
            NavigationStack[0] = viewItem;
        }

        SelectedItem = NavigationStack[0];
        ClearNavigationStack(1);
    }

    private void RecreateBreadcrumbsFromHistory(INavigationViewItem item)
    {

    }

    private void AddToNavigationStackHistory(INavigationViewItem viewItem)
    {
        var lastItem = NavigationStack[NavigationStack.Count - 1];
        var startIndex = NavigationStack.IndexOf(viewItem);

        if (startIndex < 0)
            startIndex = 0;

        _complexNavigationStackHistory.Add(lastItem, new INavigationViewItem[NavigationStack.Count - 1 - startIndex]);

        int i = 0;
        for (int j = startIndex; j < NavigationStack.Count - 1; j++)
        {
            _complexNavigationStackHistory[lastItem][i] = NavigationStack[j];
            i++;
        }
    }

    private void ClearNavigationStack(int navigationStackItemIndex)
    {
        var navigationStackCount = NavigationStack.Count;
        var length = navigationStackCount - navigationStackItemIndex;

        if (length == 0)
            return;

        INavigationViewItem[] buffer;

#if NET6_0_OR_GREATER

        buffer = System.Buffers.ArrayPool<INavigationViewItem>.Shared.Rent(length);
#else
        buffer = new INavigationViewItem[length];
#endif

        int i = 0;
        for (int j = navigationStackItemIndex; j <= navigationStackCount - 1; j++)
        {
            buffer[i] = NavigationStack[j];
            i++;
        }

        for (var index = 0; index < length; index++)
        {
            var item = buffer[index];
            NavigationStack.Remove(item);
        }
        
#if NET6_0_OR_GREATER
        System.Buffers.ArrayPool<INavigationViewItem>.Shared.Return(buffer, true);
#endif
    }

    private void ClearNavigationStack(INavigationViewItem item)
    {
        
    }

    private void UpdateSelectionForMenuItem(INavigationViewItem viewItem)
    {
        viewItem.IsActive = true;

        if (viewItem.Icon is SymbolIcon symbolIcon && PaneDisplayMode == NavigationViewPaneDisplayMode.LeftFluent)
            symbolIcon.Filled = true;

        if (Journal.Count == 1)
            return;

        if (!PageIdOrTargetTagNavigationViewsDictionary.TryGetValue(Journal[Journal.Count - 2], out var previousItem))
            return;

        previousItem.IsActive = false;

        if (previousItem.Icon is SymbolIcon previousSymbolIcon && PaneDisplayMode == NavigationViewPaneDisplayMode.LeftFluent)
            previousSymbolIcon.Filled = false;
    }
}
