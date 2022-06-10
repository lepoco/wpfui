// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Media;

namespace WPFUI.TitleBar;

/// <summary>
/// Represents a snap layout button.
/// </summary>
internal class SnapLayoutButton
{
    /// <summary>
    /// Visual controls of the button.
    /// </summary>
    private readonly WPFUI.Controls.Button _visual;

    /// <summary>
    /// Type of the button.
    /// </summary>
    public readonly TitleBarButton Type;

    /// <summary>
    /// Rendered size of the button control.
    /// </summary>
    private Size _renderedSize;

    /// <summary>
    /// Whether the button is clicked.
    /// </summary>
    public bool IsClickedDown { get; set; }

    /// <summary>
    /// Whether the mouse is over the button.
    /// </summary>
    public bool IsHovered { get; private set; }

    /// <summary>
    /// Creates new instance and sets internals.
    /// </summary>
    public SnapLayoutButton(WPFUI.Controls.Button button, TitleBarButton type, double dpiScale)
    {
        _visual = button ?? throw new InvalidOperationException($"Parameter button of the {typeof(SnapLayoutButton)} cannot be null.");

        // TODO: If application is DPI aware, the scale can vary depends on the screen
        // Should also react to DPI change and adjust the Size

        if (button.IsLoaded)
        {
            DefineButtonSize(dpiScale);
        }
        else
        {
            button.Loaded += (_, _) =>
            {
                DefineButtonSize(dpiScale);
            };
        }

        Type = type;
        IsHovered = false;
        IsClickedDown = false;
    }

    /// <summary>
    /// Invokes click on the button.
    /// </summary>
    public void InvokeClick()
    {
        if (new ButtonAutomationPeer(_visual).GetPattern(PatternInterface.Invoke) is IInvokeProvider invokeProvider)
            invokeProvider.Invoke();

        IsClickedDown = false;
    }

    /// <summary>
    /// Forces button background to change.
    /// </summary>
    public void Hover(SolidColorBrush hoverBrush)
    {
        if (IsHovered)
            return;

        _visual.Background = hoverBrush;
        IsHovered = true;
    }

    /// <summary>
    /// Forces button background to change.
    /// </summary>
    public void RemoveHover(SolidColorBrush regularBrush)
    {
        if (!IsHovered)
            return;

        _visual.Background = regularBrush;

        IsHovered = false;
        IsClickedDown = false;
    }

    /// <summary>
    /// Indicates whether the mouse is over the button.
    /// </summary>
    public bool IsMouseOver(IntPtr positionPointer)
    {
        // This method will be invoked very often and must be as simple as possible.

        // Pointer carries no data
        if (positionPointer == IntPtr.Zero)
            return false;

        // Invalid button size
        if (_renderedSize.Height == 0 && _renderedSize.Width == 0)
            return false;

        var positionWords = positionPointer.ToInt32();

        if (positionWords < 1)
            return false;

        // The low-order word specifies the x-coordinate of the cursor.The coordinate is relative to the upper-left corner of the screen.
        var positionX = positionWords & 0xffff;
        // The high-order word specifies the y-coordinate of the cursor.The coordinate is relative to the upper-left corner of the screen.
        var positionY = positionWords >> 0x0010;

        // The screen has no negative positions
        if (positionX < 0 || positionY < 0)
            return false;

        // Button area on screen
        Rect rect;

        try
        {
            // Can throw exception during translation
            rect = new Rect(
                _visual.PointToScreen(new Point()),
                _renderedSize);
        }
        catch (Exception e)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"ERROR | {e}", "WPFUI.SnapLayout");
#endif
            return false; // or not to false, that is the question
        }

        // Whether the cursor is inside the button area
        return rect.Contains(new Point(positionX, positionY));
    }

    private void DefineButtonSize(double dpiScale)
    {
        // If the screen is scaled, the pixels/dots do not reflect the rendered size
        var renderedWidth = _visual.ActualWidth * dpiScale;
        var renderedHeight = _visual.ActualHeight * dpiScale;

        // Well, the one pixel button is probably not correct
        if (renderedWidth < 1 || renderedHeight < 1)
            _renderedSize = new Size(0d, 0d);
        else
            _renderedSize = new Size(renderedWidth, renderedHeight);
    }
}
