// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

// Based on Windows UI Library
// Copyright(c) Microsoft Corporation.All rights reserved.

using System;
using System.ComponentModel;
using System.Windows;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <content>
/// Defines the template parts for the <see cref="NavigationView"/> control
/// </content>
[TemplatePart(
    Name = TemplateElementNavigationViewContentPresenter,
    Type = typeof(NavigationViewContentPresenter)
)]
[TemplatePart(
    Name = TemplateElementMenuItemsItemsControl,
    Type = typeof(System.Windows.Controls.ItemsControl)
)]
[TemplatePart(
    Name = TemplateElementFooterMenuItemsItemsControl,
    Type = typeof(System.Windows.Controls.ItemsControl)
)]
[TemplatePart(Name = TemplateElementBackButton, Type = typeof(System.Windows.Controls.Button))]
[TemplatePart(Name = TemplateElementToggleButton, Type = typeof(System.Windows.Controls.Button))]
[TemplatePart(
    Name = TemplateElementAutoSuggestBoxSymbolButton,
    Type = typeof(System.Windows.Controls.Button)
)]
[TemplatePart(Name = TemplateElementPaneColumn, Type = typeof(System.Windows.Controls.ColumnDefinition))]
public partial class NavigationView
{
    /// <summary>
    /// Template element represented by the <c>PART_MenuItemsItemsControl</c> name.
    /// </summary>
    private const string TemplateElementNavigationViewContentPresenter =
        "PART_NavigationViewContentPresenter";

    /// <summary>
    /// Template element represented by the <c>PART_MenuItemsItemsControl</c> name.
    /// </summary>
    private const string TemplateElementMenuItemsItemsControl = "PART_MenuItemsItemsControl";

    /// <summary>
    /// Template element represented by the <c>PART_FooterMenuItemsItemsControl</c> name.
    /// </summary>
    private const string TemplateElementFooterMenuItemsItemsControl = "PART_FooterMenuItemsItemsControl";

    /// <summary>
    /// Template element represented by the <c>PART_BackButton</c> name.
    /// </summary>
    private const string TemplateElementBackButton = "PART_BackButton";

    /// <summary>
    /// Template element represented by the <c>PART_ToggleButton</c> name.
    /// </summary>
    private const string TemplateElementToggleButton = "PART_ToggleButton";

    /// <summary>
    /// Template element represented by the <c>PART_AutoSuggestBoxSymbolButton</c> name.
    /// </summary>
    private const string TemplateElementAutoSuggestBoxSymbolButton = "PART_AutoSuggestBoxSymbolButton";

    /// <summary>
    /// Template element represented by the <c>PART_PaneColumn</c> name.
    /// </summary>
    private const string TemplateElementPaneColumn = "PART_PaneColumn";

    /// <summary>
    /// Gets or sets the control responsible for rendering the content.
    /// </summary>
    protected NavigationViewContentPresenter NavigationViewContentPresenter { get; set; } = null!;

    /// <summary>
    /// Gets or sets the control located at the top of the pane with left arrow icon.
    /// </summary>
    protected System.Windows.Controls.ItemsControl MenuItemsItemsControl { get; set; } = null!;

    /// <summary>
    /// Gets or sets the control located at the top of the pane with hamburger icon.
    /// </summary>
    protected System.Windows.Controls.ItemsControl FooterMenuItemsItemsControl { get; set; } = null!;

    /// <summary>
    /// Gets or sets the control located at the top of the pane with left arrow icon.
    /// </summary>
    protected System.Windows.Controls.Button? BackButton { get; set; }

    /// <summary>
    /// Gets or sets the control located at the top of the pane with hamburger icon.
    /// </summary>
    protected System.Windows.Controls.Button? ToggleButton { get; set; }

    /// <summary>
    /// Gets or sets the control that is visitable if PaneDisplayMode="Left" and in compact state
    /// </summary>
    protected System.Windows.Controls.Button? AutoSuggestBoxSymbolButton { get; set; }

    /// <summary>
    /// Gets or sets the pane column definition for GridSplitter support.
    /// </summary>
    protected System.Windows.Controls.ColumnDefinition? PaneColumn { get; set; }

    private DependencyPropertyDescriptor? _openPaneLengthDescriptor;
    private DependencyPropertyDescriptor? _isPaneOpenDescriptor;

    /// <inheritdoc />
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        NavigationViewContentPresenter = GetTemplateChild<NavigationViewContentPresenter>(
            TemplateElementNavigationViewContentPresenter
        );
        MenuItemsItemsControl = GetTemplateChild<System.Windows.Controls.ItemsControl>(
            TemplateElementMenuItemsItemsControl
        );
        FooterMenuItemsItemsControl = GetTemplateChild<System.Windows.Controls.ItemsControl>(
            TemplateElementFooterMenuItemsItemsControl
        );

        MenuItemsItemsControl.SetCurrentValue(
            System.Windows.Controls.ItemsControl.ItemsSourceProperty,
            MenuItems
        );
        FooterMenuItemsItemsControl.SetCurrentValue(
            System.Windows.Controls.ItemsControl.ItemsSourceProperty,
            FooterMenuItems
        );

        if (NavigationViewContentPresenter is not null)
        {
            NavigationViewContentPresenter.Navigated -= OnNavigationViewContentPresenterNavigated;
            NavigationViewContentPresenter.Navigated += OnNavigationViewContentPresenterNavigated;
        }

        if (
            GetTemplateChild(TemplateElementAutoSuggestBoxSymbolButton)
            is System.Windows.Controls.Button autoSuggestBoxSymbolButton
        )
        {
            AutoSuggestBoxSymbolButton = autoSuggestBoxSymbolButton;

            AutoSuggestBoxSymbolButton.Click -= AutoSuggestBoxSymbolButtonOnClick;
            AutoSuggestBoxSymbolButton.Click += AutoSuggestBoxSymbolButtonOnClick;
        }

        if (GetTemplateChild(TemplateElementBackButton) is System.Windows.Controls.Button backButton)
        {
            BackButton = backButton;

            BackButton.Click -= OnBackButtonClick;
            BackButton.Click += OnBackButtonClick;
        }

        if (GetTemplateChild(TemplateElementToggleButton) is System.Windows.Controls.Button toggleButton)
        {
            ToggleButton = toggleButton;

            ToggleButton.Click -= OnToggleButtonClick;
            ToggleButton.Click += OnToggleButtonClick;
        }

        // Clean up previous event handlers
        if (_openPaneLengthDescriptor != null)
        {
            _openPaneLengthDescriptor.RemoveValueChanged(this, OnPanePropertyChanged);
            _openPaneLengthDescriptor = null;
        }

        if (_isPaneOpenDescriptor != null)
        {
            _isPaneOpenDescriptor.RemoveValueChanged(this, OnPanePropertyChanged);
            _isPaneOpenDescriptor = null;
        }

        // Initialize PaneColumn width for GridSplitter support
        var paneColumn = GetTemplateChild(TemplateElementPaneColumn) as System.Windows.Controls.ColumnDefinition;
        if (paneColumn != null)
        {
            PaneColumn = paneColumn;
            
            // Set initial width using OpenPaneLength
            if (IsPaneOpen && OpenPaneLength > 0)
            {
                PaneColumn.Width = new GridLength(OpenPaneLength);
            }
            else if (!IsPaneOpen)
            {
                PaneColumn.Width = new GridLength(40.0);
            }
            
            // Update on next render pass to avoid issues
            Dispatcher.BeginInvoke(new Action(UpdatePaneColumnWidth), System.Windows.Threading.DispatcherPriority.Loaded);

            // Listen to OpenPaneLength and IsPaneOpen changes
            _openPaneLengthDescriptor = DependencyPropertyDescriptor.FromProperty(OpenPaneLengthProperty, typeof(NavigationView));
            if (_openPaneLengthDescriptor != null)
            {
                _openPaneLengthDescriptor.AddValueChanged(this, OnPanePropertyChanged);
            }

            _isPaneOpenDescriptor = DependencyPropertyDescriptor.FromProperty(IsPaneOpenProperty, typeof(NavigationView));
            if (_isPaneOpenDescriptor != null)
            {
                _isPaneOpenDescriptor.AddValueChanged(this, OnPanePropertyChanged);
            }
        }
        else
        {
            PaneColumn = null;
        }
    }

    private void OnPanePropertyChanged(object? sender, EventArgs e)
    {
        if (Dispatcher.CheckAccess())
        {
            UpdatePaneColumnWidth();
        }
        else
        {
            Dispatcher.BeginInvoke(new Action(UpdatePaneColumnWidth), System.Windows.Threading.DispatcherPriority.Normal);
        }
    }

    private void UpdatePaneColumnWidth()
    {
        if (PaneColumn is null)
        {
            return;
        }

        try
        {
            // Don't update if GridSplitter is enabled and user has manually resized
            // This allows GridSplitter to control the width
            var currentWidth = PaneColumn.Width.IsAbsolute ? PaneColumn.Width.Value : (IsPaneOpen ? OpenPaneLength : 40.0);
            var expectedWidth = IsPaneOpen ? OpenPaneLength : 40.0;

            if (IsGridSplitterEnabled && Math.Abs(currentWidth - expectedWidth) > 1.0)
            {
                // User has manually resized, don't override
                return;
            }

            var targetWidth = expectedWidth;
            if (Math.Abs(currentWidth - targetWidth) > 0.1)
            {
                PaneColumn.SetCurrentValue(System.Windows.Controls.ColumnDefinition.WidthProperty, new GridLength(targetWidth));
            }
        }
        catch
        {
            // Ignore errors during initialization
        }
    }

    protected T GetTemplateChild<T>(string name)
        where T : DependencyObject
    {
        if (GetTemplateChild(name) is not T dependencyObject)
        {
            throw new ArgumentNullException(name);
        }

        return dependencyObject;
    }
}
