// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Abstractions;

namespace Wpf.Ui.DependencyInjection;

/// <summary>
/// Service that provides pages for navigation.
/// </summary>
public class DependencyInjectionNavigationViewPageProvider(IServiceProvider serviceProvider)
    : INavigationViewPageProvider
{
    /// <inheritdoc />
    public object? GetPage(Type pageType)
    {
        return serviceProvider.GetService(pageType);
    }
}
