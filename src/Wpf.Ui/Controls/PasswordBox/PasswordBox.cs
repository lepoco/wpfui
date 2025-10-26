// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// The modified password control.
/// </summary>
public partial class PasswordBox : TextBox
{
    private readonly PasswordHelper _passwordHelper;
    private bool _isUpdating;

    /// <summary>
    /// Identifies the <see cref="Password"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register(
        nameof(Password),
        typeof(string),
        typeof(PasswordBox),
        new PropertyMetadata(string.Empty, OnPasswordChanged)
    );

    /// <summary>
    /// Identifies the <see cref="PasswordChar"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty PasswordCharProperty = DependencyProperty.Register(
        nameof(PasswordChar),
        typeof(char),
        typeof(PasswordBox),
        new PropertyMetadata('*', OnPasswordCharChanged)
    );

    /// <summary>
    /// Identifies the <see cref="IsPasswordRevealed"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsPasswordRevealedProperty = DependencyProperty.Register(
        nameof(IsPasswordRevealed),
        typeof(bool),
        typeof(PasswordBox),
        new PropertyMetadata(false, OnIsPasswordRevealedChanged)
    );

    /// <summary>
    /// Identifies the <see cref="RevealButtonEnabled"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty RevealButtonEnabledProperty = DependencyProperty.Register(
        nameof(RevealButtonEnabled),
        typeof(bool),
        typeof(PasswordBox),
        new PropertyMetadata(true)
    );

    /// <summary>
    /// Identifies the <see cref="PasswordChanged"/> routed event.
    /// </summary>
    public static readonly RoutedEvent PasswordChangedEvent = EventManager.RegisterRoutedEvent(
        nameof(PasswordChanged),
        RoutingStrategy.Bubble,
        typeof(RoutedEventHandler),
        typeof(PasswordBox)
    );

    /// <summary>
    /// Initializes a new instance of the <see cref="PasswordBox"/> class.
    /// </summary>
    public PasswordBox()
    {
        _passwordHelper = new PasswordHelper(this);
    }

    /// <summary>
    /// Gets or sets the actual password (not asterisks).
    /// </summary>
    public string Password
    {
        get => (string)GetValue(PasswordProperty);
        set => SetValue(PasswordProperty, value);
    }

    /// <summary>
    /// Gets or sets the character used to mask the password.
    /// </summary>
    public char PasswordChar
    {
        get => (char)GetValue(PasswordCharProperty);
        set => SetValue(PasswordCharProperty, value);
    }

    /// <summary>
    /// Gets a value indicating whether the password is currently revealed.
    /// </summary>
    public bool IsPasswordRevealed
    {
        get => (bool)GetValue(IsPasswordRevealedProperty);
        private set => SetValue(IsPasswordRevealedProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether gets or sets whether the password reveal button is enabled.
    /// </summary>
    public bool RevealButtonEnabled
    {
        get => (bool)GetValue(RevealButtonEnabledProperty);
        set => SetValue(RevealButtonEnabledProperty, value);
    }

    /// <summary>
    /// Occurs when the password content changes.
    /// </summary>
    public event RoutedEventHandler PasswordChanged
    {
        add => AddHandler(PasswordChangedEvent, value);
        remove => RemoveHandler(PasswordChangedEvent, value);
    }

    /// <inheritdoc/>
    protected override void OnTextChanged(TextChangedEventArgs e)
    {
        UpdateTextContents(isTriggeredByTextInput: true);
        SetPlaceholderTextVisibility();
        RevealClearButton();

        if (!_isUpdating)
        {
            base.OnTextChanged(e);
        }
    }

    /// <summary>
    /// Called when the <see cref="Password"/> property changes.
    /// </summary>
    protected virtual void OnPasswordChanged() => UpdateTextContents(isTriggeredByTextInput: false);

    /// <summary>
    /// Called when the <see cref="PasswordChar"/> property changes.
    /// </summary>
    protected virtual void OnPasswordCharChanged()
    {
        if (IsPasswordRevealed)
        {
            return;
        }

        UpdateWithLock(() => SetCurrentValue(TextProperty, new string(PasswordChar, Password.Length)));
    }

    /// <summary>
    /// Called when the <see cref="IsPasswordRevealed"/> property changes.
    /// </summary>
    protected virtual void OnIsPasswordRevealedChanged()
    {
        UpdateWithLock(() => SetCurrentValue(TextProperty, IsPasswordRevealed ? Password : new string(PasswordChar, Password.Length)));
    }

    /// <inheritdoc/>
    protected override void OnTemplateButtonClick(string? parameter)
    {
        if (parameter == "reveal")
        {
            SetCurrentValue(IsPasswordRevealedProperty, !IsPasswordRevealed);
            _ = Focus();
            CaretIndex = Text.Length;
        }
        else
        {
            base.OnTemplateButtonClick(parameter);
        }
    }

    /// <summary>
    /// Updates the text contents based on the current state.
    /// </summary>
    /// <param name="isTriggeredByTextInput">True if triggered by user text input; false if triggered by property change.</param>
    private void UpdateTextContents(bool isTriggeredByTextInput)
    {
        if (_isUpdating)
        {
            return;
        }

        if (IsPasswordRevealed)
        {
            HandleRevealedModeUpdate(isTriggeredByTextInput);
            return;
        }

        HandleHiddenModeUpdate(isTriggeredByTextInput);
    }

    /// <summary>
    /// Handles updates when password is in revealed mode.
    /// </summary>
    /// <param name="isTriggeredByTextInput">True if triggered by user text input.</param>
    private void HandleRevealedModeUpdate(bool isTriggeredByTextInput)
    {
        if (Password == Text)
        {
            return;
        }

        UpdateWithLock(() =>
        {
            if (isTriggeredByTextInput)
            {
                SetCurrentValue(PasswordProperty, Text);
            }
            else
            {
                SetCurrentValue(TextProperty, Password);
                CaretIndex = Text.Length;
            }

            RaisePasswordChangedEvent();
        });
    }

    /// <summary>
    /// Handles updates when password is in hidden mode.
    /// </summary>
    /// <param name="isTriggeredByTextInput">True if triggered by user text input.</param>
    private void HandleHiddenModeUpdate(bool isTriggeredByTextInput)
    {
        var caretIndex = CaretIndex;
        var newPassword = isTriggeredByTextInput ? _passwordHelper.GetNewPassword() : Password;

        UpdateWithLock(() =>
        {
            SetCurrentValue(TextProperty, new string(PasswordChar, newPassword.Length));
            SetCurrentValue(PasswordProperty, newPassword);
            CaretIndex = caretIndex;
            RaisePasswordChangedEvent();
        });
    }

    /// <summary>
    /// Executes an action while preventing recursive updates.
    /// </summary>
    /// <param name="updateAction">The action to execute.</param>
    private void UpdateWithLock(Action updateAction)
    {
        _isUpdating = true;
        updateAction();
        _isUpdating = false;
    }

    /// <summary>
    /// Raises the <see cref="PasswordChangedEvent"/>.
    /// </summary>
    private void RaisePasswordChangedEvent() => RaiseEvent(new RoutedEventArgs(PasswordChangedEvent));

    /// <summary>
    /// Handles changes to the <see cref="Password"/> dependency property.
    /// </summary>
    /// <param name="dependencyObject">The <see cref="DependencyObject"/> that raised the event.</param>
    /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> containing the event data.</param>
    private static void OnPasswordChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        if (dependencyObject is PasswordBox passwordBox)
        {
            passwordBox.OnPasswordChanged();
        }
    }

    /// <summary>
    /// Handles changes to the <see cref="PasswordChar"/> dependency property.
    /// </summary>
    /// <param name="dependencyObject">The <see cref="DependencyObject"/> instance where the change occurred.</param>
    /// <param name="e">Event data that contains information about the property change.</param>
    private static void OnPasswordCharChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        if (dependencyObject is PasswordBox passwordBox)
        {
            passwordBox.OnPasswordCharChanged();
        }
    }

    /// <summary>
    /// Handles changes to the <see cref="IsPasswordRevealed"/> dependency property.
    /// </summary>
    /// <param name="dependencyObject">The <see cref="DependencyObject"/> instance where the property changed.</param>
    /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> containing the old and new values.</param>
    private static void OnIsPasswordRevealedChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        if (dependencyObject is PasswordBox passwordBox)
        {
            passwordBox.OnIsPasswordRevealedChanged();
        }
    }
}