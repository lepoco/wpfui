// Based on Windows UI Library
// Copyright(c) Microsoft Corporation.All rights reserved.

// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace Wpf.Ui.Controls.Navigation;

/// <summary>
/// Event triggered by NavigationView.
/// </summary>
/// <param name="sender">Current navigation instance.</param>
#if NET5_0_OR_GREATER
public delegate void NavigationViewEvent([NotNull] object sender, RoutedEventArgs e);
#else
public delegate void NavigationViewEvent(object sender, RoutedEventArgs e);
#endif
