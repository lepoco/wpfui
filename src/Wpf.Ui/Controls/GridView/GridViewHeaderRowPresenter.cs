// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Reflection;

namespace Wpf.Ui.Controls;

/// <summary>
/// Extends <see cref="System.Windows.Controls.GridViewHeaderRowPresenter"/>, and adds layout support for <see cref="GridViewColumn"/>, which can have <see cref="GridViewColumn.MinWidth"/> and <see cref="GridViewColumn.MaxWidth"/>.
/// </summary>
public class GridViewHeaderRowPresenter : System.Windows.Controls.GridViewHeaderRowPresenter
{
    // use reflection to get the `HeadersPositionList` internal property. cache the `PropertyInfo` for performance
    private static readonly PropertyInfo _headersPositionListPropertyInfo =
        typeof(System.Windows.Controls.GridViewHeaderRowPresenter).GetProperty("HeadersPositionList", BindingFlags.NonPublic | BindingFlags.Instance)
        ?? throw new InvalidOperationException("The `HeadersPositionList` property was not found.");

    private List<Rect> GetHeadersPositionList() => _headersPositionListPropertyInfo.GetValue(this) as List<Rect>
           ?? throw new InvalidOperationException("HeadersPositionList is null");

    // use reflection to get the `_isHeaderDragging` private property. cache the `FieldInfo` for performance
    private static readonly FieldInfo _isHeaderDraggingFieldInfo =
        typeof(System.Windows.Controls.GridViewHeaderRowPresenter).GetField("_isHeaderDragging", BindingFlags.NonPublic | BindingFlags.Instance)
        ?? throw new InvalidOperationException("The `_isHeaderDragging` field was not found.");

    private bool IsHeaderDragging => (bool)(_isHeaderDraggingFieldInfo.GetValue(this) ?? throw new InvalidOperationException("The `_isHeaderDragging` field was not found."));

    private static readonly FieldInfo _startPosFieldInfo =
        typeof(System.Windows.Controls.GridViewHeaderRowPresenter).GetField("_startPos", BindingFlags.NonPublic | BindingFlags.Instance)
        ?? throw new InvalidOperationException("The `_startPos` field was not found.");

    // start position when dragging (position relative to GridViewHeaderRowPresenter)
    private Point StartPos => (Point)(_startPosFieldInfo.GetValue(this) ?? throw new InvalidOperationException("The `_startPos` field was not found."));

    private static readonly FieldInfo _relativeStartPosFieldInfo =
        typeof(System.Windows.Controls.GridViewHeaderRowPresenter).GetField("_relativeStartPos", BindingFlags.NonPublic | BindingFlags.Instance)
        ?? throw new InvalidOperationException("The `_relativeStartPos` field was not found.");

    // relative start position when dragging (position relative to Header)
    private Point RelativeStartPos => (Point)(_relativeStartPosFieldInfo.GetValue(this) ?? throw new InvalidOperationException("The `_relativeStartPos` field was not found."));

    private static readonly FieldInfo _currentPosFieldInfo =
        typeof(System.Windows.Controls.GridViewHeaderRowPresenter).GetField("_currentPos", BindingFlags.NonPublic | BindingFlags.Instance)
        ?? throw new InvalidOperationException("The `_currentPos` field was not found.");

    // current mouse position (position relative to GridViewHeaderRowPresenter)
    private Point CurrentPos => (Point)(_currentPosFieldInfo.GetValue(this) ?? throw new InvalidOperationException("The `_currentPos` field was not found."));

    private static readonly FieldInfo _startColumnIndexFieldInfo =
        typeof(System.Windows.Controls.GridViewHeaderRowPresenter).GetField("_startColumnIndex", BindingFlags.NonPublic | BindingFlags.Instance)
        ?? throw new InvalidOperationException("The `_startColumnIndex` field was not found.");

    // start column index when begin dragging
    private int StartColumnIndex => (int)(_startColumnIndexFieldInfo.GetValue(this) ?? throw new InvalidOperationException("The `_startColumnIndex` field was not found."));

    private static readonly FieldInfo _desColumnIndexFieldInfo =
        typeof(System.Windows.Controls.GridViewHeaderRowPresenter).GetField("_desColumnIndex", BindingFlags.NonPublic | BindingFlags.Instance)
        ?? throw new InvalidOperationException("The `_desColumnIndex` field was not found.");

    // destination column index when finish dragging
    private int DesColumnIndex => (int)(_desColumnIndexFieldInfo.GetValue(this) ?? throw new InvalidOperationException("The `_desColumnIndex` field was not found."));

    private static readonly MethodInfo _findPositionByIndexMethodInfo =
        typeof(System.Windows.Controls.GridViewHeaderRowPresenter).GetMethod("FindPositionByIndex", BindingFlags.NonPublic | BindingFlags.Instance)
        ?? throw new InvalidOperationException("The `FindPositionByIndex` method was not found.");

    // Find position by logic column index
    private Point FindPositionByIndex(int index)
    {
        return (Point)(_findPositionByIndexMethodInfo.Invoke(this, new object[] { index })
                ?? throw new InvalidOperationException("`FindPositionByIndex` method invocation resulted in null."));
    }

    /// <summary>
    /// computes the position of its children inside each child's Margin and calls Arrange
    /// on each child.
    /// </summary>
    /// <param name="arrangeSize">Size the GridViewRowPresenter will assume.</param>
    /// <returns> The actual size used. </returns>
    protected override Size ArrangeOverride(Size arrangeSize)
    {
        _ = base.ArrangeOverride(arrangeSize);

        // exit early if columns are not Wpf.Ui.Controls.GridViewColumn
        if (Columns == null || Columns.Count == 0 || Columns[0] is not GridViewColumn)
        {
            return arrangeSize;
        }

        double accumulatedWidth = 0;
        var remainingWidth = arrangeSize.Width;
        Rect rect;
        List<Rect> headersPositionList = GetHeadersPositionList();
        headersPositionList.Clear();

        for (var i = 0; i < Columns.Count; ++i)
        {
            var visualIndex = GetVisualIndex(i);
            if (VisualTreeHelper.GetChild(this, visualIndex) is not UIElement child || Columns[i] is not GridViewColumn col)
            {
                continue;
            }

            var clampedWidth = Math.Min(Math.Max(col.DesiredWidth, col.MinWidth), col.MaxWidth);
            clampedWidth = Math.Max(0, Math.Min(clampedWidth, remainingWidth));

            rect = new Rect(accumulatedWidth, 0, clampedWidth, arrangeSize.Height);
            child.Arrange(rect);

            headersPositionList.Add(rect);
            remainingWidth -= clampedWidth;
            accumulatedWidth += clampedWidth;
        }

        // Arrange padding header
        UIElement paddingHeader = VisualTreeHelper.GetChild(this, 0) as UIElement
            ?? throw new InvalidOperationException("padding header is null");
        rect = new Rect(accumulatedWidth, 0.0, Math.Max(remainingWidth, 0.0), arrangeSize.Height);
        paddingHeader.Arrange(rect);
        headersPositionList.Add(rect);

        // if re-order started, arrange floating header & indicator
        if (IsHeaderDragging)
        {
            UIElement floatingHeader = VisualTreeHelper.GetChild(this, VisualTreeHelper.GetChildrenCount(this) - 1) as UIElement
                ?? throw new InvalidOperationException("floating header is null"); // last child
            UIElement separator = VisualTreeHelper.GetChild(this, VisualTreeHelper.GetChildrenCount(this) - 2) as UIElement
                ?? throw new InvalidOperationException("indicator is null"); // second to last child

            floatingHeader.Arrange(new Rect(new Point(CurrentPos.X - RelativeStartPos.X, 0), headersPositionList[StartColumnIndex].Size));

            Point pos = FindPositionByIndex(DesColumnIndex);
            separator.Arrange(new Rect(pos, new Size(separator.DesiredSize.Width, arrangeSize.Height)));
        }

        return arrangeSize;
    }

    private int GetVisualIndex(int columnIndex)
    {
        // VisualTree: [PaddingHeader, ColumnHeaders (in reverse order), Separator, FloatingHeader]
        var index = VisualTreeHelper.GetChildrenCount(this) - 3 - columnIndex; // -1 for index counting & -2 for Seperator and FloatingHeader
        return index;
    }

    // helper method to get all visual children, useful for debugging
    /*
    public static List<UIElement> GetVisualChildren(DependencyObject parent)
    {
        var list = new List<UIElement>();
        for (var i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
        {
            DependencyObject child = VisualTreeHelper.GetChild(parent, i);
            if (child is UIElement element)
            {
                list.Add(element);
            }
        }

        return list;
    }
    */
}
