// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;

namespace WPFUI.Common
{
    /// <summary>
    /// A convenient class for creating event IDs.
    /// </summary>
    internal class EventIdentifier
    {
        private readonly Random _random = new Random();

        private uint _currentIdentifier = 0;

        /// <summary>
        /// ID length. The longer the more accurate, but more complicated to calculate and verify.
        /// </summary>
        public uint MaxValue { get; set; } = Int32.MaxValue;

        /// <summary>
        /// Creates and gets the next identifier.
        /// </summary>
        public uint GetNext()
        {
            UpdateIdentifier();

            return _currentIdentifier;
        }

        /// <summary>
        /// Checks if the identifiers are the same.
        /// </summary>
        public bool IsEqual(uint storedId)
        {
            return _currentIdentifier == storedId;
        }

        private void UpdateIdentifier()
        {
            // TODO: This isn't the most efficient event identifier, but async doesn't always create a thread. Feel free to propose something better
            _currentIdentifier = (uint)(_random.Next(1 << 30)) << 2 | (uint)(_random.Next(1 << 2));
        }
    }
}
