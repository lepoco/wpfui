// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Wpf.Ui.Common;
using Wpf.Ui.Mvvm.Services;

namespace Wpf.Ui.Demo.Services;

public class CustomNotifyIconService : NotifyIconService
{
    public CustomNotifyIconService()
    {
        TooltipText = "WPF UI - Service Icon";

        // If this icon is not defined, the application icon will be used.
        Icon = BitmapFrame.Create(new Uri("pack://application:,,,/Resources/wpfui.png", UriKind.Absolute));

        ContextMenu = new ContextMenu
        {
            FontSize = 14d,
            Items =
            {
                new Wpf.Ui.Controls.MenuItem
                {
                    Header = "Home",
                    SymbolIcon = SymbolRegular.Library28,
                    Tag = "home"
                },
                new Wpf.Ui.Controls.MenuItem
                {
                    Header = "Save",
                    SymbolIcon = SymbolRegular.Save28,
                    Tag = "save"
                },
                new Wpf.Ui.Controls.MenuItem
                {
                    Header = "Open",
                    SymbolIcon = SymbolRegular.Folder28,
                    Tag = "open"
                },
                new Separator(),
                new Wpf.Ui.Controls.MenuItem
                {
                    Header = "Reload",
                    SymbolIcon = SymbolRegular.ArrowClockwise28,
                    Tag = "reload"
                },
            }
        };

        foreach (var singleContextMenuItem in ContextMenu.Items)
            if (singleContextMenuItem is MenuItem)
                ((MenuItem)singleContextMenuItem).Click += OnMenuItemClick;
    }

    protected override void OnLeftClick()
    {
        System.Diagnostics.Debug.WriteLine($"DEBUG | WPF UI Tray event: {nameof(OnLeftClick)}", "Wpf.Ui.Demo");
    }

    private void OnMenuItemClick(object sender, RoutedEventArgs e)
    {
        if (sender is not MenuItem menuItem)
            return;

        System.Diagnostics.Debug.WriteLine($"DEBUG | WPF UI Tray clicked: {menuItem.Tag}", "Wpf.Ui.Demo");
    }
}
