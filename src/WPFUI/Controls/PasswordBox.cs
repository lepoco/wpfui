// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Controls;
using WPFUI.Common;

namespace WPFUI.Controls;

// TODO: This is an initial implementation and requires the necessary corrections, tests and adjustments.

/// <summary>
/// The modified password control.
/// </summary>
public sealed class PasswordBox : WPFUI.Controls.TextBox
{
    /// <summary>
    /// Blocks triggering overwrite when forced text editing.
    /// </summary>
    private bool _takenControl = false;

    /// <summary>
    /// Property for <see cref="Password"/>.
    /// </summary>
    public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register(nameof(Password),
        typeof(string), typeof(PasswordBox), new PropertyMetadata(String.Empty));

    /// <summary>
    /// Property for <see cref="PasswordChar"/>.
    /// </summary>
    public static readonly DependencyProperty PasswordCharProperty = DependencyProperty.Register(nameof(PasswordChar),
        typeof(char), typeof(PasswordBox), new PropertyMetadata('*', OnPasswordCharChanged));

    /// <summary>
    /// Property for <see cref="PasswordRevealMode"/>.
    /// </summary>
    public static readonly DependencyProperty PasswordRevealModeProperty = DependencyProperty.Register(nameof(PasswordRevealMode),
        typeof(RevealMode), typeof(PasswordBox), new PropertyMetadata(RevealMode.Hidden, OnPasswordRevealModeChanged));

    /// <summary>
    /// Property for <see cref="ShowRevealButton"/>.
    /// </summary>
    public static readonly DependencyProperty ShowRevealButtonProperty = DependencyProperty.Register(nameof(ShowRevealButton),
        typeof(bool), typeof(PasswordBox), new PropertyMetadata(true));

    /// <summary>
    /// Property for <see cref="TemplateButtonCommand"/>.
    /// </summary>
    public static readonly DependencyProperty TemplateButtonCommandProperty =
        DependencyProperty.Register(nameof(TemplateButtonCommand),
            typeof(Common.IRelayCommand), typeof(PasswordBox), new PropertyMetadata(null));

    /// <summary>
    /// Gets or sets currently typed text represented by asterisks.
    /// </summary>
    public string Password
    {
        get => (string)GetValue(PasswordProperty);
        internal set => SetValue(PasswordProperty, value);
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
    /// Gets or sets a value deciding whether the password should be visible as plain text.
    /// </summary>
    public RevealMode PasswordRevealMode
    {
        get => (RevealMode)GetValue(PasswordRevealModeProperty);
        set => SetValue(PasswordRevealModeProperty, value);
    }

    /// <summary>
    /// Gets or sets a value deciding whether to display the reveal password button.
    /// </summary>
    public bool ShowRevealButton
    {
        get => (bool)GetValue(ShowRevealButtonProperty);
        set => SetValue(ShowRevealButtonProperty, value);
    }

    /// <summary>
    /// Contents of the TextBox. Returns asterisks, if you want a valid password use <see cref="Password"/>.
    /// </summary>
    public new string Text
    {
        get => base.Text;
        set
        {
            SetValue(PasswordProperty, value);
            SetValue(TextProperty, new String(PasswordChar, value?.Length ?? 0));
        }
    }

    /// <summary>
    /// Command triggered after clicking the button.
    /// </summary>
    public Common.IRelayCommand TemplateButtonCommand => (Common.IRelayCommand)GetValue(TemplateButtonCommandProperty);

    /// <summary>
    /// Creates new instance and sets default <see cref="TemplateButtonCommandProperty"/>.
    /// </summary>
    public PasswordBox() =>
        SetValue(TemplateButtonCommandProperty, new Common.RelayCommand(o => ButtonOnClick(this, o)));

    /// <summary>
    /// Called when content changes.
    /// <para>Partially inspired by Leonardo T. implementation of SecureWpfLogOn.</para>
    /// </summary>
    /// <param name="e"></param>
    protected override void OnTextChanged(TextChangedEventArgs e)
    {
        if (_takenControl)
            return;

        if (PasswordRevealMode == RevealMode.Visible)
        {
            base.OnTextChanged(e);
            Password = base.Text;

            return;
        }

        string text = base.Text;
        string password = Password;
        int selectionIndex = SelectionStart;

        if (text.Length < password.Length)
        {
            password = password.Remove(selectionIndex, password.Length - text.Length);
            Password = password;
        }

        if (String.IsNullOrEmpty(text))
        {
            base.OnTextChanged(e);
            return;
        }

        // TODO: Pasting text breaks this loop.
        var newContent = text.Replace(PasswordChar.ToString(), String.Empty);

        if (newContent.Length > 1)
        {
            var index = text.IndexOf(newContent[0]);
            Password = index > password.Length - 1 ? password + newContent : password.Insert(index, newContent);
        }
        else
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == PasswordChar)
                    continue;
                Password = text.Length == password.Length ? password.Remove(i, 1).Insert(i, text[i].ToString()) : password.Insert(i, text[i].ToString());
            }
        }

        _takenControl = true;

        base.Text = new String(PasswordChar, Password.Length);
        SelectionStart = selectionIndex;

        _takenControl = false;

        base.OnTextChanged(e);
    }

    /// <summary>
    /// Updates the content of the displayed password if the character is changed.
    /// </summary>
    private void UpdatePasswordWithNewChar(char newChar)
    {
        if (PasswordRevealMode == RevealMode.Visible)
            return;
        base.Text = new String(newChar, base.Text.Length);
    }

    /// <summary>
    /// Change the display of the password if rules are supported.
    /// </summary>
    private void UpdateRevealIfPossible(RevealMode revealMode)
    {
        // TODO: I don't know if it's a good method, but somehow works

        if (revealMode == RevealMode.Visible && Password.Length > 0)
        {
            PasswordRevealMode = RevealMode.Hidden;
            return;
        }

        SetValue(TextProperty,
            revealMode == RevealMode.Visible ? Password : new String(PasswordChar, Password.Length));
    }

    /// <summary>
    /// Triggered by clicking a button in the control template.
    /// </summary>
    /// <param name="sender">Sender of the click event.</param>
    /// <param name="parameter">Additional parameters.</param>
    private void ButtonOnClick(object sender, object parameter)
    {
        if (parameter == null)
            return;

        string param = parameter as string ?? String.Empty;

#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO: {typeof(PasswordBox)} button clicked with param: {param}", "WPFUI.PasswordBox");
#endif

        switch (param)
        {
            case "reveal":
                PasswordRevealMode = PasswordRevealMode == RevealMode.Visible
                    ? RevealMode.Hidden
                    : RevealMode.Visible;

                Focus();
                CaretIndex = Text.Length;

                break;
        }
    }

    /// <summary>
    /// Static method that is called if the character is changed in the during the run.
    /// </summary>
    /// <param name="d">Instance of the <see cref="PasswordBox"/></param>
    /// <param name="e">Various property events.</param>
    private static void OnPasswordCharChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not PasswordBox control)
            return;
        control.UpdatePasswordWithNewChar(control.PasswordChar);
    }

    /// <summary>
    /// Static method that is called if the reveal mode is changed in the during the run.
    /// </summary>
    /// <param name="d">Instance of the <see cref="PasswordBox"/></param>
    /// <param name="e">Various property events.</param>
    private static void OnPasswordRevealModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not PasswordBox control)
            return;
        control.UpdateRevealIfPossible(control.PasswordRevealMode);
    }
}
