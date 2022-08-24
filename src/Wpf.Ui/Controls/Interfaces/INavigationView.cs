// Based on Windows UI Library
// Copyright(c) Microsoft Corporation.All rights reserved.

// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.Generic;
using Wpf.Ui.Controls.Navigation;

namespace Wpf.Ui.Controls.Interfaces;

/// <summary>
/// Represents a container that enables navigation of app content. It has a header, a view for the main content, and a menu pane for navigation commands.
/// </summary>
public interface INavigationView
{
    /// <summary>
    /// Gets the collection of menu items displayed in the NavigationView.
    /// </summary>
    IList<object> MenuItems { get; set; }

    /// <summary>
    /// Gets or sets an object source used to generate the content of the NavigationView menu.
    /// </summary>
    object MenuItemsSource { get; set; }

    /// <summary>
    /// Gets the list of objects to be used as navigation items in the footer menu.
    /// </summary>
    IList<object> FooterMenuItems { get; set; }

    /// <summary>
    /// Gets or sets the object that represents the navigation items to be used in the footer menu.
    /// </summary>
    object FooterMenuItemsSource { get; set; }

    /// <summary>
    /// Gets or sets a UI element that is shown at the top of the control, below the pane if PaneDisplayMode is Top.
    /// </summary>
    object ContentOverlay { get; set; }

    /// <summary>
    /// Gets or sets a value that indicates whether the back button is enabled or disabled.
    /// </summary>
    bool IsBackEnabled { get; set; }

    /// <summary>
    /// Gets or sets a value that indicates whether the back button is visible or not.
    /// Default value is "Auto", which indicates that button visibility depends on the DisplayMode setting of the NavigationView.
    /// </summary>
    NavigationViewBackButtonVisible IsBackButtonVisible { get; set; }

    /// <summary>
    /// Gets or sets a value that specifies whether the NavigationView pane is expanded to its full width.
    /// </summary>
    bool IsPaneOpen { get; set; }

    /// <summary>
    /// Gets or sets a value that determines whether the pane is shown.
    /// </summary>
    bool IsPaneVisible { get; set; }

    /// <summary>
    /// Determines the visual style of the NavigationView.
    /// </summary>
    NavigationViewVisualStyle VisualStyle { get; set; }

    /// <summary>
    /// Occurs when the NavigationView pane is opened.
    /// </summary>
    event NavigationViewEvent PaneOpened;

    /// <summary>
    /// Occurs when the NavigationView pane is closed.
    /// </summary>
    event NavigationViewEvent PaneClosed;

    /// <summary>
    /// Occurs when the currently selected item changes.
    /// </summary>
    event NavigationViewEvent SelectionChanged;

    /// <summary>
    /// Occurs when an item in the menu receives an interaction such as a click or tap.
    /// </summary>
    event NavigationViewEvent ItemInvoked;

    /// <summary>
    /// Occurs when the back button receives an interaction such as a click or tap.
    /// </summary>
    event NavigationViewEvent BackRequested;
}
