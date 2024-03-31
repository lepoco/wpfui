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
    private readonly IDictionary<Type, object?> _entires = new Dictionary<Type, object?>();

    public object? Remember(Type? entryType, NavigationCacheMode cacheMode, Func<object?> generate)
    {
        if (entryType == null)
        {
            return null;
        }

        if (cacheMode == NavigationCacheMode.Disabled)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine(
                $"Cache for {entryType} is disabled. Generating instance using action..."
            );
#endif

            return generate.Invoke();
        }

        if (!_entires.TryGetValue(entryType, out var value))
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine(
                $"{entryType} not found in cache, generating instance using action..."
            );
#endif

            value = generate.Invoke();

            _entires.Add(entryType, value);
        }

#if DEBUG
        System.Diagnostics.Debug.WriteLine($"{entryType} found in cache.");
#endif

        return value;
    }
}
