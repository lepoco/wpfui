// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using Wpf.Ui.Tray.Controls;

namespace Wpf.Ui.Tray;

/// <summary>
/// Event triggered on successful navigation.
/// </summary>
/// <param name="sender">Source of the event, which should be the current navigation instance.</param>
/// <param name="e">Event data containing information about the navigation event.</param>
#if NET5_0_OR_GREATER
public delegate void RoutedNotifyIconEvent([NotNull] NotifyIcon sender, RoutedEventArgs e);
#else
public delegate void RoutedNotifyIconEvent(NotifyIcon sender, RoutedEventArgs e);
#endif
