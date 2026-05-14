// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Text;

namespace Wpf.Ui.Tray.Controls;

/// <summary>
/// Specifies the icon displayed in a tray balloon tip notification.
/// </summary>
public enum ToolTipIcon
{
    /// <summary>
    /// No icon is displayed in the balloon tip notification.
    /// </summary>
    None,

    /// <summary>
    /// Displays an informational icon in the balloon tip notification.
    /// </summary>
    Info,

    /// <summary>
    /// Displays a warning icon in the balloon tip notification.
    /// </summary>
    Warning,

    /// <summary>
    /// Displays an error icon in the balloon tip notification.
    /// </summary>
    Error
}
