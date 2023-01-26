// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using Wpf.Ui.Contracts;
using Wpf.Ui.Controls.Navigation;

namespace Wpf.Ui.Services;

/// <summary>
/// A service that provides methods related to navigation.
/// </summary>
public partial class NavigationService : INavigationService
{
    /// <summary>
    /// Locally attached service provider.
    /// </summary>
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Locally attached page service.
    /// </summary>
    private IPageService? _pageService;

    /// <summary>
    /// Control representing navigation.
    /// </summary>
    protected INavigationView? NavigationControl;

    public NavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc />
    public INavigationView GetNavigationControl()
    {
        return NavigationControl ?? throw new ArgumentNullException(nameof(NavigationControl));
    }

    /// <inheritdoc />
    public void SetNavigationControl(INavigationView navigation)
    {
        NavigationControl = navigation;

        if (_pageService != null)
        {
            NavigationControl.SetPageService(_pageService);

            return;
        }

        NavigationControl.SetServiceProvider(_serviceProvider);
    }

    /// <inheritdoc />
    public void SetPageService(IPageService pageService)
    {
        if (NavigationControl == null)
        {
            _pageService = pageService;

            return;
        }

        NavigationControl.SetPageService(_pageService);
    }

    /// <inheritdoc />
    public bool Navigate(Type pageType)
    {
        if (NavigationControl == null)
            return false;

        return NavigationControl.Navigate(pageType);
    }

    /// <inheritdoc />
    public bool Navigate(string pageTag)
    {
        if (NavigationControl == null)
            return false;

        return NavigationControl.Navigate(pageTag);
    }

    /// <inheritdoc />
    public bool NavigateWithHierarchy(Type pageType)
    {
        if (NavigationControl == null)
            return false;

        return NavigationControl.NavigateWithHierarchy(pageType);
    }

    /// <inheritdoc />
    public bool NavigateWithHierarchy(string pageIdOrTargetTag)
    {
        if (NavigationControl == null)
            return false;

        return NavigationControl.NavigateWithHierarchy(pageIdOrTargetTag);
    }
}
