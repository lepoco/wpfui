// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace WPFUI.Theme
{
    public class Watcher
    {
        private Style _currentTheme = Style.Light;

        public Watcher()
        {
            this._currentTheme = Manager.GetSystemTheme();
            SystemParameters.StaticPropertyChanged += ChangeThemeBySystem;

            Manager.Switch(Manager.GetSystemTheme());
            ChangeAccentColor(SystemParameters.WindowGlassColor);
        }

        //protected override void OnClosed(EventArgs e)
        //{
        //    SystemParameters.StaticPropertyChanged -= this.SystemParameters_StaticPropertyChanged;
        //    base.OnClosed(e);
        //}

        private void OnThemeChanged()
        {
#if DEBUG
            Debug.WriteLine("Theme Changed");
#endif
            Manager.Switch(Manager.GetSystemTheme());
        }

        private void ChangeThemeBySystem(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //Debug.WriteLine("Property Changed");
            //Debug.WriteLine(e.PropertyName);
            //Debug.WriteLine(SystemParameters.WindowGlassColor);


            //Debug.WriteLine("WindowGlassBrush");
            //Debug.WriteLine(SystemParameters.WindowGlassBrush);

            //Debug.WriteLine("NavigationChromeStyleKey");
            //Debug.WriteLine(SystemParameters.NavigationChromeStyleKey);

            //Debug.WriteLine("HighContrast");
            //Debug.WriteLine(SystemParameters.HighContrast);

            if (e.PropertyName == "WindowGlassColor")
            {
                Style systemTheme = Manager.GetSystemTheme();

                if (this._currentTheme != systemTheme)
                {
                    Manager.Switch(systemTheme);
                }

                ChangeAccentColor(SystemParameters.WindowGlassColor);
            }
        }

        private void ChangeAccentColor(Color accentColor)
        {
            Color alternativeColor = accentColor;
            switch (this._currentTheme)
            {
                case Style.Dark:
                    alternativeColor = Color.Multiply(accentColor, (float)2);
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

            Application.Current.Resources["WUAccent"] = alternativeBrush;
            Application.Current.Resources["WUElementActive"] = alternativeBrush;
            Application.Current.Resources["WUHyperlink"] = alternativeBrush;
            Application.Current.Resources["WUButton"] = alternativeBrush;
            Application.Current.Resources["WUButtonHover"] = systemBrush;
        }
    }
}
