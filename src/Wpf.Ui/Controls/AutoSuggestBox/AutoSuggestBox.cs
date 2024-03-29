// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Wpf.Ui.Input;
using Wpf.Ui.Interop;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Represents a text control that makes suggestions to users as they enter text using a keyboard. The app is notified when text has been changed by the user and is responsible for providing relevant suggestions for this control to display.
/// </summary>
/// <example>
/// <code lang="xml">
/// &lt;ui:AutoSuggestBox x:Name="AutoSuggestBox" PlaceholderText="Search"&gt;
///     &lt;ui:AutoSuggestBox.Icon&gt;
///         &lt;ui:IconSourceElement&gt;
///             &lt;ui:SymbolIconSource Symbol="Search24" /&gt;
///         &lt;/ui:IconSourceElement&gt;
///     &lt;/ui:AutoSuggestBox.Icon&gt;
/// &lt;/ui:AutoSuggestBox&gt;
/// </code>
/// </example>
//[ToolboxItem(true)]
//[ToolboxBitmap(typeof(AutoSuggestBox), "AutoSuggestBox.bmp")]
[TemplatePart(Name = ElementTextBox, Type = typeof(TextBox))]
[TemplatePart(Name = ElementSuggestionsPopup, Type = typeof(Popup))]
[TemplatePart(Name = ElementSuggestionsList, Type = typeof(ListView))]
public class AutoSuggestBox : System.Windows.Controls.ItemsControl, IIconControl
{
    protected const string ElementTextBox = "PART_TextBox";
    protected const string ElementSuggestionsPopup = "PART_SuggestionsPopup";
    protected const string ElementSuggestionsList = "PART_SuggestionsList";

    /// <summary>
    /// Property for <see cref="OriginalItemsSource"/>.
    /// </summary>
    public static readonly DependencyProperty OriginalItemsSourceProperty = DependencyProperty.Register(
        nameof(OriginalItemsSource),
        typeof(IList),
        typeof(AutoSuggestBox),
        new PropertyMetadata(Array.Empty<object>())
    );

    /// <summary>
    /// Property for <see cref="IsSuggestionListOpen"/>.
    /// </summary>
    public static readonly DependencyProperty IsSuggestionListOpenProperty = DependencyProperty.Register(
        nameof(IsSuggestionListOpen),
        typeof(bool),
        typeof(AutoSuggestBox),
        new PropertyMetadata(false)
    );

    /// <summary>
    /// Property for <see cref="Text"/>.
    /// </summary>
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
        nameof(Text),
        typeof(string),
        typeof(AutoSuggestBox),
        new PropertyMetadata(String.Empty, TextPropertyChangedCallback)
    );

    /// <summary>
    /// Property for <see cref="PlaceholderText"/>.
    /// </summary>
    public static readonly DependencyProperty PlaceholderTextProperty = DependencyProperty.Register(
        nameof(PlaceholderText),
        typeof(string),
        typeof(AutoSuggestBox),
        new PropertyMetadata(String.Empty)
    );

    /// <summary>
    /// Property for <see cref="UpdateTextOnSelect"/>.
    /// </summary>
    public static readonly DependencyProperty UpdateTextOnSelectProperty = DependencyProperty.Register(
        nameof(UpdateTextOnSelect),
        typeof(bool),
        typeof(AutoSuggestBox),
        new PropertyMetadata(true)
    );

    /// <summary>
    /// Property for <see cref="MaxSuggestionListHeight"/>.
    /// </summary>
    public static readonly DependencyProperty MaxSuggestionListHeightProperty = DependencyProperty.Register(
        nameof(MaxSuggestionListHeight),
        typeof(double),
        typeof(AutoSuggestBox),
        new PropertyMetadata(0d)
    );

    /// <summary>
    /// Property for <see cref="Icon"/>.
    /// </summary>
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        nameof(Icon),
        typeof(IconElement),
        typeof(AutoSuggestBox),
        new PropertyMetadata(null)
    );

    /// <summary>
    /// Property for <see cref="FocusCommand"/>.
    /// </summary>
    public static readonly DependencyProperty FocusCommandProperty = DependencyProperty.Register(
        nameof(FocusCommand),
        typeof(ICommand),
        typeof(AutoSuggestBox),
        new PropertyMetadata(null)
    );

    /// <summary>
    /// Gets or sets your items here if you want to use the default filtering
    /// </summary>
    public IList OriginalItemsSource
    {
        get => (IList)GetValue(OriginalItemsSourceProperty);
        set => SetValue(OriginalItemsSourceProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the drop-down portion of the <see cref="AutoSuggestBox"/> is open.
    /// </summary>
    public bool IsSuggestionListOpen
    {
        get => (bool)GetValue(IsSuggestionListOpenProperty);
        set => SetValue(IsSuggestionListOpenProperty, value);
    }

    /// <summary>
    /// Gets or sets the text that is shown in the control.
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
    /// Gets or sets the maximum height for the drop-down portion of the <see cref="AutoSuggestBox"/> control.
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
        get => (IconElement?)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <summary>
    /// Gets command used for focusing control.
    /// </summary>
    public ICommand FocusCommand => (ICommand)GetValue(FocusCommandProperty);

    /// <summary>
    /// Routed event for <see cref="QuerySubmitted"/>.
    /// </summary>
    public static readonly RoutedEvent QuerySubmittedEvent = EventManager.RegisterRoutedEvent(
        nameof(QuerySubmitted),
        RoutingStrategy.Bubble,
        typeof(TypedEventHandler<AutoSuggestBox, AutoSuggestBoxQuerySubmittedEventArgs>),
        typeof(AutoSuggestBox)
    );

    /// <summary>
    /// Routed event for <see cref="SuggestionChosen"/>.
    /// </summary>
    public static readonly RoutedEvent SuggestionChosenEvent = EventManager.RegisterRoutedEvent(
        nameof(SuggestionChosen),
        RoutingStrategy.Bubble,
        typeof(TypedEventHandler<AutoSuggestBox, AutoSuggestBoxSuggestionChosenEventArgs>),
        typeof(AutoSuggestBox)
    );

    /// <summary>
    /// Routed event for <see cref="TextChanged"/>.
    /// </summary>
    public static readonly RoutedEvent TextChangedEvent = EventManager.RegisterRoutedEvent(
        nameof(TextChanged),
        RoutingStrategy.Bubble,
        typeof(TypedEventHandler<AutoSuggestBox, AutoSuggestBoxTextChangedEventArgs>),
        typeof(AutoSuggestBox)
    );

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

    protected TextBox? TextBox = null;
    protected Popup SuggestionsPopup = null!;
    protected ListView? SuggestionsList = null!;
    private bool _changingTextAfterSuggestionChosen;
    private bool _isChangedTextOutSideOfTextBox;
    private object? _selectedItem;
    private bool? _isHwndHookSubscribed;

    public AutoSuggestBox()
    {
        Loaded += static (sender, _) =>
        {
            var self = (AutoSuggestBox)sender;

            self.AcquireTemplateResources();
        };

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
        _isHwndHookSubscribed = false;

        AcquireTemplateResources();
    }

    /// <inheritdoc cref="UIElement.Focus" />
    public new bool Focus()
    {
        return TextBox!.Focus();
    }

    protected T GetTemplateChild<T>(string name)
        where T : DependencyObject
    {
        if (GetTemplateChild(name) is not T dependencyObject)
        {
            throw new ArgumentNullException(name);
        }

        return dependencyObject;
    }

    protected virtual void AcquireTemplateResources()
    {
        // Unsubscribe each handler before subscription, to prevent memory leak from double subscriptions.
        // Unsubscription is safe, even if event has never been subscribed to.
        if (TextBox != null)
        {
            TextBox.PreviewKeyDown -= TextBoxOnPreviewKeyDown;
            TextBox.PreviewKeyDown += TextBoxOnPreviewKeyDown;
            TextBox.TextChanged -= TextBoxOnTextChanged;
            TextBox.TextChanged += TextBoxOnTextChanged;
            TextBox.LostKeyboardFocus -= TextBoxOnLostKeyboardFocus;
            TextBox.LostKeyboardFocus += TextBoxOnLostKeyboardFocus;
        }

        if (SuggestionsList != null)
        {
            SuggestionsList.SelectionChanged -= SuggestionsListOnSelectionChanged;
            SuggestionsList.SelectionChanged += SuggestionsListOnSelectionChanged;
            SuggestionsList.PreviewKeyDown -= SuggestionsListOnPreviewKeyDown;
            SuggestionsList.PreviewKeyDown += SuggestionsListOnPreviewKeyDown;
            SuggestionsList.LostKeyboardFocus -= SuggestionsListOnLostKeyboardFocus;
            SuggestionsList.LostKeyboardFocus += SuggestionsListOnLostKeyboardFocus;
            SuggestionsList.PreviewMouseLeftButtonUp -= SuggestionsListOnPreviewMouseLeftButtonUp;
            SuggestionsList.PreviewMouseLeftButtonUp += SuggestionsListOnPreviewMouseLeftButtonUp;
        }

        if (_isHwndHookSubscribed.HasValue && !_isHwndHookSubscribed.Value)
        {
            var hwnd = (HwndSource)PresentationSource.FromVisual(this)!;
            hwnd.AddHook(Hook);
            _isHwndHookSubscribed = true;
        }
    }

    protected virtual void ReleaseTemplateResources()
    {
        if (TextBox != null)
        {
            TextBox.PreviewKeyDown -= TextBoxOnPreviewKeyDown;
            TextBox.TextChanged -= TextBoxOnTextChanged;
            TextBox.LostKeyboardFocus -= TextBoxOnLostKeyboardFocus;
        }

        if (SuggestionsList != null)
        {
            SuggestionsList.SelectionChanged -= SuggestionsListOnSelectionChanged;
            SuggestionsList.PreviewKeyDown -= SuggestionsListOnPreviewKeyDown;
            SuggestionsList.LostKeyboardFocus -= SuggestionsListOnLostKeyboardFocus;
            SuggestionsList.PreviewMouseLeftButtonUp -= SuggestionsListOnPreviewMouseLeftButtonUp;
        }

        if (
            (_isHwndHookSubscribed.HasValue && _isHwndHookSubscribed.Value)
            && PresentationSource.FromVisual(this) is HwndSource source
        )
        {
            source.RemoveHook(Hook);
            _isHwndHookSubscribed = false;
        }
    }

    /// <summary>
    /// Method for <see cref="QuerySubmitted"/>.
    /// </summary>
    /// <param name="queryText">Currently submitted query text.</param>
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
    /// <param name="selectedItem">Currently selected item.</param>
    protected virtual void OnSuggestionChosen(object selectedItem)
    {
        var args = new AutoSuggestBoxSuggestionChosenEventArgs(SuggestionChosenEvent, this)
        {
            SelectedItem = selectedItem
        };

        RaiseEvent(args);

        if (UpdateTextOnSelect && !args.Handled)
        {
            UpdateTexBoxTextAfterSelection(selectedItem);
        }
    }

    /// <summary>
    /// Method for <see cref="TextChanged"/>.
    /// </summary>
    /// <param name="reason">Data for the text changed event.</param>
    /// <param name="text">Changed text.</param>
    protected virtual void OnTextChanged(AutoSuggestionBoxTextChangeReason reason, string text)
    {
        var args = new AutoSuggestBoxTextChangedEventArgs(TextChangedEvent, this)
        {
            Reason = reason,
            Text = text
        };

        RaiseEvent(args);

        if (args is { Handled: false, Reason: AutoSuggestionBoxTextChangeReason.UserInput })
        {
            DefaultFiltering(text);
        }
    }

    private void TextBoxOnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key is Key.Escape)
        {
            SetCurrentValue(IsSuggestionListOpenProperty, false);
            return;
        }

        if (e.Key is Key.Enter)
        {
            SetCurrentValue(IsSuggestionListOpenProperty, false);
            OnQuerySubmitted(TextBox!.Text);
            return;
        }

        if (e.Key is not Key.Down || !IsSuggestionListOpen)
        {
            return;
        }

        _ = SuggestionsList?.Focus();
    }

    private void TextBoxOnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        if (e.NewFocus is ListView)
        {
            return;
        }

        SetCurrentValue(IsSuggestionListOpenProperty, false);
    }

    private void TextBoxOnTextChanged(object sender, TextChangedEventArgs e)
    {
        AutoSuggestionBoxTextChangeReason changeReason = AutoSuggestionBoxTextChangeReason.UserInput;

        if (_changingTextAfterSuggestionChosen)
        {
            changeReason = AutoSuggestionBoxTextChangeReason.SuggestionChosen;
        }

        if (_isChangedTextOutSideOfTextBox)
        {
            changeReason = AutoSuggestionBoxTextChangeReason.ProgrammaticChange;
        }

        OnTextChanged(changeReason, TextBox!.Text);

        SuggestionsList!.SetCurrentValue(Selector.SelectedItemProperty, null);

        if (changeReason is not AutoSuggestionBoxTextChangeReason.UserInput)
        {
            return;
        }

        SetCurrentValue(IsSuggestionListOpenProperty, true);
    }

    private void SuggestionsListOnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        if (e.NewFocus is ListViewItem)
        {
            return;
        }

        SetCurrentValue(IsSuggestionListOpenProperty, false);
    }

    private void SuggestionsListOnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key is not Key.Enter)
        {
            return;
        }

        SetCurrentValue(IsSuggestionListOpenProperty, false);

        OnSelectedChanged(SuggestionsList!.SelectedItem);
    }

    private void SuggestionsListOnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (SuggestionsList!.SelectedItem is not null)
        {
            return;
        }

        SetCurrentValue(IsSuggestionListOpenProperty, false);

        if (_selectedItem is not null)
        {
            OnSuggestionChosen(_selectedItem);
        }
    }

    private void SuggestionsListOnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (SuggestionsList!.SelectedItem is null)
        {
            return;
        }

        OnSelectedChanged(SuggestionsList.SelectedItem);
    }

    private IntPtr Hook(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
    {
        if (!IsSuggestionListOpen)
        {
            return IntPtr.Zero;
        }

        var message = (User32.WM)msg;

        if (message is User32.WM.NCACTIVATE or User32.WM.WINDOWPOSCHANGED)
        {
            SetCurrentValue(IsSuggestionListOpenProperty, false);
        }

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

        TextBox!.SetCurrentValue(System.Windows.Controls.TextBox.TextProperty, GetStringFromObj(selectedObj));

        _changingTextAfterSuggestionChosen = false;
    }

    private void DefaultFiltering(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            SetCurrentValue(ItemsSourceProperty, OriginalItemsSource);
            return;
        }

        var splitText = text.ToLowerInvariant().Split(' ');
        var suitableItems = OriginalItemsSource
            .Cast<object>()
            .Where(item =>
            {
                var itemText = GetStringFromObj(item)?.ToLowerInvariant();
                return splitText.All(key => itemText?.Contains(key) ?? false);
            })
            .ToList();

        SetCurrentValue(ItemsSourceProperty, suitableItems);
    }

    private string? GetStringFromObj(object obj)
    {
        // uses reflection. maybe it needs some optimization?
        var displayMemberPathText = !string.IsNullOrEmpty(DisplayMemberPath) && obj.GetType().GetProperty(DisplayMemberPath)?.GetValue(obj) is string value
            ? value
            : null;

        return displayMemberPathText ?? obj as string ?? obj.ToString();
    }

    private static void TextPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var self = (AutoSuggestBox)d;
        var newText = (string)e.NewValue;

        if (self.TextBox is null)
        {
            return;
        }

        if (self.TextBox.Text == newText)
        {
            return;
        }

        self._isChangedTextOutSideOfTextBox = true;

        self.TextBox.SetCurrentValue(System.Windows.Controls.TextBox.TextProperty, newText);

        self._isChangedTextOutSideOfTextBox = false;
    }
}
