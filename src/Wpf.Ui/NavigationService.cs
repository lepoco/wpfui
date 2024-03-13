// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Controls;

namespace Wpf.Ui;

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

    /// <summary>
    /// Initializes a new instance of the <see cref="NavigationService"/> class.
    /// </summary>
    /// <param name="serviceProvider">Service provider providing page instances.</param>
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

        ThrowIfPageServiceIsNull();

        NavigationControl.SetPageService(_pageService!);
    }

    /// <inheritdoc />
    public bool Navigate(Type pageType)
    {
        ThrowIfNavigationControlIsNull();

        return NavigationControl!.Navigate(pageType);
    }

    /// <inheritdoc />
    public bool Navigate(Type pageType, object? dataContext)
    {
        ThrowIfNavigationControlIsNull();

        return NavigationControl!.Navigate(pageType, dataContext);
    }

    /// <inheritdoc />
    public bool Navigate(string pageTag)
    {
        ThrowIfNavigationControlIsNull();

        return NavigationControl!.Navigate(pageTag);
    }

    /// <inheritdoc />
    public bool Navigate(string pageTag, object? dataContext)
    {
        ThrowIfNavigationControlIsNull();

        return NavigationControl!.Navigate(pageTag, dataContext);
    }

    /// <inheritdoc />
    public bool GoBack()
    {
        ThrowIfNavigationControlIsNull();

        return NavigationControl!.GoBack();
    }

    /// <inheritdoc />
    public bool NavigateWithHierarchy(Type pageType)
    {
        ThrowIfNavigationControlIsNull();

        return NavigationControl!.NavigateWithHierarchy(pageType);
    }

    /// <inheritdoc />
    public bool NavigateWithHierarchy(Type pageType, object? dataContext)
    {
        ThrowIfNavigationControlIsNull();

        return NavigationControl!.NavigateWithHierarchy(pageType, dataContext);
    }

    private void ThrowIfNavigationControlIsNull()
    {
        if (NavigationControl is null)
        {
            throw new ArgumentNullException(nameof(NavigationControl));
        }
    }

    private void ThrowIfPageServiceIsNull()
    {
        if (_pageService is null)
        {
            throw new ArgumentNullException(nameof(_pageService));
        }
    }
}
