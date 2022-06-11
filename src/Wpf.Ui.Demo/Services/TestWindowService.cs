// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using Wpf.Ui.Demo.Services.Contracts;

namespace Wpf.Ui.Demo.Services;

public class TestWindowService : ITestWindowService
{
    private readonly IServiceProvider _serviceProvider;

    public TestWindowService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Show(Type windowType)
    {
        if (!typeof(Window).IsAssignableFrom(windowType))
            throw new InvalidOperationException($"The window class should be derived from {typeof(Window)}.");

        var windowInstance = _serviceProvider.GetService(windowType) as Window;

        windowInstance?.Show();
    }

    public T Show<T>() where T : class
    {
        if (!typeof(Window).IsAssignableFrom(typeof(T)))
            throw new InvalidOperationException($"The window class should be derived from {typeof(Window)}.");

        var windowInstance = _serviceProvider.GetService(typeof(T)) as Window;

        if (windowInstance == null)
            throw new InvalidOperationException("Window is not registered as service.");

        windowInstance.Show();

        return (T)Convert.ChangeType(windowInstance, typeof(T));
    }
}

