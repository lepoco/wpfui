// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;

namespace WPFUI.Controls
{
    public partial class Dialog : System.Windows.Controls.ContentControl
    {
        public static readonly DependencyProperty ShowProperty = DependencyProperty.Register("Show",
            typeof(bool), typeof(Dialog), new PropertyMetadata(false));

        public static readonly DependencyProperty ButtonLeftCommandProperty =
            DependencyProperty.Register("ButtonLeftCommand",
                typeof(Common.RelayCommand), typeof(Dialog), new PropertyMetadata(null));

        public static readonly DependencyProperty ButtonRightCommandProperty =
            DependencyProperty.Register("ButtonRightCommand",
                typeof(Common.RelayCommand), typeof(Dialog), new PropertyMetadata(null));

        public event RoutedEventHandler Click; 
        
        public bool Show
        {
            get => (bool)GetValue(ShowProperty);
            set => SetValue(ShowProperty, value);
        }

        public Common.RelayCommand ButtonLeftCommand => (Common.RelayCommand)GetValue(ButtonLeftCommandProperty);

        public Common.RelayCommand ButtonRightCommand => (Common.RelayCommand)GetValue(ButtonRightCommandProperty);

        public Dialog()
        {
            SetValue(ButtonLeftCommandProperty, new Common.RelayCommand(o => Click?.Invoke(this, new RoutedEventArgs { })));
            SetValue(ButtonRightCommandProperty, new Common.RelayCommand(o => SetValue(ShowProperty, false)));
        }
    }
}
