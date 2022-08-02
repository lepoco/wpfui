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
    private readonly FrameworkElement[] _instances;
    private readonly List<int> _history = new();
    private readonly IPageService? _pageService;
    private readonly ArrayPool<INavigationItem> _arrayPool = ArrayPool<INavigationItem>.Create();

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

    }

    public void NavigateTo(string tag, object? dataContext = null)
    {
        Guard.IsNotNullOrEmpty(tag, nameof(tag));

        if (tag == "..")
        {
            NavigateBack();
            return;
        }

        _addToNavigationStack = tag.Contains("//");
        if (_addToNavigationStack)
            tag = tag.Replace("//", string.Empty).Trim();

        var itemId = GetItemId(item => item.PageTag == tag);
        if (itemId < 0)
            ThrowHelper.ThrowArgumentException($"Item with: {tag} tag not found");

        NavigateInternal(itemId, dataContext);
    }

    public void NavigateTo(Type type, object? dataContext = null)
    {
        var itemId = GetItemId(serviceItem => serviceItem.PageType == type);
        if (itemId < 0)
            ThrowHelper.ThrowArgumentException($"Item with: {type} type not found");

        NavigateInternal(itemId, dataContext);
    }

    public void NavigateTo(int id, object? dataContext = null)
    {
        NavigateInternal(id, dataContext);
    }

    private void NavigateBack()
    {
        if (_history.Count <= 1)
            return;

        var itemId = _history[_history.Count - 2];
        _isBackwardsNavigated = true;
        NavigateInternal(itemId, null);
    }

    private void NavigateInternal(int itemId, object? dataContext)
    {
        if (_navigationItems.ElementAtOrDefault(itemId) is not { } item)
            return;

        switch (NavigationStack.Count)
        {
            case > 0 when NavigationStack[NavigationStack.Count -1] == item:
                return;
            case 0:
                NavigationStack.Add(item);
                break;
        }

        AddToNavigationStack(item);

        item.IsActive = true;

        if (_isBackwardsNavigated)
        {
            _isBackwardsNavigated = false;
            _history.RemoveAt(_history.LastIndexOf(_history[_history.Count - 2]));
            _history.RemoveAt(_history.LastIndexOf(_history[_history.Count - 1]));
        }

        _history.Add(itemId);

        PerformNavigation((itemId, item), dataContext);
    }

    private void AddToNavigationStack(INavigationItem item)
    {
        if (_addToNavigationStack && !NavigationStack.Contains(item))
            NavigationStack.Add(item);

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
            if (index < navigationStackCount - 1)
                ClearNavigationStack(++index);
        }
    }

    #region PerformNavigation

    private void PerformNavigation((int itemId, INavigationItem item) itemData, object? dataContext)
    {
        if (_pageService is not null)
        {
            NavigateByService(itemData.item);
            return;
        }

        if (itemData.item.Cache)
        {
            NavigateWithCache(itemData, dataContext);
            return;
        }

        NavigateWithoutCache(itemData.item, dataContext);
    }

    private void NavigateByService(INavigationItem item)
    {
        var instance = _pageService!.GetPage(item.PageType);
        Guard.IsNotNull(instance, "Page instance");

        _frame.Navigate(instance);
    }

    private void NavigateWithCache((int itemId, INavigationItem item) itemData, object? dataContext)
    {

    }

    private void NavigateWithoutCache(INavigationItem item, object? dataContext)
    {

    }

    #endregion

    private void ClearNavigationStack(int navigationStackItemIndex)
    {
        var navigationStackCount = NavigationStack.Count;
        var buffer = _arrayPool.Rent(navigationStackCount - navigationStackItemIndex);

        int i = 0;
        for (int j = navigationStackItemIndex; j <= navigationStackCount - 1; j++)
        {
            buffer[i] = NavigationStack[j];
            i++;
        }

        foreach (var item in buffer)
            NavigationStack.Remove(item);

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
}
