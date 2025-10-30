// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Gallery.Models;

/// <summary>
/// Application configuration model.
/// </summary>
public class AppConfig
{
    /// <summary>
    /// Gets or sets the configurations folder.
    /// </summary>
    public string? ConfigurationsFolder { get; set; }

    /// <summary>
    /// Gets or sets the app properties file name.
    /// </summary>
    public string? AppPropertiesFileName { get; set; }

    /// <summary>
    /// Gets or sets the navigation pane width.
    /// </summary>
    public double NavigationPaneWidth { get; set; } = 310.0;
}

