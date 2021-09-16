// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;
using WPFUI.Common;

namespace WPFUI.Controls
{
    public partial class CardCollapse : ContentControl
    {
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", 
            typeof(string), typeof(CardCollapse), new PropertyMetadata(""));
        
        public static readonly DependencyProperty GlyphProperty = DependencyProperty.Register("Glyph", 
            typeof(MiconIcon), typeof(CardCollapse), new PropertyMetadata(MiconIcon.None, OnGlyphChanged));
        
        public static readonly DependencyProperty RawGlyphProperty = DependencyProperty.Register("RawGlyph", 
            typeof(string), typeof(CardCollapse), new PropertyMetadata(""));
        
        public static readonly DependencyProperty IsContentVisibleProperty = DependencyProperty.Register("IsContentVisible", 
            typeof(bool), typeof(CardCollapse), new PropertyMetadata(false));
        
        public static readonly DependencyProperty IsGlyphProperty = DependencyProperty.Register("IsGlyph", 
            typeof(bool), typeof(CardCollapse), new PropertyMetadata(false));
        
        public static readonly DependencyProperty BorderCommandProperty =
            DependencyProperty.Register("BorderCommand", 
                typeof(RelayCommand), typeof(CardCollapse), new PropertyMetadata(null));

        public RelayCommand BorderCommand => (RelayCommand) GetValue(BorderCommandProperty);

        public bool IsGlyph => (bool) GetValue(IsGlyphProperty);

        public string Title
        {
            get => (string) GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public bool IsContentVisible
        {
            get => (bool) GetValue(IsContentVisibleProperty);
            set => SetValue(IsContentVisibleProperty, value);
        }

        public MiconIcon Glyph
        {
            get => (MiconIcon) GetValue(GlyphProperty);
            set => SetValue(GlyphProperty, value);
        }

        public CardCollapse() => SetValue(BorderCommandProperty, new RelayCommand(o => CardOnClick()));

        private void CardOnClick() => IsContentVisible = !IsContentVisible;

        private static void OnGlyphChanged(DependencyObject dependency, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (dependency is not CardCollapse control) return;
            control.SetValue(IsGlyphProperty, true);
            control.SetValue(RawGlyphProperty, MiconGlyph.ToString(control.Glyph));
        }
    }
}
