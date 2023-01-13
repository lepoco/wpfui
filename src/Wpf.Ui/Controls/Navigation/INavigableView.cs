// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Controls.Navigation;

/// <summary>
/// A component whose ViewModel is separate from the DataContext and can be navigated by <see cref="INavigationView"/>.
/// </summary>
public interface INavigableView<out T>
{
    /// <summary>
    /// ViewModel used by the view.
    /// Optionally, it may implement <see cref="INavigationAware"/> and be navigated by <see cref="INavigationView"/>.
    /// </summary>
    T ViewModel { get; }
}
