// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using WPFUI.Common;

namespace WPFUI.Mvvm.Contracts;

/// <summary>
/// Represents a contract with the service that provides global <see cref="WPFUI.Controls.Snackbar"/>.
/// </summary>
public interface ISnackbarService
{
    void ShowSnackbar(string title);

    void ShowSnackbar(string title, string message);

    void ShowSnackbar(string title, string message, SymbolRegular icon);

    void CloseSnackbar();
}
