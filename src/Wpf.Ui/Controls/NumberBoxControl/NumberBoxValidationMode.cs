// This Source Code is partially based on the source code provided by the .NET Foundation.
// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) .NET Foundation Contributors, WPF UI Contributors, Leszek Pomianowski.
// All Rights Reserved.

namespace Wpf.Ui.Controls.NumberBoxControl;

/// <summary>
/// Defines values that specify the input validation behavior of a <see cref="NumberBox"/> when invalid input is entered.
/// </summary>
public enum NumberBoxValidationMode
{
    /// <summary>
    /// Input validation is disabled.
    /// </summary>
    InvalidInputOverwritten,

    /// <summary>
    /// Invalid input is replaced by <see cref="NumberBox"/> PlaceholderText text.
    /// </summary>
    Disabled
}
