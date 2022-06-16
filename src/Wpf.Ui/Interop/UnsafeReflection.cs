// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Appearance;
using Wpf.Ui.TaskBar;

namespace Wpf.Ui.Interop;

/// <summary>
/// A set of dangerous methods to modify the appearance.
/// </summary>
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
    /// Casts <see cref="TaskBarProgressState"/> to <see cref="ShObjIdl.TBPFLAG"/>.
    /// </summary>
    public static ShObjIdl.TBPFLAG Cast(TaskBarProgressState taskBarProgressState)
    {
        return taskBarProgressState switch
        {
            TaskBarProgressState.Indeterminate => ShObjIdl.TBPFLAG.TBPF_INDETERMINATE,
            TaskBarProgressState.Error => ShObjIdl.TBPFLAG.TBPF_ERROR,
            TaskBarProgressState.Paused => ShObjIdl.TBPFLAG.TBPF_PAUSED,
            TaskBarProgressState.Normal => ShObjIdl.TBPFLAG.TBPF_NORMAL,
            _ => Wpf.Ui.Interop.ShObjIdl.TBPFLAG.TBPF_NOPROGRESS
        };
    }

    /// <summary>
    /// Casts <see cref="ShObjIdl.TBPFLAG"/> to <see cref="TaskBarProgressState"/>.
    /// </summary>
    public static TaskBarProgressState Cast(ShObjIdl.TBPFLAG progressState)
    {
        return progressState switch
        {
            ShObjIdl.TBPFLAG.TBPF_INDETERMINATE => TaskBarProgressState.Indeterminate,
            ShObjIdl.TBPFLAG.TBPF_ERROR => TaskBarProgressState.Error,
            ShObjIdl.TBPFLAG.TBPF_PAUSED => TaskBarProgressState.Paused,
            ShObjIdl.TBPFLAG.TBPF_NORMAL => TaskBarProgressState.Normal,
            _ => TaskBarProgressState.None
        };
    }
}
