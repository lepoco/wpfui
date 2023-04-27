// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Diagnostics.CodeAnalysis;
using System.Windows;
using Wpf.Ui.Controls.SnackbarControl;

namespace Wpf.Ui.Common;

/// <summary>
/// Event triggered on via <see cref="Controls.SnackbarControl.Snackbar"/>.
/// </summary>
/// <param name="sender">Current <see cref="Controls.SnackbarControl.Snackbar"/> instance.</param>
#if NET5_0_OR_GREATER

/* Unmerged change from project 'Wpf.Ui (net6.0-windows)'
Before:
public delegate void RoutedSnackbarEvent([NotNull] Controls.Snackbar sender, RoutedEventArgs e);
After:
public delegate void RoutedSnackbarEvent([NotNull] Controls.SnackbarControl.Snackbar sender, RoutedEventArgs e);
*/
public delegate void RoutedSnackbarEvent([NotNull] Snackbar sender, RoutedEventArgs e);
#else
public delegate void RoutedSnackbarEvent(Controls.SnackbarControl.Snackbar sender, RoutedEventArgs e);
#endif
