// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows.Input;

namespace Wpf.Ui.Input;

/// <summary>
/// An interface expanding <see cref="ICommand"/> with the ability to raise
/// the <see cref="ICommand.CanExecuteChanged"/> event externally.
/// </summary>
public interface IRelayCommand : ICommand
{
    /// <summary>
    /// Notifies that the <see cref="ICommand.CanExecute"/> property has changed.
    /// </summary>
    void NotifyCanExecuteChanged();
}
