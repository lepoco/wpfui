using System.Windows.Controls;

namespace Wpf.Ui.Controls;

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

        GridViewColumnCollection columns = Columns;
        double accumulatedWidth = 0;
        var remainingWidth = arrangeSize.Width;

        if (columns != null)
        {
            for (var i = 0; i < columns.Count; ++i)
            {
                if (columns[i] is not GridViewColumn col)
                {
                    continue;
                }

                // use ActualIndex to track reordering when columns were dragged around
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
        }

        return arrangeSize;
    }
}
