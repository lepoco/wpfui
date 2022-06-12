// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.TaskBar;

/// <summary>
/// Specifies the state of the progress indicator in the Windows task bar.
/// <see href="https://docs.microsoft.com/en-us/dotnet/api/system.windows.shell.taskbaritemprogressstate?view=windowsdesktop-5.0"/>
/// </summary>
public enum TaskBarProgressState
{
    /// <summary>
    /// No progress indicator is displayed in the task bar area.
    /// </summary>
    None = 0x0,

    /// <summary>
    /// A pulsing green (W10) or gray (W11) indicator is displayed in the task bar area.
    /// </summary>
    Indeterminate = 0x1,

    /// <summary>
    /// A green progress indicator is displayed in the task bar area.
    /// </summary>
    Normal = 0x2,

    /// <summary>
    /// A red progress indicator is displayed in the task bar area.
    /// </summary>
    Error = 0x4,

    /// <summary>
    /// A yellow progress indicator is displayed in the task bar area.
    /// </summary>
    Paused = 0x8
}

