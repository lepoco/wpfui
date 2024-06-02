// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Abstractions;
using Wpf.Ui.Controls;

namespace Wpf.Ui;

/// <summary>
/// Represents a contract with a <see cref="System.Windows.FrameworkElement"/> that contains <see cref="INavigationView"/>.
/// Through defined <see cref="IPageService"/> service allows you to use the Dependency Injection pattern in <c>WPF UI</c> navigation.
/// </summary>
public interface INavigationService
{
    /// <summary>
    /// Lets you navigate to the selected page based on it's type. Should be used with <see cref="IPageService"/>.
    /// </summary>
    /// <param name="pageType"><see langword="Type"/> of the page.</param>
    /// <returns><see langword="true"/> if the operation succeeds. <see langword="false"/> otherwise.</returns>
    bool Navigate(Type pageType);

    /// <summary>
    /// Lets you navigate to the selected page based on it's type, Should be used with <see cref="IPageService"/>.
    /// </summary>
    /// <param name="pageType"><see langword="Type"/> of the page.</param>
    /// <param name="dataContext">DataContext <see cref="object"/></param>
    /// <returns><see langword="true"/> if the operation succeeds. <see langword="false"/> otherwise.</returns>
    bool Navigate(Type pageType, object? dataContext);

    /// <summary>
    /// Lets you navigate to the selected page based on it's tag. Should be used with <see cref="IPageService"/>.
    /// </summary>
    /// <param name="pageIdOrTargetTag">Id or tag of the page.</param>
    /// <returns><see langword="true"/> if the operation succeeds. <see langword="false"/> otherwise.</returns>
    bool Navigate(string pageIdOrTargetTag);

    /// <summary>
    /// Lets you navigate to the selected page based on it's tag. Should be used with <see cref="IPageService"/>.
    /// </summary>
    /// <param name="pageIdOrTargetTag">Id or tag of the page.</param>
    /// <param name="dataContext">DataContext <see cref="object"/></param>
    /// <returns><see langword="true"/> if the operation succeeds. <see langword="false"/> otherwise.</returns>
    bool Navigate(string pageIdOrTargetTag, object? dataContext);

    /// <summary>
    /// Synchronously adds an element to the navigation stack and navigates current navigation Frame to the
    /// </summary>
    /// <param name="pageType">Type of control to be synchronously added to the navigation stack</param>
    /// <returns><see langword="true"/> if the operation succeeds. <see langword="false"/> otherwise.</returns>
    bool NavigateWithHierarchy(Type pageType);

    /// <summary>
    /// Synchronously adds an element to the navigation stack and navigates current navigation Frame to the
    /// </summary>
    /// <param name="pageType">Type of control to be synchronously added to the navigation stack</param>
    /// <param name="dataContext">DataContext <see cref="object"/></param>
    /// <returns><see langword="true"/> if the operation succeeds. <see langword="false"/> otherwise.</returns>
    bool NavigateWithHierarchy(Type pageType, object? dataContext);

    /// <summary>
    /// Provides direct access to the control responsible for navigation.
    /// </summary>
    /// <returns>Instance of the <see cref="INavigationView"/> control.</returns>
    INavigationView GetNavigationControl();

    /// <summary>
    /// Lets you attach the control that represents the <see cref="INavigationView"/>.
    /// </summary>
    /// <param name="navigation">Instance of the <see cref="INavigationView"/>.</param>
    void SetNavigationControl(INavigationView navigation);

    /// <summary>
    /// Lets you attach the service that delivers page instances to <see cref="INavigationView"/>.
    /// </summary>
    /// <param name="pageService">Instance of the <see cref="IPageService"/>.</param>
    void SetPageService(IPageService pageService);

    /// <summary>
    /// Navigates the NavigationView to the previous journal entry.
    /// </summary>
    /// <returns><see langword="true"/> if the operation succeeds. <see langword="false"/> otherwise.</returns>
    bool GoBack();
}
