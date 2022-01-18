// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;

namespace WPFUI.Theme
{
    /// <summary>
    /// Listens for <see cref="SystemParameters"/> changes while waiting for <see cref="SystemParameters.StaticPropertyChanged"/> to change, then switches theme with <see cref="Manager.Switch"/>.
    /// </summary>
    public sealed class Watcher
    {
        private readonly Style _currentTheme;

        /// <summary>
        /// Creates new instance of <see cref="Watcher"/>.
        /// </summary>
        public Watcher()
        {
            _currentTheme = Manager.GetSystemTheme();

            SystemParameters.StaticPropertyChanged += OnSystemPropertyChanged;
        }

        /// <summary>
        /// Creates new instance of <see cref="Watcher"/> and triggers <see cref="Watcher.Switch"/>.
        /// </summary>
        /// <returns></returns>
        public static Watcher Start()
        {
            Watcher instance = new();

            instance.Switch();

            return instance;
        }

        /// <summary>
        /// Gets current theme using <see cref="Manager.GetSystemTheme"/> and set accents to <see cref="SystemParameters.WindowGlassColor"/>.
        /// </summary>
        public void Switch()
        {
            Manager.Switch(Manager.GetSystemTheme());
            Manager.SetSystemAccent();
        }

        private void OnSystemPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "WindowGlassColor")
            {
                return;
            }

            Style systemTheme = Manager.GetSystemTheme();

            if (systemTheme != _currentTheme)
            {
                Manager.Switch(systemTheme);
                Manager.SetSystemAccent();
            }
        }
    }
}