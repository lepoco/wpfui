// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

#nullable enable
using System;
using System.Windows.Controls;
using CommunityToolkit.Diagnostics;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace Wpf.Ui.Mvvm.Services;

/// <summary>
/// A service that provides methods related to navigation.
/// </summary>
public partial class NavigationService : INavigationService
{
    private INavigation? _navigationControl;
    
    /// <summary>
    /// Locally attached page service.
    /// </summary>
    private IPageService? _pageService;

    /// <summary>
    /// Control representing navigation.
    /// </summary>
    private INavigation NavigationControl
    {
        get
        {
            Guard.IsNotNull(_navigationControl, nameof(NavigationControl));
            return _navigationControl;
        }
        set => _navigationControl = value;
    }

    public void Initialize(INavigation navigation, IPageService pageService)
    {
        NavigationControl = navigation;
        NavigationControl.SetIPageService(pageService);
    }

    /// <inheritdoc />
    public Frame GetFrame()
    {
        return NavigationControl.Frame;
    }

    /// <inheritdoc />
    public void SetFrame(Frame frame)
    {
        NavigationControl.Frame = frame;
    }

    /// <inheritdoc />
    public INavigation GetNavigationControl()
    {
        return NavigationControl;
    }

    public void SetNavigationControl(INavigation navigation)
    {
        NavigationControl = navigation;

        if (_pageService is not null)
            NavigationControl.SetIPageService(_pageService);
    }

    public void SetPageService(IPageService pageService)
    {
        if (_navigationControl is null)
        {
            _pageService = pageService;
            return;
        }

        NavigationControl.SetIPageService(pageService);
    }

    /// <inheritdoc />
    public void NavigateTo(string pageTag, object? dataContext = null)
    {
        NavigationControl.NavigateTo(pageTag, dataContext);
    }

    /// <inheritdoc />
    public void NavigateTo(Type type, object? dataContext = null)
    {
        NavigationControl.NavigateTo(type, dataContext);
    }
}
