// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
