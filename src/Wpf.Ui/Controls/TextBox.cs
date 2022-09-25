// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls.Interfaces;
using Brush = System.Windows.Media.Brush;
using SystemColors = System.Windows.SystemColors;

// TODO: Fix clear button

namespace Wpf.Ui.Controls;

/// <summary>
/// Extended <see cref="System.Windows.Controls.TextBox"/> with additional parameters like <see cref="PlaceholderText"/>.
/// </summary>
public class TextBox : System.Windows.Controls.TextBox, IIconControl
{
    /// <summary>
    /// Property for <see cref="Icon"/>.
    /// </summary>
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon),
        typeof(Common.SymbolRegular), typeof(TextBox),
        new PropertyMetadata(Common.SymbolRegular.Empty));

    /// <summary>
    /// Property for <see cref="IconPlacement"/>.
    /// </summary>
    public static readonly DependencyProperty IconPlacementProperty = DependencyProperty.Register(
        nameof(IconPlacement),
        typeof(Common.ElementPlacement), typeof(TextBox),
        new PropertyMetadata(Common.ElementPlacement.Left));

    /// <summary>
    /// Property for <see cref="IconFilled"/>.
    /// </summary>
    public static readonly DependencyProperty IconFilledProperty = DependencyProperty.Register(nameof(IconFilled),
        typeof(bool), typeof(TextBox), new PropertyMetadata(false));

    /// <summary>
    /// DependencyProperty for <see cref="IconForeground" /> property.
    /// </summary>
    public static readonly DependencyProperty IconForegroundProperty =
        DependencyProperty.RegisterAttached(
            nameof(IconForeground),
            typeof(Brush),
            typeof(TextBox),
            new FrameworkPropertyMetadata(
                SystemColors.ControlTextBrush,
                FrameworkPropertyMetadataOptions.Inherits));

    /// <summary>
    /// Property for <see cref="PlaceholderText"/>.
    /// </summary>
    public static readonly DependencyProperty PlaceholderTextProperty = DependencyProperty.Register(nameof(PlaceholderText),
        typeof(string), typeof(TextBox), new PropertyMetadata(String.Empty));

    /// <summary>
    /// Property for <see cref="PlaceholderEnabled"/>.
    /// </summary>
    public static readonly DependencyProperty PlaceholderEnabledProperty = DependencyProperty.Register(
        nameof(PlaceholderEnabled),
        typeof(bool), typeof(TextBox), new PropertyMetadata(true));

    /// <summary>
    /// Property for <see cref="ClearButtonEnabled"/>.
    /// </summary>
    public static readonly DependencyProperty ClearButtonEnabledProperty = DependencyProperty.Register(nameof(ClearButtonEnabled),
        typeof(bool), typeof(TextBox), new PropertyMetadata(true));

    /// <summary>
    /// Property for <see cref="ShowClearButton"/>.
    /// </summary>
    public static readonly DependencyProperty ShowClearButtonProperty = DependencyProperty.Register(nameof(ShowClearButton),
        typeof(bool), typeof(TextBox), new PropertyMetadata(false));

    /// <summary>
    /// Property for <see cref="TemplateButtonCommand"/>.
    /// </summary>
    public static readonly DependencyProperty TemplateButtonCommandProperty =
        DependencyProperty.Register(nameof(TemplateButtonCommand),
            typeof(Common.IRelayCommand), typeof(TextBox), new PropertyMetadata(null));

    /// <inheritdoc />
    public Common.SymbolRegular Icon
    {
        get => (Common.SymbolRegular)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <summary>
    /// Defines which side the icon should be placed on.
    /// </summary>
    public Common.ElementPlacement IconPlacement
    {
        get => (Common.ElementPlacement)GetValue(IconPlacementProperty);
        set => SetValue(IconPlacementProperty, value);
    }

    /// <inheritdoc />
    public bool IconFilled
    {
        get => (bool)GetValue(IconFilledProperty);
        set => SetValue(IconFilledProperty, value);
    }

    /// <summary>
    /// The Foreground property specifies the foreground brush of an element's <see cref="Icon"/>.
    /// </summary>
    public Brush IconForeground
    {
        get => (Brush)GetValue(IconForegroundProperty);
        set => SetValue(IconForegroundProperty, value);
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
    /// Command triggered after clicking the button.
    /// </summary>
    public Common.IRelayCommand TemplateButtonCommand => (Common.IRelayCommand)GetValue(TemplateButtonCommandProperty);

    /// <summary>
    /// Creates a new instance and assigns default events.
    /// </summary>
    public TextBox()
    {
        SetValue(TemplateButtonCommandProperty, new Common.RelayCommand(o => OnTemplateButtonClick(this, o)));
    }

    /// <inheritdoc />
    protected override void OnTextChanged(TextChangedEventArgs e)
    {
        base.OnTextChanged(e);

        if (PlaceholderEnabled && Text.Length > 0)
            PlaceholderEnabled = false;

        if (!PlaceholderEnabled && Text.Length < 1)
            PlaceholderEnabled = true;

        RevealClearButton();
    }

    /// <inheritdoc />
    protected override void OnGotFocus(RoutedEventArgs e)
    {
        base.OnGotFocus(e);

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
            ShowClearButton = Text.Length > 0;
    }

    /// <summary>
    /// Hides the clear button by <see cref="ShowClearButton"/> property.
    /// </summary>
    protected void HideClearButton()
    {
        if (ClearButtonEnabled && !IsKeyboardFocusWithin && ShowClearButton)
            ShowClearButton = false;
    }

    /// <summary>
    /// Triggered by clicking a button in the control template.
    /// </summary>
    /// <param name="sender">Sender of the click event.</param>
    /// <param name="parameter">Additional parameters.</param>
    protected virtual void OnTemplateButtonClick(object sender, object parameter)
    {
        if (parameter == null)
            return;

        var param = parameter as string ?? String.Empty;

#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO: {typeof(TextBox)} button clicked with param: {param}", "Wpf.Ui.TextBox");
#endif

        switch (param)
        {
            case "clear":
                if (Text.Length > 0)
                    Text = String.Empty;

                break;
        }
    }
}
