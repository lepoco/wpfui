// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Threading;

namespace Wpf.Ui.Abstractions.Controls;

/// <summary>
/// Notifies class about being navigated.
/// </summary>
public interface INavigationAware
{
    /// <summary>
    /// Asynchronously handles the event that is fired after the component is navigated to.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    ValueTask OnNavigatedToAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously handles the event that is fired before the component is navigated from.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    ValueTask OnNavigatedFromAsync(CancellationToken cancellationToken = default);
}
