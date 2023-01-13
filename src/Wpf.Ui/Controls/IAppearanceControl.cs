// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Controls;

/// <summary>
/// UI <see cref="System.Windows.Controls.Control"/> with <see cref="ControlAppearance"/> attributes.
/// </summary>
public interface IAppearanceControl
{
    /// <summary>
    /// Gets or sets the <see cref="Appearance"/> of the control, if available.
    /// </summary>
    public ControlAppearance Appearance { get; set; }
}
