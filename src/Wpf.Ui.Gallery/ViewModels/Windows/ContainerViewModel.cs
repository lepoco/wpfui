// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using Wpf.Ui.Common;
using Wpf.Ui.Controls.Navigation;
using Wpf.Ui.Gallery.Views.Pages;
using Wpf.Ui.Gallery.Views.Pages.BasicInput;
using Wpf.Ui.Gallery.Views.Pages.DialogsAndFlyouts;
using Wpf.Ui.Gallery.Views.Pages.Icons;
using Wpf.Ui.Gallery.Views.Pages.Media;
using Wpf.Ui.Gallery.Views.Pages.Navigation;
using Wpf.Ui.Gallery.Views.Pages.StatusAndInfo;
using Wpf.Ui.Gallery.Views.Pages.Text;

namespace Wpf.Ui.Gallery.ViewModels.Windows;

public partial class ContainerViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;

    [ObservableProperty]
    private string _applicationTitle;

    [ObservableProperty] private ICollection<object> _menuItems;

    [ObservableProperty] private ICollection<object> _footerMenuItems = new ObservableCollection<object>();

    public ContainerViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        ApplicationTitle = "WPF UI Gallery";

        MenuItems = new ObservableCollection<object>
        {
            new NavigationViewItem { Content = "Home", Icon = SymbolRegular.Home24, TargetPageType = typeof(DashboardPage) },
            new NavigationViewItem { Content = "All Controls", Icon = SymbolRegular.List24, TargetPageType = typeof(AllControlsPage) },
            new NavigationViewItemSeparator(),
            new NavigationViewItem {Content = "Basic input", Icon = SymbolRegular.CheckboxChecked24, TargetPageType = typeof(BasicInputPage), MenuItems = new ObservableCollection<object>
            {
                new NavigationViewItem { Content = "Anchor", TargetPageType = typeof(AnchorPage) },
                new NavigationViewItem { Content = "Button", TargetPageType = typeof(ButtonPage) },
                new NavigationViewItem { Content = "Hyperlink", TargetPageType = typeof(HyperlinkPage) },
                new NavigationViewItem { Content = "ToggleButton", TargetPageType = typeof(ToggleButtonPage) },
                new NavigationViewItem { Content = "ToggleSwitch", TargetPageType = typeof(ToggleSwitchPage) },
                new NavigationViewItem { Content = "CheckBox", TargetPageType = typeof(CheckBoxPage) },
                new NavigationViewItem { Content = "ComboBox", TargetPageType = typeof(ComboBoxPage) },
                new NavigationViewItem { Content = "RadioButton", TargetPageType = typeof(RadioButtonPage) },
                new NavigationViewItem { Content = "Rating", TargetPageType = typeof(RatingPage) },
                new NavigationViewItem { Content = "ThumbRate", TargetPageType = typeof(ThumbRatePage) },
                new NavigationViewItem { Content = "Slider", TargetPageType = typeof(SliderPage) },
            }},
            new NavigationViewItem {Content = "Dialogs and Flyouts", Icon = SymbolRegular.Chat24, TargetPageType = typeof(DialogsAndFlyoutsPage), MenuItems = new ObservableCollection<object>
            {
                new NavigationViewItem { Content = "Snackbar", TargetPageType = typeof(SnackbarPage) },
                new NavigationViewItem { Content = "Dialog", TargetPageType = typeof(DialogPage) },
                new NavigationViewItem { Content = "Flyout", TargetPageType = typeof(FlyoutPage) },
                new NavigationViewItem { Content = "MessageBox", TargetPageType = typeof(MessageBoxPage) },
            }},
            new NavigationViewItem {Content = "Media", Icon = SymbolRegular.PlayCircle24, TargetPageType = typeof(MediaPage), MenuItems = new ObservableCollection<object>
            {
                new NavigationViewItem { Content = "WebView", TargetPageType = typeof(WebViewPage) },
            }},
            new NavigationViewItem {Content = "Navigation", Icon = SymbolRegular.Navigation24, TargetPageType = typeof(NavigationPage), MenuItems = new ObservableCollection<object>
            {
                new NavigationViewItem { Content = "NavigationView", TargetPageType = typeof(NavigationViewPage) },
                new NavigationViewItem { Content = "TabControl", TargetPageType = typeof(TabControlPage) },
            }},
            new NavigationViewItem {Content = "Status and Info", Icon = SymbolRegular.ChatBubblesQuestion24, TargetPageType = typeof(StatusAndInfoPage), MenuItems = new ObservableCollection<object>
            {
                new NavigationViewItem { Content = "InfoBar", TargetPageType = typeof(InfoBarPage) },
                new NavigationViewItem { Content = "ProgressBar", TargetPageType = typeof(ProgressBarPage) },
                new NavigationViewItem { Content = "ProgressRing", TargetPageType = typeof(ProgressRingPage) },
                new NavigationViewItem { Content = "ToolTip", TargetPageType = typeof(ToolTipPage) },
            }},
            new NavigationViewItem {Content = "Text", Icon = SymbolRegular.DrawText24, TargetPageType = typeof(TextPage), MenuItems = new ObservableCollection<object>
            {
                new NavigationViewItem { Content = "AutoSuggestBox", TargetPageType = typeof(AutoSuggestBoxPage) },
                new NavigationViewItem { Content = "NumberBox", TargetPageType = typeof(NumberBoxPage) },
                new NavigationViewItem { Content = "PasswordBox", TargetPageType = typeof(PasswordBoxPage) },
                new NavigationViewItem { Content = "RichTextBox", TargetPageType = typeof(RichTextBoxPage) },
                new NavigationViewItem { Content = "Label", TargetPageType = typeof(LabelPage) },
                new NavigationViewItem { Content = "TextBlock", TargetPageType = typeof(TextBlockPage) },
                new NavigationViewItem { Content = "TextBox", TargetPageType = typeof(TextBoxPage) },
            }},
            new NavigationViewItem {Content = "Icons", Icon = SymbolRegular.Fluent24, TargetPageType = typeof(IconsPage), MenuItems = new ObservableCollection<object>
            {
                new NavigationViewItem { Content = "SymbolIcon", TargetPageType = typeof(SymbolIconPage) },
                new NavigationViewItem { Content = "FontIcon", TargetPageType = typeof(FontIconPage) },
            }}
        };

        var toggleThemeNavigationViewItem = new NavigationViewItem
        {
            Content = "Toggle theme",
            Icon = SymbolRegular.PaintBrush24
        };
        toggleThemeNavigationViewItem.Click += OnToggleThemeClicked;

        FooterMenuItems.Add(toggleThemeNavigationViewItem);
        FooterMenuItems.Add(new NavigationViewItem { Content = "Settings", Icon = SymbolRegular.Settings24, TargetPageType = typeof(SettingsPage) });
    }

    private void OnToggleThemeClicked(object sender, RoutedEventArgs e)
    {
        var currentTheme = Wpf.Ui.Appearance.Theme.GetAppTheme();

        Wpf.Ui.Appearance.Theme.Apply(currentTheme == Wpf.Ui.Appearance.ThemeType.Light ? Wpf.Ui.Appearance.ThemeType.Dark : Wpf.Ui.Appearance.ThemeType.Light);
    }
}
