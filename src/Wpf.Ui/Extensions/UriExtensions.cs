// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Linq;

namespace Wpf.Ui.Extensions;

/// <summary>
/// Extensions for <see cref="Uri"/> class.
/// </summary>
public static class UriExtensions
{
    /// <summary>
    /// Removes last segment of the <see cref="Uri"/>.
    /// </summary>
    public static Uri TrimLastSegment(this Uri uri)
    {
        if (uri.Segments.Length < 2)
            return uri;

#if NET5_0_OR_GREATER
        var uriLastSegmentLength = uri.Segments[^1].Length;
#else
        var uriLastSegmentLength = uri.Segments[uri.Segments.Length - 1].Length;
#endif
        var uriOriginalString = uri.ToString();

        return new Uri(uriOriginalString.Substring(0, uriOriginalString.Length - uriLastSegmentLength), UriKind.RelativeOrAbsolute);
    }

    /// <summary>
    /// Determines whether the end of <see cref="Uri"/> is equal to provided value.
    /// </summary>
    public static bool EndsWith(this Uri uri, string value)
    {
        var uriString = uri.ToString();

        return uri.ToString().EndsWith(value);
    }

    /// <summary>
    /// Append provided segments to the <see cref="Uri"/>.
    /// </summary>
    public static Uri Append(this Uri uri, params string[] segments)
    {
        if (!uri.IsAbsoluteUri)
            return uri; // or throw?

        return new Uri(segments.Aggregate(uri.AbsoluteUri, (current, path) => String.Format("{0}/{1}", current.TrimEnd('/').TrimEnd('\\'), path.TrimStart('/').TrimStart('\\'))));
    }

    /// <summary>
    /// Append new <see cref="Uri"/> to the <see cref="Uri"/>.
    /// </summary>
    public static Uri Append(this Uri uri, Uri value)
    {
        return new Uri(String.Format("{0}/{1}", uri.ToString().TrimEnd('/').TrimEnd('\\'), value.ToString().TrimStart('/').TrimStart('\\')), UriKind.RelativeOrAbsolute);
    }
}

