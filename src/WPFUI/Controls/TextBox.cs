// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WPFUI.Controls.Interfaces;

// TODO: Add optional X icon to clear stuff in input

namespace WPFUI.Controls;

/// <summary>
/// Extended <see cref="System.Windows.Controls.TextBox"/> with additional parameters like <see cref="Placeholder"/>.
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
    /// Property for <see cref="IconPosition"/>.
    /// </summary>
    public static readonly DependencyProperty IconPositionProperty = DependencyProperty.Register(
        nameof(IconPosition),
        typeof(Common.ElementPosition), typeof(TextBox),
        new PropertyMetadata(Common.ElementPosition.Left));

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
    /// Property for <see cref="Placeholder"/>.
    /// </summary>
    public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(nameof(Placeholder),
        typeof(string), typeof(TextBox), new PropertyMetadata(String.Empty));

    /// <summary>
    /// Property for <see cref="PlaceholderVisible"/>.
    /// </summary>
    public static readonly DependencyProperty PlaceholderVisibleProperty = DependencyProperty.Register(
        nameof(PlaceholderVisible),
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
    public Common.ElementPosition IconPosition
    {
        get => (Common.ElementPosition)GetValue(IconPositionProperty);
        set => SetValue(IconPositionProperty, value);
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
    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    /// <summary>
    /// Gets or sets a value determining whether to display the placeholder.
    /// </summary>
    public bool PlaceholderVisible
    {
        get => (bool)GetValue(PlaceholderVisibleProperty);
        set => SetValue(PlaceholderVisibleProperty, value);
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
        SetValue(TemplateButtonCommandProperty, new Common.RelayCommand(o => ButtonOnClick(this, o)));
    }

    /// <inheritdoc />
    protected override void OnTextChanged(TextChangedEventArgs e)
    {
        base.OnTextChanged(e);

        if (PlaceholderVisible && Text.Length > 0)
            PlaceholderVisible = false;

        if (!PlaceholderVisible && Text.Length < 1)
            PlaceholderVisible = true;

        if (IsFocused && ClearButtonEnabled)
            ShowClearButton = Text.Length > 0;
    }

    /// <inheritdoc />
    protected override void OnGotFocus(RoutedEventArgs e)
    {
        base.OnGotFocus(e);

        if (Text.Length > 0 && ClearButtonEnabled)
            ShowClearButton = true;
    }

    /// <inheritdoc />
    protected override async void OnLostFocus(RoutedEventArgs e)
    {
        base.OnLostFocus(e);

        // The field loses focus, so the button disappears, so you can't press it. Need to delay it a bit.
        await Task.Run(async () =>
        {
            // Below 100 doesn't always catch, I know it's visible and there is another way to fix it... but it works
            await Task.Delay(128);

            await Dispatcher.InvokeAsync(() =>
            {
                if (ShowClearButton && ClearButtonEnabled)
                    ShowClearButton = false;
            });
        });
    }

    /// <summary>
    /// Triggered by clicking a button in the control template.
    /// </summary>
    /// <param name="sender">Sender of the click event.</param>
    /// <param name="parameter">Additional parameters.</param>
    protected virtual void ButtonOnClick(object sender, object parameter)
    {
        if (parameter == null)
            return;

        string param = parameter as string ?? String.Empty;

#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO: {typeof(TextBox)} button clicked with param: {param}", "WPFUI.TextBox");
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
