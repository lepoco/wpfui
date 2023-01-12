// This Source Code is partially based on the source code provided by the .NET Foundation.
// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) .NET Foundation Contributors, WPF UI Contributors, Leszek Pomianowski.
// All Rights Reserved.

using System.Windows;

namespace Wpf.Ui.Dpi;

public sealed class DpiChangedEventArgs : RoutedEventArgs
{
    /// <summary>
    /// Gets the scale information after a DPI change.
    /// </summary>
    public Dpi NewDpi { get; }

    /// <summary>
    /// Gets the DPI scale information before a DPI change.
    /// </summary>
    public Dpi OldDpi { get; }
}
