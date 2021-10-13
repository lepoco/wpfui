// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Based on Show Progress on Windows Taskbar Icon
// Copyright (C) 2014 Sean Sexton https://wpf.2000things.com/2014/03/19/1032-show-progress-on-windows-taskbar-icon/
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;

namespace WPFUI.Taskbar
{
    /// <summary>
    /// Represent error conditions, and warning conditions.
    /// <see href="https://docs.microsoft.com/en-us/windows/win32/seccrypto/common-hresult-values"/>
    /// </summary>
    [Flags]
    internal enum HResult
    {
        /// <summary>
        /// Operation successful.
        /// </summary>
        S_OK = 0x00000000,

        /// <summary>
        /// Operation aborted.
        /// </summary>
        //E_ABORT = 0x80004004,

        /// <summary>
        /// General access denied error.
        /// </summary>
        //E_ACCESSDENIED = 0x80070005,

        /// <summary>
        /// Unspecified failure.
        /// </summary>
        //E_FAIL = 0x80004005,

        /// <summary>
        /// Handle that is not valid.
        /// </summary>
        //E_HANDLE = 0x80070006,

        /// <summary>
        /// One or more arguments are not valid.
        /// </summary>
        //E_INVALIDARG = 0x80070057,

        /// <summary>
        /// No such interface supported.
        /// </summary>
        //E_NOINTERFACE = 0x80004002,

        /// <summary>
        /// Not implemented.
        /// </summary>
        //E_NOTIMPL = 0x80004001,

        /// <summary>
        /// Failed to allocate necessary memory.
        /// </summary>
        //E_OUTOFMEMORY = 0x8007000E,

        /// <summary>
        /// Pointer that is not valid.
        /// </summary>
        //E_POINTER = 0x80004003,

        /// <summary>
        /// Unexpected failure.
        /// </summary>
        //E_UNEXPECTED = 0x8000FFFF,
    }
}
