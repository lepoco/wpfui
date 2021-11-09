// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.ComponentModel;
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
        /// Event handled triggered by <see cref="OnPropertyChanged(string)"/>.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the instance of <see cref="System.Windows.Controls.Page"/> that will be used to display in frame.
        /// </summary>
        public Object Instance { get; set; }

        /// <summary>
        /// Gets or sets the type of <see cref="System.Windows.Controls.Page"/> that will be used to create the instance.
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Gets or sets the name of the item that will be displayed on the menu.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets an identifier by which the menu can be navigated with the help of <see cref="Controls.Navigation.Navigate(string, bool)"/>.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Gets or sets method that will be run when clicking.
        /// </summary>
        public Action Action { get; set; }

        /// <summary>
        /// Gets information whether an <see cref="NavItem"/> has subelements.
        /// </summary>
        public bool IsDropdown { get { return SubItems != null && SubItems.Length > 0; } }

        /// <summary>
        /// Gets or sets subelements for the <see cref="NavItem"/>. If it is established, the main instance is ignored.
        /// </summary>
        public NavItem[] SubItems { get; set; }

        /// <summary>
        /// Gets or sets the icon that will be displayed in the menu.
        /// </summary>
        public Common.Icon Icon { get; set; }

        /// <summary>
        /// Gets the unicode character that corresponds to the selected icon.
        /// </summary>
        public char RawIcon => Common.Glyph.ToGlyph(this.Icon);

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
                this.Image = new BitmapImage(new Uri(value));
            }
        }

        /// <summary>
        /// Gets or sets whether the current item is active at the moment.
        /// </summary>
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (value != this._isActive)
                {
                    this._isActive = value;
                    this.OnPropertyChanged("IsActive");
                }
            }
        }

        /// <summary>
        /// Triggered to inform the view that the value has been updated.
        /// </summary>
        /// <param name="name">Name of the current updating element.</param>
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
