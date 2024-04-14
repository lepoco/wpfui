// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// A component whose ViewModel is separate from the DataContext and can be navigated by <see cref="INavigationView"/>.
/// </summary>
/// <typeparam name="T">The type of the ViewModel associated with the view. This type optionally may implement <see cref="INavigationAware"/> to participate in navigation processes.</typeparam>
public interface INavigableView<out T>
{
    /// <summary>
    /// Gets the view model used by the view.
    /// Optionally, it may implement <see cref="INavigationAware"/> and be navigated by <see cref="INavigationView"/>.
    /// </summary>
    T ViewModel { get; }
}
