// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WPFUI.Controls
{
    
    public partial class CardProfile : ContentControl
    {
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", 
            typeof(BitmapSource), typeof(CardProfile), new PropertyMetadata(new BitmapImage()));

        public BitmapSource Source
        {
            get => GetValue(SourceProperty) as BitmapSource;
            set => SetValue(SourceProperty, value);
        }
    }
}
