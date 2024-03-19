// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Controls;

/// <summary>
/// Extends <see cref="System.Windows.Controls.GridViewHeaderRowPresenter"/>, and adds layout support for <see cref="GridViewColumn"/>, which can have <see cref="GridViewColumn.MinWidth"/> and <see cref="GridViewColumn.MaxWidth"/>.
/// </summary>
public class GridViewHeaderRowPresenter : System.Windows.Controls.GridViewHeaderRowPresenter
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
}
