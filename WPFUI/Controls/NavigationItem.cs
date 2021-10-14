// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;

namespace WPFUI.Controls
{
    public partial class NavigationItem : System.Windows.Controls.Button
    {
        public static readonly DependencyProperty GlyphProperty = DependencyProperty.Register("Glyph", 
            typeof(Common.Icon), typeof(NavigationItem), new PropertyMetadata(Common.Icon.Empty, OnGlyphChanged));
        
        public static readonly DependencyProperty RawGlyphProperty = DependencyProperty.Register("RawGlyph", 
            typeof(string), typeof(NavigationItem), new PropertyMetadata(""));
        
        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register("IsActive", 
            typeof(bool), typeof(NavigationItem), new PropertyMetadata(false));
        
        public static readonly DependencyProperty IsGlyphProperty = DependencyProperty.Register("IsGlyph",
            typeof(bool), typeof(NavigationItem), new PropertyMetadata(false));

        public bool IsGlyph => (bool) this.GetValue(IsGlyphProperty); 
        
        public bool IsActive
        {
            get => (bool) GetValue(IsActiveProperty);
            set => SetValue(IsActiveProperty, value);
        }

        public Common.Icon Glyph
        {
            get => (Common.Icon) GetValue(GlyphProperty);
            set => SetValue(GlyphProperty, value);
        }

        private static void OnGlyphChanged(DependencyObject dependency, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (dependency is not NavigationItem control) return;

            if (control.Glyph == Common.Icon.Empty) return;

            control.SetValue(IsGlyphProperty, true);
            control.SetValue(RawGlyphProperty, Common.Glyph.ToString(control.Glyph));
        }
    }
}
