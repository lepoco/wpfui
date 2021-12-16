// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WPFUI.Common
{
    /// <summary>
    /// An interactive element that is a block in the menu for the <see cref="WPFUI.Controls.Navigation"/> control.
    /// </summary>
    public class NavItem : INotifyPropertyChanged
    {
        private bool _isActive = false;

        /// <summary>
        /// Handles changes of properties.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the instance of <see cref="System.Windows.Controls.Page"/> that will be used to display in frame.
        /// </summary>
        public Object Instance { get; set; }

        /// <summary>
        /// Gets or sets the type of <see cref="System.Windows.Controls.Page"/> that will be used to create the instance.
        /// </summary>
        public Type Type { get; set; } = null;

        /// <summary>
        /// Gets or sets the name of the item that will be displayed on the menu.
        /// </summary>
        public string Name { get; set; } = String.Empty;

        /// <summary>
        /// Gets or sets an identifier by which the menu can be navigated with the help of <see cref="Controls.Navigation.Navigate(string, bool)"/>.
        /// </summary>
        public string Tag { get; set; } = String.Empty;

        /// <summary>
        /// Gets information whether an <see cref="NavItem"/> has subelements.
        /// </summary>
        public bool IsDropdown => SubItems != null && SubItems.Length > 0;

        /// <summary>
        /// Gets or sets subelements for the <see cref="NavItem"/>. If it is established, the main instance is ignored.
        /// </summary>
        public NavItem[] SubItems { get; set; }

        /// <summary>
        /// Gets or sets the icon that will be displayed in the menu.
        /// </summary>
        public Icon Icon { get; set; }

        /// <summary>
        /// Gets the unicode character that corresponds to the selected icon.
        /// </summary>
        public char RawIcon => Glyph.ToGlyph(Icon);

        /// <summary>
        /// Gets or sets the graphical icon that will be displayed in the menu.
        /// </summary>
        public BitmapSource Image { get; set; }

        /// <summary>
        /// Sets image src using <see cref="Uri"/> or <see cref="string"/>.
        /// </summary>
        public string ImageUri
        {
            set
            {
                Image = new BitmapImage(new Uri(value));
            }
        }

        /// <summary>
        /// Item visibility.
        /// </summary>
        public Visibility Visibility { get; set; } = Visibility.Visible;

        /// <summary>
        /// Gets or sets whether the current item is active at the moment.
        /// </summary>
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (value != _isActive)
                {
                    _isActive = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsActive"));
                }
            }
        }

        /// <summary>
        /// Gets information whether the navigation item was instantiated correctly.
        /// </summary>
        public bool IsValid => !String.IsNullOrEmpty(Tag) && Type != null;

        /// <summary>
        /// Triggered after clicking action button.
        /// </summary>
        public event RoutedEventHandler Click;

        /// <summary>
        /// Triggers <see cref="Click"/> and sets <see cref="IsActive"/> to <see langword="true"/>.
        /// </summary>
        /// <param name="sender"></param>
        public void Invoke(object sender)
        {
            IsActive = true;

            Click?.Invoke(sender, new RoutedEventArgs() { });

            if (Type != null && Type.GetMethod("OnNavigationRequest") != null)
                Type.GetMethod("OnNavigationRequest")?.Invoke(Instance, new[] { sender });
        }
    }
}
