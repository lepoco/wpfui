// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows.Input;

namespace Wpf.Ui.Common;

/// <summary>
/// A generic interface representing a more specific version of <see cref="IRelayCommand"/>.
/// </summary>
/// <typeparam name="T">The type used as argument for the interface methods.</typeparam>
public interface IRelayCommand<in T> : IRelayCommand
{
    /// <summary>
    /// Provides a strongly-typed variant of <see cref="ICommand.CanExecute(object)"/>.
    /// </summary>
    /// <param name="parameter">The input parameter.</param>
    /// <returns>Whether or not the current command can be executed.</returns>
    /// <remarks>Use this overload to avoid boxing, if <typeparamref name="T"/> is a value type.</remarks>
    bool CanExecute(T? parameter);

    /// <summary>
    /// Provides a strongly-typed variant of <see cref="ICommand.Execute(object)"/>.
    /// </summary>
    /// <param name="parameter">The input parameter.</param>
    /// <remarks>Use this overload to avoid boxing, if <typeparamref name="T"/> is a value type.</remarks>
    void Execute(T? parameter);
}
