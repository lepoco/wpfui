// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Interop;
using Wpf.Ui.Interop;

namespace Wpf.Ui.TaskBar;

/// <summary>
/// Allows to change the status of the displayed notification in the application icon on the TaskBar.
/// </summary>
public static class TaskBarProgress
{
    /// <summary>
    /// Gets a value indicating whether the current operating system supports task bar manipulation.
    /// </summary>
    private static bool IsSupported()
    {
        return Win32.Utilities.IsOSWindows7OrNewer;
    }

    /// <summary>
    /// Allows to change the status of the progress bar in the task bar.
    /// </summary>
    /// <param name="window">Window to manipulate.</param>
    /// <param name="taskBarProgressState">State of the progress indicator.</param>
    public static bool SetState(Window window, TaskBarProgressState taskBarProgressState)
    {
        if (window == null)
            return false;

        if (window.IsLoaded)
            return SetState(new WindowInteropHelper(window).Handle, taskBarProgressState);

        window.Loaded += (_, _) =>
        {
            SetState(new WindowInteropHelper(window).Handle, taskBarProgressState);
        };

        return true;
    }

    /// <summary>
    /// Allows to change the status of the progress bar in the task bar.
    /// </summary>
    /// <param name="hWnd">Window handle.</param>
    /// <param name="taskBarProgressState">State of the progress indicator.</param>
    public static bool SetState(IntPtr hWnd, TaskBarProgressState taskBarProgressState)
    {
        if (!IsSupported())
            throw new Exception("Taskbar functions not available.");

        return UnsafeNativeMethods.SetTaskbarState(hWnd, UnsafeReflection.Cast(taskBarProgressState));
    }

    /// <summary>
    /// Allows to change the fill of the task bar.
    /// </summary>
    /// <param name="window">Window to manipulate.</param>
    /// <param name="taskBarProgressState">Progress sate to set.</param>
    /// <param name="current">Current value to display</param>
    public static bool SetValue(Window window, TaskBarProgressState taskBarProgressState, int current)
    {
        if (current > 100)
            current = 100;

        if (current < 0)
            current = 0;

        return SetValue(window, taskBarProgressState, current, 100);
    }

    /// <summary>
    /// Allows to change the fill of the task bar.
    /// </summary>
    /// <param name="window">Window to manipulate.</param>
    /// <param name="taskBarProgressState">Progress sate to set.</param>
    /// <param name="current">Current value to display</param>
    /// <param name="total">Total number for division.</param>
    public static bool SetValue(Window window, TaskBarProgressState taskBarProgressState, int current, int total)
    {
        if (window == null)
            return false;

        if (window.IsLoaded)
            return SetValue(new WindowInteropHelper(window).Handle, taskBarProgressState, current, total);

        window.Loaded += (_, _) =>
        {
            SetValue(new WindowInteropHelper(window).Handle, taskBarProgressState, current, total);
        };

        return false;
    }

    /// <summary>
    /// Allows to change the fill of the task bar.
    /// </summary>
    /// <param name="hWnd">Window handle.</param>
    /// <param name="taskBarProgressState">Progress sate to set.</param>
    /// <param name="current">Current value to display</param>
    public static bool SetValue(IntPtr hWnd, TaskBarProgressState taskBarProgressState, int current)
    {
        if (current > 100)
            current = 100;

        if (current < 0)
            current = 0;

        return SetValue(hWnd, taskBarProgressState, current, 100);
    }

    /// <summary>
    /// Allows to change the fill of the task bar.
    /// </summary>
    /// <param name="hWnd">Window handle.</param>
    /// <param name="taskBarProgressState">Progress sate to set.</param>
    /// <param name="current">Current value to display</param>
    /// <param name="total">Total number for division.</param>
    public static bool SetValue(IntPtr hWnd, TaskBarProgressState taskBarProgressState, int current, int total)
    {
        if (!IsSupported())
            throw new Exception("Taskbar functions not available.");

        return UnsafeNativeMethods.SetTaskbarValue(hWnd, UnsafeReflection.Cast(taskBarProgressState), current, total);
    }
}
