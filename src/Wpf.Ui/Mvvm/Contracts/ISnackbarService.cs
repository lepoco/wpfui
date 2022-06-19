// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Threading.Tasks;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;

namespace Wpf.Ui.Mvvm.Contracts;

/// <summary>
/// Represents a contract with the service that provides global <see cref="Wpf.Ui.Controls.Snackbar"/>.
/// </summary>
public interface ISnackbarService
{
    /// <summary>
    /// Sets the <see cref="Snackbar"/> 
    /// </summary>
    /// <param name="snackbar"></param>
    void SetSnackbarControl(Snackbar snackbar);

    /// <summary>
    /// Sets <see cref="Snackbar.Title"/> and <see cref="Snackbar.Message"/>, then shows the <see cref="Snackbar"/> for the amount of time specified in <see cref="Snackbar.Timeout"/>.
    /// </summary>
    /// <param name="title"></param>
    /// <param name="message"></param>
    void ShowSnackbar(string title, string message);


    /// <summary>
    /// Sets <see cref="Snackbar.Title"/>, <see cref="Snackbar.Message"/>, <see cref="Snackbar.Icon"/> then shows the <see cref="Snackbar"/> for the amount of time specified in <see cref="Snackbar.Timeout"/>.
    /// </summary>
    /// <param name="title"></param>
    /// <param name="message"></param>
    /// <param name="icon"></param>
    void ShowSnackbar(string title, string message, SymbolRegular icon);

    /// <summary>
    /// Asynchronously sets <see cref="Snackbar.Title"/> and <see cref="Snackbar.Message"/>, then shows the <see cref="Snackbar"/> for the amount of time specified in <see cref="Snackbar.Timeout"/>.
    /// </summary>
    /// <param name="title"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    Task<bool> ShowSnackbarAsync(string title, string message);

    /// <summary>
    /// Asynchronously sets <see cref="Snackbar.Title"/>, <see cref="Snackbar.Message"/>, <see cref="Snackbar.Icon"/> then shows the <see cref="Snackbar"/> for the amount of time specified in <see cref="Snackbar.Timeout"/>.
    /// </summary>
    /// <param name="title"></param>
    /// <param name="message"></param>
    /// <param name="icon"></param>
    /// <returns></returns>
    Task<bool> ShowSnackbarAsync(string title, string message, SymbolRegular icon);
}
