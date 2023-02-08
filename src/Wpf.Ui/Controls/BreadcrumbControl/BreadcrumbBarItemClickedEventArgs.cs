// Based on Windows UI Library
// Copyright(c) Microsoft Corporation.All rights reserved.

// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;

namespace Wpf.Ui.Controls.BreadcrumbControl;

public sealed class BreadcrumbBarItemClickedEventArgs : RoutedEventArgs
{
    public BreadcrumbBarItemClickedEventArgs(RoutedEvent routedEvent, object source, object item, int index) : base(routedEvent, source)
    {
        Item = item;
        Index = index;
    }

    /// <summary>
    /// Gets the Content property value of the BreadcrumbBarItem that is clicked.
    /// </summary>
    public object Item { get; }
    /// <summary>
    /// Gets the index of the item that was clicked.
    /// </summary>
    public int Index { get; }
}
