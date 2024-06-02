// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Abstractions;
using Wpf.Ui.Controls;

namespace Wpf.Ui;

/// <summary>
/// Represents a contract with a <see cref="System.Windows.Window"/> that contains <see cref="INavigationView"/>.
/// Through defined <see cref="IPageService"/> service allows you to use the MVVM model in <c>WPF UI</c> navigation.
/// </summary>
public interface INavigationWindow
{
    /// <summary>
    /// Provides direct access to the control responsible for navigation.
    /// </summary>
    /// <returns>Instance of the <see cref="INavigationView"/> control.</returns>
    INavigationView GetNavigation();

    /// <summary>
    /// Lets you navigate to the selected page based on it's type. Should be used with <see cref="IPageService"/>.
    /// </summary>
    /// <param name="pageType"><see langword="Type"/> of the page.</param>
    /// <returns><see langword="true"/> if the operation succeeds. <see langword="false"/> otherwise.</returns>
    bool Navigate(Type pageType);

    /// <summary>
    /// Lets you attach the service provider that delivers page instances to <see cref="INavigationView"/>.
    /// </summary>
    /// <param name="serviceProvider">Instance of the <see cref="IServiceProvider"/>.</param>
    void SetServiceProvider(IServiceProvider serviceProvider);

    /// <summary>
    /// Lets you attach the service that delivers page instances to <see cref="INavigationView"/>.
    /// </summary>
    /// <param name="pageService">Instance of the <see cref="IPageService"/> with attached service provider.</param>
    void SetPageService(IPageService pageService);

    /// <summary>
    /// Triggers the command to open a window.
    /// </summary>
    void ShowWindow();

    /// <summary>
    /// Triggers the command to close a window.
    /// </summary>
    void CloseWindow();
}
