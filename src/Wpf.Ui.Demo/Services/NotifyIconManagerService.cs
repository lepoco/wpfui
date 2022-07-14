// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Wpf.Ui.Common;
using Wpf.Ui.Mvvm.Contracts;

namespace Wpf.Ui.Demo.Services;

public class NotifyIconManagerService
{
    private readonly INotifyIconService _iconService;

    public bool IsRegistered => _iconService.IsRegistered;

    public NotifyIconManagerService(INotifyIconService iconService)
    {
        _iconService = iconService;

        _iconService.TooltipText = "WPF UI - Service Icon";
        _iconService.Icon = GetImage("pack://application:,,,/Resources/wpfui.png");
        _iconService.ContextMenu = new ContextMenu
        {
            Items =
            {
                new Wpf.Ui.Controls.MenuItem
                {
                    Header = "Home",
                    SymbolIcon = SymbolRegular.Library28 },
                new Wpf.Ui.Controls.MenuItem
                {
                    Header = "Save",
                    SymbolIcon = SymbolRegular.Save28
                },
                new Wpf.Ui.Controls.MenuItem
                {
                    Header = "Open",
                    SymbolIcon = SymbolRegular.Folder28
                },
                new Separator(),
                new Wpf.Ui.Controls.MenuItem
                {
                    Header = "Reload",
                    SymbolIcon = SymbolRegular.ArrowClockwise28
                },
            }
        };

        foreach (var singleContextMenuItem in _iconService.ContextMenu.Items)
            if (singleContextMenuItem is MenuItem)
                (singleContextMenuItem as MenuItem).Click += OnMenuItemClick;
    }
    public bool Register()
        => _iconService.Register();

    public void SetParentWindow(Window window)
        => _iconService.SetParentWindow(window);

    private ImageSource GetImage(string absolutePath)
    {
        var bitmap = new BitmapImage();

        bitmap.BeginInit();
        bitmap.UriSource = new Uri(absolutePath, UriKind.Absolute);
        bitmap.EndInit();

        return bitmap;
    }

    private void OnMenuItemClick(object sender, RoutedEventArgs e)
    {
        if (sender is not MenuItem menuItem)
            return;

        System.Diagnostics.Debug.WriteLine($"DEBUG | WPF UI Tray clicked: {menuItem.Tag}", "Wpf.Ui.Demo");
    }
}
