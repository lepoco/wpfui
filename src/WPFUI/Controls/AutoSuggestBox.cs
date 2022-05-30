// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace WPFUI.Controls;

/// <summary>
/// Represents a text control that makes suggestions to users as they enter text using a keyboard.
/// </summary>
[TemplatePart(Name = "PART_Popup", Type = typeof(System.Windows.Controls.Primitives.Popup))]
[TemplatePart(Name = "PART_SuggestionsPresenter", Type = typeof(System.Windows.Controls.ListView))]
public class AutoSuggestBox : WPFUI.Controls.TextBox
{
    /// <summary>
    /// The current text in <see cref="System.Windows.Controls.TextBox.Text"/> used for validation purposes.
    /// </summary>
    private string _currentText;

    /// <summary>
    /// Template element represented by the <c>PART_Popup</c> name.
    /// </summary>
    private const string ElementPopup = "PART_Popup";

    /// <summary>
    /// Template element represented by the <c>PART_SuggestionsPresenter</c> name.
    /// </summary>
    private const string ElementSuggestionsPresenter = "PART_SuggestionsPresenter";

    /// <summary>
    /// Popup with suggestions.
    /// </summary>
    protected Popup Popup { get; private set; }

    /// <summary>
    /// List of suggestions inside <see cref="Popup"/>.
    /// </summary>
    protected ListView SuggestionsPresenter { get; private set; }

    /// <summary>
    /// Property for <see cref="ItemsSource"/>.
    /// </summary>
    public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource),
        typeof(IEnumerable<string>), typeof(AutoSuggestBox),
        new PropertyMetadata((IEnumerable<string>)null, OnItemsSourceChanged));

    /// <summary>
    /// Property for <see cref="FilteredItemsSource"/>.
    /// </summary>
    public static readonly DependencyProperty FilteredItemsSourceProperty = DependencyProperty.Register(nameof(FilteredItemsSource),
        typeof(IEnumerable<string>), typeof(AutoSuggestBox),
        new PropertyMetadata((IEnumerable<string>)null));

    /// <summary>
    /// Property for <see cref="IsSuggestionListOpen"/>.
    /// </summary>
    public static readonly DependencyProperty IsSuggestionListOpenProperty = DependencyProperty.Register(nameof(IsSuggestionListOpen),
        typeof(bool), typeof(AutoSuggestBox),
        new PropertyMetadata(false));

    /// <summary>
    /// Property for <see cref="MaxDropDownHeight"/>.
    /// </summary>
    public static readonly DependencyProperty MaxDropDownHeightProperty = DependencyProperty.Register(nameof(MaxDropDownHeight),
        typeof(double), typeof(AutoSuggestBox),
        new PropertyMetadata(240d));

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
    public IEnumerable<string> ItemsSource
    {
        get => (IEnumerable<string>)GetValue(ItemsSourceProperty);
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
    public IEnumerable<string> FilteredItemsSource
    {
        get => (IEnumerable<string>)GetValue(FilteredItemsSourceProperty);
        private set
        {
            if (value == null)
                ClearValue(FilteredItemsSourceProperty);
            else
                SetValue(FilteredItemsSourceProperty, value);
        }
    }

    /// <summary>
    /// Gets or sets a value representing whether the suggestion list should be opened.
    /// </summary>
    public bool IsSuggestionListOpen
    {
        get => (bool)GetValue(IsSuggestionListOpenProperty);
        set => SetValue(IsSuggestionListOpenProperty, value);
    }

    /// <summary>
    /// Gets or sets the maximum height of the drop-down list with suggestions.
    /// </summary>
    public double MaxDropDownHeight
    {
        get => (double)GetValue(MaxDropDownHeightProperty);
        set => SetValue(MaxDropDownHeightProperty, value);
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

    /// <summary>
    /// Invoked whenever application code or an internal process,
    /// such as a rebuilding layout pass, calls the ApplyTemplate method.
    /// </summary>
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        Popup = GetTemplateChild(ElementPopup) as Popup;
        SuggestionsPresenter = GetTemplateChild(ElementSuggestionsPresenter) as ListView;

        if (SuggestionsPresenter == null)
            return;

        SuggestionsPresenter.SelectionChanged += OnSuggestionsPresenterSelectionChanged;
        SuggestionsPresenter.LostFocus += OnSuggestionsPresenterLostFocus;
    }

    /// <inheritdoc />
    protected override void OnTextChanged(TextChangedEventArgs e)
    {
        base.OnTextChanged(e);

        if (ItemsSource == null || !ItemsSource.Any())
            return;

        var newText = Text;

        if (_currentText == newText)
            return;

        if (String.IsNullOrEmpty(newText))
        {
            FilteredItemsSource = ItemsSource;
        }
        else
        {
            var formattedNewText = newText.ToLower();

            FilteredItemsSource = ItemsSource.Where(elem => elem.ToLower().Contains(formattedNewText)).ToArray();
        }

        OnQuerySubmitted();

        IsSuggestionListOpen = true;
    }

    /// <inheritdoc />
    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.Key == Key.Down && Popup != null && SuggestionsPresenter != null && Popup.IsOpen)
        {
            SuggestionsPresenter.Focus();

            e.Handled = true;

            return;
        }

        base.OnKeyDown(e);
    }

    /// <summary>
    /// This virtual method is called after presenter containing suggestion loses focus.
    /// </summary>
    protected virtual void OnSuggestionsPresenterLostFocus(object sender, RoutedEventArgs e)
    {
        if (!IsFocused)
            IsSuggestionListOpen = false;
    }

    /// <summary>
    /// This virtual method is called after one of the suggestion is selected.
    /// </summary>
    protected virtual void OnSuggestionsPresenterSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is not ListView listView)
            return;

        var selected = listView.SelectedItem;

        listView.UnselectAll();

        _currentText = selected?.ToString() ?? String.Empty;

        Text = _currentText;
        CaretIndex = _currentText.Length;
        IsSuggestionListOpen = false;

        Focus();

        OnSuggestionChosen();
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
    protected virtual void OnItemsSourceChanged(IEnumerable<string> itemsSource)
    {
        FilteredItemsSource = itemsSource;
    }

    private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not AutoSuggestBox autoSuggestBox)
            return;

        autoSuggestBox.OnItemsSourceChanged(e.NewValue as IEnumerable<string>);
    }
}

