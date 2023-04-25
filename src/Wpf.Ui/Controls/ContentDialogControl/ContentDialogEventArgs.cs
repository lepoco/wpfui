// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;

namespace Wpf.Ui.Controls.ContentDialogControl;

public class ContentDialogClosingEventArgs : RoutedEventArgs
{
    public ContentDialogClosingEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source)
    {
    }

    public required ContentDialogResult Result { get; init; }
    public bool Cancel { get; set; }
}

public class ContentDialogClosedEventArgs : RoutedEventArgs
{
    public ContentDialogClosedEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source)
    {
    }

    public required ContentDialogResult Result { get; init; }
}

public class ContentDialogButtonClickEventArgs : RoutedEventArgs
{
    public ContentDialogButtonClickEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source)
    {
    }

    public required ContentDialogButton Button { get; init; }
}
