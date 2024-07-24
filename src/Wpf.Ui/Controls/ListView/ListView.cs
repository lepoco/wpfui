// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Controls;

/// <summary>
/// Extends <see cref="System.Windows.Controls.ListView"/>, and adds customized support <see cref="ListViewViewState.GridView"/> or <see cref="ListViewViewState.Default"/>.
/// </summary>
/// <example>
/// <code lang="xml">
/// &lt;ui:ListView ItemsSource="{Binding ...}" &gt;
///     &lt;ui:ListView.View&gt;
///         &lt;ui:GridView&gt;
///             &lt;GridViewColumn
///                 DisplayMemberBinding="{Binding FirstName}"
///                 Header="First Name" /&gt;
///             &lt;GridViewColumn
///                 DisplayMemberBinding="{Binding LastName}"
///                 Header="Last Name" /&gt;
///         &lt;/ui:GridView&gt;
///     &lt;/ui:ListView.View&gt;
/// &lt;/ui:ListView&gt;
/// </code>
/// </example>
public class ListView : System.Windows.Controls.ListView
{
    /// <summary>Identifies the <see cref="ViewState"/> dependency property.</summary>
    public static readonly DependencyProperty ViewStateProperty = DependencyProperty.Register(
        nameof(ViewState),
        typeof(ListViewViewState),
        typeof(ListView),
        new FrameworkPropertyMetadata(ListViewViewState.Default, OnViewStateChanged)
    );

    /// <summary>
    /// Gets or sets the view state of the <see cref="ListView"/>, enabling custom logic based on the current view.
    /// </summary>
    /// <value>The current view state of the <see cref="ListView"/>.</value>
    public ListViewViewState ViewState
    {
        get => (ListViewViewState)GetValue(ViewStateProperty);
        set => SetValue(ViewStateProperty, value);
    }

    private static void OnViewStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not ListView self)
        {
            return;
        }

        self.OnViewStateChanged(e);
    }

    protected virtual void OnViewStateChanged(DependencyPropertyChangedEventArgs e)
    {
        // Hook for derived classes to react to ViewState property changes
    }

    public ListView()
    {
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        Loaded -= OnLoaded; // prevent memory leaks

        // Setup initial ViewState and hook into View property changes
        var descriptor = DependencyPropertyDescriptor.FromProperty(
            System.Windows.Controls.ListView.ViewProperty,
            typeof(System.Windows.Controls.ListView)
        );
        descriptor?.AddValueChanged(this, OnViewPropertyChanged);
        UpdateViewState(); // set the initial state
    }

    private void OnViewPropertyChanged(object? sender, EventArgs e)
    {
        UpdateViewState();
    }

    private void UpdateViewState()
    {
        ListViewViewState viewState = View switch
        {
            System.Windows.Controls.GridView => ListViewViewState.GridView,
            null => ListViewViewState.Default,
            _ => ListViewViewState.Default
        };

        SetCurrentValue(ViewStateProperty, viewState);
    }

    static ListView()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(ListView),
            new FrameworkPropertyMetadata(typeof(ListView))
        );
    }

    protected override DependencyObject GetContainerForItemOverride()
    {
        return new ListViewItem();
    }

    protected override bool IsItemItsOwnContainerOverride(object item)
    {
        return item is ListViewItem;
    }
}
