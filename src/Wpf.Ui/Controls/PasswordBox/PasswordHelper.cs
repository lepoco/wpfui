// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;

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
        /// <param name="textChanges">The text changes.</param>
        /// <returns>The updated password string.</returns>
        /// <remarks>
        /// Handles three scenarios:
        /// 1. When text is being deleted
        /// 2. When password is revealed (plain text mode)
        /// 3. When password is hidden (masked character mode)
        /// </remarks>
        public string GetNewPassword(ICollection<TextChange> textChanges)
        {
            _currentPassword = _passwordBox.Password;
            _newPassword = _currentPassword;
            _currentText = _passwordBox.Text;

            if (_passwordBox.IsPasswordRevealed)
            {
                _newPassword = _currentText;
                return _newPassword;
            }

            return HandleHiddenModeChanges(textChanges);
        }

        /// <summary>
        /// Handles password changes when in hidden (masked) mode.
        /// </summary>
        /// <param name="textChanges">The text changes.</param>
        /// <returns>The updated password string.</returns>
        private string HandleHiddenModeChanges(ICollection<TextChange> textChanges)
        {
            string password = _currentPassword ?? "";

            foreach (TextChange textChange in textChanges)
            {
                if (textChange.RemovedLength > 0)
                {
                    password = password.Remove(textChange.Offset, textChange.RemovedLength);
                }

                if (textChange.AddedLength > 0)
                {
                    string insertedText = _currentText.Substring(textChange.Offset, textChange.AddedLength);
                    password = password.Insert(textChange.Offset, insertedText);
                }
            }

            return password;
        }
    }
}
