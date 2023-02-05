// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;

namespace Wpf.Ui.Controls.AutoSuggestBoxControl;

/// <summary>
/// Provides event data for the <see cref="AutoSuggestBox.QuerySubmitted"/> event.
/// </summary>
public sealed class AutoSuggestBoxQuerySubmittedEventArgs : RoutedEventArgs
{
    public AutoSuggestBoxQuerySubmittedEventArgs(RoutedEvent eventArgs, object sender) : base(eventArgs, sender)
    {

    }

    public required string QueryText { get; init; }
}
