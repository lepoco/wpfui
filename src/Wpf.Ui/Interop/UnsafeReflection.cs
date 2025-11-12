// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

/* This Source Code is partially based on reverse engineering of the Windows Operating System,
   and is intended for use on Windows systems only.
   This Source Code is partially based on the source code provided by the .NET Foundation. */

using Windows.Win32.Graphics.Dwm;
using Windows.Win32.UI.Shell;
using Wpf.Ui.Controls;
using Wpf.Ui.TaskBar;

namespace Wpf.Ui.Interop;

/// <summary>
/// A set of dangerous methods to modify the appearance.
/// </summary>
internal static class UnsafeReflection
{
    /// <summary>
    /// Casts <see cref="WindowCornerPreference" /> to <see cref="DWM_WINDOW_CORNER_PREFERENCE" />.
    /// </summary>
    public static DWM_WINDOW_CORNER_PREFERENCE Cast(WindowCornerPreference cornerPreference)
    {
        return cornerPreference switch
        {
            WindowCornerPreference.Round => DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_ROUND,
            WindowCornerPreference.RoundSmall => DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_ROUNDSMALL,
            WindowCornerPreference.DoNotRound => DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_DONOTROUND,
            _ => DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_DEFAULT,
        };
    }

    /// <summary>
    /// Casts <see cref="TaskBarProgressState" /> to <see cref="TBPFLAG" />.
    /// </summary>
    public static TBPFLAG Cast(TaskBarProgressState taskBarProgressState)
    {
        return taskBarProgressState switch
        {
            TaskBarProgressState.Indeterminate => TBPFLAG.TBPF_INDETERMINATE,
            TaskBarProgressState.Error => TBPFLAG.TBPF_ERROR,
            TaskBarProgressState.Paused => TBPFLAG.TBPF_PAUSED,
            TaskBarProgressState.Normal => TBPFLAG.TBPF_NORMAL,
            _ => TBPFLAG.TBPF_NOPROGRESS,
        };
    }
}
