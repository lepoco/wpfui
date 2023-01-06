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
        new PropertyMetadata(OnMenuItemsPropertyChanged));

    /// <summary>
    /// Property for <see cref="MenuItemsSource"/>.
    /// </summary>
    public static readonly DependencyProperty MenuItemsSourceProperty = DependencyProperty.Register(nameof(MenuItemsSource),
        typeof(object), typeof(NavigationViewItem),
        new PropertyMetadata(((object)null!), OnMenuItemsSourcePropertyChanged));

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
        new PropertyMetadata((object)null!));

    /// <summary>
    /// Property for <see cref="TargetPageTag"/>.
    /// </summary>
    public static readonly DependencyProperty TargetPageTagProperty = DependencyProperty.Register(nameof(TargetPageTag),
        typeof(string), typeof(NavigationViewItem), new PropertyMetadata(String.Empty));

    /// <summary>
    /// Property for <see cref="TargetPageType"/>.
    /// </summary>
    public static readonly DependencyProperty TargetPageTypeProperty = DependencyProperty.Register(nameof(TargetPageType),
        typeof(Type), typeof(NavigationViewItem), new PropertyMetadata((Type)null!));

    /// <inheritdoc/>
    public IList? MenuItems
    {
        get => (IList?)GetValue(MenuItemsProperty);
        set => SetValue(MenuItemsProperty, value);
    }

    /// <inheritdoc/>
    [Bindable(true)]
    public object MenuItemsSource
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
        internal set => SetValue(IsActiveProperty, value);
    }

    /// <inheritdoc />
    [Browsable(false), ReadOnly(true)]
    public bool IsExpanded
    {
        get => (bool)GetValue(IsExpandedProperty);
        internal set => SetValue(IsExpandedProperty, value);
    }

    /// <inheritdoc />
    [Bindable(true), Category("Appearance")]
    public object Icon
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
    public Type TargetPageType
    {
        get => (Type)GetValue(TargetPageTypeProperty);
        set => SetValue(TargetPageTypeProperty, value);
    }

    /// <inheritdoc />
    public string Id { get; }

    static NavigationViewItem()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationViewItem), new FrameworkPropertyMetadata(typeof(NavigationViewItem)));
    }

    public NavigationViewItem()
    {
        Id = Guid.NewGuid().ToString("n");

        SetValue(MenuItemsProperty,
            new ObservableCollection<object>());
    }

    /// <inheritdoc />
    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);

        if (MenuItems?.Count > 0)
            HasMenuItems = true;

        if (MenuItemsSource is IList menuItemsSourceList)
        {
            if (MenuItems != null)
                MenuItems = null;

            MenuItems = menuItemsSourceList;
        }

        if (String.IsNullOrWhiteSpace(TargetPageTag))
            TargetPageTag = Content?.ToString()!.ToLower()!.Trim() ?? String.Empty;
    }

    /// <inheritdoc />
    protected override void OnClick()
    {
        if (HasMenuItems)
            IsExpanded = !IsExpanded;

        // TODO: If clicked on chevron, do not call parent event

        if (TargetPageType != null && NavigationView.GetNavigationParent(this) is { } navigationView)
            navigationView.OnNavigationViewItemClick(this);

        base.OnClick();
    }

    private static void OnMenuItemsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not NavigationViewItem navigationViewItem)
            return;

        navigationViewItem.HasMenuItems = navigationViewItem.MenuItems.Count > 0;
    }

    private static void OnMenuItemsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not NavigationViewItem navigationViewItem)
            return;

        if (e.NewValue is not IList enumerableNewValue)
            return;

        if (navigationViewItem.MenuItems != null)
            navigationViewItem.MenuItems = null;

        navigationViewItem.MenuItems = enumerableNewValue;
    }
}
