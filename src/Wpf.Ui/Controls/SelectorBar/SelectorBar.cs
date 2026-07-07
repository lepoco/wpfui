// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

/* Based on Windows UI Library
   Copyright(c) Microsoft Corporation.All rights reserved. */

using System.Windows.Controls;
using System.Windows.Input;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// A control that lets a user switch between a small number of different sets or views of data.
/// One item at a time can be selected.
/// </summary>
/// <remarks>
/// <para>
/// The <see cref="SelectorBar"/> is a lightweight control that supports an icon and text.
/// It is intended to present a limited number of options and does not rearrange items to adapt to different window sizes.
/// </para>
/// <para>
/// This control mirrors the WinUI 3 <c>SelectorBar</c> control.
/// Selection is always <see cref="SelectionMode.Single"/>; attempts to change the mode are coerced back.
/// </para>
/// </remarks>
/// <example>
/// <code lang="xml">
/// &lt;ui:SelectorBar&gt;
///     &lt;ui:SelectorBarItem Text="Recent" Icon="{ui:SymbolIcon Clock24}" /&gt;
///     &lt;ui:SelectorBarItem Text="Shared" Icon="{ui:SymbolIcon Share24}" /&gt;
///     &lt;ui:SelectorBarItem Text="Favorites" Icon="{ui:SymbolIcon Star24}" /&gt;
/// &lt;/ui:SelectorBar&gt;
/// </code>
/// </example>
[StyleTypedProperty(Property = nameof(ItemContainerStyle), StyleTargetType = typeof(SelectorBarItem))]
public class SelectorBar : System.Windows.Controls.ListBox
{
    /// <summary>Identifies the <see cref="SelectionChanged"/> routed event.</summary>
    public static new readonly RoutedEvent SelectionChangedEvent = EventManager.RegisterRoutedEvent(
        nameof(SelectionChanged),
        RoutingStrategy.Bubble,
        typeof(TypedEventHandler<SelectorBar, SelectorBarSelectionChangedEventArgs>),
        typeof(SelectorBar)
    );

    static SelectorBar()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(SelectorBar),
            new FrameworkPropertyMetadata(typeof(SelectorBar))
        );

        SelectionModeProperty.OverrideMetadata(
            typeof(SelectorBar),
            new FrameworkPropertyMetadata(SelectionMode.Single, null, CoerceSelectionMode)
        );
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SelectorBar"/> class.
    /// </summary>
    public SelectorBar()
    {
        SelectionMode = SelectionMode.Single;
    }

    /// <summary>
    /// Occurs when the currently selected item changes.
    /// </summary>
    public new event TypedEventHandler<SelectorBar, SelectorBarSelectionChangedEventArgs> SelectionChanged
    {
        add => AddHandler(SelectionChangedEvent, value);
        remove => RemoveHandler(SelectionChangedEvent, value);
    }

    /// <inheritdoc/>
    protected override DependencyObject GetContainerForItemOverride() => new SelectorBarItem();

    /// <inheritdoc/>
    protected override bool IsItemItsOwnContainerOverride(object item) => item is SelectorBarItem;

    /// <inheritdoc/>
    protected override void OnSelectionChanged(SelectionChangedEventArgs e)
    {
        base.OnSelectionChanged(e);

        RaiseEvent(new SelectorBarSelectionChangedEventArgs(SelectionChangedEvent, this));
    }

    /// <inheritdoc/>
    protected override void OnKeyDown(KeyEventArgs e)
    {
        bool handled = e.Key switch
        {
            Key.Left => MoveSelection(-1),
            Key.Right => MoveSelection(1),
            Key.Home => SelectFirstItem(),
            Key.End => SelectLastItem(),
            _ => false,
        };

        if (handled)
        {
            e.Handled = true;
            return;
        }

        base.OnKeyDown(e);
    }

    private static object CoerceSelectionMode(DependencyObject d, object? baseValue) =>
        SelectionMode.Single;

    private bool MoveSelection(int step)
    {
        if (Items.Count == 0)
        {
            return false;
        }

        int startIndex = SelectedIndex;
        int candidateIndex = startIndex < 0
            ? (step > 0 ? 0 : Items.Count - 1)
            : startIndex + step;

        while (candidateIndex >= 0 && candidateIndex < Items.Count)
        {
            if (TrySelectItem(candidateIndex))
            {
                return true;
            }

            candidateIndex += step;
        }

        return false;
    }

    private bool SelectFirstItem()
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (TrySelectItem(i))
            {
                return true;
            }
        }

        return false;
    }

    private bool SelectLastItem()
    {
        for (int i = Items.Count - 1; i >= 0; i--)
        {
            if (TrySelectItem(i))
            {
                return true;
            }
        }

        return false;
    }

    private bool TrySelectItem(int index)
    {
        if (!IsSelectableIndex(index))
        {
            return false;
        }

        if (SelectedIndex == index)
        {
            return true;
        }

        SetCurrentValue(SelectedIndexProperty, index);
        FocusContainer(index);

        return true;
    }

    private bool IsSelectableIndex(int index)
    {
        if (Items[index] is SelectorBarItem directItem)
        {
            return directItem.IsEnabled;
        }

        var container = ItemContainerGenerator.ContainerFromIndex(index) as SelectorBarItem;

        return container?.IsEnabled ?? true;
    }

    private void FocusContainer(int index)
    {
        if (ItemContainerGenerator.ContainerFromIndex(index) is SelectorBarItem container)
        {
            _ = container.Focus();
        }
    }
}
