// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Wpf.Ui.Common;
using Wpf.Ui.Mvvm.Services;

namespace Wpf.Ui.Demo.Services;

public class NotifyIconService : NotifyIconServiceBase
{
    public override bool Register()
    {
        if (IsRegistered)
            return false;

        InitializeContent();

        if (ParentWindow != null)
        {
            ParentHandle = new WindowInteropHelper(ParentWindow).Handle;

            base.Register();
        }

        if (ParentHandle == IntPtr.Zero)
            return false;

        return base.Register();
    }

    private void InitializeContent()
    {
        TooltipText = "WPF UI - Service Icon";
        Icon = GetImage("pack://application:,,,/Resources/wpfui.png");

        ContextMenu = new ContextMenu
        {
            Items =
            {
                new Wpf.Ui.Controls.MenuItem
                {
                    Header = "Home",
                    SymbolIcon = SymbolRegular.Library28
                },
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

        foreach (var singleContextMenuItem in ContextMenu.Items)
            if (singleContextMenuItem is MenuItem)
                (singleContextMenuItem as MenuItem).Click += OnMenuItemClick;
    }

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
