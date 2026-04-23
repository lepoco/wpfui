// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Abstractions.Controls;

namespace Wpf.Ui.Demo.Mvvm.ViewModels;

public abstract class ViewModel : ObservableObject, INavigationAware
{
    /// <inheritdoc />
    public virtual ValueTask OnNavigatedToAsync(CancellationToken cancellationToken = default)
    {
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
        OnNavigatedFrom(cancellationToken);

        return default;
    }

    /// <summary>
    /// Handles the event that is fired before the component is navigated from.
    /// </summary>
    // ReSharper disable once MemberCanBeProtected.Global
    public virtual void OnNavigatedFrom(CancellationToken cancellationToken = default) { }
}
