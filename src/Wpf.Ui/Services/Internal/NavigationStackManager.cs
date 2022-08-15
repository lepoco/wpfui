using System;
using System.Buffers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Wpf.Ui.Controls.Interfaces;

namespace Wpf.Ui.Services.Internal;

internal sealed class NavigationStackManager : IDisposable
{
    private readonly NavigationManager _navigationManager;
    private readonly ArrayPool<INavigationItem> _arrayPool = ArrayPool<INavigationItem>.Create();
    private readonly Dictionary<INavigationItem, INavigationItem[]> _complexHistory = new();
    public readonly ObservableCollection<INavigationItem> NavigationStack = new();

    public NavigationStackManager(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
    }

    public void Dispose()
    {
        NavigationStack.Clear();
        _complexHistory.Clear();
    }

    public bool AddFirstItemAndCheckIfNavigatingToCurrentItem(INavigationItem item)
    {
        switch (NavigationStack.Count)
        {
            case > 0 when NavigationStack[NavigationStack.Count -1] == item:
                return false;
            case 0:
                NavigationStack.Add(item);
                return true;
        }

        return true;
    }

    public void AddToNavigationStack(INavigationItem item, bool addToNavigationStack, bool isBackwardsNavigated)
    {
        if (isBackwardsNavigated)
            RecreateBreadcrumbsFromHistory(item);

        if (addToNavigationStack && !NavigationStack.Contains(item))
        {
            item.WasInBreadcrumb = true;
            NavigationStack.Add(item);
        }

        if (!addToNavigationStack)
            UpdateCurrentItem(item);

        ClearNavigationStack(item);
    }

    private void UpdateCurrentItem(INavigationItem item)
    {
        if (item.IsHidden || NavigationStack.Contains(item))
            return;

        if (NavigationStack.Count > 1)
            AddToHistory(item);

        NavigationStack[0].IsActive = false;
        NavigationStack[0] = item;

        ClearNavigationStack(1);
    }

    private void RecreateBreadcrumbsFromHistory(INavigationItem item)
    {
        if (!item.WasInBreadcrumb && !_complexHistory.ContainsKey(item))
            return;

        var history = _complexHistory[item];

        var startIndex = 0;

        var index = _navigationManager.GetItemId(navigationItem => navigationItem == history[0]);
        if (index > 0 && !history[0].IsHidden)
        {
            startIndex = 1;
            NavigationStack[0].IsActive = false;
            NavigationStack[0] = history[0];
        }

        for (var i = startIndex; i < history.Length; i++)
        {
            var historyItem = history[i];
            AddToNavigationStack(historyItem, true, false);
            historyItem.WasInBreadcrumb = false;
        }

        _complexHistory.Remove(item);
        AddToNavigationStack(item, true, false);
    }

    private void ClearNavigationStack(INavigationItem item)
    {
        var navigationStackCount = NavigationStack.Count;
        if (navigationStackCount <= 1)
            return;

        var navItem = NavigationStack[NavigationStack.Count - 2];
        if (navItem.IsHidden)
            navItem.IsActive = false;

        var index = NavigationStack.IndexOf(item);
        if (index >= navigationStackCount - 1 || _complexHistory.ContainsKey(item))
            return;

        AddToHistory(item);

        ClearNavigationStack(++index);
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
        }

        _arrayPool.Return(buffer, true);
    }

    private void AddToHistory(INavigationItem item)
    {
        var lastItem = NavigationStack[NavigationStack.Count - 1];
        var startIndex = NavigationStack.IndexOf(item);
        if (startIndex < 0)
            startIndex = 0;

        if (_complexHistory.ContainsKey(lastItem))
            _complexHistory.Remove(lastItem);

        _complexHistory.Add(lastItem, new INavigationItem[NavigationStack.Count - 1 - startIndex]);

        int i = 0;
        for (int j = startIndex; j < NavigationStack.Count - 1; j++)
        {
            _complexHistory[lastItem][i] = NavigationStack[j];
            i++;
        }
    }
}
