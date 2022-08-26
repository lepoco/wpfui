// Based on Windows UI Library
// Copyright(c) Microsoft Corporation.All rights reserved.

// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using Wpf.Ui.Controls.Interfaces;

namespace Wpf.Ui.Controls.Navigation;

// https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.navigationview?view=winrt-22621

/// <summary>
/// Represents a container that enables navigation of app content. It has a header, a view for the main content, and a menu pane for navigation commands.
/// </summary>
[ToolboxItem(true)]
[System.Drawing.ToolboxBitmap(typeof(NavigationView), "NavigationView.bmp")]
public partial class NavigationView : System.Windows.Controls.ContentControl, INavigationView
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

        Unloaded += OnUnloaded;
        SizeChanged += OnSizeChanged;
    }

    /// <inheritdoc />
    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);

        if (MenuItems is { Count: > 0 })
            MenuItemsSource = MenuItems;

        if (FooterMenuItems is { Count: > 0 })
            FooterMenuItemsSource = FooterMenuItems;

        if (ItemTemplate != null)
            UpdateMenuItemsTemplate();

        UpdateActiveNavigationViewItem();
    }

    /// <inheritdoc />
    protected override void OnRender(DrawingContext drawingContext)
    {
        base.OnRender(drawingContext);
    }

    /// <summary>
    /// This virtual method is called when this element is detached form a loaded tree.
    /// </summary>
    protected virtual void OnUnloaded(object sender, RoutedEventArgs e)
    {
    }

    /// <summary>
    /// This virtual method is called when <see cref="BackButton"/> is clicked.
    /// </summary>
    protected virtual void OnBackButtonClick(object sender, RoutedEventArgs e)
    {
        System.Diagnostics.Debug.WriteLine("Back");
    }

    /// <summary>
    /// This virtual method is called when <see cref="ToggleButton"/> is clicked.
    /// </summary>
    protected virtual void OnToggleButtonClick(object sender, RoutedEventArgs e)
    {
        System.Diagnostics.Debug.WriteLine("Toggle");
    }

    /// <summary>
    /// This virtual method is called when ActualWidth or ActualHeight (or both) changed.
    /// </summary>
    protected virtual void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
    }

    /// <summary>
    /// This virtual method is called when source of the menu items is changed.
    /// </summary>
    protected virtual void OnMenuItemsSourceChanged()
    {
        if (MenuItemsSource == null)
            return;

        if (MenuItemsSource is IEnumerable enumerableItemsSource)
            foreach (var singleMenuItem in enumerableItemsSource)
                if (singleMenuItem is NavigationViewItem singleNavigationViewItem)
                    UpdateSingleMenuItem(singleNavigationViewItem);

        UpdateActiveNavigationViewItem();
    }

    /// <summary>
    /// This virtual method is called when source of the footer menu items is changed.
    /// </summary>
    protected virtual void OnFooterMenuItemsSourceChanged()
    {
        if (FooterMenuItemsSource is IEnumerable enumerableItemsSource)
            foreach (var singleMenuItem in enumerableItemsSource)
                if (singleMenuItem is NavigationViewItem singleNavigationViewItem)
                    UpdateSingleMenuItem(singleNavigationViewItem);

        UpdateActiveNavigationViewItem();
    }

    /// <summary>
    /// This virtual method is called when <see cref="ItemTemplate"/> is changed.
    /// </summary>
    protected virtual void OnItemTemplateChanged()
    {
        UpdateMenuItemsTemplate();
    }

    internal void UpdateSingleMenuItem(NavigationViewItem navigationViewItem)
    {
        System.Diagnostics.Debug.WriteLine($"DEBUG | {navigationViewItem.GetHashCode()} - {navigationViewItem.TargetPageTag ?? "NO_TAG"} | REGISTERED");

        navigationViewItem.Click -= OnNavigationViewItemClick;
        navigationViewItem.Click += OnNavigationViewItemClick;
    }

    private void OnNavigationViewItemClick(object sender, RoutedEventArgs e)
    {
        if (sender is not INavigationViewItem navigationViewItem)
            return;

        OnItemInvoked();

        if (navigationViewItem == SelectedItem)
            return;

        SelectedItem = navigationViewItem;

        UpdateActiveNavigationViewItem();

        System.Diagnostics.Debug.WriteLine($"DEBUG | {navigationViewItem.GetHashCode()} - {navigationViewItem.TargetPageTag ?? "NO_TAG"} | CLICKED");
    }

    private void UpdateMenuItemsTemplate()
    {
        if (MenuItemsSource is IEnumerable enumerableItemsSource)
            foreach (var singleMenuItem in enumerableItemsSource)
                if (singleMenuItem is NavigationViewItem singleNavigationViewItem)
                    if (ItemTemplate != null && singleNavigationViewItem.Template != ItemTemplate)
                        singleNavigationViewItem.Template = ItemTemplate;

        if (FooterMenuItemsSource is IEnumerable enumerableFooterItemsSource)
            foreach (var singleMenuItem in enumerableFooterItemsSource)
                if (singleMenuItem is NavigationViewItem singleNavigationViewItem)
                    if (ItemTemplate != null && singleNavigationViewItem.Template != ItemTemplate)
                        singleNavigationViewItem.Template = ItemTemplate;
    }

    private void UpdateActiveNavigationViewItem()
    {
        if (MenuItemsSource is IEnumerable enumerableMenuItems)
        {
            foreach (var singleMenuItem in enumerableMenuItems)
            {
                if (singleMenuItem is not NavigationViewItem navigationViewItem)
                    continue;

                navigationViewItem.IsActive = navigationViewItem == SelectedItem;

                if (navigationViewItem.MenuItems is IEnumerable enumerableSubMenuItems)
                {
                    foreach (var singleSubMenuItem in enumerableSubMenuItems)
                    {
                        if (singleSubMenuItem is not NavigationViewItem navigationViewSubItem)
                            continue;

                        navigationViewSubItem.IsActive = navigationViewSubItem == SelectedItem;
                    }
                }
            }
        }

        if (FooterMenuItemsSource is IEnumerable enumerableFooterMenuItems)
        {
            foreach (var singleFooterMenuItem in enumerableFooterMenuItems)
            {
                if (singleFooterMenuItem is not NavigationViewItem navigationViewItem)
                    continue;

                navigationViewItem.IsActive = navigationViewItem == SelectedItem;

                if (navigationViewItem.MenuItems is IEnumerable enumerableSubMenuItems)
                {
                    foreach (var singleSubMenuItem in enumerableSubMenuItems)
                    {
                        if (singleSubMenuItem is not NavigationViewItem navigationViewSubItem)
                            continue;

                        navigationViewSubItem.IsActive = navigationViewSubItem == SelectedItem;
                    }
                }
            }
        }
    }
}
