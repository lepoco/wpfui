// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Controls;

namespace Wpf.Ui.Extensions;

/// <summary>
/// Extensions for the <see cref="INavigationService"/>.
/// </summary>
public static class NavigationServiceExtensions
{
    /// <summary>
    /// Sets the pane display mode of the navigation service.
    /// </summary>
    /// <param name="navigationService">The navigation service.</param>
    /// <param name="paneDisplayMode">The pane display mode.</param>
    /// <returns>Same <see cref="INavigationService"/> so multiple calls can be chained.</returns>
    public static INavigationService SetPaneDisplayMode(
        this INavigationService navigationService,
        NavigationViewPaneDisplayMode paneDisplayMode
    )
    {
        INavigationView? navigationControl = navigationService.GetNavigationControl();

        if (navigationControl is not null)
        {
            navigationControl.PaneDisplayMode = paneDisplayMode;
        }

        return navigationService;
    }
}
