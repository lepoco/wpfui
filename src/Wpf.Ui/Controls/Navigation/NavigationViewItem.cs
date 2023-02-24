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
using System.Windows.Controls;
using System.Windows.Input;
using Wpf.Ui.Common;
using Wpf.Ui.Controls.IconElements;
using Wpf.Ui.Converters;

namespace Wpf.Ui.Controls.Navigation;

// https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.navigationviewitem?view=winrt-22621

/// <summary>
/// Represents the container for an item in a NavigationView control.
/// When needed, it can be used as a normal button with a <see cref="System.Windows.Controls.Primitives.ButtonBase.Click"/> action.
/// </summary>
[ToolboxItem(true)]
[System.Drawing.ToolboxBitmap(typeof(NavigationViewItem), "NavigationViewItem.bmp")]
[TemplatePart(Name = TemplateElementChevronGrid, Type = typeof(Grid))]
public class NavigationViewItem : System.Windows.Controls.Primitives.ButtonBase, INavigationViewItem
{
    protected const string TemplateElementChevronGrid = "PART_ChevronGrid";

    #region Static properties

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
        typeof(IconElement), typeof(NavigationViewItem),
        new PropertyMetadata(null, null, IconSourceElementConverter.ConvertToIconElement));

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

    #endregion

    #region Properties

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
    public IconElement? Icon
    {
        get => (IconElement) GetValue(IconProperty);
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

    #endregion

    /// <inheritdoc />
    public INavigationViewItem? NavigationViewItemParent { get; set; }

    /// <inheritdoc />
    public bool IsMenuElement { get; set; }

    /// <inheritdoc />
    public string Id { get; }

    protected Grid? ChevronGrid;

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
        SetValue(IconProperty, new IconElements.SymbolIcon { Symbol = icon });
    }

    public NavigationViewItem(string name, SymbolRegular icon, Type targetPageType, IList menuItems) : this(name, icon, targetPageType)
    {
        SetValue(MenuItemsProperty, menuItems);
    }

    /// <summary>
    /// Correctly activates
    /// </summary>
    public virtual void Activate(INavigationView navigationView)
    {
        IsActive = true;

        if (!navigationView.IsPaneOpen && NavigationViewItemParent is not null)
            NavigationViewItemParent.Activate(navigationView);

        if (navigationView.IsPaneOpen && NavigationViewItemParent is not null)
            NavigationViewItemParent.IsExpanded = true;

        if (Icon is IconElements.SymbolIcon symbolIcon && navigationView.PaneDisplayMode == NavigationViewPaneDisplayMode.LeftFluent)
            symbolIcon.Filled = true;
    }

    /// <summary>
    /// Correctly deactivates
    /// </summary>
    public virtual void Deactivate(INavigationView navigationView)
    {
        IsActive = false;
        NavigationViewItemParent?.Deactivate(navigationView);

        if (!navigationView.IsPaneOpen && HasMenuItems)
            IsExpanded = false;

        if (Icon is IconElements.SymbolIcon symbolIcon && navigationView.PaneDisplayMode == NavigationViewPaneDisplayMode.LeftFluent)
            symbolIcon.Filled = false;
    }

    /// <inheritdoc />
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if (GetTemplateChild(TemplateElementChevronGrid) is Grid chevronGrid)
        {
            ChevronGrid = chevronGrid;
        }
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
        if (NavigationView.GetNavigationParent(this) is not { } navigationView)
            return;

        if (HasMenuItems && navigationView.IsPaneOpen)
            IsExpanded = !IsExpanded;

        if (TargetPageType is not null)
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

        if (NavigationView.GetNavigationParent(this) is not { } navigationView)
            return;

        if (!navigationView.IsPaneOpen || navigationView.PaneDisplayMode != NavigationViewPaneDisplayMode.Left || ChevronGrid is null)
        {
            base.OnMouseDown(e);
            return;
        }

        var mouseOverChevron = ActualWidth < e.GetPosition(this).X + ChevronGrid.ActualWidth;
        if (!mouseOverChevron)
        {
            base.OnMouseDown(e);
            return;
        }

        IsExpanded = !IsExpanded;

        for (int i = 0; i < MenuItems.Count; i++)
        {
            object? menuItem = MenuItems[i];

            if (menuItem is not INavigationViewItem { IsActive: true })
                continue;

            if (IsExpanded)
                Deactivate(navigationView);
            else
                Activate(navigationView);

            break;
        }

        e.Handled = true;
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
