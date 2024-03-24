// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Diagnostics;
using System.Reflection;
using System.Windows.Controls;

namespace Wpf.Ui.Controls;

/// <summary>
/// Extends <see cref="System.Windows.Controls.GridViewHeaderRowPresenter"/>, and adds layout support for <see cref="GridViewColumn"/>, which can have <see cref="GridViewColumn.MinWidth"/> and <see cref="GridViewColumn.MaxWidth"/>.
/// </summary>
public class GridViewHeaderRowPresenter : System.Windows.Controls.GridViewHeaderRowPresenter
{
    public GridViewHeaderRowPresenter()
    {
        Loaded += OnLoaded;
    }

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

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        UpdateIndicatorStyle();
    }

    private void UpdateIndicatorStyle()
    {
        FieldInfo? indicatorField = typeof(System.Windows.Controls.GridViewHeaderRowPresenter).GetField(
            "_indicator",
            BindingFlags.NonPublic | BindingFlags.Instance
        );

        if (indicatorField == null)
        {
            Debug.WriteLine("Failed to get the _indicator field");
            return;
        }

        if (indicatorField.GetValue(this) is Separator indicator)
        {
            indicator.Margin = new Thickness(0);
            indicator.Width = 3.0;

            ResourceDictionary resourceDictionary =
                new()
                {
                    Source = new Uri(
                        "pack://application:,,,/Wpf.Ui;component/Controls/GridView/GridViewHeaderRowIndicator.xaml",
                        UriKind.Absolute
                    )
                };

            if (resourceDictionary["GridViewHeaderRowIndicatorTemplate"] is ControlTemplate template)
            {
                indicator.Template = template;
            }
            else
            {
                Debug.WriteLine("Failed to get the GridViewHeaderRowIndicatorTemplate");
            }
        }
    }
}
