// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace Wpf.Ui.Common;

/// <summary>
/// Event triggered on via <see cref="Controls.Dialog"/>.
/// </summary>
/// <param name="sender">Current <see cref="Controls.Dialog"/> instance.</param>
#if NET5_0_OR_GREATER
public delegate void RoutedDialogEvent([NotNull] Controls.Dialog sender, RoutedEventArgs e);
#else
public delegate void RoutedDialogEvent(Controls.Dialog sender, RoutedEventArgs e);
#endif
