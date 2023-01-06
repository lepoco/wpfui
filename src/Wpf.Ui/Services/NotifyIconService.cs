// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Wpf.Ui.Contracts;

namespace Wpf.Ui.Services;

/// <summary>
/// Base implementation of the notify icon service.
/// </summary>
public class NotifyIconService : INotifyIconService
{
    private readonly Internal.NotifyIconService _notifyIconService;

    public Window ParentWindow { get; internal set; } = null!;

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
        _notifyIconService = new Internal.NotifyIconService();

        RegisterHandlers();
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
        if (ParentWindow != null)
            ParentWindow.Closing -= OnParentWindowClosing;

        ParentWindow = parentWindow;
        ParentWindow.Closing += OnParentWindowClosing;
    }

    /// <summary>
    /// This virtual method is called when the user clicks the left mouse button on the tray icon.
    /// </summary>
    protected virtual void OnLeftClick()
    {
    }

    /// <summary>
    /// This virtual method is called when the user double-clicks the left mouse button on the tray icon.
    /// </summary>
    protected virtual void OnLeftDoubleClick()
    {
    }

    /// <summary>
    /// This virtual method is called when the user clicks the right mouse button on the tray icon.
    /// </summary>
    protected virtual void OnRightClick()
    {
    }

    /// <summary>
    /// This virtual method is called when the user double-clicks the right mouse button on the tray icon.
    /// </summary>
    protected virtual void OnRightDoubleClick()
    {
    }

    /// <summary>
    /// This virtual method is called when the user clicks the middle mouse button on the tray icon.
    /// </summary>
    protected virtual void OnMiddleClick()
    {
    }

    /// <summary>
    /// This virtual method is called when the user double-clicks the middle mouse button on the tray icon.
    /// </summary>
    protected virtual void OnMiddleDoubleClick()
    {
    }

    private void OnParentWindowClosing(object sender, CancelEventArgs e)
    {
        _notifyIconService.Dispose();
    }

    private void RegisterHandlers()
    {
        _notifyIconService.LeftClick += OnLeftClick;
        _notifyIconService.LeftDoubleClick += OnLeftDoubleClick;
        _notifyIconService.RightClick += OnRightClick;
        _notifyIconService.RightDoubleClick += OnRightDoubleClick;
        _notifyIconService.MiddleClick += OnMiddleClick;
        _notifyIconService.MiddleDoubleClick += OnMiddleDoubleClick;
    }
}
