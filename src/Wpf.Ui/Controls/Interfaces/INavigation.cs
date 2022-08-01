// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

#nullable enable
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
    /// Clears all initialized instances of the pages.
    /// </summary>
    void ClearCache();

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="pageTag"></param>
    /// <param name="dataContext"></param>
    void NavigateTo(string pageTag, object? dataContext = null);

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="type"></param>
    /// <param name="dataContext"></param>
    void NavigateTo(Type type, object? dataContext = null);
}
