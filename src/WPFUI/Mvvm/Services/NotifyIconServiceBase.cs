// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using WPFUI.Mvvm.Contracts;
using WPFUI.Tray;

namespace WPFUI.Mvvm.Services;

/// <summary>
/// Base implementation of the notify icon service.
/// </summary>
public abstract class NotifyIconServiceBase : INotifyIconService, IDisposable
{
    /// <summary>
    /// Provides a set of information for Shell32 to manipulate the icon.
    /// </summary>
    internal Interop.Shell32.NOTIFYICONDATA ShellIconData { get; set; }

    /// <summary>
    /// Gets or sets the hWnd that will receive messages for the icon.
    /// </summary>
    internal HwndSource HookWindow { get; set; }

    /// <summary>
    /// Gets or sets the hWnd that the icon belongs to.
    /// </summary>
    internal IntPtr ParentHandle { get; set; }

    /// <summary>
    /// Whether the control is disposed.
    /// </summary>
    protected bool Disposed = false;

    /// <inheritdoc />
    public int Id { get; internal set; }

    /// <inheritdoc />
    public bool IsRegistered { get; set; }

    /// <inheritdoc />
    public string TooltipText { get; set; }

    /// <inheritdoc />
    public ContextMenu ContextMenu { get; set; }

    /// <inheritdoc />
    public ImageSource Icon { get; set; }

    /// <summary>
    /// Class destructor.
    /// </summary>
    ~NotifyIconServiceBase()
    {
        Dispose(false);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);

        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// If disposing equals <see langword="true"/>, the method has been called directly or indirectly
    /// by a user's code. Managed and unmanaged resources can be disposed. If disposing equals <see langword="false"/>,
    /// the method has been called by the runtime from inside the finalizer and you should not
    /// reference other objects.
    /// <para>Only unmanaged resources can be disposed.</para>
    /// </summary>
    /// <param name="disposing">If disposing equals <see langword="true"/>, dispose all managed and unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (Disposed)
            return;

        Disposed = true;

        if (!disposing)
            return;

#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO | {typeof(NotifyIconServiceBase)} disposed.", "WPFUI.NotifyIconService");
#endif

        Unregister();
    }

    /// <inheritdoc />
    public bool Register(IntPtr parentHandle)
    {
        ParentHandle = parentHandle;

        return TrayManager.Register(this);
    }

    /// <inheritdoc />
    public bool Unregister()
    {
        return false;
    }

    /// <summary>
    /// A callback function that processes messages sent to a window.
    /// The WNDPROC type defines a pointer to this callback function.
    /// </summary>
    internal IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        return IntPtr.Zero;
    }
}

