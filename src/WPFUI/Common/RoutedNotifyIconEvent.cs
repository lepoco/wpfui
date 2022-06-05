// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Diagnostics.CodeAnalysis;
using System.Windows;
using WPFUI.Tray;

namespace WPFUI.Common;

/// <summary>
/// Event triggered on successful navigation.
/// </summary>
/// <param name="sender">Current navigation instance.</param>
#if NET5_0_OR_GREATER
public delegate void RoutedNotifyIconEvent([NotNull] INotifyIcon sender, RoutedEventArgs e);
#else
public delegate void RoutedNotifyIconEvent(INotifyIcon sender, RoutedEventArgs e);
#endif
