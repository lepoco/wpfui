using System.Windows;
using System;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

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
    public static readonly DependencyProperty IsSuggestionListOpenProperty = DependencyProperty.Register(nameof(IsSuggestionListOpen),
        typeof(bool), typeof(NewAutoSuggestBox),
        new PropertyMetadata(false));

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

    #endregion

    protected System.Windows.Controls.TextBox TextBox = null!;
    protected Popup SuggestionsPopup = null!;
    protected ListView SuggestionsList = null!;

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

        SuggestionsList.SelectionChanged += SuggestionsListOnSelectionChanged;
        SuggestionsList.PreviewKeyDown += SuggestionsListOnPreviewKeyDown;
        SuggestionsList.LostKeyboardFocus += SuggestionsListOnLostKeyboardFocus;
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

        SuggestionsList.SelectionChanged -= SuggestionsListOnSelectionChanged;
        SuggestionsList.PreviewKeyDown -= SuggestionsListOnPreviewKeyDown;
        SuggestionsList.LostKeyboardFocus -= SuggestionsListOnLostKeyboardFocus;
    }

    private void TextBoxOnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key is not Key.Down || !IsSuggestionListOpen)
            return;

        SuggestionsList.Focus();
    }

    private void SuggestionsListOnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        Debug.WriteLine("Losing focus");

        if (e.NewFocus is ListViewItem)
            return;

        IsSuggestionListOpen = false;
    }

    private void TextBoxOnTextChanged(object sender, TextChangedEventArgs e)
    {
        IsSuggestionListOpen = true;
    }

    private void SuggestionsListOnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key is not Key.Enter)
            return;

        var q = SuggestionsList.SelectedItem;
    }

    private void SuggestionsListOnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Debug.WriteLine($"Selected | {e.AddedItems[0]}");
    }
}
