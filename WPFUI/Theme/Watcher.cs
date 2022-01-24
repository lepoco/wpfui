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
        private const string GlassColorPropertyName = "WindowGlassColor";

        /// <summary>
        /// Stores current theme.
        /// </summary>
        private Style _currentTheme;

        /// <summary>
        /// Gets or sets value deciding if the mica effect is to be automatically applied.
        /// </summary>
        public bool UseMica { get; set; } = false;

        /// <summary>
        /// Gets or sets value deciding if the system accent color is to be automatically updated.
        /// </summary>
        public bool UseAccent { get; set; } = false;

        /// <summary>
        /// Creates new instance of <see cref="Watcher"/>.
        /// </summary>
        public Watcher()
        {
            _currentTheme = SystemTheme.GetTheme();

            SystemParameters.StaticPropertyChanged += OnSystemPropertyChanged;
        }

        /// <summary>
        /// Creates new instance of <see cref="Watcher"/> and triggers <see cref="Manager.Switch"/>.
        /// </summary>
        public static Watcher Start(bool useMica = false, bool useAccent = false)
        {
            Watcher instance = new()
            {
                UseAccent = useAccent,
                UseMica = useMica
            };

            Manager.Switch(SystemTheme.GetTheme(), instance.UseMica, instance.UseAccent);

            return instance;
        }

        /// <summary>
        /// Triggered when one of the system properties changes.
        /// </summary>
        private void OnSystemPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != GlassColorPropertyName) return;

            Style systemTheme = SystemTheme.GetTheme();

            if (systemTheme == _currentTheme) return;

            _currentTheme = systemTheme;

            Manager.Switch(SystemTheme.GetTheme(), UseMica, UseAccent);
        }
    }
}