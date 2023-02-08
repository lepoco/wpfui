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
using System.Windows;
using System.Windows.Input;
using Wpf.Ui.Common;

namespace Wpf.Ui.Controls.Navigation;

// https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.navigationviewitem?view=winrt-22621

/// <summary>
/// Represents the container for an item in a NavigationView control.
/// When needed, it can be used as a normal button with a <see cref="System.Windows.Controls.Primitives.ButtonBase.Click"/> action.
/// </summary>
[ToolboxItem(true)]
[System.Drawing.ToolboxBitmap(typeof(NavigationViewItem), "NavigationViewItem.bmp")]
public class NavigationViewItem : System.Windows.Controls.Primitives.ButtonBase, INavigationViewItem
{
    /// <summary>
    /// Property for <see cref="MenuItems"/>.
    /// </summary>
    public static readonly DependencyProperty MenuItemsProperty = DependencyProperty.Register(nameof(MenuItems),
        typeof(IList), typeof(NavigationViewItem),
        new PropertyMetadata(new ObservableCollection<object>(), OnMenuItemsPropertyChanged));

    /// <summary>
    /// Property for <see cref="MenuItemsSource"/>.
    /// </summary>
    public static readonly DependencyProperty MenuItemsSourceProperty = DependencyProperty.Register(nameof(MenuItemsSource),
        typeof(object), typeof(NavigationViewItem),
        new PropertyMetadata(null, OnMenuItemsSourcePropertyChanged));

    /// <summary>
    /// Property for <see cref="HasMenuItems"/>.
    /// </summary>
    public static readonly DependencyProperty HasMenuItemsProperty = DependencyProperty.Register(nameof(HasMenuItems),
        typeof(bool), typeof(NavigationViewItem), new PropertyMetadata(false));

    /// <summary>
    /// Property for <see cref="IsActive"/>.
    /// </summary>
    public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register(nameof(IsActive),
        typeof(bool), typeof(NavigationViewItem), new PropertyMetadata(false));

    /// <summary>
    /// Property for <see cref="IsExpanded"/>.
    /// </summary>
    public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(nameof(IsExpanded),
        typeof(bool), typeof(NavigationViewItem), new PropertyMetadata(false));

    /// <summary>
    /// Property for <see cref="Icon"/>.
    /// </summary>
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon),
        typeof(object), typeof(NavigationViewItem),
        new PropertyMetadata(null));

    /// <summary>
    /// Property for <see cref="TargetPageTag"/>.
    /// </summary>
    public static readonly DependencyProperty TargetPageTagProperty = DependencyProperty.Register(nameof(TargetPageTag),
        typeof(string), typeof(NavigationViewItem), new PropertyMetadata(string.Empty));

    /// <summary>
    /// Property for <see cref="TargetPageType"/>.
    /// </summary>
    public static readonly DependencyProperty TargetPageTypeProperty = DependencyProperty.Register(nameof(TargetPageType),
        typeof(Type), typeof(NavigationViewItem), new PropertyMetadata(null));

    /// <inheritdoc/>
    public IList MenuItems
    {
        get => (IList)GetValue(MenuItemsProperty);
        set => SetValue(MenuItemsProperty, value);
    }

    /// <inheritdoc/>
    [Bindable(true)]
    public object? MenuItemsSource
    {
        get => GetValue(MenuItemsSourceProperty);
        set
        {
            if (value == null)
                ClearValue(MenuItemsSourceProperty);
            else
                SetValue(MenuItemsSourceProperty, value);
        }
    }

    /// <summary>
    /// Gets a value indicating whether the <see cref="NavigationViewItem"/> has <see cref="MenuItems"/>.
    /// </summary>
    [Browsable(false), ReadOnly(true)]
    public bool HasMenuItems
    {
        get => (bool)GetValue(HasMenuItemsProperty);
        private set => SetValue(HasMenuItemsProperty, value);
    }

    /// <inheritdoc />
    [Browsable(false), ReadOnly(true)]
    public bool IsActive
    {
        get => (bool)GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }

    /// <inheritdoc />
    [Browsable(false), ReadOnly(true)]
    public bool IsExpanded
    {
        get => (bool)GetValue(IsExpandedProperty);
        set => SetValue(IsExpandedProperty, value);
    }

    /// <inheritdoc />
    [Bindable(true), Category("Appearance")]
    public object? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <inheritdoc />
    public string TargetPageTag
    {
        get => (string)GetValue(TargetPageTagProperty);
        set => SetValue(TargetPageTagProperty, value);
    }

    /// <inheritdoc />
    public Type? TargetPageType
    {
        get => (Type)GetValue(TargetPageTypeProperty);
        set => SetValue(TargetPageTypeProperty, value);
    }

    /// <inheritdoc />
    public INavigationViewItem? NavigationViewItemParent { get; set; }

    /// <inheritdoc />
    public bool IsMenuElement { get; set; }

    /// <inheritdoc />
    public string Id { get; }

    static NavigationViewItem()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationViewItem), new FrameworkPropertyMetadata(typeof(NavigationViewItem)));
    }

    public NavigationViewItem()
    {
        Id = Guid.NewGuid().ToString("n");

        //Just in case
        Unloaded += static (sender, _) => ((NavigationViewItem)sender).NavigationViewItemParent = null;
    }

    public NavigationViewItem(Type targetPageType) : this()
    {
        SetValue(TargetPageTypeProperty, targetPageType);
    }

    public NavigationViewItem(string name, Type targetPageType) : this(targetPageType)
    {
        SetValue(ContentProperty, name);
    }

    public NavigationViewItem(string name, SymbolRegular icon, Type targetPageType) : this(targetPageType)
    {
        SetValue(ContentProperty, name);
        SetValue(IconProperty, new SymbolIcon { Symbol = icon });
    }

    public NavigationViewItem(string name, SymbolRegular icon, Type targetPageType, IList menuItems) : this(name, icon, targetPageType)
    {
        SetValue(MenuItemsProperty, menuItems);
    }

    /// <inheritdoc />
    public void Activate(NavigationViewPaneDisplayMode paneDisplayMode)
    {
        IsActive = true;

        if (Icon is SymbolIcon symbolIcon && paneDisplayMode == NavigationViewPaneDisplayMode.LeftFluent)
            symbolIcon.Filled = true;

        if (NavigationViewItemParent is not null)
            NavigationViewItemParent.IsExpanded = true;
    }

    /// <inheritdoc />
    public void Deactivate(NavigationViewPaneDisplayMode paneDisplayMode)
    {
        IsActive = false;

        if (Icon is SymbolIcon symbolIcon && paneDisplayMode == NavigationViewPaneDisplayMode.LeftFluent)
            symbolIcon.Filled = false;
    }

    /// <inheritdoc />
    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);

        if (string.IsNullOrWhiteSpace(TargetPageTag) && Content is not null)
        {
            TargetPageTag = Content as string ?? Content.ToString()?.ToLower().Trim() ?? string.Empty;
        }
    }

    /// <inheritdoc />
    protected override void OnClick()
    {
        if (HasMenuItems)
            IsExpanded = !IsExpanded;

        if (TargetPageType != null && NavigationView.GetNavigationParent(this) is { } navigationView)
            navigationView.OnNavigationViewItemClick(this);

        base.OnClick();
    }

    /// <summary>
    /// Is called when mouse is clicked down.
    /// </summary>
    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        if (!HasMenuItems || e.LeftButton != MouseButtonState.Pressed)
        {
            base.OnMouseDown(e);

            return;
        }

        if (GetTemplateChild("PART_ChevronGrid") is not System.Windows.Controls.Grid chevronGrid)
        {
            base.OnMouseDown(e);

            return;
        }

        var parentNavigationView = NavigationView.GetNavigationParent(this);

        if (parentNavigationView?.IsPaneOpen ?? parentNavigationView?.PaneDisplayMode != NavigationViewPaneDisplayMode.Left)
        {
            base.OnMouseDown(e);
            return;
        }

        var mouseOverChevron = ActualWidth < e.GetPosition(this).X + chevronGrid.ActualWidth;

        if (!mouseOverChevron)
        {
            base.OnMouseDown(e);
            return;
        }

        e.Handled = true;

        // TODO: If shift, expand all

        IsExpanded = !IsExpanded;
    }

    private static void OnMenuItemsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not NavigationViewItem navigationViewItem)
            return;

        navigationViewItem.HasMenuItems = navigationViewItem.MenuItems.Count > 0;

        foreach (var menuItem in navigationViewItem.MenuItems)
        {
            if (menuItem is not INavigationViewItem item)
                continue;

            item.NavigationViewItemParent = navigationViewItem;
        }
    }

    private static void OnMenuItemsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not NavigationViewItem navigationViewItem || e.NewValue is not IList enumerableNewValue)
            return;

        navigationViewItem.MenuItems = enumerableNewValue;

        if (navigationViewItem.MenuItems.Count > 0)
            navigationViewItem.HasMenuItems = true;
    }
}
