// This Source Code is partially based on reverse engineering of the Windows Operating System,
// and is intended for use on Windows systems only.
// This Source Code is partially based on the source code provided by the .NET Foundation.
// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski.
// All Rights Reserved.

// NOTE
// I split unmanaged code stuff into the NativeMethods library.
// If you have suggestions for the code below, please submit your changes there.
// https://github.com/lepoco/nativemethods

using System.Runtime.InteropServices;

namespace Wpf.Ui.Interop;

/// <summary>
/// Used by multiple technologies.
/// </summary>
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
internal class Kernel32
{
    /// <summary>
    /// Retrieves the calling thread's last-error code value. The last-error code is maintained on a per-thread basis. Multiple threads do not overwrite each other's last-error code.
    /// </summary>
    /// <returns>The return value is the calling thread's last-error code.</returns>
    [DllImport(Libraries.Kernel32)]
    public static extern int GetLastError();

    /// <summary>
    /// Sets the last-error code for the calling thread.
    /// </summary>
    /// <param name="dwErrorCode">The last-error code for the thread.</param>
    [DllImport(Libraries.Kernel32, ExactSpelling = true, CharSet = CharSet.Auto)]
    public static extern void SetLastError([In] int dwErrorCode);

    /// <summary>
    /// Determines whether the calling process is being debugged by a user-mode debugger.
    /// </summary>
    /// <returns>If the current process is running in the context of a debugger, the return value is nonzero.</returns>
    [DllImport(Libraries.Kernel32, ExactSpelling = true, CharSet = CharSet.Auto)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool IsDebuggerPresent();
}
