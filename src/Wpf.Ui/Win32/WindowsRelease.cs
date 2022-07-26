// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Win32;

/// <summary>
/// Collection of Windows releases with it's build numbers.
/// </summary>
internal enum WindowsRelease
{
    /// <summary>
    /// Windows 95.
    /// </summary>
    Windows95 = 950, // The good old days

    /// <summary>
    /// Windows 98.
    /// </summary>
    Windows98 = 1998,

    /// <summary>
    /// Windows 2000.
    /// </summary>
    Windows2000 = 2195,

    /// <summary>
    /// Windows XP.
    /// </summary>
    WindowsXP = 2600,

    /// <summary>
    /// Windows Me.
    /// </summary>
    WindowsMe = 3000,

    /// <summary>
    /// Windows 7.
    /// </summary>
    Windows7 = 7600,

    /// <summary>
    /// Windows 7, Service Pack 1.
    /// </summary>
    Windows7Sp1 = 7601,

    /// <summary>
    /// Windows 8.
    /// </summary>
    Windows8 = 9200,

    /// <summary>
    /// Windows 8.1.
    /// </summary>
    Windows81 = 9600,

    /// <summary>
    /// Windows 10, Version 1507.
    /// </summary>
    Windows10 = 10240,

    /// <summary>
    /// Windows 10 November Update, Version 1507.
    /// </summary>
    Windows10V1511 = 10586,

    /// <summary>
    /// Windows 10 Anniversary Update, Version 1607.
    /// </summary>
    Windows10V1607 = 14393,

    /// <summary>
    /// Windows 10 Creators Update, Version 1703.
    /// </summary>
    Windows10V1703 = 15063,

    /// <summary>
    /// Windows 10 October 2018 Update, Version 1809.
    /// </summary>
    Windows10V1809 = 17763,

    /// <summary>
    /// Windows 10 May 2019 Update, Version 1903.
    /// </summary>
    Windows10V19H1 = 18362,

    /// <summary>
    /// Windows 10 Insider Preview Build 18985
    /// </summary>
    Windows10Insider1 = 18985,

    /// <summary>
    /// Windows 10 May 2020 Update, Version 2004.
    /// </summary>
    Windows10V20H1 = 19041,

    /// <summary>
    /// Windows 10 October 2020 Update, Version 20H2.
    /// </summary>
    Windows10V20H2 = 19042,

    /// <summary>
    /// Windows 10 May 2021 Update, Version 21H1.
    /// </summary>
    Windows10V21H1 = 19043,

    /// <summary>
    /// Windows 10 November 2021 Update, Version 21H2.
    /// </summary>
    Windows10V21H2 = 19044,

    /// <summary>
    /// Windows Server 2022.
    /// </summary>
    WindowsServer2022 = 20348,

    /// <summary>
    /// Windows 11, Version 21H2.
    /// </summary>
    Windows11 = 22000,

    /// <summary>
    /// Windows 11 Insider Preview Build 22523 - Dev Channel.
    /// </summary>
    Windows11Insider1 = 22523,

    /// <summary>
    /// Windows 11 Insider Preview Build 22557 - Dev Channel.
    /// </summary>
    Windows11Insider2 = 22557,
}
