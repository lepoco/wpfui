// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace WPFUI.Controls;

/// <summary>
/// Represents a text control that makes suggestions to users as they enter text using a keyboard.
/// </summary>
public class AutoSuggestBox : WPFUI.Controls.TextBox
{
    /// <summary>
    /// Popup with suggestions.
    /// </summary>
    protected Popup Popup { get; private set; }

    /// <summary>
    /// Property for <see cref="ItemsSource"/>.
    /// </summary>
    public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource),
        typeof(IEnumerable), typeof(AutoSuggestBox),
        new PropertyMetadata((IEnumerable)null, OnItemsSourceChanged));

    /// <summary>
    /// Property for <see cref="FilteredItemsSource"/>.
    /// </summary>
    public static readonly DependencyProperty FilteredItemsSourceProperty = DependencyProperty.Register(nameof(FilteredItemsSource),
        typeof(IEnumerable), typeof(AutoSuggestBox),
        new PropertyMetadata((IEnumerable)null));

    /// <summary>
    /// Routed event for <see cref="QuerySubmitted"/>.
    /// </summary>
    public static readonly RoutedEvent QuerySubmittedEvent = EventManager.RegisterRoutedEvent(
        nameof(QuerySubmitted), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(AutoSuggestBox));

    /// <summary>
    /// Routed event for <see cref="SuggestionChosen"/>.
    /// </summary>
    public static readonly RoutedEvent SuggestionChosenEvent = EventManager.RegisterRoutedEvent(
        nameof(SuggestionChosen), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(AutoSuggestBox));

    /// <summary>
    /// ItemsSource specifies a collection used to generate the list of suggestions
    /// for <see cref="AutoSuggestBox"/>.
    /// </summary>
    public IEnumerable ItemsSource
    {
        get => (IEnumerable)GetValue(ItemsSourceProperty);
        set
        {
            if (value == null)
                ClearValue(ItemsSourceProperty);
            else
                SetValue(ItemsSourceProperty, value);
        }
    }

    /// <summary>
    /// Filtered <see cref="ItemsSource"/> based on provided text.
    /// </summary>
    public IEnumerable FilteredItemsSource
    {
        get => (IEnumerable)GetValue(FilteredItemsSourceProperty);
        private set
        {
            if (value == null)
                ClearValue(FilteredItemsSourceProperty);
            else
                SetValue(FilteredItemsSourceProperty, value);
        }
    }

    /// <summary>
    /// Event occurs when a user commits a query string.
    /// </summary>
    public event RoutedEventHandler QuerySubmitted
    {
        add => AddHandler(QuerySubmittedEvent, value);
        remove => RemoveHandler(QuerySubmittedEvent, value);
    }

    /// <summary>
    /// Event occurs when the user selects an item from the recommended ones.
    /// </summary>
    public event RoutedEventHandler SuggestionChosen
    {
        add => AddHandler(SuggestionChosenEvent, value);
        remove => RemoveHandler(SuggestionChosenEvent, value);
    }

    /// <inheritdoc />
    protected override void OnTemplateChanged(ControlTemplate oldTemplate, ControlTemplate newTemplate)
    {
        base.OnTemplateChanged(oldTemplate, newTemplate);
    }

    /// <inheritdoc />
    protected override void OnTextChanged(TextChangedEventArgs e)
    {
        base.OnTextChanged(e);

        var popupRaw = Template.FindName("PART_Popup", this);

        // TEST

        if (popupRaw is Popup popup)
            Popup = popup;

        if (Popup != null)
            Popup.IsOpen = true;
        // Raise popup
    }

    /// <summary>
    /// This virtual method is called after submitting a query.
    /// </summary>
    protected virtual void OnQuerySubmitted()
    {
        var newEvent = new RoutedEventArgs(QuerySubmittedEvent, this);
        RaiseEvent(newEvent);
    }

    /// <summary>
    /// This virtual method is called after selecting a suggestion.
    /// </summary>
    protected virtual void OnSuggestionChosen()
    {
        var newEvent = new RoutedEventArgs(SuggestionChosenEvent, this);
        RaiseEvent(newEvent);
    }

    /// <summary>
    /// This virtual method is called after <see cref="ItemsSource"/> is changed.
    /// </summary>
    protected virtual void OnItemsSourceChanged(IEnumerable itemsSource)
    {
        FilteredItemsSource = itemsSource;
    }

    private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not AutoSuggestBox autoSuggestBox)
            return;

        autoSuggestBox.OnItemsSourceChanged(e.NewValue as IEnumerable);
    }
}

