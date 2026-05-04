// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Threading;

namespace Wpf.Ui.Abstractions.Controls;

/// <summary>
/// Provides a base class for navigation-aware components.
/// </summary>
public abstract class NavigationAware : INavigationAware
{
    /// <inheritdoc />
    public virtual ValueTask OnNavigatedToAsync(CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return default;
        }

        OnNavigatedTo(cancellationToken);

        return default;
    }

    /// <summary>
    /// Handles the event that is fired after the component is navigated to.
    /// </summary>
    // ReSharper disable once MemberCanBeProtected.Global
    public virtual void OnNavigatedTo(CancellationToken cancellationToken = default) { }

    /// <inheritdoc />
    public virtual ValueTask OnNavigatedFromAsync(CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return default;
        }

        OnNavigatedFrom(cancellationToken);

        return default;
    }

    /// <summary>
    /// Handles the event that is fired before the component is navigated from.
    /// </summary>
    // ReSharper disable once MemberCanBeProtected.Global
    public virtual void OnNavigatedFrom(CancellationToken cancellationToken = default) { }
}
