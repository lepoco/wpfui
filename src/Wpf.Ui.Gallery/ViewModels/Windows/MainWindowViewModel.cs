// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.ObjectModel;
using Wpf.Ui.Controls;
using Wpf.Ui.Gallery.Views.Pages;
using Wpf.Ui.Gallery.Views.Pages.BasicInput;
using Wpf.Ui.Gallery.Views.Pages.Collections;
using Wpf.Ui.Gallery.Views.Pages.DateAndTime;
using Wpf.Ui.Gallery.Views.Pages.DesignGuidance;
using Wpf.Ui.Gallery.Views.Pages.DialogsAndFlyouts;
using Wpf.Ui.Gallery.Views.Pages.Layout;
using Wpf.Ui.Gallery.Views.Pages.Media;
using Wpf.Ui.Gallery.Views.Pages.Navigation;
using Wpf.Ui.Gallery.Views.Pages.StatusAndInfo;
using Wpf.Ui.Gallery.Views.Pages.Text;
using Wpf.Ui.Gallery.Views.Pages.Windows;

namespace Wpf.Ui.Gallery.ViewModels.Windows;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private string _applicationTitle = "WPF UI Gallery";

    [ObservableProperty]
    private ICollection<object> _menuItems = new ObservableCollection<object>
    {
        new NavigationViewItem("Home", SymbolRegular.Home24, typeof(DashboardPage)),
        new NavigationViewItem()
        {
            Content = "Design guidance",
            Icon = new SymbolIcon { Symbol = SymbolRegular.DesignIdeas24 },
            MenuItems = new object[]
            {
                new NavigationViewItem(
                    "Typography",
                    SymbolRegular.TextFont24,
                    typeof(TypographyPage)
                ),
                new NavigationViewItem("Icons", SymbolRegular.Diversity24, typeof(IconsPage)),
                new NavigationViewItem("Colors", SymbolRegular.Color24, typeof(ColorsPage))
            }
        },
        new NavigationViewItem("All samples", SymbolRegular.List24, typeof(AllControlsPage)),
        new NavigationViewItemSeparator(),
        new NavigationViewItem
        {
            Content = "Basic input",
            Icon = new SymbolIcon { Symbol = SymbolRegular.CheckboxChecked24 },
            TargetPageType = typeof(BasicInputPage),
            MenuItems = new object[]
            {
                new NavigationViewItem { Content = "Anchor", TargetPageType = typeof(AnchorPage) },
                new NavigationViewItem { Content = "Button", TargetPageType = typeof(ButtonPage) },
                new NavigationViewItem
                {
                    Content = "Hyperlink",
                    TargetPageType = typeof(HyperlinkPage)
                },
                new NavigationViewItem
                {
                    Content = "ToggleButton",
                    TargetPageType = typeof(ToggleButtonPage)
                },
                new NavigationViewItem
                {
                    Content = "ToggleSwitch",
                    TargetPageType = typeof(ToggleSwitchPage)
                },
                new NavigationViewItem
                {
                    Content = "CheckBox",
                    TargetPageType = typeof(CheckBoxPage)
                },
                new NavigationViewItem
                {
                    Content = "ComboBox",
                    TargetPageType = typeof(ComboBoxPage)
                },
                new NavigationViewItem
                {
                    Content = "RadioButton",
                    TargetPageType = typeof(RadioButtonPage)
                },
                new NavigationViewItem
                {
                    Content = "RatingControl",
                    TargetPageType = typeof(RatingPage)
                },
                new NavigationViewItem
                {
                    Content = "ThumbRate",
                    TargetPageType = typeof(ThumbRatePage)
                },
                new NavigationViewItem { Content = "Slider", TargetPageType = typeof(SliderPage) },
            }
        },
        new NavigationViewItem
        {
            Content = "Collections",
            Icon = new SymbolIcon { Symbol = SymbolRegular.Table24 },
            TargetPageType = typeof(CollectionsPage),
            MenuItems = new object[]
            {
                new NavigationViewItem
                {
                    Content = "DataGrid",
                    TargetPageType = typeof(DataGridPage)
                },
                new NavigationViewItem
                {
                    Content = "ListBox",
                    TargetPageType = typeof(ListBoxPage)
                },
                new NavigationViewItem
                {
                    Content = "ListView",
                    TargetPageType = typeof(ListViewPage)
                },
                new NavigationViewItem
                {
                    Content = "TreeView",
                    TargetPageType = typeof(TreeViewPage)
                },
#if DEBUG
                new NavigationViewItem
                {
                    Content = "TreeList",
                    TargetPageType = typeof(TreeListPage)
                },
#endif
            }
        },
        new NavigationViewItem
        {
            Content = "Date & time",
            Icon = new SymbolIcon { Symbol = SymbolRegular.CalendarClock24 },
            TargetPageType = typeof(DateAndTimePage),
            MenuItems = new object[]
            {
                new NavigationViewItem
                {
                    Content = "Calendar",
                    TargetPageType = typeof(CalendarPage)
                },
                new NavigationViewItem
                {
                    Content = "DatePicker",
                    TargetPageType = typeof(DatePickerPage)
                },
            }
        },
        new NavigationViewItem
        {
            Content = "Dialogs & flyouts",
            Icon = new SymbolIcon { Symbol = SymbolRegular.Chat24 },
            TargetPageType = typeof(DialogsAndFlyoutsPage),
            MenuItems = new object[]
            {
                new NavigationViewItem
                {
                    Content = "Snackbar",
                    TargetPageType = typeof(SnackbarPage)
                },
                new NavigationViewItem
                {
                    Content = "ContentDialog",
                    TargetPageType = typeof(ContentDialogPage)
                },
                new NavigationViewItem { Content = "Flyout", TargetPageType = typeof(FlyoutPage) },
                new NavigationViewItem
                {
                    Content = "MessageBox",
                    TargetPageType = typeof(MessageBoxPage)
                },
            }
        },
#if DEBUG
        new NavigationViewItem
        {
            Content = "Layout",
            Icon = new SymbolIcon { Symbol = SymbolRegular.News24 },
            TargetPageType = typeof(LayoutPage),
            MenuItems = new object[]
            {
                new NavigationViewItem
                {
                    Content = "Expander",
                    TargetPageType = typeof(ExpanderPage)
                },
            }
        },
#endif
        new NavigationViewItem
        {
            Content = "Media",
            Icon = new SymbolIcon { Symbol = SymbolRegular.PlayCircle24 },
            TargetPageType = typeof(MediaPage),
            MenuItems = new object[]
            {
                new NavigationViewItem { Content = "Image", TargetPageType = typeof(ImagePage) },
                new NavigationViewItem { Content = "Canvas", TargetPageType = typeof(CanvasPage) },
                new NavigationViewItem
                {
                    Content = "WebView",
                    TargetPageType = typeof(WebViewPage)
                },
                new NavigationViewItem
                {
                    Content = "WebBrowser",
                    TargetPageType = typeof(WebBrowserPage)
                },
            }
        },
        new NavigationViewItem
        {
            Content = "Navigation",
            Icon = new SymbolIcon { Symbol = SymbolRegular.Navigation24 },
            TargetPageType = typeof(NavigationPage),
            MenuItems = new object[]
            {
                new NavigationViewItem
                {
                    Content = "BreadcrumbBar",
                    TargetPageType = typeof(BreadcrumbBarPage)
                },
                new NavigationViewItem
                {
                    Content = "NavigationView",
                    TargetPageType = typeof(NavigationViewPage)
                },
                new NavigationViewItem { Content = "Menu", TargetPageType = typeof(MenuPage) },
                new NavigationViewItem
                {
                    Content = "Multilevel navigation",
                    TargetPageType = typeof(MultilevelNavigationPage)
                },
                new NavigationViewItem
                {
                    Content = "TabControl",
                    TargetPageType = typeof(TabControlPage)
                },
            }
        },
        new NavigationViewItem
        {
            Content = "Status & info",
            Icon = new SymbolIcon { Symbol = SymbolRegular.ChatBubblesQuestion24 },
            TargetPageType = typeof(StatusAndInfoPage),
            MenuItems = new object[]
            {
                new NavigationViewItem
                {
                    Content = "InfoBar",
                    TargetPageType = typeof(InfoBarPage)
                },
                new NavigationViewItem
                {
                    Content = "ProgressBar",
                    TargetPageType = typeof(ProgressBarPage)
                },
                new NavigationViewItem
                {
                    Content = "ProgressRing",
                    TargetPageType = typeof(ProgressRingPage)
                },
                new NavigationViewItem
                {
                    Content = "ToolTip",
                    TargetPageType = typeof(ToolTipPage)
                },
            }
        },
        new NavigationViewItem
        {
            Content = "Text",
            Icon = new SymbolIcon { Symbol = SymbolRegular.DrawText24 },
            TargetPageType = typeof(TextPage),
            MenuItems = new object[]
            {
                new NavigationViewItem
                {
                    Content = "AutoSuggestBox",
                    TargetPageType = typeof(AutoSuggestBoxPage)
                },
                new NavigationViewItem
                {
                    Content = "NumberBox",
                    TargetPageType = typeof(NumberBoxPage)
                },
                new NavigationViewItem
                {
                    Content = "PasswordBox",
                    TargetPageType = typeof(PasswordBoxPage)
                },
                new NavigationViewItem
                {
                    Content = "RichTextBox",
                    TargetPageType = typeof(RichTextBoxPage)
                },
                new NavigationViewItem { Content = "Label", TargetPageType = typeof(LabelPage) },
                new NavigationViewItem
                {
                    Content = "TextBlock",
                    TargetPageType = typeof(TextBlockPage)
                },
                new NavigationViewItem
                {
                    Content = "TextBox",
                    TargetPageType = typeof(TextBoxPage)
                },
            }
        },
        new NavigationViewItem("Windows", SymbolRegular.WindowApps24, typeof(WindowsPage))
    };

    [ObservableProperty]
    private ICollection<object> _footerMenuItems = new ObservableCollection<object>()
    {
        new NavigationViewItem
        {
            Content = "Settings",
            Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
            TargetPageType = typeof(SettingsPage)
        }
    };

    [ObservableProperty]
    private ObservableCollection<MenuItem> _trayMenuItems =
        new()
        {
            new Wpf.Ui.Controls.MenuItem { Header = "Home", Tag = "tray_home" },
            new Wpf.Ui.Controls.MenuItem { Header = "Close", Tag = "tray_close" }
        };
}
