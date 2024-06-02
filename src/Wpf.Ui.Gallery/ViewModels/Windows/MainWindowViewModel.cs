// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
using Wpf.Ui.Gallery.Views.Pages.OpSystem;
using Wpf.Ui.Gallery.Views.Pages.StatusAndInfo;
using Wpf.Ui.Gallery.Views.Pages.Text;
using Wpf.Ui.Gallery.Views.Pages.Windows;

namespace Wpf.Ui.Gallery.ViewModels.Windows;

public partial class MainWindowViewModel : ViewModel
{
    [ObservableProperty]
    private string _applicationTitle = "WPF UI Gallery";

    [ObservableProperty]
    private ObservableCollection<object> _menuItems =
    [
        new NavigationViewItem("Home", SymbolRegular.Home24, typeof(DashboardPage)),
        new NavigationViewItem()
        {
            Content = "Design guidance",
            Icon = new SymbolIcon { Symbol = SymbolRegular.DesignIdeas24 },
            MenuItemsSource = new object[]
            {
                new NavigationViewItem("Typography", SymbolRegular.TextFont24, typeof(TypographyPage)),
                new NavigationViewItem("Icons", SymbolRegular.Diversity24, typeof(IconsPage)),
                new NavigationViewItem("Colors", SymbolRegular.Color24, typeof(ColorsPage))
            }
        },
        new NavigationViewItem("All samples", SymbolRegular.List24, typeof(AllControlsPage)),
        new NavigationViewItemSeparator(),
        new NavigationViewItem("Basic Input", SymbolRegular.CheckboxChecked24, typeof(BasicInputPage))
        {
            MenuItemsSource = new object[]
            {
                new NavigationViewItem(nameof(Anchor), typeof(AnchorPage)),
                new NavigationViewItem(nameof(Wpf.Ui.Controls.Button), typeof(ButtonPage)),
                new NavigationViewItem(nameof(DropDownButton), typeof(DropDownButtonPage)),
                new NavigationViewItem(nameof(HyperlinkButton), typeof(HyperlinkButtonPage)),
                new NavigationViewItem(nameof(ToggleButton), typeof(ToggleButtonPage)),
                new NavigationViewItem(nameof(ToggleSwitch), typeof(ToggleSwitchPage)),
                new NavigationViewItem(nameof(CheckBox), typeof(CheckBoxPage)),
                new NavigationViewItem(nameof(ComboBox), typeof(ComboBoxPage)),
                new NavigationViewItem(nameof(RadioButton), typeof(RadioButtonPage)),
                new NavigationViewItem(nameof(RatingControl), typeof(RatingPage)),
                new NavigationViewItem(nameof(ThumbRate), typeof(ThumbRatePage)),
                new NavigationViewItem(nameof(SplitButton), typeof(SplitButtonPage)),
                new NavigationViewItem(nameof(Slider), typeof(SliderPage)),
            }
        },
        new NavigationViewItem
        {
            Content = "Collections",
            Icon = new SymbolIcon { Symbol = SymbolRegular.Table24 },
            TargetPageType = typeof(CollectionsPage),
            MenuItemsSource = new object[]
            {
                new NavigationViewItem(nameof(System.Windows.Controls.DataGrid), typeof(DataGridPage)),
                new NavigationViewItem(nameof(ListBox), typeof(ListBoxPage)),
                new NavigationViewItem(nameof(Ui.Controls.ListView), typeof(ListViewPage)),
                new NavigationViewItem(nameof(TreeView), typeof(TreeViewPage)),
#if DEBUG
                new NavigationViewItem("TreeList", typeof(TreeListPage)),
#endif
            }
        },
        new NavigationViewItem("Date & time", SymbolRegular.CalendarClock24, typeof(DateAndTimePage))
        {
            MenuItemsSource = new object[]
            {
                new NavigationViewItem(nameof(CalendarDatePicker), typeof(CalendarDatePickerPage)),
                new NavigationViewItem(nameof(System.Windows.Controls.Calendar), typeof(CalendarPage)),
                new NavigationViewItem(nameof(DatePicker), typeof(DatePickerPage)),
                new NavigationViewItem(nameof(TimePicker), typeof(TimePickerPage))
            }
        },
        new NavigationViewItem("Dialogs & flyouts", SymbolRegular.Chat24, typeof(DialogsAndFlyoutsPage))
        {
            MenuItemsSource = new object[]
            {
                new NavigationViewItem(nameof(Snackbar), typeof(SnackbarPage)),
                new NavigationViewItem(nameof(ContentDialog), typeof(ContentDialogPage)),
                new NavigationViewItem(nameof(Flyout), typeof(FlyoutPage)),
                new NavigationViewItem(nameof(Wpf.Ui.Controls.MessageBox), typeof(MessageBoxPage)),
            }
        },
#if DEBUG
        new NavigationViewItem("Layout", SymbolRegular.News24, typeof(LayoutPage))
        {
            MenuItemsSource = new object[]
            {
                new NavigationViewItem("Expander", typeof(ExpanderPage)),
                new NavigationViewItem("CardControl", typeof(CardControlPage)),
                new NavigationViewItem("CardAction", typeof(CardActionPage))
            },
        },
#endif
        new NavigationViewItem
        {
            Content = "Media",
            Icon = new SymbolIcon { Symbol = SymbolRegular.PlayCircle24 },
            TargetPageType = typeof(MediaPage),
            MenuItemsSource = new object[]
            {
                new NavigationViewItem("Image", typeof(ImagePage)),
                new NavigationViewItem("Canvas", typeof(CanvasPage)),
                new NavigationViewItem("WebView", typeof(WebViewPage)),
                new NavigationViewItem("WebBrowser", typeof(WebBrowserPage))
            }
        },
        new NavigationViewItem("Navigation", SymbolRegular.Navigation24, typeof(NavigationPage))
        {
            MenuItemsSource = new object[]
            {
                new NavigationViewItem("BreadcrumbBar", typeof(BreadcrumbBarPage)),
                new NavigationViewItem("NavigationView", typeof(NavigationViewPage)),
                new NavigationViewItem("Menu", typeof(MenuPage)),
                new NavigationViewItem("Multilevel navigation", typeof(MultilevelNavigationPage)),
                new NavigationViewItem("TabControl", typeof(TabControlPage))
            }
        },
        new NavigationViewItem(
            "Status & info",
            SymbolRegular.ChatBubblesQuestion24,
            typeof(StatusAndInfoPage)
        )
        {
            MenuItemsSource = new object[]
            {
                new NavigationViewItem("InfoBadge", typeof(InfoBadgePage)),
                new NavigationViewItem("InfoBar", typeof(InfoBarPage)),
                new NavigationViewItem("ProgressBar", typeof(ProgressBarPage)),
                new NavigationViewItem("ProgressRing", typeof(ProgressRingPage)),
                new NavigationViewItem("ToolTip", typeof(ToolTipPage))
            }
        },
        new NavigationViewItem("Text", SymbolRegular.DrawText24, typeof(TextPage))
        {
            MenuItemsSource = new object[]
            {
                new NavigationViewItem(nameof(AutoSuggestBox), typeof(AutoSuggestBoxPage)),
                new NavigationViewItem(nameof(NumberBox), typeof(NumberBoxPage)),
                new NavigationViewItem(nameof(Wpf.Ui.Controls.PasswordBox), typeof(PasswordBoxPage)),
                new NavigationViewItem(nameof(Wpf.Ui.Controls.RichTextBox), typeof(RichTextBoxPage)),
                new NavigationViewItem(nameof(Label), typeof(LabelPage)),
                new NavigationViewItem(nameof(Wpf.Ui.Controls.TextBlock), typeof(TextBlockPage)),
                new NavigationViewItem(nameof(Wpf.Ui.Controls.TextBox), typeof(TextBoxPage)),
            }
        },
        new NavigationViewItem("System", SymbolRegular.Desktop24, typeof(OpSystemPage))
        {
            MenuItemsSource = new object[]
            {
                new NavigationViewItem("Clipboard", typeof(ClipboardPage)),
                new NavigationViewItem("FilePicker", typeof(FilePickerPage)),
            }
        },
        new NavigationViewItem("Windows", SymbolRegular.WindowApps24, typeof(WindowsPage))
    ];

    [ObservableProperty]
    private ObservableCollection<object> _footerMenuItems =
    [
        new NavigationViewItem("Settings", SymbolRegular.Settings24, typeof(SettingsPage))
    ];

    [ObservableProperty]
    private ObservableCollection<Wpf.Ui.Controls.MenuItem> _trayMenuItems =
    [
        new Wpf.Ui.Controls.MenuItem { Header = "Home", Tag = "tray_home" },
        new Wpf.Ui.Controls.MenuItem { Header = "Close", Tag = "tray_close" }
    ];
}
