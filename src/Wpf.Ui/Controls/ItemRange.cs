// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

// Based on VirtualizingWrapPanel created by S. Bäumlisberger licensed under MIT license.
// https://github.com/sbaeumlisberger/VirtualizingWrapPanel
// Copyright (C) S. Bäumlisberger
// All Rights Reserved.

namespace Wpf.Ui.Controls;

/// <summary>
/// Items range.
/// <para>Based on <see href="https://github.com/sbaeumlisberger/VirtualizingWrapPanel"/>.</para>
/// </summary>
public struct ItemRange
{
    public int StartIndex { get; }
    public int EndIndex { get; }

    public ItemRange(int startIndex, int endIndex)
        : this()
    {
        StartIndex = startIndex;
        EndIndex = endIndex;
    }

    public bool Contains(int itemIndex)
    {
        return itemIndex >= StartIndex && itemIndex <= EndIndex;
    }
}
