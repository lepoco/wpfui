// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using Wpf.Ui.Extensions;

namespace Wpf.Ui.Common;

/// <summary>
/// Class used to create identifiers of threads or tasks that can be performed multiple times within one instance.
/// <see cref="Current"/> represents roughly the time in microseconds at which it was taken.
/// </summary>
internal class EventIdentifier
{
    /// <summary>
    /// Current identifier.
    /// </summary>
    public long Current { get; internal set; } = 0;

    /// <summary>
    /// Creates and gets the next identifier.
    /// </summary>
    public long GetNext()
    {
        UpdateIdentifier();

        return Current;
    }

    /// <summary>
    /// Checks if the identifiers are the same.
    /// </summary>
    public bool IsEqual(long storedId) => Current == storedId;

    /// <summary>
    /// Creates and assigns a random value with an extra time code if possible.
    /// </summary>
    private void UpdateIdentifier() => Current = DateTime.Now.GetMicroTimestamp();
}
