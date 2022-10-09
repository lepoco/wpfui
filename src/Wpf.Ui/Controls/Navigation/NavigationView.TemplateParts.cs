// Based on Windows UI Library
// Copyright(c) Microsoft Corporation.All rights reserved.

// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;

namespace Wpf.Ui.Controls.Navigation;

[TemplatePart(Name = "PART_NavigationViewContentPresenter", Type = typeof(NavigationViewContentPresenter))]
[TemplatePart(Name = "PART_MenuItemsItemsControl", Type = typeof(System.Windows.Controls.ItemsControl))]
[TemplatePart(Name = "PART_FooterMenuItemsItemsControl", Type = typeof(System.Windows.Controls.ItemsControl))]
[TemplatePart(Name = "PART_BackButton", Type = typeof(System.Windows.Controls.Button))]
[TemplatePart(Name = "PART_ToggleButton", Type = typeof(System.Windows.Controls.Button))]
public partial class NavigationView
{
    /// <summary>
    /// Template element represented by the <c>PART_MenuItemsItemsControl</c> name.
    /// </summary>
    private const string TemplateElementNavigationViewContentPresenter = "PART_NavigationViewContentPresenter";

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
    /// Control responsible for rendering the content.
    /// </summary>
    protected NavigationViewContentPresenter NavigationViewContentPresenter;

    /// <summary>
    /// Control located at the top of the pane with left arrow icon.
    /// </summary>
    protected System.Windows.Controls.ItemsControl MenuItemsItemsControl;

    /// <summary>
    /// Control located at the top of the pane with hamburger icon.
    /// </summary>
    protected System.Windows.Controls.ItemsControl FooterMenuItemsItemsControl;

    /// <summary>
    /// Control located at the top of the pane with left arrow icon.
    /// </summary>
    protected System.Windows.Controls.Button BackButton;

    /// <summary>
    /// Control located at the top of the pane with hamburger icon.
    /// </summary>
    protected System.Windows.Controls.Button ToggleButton;

    /// <inheritdoc />
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if (GetTemplateChild(TemplateElementNavigationViewContentPresenter) is NavigationViewContentPresenter navigationViewContentPresenter)
            NavigationViewContentPresenter = navigationViewContentPresenter;

        if (GetTemplateChild(TemplateElementMenuItemsItemsControl) is System.Windows.Controls.ItemsControl menuItemsItemsControl)
            MenuItemsItemsControl = menuItemsItemsControl;

        if (GetTemplateChild(TemplateElementFooterMenuItemsItemsControl) is System.Windows.Controls.ItemsControl footerMenuItemsItemsControl)
            FooterMenuItemsItemsControl = footerMenuItemsItemsControl;

        if (GetTemplateChild(TemplateElementBackButton) is System.Windows.Controls.Button backButton)
            BackButton = backButton;

        if (GetTemplateChild(TemplateElementToggleButton) is System.Windows.Controls.Button toggleButton)
            ToggleButton = toggleButton;

        if (NavigationViewContentPresenter != null)
            NavigationViewContentPresenter.Navigated += OnNavigationViewContentPresenterNavigated;

        if (MenuItemsItemsControl != null)
            MenuItemsItemsControl.ItemsSource = MenuItems;

        if (FooterMenuItemsItemsControl != null)
            FooterMenuItemsItemsControl.ItemsSource = FooterMenuItems;

        if (BackButton != null)
        {
            BackButton.Click -= OnBackButtonClick;
            BackButton.Click += OnBackButtonClick;
        }

        if (ToggleButton != null)
        {
            ToggleButton.Click -= OnToggleButtonClick;
            ToggleButton.Click += OnToggleButtonClick;
        }
    }
}
