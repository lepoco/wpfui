// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Input;

namespace Wpf.Ui.Common;

/// <summary>
/// A command whose sole purpose is to relay its functionality to other objects by invoking delegates. The
/// default return value for the <see cref="CanExecute"/> method is <see langword="true"/>.
/// </summary>
public interface IRelayCommand : ICommand
{
    /// <summary>
    /// Gets encapsulated canExecute function.
    /// </summary>
    /// <param name="parameter"></param>
    public new bool CanExecute(object parameter);

    /// <summary>
    /// Triggers action with provided parameter.
    /// </summary>
    /// <param name="parameter">Argument to be passed.</param>
    public new void Execute(object parameter);
}
