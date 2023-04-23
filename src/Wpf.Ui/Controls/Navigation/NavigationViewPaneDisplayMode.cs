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
    /// The pane is shown on the left side of the control.
    /// </summary>
    Left,

    /// <summary>
    /// The pane is shown on the left side of the control. Only the pane icons are shown.
    /// </summary>
    LeftMinimal,

    /// <summary>
    /// The pane is shown on the left side of the control. Large icons with titles underneath are the only display option. Does not support <see cref="Navigation.INavigationViewItem.MenuItems"/>.
    /// <para>Similar to the Windows Store (2022) app.</para>
    /// </summary>
    LeftFluent,

    /// <summary>
    /// The pane is shown at the top of the control.
    /// </summary>
    Top,
    Bottom
}

