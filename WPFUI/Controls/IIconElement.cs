// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace WPFUI.Controls
{
    /// <summary>
    /// UI element with <see cref="Common.Icon"/>.
    /// </summary>
    public interface IIconElement
    {
        /// <summary>
        /// Gets or sets displayed <see cref="Common.Icon"/>.
        /// </summary>
        Common.Icon Icon { get; set; }

        /// <summary>
        /// Defines whether or not we should use the <see cref="Common.IconFilled"/>.
        /// </summary>
        bool IconFilled { get; set; }
    }
}
