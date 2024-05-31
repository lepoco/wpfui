// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

/* Based on Windows UI Library
   Copyright(c) Microsoft Corporation.All rights reserved. */

namespace Wpf.Ui.Controls;

/// <summary>
/// Represents a method that handles general events.
/// </summary>
/// <typeparam name="TSender">The type of the sender.</typeparam>
/// <typeparam name="TArgs">The type of the event data.</typeparam>
/// <param name="sender">The source of the event.</param>
/// <param name="args">An object that contains the event data.</param>
public delegate void TypedEventHandler<in TSender, in TArgs>(TSender sender, TArgs args)
    where TSender : DependencyObject
    where TArgs : RoutedEventArgs;
