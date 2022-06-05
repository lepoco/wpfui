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
    private volatile Dictionary<IntPtr, ProgressState> _progressStates;

    /// <summary>
    /// Creates new instance and defines dictionary for progress states.
    /// </summary>
    public TaskbarService()
    {
        _progressStates = new Dictionary<IntPtr, ProgressState>();
    }

    /// <inheritdoc />
    public virtual ProgressState GetState(IntPtr hWnd)
    {
        if (!_progressStates.TryGetValue(hWnd, out var progressState))
            return ProgressState.None;

        return progressState;
    }

    /// <inheritdoc />
    public virtual ProgressState GetState(Window window)
    {
        if (window == null)
            return ProgressState.None;

        var windowHandle = new WindowInteropHelper(window).Handle;

        if (!_progressStates.TryGetValue(windowHandle, out var progressState))
            return ProgressState.None;

        return progressState;
    }

    /// <inheritdoc />
    public virtual bool SetState(Window window, ProgressState progressState)
    {
        if (window == null)
            return false;

        return TaskbarProgress.SetState(window, progressState);
    }

    /// <inheritdoc />
    public virtual bool SetValue(Window window, ProgressState progressState, int current, int total)
    {
        if (window == null)
            return false;

        return TaskbarProgress.SetValue(window, progressState, current, total);
    }

    /// <inheritdoc />
    public virtual bool SetValue(Window window, int current, int total)
    {
        if (window == null)
            return false;

        var windowHandle = new WindowInteropHelper(window).Handle;

        if (!_progressStates.TryGetValue(windowHandle, out var progressState))
            return TaskbarProgress.SetValue(window, ProgressState.Normal, current, total);

        return TaskbarProgress.SetValue(window, progressState, current, total);
    }

    /// <inheritdoc />
    public virtual bool SetState(IntPtr hWnd, ProgressState progressState)
    {
        return TaskbarProgress.SetState(hWnd, progressState);
    }

    public virtual bool SetValue(IntPtr hWnd, ProgressState progressState, int current, int total)
    {
        return TaskbarProgress.SetValue(hWnd, progressState, current, total);
    }

    /// <inheritdoc />
    public virtual bool SetValue(IntPtr hWnd, int current, int total)
    {
        if (!_progressStates.TryGetValue(hWnd, out var progressState))
            return TaskbarProgress.SetValue(hWnd, ProgressState.Normal, current, total);

        return TaskbarProgress.SetValue(hWnd, progressState, current, total);
    }
}
