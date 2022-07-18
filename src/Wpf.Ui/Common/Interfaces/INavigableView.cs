// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Controls.Interfaces;

namespace Wpf.Ui.Common.Interfaces;

/// <summary>
/// A view whose ViewModel is separate from the DataContext and can be navigated by <see cref="INavigation"/>.
/// </summary>
public interface INavigableView<out T>
{
    /// <summary>
    /// ViewModel used by the view.
    /// Optionally, it may implement <see cref="INavigationAware"/> and be navigated by <see cref="INavigation"/>.
    /// </summary>
    T ViewModel { get; }
}
