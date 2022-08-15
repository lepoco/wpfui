using System;
using System.Buffers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Wpf.Ui.Controls.Interfaces;

namespace Wpf.Ui.Services.Internal;

internal sealed class NavigationStackManager : IDisposable
{
    private readonly ArrayPool<INavigationItem> _arrayPool = ArrayPool<INavigationItem>.Create();
    private readonly List<INavigationItem> _navigationStackHistory = new();

    public readonly ObservableCollection<INavigationItem> NavigationStack = new();

    public void Dispose()
    {
        NavigationStack.Clear();
        _navigationStackHistory.Clear();
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
        if (item.IsHidden)
            return;

        NavigationStack[0].IsActive = false;
        NavigationStack[0] = item;

        ClearNavigationStack(1);
    }

    private void RecreateBreadcrumbsFromHistory(INavigationItem item)
    {
        if (!item.WasInBreadcrumb && _navigationStackHistory.Count < 1)
            return;

        for (var i = 0; i < _navigationStackHistory.Count - 1; i++)
        {
            var historyItem = _navigationStackHistory[i];
            AddToNavigationStack(historyItem, true, false);
            historyItem.WasInBreadcrumb = false;
        }

        _navigationStackHistory.Clear();
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
        if (index < navigationStackCount - 1 && _navigationStackHistory.Count == 0)
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

            if (length > 1)
                _navigationStackHistory.Add(item);
        }

        _arrayPool.Return(buffer, true);
    }
}
