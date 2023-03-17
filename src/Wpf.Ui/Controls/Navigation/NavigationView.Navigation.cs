// Based on Windows UI Library
// Copyright(c) Microsoft Corporation.All rights reserved.

// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using Wpf.Ui.Contracts;

namespace Wpf.Ui.Controls.Navigation;

public partial class NavigationView
{
    protected readonly List<string> Journal = new(50);
    protected readonly ObservableCollection<INavigationViewItem> NavigationStack = new();

    private readonly Dictionary<INavigationViewItem, List<INavigationViewItem?[]>> _complexNavigationStackHistory = new();

    private IServiceProvider? _serviceProvider;
    private IPageService? _pageService;

    private int _currentIndexInJournal;

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
            return TryToNavigateWithoutINavigationViewItem(pageType, false, dataContext);

        return NavigateInternal(navigationViewItem, dataContext);
    }

    /// <inheritdoc />
    public virtual bool Navigate(string pageIdOrTargetTag, object? dataContext = null)
    {
        if (!PageIdOrTargetTagNavigationViewsDictionary.TryGetValue(pageIdOrTargetTag, out var navigationViewItem))
            return false;

        return NavigateInternal(navigationViewItem, dataContext);
    }

    /// <inheritdoc />
    public virtual bool NavigateWithHierarchy(Type pageType, object? dataContext = null)
    {
        if (!PageTypeNavigationViewsDictionary.TryGetValue(pageType, out var navigationViewItem))
            return TryToNavigateWithoutINavigationViewItem(pageType, true, dataContext);

        return NavigateInternal(navigationViewItem, dataContext, true);
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
        throw new NotImplementedException();

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

        var itemId = Journal[^2];

        OnBackRequested();
        return NavigateInternal(PageIdOrTargetTagNavigationViewsDictionary[itemId], null, false, true);
    }

    /// <inheritdoc />
    public virtual void ClearJournal()
    {
        _currentIndexInJournal = 0;

        Journal.Clear();
        _complexNavigationStackHistory.Clear();
    }

    private bool TryToNavigateWithoutINavigationViewItem(Type pageType, bool addToNavigationStack, object? dataContext = null)
    {
        var navigationViewItem = new NavigationViewItem(pageType);

        if (!NavigateInternal(navigationViewItem, dataContext, addToNavigationStack))
            return false;

        PageTypeNavigationViewsDictionary.Add(pageType, navigationViewItem);
        PageIdOrTargetTagNavigationViewsDictionary.Add(navigationViewItem.Id, navigationViewItem);

        return true;
    }

    private bool NavigateInternal(INavigationViewItem viewItem, object? dataContext = null, bool addToNavigationStack = false, bool isBackwardsNavigated = false)
    {
        if (NavigationStack.Count > 0 && NavigationStack[^1] == viewItem)
            return false;

        Debug.WriteLineIf(EnableDebugMessages, $"DEBUG | {viewItem.Id} - {(string.IsNullOrEmpty(viewItem.TargetPageTag) ? "NO_TAG" : viewItem.TargetPageTag)} - {viewItem.TargetPageType} | NAVIGATED");

        var pageInstance = GetNavigationItemInstance(viewItem);

        if (OnNavigating(pageInstance))
        {
            Debug.WriteLineIf(EnableDebugMessages, "Navigation canceled");
            return false;
        }

        ApplyAttachedProperties(viewItem, pageInstance);
        UpdateContent(pageInstance, dataContext);

        AddToNavigationStack(viewItem, addToNavigationStack, isBackwardsNavigated);
        AddToJournal(viewItem, isBackwardsNavigated);

        if (SelectedItem != NavigationStack[0] && NavigationStack[0].IsMenuElement)
        {
            SelectedItem = NavigationStack[0];
            OnSelectionChanged();
        }
        
        return true;
    }

    private void AddToJournal(INavigationViewItem viewItem, bool isBackwardsNavigated)
    {
        if (isBackwardsNavigated)
        {
            Journal.RemoveAt(Journal.LastIndexOf(Journal[^2]));
            Journal.RemoveAt(Journal.LastIndexOf(Journal[^1]));

            _currentIndexInJournal -= 2;
        }

        Journal.Add(viewItem.Id);
        _currentIndexInJournal++;

        IsBackEnabled = CanGoBack;

#if DEBUG
        Debug.WriteLineIf(EnableDebugMessages, $"JOURNAL INDEX {_currentIndexInJournal}");
        if (Journal.Count > 0)
            Debug.WriteLineIf(EnableDebugMessages, $"JOURNAL LAST ELEMENT {Journal[^1]}");
#endif
    }

    private object GetNavigationItemInstance(INavigationViewItem viewItem)
    {
        if (viewItem.TargetPageType is null)
            throw new ArgumentNullException(nameof(viewItem.TargetPageType));

        if (_serviceProvider is not null)
        {
            return _serviceProvider.GetService(viewItem.TargetPageType) ??
                   new ArgumentNullException($"{nameof(_serviceProvider.GetService)} returned null");
        }

        if (_pageService is not null)
        {
            return _pageService.GetPage(viewItem.TargetPageType) ??
                   throw new ArgumentNullException($"{nameof(_pageService.GetPage)} returned null");
        }

        return NavigationViewActivator.CreateInstance(viewItem.TargetPageType) ??
               throw new ArgumentException("Failed to create instance of the page");
    }

    private static void ApplyAttachedProperties(INavigationViewItem viewItem, object pageInstance)
    {
        if (pageInstance is FrameworkElement frameworkElement && GetHeaderContent(frameworkElement) is {} headerContent)
            viewItem.Content = headerContent;
    }

    private void UpdateContent(object? content, object? dataContext = null)
    {
        if (dataContext is not null && content is FrameworkElement frameworkViewContent)
            frameworkViewContent.DataContext = dataContext;

        NavigationViewContentPresenter.Navigate(content);
    }

    #region Navigation stack methods

    private void AddToNavigationStack(INavigationViewItem viewItem, bool addToNavigationStack, bool isBackwardsNavigated)
    {
        if (isBackwardsNavigated)
            RecreateNavigationStackFromHistory(viewItem);

        if (addToNavigationStack && !NavigationStack.Contains(viewItem))
        {
            viewItem.Activate(this);
            NavigationStack.Add(viewItem);
        }

        if (!addToNavigationStack)
            UpdateCurrentNavigationStackItem(viewItem);

        ClearNavigationStack(viewItem);
    }

    private void UpdateCurrentNavigationStackItem(INavigationViewItem viewItem)
    {
        if (NavigationStack.Contains(viewItem))
            return;

        if (NavigationStack.Count > 1)
            AddToNavigationStackHistory(viewItem);

        if (NavigationStack.Count == 0)
        {
            viewItem.Activate(this);
            NavigationStack.Add(viewItem);
        }
        else
        {
            ReplaceThirstElementInNavigationStack(viewItem);
        }

        ClearNavigationStack(1);
    }

    private void RecreateNavigationStackFromHistory(INavigationViewItem item)
    {
        if (!_complexNavigationStackHistory.TryGetValue(item, out var historyList) || historyList.Count == 0)
            return;

        var latestHistory = historyList[^1];
        var startIndex = 0;

        if (latestHistory[0]!.IsMenuElement)
        {
            startIndex = 1;
            ReplaceThirstElementInNavigationStack(latestHistory[0]!);
        }

        for (int i = startIndex; i < latestHistory.Length; i++)
        {
            if (latestHistory[i] is null)
                break;

            AddToNavigationStack(latestHistory[i]!, true, false);
        }

        historyList.Remove(latestHistory);
        /*if (historyList.Count == 0)
            _complexNavigationStackHistory.Remove(item);
        */

#if NET6_0_OR_GREATER
        System.Buffers.ArrayPool<INavigationViewItem>.Shared.Return(latestHistory!, true);
#endif

        AddToNavigationStack(item, true, false);
    }

    private void AddToNavigationStackHistory(INavigationViewItem viewItem)
    {
        var lastItem = NavigationStack[^1];
        var startIndex = NavigationStack.IndexOf(viewItem);

        if (startIndex < 0)
            startIndex = 0;
        
        if (!_complexNavigationStackHistory.TryGetValue(lastItem, out var historyList))
        {
            historyList = new List<INavigationViewItem?[]>(5);
            _complexNavigationStackHistory.Add(lastItem,  historyList);
        }

        int arrayLength = NavigationStack.Count - 1 - startIndex;
        INavigationViewItem[] array;

        //Initializing an array every time well... not an ideal

#if NET6_0_OR_GREATER
        array = System.Buffers.ArrayPool<INavigationViewItem>.Shared.Rent(arrayLength);
#else
        array = new INavigationViewItem[arrayLength];
#endif

        historyList.Add(array);
        var latestHistory = historyList[^1];

        int i = 0;
        for (int j = startIndex; j < NavigationStack.Count - 1; j++)
        {
            latestHistory[i] = NavigationStack[j];
            i++;
        }
    }

    private void ClearNavigationStack(int navigationStackItemIndex)
    {
        var navigationStackCount = NavigationStack.Count;
        var length = navigationStackCount - navigationStackItemIndex;

        if (length == 0)
            return;

        for (int j = navigationStackCount - 1; j >= navigationStackCount - length; j--)
        {
            NavigationStack.Remove(NavigationStack[j]);
        }
    }

    private void ClearNavigationStack(INavigationViewItem item)
    {
        var navigationStackCount = NavigationStack.Count;
        if (navigationStackCount <= 1)
            return;

        var index = NavigationStack.IndexOf(item);
        if (index >= navigationStackCount - 1)
            return;

        AddToNavigationStackHistory(item);
        ClearNavigationStack(++index);
    }

    private void ReplaceThirstElementInNavigationStack(INavigationViewItem newItem)
    {
        NavigationStack[0].Deactivate(this);
        NavigationStack[0] = newItem;
        NavigationStack[0].Activate(this);
    }

    #endregion
}
