// Based on Windows UI Library
// Copyright(c) Microsoft Corporation.All rights reserved.

// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Animations;
using Wpf.Ui.Common;
using Wpf.Ui.Contracts;
using Wpf.Ui.Controls.AutoSuggestBoxControl;
using Wpf.Ui.Controls.BreadcrumbControl;
using Wpf.Ui.Controls.TitleBarControl;

namespace Wpf.Ui.Controls.Navigation;

/// <summary>
/// Represents a container that enables navigation of app content. It has a header, a view for the main content, and a menu pane for navigation commands.
/// </summary>
public interface INavigationView
{
    /// <summary>
    /// Gets or sets the header content.
    /// </summary>
    object? Header { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="Header"/> visibility.
    /// </summary>
    Visibility HeaderVisibility { get; set; }

    /// <summary>
    /// Gets or sets a value that indicates whether the header is always visible.
    /// </summary>
    bool AlwaysShowHeader { get; set; }

    /// <summary>
    /// Gets the collection of menu items displayed in the NavigationView.
    /// </summary>
    IList MenuItems { get; set; }

    /// <summary>
    /// Gets or sets an object source used to generate the content of the NavigationView menu.
    /// </summary>
    object? MenuItemsSource { get; set; }

    /// <summary>
    /// Gets the list of objects to be used as navigation items in the footer menu.
    /// </summary>
    IList FooterMenuItems { get; set; }

    /// <summary>
    /// Gets or sets the object that represents the navigation items to be used in the footer menu.
    /// </summary>
    object? FooterMenuItemsSource { get; set; }

    /// <summary>
    /// Gets the selected item.
    /// </summary>
    INavigationViewItem? SelectedItem { get; }

    /// <summary>
    /// Gets or sets a UI element that is shown at the top of the control, below the pane if PaneDisplayMode is Top.
    /// </summary>
    object? ContentOverlay { get; set; }

    /// <summary>
    /// Gets a value that indicates whether the back button is enabled or disabled.
    /// </summary>
    bool IsBackEnabled { get; }

    /// <summary>
    /// Gets or sets a value that indicates whether the back button is visible or not.
    /// Default value is "Auto", which indicates that button visibility depends on the DisplayMode setting of the NavigationView.
    /// </summary>
    NavigationViewBackButtonVisible IsBackButtonVisible { get; set; }

    /// <summary>
    /// Gets or sets a value that indicates whether the toggle button is visible.
    /// </summary>
    bool IsPaneToggleVisible { get; set; }

    /// <summary>
    /// Gets or sets a value that specifies whether the NavigationView pane is expanded to its full width.
    /// </summary>
    bool IsPaneOpen { get; set; }

    /// <summary>
    /// Gets or sets a value that determines whether the pane is shown.
    /// </summary>
    bool IsPaneVisible { get; set; }

    /// <summary>
    /// Gets or sets the width of the NavigationView pane when it's fully expanded.
    /// </summary>
    double OpenPaneLength { get; set; }

    /// <summary>
    /// Gets or sets the width of the NavigationView pane in its compact display mode.
    /// </summary>
    double CompactPaneLength { get; set; }

    /// <summary>
    /// Gets or sets the content for the pane header.
    /// </summary>
    object? PaneHeader { get; set; }

    /// <summary>
    /// Gets or sets the label adjacent to the menu icon when the NavigationView pane is open.
    /// </summary>
    string? PaneTitle { get; set; }

    /// <summary>
    /// Gets or sets the content for the pane footer.
    /// </summary>
    object? PaneFooter { get; set; }

    /// <summary>
    /// Gets a value that specifies how the pane and content areas of a NavigationView are being shown.
    /// <para>It is not the same PaneDisplayMode as in WinUi.</para>
    /// </summary>
    NavigationViewPaneDisplayMode PaneDisplayMode { get; set; }

    /// <summary>
    /// Gets or sets an TitleBar to be displayed in the NavigationView.
    /// </summary>
    TitleBar? TitleBar { get; set; }

    /// <summary>
    /// Gets or sets an AutoSuggestBox to be displayed in the NavigationView.
    /// </summary>
    AutoSuggestBox? AutoSuggestBox { get; set; }

    /// <summary>
    /// Gets or sets an BreadcrumbBar that is in <see cref="Header"/>.
    /// </summary>
    BreadcrumbBar? BreadcrumbBar { get; set; }

    /// <summary>
    /// Template Property for <see cref="MenuItems"/> and <see cref="FooterMenuItems"/>.
    /// </summary>
    ControlTemplate? ItemTemplate { get; set; }

    /// <summary>
    /// Gets or sets a value deciding how long the effect of the transition between the pages should take.
    /// </summary>
    int TransitionDuration { get; set; }

    /// <summary>
    /// Gets or sets type of <see cref="INavigationView"/> transitions during navigation.
    /// </summary>
    TransitionType TransitionType { get; set; }

    /// <summary>
    /// Gets or sets margin for a Frame of <see cref="INavigationView"/>
    /// </summary>
    Thickness FrameMargin { get; set; }

    /// <summary>
    /// Occurs when the NavigationView pane is opened.
    /// </summary>
    event TypedEventHandler<NavigationView, RoutedEventArgs> PaneOpened;

    /// <summary>
    /// Occurs when the NavigationView pane is closed.
    /// </summary>
    event TypedEventHandler<NavigationView, RoutedEventArgs> PaneClosed;

    /// <summary>
    /// Occurs when the currently selected item changes.
    /// </summary>
    event TypedEventHandler<NavigationView, RoutedEventArgs> SelectionChanged;

    /// <summary>
    /// Occurs when an item in the menu receives an interaction such as a click or tap.
    /// </summary>
    event TypedEventHandler<NavigationView, RoutedEventArgs> ItemInvoked;

    /// <summary>
    /// Occurs when the back button receives an interaction such as a click or tap.
    /// </summary>
    event TypedEventHandler<NavigationView, RoutedEventArgs> BackRequested;

    /// <summary>
    /// Occurs when a new navigation is requested
    /// </summary>
    event TypedEventHandler<NavigationView, NavigatingCancelEventArgs> Navigating;

    /// <summary>
    /// Gets a value that indicates whether there is at least one entry in back navigation history.
    /// </summary>
    bool CanGoBack { get; }

    /// <summary>
    /// Synchronously navigates current navigation Frame to the
    /// given Element.
    /// </summary>
    bool Navigate(Type pageType, object? dataContext = null);

    /// <summary>
    /// Synchronously navigates current navigation Frame to the
    /// given Element.
    /// </summary>
    bool Navigate(string pageIdOrTargetTag, object? dataContext = null);

    /// <summary>
    /// Synchronously adds an element to the navigation stack and navigates current navigation Frame to the
    /// </summary>
    /// <param name="pageType"></param>
    /// <param name="dataContext"></param>
    /// <returns></returns>
    bool NavigateWithHierarchy(Type pageType, object? dataContext = null);

    /// <summary>
    /// Replaces the contents of the navigation frame, without changing the currently selected item or triggering an <see cref="SelectionChanged"/>.
    /// </summary>
    bool ReplaceContent(Type pageTypeToEmbed);

    /// <summary>
    /// Replaces the contents of the navigation frame, without changing the currently selected item or triggering an <see cref="SelectionChanged"/>.
    /// </summary>
    bool ReplaceContent(UIElement pageInstanceToEmbed, object? dataContext = null);

    /// <summary>
    /// Navigates the NavigationView to the next journal entry.
    /// </summary>
    /// <returns><see langword="true"/> if successfully navigated forward, otherwise <see langword="false"/>.</returns>
    bool GoForward();

    /// <summary>
    /// Navigates the NavigationView to the previous journal entry. 
    /// </summary>
    /// <returns><see langword="true"/> if successfully navigated backward, otherwise <see langword="false"/>.</returns>
    bool GoBack();

    /// <summary>
    /// Clears the NavigationView history.
    /// </summary>
    void ClearJournal();

    /// <summary>
    /// Allows you to assign to the NavigationView a special service responsible for retrieving the page instances.
    /// </summary>
    void SetPageService(IPageService pageService);

    /// <summary>
    /// Allows you to assign a general <see cref="IServiceProvider"/> to the NavigationView that will be used to retrieve page instances and view models.
    /// </summary>
    void SetServiceProvider(IServiceProvider serviceProvider);
}
