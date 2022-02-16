// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;

// https://docs.microsoft.com/en-us/fluent-ui/web-components/components/badge

namespace WPFUI.Controls
{
    /// <summary>
    /// Used to highlight an item, attract attention or flag status.
    /// </summary>
    public class Badge : System.Windows.Controls.ContentControl
    {
        /// <summary>
        /// Property for <see cref="Appearance"/>.
        /// </summary>
        public static readonly DependencyProperty AppearanceProperty = DependencyProperty.Register(nameof(Appearance),
            typeof(Common.Appearance), typeof(Badge),
            new PropertyMetadata(Common.Appearance.Primary));

        /// <summary>
        /// Gets or sets the <see cref="Common.Appearance"/> of the control, if available.
        /// </summary>
        public Common.Appearance Appearance
        {
            get => (Common.Appearance)GetValue(AppearanceProperty);
            set => SetValue(AppearanceProperty, value);
        }
    }
}
