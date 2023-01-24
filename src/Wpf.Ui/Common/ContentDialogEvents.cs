// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.


using System.Windows;
using Wpf.Ui.Controls;

namespace Wpf.Ui.Common;

/// <summary>
/// Event triggered on via <see cref="ContentDialog"/>.
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
public delegate void ContentDialogHidingEvent(ContentDialog sender, ContentDialogHidingEventArgs e);

/// <summary>
/// See <see cref="ContentDialog.Hiding"/>
/// </summary>
public class ContentDialogHidingEventArgs : RoutedEventArgs
{
    public ContentDialogHidingEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source) {}

    public bool Cancel { get; set; }
}
