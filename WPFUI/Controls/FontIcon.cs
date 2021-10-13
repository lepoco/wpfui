// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;

namespace WPFUI.Controls
{
    public partial class FontIcon : System.Windows.Controls.Label
    {
        public static readonly DependencyProperty GlyphProperty = DependencyProperty.Register(
            "Glyph",
            typeof(char),
            typeof(FontIcon),
            new PropertyMetadata('\uE006')
        );

        public char Glyph
        {
            get => (char) GetValue(GlyphProperty);
            set => SetValue(GlyphProperty, value);
        }
    }
}
