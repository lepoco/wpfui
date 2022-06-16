// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Based on VirtualizingWrapPanel created by S. Bäumlisberger licensed under MIT license.
// https://github.com/sbaeumlisberger/VirtualizingWrapPanel
// Copyright (C) S. Bäumlisberger, Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Common;

/// <summary>
/// Items range.
/// <para>Based on <see href="https://github.com/sbaeumlisberger/VirtualizingWrapPanel"/>.</para>
/// </summary>
public struct ItemRange
{
    public int StartIndex { get; }
    public int EndIndex { get; }

    public ItemRange(int startIndex, int endIndex) : this()
    {
        StartIndex = startIndex;
        EndIndex = endIndex;
    }

    public bool Contains(int itemIndex)
    {
        return itemIndex >= StartIndex && itemIndex <= EndIndex;
    }
}

