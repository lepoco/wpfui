// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

// Based on Windows UI Library
// Copyright(c) Microsoft Corporation.All rights reserved.

using System.Windows.Controls;

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
[TemplatePart(Name = TemplateElementContentScrollViewer, Type = typeof(ScrollViewer))]
public partial class NavigationView
{
    /// <summary>
    /// Template element represented by the <c>PART_NavigationViewContentPresenter</c> name.
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
    /// Template element represented by the <c>PART_ContentScrollViewer</c> name.
    /// </summary>
    private const string TemplateElementContentScrollViewer = "PART_ContentScrollViewer";

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
    /// Gets or sets the <see cref="ScrollViewer"/> that hosts the content of the <see cref="Page"/>.
    /// </summary>
    protected ScrollViewer? ContentScrollViewer { get; set; }

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
            
            // We need to wait for the NavigationViewContentPresenter to apply its template
            // so we can find the ContentScrollViewer
            NavigationViewContentPresenter.ApplyTemplate();
            FindContentScrollViewer();
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
    }

    private void FindContentScrollViewer()
    {
        if (NavigationViewContentPresenter?.Template is null)
        {
            return;
        }

        // Try to find the ContentScrollViewer within the NavigationViewContentPresenter's template
        if (NavigationViewContentPresenter.Template.FindName(TemplateElementContentScrollViewer, NavigationViewContentPresenter) is ScrollViewer scrollViewer)
        {
            ContentScrollViewer = scrollViewer;
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
