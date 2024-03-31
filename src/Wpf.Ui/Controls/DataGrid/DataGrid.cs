// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.Specialized;
using System.Windows.Controls;
using System.Windows.Data;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// A DataGrid control that displays data in rows and columns and allows
/// for the entering and editing of data.
/// </summary>
[StyleTypedProperty(Property = nameof(CheckBoxColumnElementStyle), StyleTargetType = typeof(CheckBox))]
[StyleTypedProperty(Property = nameof(CheckBoxColumnEditingElementStyle), StyleTargetType = typeof(CheckBox))]
public class DataGrid : System.Windows.Controls.DataGrid
{
    /// <summary>Identifies the <see cref="CheckBoxColumnElementStyle"/> dependency property.</summary>
    public static readonly DependencyProperty CheckBoxColumnElementStyleProperty =
        DependencyProperty.Register(
            nameof(CheckBoxColumnElementStyle),
            typeof(Style),
            typeof(DataGrid),
            new FrameworkPropertyMetadata(null)
        );

    /// <summary>Identifies the <see cref="CheckBoxColumnEditingElementStyle"/> dependency property.</summary>
    public static readonly DependencyProperty CheckBoxColumnEditingElementStyleProperty =
        DependencyProperty.Register(
            nameof(CheckBoxColumnEditingElementStyle),
            typeof(Style),
            typeof(DataGrid),
            new FrameworkPropertyMetadata(null)
        );

    /// <summary>
    /// Gets or sets the style which is applied to all checkbox column in the DataGrid
    /// </summary>
    public Style? CheckBoxColumnElementStyle
    {
        get => (Style?)GetValue(CheckBoxColumnElementStyleProperty);
        set => SetValue(CheckBoxColumnElementStyleProperty, value);
    }

    /// <summary>
    /// Gets or sets the style for all the column checkboxes in the DataGrid
    /// </summary>
    public Style? CheckBoxColumnEditingElementStyle
    {
        get => (Style?)GetValue(CheckBoxColumnEditingElementStyleProperty);
        set => SetValue(CheckBoxColumnEditingElementStyleProperty, value);
    }

    protected override void OnInitialized(EventArgs e)
    {
        Columns.CollectionChanged += ColumnsOnCollectionChanged;

        UpdateColumnElementStyles();

        base.OnInitialized(e);
    }

    private void ColumnsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        UpdateColumnElementStyles();
    }

    private void UpdateColumnElementStyles()
    {
        foreach (DataGridColumn singleColumn in Columns)
        {
            UpdateSingleColumn(singleColumn);
        }
    }

    private void UpdateSingleColumn(DataGridColumn dataGridColumn)
    {
        if (dataGridColumn is DataGridCheckBoxColumn checkBoxColumn)
        {
            if (
                checkBoxColumn.ReadLocalValue(DataGridCheckBoxColumn.ElementStyleProperty)
                == DependencyProperty.UnsetValue
            )
            {
                BindingOperations.SetBinding(
                    checkBoxColumn,
                    DataGridCheckBoxColumn.ElementStyleProperty,
                    new Binding { Path = new PropertyPath(CheckBoxColumnElementStyleProperty), Source = this }
                );
            }

            if (
                checkBoxColumn.ReadLocalValue(DataGridCheckBoxColumn.EditingElementStyleProperty)
                == DependencyProperty.UnsetValue
            )
            {
                BindingOperations.SetBinding(
                    checkBoxColumn,
                    DataGridCheckBoxColumn.EditingElementStyleProperty,
                    new Binding
                    {
                        Path = new PropertyPath(CheckBoxColumnEditingElementStyleProperty),
                        Source = this
                    }
                );
            }
        }
    }
}
