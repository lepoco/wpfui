#nullable enable
using System;
using System.Collections.Generic;
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
    private readonly List<int> _navigationStack = new();
    private readonly IPageService? _pageService;

    private bool _isBackwardsNavigated;
    private int _previousItemId = -1;

    public bool CanGoBack => _history.Count > 1;
    public INavigationItem Current { get; private set; } = null!;

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

        var itemId = GetItemId(item => item.PageTag == tag);
        NavigateInternal(itemId, dataContext);
    }

    public void NavigateTo(Type type, object? dataContext = null)
    {
        var itemId = GetItemId(serviceItem => serviceItem.PageType == type);
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
        if (ReferenceEquals(Current, _navigationItems.ElementAtOrDefault(itemId)))
            return;

        if (_isBackwardsNavigated)
        {
            _isBackwardsNavigated = false;
            _history.RemoveAt(_history.LastIndexOf(_history[_history.Count - 2]));
            _history.RemoveAt(_history.LastIndexOf(_history[_history.Count - 1]));
        }

        _history.Add(itemId);

        PerformNavigation(itemId, dataContext);
    }

    #region PerformNavigation

    private void PerformNavigation(int itemId, object? dataContext)
    {
        if (_navigationItems.ElementAtOrDefault(itemId) is not { } item)
            return;

        if (_navigationItems.ElementAtOrDefault(_previousItemId) is {} previousItem)
            previousItem.IsActive = false;

        item.IsActive = true;
        Current = item;
        _previousItemId = itemId;

        if (_pageService is not null)
        {
            NavigateByService(item);
            return;
        }

        if (item.Cache)
        {
            NavigateWithCache(itemId, item, dataContext);
            return;
        }

        NavigateWithoutCache(item, dataContext);
    }

    private void NavigateByService(INavigationItem item)
    {
        var instance = _pageService!.GetPage(item.PageType);
        Guard.IsNotNull(instance, "Page instance");

        _frame.Navigate(instance);
    }

    private void NavigateWithCache(int itemId, INavigationItem item, object? dataContext)
    {

    }

    private void NavigateWithoutCache(INavigationItem item, object? dataContext)
    {

    }

    #endregion

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
