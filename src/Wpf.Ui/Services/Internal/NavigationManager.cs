#nullable enable
using System;
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
    private readonly NavigationStackManager _navigationStackManager;
    private readonly Frame _frame;
    private readonly FrameworkElement?[] _instances;
    private readonly IPageService? _pageService;

    private bool _isBackwardsNavigated;
    private bool _addToNavigationStack;

    public bool CanGoBack => History.Count > 1;
    public readonly List<int> History = new();
    public ObservableCollection<INavigationItem> NavigationStack => _navigationStackManager.NavigationStack;
    public readonly INavigationItem[] NavigationItems;

    public NavigationManager(Frame frame, IPageService? pageService, INavigationItem[] navigationItems)
    {
        _instances = new FrameworkElement[navigationItems.Length];

        NavigationItems = navigationItems;
        _frame = frame;
        _pageService = pageService;

        _navigationStackManager = new NavigationStackManager(this);
    }

    #region Public methods

    public void Dispose()
    {
        _navigationStackManager.Dispose();
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

    public int GetItemId(Func<INavigationItem, bool> prediction)
    {
        int selectedIndex = -1;

        for (int i = 0; i < NavigationItems.Length; i++)
        {
            if (!prediction.Invoke(NavigationItems[i])) continue;

            selectedIndex = i;
            break;
        }

        return selectedIndex;
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
        if (NavigationItems.ElementAtOrDefault(itemId) is not { } item)
            return false;

        var instance = GetFrameworkElement((itemId, item), dataContext);

        if (!CheckForNavigationCanceling(instance))
        {
            _addToNavigationStack = false;
            return false;
        }

        if (!_navigationStackManager.AddFirstItemAndCheckIfNavigatingToCurrentItem(item))
            return false;

        _navigationStackManager.AddToNavigationStack(item, _addToNavigationStack, _isBackwardsNavigated);
        ActivateItem(item);
        AddToHistory(itemId);

        _frame.Navigate(instance);
        _addToNavigationStack = false;
        return true;
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

        History.Add(itemId);
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

        var navigationFrom = History.Count > 0 ? NavigationItems[History[History.Count - 1]] : null;
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

    #endregion
}
