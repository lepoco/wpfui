// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

// This Source Code is partially based on the source code provided by the .NET Foundation.

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// An interface that returns a string representation of a provided value, using distinct format methods to format several data types.
/// </summary>
public interface INumberFormatter
{
    /// <summary>
    /// Returns a string representation of a <see cref="double"/> value.
    /// </summary>
    string FormatDouble(double? value);

    /// <summary>
    /// Returns a string representation of an <see cref="int"/> value.
    /// </summary>
    string FormatInt(int? value);

    /// <summary>
    /// Returns a string representation of a <see cref="uint"/> value.
    /// </summary>
    string FormatUInt(uint? value);
}
