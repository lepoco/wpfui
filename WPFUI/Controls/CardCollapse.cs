// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Controls;

namespace WPFUI.Controls
{
    // TODO: Inherit from Expander

    /// <summary>
    /// Inherited from the <see cref="System.Windows.Controls.ContentControl"/> control which can hide the collapsable content.
    /// </summary>
    public class CardCollapse : ContentControl
    {
        #region Properties
        /// <summary>
        /// Property for <see cref="Title"/>.
        /// </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title),
            typeof(string), typeof(CardCollapse), new PropertyMetadata(String.Empty));

        /// <summary>
        /// Property for <see cref="Subtitle"/>.
        /// </summary>
        public static readonly DependencyProperty SubtitleProperty = DependencyProperty.Register(nameof(Subtitle),
            typeof(string), typeof(CardCollapse), new PropertyMetadata(String.Empty));

        /// <summary>
        /// Property for <see cref="Glyph"/>.
        /// </summary>
        public static readonly DependencyProperty GlyphProperty = DependencyProperty.Register(nameof(Glyph),
            typeof(Common.Icon), typeof(CardCollapse), new PropertyMetadata(Common.Icon.Empty, OnGlyphChanged));

        /// <summary>
        /// <see cref="System.String"/> property for <see cref="Glyph"/>.
        /// </summary>
        public static readonly DependencyProperty RawGlyphProperty = DependencyProperty.Register(nameof(RawGlyph),
            typeof(string), typeof(CardCollapse), new PropertyMetadata(String.Empty));

        /// <summary>
        /// <see cref="System.String"/> property for <see cref="Filled"/>.
        /// </summary>
        public static readonly DependencyProperty FilledProperty = DependencyProperty.Register(nameof(Filled),
            typeof(bool), typeof(CardCollapse), new PropertyMetadata(false, OnGlyphChanged));

        /// <summary>
        /// Property for <see cref="IsOpened"/>.
        /// </summary>
        public static readonly DependencyProperty IsOpenedProperty = DependencyProperty.Register(nameof(IsOpened),
            typeof(bool), typeof(CardCollapse), new PropertyMetadata(false));

        /// <summary>
        /// Property for <see cref="IsGlyph"/>.
        /// </summary>
        public static readonly DependencyProperty IsGlyphProperty = DependencyProperty.Register(nameof(IsGlyph),
            typeof(bool), typeof(CardCollapse), new PropertyMetadata(false));

        /// <summary>
        /// Property for <see cref="AdditionalContent"/>.
        /// </summary>
        public static readonly DependencyProperty AdditionalContentProperty =
            DependencyProperty.Register(nameof(AdditionalContent), typeof(object), typeof(CardCollapse),
                new PropertyMetadata(null, OnAdditionalContentChanged));

        /// <summary>
        /// Property for <see cref="BorderCommand"/>.
        /// </summary>
        public static readonly DependencyProperty BorderCommandProperty =
            DependencyProperty.Register(nameof(BorderCommand),
                typeof(Common.RelayCommand), typeof(CardCollapse), new PropertyMetadata(null));

        /// <summary>
        /// Routed event for <see cref="ContentOpening"/>.
        /// </summary>
        public static readonly RoutedEvent ContentOpeningEvent = EventManager.RegisterRoutedEvent(
            nameof(ContentOpening), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CardCollapse));

        /// <summary>
        /// Routed event for <see cref="ContentClosing"/>.
        /// </summary>
        public static readonly RoutedEvent ContentClosingEvent = EventManager.RegisterRoutedEvent(
            nameof(ContentClosing), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CardCollapse));

        #endregion

        #region Public

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
            set
            {
                if (IsOpened == value)
                {
                    return;
                }

                SetValue(IsOpenedProperty, value);
                RaiseEvent(value ? new RoutedEventArgs(ContentOpeningEvent, this) : new RoutedEventArgs(ContentClosingEvent, this));
            }
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
        /// Defines whether or not we should use the <see cref="Common.IconFilled"/>.
        /// </summary>
        public bool Filled
        {
            get => (bool)GetValue(FilledProperty);
            set => SetValue(FilledProperty, value);
        }

        /// <summary>
        /// Gets or sets additional content displayed next to the chevron.
        /// </summary>
        public object AdditionalContent
        {
            get => GetValue(AdditionalContentProperty);
            set => SetValue(AdditionalContentProperty, value);
        }

        /// <summary>
        /// Gets or sets <see cref="RoutedEvent"/> triggered when the content is to be shown or hidden.
        /// </summary>
        public event RoutedEventHandler ContentOpening
        {
            add => AddHandler(ContentOpeningEvent, value);
            remove => RemoveHandler(ContentOpeningEvent, value);
        }

        /// <summary>
        /// Gets or sets <see cref="RoutedEvent"/> triggered when the content is to be shown or hidden.
        /// </summary>
        public event RoutedEventHandler ContentClosing
        {
            add => AddHandler(ContentClosingEvent, value);
            remove => RemoveHandler(ContentClosingEvent, value);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the class and sets the default <see cref="Common.RelayCommand"/> of <see cref="BorderCommand"/>.
        /// </summary>
        public CardCollapse() => SetValue(BorderCommandProperty, new Common.RelayCommand(o => CardOnClick()));

        #endregion

        #region Methods

        private void CardOnClick() => IsOpened = !IsOpened;

        private static void OnGlyphChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not CardCollapse control) return;

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

        private static void OnAdditionalContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // TODO: Verify content
        }

        #endregion
    }
}