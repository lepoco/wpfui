// This Source Code is partially based on reverse engineering of the Windows Operating System,
// and is intended for use on Windows systems only.
// This Source Code is partially based on the source code provided by the .NET Foundation.
// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski.
// All Rights Reserved.

using System;
using System.Runtime.InteropServices;

namespace Wpf.Ui.Interop.WinDef;

/// <summary>
/// The RECT structure defines a rectangle by the coordinates of its upper-left and lower-right corners.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
// ReSharper disable InconsistentNaming
public struct RECT
{
    private int _left;
    private int _top;
    private int _right;
    private int _bottom;

    /// <summary>
    /// Specifies the x-coordinate of the upper-left corner of the rectangle.
    /// </summary>
    public int Left
    {
        get { return _left; }
        set { _left = value; }
    }

    /// <summary>
    /// Specifies the x-coordinate of the lower-right corner of the rectangle.
    /// </summary>
    public int Right
    {
        get { return _right; }
        set { _right = value; }
    }

    /// <summary>
    /// Specifies the y-coordinate of the upper-left corner of the rectangle.
    /// </summary>
    public int Top
    {
        get { return _top; }
        set { _top = value; }
    }

    /// <summary>
    /// Specifies the y-coordinate of the lower-right corner of the rectangle.
    /// </summary>
    public int Bottom
    {
        get { return _bottom; }
        set { _bottom = value; }
    }

    /// <summary>
    /// Specifies the width of the rectangle.
    /// </summary>
    public int Width
    {
        get { return _right - _left; }
    }

    /// <summary>
    /// Specifies the height of the rectangle.
    /// </summary>
    public int Height
    {
        get { return _bottom - _top; }
    }

    /// <summary>
    /// Specifies the position of the rectangle.
    /// </summary>
    public POINT Position
    {
        get { return new POINT { x = _left, y = _top }; }
    }

    /// <summary>
    /// Specifies the size of the rectangle.
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
    /// Combines two RECTs.
    /// </summary>
    public static RECT Union(RECT rect1, RECT rect2)
    {
        return new RECT
        {
            Left = Math.Min(rect1.Left, rect2.Left),
            Top = Math.Min(rect1.Top, rect2.Top),
            Right = Math.Max(rect1.Right, rect2.Right),
            Bottom = Math.Max(rect1.Bottom, rect2.Bottom),
        };
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is not RECT)
            return false;

        try
        {
            var rc = (RECT)obj;

            return rc._bottom == _bottom
                && rc._left == _left
                && rc._right == _right
                && rc._top == _top;
        }
        catch (InvalidCastException)
        {
            return false;
        }
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return _top.GetHashCode() ^ _bottom.GetHashCode() ^ _left.GetHashCode() ^ _right.GetHashCode();
    }
}
