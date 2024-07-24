// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Microsoft.Extensions.DependencyInjection;
using Wpf.Ui.Abstractions;

namespace Wpf.Ui.DependencyInjection;

/// <summary>
/// Provides extension methods for <see cref="IServiceCollection"/> to support WPF UI navigation and services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the services necessary for page navigation within a WPF UI NavigationView.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>

    public static IServiceCollection AddNavigationViewPageProvider(this IServiceCollection services)
    {
        _ = services.AddSingleton<
            INavigationViewPageProvider,
            DependencyInjectionNavigationViewPageProvider
        >();

        return services;
    }
}
