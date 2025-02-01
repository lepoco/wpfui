// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Abstractions;
using Wpf.Ui.Controls;

namespace Wpf.Ui;

/// <summary>
/// A service that provides methods related to navigation.
/// </summary>
public partial class NavigationService(INavigationViewPageProvider pageProvider) : INavigationService
{
    /// <summary>
    /// Gets or sets the control representing navigation.
    /// </summary>
    protected INavigationView? NavigationControl { get; set; }

    /// <inheritdoc />
    public INavigationView GetNavigationControl()
    {
        return NavigationControl ?? throw new ArgumentNullException(nameof(NavigationControl));
    }

    /// <inheritdoc />
    public void SetNavigationControl(INavigationView navigation)
    {
        NavigationControl = navigation;
        NavigationControl.SetPageProviderService(pageProvider);
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

    protected void ThrowIfNavigationControlIsNull()
    {
        if (NavigationControl is null)
        {
            throw new ArgumentNullException(nameof(NavigationControl));
        }
    }
}
