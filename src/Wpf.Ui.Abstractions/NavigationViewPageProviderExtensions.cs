// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Abstractions;

/// <summary>
/// Provides extension methods for the INavigationViewPageProvider interface.
/// </summary>
public static class NavigationViewPageProviderExtensions
{
    /// <summary>
    /// Retrieves a page of the specified type from the page service.
    /// </summary>
    /// <typeparam name="TPage">The type of the page to retrieve.</typeparam>
    /// <param name="navigationViewPageProvider">The page service instance.</param>
    /// <returns>An instance of the specified page type, or null if the page is not found.</returns>
    public static TPage? GetPage<TPage>(this INavigationViewPageProvider navigationViewPageProvider)
        where TPage : class
    {
        return navigationViewPageProvider.GetPage(typeof(TPage)) as TPage;
    }

    /// <summary>
    /// Retrieves a page of the specified type from the page service.
    /// Throws a NavigationException if the page is not found.
    /// </summary>
    /// <typeparam name="TPage">The type of the page to retrieve.</typeparam>
    /// <param name="navigationViewPageProvider">The page service instance.</param>
    /// <returns>An instance of the specified page type.</returns>
    /// <exception cref="NavigationException">Thrown when the specified page type is not found.</exception>
    public static TPage GetRequiredPage<TPage>(this INavigationViewPageProvider navigationViewPageProvider)
        where TPage : class
    {
        return navigationViewPageProvider.GetPage(typeof(TPage)) as TPage
            ?? throw new NavigationException($"{typeof(TPage)} page not found.");
    }
}
