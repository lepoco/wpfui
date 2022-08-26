// Based on Windows UI Library
// Copyright(c) Microsoft Corporation.All rights reserved.

// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Controls.Navigation;

/// <summary>
/// Defines constants that specify how and where the NavigationView pane is shown.
/// </summary>
public enum NavigationViewPaneDisplayMode
{
    /// <summary>
    /// The pane is shown on the left side of the control. Only the pane icons are shown by default.
    /// Standard visual style of navigation in WinUi 3.
    /// </summary>
    LeftCompact,

    /// <summary>
    /// Wide buttons, with full titles, similar to the settings app for Windows 11.
    /// </summary>
    LeftWide,

    /// <summary>
    /// Modern, tiled style of navigation, similar to the Windows Store app.
    /// </summary>
    LeftFluent,

    /// <summary>
    /// The pane is shown at the top of the control.
    /// </summary>
    Top
}

