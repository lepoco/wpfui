// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

// Based on Windows UI Library
// Copyright(c) Microsoft Corporation.All rights reserved.

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Provides data for the <see cref="SelectorBar.SelectionChanged"/> event.
/// </summary>
public sealed class SelectorBarSelectionChangedEventArgs : RoutedEventArgs
{
    public SelectorBarSelectionChangedEventArgs(RoutedEvent routedEvent, object source)
        : base(routedEvent, source) { }
}
