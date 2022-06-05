// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Controls;
using WPFUI.Common;
using WPFUI.Controls.Interfaces;

namespace WPFUI.Services;

/// <summary>
/// Internal activator for navigation purposes.
/// </summary>
internal static class NavigationServiceActivator
{
    /// <summary>
    /// Creates new instance of type derived from <see cref="FrameworkElement"/>.
    /// </summary>
    /// <param name="pageType"><see cref="FrameworkElement"/> to instantiate.</param>
    /// <returns>Instance of the <see cref="FrameworkElement"/> object or <see langword="null"/>.</returns>
    public static FrameworkElement CreateInstance(Type pageType)
    {
        return CreateInstance(pageType, null);
    }

    /// <summary>
    /// Creates new instance of type derived from <see cref="FrameworkElement"/>.
    /// </summary>
    /// <param name="pageType"><see cref="FrameworkElement"/> to instantiate.</param>
    /// <param name="dataContext">Additional context to set.</param>
    /// <returns>Instance of the <see cref="FrameworkElement"/> object or <see langword="null"/>.</returns>
    public static FrameworkElement CreateInstance(Type pageType, object dataContext)
    {
        // TODO: Refactor

        if (!typeof(FrameworkElement).IsAssignableFrom(pageType))
            throw new InvalidCastException(
                $"PageType of the ${typeof(INavigationItem)} must be derived from {typeof(FrameworkElement)}");

        if (DesignerHelper.IsInDesignMode)
            return new Page { Content = new TextBlock { Text = "Preview" } };

        if (pageType.GetConstructor(Type.EmptyTypes) == null)
            throw new InvalidOperationException("The page does not have a parameterless constructor. If you are using IServicePage do not navigate initially and don't use Cache or Precache.");

        if (dataContext != null)
        {
            var dataContextConstructor = pageType.GetConstructor(new[] { dataContext.GetType() });

            // Return instance which has constructor with matching datacontext type
            if (dataContextConstructor != null)
                return dataContextConstructor.Invoke(new[] { dataContext }) as FrameworkElement;
        }

        var emptyConstructor = pageType.GetConstructor(Type.EmptyTypes);

        if (emptyConstructor == null)
            return null;

        var instance = emptyConstructor.Invoke(null) as FrameworkElement;

        if (dataContext != null)
            instance!.DataContext = dataContext;

        return instance;
    }
}
