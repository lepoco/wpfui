// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;

namespace WPFUI.Taskbar;

/// <summary>
/// Allows to change the status of the displayed notification in the application icon on the TaskBar.
/// <para>
/// Based on the work of <see href="https://wpf.2000things.com/2014/03/19/1032-show-progress-on-windows-taskbar-icon/">Sean Sexton</see>.
/// </para>
/// </summary>
public static class Progress
{
    private static Interop.ShObjIdl.ITaskbarList4 _taskbarList;

    static Progress()
    {
        if (!IsSupported())
            throw new Exception("Taskbar functions not available");

        _taskbarList = new Interop.ShObjIdl.CTaskbarList() as Interop.ShObjIdl.ITaskbarList4;

        _taskbarList?.HrInit();
    }

    private static bool IsSupported()
    {
        return Win32.Utilities.IsOSWindows7OrNewer;
    }

    /// <summary>
    /// Allows to change the status of the progress bar in the task bar.
    /// </summary>
    /// <param name="state">State of the progress indicator.</param>
    /// <param name="dispatchInvoke">Run with the main <see cref="Application"/> thread.</param>
    public static void SetState(ProgressState state, bool dispatchInvoke = false)
    {
        if (!dispatchInvoke)
        {
            SetProgressState(state);

            return;
        }

        Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
        {
            SetProgressState(state);
        }));
    }

    /// <summary>
    /// Allows to change the fill of the task bar.
    /// </summary>
    /// <param name="current">Current value to display</param>
    /// <param name="max">Maximum number for division.</param>
    /// <param name="dispatchInvoke">Run with the main <see cref="Application"/> thread.</param>
    public static void SetValue(int current, int max, bool dispatchInvoke = false)
    {
        if (!dispatchInvoke)
        {
            SetProgressValue(current, max);

            return;
        }

        // using System.Windows.Interop
        Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
        {
            SetProgressValue(current, max);
        }));
    }

    private static void SetProgressState(ProgressState state)
    {
        // Application.Current.MainWindow.TaskbarItemInfo.ProgressState = (System.Windows.Shell.TaskbarItemProgressState) state;
        //_taskbarList.SetProgressState(GetHandle(), state);
    }

    private static void SetProgressValue(int current, int max)
    {
        _taskbarList.SetProgressValue(
                 GetHandle(),
                 Convert.ToUInt64(current),
                 Convert.ToUInt64(max));
    }

    private static IntPtr GetHandle()
    {
        if (Application.Current.MainWindow != null)
            return new WindowInteropHelper(Application.Current.MainWindow).Handle;

        return IntPtr.Zero;
    }
}
