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
    private class PasswordHelper
    {
        private readonly PasswordBox _passwordBox;
        private string _currentText;
        private string _newPasswordValue;
        private string _currentPassword;

        public PasswordHelper(PasswordBox passwordBox)
        {
            _passwordBox = passwordBox;
            _currentText = string.Empty;
            _newPasswordValue = string.Empty;
            _currentPassword = string.Empty;
        }

        public string GetNewPassword()
        {
            _currentPassword = GetPassword();
            _newPasswordValue = _currentPassword;
            _currentText = _passwordBox.Text;
            var selectionIndex = _passwordBox.SelectionStart;
            var passwordChar = _passwordBox.PasswordChar;
            var newCharacters = _currentText.Replace(passwordChar.ToString(), string.Empty);
            bool isDeleted = false;

            if (IsDeleteOption())
            {
                _newPasswordValue = _currentPassword.Remove(
                    selectionIndex,
                    _currentPassword.Length - _currentText.Length
                );
                isDeleted = true;
            }

            switch (newCharacters.Length)
            {
                case > 1:
                {
                    var index = _currentText.IndexOf(newCharacters[0]);

                    _newPasswordValue =
                        index > _newPasswordValue.Length - 1
                            ? _newPasswordValue + newCharacters
                            : _newPasswordValue.Insert(index, newCharacters);
                    break;
                }

                case 1:
                {
                    for (int i = 0; i < _currentText.Length; i++)
                    {
                        if (_currentText[i] == passwordChar)
                        {
                            continue;
                        }

                        UpdatePasswordWithInputCharacter(i, _currentText[i].ToString());
                        break;
                    }

                    break;
                }

                case 0 when !isDeleted:
                {
                    // The input is a PasswordChar, which is to be inserted at the designated position.
                    int insertIndex = selectionIndex - 1;
                    UpdatePasswordWithInputCharacter(insertIndex, passwordChar.ToString());
                    break;
                }
            }

            return _newPasswordValue;
        }

        private void UpdatePasswordWithInputCharacter(int insertIndex, string insertValue)
        {
            Debug.Assert(_currentText == _passwordBox.Text, "_currentText == _passwordBox.Text");

            if (_currentText.Length == _newPasswordValue.Length)
            {
                // If it's a direct character replacement, remove the existing one before inserting the new one.
                _newPasswordValue = _newPasswordValue.Remove(insertIndex, 1).Insert(insertIndex, insertValue);
            }
            else
            {
                _newPasswordValue = _newPasswordValue.Insert(insertIndex, insertValue);
            }
        }

        private bool IsDeleteOption()
        {
            Debug.Assert(_currentText == _passwordBox.Text, "_currentText == _passwordBox.Text");
            Debug.Assert(
                _currentPassword == _passwordBox.Password,
                "_currentPassword == _passwordBox.Password"
            );

            return _currentText.Length < _currentPassword.Length;
        }

        public string GetPassword() => _passwordBox.Password ?? string.Empty;
    }
}
