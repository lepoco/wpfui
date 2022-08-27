// Based on Windows UI Library
// Copyright(c) Microsoft Corporation.All rights reserved.

// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

#nullable enable

using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace Wpf.Ui.Controls.Navigation;

public partial class NavigationView
{
    private IServiceProvider? _serviceProvider = null;

    private IPageService? _pageService = null;

    private readonly List<int> _history = new();

    /// <summary>
    /// Gets a value that indicates whether there is at least one entry in back navigation history.
    /// </summary>
    public bool CanGoBack => _history.Count > 1;

    /// <summary>
    /// Allows you to assign to the NavigationView a special service responsible for retrieving the page instances.
    /// </summary>
    public void SetPageService(IPageService pageService)
        => _pageService = pageService;

    /// <summary>
    /// Allows you to assign a general <see cref="IServiceProvider"/> to the NavigationView that will be used to retrieve page instances and view models.
    /// </summary>
    public void SetServiceProvider(IServiceProvider serviceProvider)
        => _serviceProvider = serviceProvider;

    /// <summary>
    /// This method synchronously navigates this Frame to the
    /// given Element.
    /// </summary>
    public void Navigate(Type pageType)
    {

    }

    public void Navigate(string pageTag)
    {

    }

    /// <inheritdoc />
    public bool GoForward()
    {
        var x = new Frame();

        return false;
    }

    /// <inheritdoc />
    public bool GoBack()
    {
        if (_history.Count <= 1)
            return false;

        //_isBackNavigated = true;

        //return NavigateInternal(_history[_history.Count - 2], null!);

        return false;
    }

    protected void NavigateInternal(INavigationViewItem menuItem)
    {

    }
}
