// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Abstractions;

/// <summary>
/// Represents errors that occur during navigation.
/// </summary>
public sealed class NavigationException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NavigationException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public NavigationException(string message)
        : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="NavigationException"/> class with a specified error message
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="e">The exception that is the cause of the current exception.</param>
    /// <param name="message">The message that describes the error.</param>
    public NavigationException(Exception e, string message)
        : base(message, e) { }
}
