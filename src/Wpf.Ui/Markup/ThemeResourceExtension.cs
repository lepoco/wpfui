// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Markup;

namespace Wpf.Ui.Markup;

/// <summary>
/// Class for Xaml markup extension for static theme resource references.
/// </summary>
[MarkupExtensionReturnType(typeof(object))]
public class ThemeResourceExtension : MarkupExtension
{
    /// <summary>
    ///  The key in a Resource Dictionary used to find the object referred to by this
    ///  Markup Extension.
    /// </summary>
    [ConstructorArgument("resourceKey")] // Uses an instance descriptor
    public ThemeResource ResourceKey { get; set; }

    /// <summary>
    ///  Constructor that takes no parameters
    /// </summary>
    public ThemeResourceExtension()
    {
        ResourceKey = ThemeResource.Unknown;
    }

    /// <summary>
    ///  Constructor that takes the resource key that this is a static reference to.
    /// </summary>
    public ThemeResourceExtension(ThemeResource resourceKey)
    {
        ResourceKey = resourceKey;
    }

    /// <summary>
    ///  Return an object that should be set on the targetObject's targetProperty
    ///  for this markup extension.  For DynamicResourceExtension, this is the object found in
    ///  a resource dictionary in the current parent chain that is keyed by ResourceKey
    /// </summary>
    /// <returns>
    ///  The object to set on this property.
    /// </returns>
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (ResourceKey == ThemeResource.Unknown)
            throw new InvalidOperationException("Missing theme resource key.");

        if (serviceProvider != null)
        {
            // DynamicResourceExtensions are not allowed On CLR props except for Setter,Trigger,Condition (bugs 1183373,1572537)

            DependencyObject targetDependencyObject;
            DependencyProperty targetDependencyProperty;

            //Helper.CheckCanReceiveMarkupExtension(this, serviceProvider, out targetDependencyObject, out targetDependencyProperty);
        }

        var applicationResource = Application.Current.Resources[ResourceKey.ToString()];

        return applicationResource;
    }
}
