// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Controls;

namespace WPFUI.Controls
{
    public partial class CardAction : Button
    {
        public static readonly DependencyProperty GlyphProperty = DependencyProperty.Register("Glyph", 
            typeof(Common.MiconIcon), typeof(CardAction), new PropertyMetadata(Common.MiconIcon.None, OnGlyphChanged));
        
        public static readonly DependencyProperty RawGlyphProperty = DependencyProperty.Register("RawGlyph", 
            typeof(string), typeof(CardAction), new PropertyMetadata(""));
        
        public static readonly DependencyProperty IsGlyphProperty = DependencyProperty.Register("IsGlyph", 
            typeof(bool), typeof(CardAction), new PropertyMetadata(false));
        
        //public static readonly DependencyProperty BorderCommandProperty =
        //    DependencyProperty.Register("BorderCommand", typeof(Common.RelayCommand), typeof(CardAction), new PropertyMetadata(null));

        //public Common.RelayCommand BorderCommand => (Common.RelayCommand) GetValue(BorderCommandProperty);

        //public Action Click { get; set; }

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

        public CardAction()
        {
            //Click = () => { };
            //SetValue(BorderCommandProperty, new Common.RelayCommand(o => Click()));
        }

        private static void OnGlyphChanged(DependencyObject dependency, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (dependency is not CardAction control) return;
            control.SetValue(IsGlyphProperty, true);
            control.SetValue(RawGlyphProperty, Common.MiconGlyph.ToString(control.Glyph));
        }
    }
}
