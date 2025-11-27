// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Represents the method that handles the <see cref="TabControlExtensions.TabClosingEvent"/> event.
/// </summary>
public delegate void TabClosingEventHandler(object sender, TabClosingEventArgs e);

/// <summary>
/// Represents the method that handles the <see cref="TabControlExtensions.TabAddingEvent"/> event.
/// </summary>
public delegate void TabAddingEventHandler(object sender, TabAddingEventArgs e);

