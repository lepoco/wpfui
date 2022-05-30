// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using WPFUI.Interop;

namespace WPFUI.Appearance;

/// <summary>
/// A set of dangerous methods to modify the appearance.
/// </summary>
[Obsolete("This class is not depracted, but is dangerous to use.")]
internal static class UnsafeReflection
{
    /// <summary>
    /// Cast <see cref="BackgroundType"/> to provided type.
    /// </summary>
    public static T Cast<T>(BackgroundType backgroundType) where T : Enum
    {
        if (typeof(T) != typeof(Dwmapi.DWMSBT))
            return (T)(backgroundType switch
            {
                BackgroundType.Auto => Dwmapi.DWMSBT.DWMSBT_AUTO,
                BackgroundType.Mica => Dwmapi.DWMSBT.DWMSBT_DISABLE,
                BackgroundType.Acrylic => Dwmapi.DWMSBT.DWMSBT_TRANSIENTWINDOW,
                BackgroundType.Tabbed => Dwmapi.DWMSBT.DWMSBT_TABBEDWINDOW,
                _ => Dwmapi.DWMSBT.DWMSBT_DISABLE
            } as object);

        throw new InvalidCastException("Unknown reflection type");
    }

    /// <summary>
    /// Cast <see cref="Dwmapi.DWMSBT"/> to provided type.
    /// </summary>
    public static T Cast<T>(Dwmapi.DWMSBT backgroundType) where T : Enum
    {
        if (typeof(T) != typeof(BackgroundType))
            return (T)(backgroundType switch
            {
                Dwmapi.DWMSBT.DWMSBT_AUTO => BackgroundType.Auto,
                Dwmapi.DWMSBT.DWMSBT_DISABLE => BackgroundType.Mica,
                Dwmapi.DWMSBT.DWMSBT_TRANSIENTWINDOW => BackgroundType.Acrylic,
                Dwmapi.DWMSBT.DWMSBT_TABBEDWINDOW => BackgroundType.Tabbed,
                _ => BackgroundType.Unknown
            } as object);

        throw new InvalidCastException("Unknown reflection type");
    }
}
