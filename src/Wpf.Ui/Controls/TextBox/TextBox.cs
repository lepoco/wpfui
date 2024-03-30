// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Diagnostics;
using System.Windows.Controls;
using Wpf.Ui.Converters;
using Wpf.Ui.Input;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Extended <see cref="System.Windows.Controls.TextBox"/> with additional parameters like <see cref="PlaceholderText"/>.
/// </summary>
public class TextBox : System.Windows.Controls.TextBox
{
    #region Static properties

    /// <summary>Identifies the <see cref="Icon"/> dependency property.</summary>
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        nameof(Icon),
        typeof(IconElement),
        typeof(TextBox),
        new PropertyMetadata(null, null, IconElement.Coerce)
    );

    /// <summary>Identifies the <see cref="IconPlacement"/> dependency property.</summary>
    public static readonly DependencyProperty IconPlacementProperty = DependencyProperty.Register(
        nameof(IconPlacement),
        typeof(ElementPlacement),
        typeof(TextBox),
        new PropertyMetadata(ElementPlacement.Left)
    );

    /// <summary>Identifies the <see cref="PlaceholderText"/> dependency property.</summary>
    public static readonly DependencyProperty PlaceholderTextProperty = DependencyProperty.Register(
        nameof(PlaceholderText),
        typeof(string),
        typeof(TextBox),
        new PropertyMetadata(string.Empty)
    );

    /// <summary>Identifies the <see cref="PlaceholderEnabled"/> dependency property.</summary>
    public static readonly DependencyProperty PlaceholderEnabledProperty = DependencyProperty.Register(
        nameof(PlaceholderEnabled),
        typeof(bool),
        typeof(TextBox),
        new PropertyMetadata(true)
    );

    /// <summary>Identifies the <see cref="ClearButtonEnabled"/> dependency property.</summary>
    public static readonly DependencyProperty ClearButtonEnabledProperty = DependencyProperty.Register(
        nameof(ClearButtonEnabled),
        typeof(bool),
        typeof(TextBox),
        new PropertyMetadata(true)
    );

    /// <summary>Identifies the <see cref="ShowClearButton"/> dependency property.</summary>
    public static readonly DependencyProperty ShowClearButtonProperty = DependencyProperty.Register(
        nameof(ShowClearButton),
        typeof(bool),
        typeof(TextBox),
        new PropertyMetadata(false)
    );

    /// <summary>Identifies the <see cref="IsTextSelectionEnabled"/> dependency property.</summary>
    public static readonly DependencyProperty IsTextSelectionEnabledProperty = DependencyProperty.Register(
        nameof(IsTextSelectionEnabled),
        typeof(bool),
        typeof(TextBox),
        new PropertyMetadata(false)
    );

    /// <summary>Identifies the <see cref="TemplateButtonCommand"/> dependency property.</summary>
    public static readonly DependencyProperty TemplateButtonCommandProperty = DependencyProperty.Register(
        nameof(TemplateButtonCommand),
        typeof(IRelayCommand),
        typeof(TextBox),
        new PropertyMetadata(null)
    );

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets displayed <see cref="IconElement"/>.
    /// </summary>
    public IconElement? Icon
    {
        get => (IconElement?)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <summary>
    /// Gets or sets which side the icon should be placed on.
    /// </summary>
    public ElementPlacement IconPlacement
    {
        get => (ElementPlacement)GetValue(IconPlacementProperty);
        set => SetValue(IconPlacementProperty, value);
    }

    /// <summary>
    /// Gets or sets numbers pattern.
    /// </summary>
    public string PlaceholderText
    {
        get => (string)GetValue(PlaceholderTextProperty);
        set => SetValue(PlaceholderTextProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether to display the placeholder text.
    /// </summary>
    public bool PlaceholderEnabled
    {
        get => (bool)GetValue(PlaceholderEnabledProperty);
        set => SetValue(PlaceholderEnabledProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether to enable the clear button.
    /// </summary>
    public bool ClearButtonEnabled
    {
        get => (bool)GetValue(ClearButtonEnabledProperty);
        set => SetValue(ClearButtonEnabledProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether to show the clear button when <see cref="TextBox"/> is focused.
    /// </summary>
    public bool ShowClearButton
    {
        get => (bool)GetValue(ShowClearButtonProperty);
        protected set => SetValue(ShowClearButtonProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether text selection is enabled.
    /// </summary>
    public bool IsTextSelectionEnabled
    {
        get => (bool)GetValue(IsTextSelectionEnabledProperty);
        set => SetValue(IsTextSelectionEnabledProperty, value);
    }

    /// <summary>
    /// Gets the command triggered when clicking the button.
    /// </summary>
    public IRelayCommand TemplateButtonCommand => (IRelayCommand)GetValue(TemplateButtonCommandProperty);

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="TextBox"/> class.
    /// </summary>
    public TextBox()
    {
        SetValue(TemplateButtonCommandProperty, new RelayCommand<string>(OnTemplateButtonClick));
    }

    /// <inheritdoc />
    protected override void OnTextChanged(TextChangedEventArgs e)
    {
        base.OnTextChanged(e);

        if (PlaceholderEnabled && Text.Length > 0)
        {
            PlaceholderEnabled = false;
        }

        if (!PlaceholderEnabled && Text.Length < 1)
        {
            PlaceholderEnabled = true;
        }

        RevealClearButton();
    }

    /// <inheritdoc />
    protected override void OnGotFocus(RoutedEventArgs e)
    {
        base.OnGotFocus(e);

        CaretIndex = Text.Length;

        RevealClearButton();
    }

    /// <inheritdoc />
    protected override void OnLostFocus(RoutedEventArgs e)
    {
        base.OnLostFocus(e);

        HideClearButton();
    }

    /// <summary>
    /// Reveals the clear button by <see cref="ShowClearButton"/> property.
    /// </summary>
    protected void RevealClearButton()
    {
        if (ClearButtonEnabled && IsKeyboardFocusWithin)
        {
            ShowClearButton = Text.Length > 0;
        }
    }

    /// <summary>
    /// Hides the clear button by <see cref="ShowClearButton"/> property.
    /// </summary>
    protected void HideClearButton()
    {
        if (ClearButtonEnabled && !IsKeyboardFocusWithin && ShowClearButton)
        {
            ShowClearButton = false;
        }
    }

    /// <summary>
    /// Triggered when the user clicks the clear text button.
    /// </summary>
    protected virtual void OnClearButtonClick()
    {
        if (Text.Length > 0)
        {
            Text = string.Empty;
        }
    }

    /// <summary>
    /// Triggered by clicking a button in the control template.
    /// </summary>
    protected virtual void OnTemplateButtonClick(string? parameter)
    {
        Debug.WriteLine($"INFO: {typeof(TextBox)} button clicked", "Wpf.Ui.TextBox");

        OnClearButtonClick();
    }
}
