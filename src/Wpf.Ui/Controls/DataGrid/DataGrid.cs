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
[StyleTypedProperty(Property = nameof(TextBoxColumnElementStyle), StyleTargetType = typeof(TextBox))]
[StyleTypedProperty(Property = nameof(TextBoxColumnEditingElementStyle), StyleTargetType = typeof(TextBox))]
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

    /// <summary>Identifies the <see cref="TextBoxColumnElementStyle"/> dependency property.</summary>
    public static readonly DependencyProperty TextBoxColumnElementStyleProperty =
        DependencyProperty.Register(
            nameof(TextBoxColumnElementStyle),
            typeof(Style),
            typeof(DataGrid),
            new FrameworkPropertyMetadata(null)
        );

    /// <summary>Identifies the <see cref="TextBoxColumnEditingElementStyle"/> dependency property.</summary>
    public static readonly DependencyProperty TextBoxColumnEditingElementStyleProperty =
        DependencyProperty.Register(
            nameof(TextBoxColumnEditingElementStyle),
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

    /// <summary>
    /// Gets or sets the style which is applied to all textbox column in the DataGrid
    /// </summary>
    public Style? TextBoxColumnElementStyle
    {
        get => (Style?)GetValue(TextBoxColumnElementStyleProperty);
        set => SetValue(TextBoxColumnElementStyleProperty, value);
    }

    /// <summary>
    /// Gets or sets the style for all the column textboxes in the DataGrid
    /// </summary>
    public Style? TextBoxColumnEditingElementStyle
    {
        get => (Style?)GetValue(TextBoxColumnEditingElementStyleProperty);
        set => SetValue(TextBoxColumnEditingElementStyleProperty, value);
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
        switch (dataGridColumn)
        {
            case DataGridCheckBoxColumn checkBoxColumn:
                if (
                    checkBoxColumn.ReadLocalValue(DataGridBoundColumn.ElementStyleProperty)
                    == DependencyProperty.UnsetValue
                )
                {
                    _ = BindingOperations.SetBinding(
                        checkBoxColumn,
                        DataGridBoundColumn.ElementStyleProperty,
                        new Binding { Path = new PropertyPath(CheckBoxColumnElementStyleProperty), Source = this }
                    );
                }

                if (
                    checkBoxColumn.ReadLocalValue(DataGridBoundColumn.EditingElementStyleProperty)
                    == DependencyProperty.UnsetValue
                )
                {
                    _ = BindingOperations.SetBinding(
                        checkBoxColumn,
                        DataGridBoundColumn.EditingElementStyleProperty,
                        new Binding
                        {
                            Path = new PropertyPath(CheckBoxColumnEditingElementStyleProperty), Source = this
                        }
                    );
                }

                break;

            case DataGridTextColumn textBoxColumn:
                if (
                    textBoxColumn.ReadLocalValue(DataGridBoundColumn.ElementStyleProperty)
                    == DependencyProperty.UnsetValue
                )
                {
                    _ = BindingOperations.SetBinding(
                        textBoxColumn,
                        DataGridBoundColumn.ElementStyleProperty,
                        new Binding { Path = new PropertyPath(TextBoxColumnElementStyleProperty), Source = this }
                    );
                }

                if (
                    textBoxColumn.ReadLocalValue(DataGridBoundColumn.EditingElementStyleProperty)
                    == DependencyProperty.UnsetValue
                )
                {
                    _ = BindingOperations.SetBinding(
                        textBoxColumn,
                        DataGridBoundColumn.EditingElementStyleProperty,
                        new Binding { Path = new PropertyPath(TextBoxColumnEditingElementStyleProperty), Source = this }
                    );
                }

                break;
        }
    }
}
