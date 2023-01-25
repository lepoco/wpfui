// Based on Windows UI Library
// Copyright(c) Microsoft Corporation.All rights reserved.

// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using Wpf.Ui.Common;

namespace Wpf.Ui.Controls.Navigation;

// https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.navigationview?view=winrt-22621

/// <summary>
/// Represents a container that enables navigation of app content. It has a header, a view for the main content, and a menu pane for navigation commands.
/// </summary>
[ToolboxItem(true)]
[System.Drawing.ToolboxBitmap(typeof(NavigationView), "NavigationView.bmp")]
public partial class NavigationView : System.Windows.Controls.Control, INavigationView
{
    private readonly ObservableCollection<string> _autoSuggestBoxItems = new();
    private readonly ObservableCollection<NavigationViewBreadcrumbItem> _breadcrumbBarItems = new();

    /// <inheritdoc/>
    public INavigationViewItem? SelectedItem { get; private set; }

    /// <summary>
    /// Static constructor which overrides default property metadata.
    /// </summary>
    static NavigationView()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationView), new FrameworkPropertyMetadata(typeof(NavigationView)));
    }

    public NavigationView()
    {
        NavigationParent = this;

        SetValue(MenuItemsProperty, new ObservableCollection<object>());
        SetValue(FooterMenuItemsProperty, new ObservableCollection<object>());

        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
        SizeChanged += OnSizeChanged;
    }

    /// <inheritdoc />
    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);

        if (Header is BreadcrumbBar breadcrumbBar)
        {
            breadcrumbBar.ItemsSource = _breadcrumbBarItems;
            breadcrumbBar.ItemTemplate ??= Application.Current.TryFindResource("NavigationViewItemDataTemplate") as DataTemplate;
            breadcrumbBar.ItemClicked += BreadcrumbBarOnItemClicked;
        }

        if (AutoSuggestBox is not null)
        {
            AutoSuggestBox.ItemsSource = _autoSuggestBoxItems;
            AutoSuggestBox.SuggestionChosen += AutoSuggestBoxOnSuggestionChosen;
        }

        InvalidateArrange();
        InvalidateVisual();
        UpdateLayout();

        UpdateAutoSuggestBoxSuggestions();
        UpdateSelectionForMenuItems();
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

        NavigationViewContentPresenter.Navigated -= OnNavigationViewContentPresenterNavigated;

        if (AutoSuggestBox is not null)
            AutoSuggestBox.SuggestionChosen -= AutoSuggestBoxOnSuggestionChosen;

        if (Header is BreadcrumbBar breadcrumbBar)
        {
            breadcrumbBar.ItemClicked -= BreadcrumbBarOnItemClicked;
        }
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
    protected virtual void OnBackButtonClick(object sender, RoutedEventArgs e) => GoBack();

    /// <summary>
    /// This virtual method is called when <see cref="ToggleButton"/> is clicked.
    /// </summary>
    protected virtual void OnToggleButtonClick(object sender, RoutedEventArgs e)
    {
        Debug.WriteLine("Toggle");
    }

    /// <summary>
    /// This virtual method is called when source of the menu items is changed.
    /// </summary>
    protected virtual void OnMenuItemsChanged()
    {
        InvalidateArrange();
        InvalidateVisual();
        UpdateLayout();

        UpdateAutoSuggestBoxSuggestions();
        UpdateSelectionForMenuItems();
    }

    /// <summary>
    /// This virtual method is called when source of the footer menu items is changed.
    /// </summary>
    protected virtual void OnFooterMenuItemsChanged()
    {
        UpdateAutoSuggestBoxSuggestions();
        UpdateSelectionForMenuItems();
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
        UpdateMenuItemsTemplate(MenuItems);
        UpdateMenuItemsTemplate(FooterMenuItems);
    }

    internal void ToggleAllExpands()
    {
        // TODO: When shift clicked on navigationviewitem
    }

    internal void OnNavigationViewItemClick(NavigationViewItem navigationViewItem)
    {
        OnItemInvoked();

        NavigateInternal(navigationViewItem, null, true, false);
    }

    protected virtual void BreadcrumbBarOnItemClicked(BreadcrumbBar sender, BreadcrumbBarItemClickedEventArgs e)
    {
        
    }

    protected void UpdateAutoSuggestBoxSuggestions()
    {
        if (AutoSuggestBox == null)
            return;

        _autoSuggestBoxItems.Clear();

        AddItemsToAutoSuggestBoxItemsForMenuItems(MenuItems);
        AddItemsToAutoSuggestBoxItemsForMenuItems(FooterMenuItems);
    }

    /// <summary>
    /// Navigate to the page after its name is selected in <see cref="AutoSuggestBox"/>.
    /// </summary>
    protected void AutoSuggestBoxOnSuggestionChosen(object sender, RoutedEventArgs e)
    {
        if (sender is not AutoSuggestBox { ChosenSuggestion: string selectedSuggestBoxItem })
            return;

        if (string.IsNullOrEmpty(selectedSuggestBoxItem))
            return;

        if (NavigateToMenuItemFromAutoSuggestBox(MenuItems, selectedSuggestBoxItem))
            return;

        NavigateToMenuItemFromAutoSuggestBox(FooterMenuItems, selectedSuggestBoxItem);
    }

    private void AddItemsToAutoSuggestBoxItemsForMenuItems(IList? list)
    {
        if (list is null)
            return;

        foreach (var singleMenuItem in list)
        {
            if (singleMenuItem is not NavigationViewItem singleNavigationViewItem)
                continue;

            if (singleNavigationViewItem is { Content: string content, TargetPageType: { } } && !string.IsNullOrWhiteSpace(content))
                _autoSuggestBoxItems.Add(content);

            if (!(singleNavigationViewItem.MenuItems?.Count > 0))
                continue;

            foreach (var subMenuItem in singleNavigationViewItem.MenuItems)
            {
                if (subMenuItem is NavigationViewItem { Content: string subContent, TargetPageType: not null } && !string.IsNullOrWhiteSpace(subContent))
                    _autoSuggestBoxItems.Add(subContent);
            }

        }
    }

    private bool NavigateToMenuItemFromAutoSuggestBox(IList? list, string selectedSuggestBoxItem)
    {
        if (list is null)
            return false;

        foreach (var singleMenuItem in list)
        {
            if (singleMenuItem is not NavigationViewItem singleNavigationViewItem)
                continue;

            if (singleNavigationViewItem.Content is string content && content == selectedSuggestBoxItem)
            {
                NavigateInternal(singleNavigationViewItem, null, true, false);
                singleNavigationViewItem.BringIntoView();
                singleNavigationViewItem.Focus();

                return true;
            }

            if (!(singleNavigationViewItem.MenuItems?.Count > 0))
                continue;

            foreach (var subMenuItem in singleNavigationViewItem.MenuItems)
            {
                if (subMenuItem is not NavigationViewItem { Content: string subContent } subMenuNavigationViewItem || subContent != selectedSuggestBoxItem)
                    continue;

                NavigateInternal(subMenuNavigationViewItem, null, true, false);
                subMenuNavigationViewItem.BringIntoView();
                subMenuNavigationViewItem.Focus();

                return true;
            }
        }

        return false;
    }

    private void UpdateMenuItemsTemplate(IList? list)
    {
        if (list is null)
            return;

        foreach (var singleMenuItem in list)
        {
            if (singleMenuItem is not NavigationViewItem singleNavigationViewItem)
                continue;

            if (ItemTemplate is not null && singleNavigationViewItem.Template != ItemTemplate)
                singleNavigationViewItem.Template = ItemTemplate;
        }
    }
}
