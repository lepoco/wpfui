// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace WPFUI.Theme
{
    /// <summary>
    /// Listens for <see cref="SystemParameters"/> changes while waiting for <see cref="SystemParameters.StaticPropertyChanged"/> to change, then switches theme with <see cref="Manager.Switch"/>.
    /// </summary>
    public class Watcher
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
            ChangeAccentColor(SystemParameters.WindowGlassColor);
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
            }

            ChangeAccentColor(SystemParameters.WindowGlassColor);
        }

        private void ChangeAccentColor(Color accentColor)
        {
            Color alternativeColor = accentColor;

            switch (_currentTheme)
            {
                case Style.Dark:
                    alternativeColor = Color.Multiply(accentColor, 2);
                    break;

                case Style.Light:
                    alternativeColor = Color.Multiply(accentColor, (float)0.6);
                    break;

                case Style.Glow:
                    alternativeColor = Color.FromRgb(219, 128, 229);
                    accentColor = Color.FromRgb(201, 146, 210);
                    break;

                case Style.CapturedMotion:
                    alternativeColor = Color.FromRgb(240, 129, 102);
                    accentColor = Color.FromRgb(223, 119, 94);
                    break;

                case Style.Sunrise:
                    alternativeColor = Color.FromRgb(32, 101, 123);
                    accentColor = Color.FromRgb(52, 117, 135);
                    break;

                case Style.Flow:
                    alternativeColor = Color.FromRgb(76, 95, 107);
                    accentColor = Color.FromRgb(96, 108, 121);
                    break;
            }
#if DEBUG
            Debug.WriteLine(accentColor);
            Debug.WriteLine(alternativeColor);
#endif

            SolidColorBrush systemBrush = new(accentColor);
            SolidColorBrush alternativeBrush = new(alternativeColor);

            Application.Current.Resources["UiBrushElementActive"] = alternativeBrush;
            Application.Current.Resources["UiBrushNavigationBadgeActive"] = alternativeBrush;
            Application.Current.Resources["UiBrushHyperlink"] = alternativeBrush;

            Application.Current.Resources["UiBrushButtonBackground"] = alternativeBrush;
            Application.Current.Resources["UiBrushButtonHover"] = systemBrush;
        }
    }
}
