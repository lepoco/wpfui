// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Controls.Interfaces;

namespace Wpf.Ui.Mvvm.Contracts;

/// <summary>
/// Represents a contract with the service that provides global <see cref="IDialogControl"/>.
/// </summary>
public interface IDialogService
{
    /// <summary>
    /// Sets the <see cref="IDialogControl"/>
    /// </summary>
    /// <param name="dialog"></param>
    void SetDialogControl(IDialogControl dialog);

    /// <summary>
    /// Provides direct access to the <see cref="IDialogControl"/>
    /// </summary>
    /// <returns>Instance of the <see cref="IDialogControl"/> control.</returns>
    IDialogControl GetDialogControl();
}
