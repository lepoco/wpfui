// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;

namespace Wpf.Ui.Markup;

/// <summary>
///  Class for Xaml markup extension for static resource references.
/// </summary>
[TypeConverter(typeof(DynamicResourceExtensionConverter))]
[MarkupExtensionReturnType(typeof(object))]
public class ThemeResourceExtension : DynamicResourceExtension
{
    /// <summary>
    ///  Constructor that takes no parameters
    /// </summary>
    public ThemeResourceExtension()
    {
        ResourceKey = ThemeResource.ApplicationBackgroundBrush.ToString();
    }

    /// <summary>
    ///  Constructor that takes the resource key that this is a static reference to.
    /// </summary>
    public ThemeResourceExtension(ThemeResource resourceKey)
    {
        if (resourceKey == ThemeResource.Unknown)
            throw new ArgumentNullException(nameof(resourceKey));

        ResourceKey = resourceKey.ToString();
    }
}
