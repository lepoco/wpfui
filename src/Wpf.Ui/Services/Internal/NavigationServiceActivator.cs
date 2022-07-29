// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common;
using Wpf.Ui.Controls.Interfaces;

namespace Wpf.Ui.Services.Internal;

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
        if (!typeof(FrameworkElement).IsAssignableFrom(pageType))
            throw new InvalidCastException(
                $"PageType of the ${typeof(INavigationItem)} must be derived from {typeof(FrameworkElement)}. {pageType} is not.");

        if (DesignerHelper.IsInDesignMode)
            return new Page { Content = new TextBlock { Text = "Pages are not rendered while using the Designer. Edit the page template directly." } };

        var instance = null as FrameworkElement;

#if NET48_OR_GREATER || NETCOREAPP3_0_OR_GREATER
        if (WpfUi.ServiceProvider != null)
        {
            var pageConstructors = pageType.GetConstructors();
            var parameterlessCount = pageConstructors.Count(ctor => ctor.GetParameters().Length == 0);
            var parameterfullCount = pageConstructors.Length - parameterlessCount;

            if (parameterlessCount == 1)
                instance = pageType.GetConstructor(Type.EmptyTypes).Invoke(null) as FrameworkElement;

            else if (parameterlessCount == 0 && parameterfullCount > 0)
            {
                var maximalCtor = pageConstructors.Select(ctor =>
                {
                    var parameters = ctor.GetParameters();
                    var argumentResolution = parameters.Select(prm =>
                    {
                        var resolved = ResolveArgument(prm.ParameterType, dataContext);
                        return resolved != null;
                    });
                    var fullyResolved = argumentResolution.All(resolved => resolved == true);
                    var score = fullyResolved ? parameters.Length : 0;

                    return score == 0 ? null : new
                    {
                        Constructor = ctor,
                        Score = score
                    };
                })
                .Where(cs => cs != null)
                .OrderBy(cs => cs.Score)
                .FirstOrDefault();

                if (maximalCtor == null)
                    throw new InvalidOperationException($"The {pageType} page does not have a parameterless constructor or the required services have not been configured for dependency injection. Use the static WpfUi class to initialize the GUI library with your service provider. If you are using {typeof(Mvvm.Contracts.IPageService)} do not navigate initially and don't use Cache or Precache.");

                var arguments = maximalCtor
                    .Constructor.GetParameters()
                    .Select(prm => ResolveArgument(prm.ParameterType, dataContext));

                instance = maximalCtor.Constructor.Invoke(arguments.ToArray()) as FrameworkElement;

                if (dataContext != null)
                    instance!.DataContext = dataContext;

                return instance;
            }
        }
        else if (dataContext != null)
        {
            var dataContextConstructor = pageType.GetConstructor(new[] { dataContext.GetType() });

            // Return instance which has constructor with matching datacontext type
            if (dataContextConstructor != null)
                return dataContextConstructor.Invoke(new[] { dataContext }) as FrameworkElement;
        }
#else
        // Very poor dependency injection
        if (dataContext != null)
        {
            var dataContextConstructor = pageType.GetConstructor(new[] { dataContext.GetType() });

            // Return instance which has constructor with matching datacontext type
            if (dataContextConstructor != null)
                return dataContextConstructor.Invoke(new[] { dataContext }) as FrameworkElement;
        }
#endif

        var emptyConstructor = pageType.GetConstructor(Type.EmptyTypes);

        if (emptyConstructor == null)
            throw new InvalidOperationException($"The {pageType} page does not have a parameterless constructor. If you are using {typeof(Mvvm.Contracts.IPageService)} do not navigate initially and don't use Cache or Precache.");

        instance = emptyConstructor.Invoke(null) as FrameworkElement;

        if (dataContext != null)
            instance!.DataContext = dataContext;

        return instance;
    }

    private static object ResolveArgument(Type tParam, object dataContext)
    {
        if (dataContext != null && dataContext.GetType() == tParam)
        {
            return dataContext;
        }

        return WpfUi.ServiceProvider.GetService(tParam);
    }
}
