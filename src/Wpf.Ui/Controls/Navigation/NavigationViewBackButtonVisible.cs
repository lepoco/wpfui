// Based on Windows UI Library
// Copyright(c) Microsoft Corporation.All rights reserved.

// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Controls.Navigation;

/// <summary>
/// Defines constants that specify whether the back button is visible in NavigationView.
/// </summary>
public enum NavigationViewBackButtonVisible
{
    /// <summary>
    /// Do not display the back button in NavigationView, and do not reserve space for it in layout.
    /// </summary>
    Collapsed,

    /// <summary>
    /// Display the back button in NavigationView.
    /// </summary>
    Visible,

    /// <summary>
    /// The system chooses whether or not to display the back button, depending on the device/form factor. On phones, tablets, desktops, and hubs, the back button is visible. On Xbox/TV, the back button is collapsed.
    /// </summary>
    Auto
}
