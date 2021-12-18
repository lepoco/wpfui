// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;

namespace WPFUI.Controls
{
    /// <summary>
    /// Inherited from the <see cref="System.Windows.Controls.Button"/> interactive card styled according to Fluent Design.
    /// </summary>
    //#if NETFRAMEWORK
    //    [ToolboxBitmap(typeof(Button))]
    //#endif
    public class CardAction : System.Windows.Controls.Button
    {
        /// <summary>
        /// Property for <see cref="ShowChevron"/>.
        /// </summary>
        public static readonly DependencyProperty ShowChevronProperty = DependencyProperty.Register(nameof(ShowChevron),
            typeof(bool), typeof(CardAction), new PropertyMetadata(true));

        /// <summary>
        /// Property for <see cref="Glyph"/>.
        /// </summary>
        public static readonly DependencyProperty GlyphProperty = DependencyProperty.Register(nameof(Glyph),
            typeof(Common.Icon), typeof(CardAction), new PropertyMetadata(Common.Icon.Empty, OnGlyphChanged));

        /// <summary>
        /// <see cref="System.String"/> property for <see cref="Glyph"/>.
        /// </summary>
        public static readonly DependencyProperty RawGlyphProperty = DependencyProperty.Register(nameof(RawGlyph),
            typeof(string), typeof(CardAction), new PropertyMetadata(""));

        /// <summary>
        /// Property for <see cref="IsGlyph"/>.
        /// </summary>
        public static readonly DependencyProperty IsGlyphProperty = DependencyProperty.Register(nameof(IsGlyph),
            typeof(bool), typeof(CardAction), new PropertyMetadata(false));

        /// <summary>
        /// <see cref="System.String"/> property for <see cref="Filled"/>.
        /// </summary>
        public static readonly DependencyProperty FilledProperty = DependencyProperty.Register(nameof(Filled),
            typeof(bool), typeof(CardAction), new PropertyMetadata(false, OnGlyphChanged));

        /// <summary>
        /// Gets or sets information whether to display the chevron icon on the right side of the card.
        /// </summary>
        public bool ShowChevron
        {
            get => (bool)GetValue(ShowChevronProperty);
            set => SetValue(ShowChevronProperty, value);
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
        /// Gets information whether the <see cref="Glyph"/> is set.
        /// </summary>
        public bool IsGlyph
        {
            get => (bool)GetValue(IsGlyphProperty);
            internal set => SetValue(IsGlyphProperty, value);
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
            if (dependency is not CardAction control) return;

            control.SetValue(IsGlyphProperty, control.Glyph != Common.Icon.Empty);

            if ((bool)control.GetValue(FilledProperty))
            {
                control.SetValue(RawGlyphProperty, Common.Glyph.ToString(Common.Glyph.Swap(control.Glyph)));
            }
            else
            {
                control.SetValue(RawGlyphProperty, Common.Glyph.ToString(control.Glyph));
            }
        }
    }
}