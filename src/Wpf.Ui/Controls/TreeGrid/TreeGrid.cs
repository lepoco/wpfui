// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.ObjectModel;
using System.Collections.Specialized;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// TODO: Work in progress.
/// </summary>
public class TreeGrid : System.Windows.Controls.Primitives.Selector
{
    /// <summary>Identifies the <see cref="Headers"/> dependency property.</summary>
    public static readonly DependencyProperty HeadersProperty = DependencyProperty.Register(
        nameof(Headers),
        typeof(ObservableCollection<TreeGridHeader>),
        typeof(TreeGrid),
        new PropertyMetadata(null)
    );

    /*
    /// <summary>
    /// Property for <see cref="Content"/>.
    /// </summary>
    public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(nameof(Content),
        typeof(object), typeof(TreeGrid), new PropertyMetadata(null, OnContentChanged));
    */

    /// <summary>
    /// Gets or sets the data used to generate the child elements of this control.
    /// </summary>
    [Bindable(true)]
    public ObservableCollection<TreeGridHeader>? Headers
    {
        get => GetValue(HeadersProperty) as ObservableCollection<TreeGridHeader>;
        set => SetValue(HeadersProperty, value);
    }

    /*/// <summary>
    /// Content is the data used to generate the child elements of this control.
    /// </summary>
    [Bindable(true)]
    public object Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }*/

    public TreeGrid()
    {
        Headers = [];
        Headers.CollectionChanged += Headers_CollectionChanged;
        /*
        var x = new System.Windows.Controls.ContentControl();
        var y = new System.Windows.Controls.ItemsControl();
        var z = new System.Windows.Controls.ListBox();
        */
    }

    private void Headers_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        throw new NotImplementedException();
    }

    /*/// <summary>
    ///  Add an object child to this control
    /// </summary>
    void IAddChild.AddChild(object value)
    {
        AddChild(value);
    }

    public void AddText(string text)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///  Add an object child to this control
    /// </summary>
    protected virtual void AddChild(object value)
    {
        // if conent is the first child or being cleared, set directly
        if (Content == null || value == null)
            Content = value;
        else
            throw new InvalidOperationException($"{typeof(TreeGrid)} cannot have multiple content");
    }*/

    protected virtual void OnHeadersChanged(DependencyPropertyChangedEventArgs e)
    {
        // Headers changed
    }

    /*
    protected virtual void OnContentChanged(DependencyPropertyChangedEventArgs e)
    {
        // Content changed
    }
    */

    /*
    private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not TreeGrid treeGrid)
        {
            return;
        }

        treeGrid.OnContentChanged(e);
    }
    */
}
