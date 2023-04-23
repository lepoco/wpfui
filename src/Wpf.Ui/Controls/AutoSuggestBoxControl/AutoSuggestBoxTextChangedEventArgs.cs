// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;

namespace Wpf.Ui.Controls.AutoSuggestBoxControl;

/// <summary>
/// Provides data for the <see cref="AutoSuggestBox.TextChanged"/> event.
/// </summary>
public sealed class AutoSuggestBoxTextChangedEventArgs : RoutedEventArgs
{
    public AutoSuggestBoxTextChangedEventArgs(RoutedEvent eventArgs, object sender) : base(eventArgs, sender)
    {

    }

    public required string Text { get; init; }
    public required AutoSuggestionBoxTextChangeReason Reason { get; init; }
}
