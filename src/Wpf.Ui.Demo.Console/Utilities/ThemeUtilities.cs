// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui;
using Wpf.Ui.Appearance;

public static class ThemeUtilities
{
    public static void ApplyTheme(this FrameworkElement frameworkElement)
    {
        ApplicationThemeManager.Apply(frameworkElement);

        ThemeChangedEvent themeChanged = (sender, args) =>
        {
            ApplicationThemeManager.Apply(frameworkElement);
            if (frameworkElement is Window window)
            {
                if (window != UiApplication.Current.MainWindow)
                {
                    WindowBackgroundManager.UpdateBackground(
                        window,
                        sender,
                        Wpf.Ui.Controls.WindowBackdropType.None,
                        true
                    );
                }
            }
        };

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

    /// <summary>
    /// Applies Resources in the <paramref name="frameworkElement"/>.
    /// </summary>
    private static void Apply(FrameworkElement frameworkElement)
    {
        if (frameworkElement is null)
        {
            return;
        }

        ResourceDictionary[] resourcesRemove = frameworkElement
            .Resources.MergedDictionaries.Where(e => e.Source is not null)
            //.Where(e => e.Source.ToString().ToLower().Contains(Wpf.Ui.Appearance.ApplicationThemeManager.LibraryNamespace))
            .Where(e => e.Source.ToString().ToLower().Contains("Wpf.Ui;"))
            .ToArray();

        foreach (ResourceDictionary? resource in resourcesRemove)
        {
            //System.Console.WriteLine(
            //    $"INFO | {typeof(MainView)} Remove {resource.Source}",
            //    "Wpf.Ui.Appearance"
            //);
            frameworkElement.Resources.MergedDictionaries.Remove(resource);
        }

        foreach (ResourceDictionary? resource in UiApplication.Current.Resources.MergedDictionaries)
        {
            //System.Console.WriteLine(
            //    $"INFO | {typeof(MainView)} Add {resource.Source}",
            //    "Wpf.Ui.Appearance"
            //);
            frameworkElement.Resources.MergedDictionaries.Add(resource);
        }

        foreach (System.Collections.DictionaryEntry resource in UiApplication.Current.Resources)
        {
            //System.Console.WriteLine(
            //    $"INFO | {typeof(MainView)} Copy Resource {resource.Key} - {resource.Value}",
            //    "Wpf.Ui.Appearance"
            //);
            frameworkElement.Resources[resource.Key] = resource.Value;
        }
    }
}
