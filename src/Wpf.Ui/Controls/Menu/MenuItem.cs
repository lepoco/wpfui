// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Extended <see cref="System.Windows.Controls.MenuItem"/> with <see cref="SymbolRegular"/> properties.
/// </summary>
public class MenuItem : System.Windows.Controls.MenuItem
{
    static MenuItem()
    {
        IconProperty.OverrideMetadata(typeof(MenuItem), new FrameworkPropertyMetadata(null));
    }

    /// <summary>
    /// Gets or sets displayed <see cref="IconElement"/>.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "WpfAnalyzers.DependencyProperty",
        "WPF0012:CLR property type should match registered type",
        Justification = "seems harmless"
    )]
    public new IconElement Icon
    {
        get => (IconElement)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }
}
