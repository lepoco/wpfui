// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Interop;
using WPFUI.Mvvm.Contracts;
using WPFUI.Taskbar;

namespace WPFUI.Mvvm.Services;

/// <summary>
/// Allows you to manage the animations of the window icon in the taskbar.
/// </summary>
public partial class TaskbarService : ITaskbarService
{
    private volatile Dictionary<IntPtr, TaskbarProgressState> _progressStates;

    /// <summary>
    /// Creates new instance and defines dictionary for progress states.
    /// </summary>
    public TaskbarService()
    {
        _progressStates = new Dictionary<IntPtr, TaskbarProgressState>();
    }

    /// <inheritdoc />
    public virtual TaskbarProgressState GetState(IntPtr hWnd)
    {
        if (!_progressStates.TryGetValue(hWnd, out var progressState))
            return TaskbarProgressState.None;

        return progressState;
    }

    /// <inheritdoc />
    public virtual TaskbarProgressState GetState(Window window)
    {
        if (window == null)
            return TaskbarProgressState.None;

        var windowHandle = new WindowInteropHelper(window).Handle;

        if (!_progressStates.TryGetValue(windowHandle, out var progressState))
            return TaskbarProgressState.None;

        return progressState;
    }

    /// <inheritdoc />
    public virtual bool SetState(Window window, TaskbarProgressState taskbarProgressState)
    {
        if (window == null)
            return false;

        return TaskbarProgress.SetState(window, taskbarProgressState);
    }

    /// <inheritdoc />
    public virtual bool SetValue(Window window, TaskbarProgressState taskbarProgressState, int current, int total)
    {
        if (window == null)
            return false;

        return TaskbarProgress.SetValue(window, taskbarProgressState, current, total);
    }

    /// <inheritdoc />
    public virtual bool SetValue(Window window, int current, int total)
    {
        if (window == null)
            return false;

        var windowHandle = new WindowInteropHelper(window).Handle;

        if (!_progressStates.TryGetValue(windowHandle, out var progressState))
            return TaskbarProgress.SetValue(window, TaskbarProgressState.Normal, current, total);

        return TaskbarProgress.SetValue(window, progressState, current, total);
    }

    /// <inheritdoc />
    public virtual bool SetState(IntPtr hWnd, TaskbarProgressState taskbarProgressState)
    {
        return TaskbarProgress.SetState(hWnd, taskbarProgressState);
    }

    public virtual bool SetValue(IntPtr hWnd, TaskbarProgressState taskbarProgressState, int current, int total)
    {
        return TaskbarProgress.SetValue(hWnd, taskbarProgressState, current, total);
    }

    /// <inheritdoc />
    public virtual bool SetValue(IntPtr hWnd, int current, int total)
    {
        if (!_progressStates.TryGetValue(hWnd, out var progressState))
            return TaskbarProgress.SetValue(hWnd, TaskbarProgressState.Normal, current, total);

        return TaskbarProgress.SetValue(hWnd, progressState, current, total);
    }
}
