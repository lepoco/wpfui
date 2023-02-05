// This Source Code is partially based on the source code provided by the .NET Foundation.
// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) .NET Foundation Contributors, WPF UI Contributors, Leszek Pomianowski.
// All Rights Reserved.

namespace Wpf.Ui.Controls.NumberBoxControl;

/// <summary>
/// An interface that returns a string representation of a provided value, using distinct format methods to format several data types.
/// </summary>
public interface INumberFormatter
{
    /// <summary>
    /// Returns a string representation of a <see cref="System.Double"/> value.
    /// </summary>
    string FormatDouble(double? value);

    /// <summary>
    /// Returns a string representation of an <see cref="System.Int32"/> value.
    /// </summary>
    string FormatInt(int? value);

    /// <summary>
    /// Returns a string representation of a <see cref="System.UInt32"/> value.
    /// </summary>
    string FormatUInt(uint? value);
}
