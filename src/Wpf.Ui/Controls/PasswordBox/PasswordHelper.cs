// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Diagnostics;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

#pragma warning disable SA1601

public partial class PasswordBox
{
    /// <summary>
    /// Helper class for managing password operations in <see cref="PasswordBox"/>.
    /// </summary>
    private class PasswordHelper
    {
        private readonly PasswordBox _passwordBox;
        private string _currentText;
        private string _newPassword;
        private string _currentPassword;

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordHelper"/> class.
        /// </summary>
        /// <param name="passwordBox">The parent <see cref="PasswordBox"/> control.</param>
        public PasswordHelper(PasswordBox passwordBox)
        {
            _passwordBox = passwordBox;
            _currentText = string.Empty;
            _newPassword = string.Empty;
            _currentPassword = string.Empty;
        }

        /// <summary>
        /// Calculates and returns the new password value based on current input.
        /// </summary>
        /// <returns>The updated password string.</returns>
        /// <remarks>
        /// Handles three scenarios:
        /// 1. When text is being deleted
        /// 2. When password is revealed (plain text mode)
        /// 3. When password is hidden (masked character mode)
        /// </remarks>
        public string GetNewPassword()
        {
            _currentPassword = _passwordBox.Password;
            _newPassword = _currentPassword;
            _currentText = _passwordBox.Text;
            int selectionIndex = _passwordBox.SelectionStart;

            if (IsDeletingText())
            {
                int charsToRemove = _currentPassword.Length - _currentText.Length;
                _newPassword = _currentPassword.Remove(selectionIndex, charsToRemove);
                return _newPassword;
            }

            if (_passwordBox.IsPasswordRevealed)
            {
                _newPassword = _currentText;
                return _newPassword;
            }

            return HandleHiddenModeChanges(selectionIndex);
        }

        /// <summary>
        /// Handles password changes when in hidden (masked) mode.
        /// </summary>
        /// <param name="selectionIndex">Current caret position in the text box.</param>
        /// <returns>The updated password string.</returns>
        /// <remarks>
        /// Manages three cases:
        /// 1. Characters were inserted
        /// 2. Character was replaced (overwrite)
        /// 3. Characters were removed
        /// </remarks>
        private string HandleHiddenModeChanges(int selectionIndex)
        {
            char passwordChar = _passwordBox.PasswordChar;
            int currentLength = _currentPassword.Length;

            if (_currentText.Length > currentLength)
            {
                // Characters were inserted
                int insertedCount = _currentText.Length - currentLength;
                string insertedText = _currentText.Substring(selectionIndex - insertedCount, insertedCount);
                _newPassword = _currentPassword.Insert(selectionIndex - insertedCount, insertedText);
            }
            else if (_currentText.Length == currentLength)
            {
                // Character was replaced (overwrite)
                for (int i = 0; i < _currentText.Length; i++)
                {
                    if (_currentText[i] != passwordChar && i < _newPassword.Length)
                    {
                        _newPassword = _newPassword.Remove(i, 1).Insert(i, _currentText[i].ToString());
                        break;
                    }
                }
            }
            else if (_currentText.Length < currentLength)
            {
                // Characters were removed (fallback)
                int removedCount = currentLength - _currentText.Length;
                _newPassword = _currentPassword.Remove(selectionIndex, removedCount);
            }

            return _newPassword;
        }

        /// <summary>
        /// Determines if the current operation is deleting text.
        /// </summary>
        /// <returns>True if text is being deleted; otherwise, false.</returns>
        private bool IsDeletingText()
        {
            Debug.Assert(_currentText == _passwordBox.Text, "Text mismatch");
            Debug.Assert(_currentPassword == _passwordBox.Password, "Password mismatch");

            return _currentText.Length < _currentPassword.Length;
        }
    }
}
