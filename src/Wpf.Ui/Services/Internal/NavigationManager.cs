#nullable enable
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Toolkit.Diagnostics;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace Wpf.Ui.Services.Internal;

internal sealed class NavigationManager : IDisposable
{
    private readonly Frame _frame;
    private readonly INavigationItem[] _navigationItems;
    private readonly FrameworkElement?[] _instances;
    private readonly List<int> _history = new();
    private readonly IPageService? _pageService;
    private readonly ArrayPool<INavigationItem> _arrayPool = ArrayPool<INavigationItem>.Create();
    private readonly List<INavigationItem> _navigationStackHistory = new();

    private bool _isBackwardsNavigated;
    private bool _addToNavigationStack;

    public bool CanGoBack => _history.Count > 1;
    public readonly ObservableCollection<INavigationItem> NavigationStack = new();

    public NavigationManager(Frame frame, IPageService? pageService, INavigationItem[] navigationItems)
    {
        _instances = new FrameworkElement[navigationItems.Length];

        _navigationItems = navigationItems;
        _frame = frame;
        _pageService = pageService;
    }

    public void Dispose()
    {
        NavigationStack.Clear();
        _navigationStackHistory.Clear();
        ClearCache();
    }

    public void Preload()
    {
        //Why URI
        //Application.LoadComponent()

        ThrowHelper.ThrowNotSupportedException("Preloading currently not supported");
    }

    public void ClearCache()
    {
        for (int i = 0; i < _instances.Length; i++)
        {
            _instances[i] = null;
        }
    }

    public bool NavigateTo(string tag, object? dataContext = null)
    {
        Guard.IsNotNullOrEmpty(tag, nameof(tag));

        if (tag == "..")
        {
            return NavigateBack();
        }

        _addToNavigationStack = tag.Contains("/");
        if (_addToNavigationStack)
            tag = tag.Remove(0, 1);

        var itemId = GetItemId(item => item.PageTag == tag);
        if (itemId < 0)
            ThrowHelper.ThrowArgumentException($"Item with: {tag} tag not found");

        return NavigateInternal(itemId, dataContext);
    }

    public bool NavigateTo(Type type, object? dataContext = null)
    {
        var itemId = GetItemId(serviceItem => serviceItem.PageType == type);
        if (itemId < 0)
            ThrowHelper.ThrowArgumentException($"Item with: {type} type not found");

        return NavigateInternal(itemId, dataContext);
    }

    public void NavigateTo(int id, object? dataContext = null)
    {
        NavigateInternal(id, dataContext);
    }

    #region NavigationInternal

    private bool NavigateBack()
    {
        if (_history.Count <= 1)
            return false;

        var itemId = _history[_history.Count - 2];
        _isBackwardsNavigated = true;
        return NavigateInternal(itemId, null);
    }

    private bool NavigateInternal(int itemId, object? dataContext)
    {
        if (_navigationItems.ElementAtOrDefault(itemId) is not { } item)
            return false;

        switch (NavigationStack.Count)
        {
            case > 0 when NavigationStack[NavigationStack.Count -1] == item:
                return false;
            case 0:
                NavigationStack.Add(item);
                break;
        }

        AddToNavigationStack(item);
        ActivateItem(item);
        AddToHistory(itemId);

        PerformNavigation((itemId, item), dataContext);
        return true;
    }

    private void AddToNavigationStack(INavigationItem item)
    {
        if (_isBackwardsNavigated && item.WasInBreadcrumb)
        {
            if (_navigationStackHistory.Count > 1)
            {
                for (var i = 0; i < _navigationStackHistory.Count - 1; i++)
                {
                    _addToNavigationStack = true;

                    var historyItem = _navigationStackHistory[i];
                    historyItem.WasInBreadcrumb = false;
                    AddToNavigationStack(historyItem);
                }

                _navigationStackHistory.Clear();
            }

            item.WasInBreadcrumb = false;
            _addToNavigationStack = true;
        }

        if (_addToNavigationStack && !NavigationStack.Contains(item))
        {
            item.WasInBreadcrumb = true;
            NavigationStack.Add(item);
        }

        if (!item.IsHidden && !_addToNavigationStack)
        {
            NavigationStack[0].IsActive = false;
            NavigationStack[0] = item;

            ClearNavigationStack(1);
        }

        var navigationStackCount = NavigationStack.Count;
        if (navigationStackCount > 1)
        {
            var navItem = NavigationStack[NavigationStack.Count - 2];
            if (navItem.IsHidden)
                navItem.IsActive = false;

            var index = NavigationStack.IndexOf(item);
            if (index < navigationStackCount - 1 && _navigationStackHistory.Count == 0)
                ClearNavigationStack(++index);
        }

        _addToNavigationStack = false;
    }

    private void ActivateItem(INavigationItem item)
    {
        if (NavigationStack.Count > 1)
        {
            if (NavigationStack[NavigationStack.Count - 1].IsHidden)
                item.IsActive = true;
        }
        else
            item.IsActive = true;
    }

    private void AddToHistory(int itemId)
    {
        if (_isBackwardsNavigated)
        {
            _isBackwardsNavigated = false;
            _history.RemoveAt(_history.LastIndexOf(_history[_history.Count - 2]));
            _history.RemoveAt(_history.LastIndexOf(_history[_history.Count - 1]));
        }

        _history.Add(itemId);
    }

    #endregion

    #region PerformNavigation

    private void PerformNavigation((int itemId, INavigationItem item) itemData, object? dataContext)
    {
        if (_pageService is not null)
        {
            NavigateByService(itemData);
            return;
        }

        if (itemData.item.Cache)
        {
            NavigateWithCache(itemData, dataContext);
            return;
        }

        if (NavigateWithoutCache(itemData.item, dataContext) is not null)
            return;

        ThrowHelper.ThrowInvalidOperationException("failed to navigate");
    }

    private void NavigateByService((int itemId, INavigationItem item) itemData)
    {
        Guard.IsNotNull(itemData.item.PageType, nameof(itemData.item.PageType));

        /*if (_instances[itemData.itemId] is not null)
        {
            //TODO
        }*/

        var instance = _pageService!.GetPage(itemData.item.PageType);
        if (instance is null)
        {
            ThrowHelper.ThrowArgumentNullException("Failed to create instance");
            return;
        }

        _frame.Navigate(instance);
    }

    private void NavigateWithCache((int itemId, INavigationItem item) itemData, object? dataContext)
    {
        if (_instances[itemData.itemId] is null)
        {
            if (NavigateWithoutCache(itemData.item, dataContext) is not { } element)
            {
                ThrowHelper.ThrowArgumentNullException("Failed to create instance");
                return;
            }

            _instances[itemData.itemId] = element;
        }

        var instance = _instances[itemData.itemId]!;
        
        if (dataContext is not null)
            instance.DataContext = dataContext;

        _frame.Navigate(instance);

        System.Diagnostics.Debug.WriteLine(
            $"DEBUG | {itemData.item.PageTag} navigated internally, with cache by it's instance.");
    }

    private FrameworkElement? NavigateWithoutCache(INavigationItem item, object? dataContext)
    {
        FrameworkElement? instance = null;

        if (item.PageType is not null)
        {
            instance = NavigationServiceActivator.CreateInstance(item.PageType, dataContext);
            _frame.Navigate(instance);
        }

        if (item.AbsolutePageSource is not null)
        {
            _frame.Navigate(item.AbsolutePageSource);
        }

#if DEBUG
        if (instance is null)
            return instance;

        string navigationType = item.PageType is not null ? "type" : "source";

        System.Diagnostics.Debug.WriteLine(
            $"DEBUG | {item.PageTag} navigated internally, without cache by it's {navigationType}.");
#endif
        return instance;
    }

    #endregion

    #region PrivateMethods

    private void ClearNavigationStack(int navigationStackItemIndex)
    {
        var navigationStackCount = NavigationStack.Count;
        var length = navigationStackCount - navigationStackItemIndex;
        var buffer = _arrayPool.Rent(length);

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

            if (length > 1)
                _navigationStackHistory.Add(item);
        }

        _arrayPool.Return(buffer, true);
    }

    private int GetItemId(Func<INavigationItem, bool> prediction)
    {
        int selectedIndex = -1;

        for (int i = 0; i < _navigationItems.Length; i++)
        {
            if (!prediction.Invoke(_navigationItems[i])) continue;

            selectedIndex = i;
            break;
        }

        return selectedIndex;
    }

    #endregion
}
