// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WPFUI.Common;
using WPFUI.Controls.Interfaces;

namespace WPFUI.Demo.Views;

/// <summary>
/// Interaction logic for Container.xaml
/// </summary>
public partial class Container
{
    public Container()
    {
        InitializeComponent();
        InitializeUi();

        RemoveTitlebar();
        ApplyBackdrop(WPFUI.Appearance.BackgroundType.Mica);

        //this
        //    .GandalfDoMagic()
        //    .ApplyDefaultBackground()
        //    .ApplyCorners(WindowCornerPreference.Round);

        InvokeSplashScreen();
    }

    private void InitializeUi()
    {
        Loaded += (sender, args) =>
        {
            // After loading the main application window,
            // we register the Watcher class, which automatically
            // changes the theme and accent of the application.
            WPFUI.Appearance.Watcher.Watch(this, Appearance.BackgroundType.Mica, true, true);

#if DEBUG
            // If we are in debug mode,
            // we add an additional page in the navigation.
            //RootNavigation.Items.Add(new WPFUI.Controls.NavigationItem
            //{
            //    Page = typeof(Pages.Debug),
            //    Content = "Debug",
            //    Icon = WPFUI.Common.SymbolRegular.Warning24,
            //    IconForeground = System.Windows.Media.Brushes.Red,
            //    IconFilled = true
            //});
#endif
        };
    }

    private void InvokeSplashScreen()
    {
        RootMainGrid.Visibility = Visibility.Collapsed;
        RootWelcomeGrid.Visibility = Visibility.Visible;

        Task.Run(async () =>
        {
            // Remember to always include Delays and Sleeps in
            // your applications to be able to charge the client for optimizations later.
            await Task.Delay(4000);

            Application.Current.Dispatcher.Invoke(() =>
            {
                RootWelcomeGrid.Visibility = Visibility.Hidden;
                RootMainGrid.Visibility = Visibility.Visible;

                RootNavigation.Navigate(0);
            });
        });
    }

    private void NavigationButtonTheme_OnClick(object sender, RoutedEventArgs e)
    {
        // We check what theme is currently
        // active and choose its opposite.
        var newTheme = WPFUI.Appearance.Theme.GetAppTheme() == WPFUI.Appearance.ThemeType.Dark
            ? WPFUI.Appearance.ThemeType.Light
            : WPFUI.Appearance.ThemeType.Dark;

        // We apply the theme to the entire application.
        WPFUI.Appearance.Theme.Apply(
            themeType: newTheme,
            backgroundEffect: WPFUI.Appearance.BackgroundType.Mica,
            updateAccent: true,
            forceBackground: false);
    }

    private void TrayMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        if (sender is not MenuItem menuItem)
            return;

        System.Diagnostics.Debug.WriteLine($"DEBUG | WPF UI Tray clicked: {menuItem.Tag}", "WPFUI.Demo");
    }

    private void RootNavigation_OnNavigated(INavigation sender, RoutedNavigationEventArgs e)
    {
        System.Diagnostics.Debug.WriteLine($"DEBUG | WPF UI Navigated to: {e.CurrentPage.PageTag}", "WPFUI.Demo");

        // This funky solution allows us to impose a negative
        // margin for Frame only for the Dashboard page, thanks
        // to which the banner will cover the entire page nicely.
        RootFrame.Margin = new Thickness(
            left: 0,
            top: e.CurrentPage.PageTag == "dashboard" ? -69 : 0,
            right: 0,
            bottom: 0);
    }

    private void RootDialog_OnButtonRightClick(object sender, RoutedEventArgs e)
    {
        RootDialog.Hide();
    }
}

