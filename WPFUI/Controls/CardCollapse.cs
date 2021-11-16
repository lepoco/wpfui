// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;

namespace WPFUI.Controls
{
    /// <summary>
    /// Inherited from the <see cref="System.Windows.Controls.ContentControl"/> control which can hide the collapsable content.
    /// </summary>
    public class CardCollapse : ContentControl
    {
        /// <summary>
        /// Property for <see cref="Title"/>.
        /// </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title",
            typeof(string), typeof(CardCollapse), new PropertyMetadata(""));

        /// <summary>
        /// Property for <see cref="Subtitle"/>.
        /// </summary>
        public static readonly DependencyProperty SubtitleProperty = DependencyProperty.Register("Subtitle",
            typeof(string), typeof(CardCollapse), new PropertyMetadata(""));

        /// <summary>
        /// Property for <see cref="Glyph"/>.
        /// </summary>
        public static readonly DependencyProperty GlyphProperty = DependencyProperty.Register("Glyph",
            typeof(Common.Icon), typeof(CardCollapse), new PropertyMetadata(Common.Icon.Empty, OnGlyphChanged));

        /// <summary>
        /// <see cref="System.String"/> property for <see cref="Glyph"/>.
        /// </summary>
        public static readonly DependencyProperty RawGlyphProperty = DependencyProperty.Register("RawGlyph",
            typeof(string), typeof(CardCollapse), new PropertyMetadata(""));

        /// <summary>
        /// Property for <see cref="IsOpened"/>.
        /// </summary>
        public static readonly DependencyProperty IsOpenedProperty = DependencyProperty.Register("IsOpened",
            typeof(bool), typeof(CardCollapse), new PropertyMetadata(false));

        /// <summary>
        /// Property for <see cref="IsGlyph"/>.
        /// </summary>
        public static readonly DependencyProperty IsGlyphProperty = DependencyProperty.Register("IsGlyph",
            typeof(bool), typeof(CardCollapse), new PropertyMetadata(false));

        /// <summary>
        /// Property for <see cref="AdditionalContent"/>.
        /// </summary>
        public static readonly DependencyProperty AdditionalContentProperty =
            DependencyProperty.Register("AdditionalContent", typeof(ContentControl), typeof(CardCollapse), null);

        /// <summary>
        /// Property for <see cref="BorderCommand"/>.
        /// </summary>
        public static readonly DependencyProperty BorderCommandProperty =
            DependencyProperty.Register("BorderCommand",
                typeof(Common.RelayCommand), typeof(CardCollapse), new PropertyMetadata(null));

        /// <summary>
        /// Command triggered after clicking the right mouse button on the control.
        /// </summary>
        public Common.RelayCommand BorderCommand => (Common.RelayCommand)GetValue(BorderCommandProperty);

        /// <summary>
        /// Gets information whether the <see cref="Glyph"/> is set.
        /// </summary>
        public bool IsGlyph => (bool)GetValue(IsGlyphProperty);

        /// <summary>
        /// Gets or sets text displayed on the left side of the card.
        /// </summary>
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        /// <summary>
        /// Gets or sets text displayed under main <see cref="Title"/>.
        /// </summary>
        public string Subtitle
        {
            get => (string)GetValue(SubtitleProperty);
            set => SetValue(SubtitleProperty, value);
        }

        /// <summary>
        /// Gets or sets information on whether the content should be collapsed.
        /// </summary>
        public bool IsOpened
        {
            get => (bool)GetValue(IsOpenedProperty);
            set => SetValue(IsOpenedProperty, value);
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
        /// Gets or sets additional content displayed next to the chevron.
        /// </summary>
        public ContentControl AdditionalContent
        {
            get => (ContentControl)GetValue(AdditionalContentProperty);
            set => SetValue(AdditionalContentProperty, value);
        }

        /// <summary>
        /// Creates a new instance of the class and sets the default <see cref="Common.RelayCommand"/> of <see cref="BorderCommand"/>.
        /// </summary>
        public CardCollapse() => SetValue(BorderCommandProperty, new Common.RelayCommand(o => CardOnClick()));

        private void CardOnClick() => IsOpened = !IsOpened;

        private static void OnGlyphChanged(DependencyObject dependency, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (dependency is not CardCollapse control) return;

            control.SetValue(IsGlyphProperty, control.Glyph != Common.Icon.Empty);
            control.SetValue(RawGlyphProperty, Common.Glyph.ToString(control.Glyph));
        }
    }
}