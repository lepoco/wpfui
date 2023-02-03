// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Wpf.Ui.Common;

namespace Wpf.Ui.Controls;

[TemplatePart(Name = ElementTextBox, Type = typeof(System.Windows.Controls.TextBox))]
[TemplatePart(Name = ElementSuggestionsPopup, Type = typeof(System.Windows.Controls.Primitives.Popup))]
[TemplatePart(Name = ElementSuggestionsList, Type = typeof(System.Windows.Controls.ListView))]
public class NewAutoSuggestBox : System.Windows.Controls.ItemsControl
{
    protected const string ElementTextBox = "PART_TextBox";
    protected const string ElementSuggestionsPopup = "PART_SuggestionsPopup";
    protected const string ElementSuggestionsList = "PART_SuggestionsList";

    #region Static properties

    /// <summary>
    /// Property for <see cref="IsSuggestionListOpen"/>.
    /// </summary>
    public static readonly DependencyProperty IsSuggestionListOpenProperty =
        DependencyProperty.Register(nameof(IsSuggestionListOpen), typeof(bool), typeof(NewAutoSuggestBox),
            new PropertyMetadata(false));

    /// <summary>
    /// Property for <see cref="Text"/>.
    /// </summary>
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(NewAutoSuggestBox),
        new PropertyMetadata(string.Empty, TextPropertyChangedCallback));

    /// <summary>
    /// Property for <see cref="PlaceholderText"/>.
    /// </summary>
    public static readonly DependencyProperty PlaceholderTextProperty = DependencyProperty.Register(nameof(PlaceholderText), typeof(string), typeof(NewAutoSuggestBox),
        new PropertyMetadata(string.Empty));

    /// <summary>
    /// Property for <see cref="UpdateTextOnSelect"/>.
    /// </summary>
    public static readonly DependencyProperty UpdateTextOnSelectProperty = DependencyProperty.Register(nameof(UpdateTextOnSelect), typeof(bool), typeof(NewAutoSuggestBox),
        new PropertyMetadata(true));

    /// <summary>
    /// Property for <see cref="MaxSuggestionListHeight"/>.
    /// </summary>
    public static readonly DependencyProperty MaxSuggestionListHeightProperty = DependencyProperty.Register(nameof(MaxSuggestionListHeight), typeof(double), typeof(NewAutoSuggestBox),
        new PropertyMetadata(0d));

    /// <summary>
    /// Property for <see cref="Icon"/>.
    /// </summary>
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon), typeof(SymbolRegular), typeof(NewAutoSuggestBox),
        new PropertyMetadata(SymbolRegular.Empty));

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets a Boolean value indicating whether the drop-down portion of the <see cref="AutoSuggestBox"/> is open.
    /// </summary>
    public bool IsSuggestionListOpen
    {
        get => (bool)GetValue(IsSuggestionListOpenProperty);
        set => SetValue(IsSuggestionListOpenProperty, value);
    }

    /// <summary>
    /// Gets or sets the text that is shown in the control
    /// </summary>
    /// <remarks>
    /// This property is not typically set in XAML.
    /// </remarks>
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>
    /// Gets or sets the placeholder text to be displayed in the control.
    /// </summary>
    /// <remarks>
    /// The placeholder text to be displayed in the control. The default is an empty string.
    /// </remarks>
    public string PlaceholderText
    {
        get => (string)GetValue(PlaceholderTextProperty);
        set => SetValue(PlaceholderTextProperty, value);
    }

    /// <summary>
    /// Gets or set the maximum height for the drop-down portion of the <see cref="AutoSuggestBox"/> control.
    /// </summary>
    public double MaxSuggestionListHeight
    {
        get => (double)GetValue(MaxSuggestionListHeightProperty);
        set => SetValue(MaxSuggestionListHeightProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether items in the view will trigger an update of the editable text part of the <see cref="NewAutoSuggestBox"/> when clicked.
    /// </summary>
    public bool UpdateTextOnSelect
    {
        get => (bool)GetValue(UpdateTextOnSelectProperty);
        set => SetValue(UpdateTextOnSelectProperty, value);
    }

    /// <summary>
    /// Gets or sets displayed <see cref="Common.SymbolRegular"/>.
    /// </summary>
    public SymbolRegular Icon
    {
        get => (SymbolRegular)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    #endregion

    #region Events

    /// <summary>
    /// Routed event for <see cref="QuerySubmitted"/>.
    /// </summary>
    public static readonly RoutedEvent QuerySubmittedEvent = EventManager.RegisterRoutedEvent(
        nameof(QuerySubmitted), RoutingStrategy.Bubble, typeof(TypedEventHandler<NewAutoSuggestBox, AutoSuggestBoxQuerySubmittedEventArgs>), typeof(NewAutoSuggestBox));

    /// <summary>
    /// Routed event for <see cref="SuggestionChosen"/>.
    /// </summary>
    public static readonly RoutedEvent SuggestionChosenEvent = EventManager.RegisterRoutedEvent(
        nameof(SuggestionChosen), RoutingStrategy.Bubble, typeof(TypedEventHandler<NewAutoSuggestBox, AutoSuggestBoxSuggestionChosenEventArgs>), typeof(NewAutoSuggestBox));

    /// <summary>
    /// Routed event for <see cref="TextChanged"/>.
    /// </summary>
    public static readonly RoutedEvent TextChangedEvent = EventManager.RegisterRoutedEvent(
        nameof(TextChanged), RoutingStrategy.Bubble, typeof(TypedEventHandler<NewAutoSuggestBox, AutoSuggestBoxTextChangedEventArgs>), typeof(NewAutoSuggestBox));

    /// <summary>
    /// Occurs when the user submits a search query.
    /// </summary>
    public event TypedEventHandler<NewAutoSuggestBox, AutoSuggestBoxQuerySubmittedEventArgs> QuerySubmitted
    {
        add => AddHandler(QuerySubmittedEvent, value);
        remove => RemoveHandler(QuerySubmittedEvent, value);
    }

    /// <summary>
    /// Event occurs when the user selects an item from the recommended ones.
    /// </summary>
    public event TypedEventHandler<NewAutoSuggestBox, AutoSuggestBoxSuggestionChosenEventArgs> SuggestionChosen
    {
        add => AddHandler(SuggestionChosenEvent, value);
        remove => RemoveHandler(SuggestionChosenEvent, value);
    }

    /// <summary>
    /// Raised after the text content of the editable control component is updated.
    /// </summary>
    public event TypedEventHandler<NewAutoSuggestBox, AutoSuggestBoxTextChangedEventArgs> TextChanged
    {
        add => AddHandler(TextChangedEvent, value);
        remove => RemoveHandler(TextChangedEvent, value);
    }

    #endregion

    protected System.Windows.Controls.TextBox TextBox = null!;
    protected Popup SuggestionsPopup = null!;
    protected ListView SuggestionsList = null!;

    private bool _isTextBoxLostFocus;
    private bool _changingTextAfterSuggestionChosen;
    private bool _isUserChangedText;

    public NewAutoSuggestBox()
    {
        Unloaded += static (sender, _) =>
        {
            var self = (NewAutoSuggestBox)sender;

            self.ReleaseTemplateResources();
        };
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        TextBox = GetTemplateChild<System.Windows.Controls.TextBox>(ElementTextBox);
        SuggestionsPopup = GetTemplateChild<Popup>(ElementSuggestionsPopup);
        SuggestionsList = GetTemplateChild<ListView>(ElementSuggestionsList);

        TextBox.PreviewKeyDown += TextBoxOnPreviewKeyDown;
        TextBox.TextChanged += TextBoxOnTextChanged;
        TextBox.LostKeyboardFocus += TextBoxOnLostKeyboardFocus;

        SuggestionsList.SelectionChanged += SuggestionsListOnSelectionChanged;
        SuggestionsList.PreviewKeyDown += SuggestionsListOnPreviewKeyDown;
        SuggestionsList.LostKeyboardFocus += SuggestionsListOnLostKeyboardFocus;
        SuggestionsList.PreviewMouseLeftButtonUp += SuggestionsListOnPreviewMouseLeftButtonUp;
    }

    protected T GetTemplateChild<T>(string name) where T : DependencyObject
    {
        if (GetTemplateChild(name) is not T dependencyObject)
            throw new ArgumentNullException(name);

        return dependencyObject;
    }

    protected virtual void ReleaseTemplateResources()
    {
        TextBox.PreviewKeyDown -= TextBoxOnPreviewKeyDown;
        TextBox.TextChanged -= TextBoxOnTextChanged;
        TextBox.LostKeyboardFocus -= TextBoxOnLostKeyboardFocus;

        SuggestionsList.SelectionChanged -= SuggestionsListOnSelectionChanged;
        SuggestionsList.PreviewKeyDown -= SuggestionsListOnPreviewKeyDown;
        SuggestionsList.LostKeyboardFocus -= SuggestionsListOnLostKeyboardFocus;
        SuggestionsList.PreviewMouseLeftButtonUp -= SuggestionsListOnPreviewMouseLeftButtonUp;
    }

    #region Events

    /// <summary>
    /// Method for <see cref="QuerySubmitted"/>.
    /// </summary>
    /// <param name="queryText"></param>
    protected virtual void OnQuerySubmitted(string queryText)
    {
        var args = new AutoSuggestBoxQuerySubmittedEventArgs(QuerySubmittedEvent, this)
        {
            QueryText = queryText
        };

        RaiseEvent(args);
    }

    /// <summary>
    /// Method for <see cref="SuggestionChosen"/>.
    /// </summary>
    /// <param name="selectedItem"></param>
    protected virtual void OnSuggestionChosen(object selectedItem)
    {
        var args = new AutoSuggestBoxSuggestionChosenEventArgs(SuggestionChosenEvent, this)
        {
            SelectedItem = selectedItem
        };

        RaiseEvent(args);

        if (UpdateTextOnSelect && !args.Handled)
            UpdateTexBoxTextAfterSelection(selectedItem);
    }

    /// <summary>
    /// Method for <see cref="TextChanged"/>.
    /// </summary>
    /// <param name="reason"></param>
    /// <param name="text"></param>
    protected virtual void OnTextChanged(AutoSuggestionBoxTextChangeReason reason, string text)
    {
        var args = new AutoSuggestBoxTextChangedEventArgs(TextChangedEvent, this)
        {
            Reason = reason,
            Text = text
        };

        RaiseEvent(args);
    }

    #endregion

    #region TextBox events

    private void TextBoxOnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key is Key.Enter)
        {
            OnQuerySubmitted(TextBox.Text);
            IsSuggestionListOpen = false;
            return;
        }

        if (e.Key is not Key.Down || !IsSuggestionListOpen)
            return;

        SuggestionsList.Focus();
    }

    private void TextBoxOnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        if (e.NewFocus is ListView)
            return;

        IsSuggestionListOpen = false;
        _isTextBoxLostFocus = true;
    }

    private void TextBoxOnTextChanged(object sender, TextChangedEventArgs e)
    {
        var changeReason = AutoSuggestionBoxTextChangeReason.UserInput;

        if (_changingTextAfterSuggestionChosen)
            changeReason = AutoSuggestionBoxTextChangeReason.SuggestionChosen;

        if (_isUserChangedText)
            changeReason = AutoSuggestionBoxTextChangeReason.ProgrammaticChange;

        OnTextChanged(changeReason, TextBox.Text);

        SuggestionsList.SelectedItem = null;

        if (_isTextBoxLostFocus)
        {
            _isTextBoxLostFocus = false;
            IsSuggestionListOpen = false;
        }
        else
        {
            IsSuggestionListOpen = true;
        }
    }

    #endregion

    #region SuggestionsList events

    private void SuggestionsListOnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        if (e.NewFocus is ListViewItem)
            return;

        IsSuggestionListOpen = false;
    }

    private void SuggestionsListOnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key is not Key.Enter)
            return;

        OnSelectedChanged(SuggestionsList.SelectedItem);
        IsSuggestionListOpen = false;
    }

    private void SuggestionsListOnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (SuggestionsList.SelectedItem is not null)
            return;

        IsSuggestionListOpen = false;
    }

    private void SuggestionsListOnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (SuggestionsList.SelectedItem is null)
            return;

        OnSelectedChanged(SuggestionsList.SelectedItem);
    }

    #endregion

    private void OnSelectedChanged(object selectedObj)
    {
        OnSuggestionChosen(selectedObj);

        _isTextBoxLostFocus = false;
    }

    private void UpdateTexBoxTextAfterSelection(object selectedObj)
    {
        _changingTextAfterSuggestionChosen = true;

        string selectedObjText = selectedObj as string ?? selectedObj.ToString();
        TextBox.Text = selectedObjText;

        _changingTextAfterSuggestionChosen = false;
    }

    private static void TextPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var self = (NewAutoSuggestBox)d;
        var newText = (string)e.NewValue;

        if (self.TextBox.Text == newText)
            return;

        self._isUserChangedText = true;
        self.TextBox.Text = newText;
        self._isUserChangedText = false;
    }
}
