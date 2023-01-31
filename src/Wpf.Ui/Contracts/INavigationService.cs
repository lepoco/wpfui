// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using Wpf.Ui.Controls.Navigation;

namespace Wpf.Ui.Contracts;

/// <summary>
/// Represents a contract with a <see cref="System.Windows.FrameworkElement"/> that contains <see cref="INavigation"/>.
/// Through defined <see cref="IPageService"/> service allows you to use the Dependency Injection pattern in <c>WPF UI</c> navigation.
/// </summary>
public interface INavigationService
{
    /// <summary>
    /// Provides direct access to the control responsible for navigation.
    /// </summary>
    /// <returns>Instance of the <see cref="INavigation"/> control.</returns>
    INavigationView GetNavigationControl();

    /// <summary>
    /// Lets you attach the control that represents the <see cref="INavigation"/>.
    /// </summary>
    /// <param name="navigation">Instance of the <see cref="INavigation"/>.</param>
    void SetNavigationControl(INavigationView navigation);

    /// <summary>
    /// Lets you attach the service that delivers page instances to <see cref="INavigation"/>.
    /// </summary>
    /// <param name="pageService">Instance of the <see cref="IPageService"/>.</param>
    void SetPageService(IPageService pageService);

    /// <summary>
    /// Lets you navigate to the selected page based on it's type. Should be used with <see cref="IPageService"/>.
    /// </summary>
    /// <param name="pageType"><see langword="Type"/> of the page.</param>
    bool Navigate(Type pageType);

    /// <summary>
    /// Lets you navigate to the selected page based on it's tag. Should be used with <see cref="IPageService"/>.
    /// </summary>
    /// <param name="pageIdOrTargetTag">Id or tag of the page.</param>
    bool Navigate(string pageIdOrTargetTag);

    /// <summary>
    /// Navigates the NavigationView to the previous journal entry. 
    /// </summary>
    /// <returns></returns>
    bool GoBack();

    /// <summary>
    /// Synchronously adds an element to the navigation stack and navigates current navigation Frame to the
    /// </summary>
    /// <param name="pageType"></param>
    /// <returns></returns>
    bool NavigateWithHierarchy(Type pageType);
}
