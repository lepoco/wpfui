// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;

namespace WPFUI.Controls
{
    public partial class CardCollapse : ContentControl
    {
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", 
            typeof(string), typeof(CardCollapse), new PropertyMetadata(""));

        public static readonly DependencyProperty SubtitleProperty = DependencyProperty.Register("Subtitle",
            typeof(string), typeof(CardCollapse), new PropertyMetadata(""));

        public static readonly DependencyProperty GlyphProperty = DependencyProperty.Register("Glyph", 
            typeof(Common.Icon), typeof(CardCollapse), new PropertyMetadata(Common.Icon.Empty, OnGlyphChanged));
        
        public static readonly DependencyProperty RawGlyphProperty = DependencyProperty.Register("RawGlyph", 
            typeof(string), typeof(CardCollapse), new PropertyMetadata(""));
        
        public static readonly DependencyProperty IsOpenedProperty = DependencyProperty.Register("IsOpened", 
            typeof(bool), typeof(CardCollapse), new PropertyMetadata(false));
        
        public static readonly DependencyProperty IsGlyphProperty = DependencyProperty.Register("IsGlyph", 
            typeof(bool), typeof(CardCollapse), new PropertyMetadata(false));
        
        public static readonly DependencyProperty BorderCommandProperty =
            DependencyProperty.Register("BorderCommand", 
                typeof(Common.RelayCommand), typeof(CardCollapse), new PropertyMetadata(null));

        public Common.RelayCommand BorderCommand => (Common.RelayCommand) GetValue(BorderCommandProperty);

        public bool IsGlyph => (bool) GetValue(IsGlyphProperty);

        public string Title
        {
            get => (string) GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public string Subtitle
        {
            get => (string)GetValue(SubtitleProperty);
            set => SetValue(SubtitleProperty, value);
        }

        public bool IsOpened
        {
            get => (bool) GetValue(IsOpenedProperty);
            set => SetValue(IsOpenedProperty, value);
        }

        public Common.Icon Glyph
        {
            get => (Common.Icon) GetValue(GlyphProperty);
            set => SetValue(GlyphProperty, value);
        }

        public CardCollapse() => SetValue(BorderCommandProperty, new Common.RelayCommand(o => CardOnClick()));

        private void CardOnClick() => IsOpened = !IsOpened;

        private static void OnGlyphChanged(DependencyObject dependency, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (dependency is not CardCollapse control) return;
            control.SetValue(IsGlyphProperty, true);
            control.SetValue(RawGlyphProperty, Common.Glyph.ToString(control.Glyph));
        }
    }
}
