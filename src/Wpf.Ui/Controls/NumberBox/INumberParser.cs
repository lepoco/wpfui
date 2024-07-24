// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

// This Source Code is partially based on the source code provided by the .NET Foundation.

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// An interface that parses a string representation of a numeric value.
/// </summary>
public interface INumberParser
{
    /// <summary>
    /// Attempts to parse a string representation of a <see cref="double"/> numeric value.
    /// </summary>
    double? ParseDouble(string? value);

    /// <summary>
    /// Attempts to parse a string representation of an <see cref="int"/> numeric value.
    /// </summary>
    int? ParseInt(string? value);

    /// <summary>
    /// Attempts to parse a string representation of an <see cref="uint"/> numeric value.
    /// </summary>
    uint? ParseUInt(string? value);
}
