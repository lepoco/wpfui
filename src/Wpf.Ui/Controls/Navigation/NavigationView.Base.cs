// Based on Windows UI Library
// Copyright(c) Microsoft Corporation.All rights reserved.

// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using Wpf.Ui.Controls.Interfaces;

namespace Wpf.Ui.Controls.Navigation;

// https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.navigationview?view=winrt-22621

/// <summary>
/// Represents a container that enables navigation of app content. It has a header, a view for the main content, and a menu pane for navigation commands.
/// </summary>
[ToolboxItem(true)]
[System.Drawing.ToolboxBitmap(typeof(NavigationView), "NavigationView.bmp")]
public partial class NavigationView : System.Windows.Controls.Control, INavigationView
{
    /// <inheritdoc/>
    public INavigationViewItem SelectedItem { get; private set; }

    /// <summary>
    /// Static constructor which overrides default property metadata.
    /// </summary>
    static NavigationView()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationView), new FrameworkPropertyMetadata(typeof(NavigationView)));
    }

    public NavigationView()
    {
        SelectedItem = null;
        NavigationParent = this;

        SetValue(MenuItemsProperty,
            new ObservableCollection<object>());

        SetValue(FooterMenuItemsProperty,
            new ObservableCollection<object>());

        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
        SizeChanged += OnSizeChanged;
    }

    /// <inheritdoc />
    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);

        if (ItemTemplate != null)
            UpdateMenuItemsTemplate();

        if (Header is NavigationViewBreadcrumb navigationViewBreadcrumb)
            navigationViewBreadcrumb.NavigationView = this;

        if (MenuItems?.Count > 0)
            OnMenuItemsChanged();

        if (FooterMenuItems?.Count > 0)
            OnFooterMenuItemsChanged();

        UpdateSelectionForMenuItems();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
    }

    /// <summary>
    /// This virtual method is called when this element is detached form a loaded tree.
    /// </summary>
    protected virtual void OnUnloaded(object sender, RoutedEventArgs e)
    {
        if (MenuItems is INotifyCollectionChanged menuItemsCollection)
            menuItemsCollection.CollectionChanged -= OnMenuItemsCollectionChanged;

        if (FooterMenuItems is INotifyCollectionChanged footerMenuItemsCollection)
            footerMenuItemsCollection.CollectionChanged -= OnFooterMenuItemsCollectionChanged;
    }

    /// <summary>
    /// This virtual method is called when ActualWidth or ActualHeight (or both) changed.
    /// </summary>
    protected virtual void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
    }

    /// <summary>
    /// This virtual method is called when <see cref="BackButton"/> is clicked.
    /// </summary>
    protected virtual void OnBackButtonClick(object sender, RoutedEventArgs e)
    {
#if DEBUG
        System.Diagnostics.Debug.WriteLine("Back");
#endif
    }

    /// <summary>
    /// This virtual method is called when <see cref="ToggleButton"/> is clicked.
    /// </summary>
    protected virtual void OnToggleButtonClick(object sender, RoutedEventArgs e)
    {
#if DEBUG
        System.Diagnostics.Debug.WriteLine("Toggle");
#endif
    }

    /// <summary>
    /// This virtual method is called when source of the menu items is changed.
    /// </summary>
    protected virtual void OnMenuItemsChanged()
    {
        UpdateSelectionForMenuItems();
    }

    /// <summary>
    /// This virtual method is called when source of the footer menu items is changed.
    /// </summary>
    protected virtual void OnFooterMenuItemsChanged()
    {
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
        UpdateMenuItemsTemplate();
    }

    internal void OnNavigationViewItemClick(NavigationViewItem navigationViewItem)
    {
        OnItemInvoked();

        NavigateInternal(navigationViewItem, null);
    }

    private void UpdateMenuItemsTemplate()
    {
        if (MenuItems is IEnumerable enumerableItemsSource)
            foreach (var singleMenuItem in enumerableItemsSource)
                if (singleMenuItem is NavigationViewItem singleNavigationViewItem)
                    if (ItemTemplate != null && singleNavigationViewItem.Template != ItemTemplate)
                        singleNavigationViewItem.Template = ItemTemplate;

        if (FooterMenuItems is IEnumerable enumerableFooterItemsSource)
            foreach (var singleMenuItem in enumerableFooterItemsSource)
                if (singleMenuItem is NavigationViewItem singleNavigationViewItem)
                    if (ItemTemplate != null && singleNavigationViewItem.Template != ItemTemplate)
                        singleNavigationViewItem.Template = ItemTemplate;
    }
}
