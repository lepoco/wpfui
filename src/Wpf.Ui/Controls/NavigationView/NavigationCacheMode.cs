// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.
//
// Based on Windows UI Library
// Copyright(c) Microsoft Corporation.All rights reserved.

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Specifies caching characteristics for a page involved in a navigation.
/// </summary>
public enum NavigationCacheMode
{
    /// <summary>
    /// The page is never cached and a new instance of the page is created on each visit.
    /// </summary>
    Disabled,

    /// <summary>
    /// The page is cached, but the cached instance is discarded when the size of the cache for the frame is exceeded.
    /// </summary>
    Enabled,

    /// <summary>
    /// The page is cached and the cached instance is reused for every visit regardless of the cache size for the frame.
    /// </summary>
    Required
}
