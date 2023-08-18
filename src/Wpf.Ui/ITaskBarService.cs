// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.TaskBar;

namespace Wpf.Ui;

/// <summary>
/// Represents a contract with a service that provides methods for manipulating the taskbar.
/// </summary>
public interface ITaskBarService
{
    /// <summary>
    /// Gets taskbar state of the selected window handle.
    /// </summary>
    /// <param name="hWnd">Window handle.</param>
    /// <returns>The current state of system TaskBar.</returns>
    TaskBarProgressState GetState(IntPtr hWnd);

    /// <summary>
    /// Gets taskbar state of the selected window.
    /// </summary>
    /// <param name="window">Selected window.</param>
    /// <returns>The current state of system TaskBar.</returns>
    TaskBarProgressState GetState(Window? window);

    /// <summary>
    /// Sets taskbar state of the selected window handle.
    /// </summary>
    /// <param name="hWnd">Window handle to modify.</param>
    /// <param name="taskBarProgressState">Progress sate to set.</param>
    /// <returns><see langword="true"/> if the operation succeeds. <see langword="false"/> otherwise.</returns>
    bool SetState(IntPtr hWnd, TaskBarProgressState taskBarProgressState);

    /// <summary>
    /// Sets taskbar value of the selected window handle.
    /// </summary>
    /// <param name="hWnd">Window handle to modify.</param>
    /// <param name="taskBarProgressState">Progress sate to set.</param>
    /// <param name="current">Current value to display.</param>
    /// <param name="total">Maximum number for division.</param>
    /// <returns><see langword="true"/> if the operation succeeds. <see langword="false"/> otherwise.</returns>
    bool SetValue(IntPtr hWnd, TaskBarProgressState taskBarProgressState, int current, int total);

    /// <summary>
    /// Sets taskbar value of the selected window handle.
    /// </summary>
    /// <param name="hWnd">Window handle to modify.</param>
    /// <param name="current">Current value to display.</param>
    /// <param name="max">Maximum number for division.</param>
    /// <returns><see langword="true"/> if the operation succeeds. <see langword="false"/> otherwise.</returns>
    bool SetValue(IntPtr hWnd, int current, int max);

    /// <summary>
    /// Sets taskbar state of the selected window.
    /// </summary>
    /// <param name="window">Window to modify.</param>
    /// <param name="taskBarProgressState">Progress sate to set.</param>
    /// <returns><see langword="true"/> if the operation succeeds. <see langword="false"/> otherwise.</returns>
    bool SetState(Window? window, TaskBarProgressState taskBarProgressState);

    /// <summary>
    /// Sets taskbar value of the selected window.
    /// </summary>
    /// <param name="window">Window to modify.</param>
    /// <param name="taskBarProgressState">Progress sate to set.</param>
    /// <param name="current">Current value to display.</param>
    /// <param name="total">Maximum number for division.</param>
    /// <returns><see langword="true"/> if the operation succeeds. <see langword="false"/> otherwise.</returns>
    bool SetValue(
        Window? window,
        TaskBarProgressState taskBarProgressState,
        int current,
        int total
    );

    /// <summary>
    /// Sets taskbar value of the selected window.
    /// </summary>
    /// <param name="window">Window to modify.</param>
    /// <param name="current">Current value to display.</param>
    /// <param name="total">Maximum number for division.</param>
    /// <returns><see langword="true"/> if the operation succeeds. <see langword="false"/> otherwise.</returns>
    bool SetValue(Window? window, int current, int total);
}
