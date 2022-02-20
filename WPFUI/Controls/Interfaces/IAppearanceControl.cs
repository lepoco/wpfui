// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace WPFUI.Controls.Interfaces
{
    /// <summary>
    /// UI <see cref="System.Windows.Controls.Control"/> with <see cref="Common.Appearance"/> attributes.
    /// </summary>
    public interface IAppearanceControl
    {
        /// <summary>
        /// Gets or sets the <see cref="Common.Appearance"/> of the control, if available.
        /// </summary>
        public Common.Appearance Appearance { get; set; }
    }
}
