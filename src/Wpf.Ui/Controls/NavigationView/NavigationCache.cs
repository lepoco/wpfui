// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

// Based on Windows UI Library
// Copyright(c) Microsoft Corporation.All rights reserved.

using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

internal class NavigationCache
{
    private IDictionary<Type, object?> _entires = new Dictionary<Type, object?>();

    public object? Remember(Type? entryType, NavigationCacheMode cacheMode, Func<object?> generate)
    {
        if (entryType == null)
        {
            return null;
        }

        if (cacheMode == NavigationCacheMode.Disabled)
        {
            return generate.Invoke();
        }

        if (!_entires.TryGetValue(entryType, out var value))
        {
            value = generate.Invoke();

            _entires.Add(entryType, value);
        }

        return value;
    }
}
