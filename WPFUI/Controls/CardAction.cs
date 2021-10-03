// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;

namespace WPFUI.Controls
{
    public partial class CardAction : System.Windows.Controls.Button
    {
        public static readonly DependencyProperty GlyphProperty = DependencyProperty.Register("Glyph", 
            typeof(Common.MiconIcon), typeof(CardAction), new PropertyMetadata(Common.MiconIcon.None, OnGlyphChanged));
        
        public static readonly DependencyProperty RawGlyphProperty = DependencyProperty.Register("RawGlyph", 
            typeof(string), typeof(CardAction), new PropertyMetadata(""));
        
        public static readonly DependencyProperty IsGlyphProperty = DependencyProperty.Register("IsGlyph", 
            typeof(bool), typeof(CardAction), new PropertyMetadata(false));

        public bool IsGlyph
        {
            get => (bool) GetValue(IsGlyphProperty);
            set => SetValue(IsGlyphProperty, value);
        }

        public Common.MiconIcon Glyph
        {
            get => (Common.MiconIcon) GetValue(GlyphProperty);
            set => SetValue(GlyphProperty, value);
        }

        private static void OnGlyphChanged(DependencyObject dependency, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (dependency is not CardAction control) return;
            control.SetValue(IsGlyphProperty, true);
            control.SetValue(RawGlyphProperty, Common.MiconGlyph.ToString(control.Glyph));
        }
    }
}
