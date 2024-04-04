// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.
//
// This Source Code is partially based on reverse engineering of the Windows Operating System,
// and is intended for use on Windows systems only.
// This Source Code is partially based on the source code provided by the .NET Foundation.

using System.Runtime.InteropServices;

namespace Wpf.Ui.Interop.WinDef;

// ReSharper disable InconsistentNaming

/// <summary>
/// The RECTL structure defines a rectangle by the coordinates of its upper-left and lower-right corners.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct RECTL
{
    private long _left;
    private long _top;
    private long _right;
    private long _bottom;

    /// <summary>
    /// Gets or sets the x-coordinate of the upper-left corner of the rectangle.
    /// </summary>
    public long Left
    {
        readonly get { return _left; }
        set { _left = value; }
    }

    /// <summary>
    /// Gets or sets the x-coordinate of the lower-right corner of the rectangle.
    /// </summary>
    public long Right
    {
        readonly get { return _right; }
        set { _right = value; }
    }

    /// <summary>
    /// Gets or sets the y-coordinate of the upper-left corner of the rectangle.
    /// </summary>
    public long Top
    {
        readonly get { return _top; }
        set { _top = value; }
    }

    /// <summary>
    /// Gets or sets the y-coordinate of the lower-right corner of the rectangle.
    /// </summary>
    public long Bottom
    {
        readonly get { return _bottom; }
        set { _bottom = value; }
    }

    /// <summary>
    /// Gets the width of the rectangle.
    /// </summary>
    public readonly long Width
    {
        get { return _right - _left; }
    }

    /// <summary>
    /// Gets the height of the rectangle.
    /// </summary>
    public readonly long Height
    {
        get { return _bottom - _top; }
    }

    /// <summary>
    /// Gets the position of the rectangle.
    /// </summary>
    public POINTL Position
    {
        get { return new POINTL { x = _left, y = _top }; }
    }

    /// <summary>
    /// Gets the size of the rectangle.
    /// </summary>
    public SIZE Size
    {
        get { return new SIZE { cx = Width, cy = Height }; }
    }

    /// <summary>
    /// Sets offset of the rectangle.
    /// </summary>
    public void Offset(int dx, int dy)
    {
        _left += dx;
        _top += dy;
        _right += dx;
        _bottom += dy;
    }

    /// <summary>
    /// Combines two RECTLs
    /// </summary>
    public static RECTL Union(RECTL rect1, RECTL rect2)
    {
        return new RECTL
        {
            Left = Math.Min(rect1.Left, rect2.Left),
            Top = Math.Min(rect1.Top, rect2.Top),
            Right = Math.Max(rect1.Right, rect2.Right),
            Bottom = Math.Max(rect1.Bottom, rect2.Bottom),
        };
    }

    /// <inheritdoc />
    public override readonly bool Equals(object? obj)
    {
        if (obj is not RECTL)
        {
            return false;
        }

        try
        {
            var rc = (RECTL)obj;
            return rc._bottom == _bottom && rc._left == _left && rc._right == _right && rc._top == _top;
        }
        catch (InvalidCastException)
        {
            return false;
        }
    }

    /// <inheritdoc />
    public override readonly int GetHashCode()
    {
        return _top.GetHashCode() ^ _bottom.GetHashCode() ^ _left.GetHashCode() ^ _right.GetHashCode();
    }

    public static bool operator ==(RECTL left, RECTL right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(RECTL left, RECTL right)
    {
        return !(left == right);
    }
}
