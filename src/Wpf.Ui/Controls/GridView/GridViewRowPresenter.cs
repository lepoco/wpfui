using System.Reflection;
using System.Windows.Controls;

namespace Wpf.Ui.Controls;

/// <summary>
/// Extends <see cref="System.Windows.Controls.GridViewRowPresenter"/>, and adds header row layout support for <see cref="GridViewColumn"/>, which can have <see cref="GridViewColumn.MinWidth"/> and <see cref="GridViewColumn.MaxWidth"/>.
/// </summary>
public class GridViewRowPresenter : System.Windows.Controls.GridViewRowPresenter
{
    protected override Size ArrangeOverride(Size arrangeSize)
    {
        // update the desired width of each column (clamps desiredwidth to MinWidth and MaxWidth)
        if (Columns != null)
        {
            foreach (GridViewColumn column in Columns.OfType<GridViewColumn>())
            {
                column.UpdateDesiredWidth();
            }
        }

        return base.ArrangeOverride(arrangeSize);
    }

    protected override Size MeasureOverride(Size constraint)
    {
        if (Columns != null)
        {
            foreach (GridViewColumn column in Columns.OfType<GridViewColumn>())
            {
                column.UpdateDesiredWidth();
            }
        }

        return base.MeasureOverride(constraint);
    }
}
