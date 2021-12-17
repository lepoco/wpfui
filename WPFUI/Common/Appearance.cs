// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace WPFUI.Common
{
    /// <summary>
    /// A collection representing the available color accents of the controls.
    /// </summary>
    public enum Appearance
    {
        /// <summary>
        /// Control color according to the current theme accent.
        /// </summary>
        Primary,

        /// <summary>
        /// Control color according to the current theme element.
        /// </summary>
        Secondary,

        /// <summary>
        /// Dark color theme.
        /// </summary>
        Dark,

        /// <summary>
        /// Light color theme.
        /// </summary>
        Light,

        /// <summary>
        /// Red color theme.
        /// </summary>
        Danger,

        /// <summary>
        /// Green color theme.
        /// </summary>
        Success,

        /// <summary>
        /// Orange color theme.
        /// </summary>
        Caution
    }
}
