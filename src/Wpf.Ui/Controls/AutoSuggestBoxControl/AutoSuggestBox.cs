// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using Wpf.Ui.Common;
using Wpf.Ui.Controls.IconElements;
using Wpf.Ui.Interop;

namespace Wpf.Ui.Controls.AutoSuggestBoxControl;

[ToolboxItem(true)]
[ToolboxBitmap(typeof(AutoSuggestBox), "AutoSuggestBox.bmp")]
[TemplatePart(Name = ElementTextBox, Type = typeof(TextBox))]
[TemplatePart(Name = ElementSuggestionsPopup, Type = typeof(Popup))]
[TemplatePart(Name = ElementSuggestionsList, Type = typeof(ListView))]
public class AutoSuggestBox : System.Windows.Controls.ItemsControl
{
    protected const string ElementTextBox = "PART_TextBox";
    protected const string ElementSuggestionsPopup = "PART_SuggestionsPopup";
    protected const string ElementSuggestionsList = "PART_SuggestionsList";

    #region Static properties

    /// <summary>
    /// Property for <see cref="OriginalItemsSource"/>.
    /// </summary>
    public static readonly DependencyProperty OriginalItemsSourceProperty =
        DependencyProperty.Register(nameof(OriginalItemsSource), typeof(IList), typeof(AutoSuggestBox),
            new PropertyMetadata(Array.Empty<object>()));

    /// <summary>
    /// Property for <see cref="IsSuggestionListOpen"/>.
    /// </summary>
    public static readonly DependencyProperty IsSuggestionListOpenProperty =
        DependencyProperty.Register(nameof(IsSuggestionListOpen), typeof(bool), typeof(AutoSuggestBox),
            new PropertyMetadata(false));

    /// <summary>
    /// Property for <see cref="Text"/>.
    /// </summary>
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(AutoSuggestBox),
        new PropertyMetadata(string.Empty, TextPropertyChangedCallback));

    /// <summary>
    /// Property for <see cref="PlaceholderText"/>.
    /// </summary>
    public static readonly DependencyProperty PlaceholderTextProperty = DependencyProperty.Register(nameof(PlaceholderText), typeof(string), typeof(AutoSuggestBox),
        new PropertyMetadata(string.Empty));

    /// <summary>
    /// Property for <see cref="UpdateTextOnSelect"/>.
    /// </summary>
    public static readonly DependencyProperty UpdateTextOnSelectProperty = DependencyProperty.Register(nameof(UpdateTextOnSelect), typeof(bool), typeof(AutoSuggestBox),
        new PropertyMetadata(true));

    /// <summary>
    /// Property for <see cref="MaxSuggestionListHeight"/>.
    /// </summary>
    public static readonly DependencyProperty MaxSuggestionListHeightProperty = DependencyProperty.Register(nameof(MaxSuggestionListHeight), typeof(double), typeof(AutoSuggestBox),
        new PropertyMetadata(0d));

    /// <summary>
    /// Property for <see cref="Icon"/>.
    /// </summary>
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon),
        typeof(IconElement), typeof(AutoSuggestBox),
        new PropertyMetadata(null));

    /// <summary>
    /// Property for <see cref="FocusCommand"/>.
    /// </summary>
    public static readonly DependencyProperty FocusCommandProperty =
        DependencyProperty.Register(nameof(FocusCommand), typeof(ICommand), typeof(AutoSuggestBox),
            new PropertyMetadata(null));

    #endregion

    #region Properties

    /// <summary>
    /// Set your items here if you want to use the default filtering
    /// </summary>
    public IList OriginalItemsSource
    {
        get => (IList)GetValue(OriginalItemsSourceProperty);
        set => SetValue(OriginalItemsSourceProperty, value);
    }

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
    /// Gets or sets a value indicating whether items in the view will trigger an update of the editable text part of the <see cref="AutoSuggestBox"/> when clicked.
    /// </summary>
    public bool UpdateTextOnSelect
    {
        get => (bool)GetValue(UpdateTextOnSelectProperty);
        set => SetValue(UpdateTextOnSelectProperty, value);
    }

    /// <summary>
    /// Gets or sets displayed <see cref="IconElement"/>.
    /// </summary>
    public IconElement? Icon
    {
        get => (IconElement)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <summary>
    /// Used for focusing control
    /// </summary>
    public ICommand FocusCommand => (ICommand)GetValue(FocusCommandProperty);

    #endregion

    #region Events

    /// <summary>
    /// Routed event for <see cref="QuerySubmitted"/>.
    /// </summary>
    public static readonly RoutedEvent QuerySubmittedEvent = EventManager.RegisterRoutedEvent(
        nameof(QuerySubmitted), RoutingStrategy.Bubble, typeof(TypedEventHandler<AutoSuggestBox, AutoSuggestBoxQuerySubmittedEventArgs>), typeof(AutoSuggestBox));

    /// <summary>
    /// Routed event for <see cref="SuggestionChosen"/>.
    /// </summary>
    public static readonly RoutedEvent SuggestionChosenEvent = EventManager.RegisterRoutedEvent(
        nameof(SuggestionChosen), RoutingStrategy.Bubble, typeof(TypedEventHandler<AutoSuggestBox, AutoSuggestBoxSuggestionChosenEventArgs>), typeof(AutoSuggestBox));

    /// <summary>
    /// Routed event for <see cref="TextChanged"/>.
    /// </summary>
    public static readonly RoutedEvent TextChangedEvent = EventManager.RegisterRoutedEvent(
        nameof(TextChanged), RoutingStrategy.Bubble, typeof(TypedEventHandler<AutoSuggestBox, AutoSuggestBoxTextChangedEventArgs>), typeof(AutoSuggestBox));

    /// <summary>
    /// Occurs when the user submits a search query.
    /// </summary>
    public event TypedEventHandler<AutoSuggestBox, AutoSuggestBoxQuerySubmittedEventArgs> QuerySubmitted
    {
        add => AddHandler(QuerySubmittedEvent, value);
        remove => RemoveHandler(QuerySubmittedEvent, value);
    }

    /// <summary>
    /// Event occurs when the user selects an item from the recommended ones.
    /// </summary>
    public event TypedEventHandler<AutoSuggestBox, AutoSuggestBoxSuggestionChosenEventArgs> SuggestionChosen
    {
        add => AddHandler(SuggestionChosenEvent, value);
        remove => RemoveHandler(SuggestionChosenEvent, value);
    }

    /// <summary>
    /// Raised after the text content of the editable control component is updated.
    /// </summary>
    public event TypedEventHandler<AutoSuggestBox, AutoSuggestBoxTextChangedEventArgs> TextChanged
    {
        add => AddHandler(TextChangedEvent, value);
        remove => RemoveHandler(TextChangedEvent, value);
    }

    #endregion

    protected TextBox TextBox = null!;
    protected Popup SuggestionsPopup = null!;
    protected ListView SuggestionsList = null!;

    private bool _changingTextAfterSuggestionChosen;
    private bool _isChangedTextOutSideOfTextBox;
    
    private object? _selectedItem;

    public AutoSuggestBox()
    {
        Unloaded += static (sender, _) =>
        {
            var self = (AutoSuggestBox)sender;

            self.ReleaseTemplateResources();
        };

        SetValue(FocusCommandProperty, new RelayCommand<object>(_ => Focus()));
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        TextBox = GetTemplateChild<TextBox>(ElementTextBox);
        SuggestionsPopup = GetTemplateChild<Popup>(ElementSuggestionsPopup);
        SuggestionsList = GetTemplateChild<ListView>(ElementSuggestionsList);

        TextBox.PreviewKeyDown += TextBoxOnPreviewKeyDown;
        TextBox.TextChanged += TextBoxOnTextChanged;
        TextBox.LostKeyboardFocus += TextBoxOnLostKeyboardFocus;

        SuggestionsList.SelectionChanged += SuggestionsListOnSelectionChanged;
        SuggestionsList.PreviewKeyDown += SuggestionsListOnPreviewKeyDown;
        SuggestionsList.LostKeyboardFocus += SuggestionsListOnLostKeyboardFocus;
        SuggestionsList.PreviewMouseLeftButtonUp += SuggestionsListOnPreviewMouseLeftButtonUp;

        var hwnd = (HwndSource)PresentationSource.FromVisual(this)!;
        hwnd.AddHook(Hook);
    }

    /// <inheritdoc cref="UIElement.Focus" />
    public new void Focus()
    {
        TextBox.Focus();
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

        if (PresentationSource.FromVisual(this) is HwndSource source)
            source.RemoveHook(Hook);
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

        if (args is { Handled: false, Reason: AutoSuggestionBoxTextChangeReason.UserInput })
            DefaultFiltering(text);
    }

    #endregion

    #region TextBox events

    private void TextBoxOnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key is Key.Escape)
        {
            IsSuggestionListOpen = false;
            return;
        }

        if (e.Key is Key.Enter)
        {
            IsSuggestionListOpen = false;
            OnQuerySubmitted(TextBox.Text);
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
    }

    private void TextBoxOnTextChanged(object sender, TextChangedEventArgs e)
    {
        var changeReason = AutoSuggestionBoxTextChangeReason.UserInput;

        if (_changingTextAfterSuggestionChosen)
            changeReason = AutoSuggestionBoxTextChangeReason.SuggestionChosen;

        if (_isChangedTextOutSideOfTextBox)
            changeReason = AutoSuggestionBoxTextChangeReason.ProgrammaticChange;

        OnTextChanged(changeReason, TextBox.Text);

        SuggestionsList.SelectedItem = null;

        if (changeReason is not AutoSuggestionBoxTextChangeReason.UserInput)
            return;

        IsSuggestionListOpen = true;
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

        IsSuggestionListOpen = false;
        OnSelectedChanged(SuggestionsList.SelectedItem);
    }

    private void SuggestionsListOnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (SuggestionsList.SelectedItem is not null)
            return;

        IsSuggestionListOpen = false;

        if (_selectedItem is not null)
            OnSuggestionChosen(_selectedItem);
    }

    private void SuggestionsListOnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (SuggestionsList.SelectedItem is null)
            return;

        OnSelectedChanged(SuggestionsList.SelectedItem);
    }

    #endregion

    private IntPtr Hook(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
    {
        if (!IsSuggestionListOpen)
            return IntPtr.Zero;

        var message = (User32.WM)msg;

        if (message is User32.WM.NCACTIVATE or User32.WM.WINDOWPOSCHANGED)
            IsSuggestionListOpen = false;

        return IntPtr.Zero;
    }

    private void OnSelectedChanged(object selectedObj)
    {
        OnSuggestionChosen(selectedObj);

        _selectedItem = selectedObj;
    }

    private void UpdateTexBoxTextAfterSelection(object selectedObj)
    {
        _changingTextAfterSuggestionChosen = true;

        TextBox.Text = GetStringFromObj(selectedObj);
        _changingTextAfterSuggestionChosen = false;
    }

    private void DefaultFiltering(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            ItemsSource = OriginalItemsSource;
            return;
        }

        var suitableItems = new List<object>();
        var splitText = text.ToLower().Split(' ');

        for (var i = 0; i < OriginalItemsSource.Count; i++)
        {
            var item = OriginalItemsSource[i];
            var itemText = GetStringFromObj(item);

            var found = splitText.All(key=> itemText.ToLower().Contains(key));

            if (found)
                suitableItems.Add(item);
        }

        ItemsSource = suitableItems;
    }

    private string GetStringFromObj(object obj)
    {
        string text = string.Empty;

        if (!string.IsNullOrEmpty(DisplayMemberPath))
        {
            //Maybe it needs some optimization?
            if (obj.GetType().GetProperty(DisplayMemberPath)?.GetValue(obj) is string value)
                text = value;
        }

        if (string.IsNullOrEmpty(text))
            text = obj as string ?? obj.ToString();

        return text;
    }

    private static void TextPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var self = (AutoSuggestBox)d;
        var newText = (string)e.NewValue;

        if (self.TextBox.Text == newText)
            return;

        self._isChangedTextOutSideOfTextBox = true;
        self.TextBox.Text = newText;
        self._isChangedTextOutSideOfTextBox = false;
    }
}
