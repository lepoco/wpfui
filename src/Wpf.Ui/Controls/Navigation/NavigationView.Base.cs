// Based on Windows UI Library
// Copyright(c) Microsoft Corporation.All rights reserved.

// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Wpf.Ui.Controls.BreadcrumbControl;
using Wpf.Ui.Controls.AutoSuggestBoxControl;

namespace Wpf.Ui.Controls.Navigation;

// https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.navigationview?view=winrt-22621

/// <summary>
/// Represents a container that enables navigation of app content. It has a header, a view for the main content, and a menu pane for navigation commands.
/// </summary>
[ToolboxItem(true)]
[System.Drawing.ToolboxBitmap(typeof(NavigationView), "NavigationView.bmp")]
public partial class NavigationView : System.Windows.Controls.Control, INavigationView
{
    /// <summary>
    /// Static constructor which overrides default property metadata.
    /// </summary>
    static NavigationView()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationView), new FrameworkPropertyMetadata(typeof(NavigationView)));
        MarginProperty.OverrideMetadata(typeof(NavigationView),
            new FrameworkPropertyMetadata(new Thickness(0, 0, 0, 0)));
    }

    public NavigationView()
    {
        NavigationParent = this;

        //It really should be here
        MenuItems = new ObservableCollection<object>();
        FooterMenuItems = new ObservableCollection<object>();

        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
        SizeChanged += OnSizeChanged;
    }

    /// <inheritdoc/>
    public INavigationViewItem? SelectedItem { get; protected set; }

    protected Dictionary<string, INavigationViewItem> PageIdOrTargetTagNavigationViewsDictionary = new();
    protected Dictionary<Type, INavigationViewItem> PageTypeNavigationViewsDictionary = new();

    private readonly ObservableCollection<string> _autoSuggestBoxItems = new();
    private readonly ObservableCollection<NavigationViewBreadcrumbItem> _breadcrumbBarItems = new();

    private static readonly Thickness s_titleBarPaneOpenMargin = new(35, 0, 0, 0);
    private static readonly Thickness s_titleBarPaneCompactMargin = new(55, 0, 0, 0);
    private static readonly Thickness s_autoSuggestBoxMargin = new(8, 8, 8, 16);
    private static readonly Thickness s_frameMargin = new(0, 50, 0, 0);

    /// <inheritdoc />
    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);

        NavigationStack.CollectionChanged += NavigationStackOnCollectionChanged;

        InvalidateArrange();
        InvalidateVisual();
        UpdateLayout();

        UpdateAutoSuggestBoxSuggestions();

        AddItemsToDictionaries();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        // TODO: Refresh
    }

    /// <summary>
    /// This virtual method is called when this element is detached form a loaded tree.
    /// </summary>
    protected virtual void OnUnloaded(object sender, RoutedEventArgs e)
    {
        Loaded -= OnLoaded;
        Unloaded -= OnUnloaded;
        SizeChanged -= OnSizeChanged;

        NavigationStack.CollectionChanged -= NavigationStackOnCollectionChanged;

        PageIdOrTargetTagNavigationViewsDictionary.Clear();
        PageTypeNavigationViewsDictionary.Clear();

        ClearJournal();

        if (AutoSuggestBox is not null)
        {
            AutoSuggestBox.SuggestionChosen -= AutoSuggestBoxOnSuggestionChosen;
            AutoSuggestBox.QuerySubmitted -= AutoSuggestBoxOnQuerySubmitted;
        }

        if (Header is BreadcrumbBar breadcrumbBar)
            breadcrumbBar.ItemClicked -= BreadcrumbBarOnItemClicked;

        if (ToggleButton is not null)
            ToggleButton.Click -= OnToggleButtonClick;

        if (BackButton is not null)
            BackButton.Click -= OnToggleButtonClick;

        if (AutoSuggestBoxSymbolButton is not null)
            AutoSuggestBoxSymbolButton.Click -= AutoSuggestBoxSymbolButtonOnClick;
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        //Back button
        if (e.ChangedButton is MouseButton.XButton1)
        {
            GoBack();
            e.Handled = true;
        }

        base.OnMouseDown(e);
    }

    /// <summary>
    /// This virtual method is called when ActualWidth or ActualHeight (or both) changed.
    /// </summary>
    protected virtual void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        // TODO: Update reveal
    }

    /// <summary>
    /// This virtual method is called when <see cref="BackButton"/> is clicked.
    /// </summary>
    protected virtual void OnBackButtonClick(object sender, RoutedEventArgs e)
    {
        GoBack();
    }

    /// <summary>
    /// This virtual method is called when <see cref="ToggleButton"/> is clicked.
    /// </summary>
    protected virtual void OnToggleButtonClick(object sender, RoutedEventArgs e)
    {
        IsPaneOpen = !IsPaneOpen;
    }

    /// <summary>
    /// This virtual method is called when <see cref="AutoSuggestBoxSymbolButton"/> is clicked.
    /// </summary>
    protected virtual void AutoSuggestBoxSymbolButtonOnClick(object sender, RoutedEventArgs e)
    {
        IsPaneOpen = !IsPaneOpen;
        AutoSuggestBox?.Focus();
    }

    /// <summary>
    /// This virtual method is called when <see cref="PaneDisplayMode"/> is changed.
    /// </summary>
    protected virtual void OnPaneDisplayModeChanged()
    {
        switch (PaneDisplayMode)
        {
            case NavigationViewPaneDisplayMode.LeftFluent:
                IsBackButtonVisible = NavigationViewBackButtonVisible.Collapsed;
                IsPaneToggleVisible = false;
                break;
        }
    }

    /// <summary>
    /// This virtual method is called when <see cref="ItemTemplate"/> is changed.
    /// </summary>
    protected virtual void OnItemTemplateChanged()
    {
        UpdateMenuItemsTemplate();
    }

    internal void ToggleAllExpands()
    {
        // TODO: When shift clicked on navigationviewitem
    }

    internal void OnNavigationViewItemClick(NavigationViewItem navigationViewItem)
    {
        OnItemInvoked();

        NavigateInternal(navigationViewItem);
    }

    protected virtual void BreadcrumbBarOnItemClicked(BreadcrumbBar sender, BreadcrumbBarItemClickedEventArgs e)
    {
        var item = (NavigationViewBreadcrumbItem)e.Item;
        Navigate(item.PageId);
    }

    private void UpdateAutoSuggestBoxSuggestions()
    {
        if (AutoSuggestBox == null)
            return;

        _autoSuggestBoxItems.Clear();

        AddItemsToAutoSuggestBoxItems();
    }

    /// <summary>
    /// Navigate to the page after its name is selected in <see cref="AutoSuggestBox"/>.
    /// </summary>
    private void AutoSuggestBoxOnSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        if (sender.IsSuggestionListOpen)
            return;

        if (args.SelectedItem is not string selectedSuggestBoxItem)
            return;

        if (NavigateToMenuItemFromAutoSuggestBox(MenuItems, selectedSuggestBoxItem))
            return;

        NavigateToMenuItemFromAutoSuggestBox(FooterMenuItems, selectedSuggestBoxItem);
    }

    private void AutoSuggestBoxOnQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        var suggestions = new List<string>();
        var querySplit = args.QueryText.Split(' ');

        foreach (var item in _autoSuggestBoxItems)
        {
            bool isMatch = true;

            foreach (string queryToken in querySplit)
            {
                if (item.IndexOf(queryToken, StringComparison.CurrentCultureIgnoreCase) < 0)
                    isMatch = false;
            }

            if (isMatch)
                suggestions.Add(item);
        }

        if (suggestions.Count <= 0)
            return;

        var element = suggestions.First();

        if (NavigateToMenuItemFromAutoSuggestBox(MenuItems, element))
            return;

        NavigateToMenuItemFromAutoSuggestBox(FooterMenuItems, element);
    }

    protected virtual void AddItemsToDictionaries(IList list)
    {
        for (var i = 0; i < list.Count; i++)
        {
            var singleMenuItem = list[i];

            if (singleMenuItem is not INavigationViewItem singleNavigationViewItem)
                continue;

            if (!PageIdOrTargetTagNavigationViewsDictionary.ContainsKey(singleNavigationViewItem.Id))
            {
                PageIdOrTargetTagNavigationViewsDictionary.Add(singleNavigationViewItem.Id, singleNavigationViewItem);
            }

            if (!PageIdOrTargetTagNavigationViewsDictionary.ContainsKey(singleNavigationViewItem.TargetPageTag))
            {
                PageIdOrTargetTagNavigationViewsDictionary.Add(singleNavigationViewItem.TargetPageTag, singleNavigationViewItem);
            }

            if (singleNavigationViewItem.TargetPageType is not null && !PageTypeNavigationViewsDictionary.ContainsKey(singleNavigationViewItem.TargetPageType))
            {
                PageTypeNavigationViewsDictionary.Add(singleNavigationViewItem.TargetPageType, singleNavigationViewItem);
            }

            singleNavigationViewItem.IsMenuElement = true;

            if (singleNavigationViewItem.MenuItems.Count <= 0)
                continue;

            AddItemsToDictionaries(singleNavigationViewItem.MenuItems);
        }
    }

    protected virtual void AddItemsToDictionaries()
    {
        AddItemsToDictionaries(MenuItems);
        AddItemsToDictionaries(FooterMenuItems);
    }

    protected virtual void AddItemsToAutoSuggestBoxItems(IList list)
    {
        for (var i = 0; i < list.Count; i++)
        {
            var singleMenuItem = list[i];

            if (singleMenuItem is not NavigationViewItem singleNavigationViewItem)
                continue;

            if (singleNavigationViewItem is { Content: string content, TargetPageType: { } } &&
                !string.IsNullOrWhiteSpace(content))
                _autoSuggestBoxItems.Add(content);

            if (singleNavigationViewItem.MenuItems.Count <= 0)
                continue;

            AddItemsToAutoSuggestBoxItems(singleNavigationViewItem.MenuItems);
        }
    }

    protected virtual void AddItemsToAutoSuggestBoxItems()
    {
        AddItemsToAutoSuggestBoxItems(MenuItems);
        AddItemsToAutoSuggestBoxItems(FooterMenuItems);
    }

    protected virtual bool NavigateToMenuItemFromAutoSuggestBox(IList list, string selectedSuggestBoxItem)
    {
        for (var i = 0; i < list.Count; i++)
        {
            var singleMenuItem = list[i];

            if (singleMenuItem is not NavigationViewItem singleNavigationViewItem)
                continue;

            if (singleNavigationViewItem.Content is string content && content == selectedSuggestBoxItem)
            {
                NavigateInternal(singleNavigationViewItem);
                singleNavigationViewItem.BringIntoView();
                singleNavigationViewItem.Focus(); // TODO: Element or content?

                return true;
            }

            if (singleNavigationViewItem.MenuItems.Count <= 0)
                continue;

            NavigateToMenuItemFromAutoSuggestBox(singleNavigationViewItem.MenuItems, selectedSuggestBoxItem);
        }

        return false;
    }

    protected virtual void UpdateMenuItemsTemplate(IList list)
    {
        for (var i = 0; i < list.Count; i++)
        {
            var singleMenuItem = list[i];

            if (singleMenuItem is not NavigationViewItem singleNavigationViewItem)
                continue;

            if (ItemTemplate is not null && singleNavigationViewItem.Template != ItemTemplate)
                singleNavigationViewItem.Template = ItemTemplate;
        }
    }

    protected virtual void UpdateMenuItemsTemplate()
    {
        UpdateMenuItemsTemplate(MenuItems);
        UpdateMenuItemsTemplate(FooterMenuItems);
    }

    protected virtual void CloseNavigationViewItemMenus()
    {
        if (Journal.Count <= 0 || IsPaneOpen)
            return;

        DeactivateMenuItems(MenuItems);
        DeactivateMenuItems(FooterMenuItems);

        var currentItem = PageIdOrTargetTagNavigationViewsDictionary[Journal[^1]];
        if (currentItem.NavigationViewItemParent is null)
        {
            currentItem.Activate(this);
            return;
        }
        
        currentItem.Deactivate(this);
        currentItem.NavigationViewItemParent?.Activate(this);
    }

    protected void DeactivateMenuItems(IList list)
    {
        for (var i = 0; i < list.Count; i++)
        {
            var singleMenuItem = list[i];

            if (singleMenuItem is not NavigationViewItem singleNavigationViewItem)
                continue;

            singleNavigationViewItem.Deactivate(this);
        }
    }

    [DebuggerStepThrough]
    private void NavigationStackOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                _breadcrumbBarItems.Add(new NavigationViewBreadcrumbItem((INavigationViewItem)e.NewItems![0]!));
                break;
            case NotifyCollectionChangedAction.Remove:
                _breadcrumbBarItems.RemoveAt(e.OldStartingIndex);
                break;
            case NotifyCollectionChangedAction.Replace:
                _breadcrumbBarItems[0] = new NavigationViewBreadcrumbItem((INavigationViewItem)e.NewItems![0]!);
                break;
            case NotifyCollectionChangedAction.Move:
                break;
            case NotifyCollectionChangedAction.Reset:
                _breadcrumbBarItems.Clear();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
