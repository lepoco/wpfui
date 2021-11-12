// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WPFUI.Controls
{
    /// <summary>
    /// Simple element that displays an image in a circular frame like in default applications for Windows 11.
    /// </summary>
    public partial class CardProfile : ContentControl
    {
        /// <summary>
        /// Property for <see cref="Source"/>.
        /// </summary>
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source",
            typeof(BitmapSource), typeof(CardProfile), new PropertyMetadata(new BitmapImage()));

        /// <summary>
        /// Address of the image to be displayed in the circular frame. Does not support transparency.
        /// </summary>
        public BitmapSource Source
        {
            get => GetValue(SourceProperty) as BitmapSource;
            set => SetValue(SourceProperty, value);
        }
    }
}