// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Appearance;

public static class ThemeUtils
{
    public static void ApplyTheme(this FrameworkElement frameworkElement)
    {
        Apply(frameworkElement);
        Wpf.Ui.Appearance.ApplicationThemeManager.Changed += (s, e) =>
        {
            Apply(frameworkElement);
            if (frameworkElement is Window w)
            {
                if (w != UiApplication.Current.MainWindow)
                    WindowBackgroundManager.UpdateBackground(w, s, Wpf.Ui.Controls.WindowBackdropType.None, true);
            }
        };

#if DEBUG
        if (frameworkElement is Window window)
        {
            window.KeyDown += (s, e) =>
            {
                if (e.Key == System.Windows.Input.Key.T)
                {
                    var applicationTheme = ApplicationThemeManager.GetAppTheme() == ApplicationTheme.Light
                       ? ApplicationTheme.Dark
                       : ApplicationTheme.Light;

                    ApplicationThemeManager.Apply(applicationTheme, updateAccent: false);
                }

                if (e.Key == System.Windows.Input.Key.C)
                {
                    var rnd = new Random();
                    var randomColor = Color.FromRgb((byte)rnd.Next(256), (byte)rnd.Next(256), (byte)rnd.Next(256));

                    ApplicationAccentColorManager.Apply(randomColor, ApplicationThemeManager.GetAppTheme());

                    var current = ApplicationThemeManager.GetAppTheme();
                    var applicationTheme = ApplicationThemeManager.GetAppTheme() == ApplicationTheme.Light
                                       ? ApplicationTheme.Dark
                                       : ApplicationTheme.Light;

                    ApplicationThemeManager.Apply(applicationTheme, updateAccent: false);
                    ApplicationThemeManager.Apply(current, updateAccent: false);
                }
            };
        }
#endif
    }

    /// <summary>
    /// Applies Resources in the <paramref name="frameworkElement"/>.
    /// </summary>
    public static void Apply(FrameworkElement frameworkElement)
    {
        if (frameworkElement is null)
            return;

        var resourcesRemove = frameworkElement.Resources.MergedDictionaries
            .Where(e => e.Source is not null)
            //.Where(e => e.Source.ToString().ToLower().Contains(Wpf.Ui.Appearance.ApplicationThemeManager.LibraryNamespace))
            .Where(e => e.Source.ToString().ToLower().Contains("Wpf.Ui;"))
            .ToArray();

        foreach (var resource in resourcesRemove)
        {
            //System.Console.WriteLine(
            //    $"INFO | {typeof(MainView)} Remove {resource.Source}",
            //    "Wpf.Ui.Appearance"
            //);
            frameworkElement.Resources.MergedDictionaries.Remove(resource);
        }

        foreach (var resource in UiApplication.Current.Resources.MergedDictionaries)
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