// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Controls;

namespace Wpf.Ui.Controls;

// TODO: This is an initial implementation and requires the necessary corrections, tests and adjustments.

/**
 * TextProperty contains asterisks OR raw password if IsPasswordRevealed is set to true
 * PasswordProperty always contains raw password
 */

/// <summary>
/// The modified password control.
/// </summary>
public class PasswordBox : Wpf.Ui.Controls.TextBox
{
    private bool _lockUpdatingContents;

    /// <summary>
    /// Property for <see cref="Password"/>.
    /// </summary>
    public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register(nameof(Password),
        typeof(string), typeof(PasswordBox), new PropertyMetadata(String.Empty, OnPasswordPropertyChanged));

    /// <summary>
    /// Property for <see cref="PasswordChar"/>.
    /// </summary>
    public static readonly DependencyProperty PasswordCharProperty = DependencyProperty.Register(nameof(PasswordChar),
        typeof(char), typeof(PasswordBox), new PropertyMetadata('*', OnPasswordCharPropertyChanged));

    /// <summary>
    /// Property for <see cref="IsPasswordRevealed"/>.
    /// </summary>
    public static readonly DependencyProperty IsPasswordRevealedProperty = DependencyProperty.Register(nameof(IsPasswordRevealed),
        typeof(bool), typeof(PasswordBox), new PropertyMetadata(false, OnPasswordRevealModePropertyChanged));

    /// <summary>
    /// Property for <see cref="RevealButtonEnabled"/>.
    /// </summary>
    public static readonly DependencyProperty RevealButtonEnabledProperty = DependencyProperty.Register(nameof(RevealButtonEnabled),
        typeof(bool), typeof(PasswordBox), new PropertyMetadata(true));

    /// <summary>
    /// Event for "Password has changed"
    /// </summary>
    public static readonly RoutedEvent PasswordChangedEvent = EventManager.RegisterRoutedEvent(
        nameof(PasswordChanged),
        RoutingStrategy.Bubble,
        typeof(RoutedEventHandler),
        typeof(PasswordBox));

    /// <summary>
    /// Gets or sets currently typed text represented by asterisks.
    /// </summary>
    public string Password
    {
        get => (string)GetValue(PasswordProperty);
        set => SetValue(PasswordProperty, value);
    }

    /// <summary>
    /// Gets or sets character used to mask the password.
    /// </summary>
    public char PasswordChar
    {
        get => (char)GetValue(PasswordCharProperty);
        set => SetValue(PasswordCharProperty, value);
    }

    /// <summary>
    /// Gets a value indicating whether the password is revealed.
    /// </summary>
    public bool IsPasswordRevealed
    {
        get => (bool)GetValue(IsPasswordRevealedProperty);
        private set => SetValue(IsPasswordRevealedProperty, value);
    }

    /// <summary>
    /// Gets or sets a value deciding whether to display the reveal password button.
    /// </summary>
    public bool RevealButtonEnabled
    {
        get => (bool)GetValue(RevealButtonEnabledProperty);
        set => SetValue(RevealButtonEnabledProperty, value);
    }

    /// <summary>
    /// Event fired from this text box when its inner content
    /// has been changed.
    /// </summary>
    /// <remarks>
    /// It is redirected from inner TextContainer.Changed event.
    /// </remarks>
    public event RoutedEventHandler PasswordChanged
    {
        add => AddHandler(PasswordChangedEvent, value);
        remove => RemoveHandler(PasswordChangedEvent, value);
    }

    public PasswordBox()
    {
        _lockUpdatingContents = false;
    }

    /// <inheritdoc />
    protected override void OnTextChanged(TextChangedEventArgs e)
    {
        UpdateTextContents(true);

        if (_lockUpdatingContents)
        {
            base.OnTextChanged(e);
        }
        else
        {
            if (PlaceholderEnabled && Text.Length > 0)
                PlaceholderEnabled = false;

            if (!PlaceholderEnabled && Text.Length < 1)
                PlaceholderEnabled = true;

            RevealClearButton();
        }
    }

    /// <summary>
    /// Is called when <see cref="Password"/> property is changing.
    /// </summary>
    protected virtual void OnPasswordChanged()
    {
        UpdateTextContents(false);
    }

    /// <summary>
    /// Is called when <see cref="PasswordChar"/> property is changing.
    /// </summary>
    protected virtual void OnPasswordCharChanged()
    {
        // If password is currently revealed,
        // do not replace displayed text with asterisks
        if (IsPasswordRevealed)
            return;

        _lockUpdatingContents = true;

        Text = new String(PasswordChar, Password.Length);

        _lockUpdatingContents = false;
    }

    protected virtual void OnPasswordRevealModeChanged()
    {
        _lockUpdatingContents = true;

        Text = IsPasswordRevealed ? Password : new String(PasswordChar, Password.Length);

        _lockUpdatingContents = false;
    }

    /// <summary>
    /// Triggered by clicking a button in the control template.
    /// </summary>
    /// <param name="sender">Sender of the click event.</param>
    /// <param name="parameter">Additional parameters.</param>
    protected override void OnTemplateButtonClick(string parameter)
    {
        base.OnTemplateButtonClick(parameter);

#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO: {typeof(PasswordBox)} button clicked with param: {parameter}", "Wpf.Ui.PasswordBox");
#endif

        switch (parameter)
        {
            case "reveal":
                IsPasswordRevealed = !IsPasswordRevealed;
                Focus();
                CaretIndex = Text.Length;

                break;
        }
    }

    private void UpdateTextContents(bool isTriggeredByTextInput)
    {
        if (_lockUpdatingContents)
            return;

        if (IsPasswordRevealed)
        {
            if (Password == Text)
                return;

            _lockUpdatingContents = true;

            if (isTriggeredByTextInput)
            {
                Password = Text;
            }
            else
            {
                Text = Password;
                CaretIndex = Text.Length;
            }

            RaiseEvent(new RoutedEventArgs(PasswordChangedEvent));

            _lockUpdatingContents = false;

            return;
        }

        var caretIndex = CaretIndex;
        var selectionIndex = SelectionStart;
        var currentPassword = Password;
        var newPasswordValue = currentPassword;

        if (isTriggeredByTextInput)
        {
            var currentText = Text;
            var newCharacters = currentText.Replace(PasswordChar.ToString(), String.Empty);

            if (currentText.Length < currentPassword.Length)
                newPasswordValue = currentPassword.Remove(selectionIndex, currentPassword.Length - currentText.Length);

            if (newCharacters.Length > 1)
            {
                var index = currentText.IndexOf(newCharacters[0]);

                newPasswordValue = index > newPasswordValue.Length - 1
                    ? newPasswordValue + newCharacters
                    : newPasswordValue.Insert(index, newCharacters);
            }
            else
            {
                for (int i = 0; i < currentText.Length; i++)
                {
                    if (currentText[i] == PasswordChar)
                        continue;

                    newPasswordValue = currentText.Length == newPasswordValue.Length
                        ? newPasswordValue
                            .Remove(i, 1)
                            .Insert(i, currentText[i].ToString())
                        : newPasswordValue
                            .Insert(i, currentText[i].ToString());
                }
            }
        }

        _lockUpdatingContents = true;

        Text = new String(PasswordChar, newPasswordValue.Length);
        Password = newPasswordValue;
        CaretIndex = caretIndex;

        RaiseEvent(new RoutedEventArgs(PasswordChangedEvent));

        _lockUpdatingContents = false;
    }

    /// <summary>
    /// Called when <see cref="Password"/> is changed.
    /// </summary>
    private static void OnPasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not PasswordBox control)
            return;

        control.OnPasswordChanged();
    }

    /// <summary>
    /// Called if the character is changed in the during the run.
    /// </summary>
    private static void OnPasswordCharPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not PasswordBox control)
            return;

        control.OnPasswordCharChanged();
    }

    /// <summary>
    /// Called if the reveal mode is changed in the during the run.
    /// </summary>
    private static void OnPasswordRevealModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not PasswordBox control)
            return;

        control.OnPasswordRevealModeChanged();
    }
}
