// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Hardware;

/// <summary>
/// An <see cref="System.Int32"/> value whose high-order word corresponds to the rendering tier for the current thread.
/// <para>Starting in the .NET Framework 4, rendering tier 1 has been redefined to only include graphics hardware that supports DirectX 9.0 or greater. Graphics hardware that supports DirectX 7 or 8 is now defined as rendering tier 0.</para>
/// </summary>
public enum RenderingTier
{
    /// <summary>
    /// No graphics hardware acceleration is available for the application on the device.
    /// All graphics features use software acceleration. The DirectX version level is less than version 9.0.
    /// </summary>
    NoAcceleration = 0x0,

    /// <summary>
    /// Most of the graphics features of WPF will use hardware acceleration
    /// if the necessary system resources are available and have not been exhausted.
    /// This corresponds to a DirectX version that is greater than or equal to 9.0.
    /// </summary>
    PartialAcceleration = 0x1,

    /// <summary>
    /// Most of the graphics features of WPF will use hardware acceleration provided the
    /// necessary system resources have not been exhausted.
    /// This corresponds to a DirectX version that is greater than or equal to 9.0.
    /// </summary>
    FullAcceleration = 0x2
}
