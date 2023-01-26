// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.ObjectModel;
using System.Windows;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Navigation;
using Wpf.Ui.Gallery.Views.Pages;
using Wpf.Ui.Gallery.Views.Pages.BasicInput;
using Wpf.Ui.Gallery.Views.Pages.Collections;
using Wpf.Ui.Gallery.Views.Pages.DateAndTime;
using Wpf.Ui.Gallery.Views.Pages.DialogsAndFlyouts;
using Wpf.Ui.Gallery.Views.Pages.Icons;
using Wpf.Ui.Gallery.Views.Pages.Media;
using Wpf.Ui.Gallery.Views.Pages.Navigation;
using Wpf.Ui.Gallery.Views.Pages.StatusAndInfo;
using Wpf.Ui.Gallery.Views.Pages.Text;
using Wpf.Ui.Gallery.Views.Pages.Windows;

namespace Wpf.Ui.Gallery.ViewModels.Windows;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;

    [ObservableProperty]
    private string _applicationTitle;

    [ObservableProperty]
    private ICollection<object> _menuItems;

    [ObservableProperty]
    private ICollection<object> _footerMenuItems = new ObservableCollection<object>();

    public MainWindowViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _applicationTitle = "WPF UI Gallery";

        _menuItems = new ObservableCollection<object>
        {
            new NavigationViewItem("Home", SymbolRegular.Home24, typeof(DashboardPage)),
            new NavigationViewItem("All Controls", SymbolRegular.List24, typeof(AllControlsPage)),
            new NavigationViewItemSeparator(),
            new NavigationViewItem {Content = "Basic input", Icon = new SymbolIcon { Symbol = SymbolRegular.CheckboxChecked24  }, TargetPageType = typeof(BasicInputPage), MenuItems = new ObservableCollection<object>
            {
                new NavigationViewItem { Content = "Anchor", TargetPageType = typeof(AnchorPage) },
                new NavigationViewItem { Content = "Button", TargetPageType = typeof(ButtonPage) },
                new NavigationViewItem { Content = "Hyperlink", TargetPageType = typeof(HyperlinkPage) },
                new NavigationViewItem { Content = "ToggleButton", TargetPageType = typeof(ToggleButtonPage) },
                new NavigationViewItem { Content = "ToggleSwitch", TargetPageType = typeof(ToggleSwitchPage) },
                new NavigationViewItem { Content = "CheckBox", TargetPageType = typeof(CheckBoxPage) },
                new NavigationViewItem { Content = "ComboBox", TargetPageType = typeof(ComboBoxPage) },
                new NavigationViewItem { Content = "RadioButton", TargetPageType = typeof(RadioButtonPage) },
                new NavigationViewItem { Content = "RatingControl", TargetPageType = typeof(RatingPage) },
                new NavigationViewItem { Content = "ThumbRate", TargetPageType = typeof(ThumbRatePage) },
                new NavigationViewItem { Content = "Slider", TargetPageType = typeof(SliderPage) },
            }},
            new NavigationViewItem {Content = "Collections", Icon = new SymbolIcon { Symbol = SymbolRegular.Table24  }, TargetPageType = typeof(CollectionsPage), MenuItems = new ObservableCollection<object>
            {
                new NavigationViewItem { Content = "DataGrid", TargetPageType = typeof(DataGridPage) },
                new NavigationViewItem { Content = "ListBox", TargetPageType = typeof(ListBoxPage) },
                new NavigationViewItem { Content = "ListView", TargetPageType = typeof(ListViewPage) },
                new NavigationViewItem { Content = "TreeView", TargetPageType = typeof(TreeViewPage) },
#if DEBUG
                new NavigationViewItem { Content = "TreeList", TargetPageType = typeof(TreeListPage) },
#endif
            }},
            new NavigationViewItem {Content = "Date and Time", Icon = new SymbolIcon { Symbol = SymbolRegular.CalendarClock24  }, TargetPageType = typeof(DateAndTimePage), MenuItems = new ObservableCollection<object>
            {
                new NavigationViewItem { Content = "Calendar", TargetPageType = typeof(CalendarPage) },
                new NavigationViewItem { Content = "DatePicker", TargetPageType = typeof(DatePickerPage) },
            }},
            new NavigationViewItem {Content = "Dialogs and Flyouts", Icon = new SymbolIcon { Symbol = SymbolRegular.Chat24  }, TargetPageType = typeof(DialogsAndFlyoutsPage), MenuItems = new ObservableCollection<object>
            {
                new NavigationViewItem { Content = "Snackbar", TargetPageType = typeof(SnackbarPage) },
                new NavigationViewItem { Content = "Dialog", TargetPageType = typeof(DialogPage) },
                new NavigationViewItem { Content = "Flyout", TargetPageType = typeof(FlyoutPage) },
                new NavigationViewItem { Content = "MessageBox", TargetPageType = typeof(MessageBoxPage) },
            }},
            new NavigationViewItem {Content = "Media", Icon = new SymbolIcon { Symbol = SymbolRegular.PlayCircle24  }, TargetPageType = typeof(MediaPage), MenuItems = new ObservableCollection<object>
            {
                new NavigationViewItem { Content = "Image", TargetPageType = typeof(ImagePage) },
                new NavigationViewItem { Content = "Canvas", TargetPageType = typeof(CanvasPage) },
                new NavigationViewItem { Content = "WebView", TargetPageType = typeof(WebViewPage) },
                new NavigationViewItem { Content = "WebBrowser", TargetPageType = typeof(WebBrowserPage) },
            }},
            new NavigationViewItem {Content = "Navigation", Icon = new SymbolIcon { Symbol = SymbolRegular.Navigation24  }, TargetPageType = typeof(NavigationPage), MenuItems = new ObservableCollection<object>
            {
                new NavigationViewItem { Content = "BreadcrumbBar", TargetPageType = typeof(BreadcrumbBarPage) },
                new NavigationViewItem { Content = "NavigationView", TargetPageType = typeof(NavigationViewPage) },
                new NavigationViewItem { Content = "Multilevel navigation demo", TargetPageType = typeof(MultilevelNavigationPage) },
                new NavigationViewItem { Content = "TabControl", TargetPageType = typeof(TabControlPage) },
            }},
            new NavigationViewItem {Content = "Status and Info", Icon = new SymbolIcon { Symbol = SymbolRegular.ChatBubblesQuestion24  }, TargetPageType = typeof(StatusAndInfoPage), MenuItems = new ObservableCollection<object>
            {
                new NavigationViewItem { Content = "InfoBar", TargetPageType = typeof(InfoBarPage) },
                new NavigationViewItem { Content = "ProgressBar", TargetPageType = typeof(ProgressBarPage) },
                new NavigationViewItem { Content = "ProgressRing", TargetPageType = typeof(ProgressRingPage) },
                new NavigationViewItem { Content = "ToolTip", TargetPageType = typeof(ToolTipPage) },
            }},
            new NavigationViewItem {Content = "Text", Icon = new SymbolIcon { Symbol = SymbolRegular.DrawText24  }, TargetPageType = typeof(TextPage), MenuItems = new ObservableCollection<object>
            {
                new NavigationViewItem { Content = "AutoSuggestBox", TargetPageType = typeof(AutoSuggestBoxPage) },
                new NavigationViewItem { Content = "NumberBox", TargetPageType = typeof(NumberBoxPage) },
                new NavigationViewItem { Content = "PasswordBox", TargetPageType = typeof(PasswordBoxPage) },
                new NavigationViewItem { Content = "RichTextBox", TargetPageType = typeof(RichTextBoxPage) },
                new NavigationViewItem { Content = "Label", TargetPageType = typeof(LabelPage) },
                new NavigationViewItem { Content = "TextBlock", TargetPageType = typeof(TextBlockPage) },
                new NavigationViewItem { Content = "TextBox", TargetPageType = typeof(TextBoxPage) },
            }},
            new NavigationViewItem {Content = "Icons", Icon = new SymbolIcon { Symbol = SymbolRegular.Fluent24  }, TargetPageType = typeof(IconsPage), MenuItems = new ObservableCollection<object>
            {
                new NavigationViewItem { Content = "SymbolIcon", TargetPageType = typeof(SymbolIconPage) },
                new NavigationViewItem { Content = "FontIcon", TargetPageType = typeof(FontIconPage) },
            }},
            new NavigationViewItem("Windows", SymbolRegular.WindowApps24, typeof(WindowsPage))
        };

        var toggleThemeNavigationViewItem = new NavigationViewItem
        {
            Content = "Toggle theme",
            Icon = new SymbolIcon { Symbol = SymbolRegular.PaintBrush24 }
        };
        toggleThemeNavigationViewItem.Click += OnToggleThemeClicked;

        _footerMenuItems.Add(toggleThemeNavigationViewItem);
        _footerMenuItems.Add(new NavigationViewItem { Content = "Settings", Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 }, TargetPageType = typeof(SettingsPage) });
    }

    private void OnToggleThemeClicked(object sender, RoutedEventArgs e)
    {
        var currentTheme = Wpf.Ui.Appearance.Theme.GetAppTheme();

        Wpf.Ui.Appearance.Theme.Apply(currentTheme == Wpf.Ui.Appearance.ThemeType.Light ? Wpf.Ui.Appearance.ThemeType.Dark : Wpf.Ui.Appearance.ThemeType.Light);
    }
}
