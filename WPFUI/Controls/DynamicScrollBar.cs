// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;

namespace WPFUI.Controls
{
    /// <summary>
    /// Custom <see cref="System.Windows.Controls.Primitives.ScrollBar"/> with events depending on actions taken by the user.
    /// </summary>
    public class DynamicScrollBar : System.Windows.Controls.Primitives.ScrollBar
    {
        /// <summary>
        /// Property for <see cref="IsScrolling"/>.
        /// </summary>
        public static readonly DependencyProperty IsScrollingProperty = DependencyProperty.Register(nameof(IsScrolling),
            typeof(bool), typeof(DynamicScrollBar), new PropertyMetadata(false, IsScrollingChangedCallback));

        /// <summary>
        /// Property for <see cref="IsInteracted"/>.
        /// </summary>
        public static readonly DependencyProperty IsInteractedProperty = DependencyProperty.Register(
            nameof(IsInteracted),
            typeof(bool), typeof(DynamicScrollBar), new PropertyMetadata(false, IsScrollingChangedCallback));

        /// <summary>
        /// Gets or sets information whether the user was scrolling for the last few seconds.
        /// </summary>
        public bool IsScrolling
        {
            get => (bool)GetValue(IsScrollingProperty);
            set => SetValue(IsScrollingProperty, value);
        }

        /// <summary>
        /// Informs whether the user has taken an action related to scrolling.
        /// </summary>
        public bool IsInteracted
        {
            get => (bool)GetValue(IsInteractedProperty);
            set
            {
                if ((bool)GetValue(IsInteractedProperty) != value)
                    SetValue(IsInteractedProperty, value);
            }
        }

        /// <summary>
        /// Creates a new instance of the class and assigns events to the mouse.
        /// </summary>
        public DynamicScrollBar()
        {
            // TODO: Something strange is happening here, sometimes it gets stuck. MouseOver is likely to exist without IsScrolling
            MouseEnter += (sender, args) => { IsInteracted = true; };
            MouseLeave += (sender, args) => { IsInteracted = IsScrolling; };
        }

        private static void IsScrollingChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not DynamicScrollBar control) return;

            control.IsInteracted = control.IsMouseOver || control.IsScrolling;
        }
    }
}