// This Source Code is partially based on the source code provided by the .NET Foundation.
// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) .NET Foundation Contributors, WPF UI Contributors, Leszek Pomianowski.
// All Rights Reserved.

namespace Wpf.Ui.Controls.NumberBoxControl;

/// <summary>
/// An interface that parses a string representation of a numeric value.
/// </summary>
public interface INumberParser
{
    /// <summary>
    /// Attempts to parse a string representation of a <see cref="System.Double"/> numeric value.
    /// </summary>
    double? ParseDouble(string? value);

    /// <summary>
    /// Attempts to parse a string representation of an <see cref="System.Int32"/> numeric value.
    /// </summary>
    int? ParseInt(string? value);

    /// <summary>
    /// Attempts to parse a string representation of an <see cref="System.UInt32"/> numeric value.
    /// </summary>
    uint? ParseUInt(string? value);
}
