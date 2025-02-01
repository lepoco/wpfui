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
[StyleTypedProperty(Property = nameof(ComboBoxColumnElementStyle), StyleTargetType = typeof(ComboBox))]
[StyleTypedProperty(Property = nameof(ComboBoxColumnEditingElementStyle), StyleTargetType = typeof(ComboBox))]
[StyleTypedProperty(Property = nameof(TextColumnElementStyle), StyleTargetType = typeof(TextBlock))]
[StyleTypedProperty(Property = nameof(TextColumnEditingElementStyle), StyleTargetType = typeof(TextBox))]
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

    /// <summary>Identifies the <see cref="ComboBoxColumnElementStyle"/> dependency property.</summary>
    public static readonly DependencyProperty ComboBoxColumnElementStyleProperty =
        DependencyProperty.Register(
            nameof(ComboBoxColumnElementStyle),
            typeof(Style),
            typeof(DataGrid),
            new FrameworkPropertyMetadata(null)
        );

    /// <summary>Identifies the <see cref="ComboBoxColumnEditingElementStyle"/> dependency property.</summary>
    public static readonly DependencyProperty ComboBoxColumnEditingElementStyleProperty =
        DependencyProperty.Register(
            nameof(ComboBoxColumnEditingElementStyle),
            typeof(Style),
            typeof(DataGrid),
            new FrameworkPropertyMetadata(null)
        );

    /// <summary>Identifies the <see cref="TextColumnElementStyle"/> dependency property.</summary>
    public static readonly DependencyProperty TextColumnElementStyleProperty =
        DependencyProperty.Register(
            nameof(TextColumnElementStyle),
            typeof(Style),
            typeof(DataGrid),
            new FrameworkPropertyMetadata(null)
        );

    /// <summary>Identifies the <see cref="TextColumnEditingElementStyle"/> dependency property.</summary>
    public static readonly DependencyProperty TextColumnEditingElementStyleProperty =
        DependencyProperty.Register(
            nameof(TextColumnEditingElementStyle),
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
    /// Gets or sets the style which is applied to all combobox column in the DataGrid
    /// </summary>
    public Style? ComboBoxColumnElementStyle
    {
        get => (Style?)GetValue(ComboBoxColumnElementStyleProperty);
        set => SetValue(ComboBoxColumnElementStyleProperty, value);
    }

    /// <summary>
    /// Gets or sets the style for all the column comboboxes in the DataGrid
    /// </summary>
    public Style? ComboBoxColumnEditingElementStyle
    {
        get => (Style?)GetValue(ComboBoxColumnEditingElementStyleProperty);
        set => SetValue(ComboBoxColumnEditingElementStyleProperty, value);
    }

    /// <summary>
    /// Gets or sets the style which is applied to all textbox column in the DataGrid
    /// </summary>
    public Style? TextColumnElementStyle
    {
        get => (Style?)GetValue(TextColumnElementStyleProperty);
        set => SetValue(TextColumnElementStyleProperty, value);
    }

    /// <summary>
    /// Gets or sets the style for all the column textboxes in the DataGrid
    /// </summary>
    public Style? TextColumnEditingElementStyle
    {
        get => (Style?)GetValue(TextColumnEditingElementStyleProperty);
        set => SetValue(TextColumnEditingElementStyleProperty, value);
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

            case DataGridComboBoxColumn comboBoxColumn:
                if (
                    comboBoxColumn.ReadLocalValue(DataGridBoundColumn.ElementStyleProperty)
                    == DependencyProperty.UnsetValue
                )
                {
                    _ = BindingOperations.SetBinding(
                        comboBoxColumn,
                        DataGridBoundColumn.ElementStyleProperty,
                        new Binding { Path = new PropertyPath(ComboBoxColumnElementStyleProperty), Source = this }
                    );
                }

                if (
                    comboBoxColumn.ReadLocalValue(DataGridBoundColumn.EditingElementStyleProperty)
                    == DependencyProperty.UnsetValue
                )
                {
                    _ = BindingOperations.SetBinding(
                        comboBoxColumn,
                        DataGridBoundColumn.EditingElementStyleProperty,
                        new Binding
                        {
                            Path = new PropertyPath(ComboBoxColumnEditingElementStyleProperty), Source = this
                        }
                    );
                }

                if (
                    comboBoxColumn.ReadLocalValue(DataGridBoundColumn.EditingElementStyleProperty)
                    == DependencyProperty.UnsetValue
                )
                {
                    _ = BindingOperations.SetBinding(
                        comboBoxColumn,
                        DataGridBoundColumn.EditingElementStyleProperty,
                        new Binding
                        {
                            Path = new PropertyPath(ComboBoxColumnEditingElementStyleProperty), Source = this
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
                        new Binding { Path = new PropertyPath(TextColumnElementStyleProperty), Source = this }
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
                        new Binding { Path = new PropertyPath(TextColumnEditingElementStyleProperty), Source = this }
                    );
                }

                break;
        }
    }
}
