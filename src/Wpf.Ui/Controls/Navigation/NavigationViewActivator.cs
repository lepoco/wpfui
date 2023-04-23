// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common;

namespace Wpf.Ui.Controls.Navigation;

/// <summary>
/// Internal activator for creating content instances of the navigation view items.
/// </summary>
internal static class NavigationViewActivator
{
    /// <summary>
    /// Creates new instance of type derived from <see cref="FrameworkElement"/>.
    /// </summary>
    /// <param name="pageType"><see cref="FrameworkElement"/> to instantiate.</param>
    /// <param name="dataContext">Additional context to set.</param>
    /// <returns>Instance of the <see cref="FrameworkElement"/> object or <see langword="null"/>.</returns>
    public static FrameworkElement? CreateInstance(Type pageType, object? dataContext = null)
    {
        if (!typeof(FrameworkElement).IsAssignableFrom(pageType))
            throw new InvalidCastException(
                $"PageType of the ${typeof(INavigationViewItem)} must be derived from {typeof(FrameworkElement)}. {pageType} is not.");

        if (DesignerHelper.IsInDesignMode)
            return new Page { Content = new TextBlock { Text = "Pages are not rendered while using the Designer. Edit the page template directly." } };

        FrameworkElement? instance;

#if NET48_OR_GREATER || NETCOREAPP3_0_OR_GREATER
        if (ControlsServices.ControlsServiceProvider != null)
        {
            var pageConstructors = pageType.GetConstructors();
            var parameterlessCount = pageConstructors.Count(ctor => ctor.GetParameters().Length == 0);
            var parameterfullCount = pageConstructors.Length - parameterlessCount;

            if (parameterlessCount == 1)
            {
                instance = InvokeParameterlessConstructor(pageType);
            }
            else if (parameterlessCount == 0 && parameterfullCount > 0)
            {
                var selectedCtor = FitBestConstructor(pageConstructors, dataContext);
                if (selectedCtor == null)
                    throw new InvalidOperationException($"The {pageType} page does not have a parameterless constructor or the required services have not been configured for dependency injection. Use the static {nameof(ControlsServices)} class to initialize the GUI library with your service provider. If you are using {typeof(Contracts.IPageService)} do not navigate initially and don't use Cache or Precache.");

                instance = InvokeElementConstructor(selectedCtor, dataContext);
                SetDataContext(instance, dataContext);
                return instance;
            }
        }
        else if (dataContext != null)
#else
        if (dataContext != null)
#endif
        {
            instance = InvokeElementConstructor(pageType, dataContext);
            if (instance != null)
                return instance;
        }

        var emptyConstructor = FindParameterlessConstructor(pageType);

        if (emptyConstructor == null)
            throw new InvalidOperationException($"The {pageType} page does not have a parameterless constructor. If you are using {typeof(Contracts.IPageService)} do not navigate initially and don't use Cache or Precache.");

        instance = emptyConstructor.Invoke(null) as FrameworkElement;
        SetDataContext(instance, dataContext);
        return instance;
    }

#if NET48_OR_GREATER || NETCOREAPP3_0_OR_GREATER
    private static object? ResolveConstructorParameter(Type tParam, object? dataContext)
    {
        if (dataContext != null && dataContext.GetType() == tParam)
        {
            return dataContext;
        }

        return ControlsServices.ControlsServiceProvider?.GetService(tParam);
    }

    /// <summary>
    /// Picks a constructor which has the most satisfiable arguments count.
    /// </summary>
    /// <param name="parameterfullCtors"></param>
    /// <param name="dataContext"></param>
    /// <returns></returns>
    private static ConstructorInfo? FitBestConstructor(ConstructorInfo[] parameterfullCtors, object? dataContext)
    {
        return parameterfullCtors.Select(ctor =>
        {
            var parameters = ctor.GetParameters();
            var argumentResolution = parameters.Select(prm =>
            {
                var resolved = ResolveConstructorParameter(prm.ParameterType, dataContext);
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
        .FirstOrDefault()?.Constructor;
    }

    private static FrameworkElement? InvokeElementConstructor(ConstructorInfo ctor, object? dataContext)
    {
        var args = ctor
            .GetParameters()
            .Select(prm =>
                 ResolveConstructorParameter(prm.ParameterType, dataContext));

        return ctor.Invoke(args.ToArray()) as FrameworkElement;
    }
#endif

    private static FrameworkElement? InvokeElementConstructor(Type tPage, object? dataContext)
    {
        var ctor = dataContext is null ? tPage.GetConstructor(Type.EmptyTypes) : tPage.GetConstructor(new[] { dataContext!.GetType() });

        if (ctor != null)
            return ctor.Invoke(new[] { dataContext }) as FrameworkElement;

        return null;
    }

    private static ConstructorInfo? FindParameterlessConstructor(Type? tPage)
    {
        return tPage?.GetConstructor(Type.EmptyTypes);
    }

    private static FrameworkElement? InvokeParameterlessConstructor(Type? tPage)
    {
        return FindParameterlessConstructor(tPage)?.Invoke(null) as FrameworkElement;
    }

    private static void SetDataContext(FrameworkElement? element, object? dataContext)
    {
        if (element != null && dataContext != null)
            element.DataContext = dataContext;
    }
}
