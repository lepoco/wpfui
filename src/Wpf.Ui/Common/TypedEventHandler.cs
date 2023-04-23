// Based on Windows UI Library
// Copyright(c) Microsoft Corporation.All rights reserved.

// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;

namespace Wpf.Ui.Common;

/// <summary>
/// Represents a method that handles general events.
/// </summary>
/// <typeparam name="TSender"></typeparam>
/// <typeparam name="TArgs"></typeparam>
/// <param name="sender"></param>
/// <param name="args"></param>
public delegate void TypedEventHandler<in TSender, in TArgs>(TSender sender, TArgs args)
    where TSender : DependencyObject
    where TArgs : RoutedEventArgs;
