// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Wpf.Ui.Demo.Console;
public partial class MainView : Window
{
    public MainView()
    {
        InitializeComponent();

        Apply(this);
        Appearance.SystemThemeWatcher.Watch(this);

        Appearance.ApplicationThemeManager.Changed += (s, e) =>
        {
            MainView.Apply(this);
        };

        //new Wpf.Ui.Controls.Button().Appearance = Controls.ControlAppearance.Primary;

        //SystemAccentColorPrimary

        //Wpf.Ui.Appearance.Accent.ApplySystemAccent();
        //Wpf.Ui.Appearance.Theme.Apply(this);
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

    private void ButtonClickChangeColor(object sender, RoutedEventArgs e)
    {
        var rnd = new Random();
        var randomColor = Color.FromRgb((byte)rnd.Next(256), (byte)rnd.Next(256), (byte)rnd.Next(256));

        Appearance.ApplicationAccentColorManager.Apply(randomColor, Appearance.ApplicationThemeManager.GetAppTheme());
        //Apply(this);

        var current = Appearance.ApplicationThemeManager.GetAppTheme();
        var applicationTheme = Appearance.ApplicationThemeManager.GetAppTheme() == Appearance.ApplicationTheme.Light
                           ? Appearance.ApplicationTheme.Dark
                           : Appearance.ApplicationTheme.Light;

        Appearance.ApplicationThemeManager.Apply(applicationTheme, updateAccent: false);
        Appearance.ApplicationThemeManager.Apply(current, updateAccent: false);

        //Appearance.ApplicationThemeManager.Changed?.Invoke(applicationTheme, Appearance.ApplicationAccentColorManager.SystemAccent);
    }

    private void ButtonClickChangeTheme(object sender, RoutedEventArgs e)
    {
        var applicationTheme = Appearance.ApplicationThemeManager.GetAppTheme() == Appearance.ApplicationTheme.Light
                           ? Appearance.ApplicationTheme.Dark
                           : Appearance.ApplicationTheme.Light;

        Appearance.ApplicationThemeManager.Apply(applicationTheme, updateAccent: false);
        //Apply(this);
    }
}