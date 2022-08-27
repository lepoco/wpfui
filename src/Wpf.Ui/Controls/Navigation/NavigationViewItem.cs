// Based on Windows UI Library
// Copyright(c) Microsoft Corporation.All rights reserved.

// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using Wpf.Ui.Controls.Interfaces;

namespace Wpf.Ui.Controls.Navigation;

// https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.navigationviewitem?view=winrt-22621

/// <summary>
/// Represents the container for an item in a NavigationView control.
/// </summary>
[ToolboxItem(true)]
[System.Drawing.ToolboxBitmap(typeof(NavigationViewItem), "NavigationViewItem.bmp")]
public class NavigationViewItem : System.Windows.Controls.Primitives.ButtonBase, INavigationViewItem, IIconControl
{
    /// <summary>
    /// Property for <see cref="MenuItems"/>.
    /// </summary>
    public static readonly DependencyProperty MenuItemsProperty = DependencyProperty.Register(nameof(MenuItems),
        typeof(IList), typeof(NavigationViewItem),
        new PropertyMetadata(new List<object> { }));

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
    /// Property for <see cref="Icon"/>.
    /// </summary>
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon),
        typeof(Common.SymbolRegular), typeof(NavigationViewItem),
        new PropertyMetadata(Common.SymbolRegular.Empty));

    /// <summary>
    /// Property for <see cref="IconFilled"/>.
    /// </summary>
    public static readonly DependencyProperty IconFilledProperty = DependencyProperty.Register(nameof(IconFilled),
        typeof(bool), typeof(NavigationViewItem), new PropertyMetadata(false));

    /// <summary>
    /// Property for <see cref="IconForeground"/>.
    /// </summary>
    public static readonly DependencyProperty IconForegroundProperty = DependencyProperty.Register(nameof(IconForeground),
        typeof(Brush), typeof(NavigationViewItem), new FrameworkPropertyMetadata(SystemColors.ControlTextBrush,
            FrameworkPropertyMetadataOptions.Inherits));

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
    public IList MenuItems
    {
        get => (IList)GetValue(MenuItemsProperty);
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
    [Bindable(true), Category("Appearance")]
    public Common.SymbolRegular Icon
    {
        get => (Common.SymbolRegular)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <inheritdoc />
    [Bindable(true), Category("Appearance")]
    public bool IconFilled
    {
        get => (bool)GetValue(IconFilledProperty);
        set => SetValue(IconFilledProperty, value);
    }

    /// <inheritdoc />
    [Bindable(true), Category("Appearance")]
    public Brush IconForeground
    {
        get => (Brush)GetValue(IconForegroundProperty);
        set => SetValue(IconForegroundProperty, value);
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

    /// <summary>
    /// Unique identifier that allows the item to be located in the navigation.
    /// </summary>
    public readonly string Id;

    static NavigationViewItem()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationViewItem), new FrameworkPropertyMetadata(typeof(NavigationViewItem)));
    }

    public NavigationViewItem()
    {
        Id = Guid.NewGuid().ToString("n");
    }

    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);

        //if (MenuItems is { Count: > 0 })
        //    MenuItemsSource = MenuItems;

        if (String.IsNullOrWhiteSpace(TargetPageTag))
            TargetPageTag = Content?.ToString()!.ToLower()!.Trim() ?? String.Empty;

        //UpdateMenuItemsBindings();
    }

    private static void OnMenuItemsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not NavigationViewItem navigationViewItem)
            return;

        navigationViewItem.OnMenuItemsSourceChanged();
    }

    /// <summary>
    /// This virtual method is called when source of the menu items is changed.
    /// </summary>
    protected virtual void OnMenuItemsSourceChanged()
    {
        UpdateMenuItemsBindings();
    }

    private void UpdateMenuItemsBindings()
    {
        var isNavigationViewItemHasSubItems = MenuItemsSource is ICollection { Count: > 0 };

        HasMenuItems = isNavigationViewItemHasSubItems;

        if (!isNavigationViewItemHasSubItems)
            return;

        if (NavigationView.GetNavigationParent(this) is not { } navigationView)
            return;

        var current = this;

        if (MenuItemsSource is IEnumerable enumerableItemsSource)
            foreach (var singleMenuItem in enumerableItemsSource)
                if (singleMenuItem is NavigationViewItem singleNavigationViewItem)
                    navigationView.UpdateSingleMenuItem(singleNavigationViewItem);
    }
}
