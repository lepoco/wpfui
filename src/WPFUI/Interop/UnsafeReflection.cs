// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using WPFUI.Appearance;
using WPFUI.Taskbar;

namespace WPFUI.Interop;

/// <summary>
/// A set of dangerous methods to modify the appearance.
/// </summary>
[Obsolete("This class is not depracted, but is dangerous to use.")]
internal static class UnsafeReflection
{
    /// <summary>
    /// Casts <see cref="BackgroundType"/> to <see cref="Dwmapi.DWMSBT"/>.
    /// </summary>
    public static Dwmapi.DWMSBT Cast(BackgroundType backgroundType)
    {
        return backgroundType switch
        {
            BackgroundType.Auto => Dwmapi.DWMSBT.DWMSBT_AUTO,
            BackgroundType.Mica => Dwmapi.DWMSBT.DWMSBT_MAINWINDOW,
            BackgroundType.Acrylic => Dwmapi.DWMSBT.DWMSBT_TRANSIENTWINDOW,
            BackgroundType.Tabbed => Dwmapi.DWMSBT.DWMSBT_TABBEDWINDOW,
            _ => Dwmapi.DWMSBT.DWMSBT_DISABLE
        };
    }

    /// <summary>
    /// Casts <see cref="Dwmapi.DWMSBT"/> to <see cref="BackgroundType"/>.
    /// </summary>
    public static BackgroundType Cast(Dwmapi.DWMSBT backgroundType)
    {
        return backgroundType switch
        {
            Dwmapi.DWMSBT.DWMSBT_AUTO => BackgroundType.Auto,
            Dwmapi.DWMSBT.DWMSBT_MAINWINDOW => BackgroundType.Mica,
            Dwmapi.DWMSBT.DWMSBT_TRANSIENTWINDOW => BackgroundType.Acrylic,
            Dwmapi.DWMSBT.DWMSBT_TABBEDWINDOW => BackgroundType.Tabbed,
            _ => BackgroundType.Unknown
        };
    }

    /// <summary>
    /// Casts <see cref="WindowCornerPreference"/> to <see cref="Dwmapi.DWM_WINDOW_CORNER_PREFERENCE"/>.
    /// </summary>
    public static Dwmapi.DWM_WINDOW_CORNER_PREFERENCE Cast(WindowCornerPreference cornerPreference)
    {
        return cornerPreference switch
        {
            WindowCornerPreference.Round => Dwmapi.DWM_WINDOW_CORNER_PREFERENCE.ROUND,
            WindowCornerPreference.RoundSmall => Dwmapi.DWM_WINDOW_CORNER_PREFERENCE.ROUNDSMALL,
            WindowCornerPreference.DoNotRound => Dwmapi.DWM_WINDOW_CORNER_PREFERENCE.DONOTROUND,
            _ => Dwmapi.DWM_WINDOW_CORNER_PREFERENCE.DEFAULT
        };
    }

    /// <summary>
    /// Casts <see cref="Dwmapi.DWM_WINDOW_CORNER_PREFERENCE"/> to <see cref="WindowCornerPreference"/>.
    /// </summary>
    public static WindowCornerPreference Cast(Dwmapi.DWM_WINDOW_CORNER_PREFERENCE cornerPreference)
    {
        return cornerPreference switch
        {
            Dwmapi.DWM_WINDOW_CORNER_PREFERENCE.ROUND => WindowCornerPreference.Round,
            Dwmapi.DWM_WINDOW_CORNER_PREFERENCE.ROUNDSMALL => WindowCornerPreference.RoundSmall,
            Dwmapi.DWM_WINDOW_CORNER_PREFERENCE.DONOTROUND => WindowCornerPreference.DoNotRound,
            _ => WindowCornerPreference.Default
        };
    }

    /// <summary>
    /// Casts <see cref="TaskbarProgressState"/> to <see cref="ShObjIdl.TBPFLAG"/>.
    /// </summary>
    public static ShObjIdl.TBPFLAG Cast(TaskbarProgressState taskbarProgressState)
    {
        return taskbarProgressState switch
        {
            TaskbarProgressState.Indeterminate => ShObjIdl.TBPFLAG.TBPF_INDETERMINATE,
            TaskbarProgressState.Error => ShObjIdl.TBPFLAG.TBPF_ERROR,
            TaskbarProgressState.Paused => ShObjIdl.TBPFLAG.TBPF_PAUSED,
            TaskbarProgressState.Normal => ShObjIdl.TBPFLAG.TBPF_NORMAL,
            _ => WPFUI.Interop.ShObjIdl.TBPFLAG.TBPF_NOPROGRESS
        };
    }

    /// <summary>
    /// Casts <see cref="ShObjIdl.TBPFLAG"/> to <see cref="TaskbarProgressState"/>.
    /// </summary>
    public static TaskbarProgressState Cast(ShObjIdl.TBPFLAG progressState)
    {
        return progressState switch
        {
            ShObjIdl.TBPFLAG.TBPF_INDETERMINATE => TaskbarProgressState.Indeterminate,
            ShObjIdl.TBPFLAG.TBPF_ERROR => TaskbarProgressState.Error,
            ShObjIdl.TBPFLAG.TBPF_PAUSED => TaskbarProgressState.Paused,
            ShObjIdl.TBPFLAG.TBPF_NORMAL => TaskbarProgressState.Normal,
            _ => TaskbarProgressState.None
        };
    }
}
