// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFUI.Controls
{
    /// <summary>
    /// Inherited from the <see cref="System.Windows.Controls.Button"/>, adding <see cref="Common.Icon"/>.
    /// </summary>
    public class Button : System.Windows.Controls.Button
    {
        /// <summary>
        /// Property for <see cref="Glyph"/>.
        /// </summary>
        public static readonly DependencyProperty GlyphProperty = DependencyProperty.Register(nameof(Glyph),
            typeof(Common.Icon), typeof(Button),
            new PropertyMetadata(Common.Icon.Empty, OnGlyphChanged));

        /// <summary>
        /// <see cref="System.String"/> property for <see cref="Glyph"/>.
        /// </summary>
        public static readonly DependencyProperty RawGlyphProperty = DependencyProperty.Register(nameof(RawGlyph),
            typeof(string), typeof(Button), new PropertyMetadata(""));

        /// <summary>
        /// Property for <see cref="IsGlyph"/>.
        /// </summary>
        public static readonly DependencyProperty IsGlyphProperty = DependencyProperty.Register(nameof(IsGlyph),
            typeof(bool), typeof(Button), new PropertyMetadata(false));

        /// <summary>
        /// Property for <see cref="Appearance"/>.
        /// </summary>
        public static readonly DependencyProperty AppearanceProperty = DependencyProperty.Register(nameof(Appearance),
            typeof(Common.Appearance), typeof(Button),
            new PropertyMetadata(Common.Appearance.Primary));

        /// <summary>
        /// Property for <see cref="HoverBackground"/>.
        /// </summary>
        public static readonly DependencyProperty HoverBackgroundProperty = DependencyProperty.Register(nameof(HoverBackground),
            typeof(Brush), typeof(Button),
            new PropertyMetadata(Border.BackgroundProperty.DefaultMetadata.DefaultValue));

        /// <summary>
        /// Property for <see cref="HoverBorderBrush"/>.
        /// </summary>
        public static readonly DependencyProperty HoverBorderBrushProperty = DependencyProperty.Register(nameof(HoverBorderBrush),
            typeof(Brush), typeof(Button),
            new PropertyMetadata(Border.BorderBrushProperty.DefaultMetadata.DefaultValue));

        /// <summary>
        /// Gets information whether the <see cref="Glyph"/> is set.
        /// </summary>
        public bool IsGlyph
        {
            get => (bool)GetValue(IsGlyphProperty);
            internal set => SetValue(IsGlyphProperty, value);
        }

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
        /// Gets or sets the <see cref="Common.Appearance"/> of the control, if available.
        /// </summary>
        public Common.Appearance Appearance
        {
            get => (Common.Appearance)GetValue(AppearanceProperty);
            set => SetValue(AppearanceProperty, value);
        }

        /// <summary>
        /// Background <see cref="Brush"/> when the user interacts with an element with a pointing device.
        /// </summary>
        [Bindable(true), Category("Appearance")]
        public Brush HoverBackground
        {
            get => (Brush)GetValue(HoverBackgroundProperty);
            set => SetValue(HoverBackgroundProperty, value);
        }

        /// <summary>
        /// Border <see cref="Brush"/> when the user interacts with an element with a pointing device.
        /// </summary>
        [Bindable(true), Category("Appearance")]
        public Brush HoverBorderBrush
        {
            get => (Brush)GetValue(HoverBorderBrushProperty);
            set => SetValue(HoverBorderBrushProperty, value);
        }

        private static void OnGlyphChanged(DependencyObject dependency, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (dependency is not Button control) return;

            control.SetValue(IsGlyphProperty, true);
            control.SetValue(RawGlyphProperty, Common.Glyph.ToString(control.Glyph));
        }
    }
}