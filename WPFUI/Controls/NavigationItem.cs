// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Media.Imaging;

namespace WPFUI.Controls
{
    /// <summary>
    /// Navigation element.
    /// </summary>
    public class NavigationItem : System.Windows.Controls.Button
    {
        /// <summary>
        /// Property for <see cref="IsActive"/>.
        /// </summary>
        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register("IsActive",
            typeof(bool), typeof(NavigationItem), new PropertyMetadata(false));

        /// <summary>
        /// Property for <see cref="Glyph"/>.
        /// </summary>
        public static readonly DependencyProperty GlyphProperty = DependencyProperty.Register("Glyph",
            typeof(Common.Icon), typeof(NavigationItem), new PropertyMetadata(Common.Icon.Empty, OnGlyphChanged));

        /// <summary>
        /// <see cref="System.String"/> property for <see cref="Glyph"/>.
        /// </summary>
        public static readonly DependencyProperty RawGlyphProperty = DependencyProperty.Register("RawGlyph",
            typeof(string), typeof(NavigationItem), new PropertyMetadata(""));

        /// <summary>
        /// Property for <see cref="IsGlyph"/>.
        /// </summary>
        public static readonly DependencyProperty IsGlyphProperty = DependencyProperty.Register("IsGlyph",
            typeof(bool), typeof(NavigationItem), new PropertyMetadata(false));

        /// <summary>
        /// Property for <see cref="Image"/>.
        /// </summary>
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image",
            typeof(BitmapSource), typeof(NavigationItem),
            new PropertyMetadata(null, OnImageChanged));

        /// <summary>
        /// Property for <see cref="IsImage"/>.
        /// </summary>
        public static readonly DependencyProperty IsImageProperty = DependencyProperty.Register("IsImage",
            typeof(bool), typeof(NavigationItem), new PropertyMetadata(false));

        /// <summary>
        /// Gets information whether the current element is active.
        /// </summary>
        public bool IsActive
        {
            get => (bool)GetValue(IsActiveProperty);
            set => SetValue(IsActiveProperty, value);
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
            set => SetValue(RawGlyphProperty, value);
        }

        /// <summary>
        /// Gets information whether the <see cref="Glyph"/> is set.
        /// </summary>
        public bool IsGlyph => (bool)this.GetValue(IsGlyphProperty);

        /// <summary>
        /// Gets or sets image displayed next to the card name instead of the icon.
        /// </summary>
        public BitmapSource Image
        {
            get => GetValue(ImageProperty) as BitmapSource;
            set => SetValue(ImageProperty, value);
        }

        /// <summary>
        /// Gets or sets information whether the element contains image icon.
        /// </summary>
        public bool IsImage
        {
            get => (bool)(GetValue(IsImageProperty) as bool?);
            set => SetValue(IsImageProperty, value);
        }

        private static void OnGlyphChanged(DependencyObject dependency, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (dependency is not NavigationItem control) return;

            control.SetValue(IsGlyphProperty, control.Glyph != Common.Icon.Empty);
            control.SetValue(RawGlyphProperty, Common.Glyph.ToString(control.Glyph));
        }

        private static void OnImageChanged(DependencyObject dependency, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (dependency is not NavigationItem control) return;

            control.SetValue(IsImageProperty, control.Image != null);
        }
    }
}