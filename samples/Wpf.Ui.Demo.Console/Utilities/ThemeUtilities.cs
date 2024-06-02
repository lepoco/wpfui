// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Appearance;

namespace Wpf.Ui.Demo.Console.Utilities;

public static class ThemeUtilities
{
    public static void ApplyTheme(this FrameworkElement frameworkElement)
    {
        ApplicationThemeManager.Apply(frameworkElement);

        void themeChanged(ApplicationTheme sender, Color args)
        {
            ApplicationThemeManager.Apply(frameworkElement);
            if (frameworkElement is Window window)
            {
                if (window != UiApplication.Current.MainWindow)
                {
                    WindowBackgroundManager.UpdateBackground(
                        window,
                        sender,
                        Wpf.Ui.Controls.WindowBackdropType.None
                    );
                }
            }
        }

        if (frameworkElement.IsLoaded)
        {
            ApplicationThemeManager.Changed += themeChanged;
        }

        frameworkElement.Loaded += (s, e) =>
        {
            ApplicationThemeManager.Changed += themeChanged;
        };
        frameworkElement.Unloaded += (s, e) =>
        {
            ApplicationThemeManager.Changed -= themeChanged;
        };

#if DEBUG
        if (frameworkElement is Window window)
        {
            window.KeyDown += (s, e) =>
            {
                if (e.Key == System.Windows.Input.Key.T)
                {
                    ChangeTheme();
                }

                if (e.Key == System.Windows.Input.Key.C)
                {
                    var rnd = new Random();
                    var randomColor = Color.FromRgb(
                        (byte)rnd.Next(256),
                        (byte)rnd.Next(256),
                        (byte)rnd.Next(256)
                    );

                    ApplicationAccentColorManager.Apply(randomColor, ApplicationThemeManager.GetAppTheme());

                    ApplicationTheme current = ApplicationThemeManager.GetAppTheme();
                    ApplicationTheme applicationTheme =
                        ApplicationThemeManager.GetAppTheme() == ApplicationTheme.Light
                            ? ApplicationTheme.Dark
                            : ApplicationTheme.Light;

                    ApplicationThemeManager.Apply(applicationTheme, updateAccent: false);
                    ApplicationThemeManager.Apply(current, updateAccent: false);
                }
            };
        }
#endif
    }

    public static void ChangeTheme()
    {
        ApplicationTheme applicationTheme =
            ApplicationThemeManager.GetAppTheme() == ApplicationTheme.Light
                ? ApplicationTheme.Dark
                : ApplicationTheme.Light;

        ApplicationThemeManager.Apply(applicationTheme, updateAccent: false);
    }
}
