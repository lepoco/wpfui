#nullable enable
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Diagnostics;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace Wpf.Ui.Services.Internal;

internal sealed class NavigationManager : IDisposable
{
    private readonly Frame _frame;
    private readonly INavigationItem[] _navigationItems;
    private readonly FrameworkElement?[] _instances;
    private readonly IPageService? _pageService;
    private readonly ArrayPool<INavigationItem> _arrayPool = ArrayPool<INavigationItem>.Create();
    private readonly List<INavigationItem> _navigationStackHistory = new();

    private bool _isBackwardsNavigated;
    private bool _addToNavigationStack;
    private bool _addToHistory = true;

    public bool CanGoBack => History.Count > 1;
    public readonly List<int> History = new();
    public readonly ObservableCollection<INavigationItem> NavigationStack = new();

    public NavigationManager(Frame frame, IPageService? pageService, INavigationItem[] navigationItems)
    {
        _instances = new FrameworkElement[navigationItems.Length];

        _navigationItems = navigationItems;
        _frame = frame;
        _pageService = pageService;
    }

    #region Public methods

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

    public bool NavigateTo(string tag, bool addToHistory, object? dataContext = null)
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

    public bool NavigateTo(Type type, bool addToHistory, object? dataContext = null)
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

    #endregion

    #region NavigationInternal

    private bool NavigateBack()
    {
        if (History.Count <= 1)
            return false;

        var itemId = History[History.Count - 2];
        _isBackwardsNavigated = true;
        return NavigateInternal(itemId, null);
    }

    private bool NavigateInternal(int itemId, object? dataContext)
    {
        if (_navigationItems.ElementAtOrDefault(itemId) is not { } item)
            return false;

        var instance = GetFrameworkElement((itemId, item), dataContext);

        if (!CheckForNavigationCanceling(instance))
        {
            _addToNavigationStack = false;
            return false;
        }

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

        _frame.Navigate(instance);
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
            History.RemoveAt(History.LastIndexOf(History[History.Count - 2]));
            History.RemoveAt(History.LastIndexOf(History[History.Count - 1]));
        }

        if (_addToHistory)
            History.Add(itemId);

        _addToHistory = true;
    }

    private bool CheckForNavigationCanceling(FrameworkElement instance)
    {
        INavigationCancelable? navigationCancelable = instance switch
        {
            INavigationCancelable cancelable => cancelable,
            {DataContext: INavigationCancelable dataContextNavigationCancelable} => dataContextNavigationCancelable,
            _ => null
        };

        if (navigationCancelable is null)
            return true;

        var navigationFrom = History.Count > 0 ? _navigationItems[History[History.Count - 1]] : null;
        return navigationCancelable.CouldNavigate(navigationFrom);
    }

    #endregion

    #region PrivateMethods

    private FrameworkElement GetFrameworkElement((int itemId, INavigationItem item) itemData, object? dataContext)
    {
        Guard.IsNotNull(itemData.item.PageType, nameof(itemData.item.PageType));

        if (_pageService is not null && _pageService!.GetPage(itemData.item.PageType) is { } fromServicesElement)
            return fromServicesElement;


        if (itemData.item.Cache)
            return GetFrameworkElementFromCache(itemData, dataContext);

        if (!itemData.item.Cache && NavigationServiceActivator.CreateInstance(itemData.item.PageType, dataContext) is { } element)
        {
            if (dataContext is not null)
                element.DataContext = dataContext;

            return element;
        }

        ThrowHelper.ThrowArgumentException("Failed to create instance");
        return null;
    }

    private FrameworkElement GetFrameworkElementFromCache((int itemId, INavigationItem item) itemData, object? dataContext)
    {
        if (_instances[itemData.itemId] is not null)
            return _instances[itemData.itemId]!;

        if (NavigationServiceActivator.CreateInstance(itemData.item.PageType, dataContext) is not { } element)
        {
            ThrowHelper.ThrowArgumentNullException("Failed to create instance");
            return null;
        }

        if (dataContext is not null)
            element.DataContext = dataContext;

        _instances[itemData.itemId] = element;
        return _instances[itemData.itemId]!;
    }

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
