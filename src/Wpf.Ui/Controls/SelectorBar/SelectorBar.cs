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
    protected override void OnGotFocus(RoutedEventArgs e)
    {
        base.OnGotFocus(e);

        // Mirror WinUI 3 SelectorBar::OnGotFocus: when focus arrives with no
        // selection (or a non-focusable selected item), select the first
        // focusable item so the pill appears and SelectionChanged fires.
        // Without this, Tab-focusing an uninitialized SelectorBar
        // (SelectedIndex == -1) leaves no selection and raises no event.
        if (SelectedIndex >= 0 && IsSelectableIndex(SelectedIndex))
        {
            return;
        }

        for (int i = 0; i < Items.Count; i++)
        {
            if (IsSelectableIndex(i))
            {
                SetCurrentValue(SelectedIndexProperty, i);
                break;
            }
        }
    }

    /// <inheritdoc/>
    protected override void OnKeyDown(KeyEventArgs e)
    {
        // Left/Right pass a visual direction; MoveSelection inverts it for RightToLeft.
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

        // `step` is the visual direction (+1 = right, -1 = left). In RightToLeft
        // the items are visually reversed, so the index delta is inverted while
        // the no-selection fallback stays anchored to the visual start/end.
        int indexDelta = FlowDirection == FlowDirection.RightToLeft ? -step : step;

        int startIndex = SelectedIndex;
        int candidateIndex = startIndex < 0
            ? (step > 0 ? 0 : Items.Count - 1)
            : startIndex + indexDelta;

        while (candidateIndex >= 0 && candidateIndex < Items.Count)
        {
            if (TrySelectItem(candidateIndex))
            {
                return true;
            }

            candidateIndex += indexDelta;
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
            return IsFocusable(directItem);
        }

        var container = ItemContainerGenerator.ContainerFromIndex(index) as SelectorBarItem;

        // An ungenerated container (e.g. before layout) is treated as selectable
        // so first-focus selection isn't blocked while items are still realizing.
        return container is null || IsFocusable(container);
    }

    // Mirrors WinUI's focusability criteria (Visibility + IsEnabled + IsTabStop).
    // In WPF, Focusable is the focus gate (WinUI's IsTabStop equivalent); WPF's
    // own IsTabStop only governs tab order, so it is intentionally not required.
    private static bool IsFocusable(SelectorBarItem? item)
    {
        return item is not null
            && item.IsEnabled
            && item.Focusable
            && item.IsVisible;
    }

    private void FocusContainer(int index)
    {
        if (ItemContainerGenerator.ContainerFromIndex(index) is SelectorBarItem container)
        {
            _ = container.Focus();
        }
    }
}
