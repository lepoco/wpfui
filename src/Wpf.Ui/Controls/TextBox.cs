// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Input;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Extended <see cref="System.Windows.Controls.TextBox"/> with additional parameters like <see cref="PlaceholderText"/>.
/// </summary>
public class TextBox : System.Windows.Controls.TextBox
{
    #region Static properties

    /// <summary>
    /// Property for <see cref="Icon"/>.
    /// </summary>
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        nameof(Icon),
        typeof(Common.SymbolRegular),
        typeof(TextBox),
        new PropertyMetadata(Common.SymbolRegular.Empty)
    );

    /// <summary>
    /// Property for <see cref="IconFilled"/>.
    /// </summary>
    public static readonly DependencyProperty IconFilledProperty = DependencyProperty.Register(
        nameof(IconFilled),
        typeof(bool),
        typeof(TextBox),
        new PropertyMetadata(false)
    );

    /// <summary>
    /// Property for <see cref="IconPlacement"/>.
    /// </summary>
    public static readonly DependencyProperty IconPlacementProperty = DependencyProperty.Register(
        nameof(IconPlacement),
        typeof(ElementPlacement),
        typeof(TextBox),
        new PropertyMetadata(ElementPlacement.Left)
    );

    /// <summary>
    /// Property for <see cref="PlaceholderText"/>.
    /// </summary>
    public static readonly DependencyProperty PlaceholderTextProperty = DependencyProperty.Register(
        nameof(PlaceholderText),
        typeof(string),
        typeof(TextBox),
        new PropertyMetadata(String.Empty)
    );

    /// <summary>
    /// Property for <see cref="PlaceholderEnabled"/>.
    /// </summary>
    public static readonly DependencyProperty PlaceholderEnabledProperty =
        DependencyProperty.Register(
            nameof(PlaceholderEnabled),
            typeof(bool),
            typeof(TextBox),
            new PropertyMetadata(true)
        );

    /// <summary>
    /// Property for <see cref="ClearButtonEnabled"/>.
    /// </summary>
    public static readonly DependencyProperty ClearButtonEnabledProperty =
        DependencyProperty.Register(
            nameof(ClearButtonEnabled),
            typeof(bool),
            typeof(TextBox),
            new PropertyMetadata(true)
        );

    /// <summary>
    /// Property for <see cref="ShowClearButton"/>.
    /// </summary>
    public static readonly DependencyProperty ShowClearButtonProperty = DependencyProperty.Register(
        nameof(ShowClearButton),
        typeof(bool),
        typeof(TextBox),
        new PropertyMetadata(false)
    );

    /// <summary>
    /// Property for <see cref="IsTextSelectionEnabledProperty"/>.
    /// </summary>
    public static readonly DependencyProperty IsTextSelectionEnabledProperty =
        DependencyProperty.Register(
            nameof(IsTextSelectionEnabled),
            typeof(bool),
            typeof(TextBox),
            new PropertyMetadata(false)
        );

    /// <summary>
    /// Property for <see cref="TemplateButtonCommand"/>.
    /// </summary>
    public static readonly DependencyProperty TemplateButtonCommandProperty =
        DependencyProperty.Register(
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
    public Common.SymbolRegular Icon
    {
        get => (Common.SymbolRegular)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <summary>
    /// Defines which side the icon should be placed on.
    /// </summary>
    public ElementPlacement IconPlacement
    {
        get => (ElementPlacement)GetValue(IconPlacementProperty);
        set => SetValue(IconPlacementProperty, value);
    }

    /// <summary>
    /// Gets or sets a value deciding whether the <see cref="Icon"/> should be filled.
    /// </summary>
    public bool IconFilled
    {
        get => (bool)GetValue(IconFilledProperty);
        set => SetValue(IconFilledProperty, value);
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
    /// Gets or sets a value determining whether to display the placeholder.
    /// </summary>
    public bool PlaceholderEnabled
    {
        get => (bool)GetValue(PlaceholderEnabledProperty);
        set => SetValue(PlaceholderEnabledProperty, value);
    }

    /// <summary>
    /// Gets or sets a value determining whether to enable the clear button.
    /// </summary>
    public bool ClearButtonEnabled
    {
        get => (bool)GetValue(ClearButtonEnabledProperty);
        set => SetValue(ClearButtonEnabledProperty, value);
    }

    /// <summary>
    /// Gets or sets a value determining whether to show the clear button when <see cref="TextBox"/> is focused.
    /// </summary>
    public bool ShowClearButton
    {
        get => (bool)GetValue(ShowClearButtonProperty);
        protected set => SetValue(ShowClearButtonProperty, value);
    }

    /// <summary>
    /// TODO
    /// </summary>
    public bool IsTextSelectionEnabled
    {
        get => (bool)GetValue(IsTextSelectionEnabledProperty);
        set => SetValue(IsTextSelectionEnabledProperty, value);
    }

    /// <summary>
    /// Command triggered after clicking the button.
    /// </summary>
    public IRelayCommand TemplateButtonCommand =>
        (IRelayCommand)GetValue(TemplateButtonCommandProperty);

    #endregion

    /// <summary>
    /// Creates a new instance and assigns default events.
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
            Text = String.Empty;
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
