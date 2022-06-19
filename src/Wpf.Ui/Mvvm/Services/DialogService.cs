// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

#nullable enable

using System;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace Wpf.Ui.Mvvm.Services;

/// <summary>
/// A service that provides methods related to displaying the <see cref="IDialogControl"/>.
/// </summary>
public class DialogService : IDialogService
{
    private IDialogControl? _dialogControl;

    /// <inheritdoc />
    public void SetDialogControl(IDialogControl dialog)
    {
        _dialogControl = dialog;
    }

    /// <inheritdoc />
    public IDialogControl GetDialogControl()
    {
        if (_dialogControl is null)
            throw new InvalidOperationException(
                $"The ${typeof(DialogService)} cannot be used unless previously defined with {typeof(IDialogControl)}.{nameof(SetDialogControl)}().");

        return _dialogControl;
    }
}
