// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using WPFUI.Common;

namespace WPFUI.Controls
{
    /// <summary>
    /// Represents a text element containing an icon glyph.
    /// </summary>
    public class Icon : System.Windows.Controls.Label
    {
        /// <summary>
        /// Property for <see cref="Glyph"/>.
        /// </summary>
        public static readonly DependencyProperty GlyphProperty = DependencyProperty.Register(nameof(Glyph),
            typeof(Common.Icon), typeof(Icon),
            new PropertyMetadata(Common.Icon.Empty, OnGlyphChanged));

        /// <summary>
        /// <see cref="System.String"/> property for <see cref="RawGlyph"/>.
        /// </summary>
        public static readonly DependencyProperty RawGlyphProperty = DependencyProperty.Register(nameof(RawGlyph),
            typeof(string), typeof(Icon), new PropertyMetadata("\uEA01"));

        /// <summary>
        /// <see cref="System.String"/> property for <see cref="Filled"/>.
        /// </summary>
        public static readonly DependencyProperty FilledProperty = DependencyProperty.Register(nameof(Filled),
            typeof(bool), typeof(Icon), new PropertyMetadata(false, OnGlyphChanged));

        /// <summary>
        /// Gets or sets displayed <see cref="Common.Icon"/>.
        /// </summary>
        public Common.Icon Glyph
        {
            get => (Common.Icon)GetValue(GlyphProperty);
            set => SetValue(GlyphProperty, value);
        }

        /// <summary>
        /// Gets or sets displayed <see cref="Common.Icon"/> as <see langword="string"/>.
        /// </summary>
        public string RawGlyph
        {
            get => (string)GetValue(RawGlyphProperty);
        }

        /// <summary>
        /// Defines whether or not we should use the <see cref="Common.IconFilled"/>.
        /// </summary>
        public bool Filled
        {
            get => (bool)GetValue(FilledProperty);
            set => SetValue(FilledProperty, value);
        }

        private static void OnGlyphChanged(DependencyObject dependency, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (dependency is not Icon control) return;

            if ((bool)control.GetValue(FilledProperty))
                control.SetValue(RawGlyphProperty, control.Glyph.Swap().GetString());
            else
                control.SetValue(RawGlyphProperty, control.Glyph.GetString());
        }
    }
}