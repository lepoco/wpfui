// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.ComponentModel;
using System.Drawing;
using System.Windows;

// https://docs.microsoft.com/en-us/fluent-ui/web-components/components/badge

namespace Wpf.Ui.Controls;

/// <summary>
/// Used to highlight an item, attract attention or flag status.
/// </summary>
[ToolboxItem(true)]
[ToolboxBitmap(typeof(Badge), "Badge.bmp")]
public class Badge : System.Windows.Controls.ContentControl, IAppearanceControl
{
    /// <summary>
    /// Property for <see cref="Appearance"/>.
    /// </summary>
    public static readonly DependencyProperty AppearanceProperty = DependencyProperty.Register(nameof(Appearance),
        typeof(Controls.ControlAppearance), typeof(Badge),
        new PropertyMetadata(Controls.ControlAppearance.Primary));

    /// <inheritdoc />
    public Controls.ControlAppearance Appearance
    {
        get => (Controls.ControlAppearance)GetValue(AppearanceProperty);
        set => SetValue(AppearanceProperty, value);
    }
}
