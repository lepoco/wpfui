// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.TaskBar;

namespace Wpf.Ui;

/// <summary>
/// Allows you to manage the animations of the window icon in the taskbar.
/// </summary>
public partial class TaskBarService : ITaskBarService
{
    private readonly Dictionary<IntPtr, TaskBarProgressState> _progressStates = new();

    /// <inheritdoc />
    public virtual TaskBarProgressState GetState(IntPtr hWnd)
    {
        if (!_progressStates.TryGetValue(hWnd, out TaskBarProgressState progressState))
        {
            return TaskBarProgressState.None;
        }

        return progressState;
    }

    /// <inheritdoc />
    public virtual TaskBarProgressState GetState(Window? window)
    {
        if (window is null)
        {
            return TaskBarProgressState.None;
        }

        IntPtr windowHandle = new WindowInteropHelper(window).Handle;

        if (!_progressStates.TryGetValue(windowHandle, out TaskBarProgressState progressState))
        {
            return TaskBarProgressState.None;
        }

        return progressState;
    }

    /// <inheritdoc />
    public virtual bool SetState(Window? window, TaskBarProgressState taskBarProgressState)
    {
        if (window is null)
        {
            return false;
        }

        return TaskBarProgress.SetState(window, taskBarProgressState);
    }

    /// <inheritdoc />
    public virtual bool SetValue(
        Window? window,
        TaskBarProgressState taskBarProgressState,
        int current,
        int total
    )
    {
        if (window is null)
        {
            return false;
        }

        return TaskBarProgress.SetValue(window, taskBarProgressState, current, total);
    }

    /// <inheritdoc />
    public virtual bool SetValue(Window? window, int current, int total)
    {
        if (window == null)
        {
            return false;
        }

        IntPtr windowHandle = new WindowInteropHelper(window).Handle;

        if (!_progressStates.TryGetValue(windowHandle, out TaskBarProgressState progressState))
        {
            return TaskBarProgress.SetValue(window, TaskBarProgressState.Normal, current, total);
        }

        return TaskBarProgress.SetValue(window, progressState, current, total);
    }

    /// <inheritdoc />
    public virtual bool SetState(IntPtr hWnd, TaskBarProgressState taskBarProgressState)
    {
        return TaskBarProgress.SetState(hWnd, taskBarProgressState);
    }

    /// <inheritdoc/>
    public virtual bool SetValue(
        IntPtr hWnd,
        TaskBarProgressState taskBarProgressState,
        int current,
        int total
    )
    {
        return TaskBarProgress.SetValue(hWnd, taskBarProgressState, current, total);
    }

    /// <inheritdoc />
    public virtual bool SetValue(IntPtr hWnd, int current, int total)
    {
        if (!_progressStates.TryGetValue(hWnd, out TaskBarProgressState progressState))
        {
            return TaskBarProgress.SetValue(hWnd, TaskBarProgressState.Normal, current, total);
        }

        return TaskBarProgress.SetValue(hWnd, progressState, current, total);
    }
}
