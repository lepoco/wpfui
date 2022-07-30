// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using Wpf.Ui.Animations;
using Wpf.Ui.Common;
using Wpf.Ui.Mvvm.Contracts;

namespace Wpf.Ui.Controls.Interfaces;

/// <summary>
/// Represents navigation class.
/// </summary>
public interface INavigation
{
    /// <summary>
    /// Service providing views.
    /// </summary>
    IPageService PageService { get; set; }

    /// <summary>
    /// Navigation item ID of the current page.
    /// <para>If set to a value less than <see langword="0"/>, no <see cref="Page"/> will be loaded during <see cref="INavigation"/> initialization.</para>
    /// </summary>
    int SelectedPageIndex { get; set; }

    /// <summary>
    /// Navigation item ID of the previous page.
    /// </summary>
    int PreviousPageIndex { get; }

    /// <summary>
    /// Creates an instance of all pages defined with <see cref="INavigationItem.PageType"/> after the <see cref="INavigation"/> is loaded.
    /// </summary>
    bool Precache { get; set; }

    /// <summary>
    /// Indicates the possibility of navigation back
    /// </summary>
    bool CanGoBack { get; }

    /// <summary>
    /// Currently used item like <see cref="INavigationItem"/>.
    /// </summary>
    INavigationItem Current { get; }

    /// <summary>
    /// Gets or sets the <see cref="System.Windows.Controls.Frame"/> in which the <see cref="System.Windows.Controls.Page"/> will be loaded after navigation.
    /// </summary>
    [Bindable(true)]
    Frame Frame { get; set; }

    /// <summary>
    /// Gets or sets the list of <see cref="INavigationControl"/> that will be displayed on the navigation.
    /// </summary>
    [Bindable(true)]
    ObservableCollection<INavigationControl> Items { get; set; }

    /// <summary>
    /// Gets or sets the list of <see cref="INavigationControl"/> which will be displayed at the bottom of the navigation and will not be scrolled.
    /// </summary>
    [Bindable(true)]
    ObservableCollection<INavigationControl> Footer { get; set; }

    /// <summary>
    /// Specifies dimension of children stacking.
    /// </summary>
    Orientation Orientation { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="RoutedNavigationEvent"/> that will be triggered during navigation.
    /// </summary>
    [Category("Behavior")]
    event RoutedNavigationEvent Navigated;

    /// <summary>
    /// Gets or sets the <see cref="RoutedNavigationEvent"/> that will be triggered during forward navigation.
    /// </summary>
    [Category("Behavior")]
    event RoutedNavigationEvent NavigatedForward;

    /// <summary>
    /// Gets or sets the <see cref="RoutedNavigationEvent"/> that will be triggered during backward navigation.
    /// </summary>
    [Category("Behavior")]
    event RoutedNavigationEvent NavigatedBackward;

    /// <summary>
    /// Gets or sets a value deciding how long the effect of the transition between the pages should take.
    /// </summary>
    [Bindable(true), Category("Appearance")]
    int TransitionDuration { get; set; }

    /// <summary>
    /// Gets or sets type of <see cref="Controls.Interfaces.INavigation"/> transitions during navigation.
    /// </summary>
    [Bindable(true), Category("Appearance")]
    TransitionType TransitionType { get; set; }

    /// <summary>
    /// Clears all navigation items.
    /// </summary>
    void Flush();

    /// <summary>
    /// Clears all initialized instances of the pages.
    /// </summary>
    void ClearCache();

    /// <summary>
    /// Navigates to the previous page using the <see cref="IPageService"/>.
    /// </summary>
    /// <returns></returns>
    bool NavigateBack();

    /// <summary>
    /// Navigates to the page using the <see cref="IPageService"/>.
    /// </summary>
    /// <param name="pageType">Type of the page to navigate.</param>
    /// <returns><see langword="true"/> if the operation was successful.</returns>
    bool Navigate(Type pageType);

    /// <summary>
    /// Navigates to the page using the <see cref="IPageService"/>.
    /// </summary>
    /// <param name="pageType">Type of the page to navigate.</param>
    /// <param name="dataContext">When an <see cref="System.Windows.Controls.Page"/> DataContext changes, all data-bound properties (on this element or any other element) whose Bindings use this DataContext will change to reflect the new value.</param>
    /// <returns><see langword="true"/> if the operation was successful.</returns>
    bool Navigate(Type pageType, object dataContext);

    /// <summary>
    /// Loads a <see cref="System.Windows.Controls.Page"/> instance into <see cref="Frame"/> based on the tag of <see cref="INavigationItem"/>.
    /// </summary>
    /// <param name="pageIndex">ID of the page to be loaded.</param>
    /// <returns><see langword="true"/> if the operation was successful.</returns>
    bool Navigate(int pageIndex);

    /// <summary>
    /// Loads a <see cref="System.Windows.Controls.Page"/> instance into <see cref="Frame"/> based on the tag of <see cref="INavigationItem"/>.
    /// </summary>
    /// <param name="pageIndex">ID of the page to be loaded.</param>
    /// <param name="dataContext">When an <see cref="System.Windows.Controls.Page"/> DataContext changes, all data-bound properties (on this element or any other element) whose Bindings use this DataContext will change to reflect the new value.</param>
    /// <returns><see langword="true"/> if the operation was successful.</returns>
    bool Navigate(int pageIndex, object dataContext);

    /// <summary>
    /// Loads a <see cref="System.Windows.Controls.Page"/> instance into <see cref="Frame"/> based on the tag of <see cref="INavigationItem"/>.
    /// </summary>
    /// <param name="pageTag"><see cref="INavigationItem.PageTag"/> to be loaded.</param>
    /// <returns><see langword="true"/> if the operation was successful.</returns>
    bool Navigate(string pageTag);

    /// <summary>
    /// Loads a <see cref="System.Windows.Controls.Page"/> instance into <see cref="Frame"/> based on the tag of <see cref="INavigationItem"/>.
    /// </summary>
    /// <param name="pageTag"><see cref="INavigationItem.PageTag"/> to be loaded.</param>
    /// <param name="dataContext">When an <see cref="System.Windows.Controls.Page"/> DataContext changes, all data-bound properties (on this element or any other element) whose Bindings use this DataContext will change to reflect the new value.</param>
    bool Navigate(string pageTag, object dataContext);

    /// <summary>
    /// Navigate to the given object that is outside the current navigation.
    /// </summary>
    /// <param name="frameworkElement">The element you want to navigate to a <see cref="Frame"/> that is not in the <see cref="Items"/> or <see cref="Footer"/> pool.</param>
    /// <returns><see langword="true"/> if the operation was successful.</returns>
    bool NavigateExternal(object frameworkElement);

    /// <summary>
    /// Navigate to the given object that is outside the current navigation.
    /// </summary>
    /// <param name="frameworkElement">The element you want to navigate to a <see cref="Frame"/> that is not in the <see cref="Items"/> or <see cref="Footer"/> pool.</param>
    /// <param name="dataContext">Context of the data for data binding.</param>
    /// <returns><see langword="true"/> if the operation was successful.</returns>
    bool NavigateExternal(object frameworkElement, object dataContext);

    /// <summary>
    /// Navigate to the given <see cref="Uri"/> that is outside the current navigation.
    /// </summary>
    /// <param name="absolutePageUri"><see cref="Uri"/> to the element you want to navigate to a <see cref="Frame"/> that is not in the <see cref="Items"/> or <see cref="Footer"/> pool.</param>
    /// <returns><see langword="true"/> if the operation was successful.</returns>
    bool NavigateExternal(Uri absolutePageUri);

    /// <summary>
    /// Navigate to the given <see cref="Uri"/> that is outside the current navigation.
    /// </summary>
    /// <param name="absolutePageUri"><see cref="Uri"/> to the element you want to navigate to a <see cref="Frame"/> that is not in the <see cref="Items"/> or <see cref="Footer"/> pool.</param>
    /// <param name="dataContext">Context of the data for data binding.</param>
    /// <returns><see langword="true"/> if the operation was successful.</returns>
    bool NavigateExternal(Uri absolutePageUri, object dataContext);

    /// <summary>
    /// Sets <see cref="System.Windows.FrameworkElement.DataContext"/> of the page.
    /// <para>If the page is not in the Cache, and is defined based on <see cref="INavigationItem.PageType"/>, its object will be created and then its DataContext will be defined.</para>
    /// </summary>
    /// <param name="pageId">Id of the page from <see cref="Items"/> or <see cref="Footer"/>.</param>
    /// <param name="dataContext">Context of the data for data binding.</param>
    /// <returns><see langword="true"/> if the operation was successful.</returns>
    bool SetContext(int pageId, object dataContext);

    /// <summary>
    /// Sets <see cref="System.Windows.FrameworkElement.DataContext"/> of the page.
    /// <para>If the page is not in the Cache, and is defined based on <see cref="INavigationItem.PageType"/>, its object will be created and then its DataContext will be defined.</para>
    /// </summary>
    /// <param name="pageTag">Tag of the page from <see cref="Items"/> or <see cref="Footer"/>.</param>
    /// <param name="dataContext">Context of the data for data binding.</param>
    /// <returns><see langword="true"/> if the operation was successful.</returns>
    bool SetContext(string pageTag, object dataContext);

    /// <summary>
    /// Tires to set the DataContext for the currently displayed page.
    /// </summary>
    /// <param name="dataContext">Data context to be set.</param>
    void SetCurrentContext(object dataContext);
}
