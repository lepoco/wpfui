// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Abstractions;

namespace Wpf.Ui.Demo.Mvvm.Services;

/// <summary>
/// Service that provides pages for navigation.
/// </summary>
public class PageService(IServiceProvider serviceProvider) : IPageService
{
    /// <inheritdoc />
    public object? GetPage(Type pageType)
    {
        if (!typeof(FrameworkElement).IsAssignableFrom(pageType))
        {
            throw new InvalidOperationException("The page should be a WPF control.");
        }

        return serviceProvider.GetService(pageType);
    }
}
