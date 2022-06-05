// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows.Controls;
using WPFUI.Controls.Interfaces;
using WPFUI.Mvvm.Contracts;

namespace WPFUI.Mvvm.Services;

/// <summary>
/// A service that provides methods related to navigation.
/// </summary>
public partial class NavigationService : INavigationService
{
    /// <summary>
    /// Locally attached page service.
    /// </summary>
    private IPageService _pageService;

    /// <summary>
    /// Control representing navigation.
    /// </summary>
    protected INavigation NavigationControl;

    /// <inheritdoc />
    public Frame GetFrame()
    {
        return NavigationControl?.Frame;
    }

    /// <inheritdoc />
    public void SetFrame(Frame frame)
    {
        if (NavigationControl == null)
            return;

        NavigationControl.Frame = frame;
    }

    /// <inheritdoc />
    public INavigation GetNavigation()
    {
        return NavigationControl;
    }

    /// <inheritdoc />
    public void SetNavigation(INavigation navigation)
    {
        NavigationControl = navigation;

        if (_pageService != null)
            NavigationControl.PageService = _pageService;
    }

    /// <inheritdoc />
    public void SetPageService(IPageService pageService)
    {
        if (NavigationControl == null)
        {
            _pageService = pageService;

            return;
        }

        NavigationControl.PageService = pageService;
    }

    /// <inheritdoc />
    public bool Navigate(Type pageType)
    {
        if (NavigationControl == null)
            return false;

        return NavigationControl.Navigate(pageType);
    }

    /// <inheritdoc />
    public bool Navigate(int pageId)
    {
        if (NavigationControl == null)
            return false;

        return NavigationControl.Navigate(pageId);
    }

    /// <inheritdoc />
    public bool Navigate(string pageTag)
    {
        if (NavigationControl == null)
            return false;

        return NavigationControl.Navigate(pageTag);
    }
}
