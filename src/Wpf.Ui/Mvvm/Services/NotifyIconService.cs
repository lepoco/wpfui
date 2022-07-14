// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Wpf.Ui.Mvvm.Contracts;

namespace Wpf.Ui.Mvvm.Services;

/// <summary>
/// Base implementation of the notify icon service.
/// </summary>
public class NotifyIconService : INotifyIconService
{
    private readonly Wpf.Ui.Services.Internal.NotifyIconService _notifyIconService;

    public Window ParentWindow { get; internal set; } = (Window)null!;

    public int Id => _notifyIconService.Id;

    public bool IsRegistered => _notifyIconService.IsRegistered;

    public string TooltipText
    {
        get => _notifyIconService.TooltipText;
        set => _notifyIconService.TooltipText = value;
    }

    public ContextMenu ContextMenu
    {
        get => _notifyIconService.ContextMenu;
        set => _notifyIconService.ContextMenu = value;
    }

    public ImageSource Icon
    {
        get => _notifyIconService.Icon;
        set => _notifyIconService.Icon = value;
    }

    public NotifyIconService()
    {
        _notifyIconService = new Ui.Services.Internal.NotifyIconService();
    }

    public bool Register()
    {
        if (ParentWindow != null)
            return _notifyIconService.Register(ParentWindow);

        return _notifyIconService.Register();
    }

    public bool Unregister()
    {
        return _notifyIconService.Unregister();
    }

    /// <inheritdoc />
    public void SetParentWindow(Window parentWindow)
    {
        ParentWindow = parentWindow;
    }
}

