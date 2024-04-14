// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.
//
// Based on Windows UI Library
// Copyright(c) Microsoft Corporation.All rights reserved.

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

internal class NavigationCache
{
    private readonly Dictionary<Type, object?> _entires = [];

    public object? Remember(Type? entryType, NavigationCacheMode cacheMode, Func<object?> generate)
    {
        if (entryType == null)
        {
            return null;
        }

        if (cacheMode == NavigationCacheMode.Disabled)
        {
            System.Diagnostics.Debug.WriteLine(
                $"Cache for {entryType} is disabled. Generating instance using action..."
            );

            return generate.Invoke();
        }

        if (!_entires.TryGetValue(entryType, out var value))
        {
            System.Diagnostics.Debug.WriteLine(
                $"{entryType} not found in cache, generating instance using action..."
            );

            value = generate.Invoke();

            _entires.Add(entryType, value);
        }

        System.Diagnostics.Debug.WriteLine($"{entryType} found in cache.");

        return value;
    }
}
