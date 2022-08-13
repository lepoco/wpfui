// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.ComponentModel;
using System.Drawing;
using System.Windows;

namespace Wpf.Ui.Controls;

/// <summary>
/// Represents a text element containing an icon glyph with selectable font family.
/// </summary>
[ToolboxItem(true)]
[ToolboxBitmap(typeof(FontIcon), "FontIcon.bmp")]
public class FontIcon : System.Windows.Controls.Control
{
    /// <summary>
    /// Property for <see cref="Glyph"/>.
    /// </summary>
    public static readonly DependencyProperty GlyphProperty = DependencyProperty.Register(
        nameof(Glyph),
        typeof(char),
        typeof(FontIcon),
        new PropertyMetadata('\uE006')
    );

    /// <summary>
    /// Gets or sets displayed <see cref="char"/>.
    /// </summary>
    public char Glyph
    {
        get => (char)GetValue(GlyphProperty);
        set => SetValue(GlyphProperty, value);
    }
}
