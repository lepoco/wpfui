// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Wpf.Ui.Tray;

/// <summary>
/// Base implementation of the notify icon service.
/// </summary>
public class NotifyIconService : INotifyIconService
{
    private readonly Internal.InternalNotifyIconManager internalNotifyIconManager;

    public Window ParentWindow { get; internal set; } = null!;

    public int Id => this.internalNotifyIconManager.Id;

    public bool IsRegistered => this.internalNotifyIconManager.IsRegistered;

    public string TooltipText
    {
        get => this.internalNotifyIconManager.TooltipText;
        set => this.internalNotifyIconManager.TooltipText = value;
    }

    public ContextMenu? ContextMenu
    {
        get => this.internalNotifyIconManager.ContextMenu;
        set => this.internalNotifyIconManager.ContextMenu = value;
    }

    public ImageSource? Icon
    {
        get => this.internalNotifyIconManager.Icon;
        set => this.internalNotifyIconManager.Icon = value;
    }

    public NotifyIconService()
    {
        this.internalNotifyIconManager = new Internal.InternalNotifyIconManager();

        RegisterHandlers();
    }

    public bool Register()
    {
        if (ParentWindow is not null)
        {
            return this.internalNotifyIconManager.Register(ParentWindow);
        }

        return this.internalNotifyIconManager.Register();
    }

    public bool Unregister()
    {
        return this.internalNotifyIconManager.Unregister();
    }

    /// <inheritdoc />
    public void SetParentWindow(Window parentWindow)
    {
        if (ParentWindow is not null)
        {
            ParentWindow.Closing -= OnParentWindowClosing;
        }

        ParentWindow = parentWindow;
        ParentWindow.Closing += OnParentWindowClosing;
    }

    /// <summary>
    /// This virtual method is called when the user clicks the left mouse button on the tray icon.
    /// </summary>
    protected virtual void OnLeftClick() { }

    /// <summary>
    /// This virtual method is called when the user double-clicks the left mouse button on the tray icon.
    /// </summary>
    protected virtual void OnLeftDoubleClick() { }

    /// <summary>
    /// This virtual method is called when the user clicks the right mouse button on the tray icon.
    /// </summary>
    protected virtual void OnRightClick() { }

    /// <summary>
    /// This virtual method is called when the user double-clicks the right mouse button on the tray icon.
    /// </summary>
    protected virtual void OnRightDoubleClick() { }

    /// <summary>
    /// This virtual method is called when the user clicks the middle mouse button on the tray icon.
    /// </summary>
    protected virtual void OnMiddleClick() { }

    /// <summary>
    /// This virtual method is called when the user double-clicks the middle mouse button on the tray icon.
    /// </summary>
    protected virtual void OnMiddleDoubleClick() { }

    private void OnParentWindowClosing(object? sender, CancelEventArgs e)
    {
        this.internalNotifyIconManager.Dispose();
    }

    private void RegisterHandlers()
    {
        this.internalNotifyIconManager.LeftClick += OnLeftClick;
        this.internalNotifyIconManager.LeftDoubleClick += OnLeftDoubleClick;
        this.internalNotifyIconManager.RightClick += OnRightClick;
        this.internalNotifyIconManager.RightDoubleClick += OnRightDoubleClick;
        this.internalNotifyIconManager.MiddleClick += OnMiddleClick;
        this.internalNotifyIconManager.MiddleDoubleClick += OnMiddleDoubleClick;
    }
}
