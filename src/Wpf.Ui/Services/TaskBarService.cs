// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Interop;
using Wpf.Ui.Contracts;
using Wpf.Ui.TaskBar;

namespace Wpf.Ui.Services;

/// <summary>
/// Allows you to manage the animations of the window icon in the taskbar.
/// </summary>
public partial class TaskBarService : ITaskBarService
{
    private volatile Dictionary<IntPtr, TaskBarProgressState> _progressStates;

    /// <summary>
    /// Creates new instance and defines dictionary for progress states.
    /// </summary>
    public TaskBarService()
    {
        _progressStates = new Dictionary<IntPtr, TaskBarProgressState>();
    }

    /// <inheritdoc />
    public virtual TaskBarProgressState GetState(IntPtr hWnd)
    {
        if (!_progressStates.TryGetValue(hWnd, out var progressState))
            return TaskBarProgressState.None;

        return progressState;
    }

    /// <inheritdoc />
    public virtual TaskBarProgressState GetState(Window window)
    {
        if (window == null)
            return TaskBarProgressState.None;

        var windowHandle = new WindowInteropHelper(window).Handle;

        if (!_progressStates.TryGetValue(windowHandle, out var progressState))
            return TaskBarProgressState.None;

        return progressState;
    }

    /// <inheritdoc />
    public virtual bool SetState(Window window, TaskBarProgressState taskBarProgressState)
    {
        if (window == null)
            return false;

        return TaskBarProgress.SetState(window, taskBarProgressState);
    }

    /// <inheritdoc />
    public virtual bool SetValue(Window window, TaskBarProgressState taskBarProgressState, int current, int total)
    {
        if (window == null)
            return false;

        return TaskBarProgress.SetValue(window, taskBarProgressState, current, total);
    }

    /// <inheritdoc />
    public virtual bool SetValue(Window window, int current, int total)
    {
        if (window == null)
            return false;

        var windowHandle = new WindowInteropHelper(window).Handle;

        if (!_progressStates.TryGetValue(windowHandle, out var progressState))
            return TaskBarProgress.SetValue(window, TaskBarProgressState.Normal, current, total);

        return TaskBarProgress.SetValue(window, progressState, current, total);
    }

    /// <inheritdoc />
    public virtual bool SetState(IntPtr hWnd, TaskBarProgressState taskBarProgressState)
    {
        return TaskBarProgress.SetState(hWnd, taskBarProgressState);
    }

    public virtual bool SetValue(IntPtr hWnd, TaskBarProgressState taskBarProgressState, int current, int total)
    {
        return TaskBarProgress.SetValue(hWnd, taskBarProgressState, current, total);
    }

    /// <inheritdoc />
    public virtual bool SetValue(IntPtr hWnd, int current, int total)
    {
        if (!_progressStates.TryGetValue(hWnd, out var progressState))
            return TaskBarProgress.SetValue(hWnd, TaskBarProgressState.Normal, current, total);

        return TaskBarProgress.SetValue(hWnd, progressState, current, total);
    }
}
