// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Controls.Navigation;

/// <summary>
/// Defines constants that specify how the pane is shown in a NavigationView.
/// <para>It is not the same DisplayMode as in WinUi.</para>
/// </summary>
public enum NavigationViewDisplayMode
{
    /// <summary>
    /// Standard visual style of navigation in WinUi 3.
    /// </summary>
    Compact,

    /// <summary>
    /// Wide buttons, with full titles, similar to the settings app for Windows 11.
    /// </summary>
    Wide,

    /// <summary>
    /// Modern, tiled style of navigation, similar to the Windows Store app.
    /// </summary>
    Fluent,
}
