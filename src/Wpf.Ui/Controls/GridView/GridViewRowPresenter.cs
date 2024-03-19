using System.Windows.Controls;

namespace Wpf.Ui.Controls;

/// <summary>
/// Extends <see cref="System.Windows.Controls.GridViewRowPresenter"/>, and adds header row layout support for <see cref="GridViewColumn"/>, which can have <see cref="GridViewColumn.MinWidth"/> and <see cref="GridViewColumn.MaxWidth"/>.
/// </summary>
public class GridViewRowPresenter : System.Windows.Controls.GridViewRowPresenter
{
    /// <summary>
    /// GridViewRowPresenter computes the position of its children inside each child's Margin and calls Arrange
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

        for (var i = 0; i < Columns.Count; ++i)
        {
            if (Columns[i] is not GridViewColumn col)
            {
                continue;
            }

            // use ActualIndex to track reordering when columns are dragged around
            var visualIndex = col.ActualIndex;
            if (VisualTreeHelper.GetChild(this, visualIndex) is not UIElement child)
            {
                continue;
            }

            var clampedWidth = Math.Min(Math.Max(col.DesiredWidth, col.MinWidth), col.MaxWidth);
            clampedWidth = Math.Max(0, Math.Min(clampedWidth, remainingWidth));

            var rect = new Rect(accumulatedWidth, 0, clampedWidth, arrangeSize.Height);
            child.Arrange(rect);

            remainingWidth -= clampedWidth;
            accumulatedWidth += clampedWidth;
        }

        return arrangeSize;
    }
}
