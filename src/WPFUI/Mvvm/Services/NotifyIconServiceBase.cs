// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using WPFUI.Mvvm.Contracts;
using WPFUI.Tray;

namespace WPFUI.Mvvm.Services;

/// <summary>
/// Base implementation of the notify icon service.
/// </summary>
public abstract class NotifyIconServiceBase : NotifyIconBase, INotifyIconService
{
    public Window ParentWindow { get; internal set; }

    /// <inheritdoc />
    public void SetParentWindow(Window parentWindow)
    {
        ParentWindow = parentWindow;
    }

    /// <inheritdoc />
    public void SetParentHandle(IntPtr parentHandle)
    {
        ParentHandle = parentHandle;
    }

    /// <inheritdoc />
    public IntPtr GetParentHandle()
    {
        return ParentHandle;
    }
}

