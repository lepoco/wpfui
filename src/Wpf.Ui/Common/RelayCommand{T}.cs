// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// This file is inspired from the MvvmLight library (lbugnion/MvvmLight)

using System;
using System.Runtime.CompilerServices;

namespace Wpf.Ui.Common;

/// <summary>
/// A generic command whose sole purpose is to relay its functionality to other
/// objects by invoking delegates. The default return value for the CanExecute
/// method is <see langword="true"/>. This class allows you to accept command parameters
/// in the <see cref="Execute(T)"/> and <see cref="CanExecute(T)"/> callback methods.
/// </summary>
/// <typeparam name="T">The type of parameter being passed as input to the callbacks.</typeparam>
public class RelayCommand<T> : IRelayCommand<T>
{
    /// <summary>
    /// The <see cref="Action"/> to invoke when <see cref="Execute(T)"/> is used.
    /// </summary>
    private readonly Action<T?> execute;

    /// <summary>
    /// The optional action to invoke when <see cref="CanExecute(T)"/> is used.
    /// </summary>
    private readonly Predicate<T?>? canExecute;

    /// <inheritdoc/>
    public event EventHandler? CanExecuteChanged;

    /// <summary>
    /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class that can always execute.
    /// </summary>
    /// <param name="execute">The execution logic.</param>
    /// <remarks>
    /// Due to the fact that the <see cref="System.Windows.Input.ICommand"/> interface exposes methods that accept a
    /// nullable <see cref="object"/> parameter, it is recommended that if <typeparamref name="T"/> is a reference type,
    /// you should always declare it as nullable, and to always perform checks within <paramref name="execute"/>.
    /// </remarks>
    /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="execute"/> is <see langword="null"/>.</exception>
    public RelayCommand(Action<T?> execute)
    {
        if (execute is null)
            throw new ArgumentNullException(nameof(execute));

        this.execute = execute;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class.
    /// </summary>
    /// <param name="execute">The execution logic.</param>
    /// <param name="canExecute">The execution status logic.</param>
    /// <remarks>See notes in <see cref="RelayCommand{T}(Action{T})"/>.</remarks>
    /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="execute"/> or <paramref name="canExecute"/> are <see langword="null"/>.</exception>
    public RelayCommand(Action<T?> execute, Predicate<T?> canExecute)
    {
        if (execute is null)
            throw new ArgumentNullException(nameof(execute));

        if (canExecute is null)
            throw new ArgumentNullException(nameof(canExecute));

        this.execute = execute;
        this.canExecute = canExecute;
    }

    /// <inheritdoc/>
    public void NotifyCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool CanExecute(T? parameter)
    {
        return this.canExecute?.Invoke(parameter) != false;
    }

    /// <inheritdoc/>
    public bool CanExecute(object? parameter)
    {
        // Special case a null value for a value type argument type.
        // This ensures that no exceptions are thrown during initialization.
        if (parameter is null && default(T) is not null)
        {
            return false;
        }

        if (!TryGetCommandArgument(parameter, out T? result))
        {
            ThrowArgumentExceptionForInvalidCommandArgument(parameter);
        }

        return CanExecute(result);
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Execute(T? parameter)
    {
        this.execute(parameter);
    }

    /// <inheritdoc/>
    public void Execute(object? parameter)
    {
        if (!TryGetCommandArgument(parameter, out T? result))
        {
            ThrowArgumentExceptionForInvalidCommandArgument(parameter);
        }

        Execute(result);
    }

    /// <summary>
    /// Tries to get a command argument of compatible type <typeparamref name="T"/> from an input <see cref="object"/>.
    /// </summary>
    /// <param name="parameter">The input parameter.</param>
    /// <param name="result">The resulting <typeparamref name="T"/> value, if any.</param>
    /// <returns>Whether or not a compatible command argument could be retrieved.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool TryGetCommandArgument(object? parameter, out T? result)
    {
        // If the argument is null and the default value of T is also null, then the
        // argument is valid. T might be a reference type or a nullable value type.
        if (parameter is null && default(T) is null)
        {
            result = default;

            return true;
        }

        // Check if the argument is a T value, so either an instance of a type or a derived
        // type of T is a reference type, an interface implementation if T is an interface,
        // or a boxed value type in case T was a value type.
        if (parameter is T argument)
        {
            result = argument;

            return true;
        }

        result = default;

        return false;
    }

    /// <summary>
    /// Throws an <see cref="ArgumentException"/> if an invalid command argument is used.
    /// </summary>
    /// <param name="parameter">The input parameter.</param>
    /// <exception cref="ArgumentException">Thrown with an error message to give info on the invalid parameter.</exception>
    internal static void ThrowArgumentExceptionForInvalidCommandArgument(object? parameter)
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        static Exception GetException(object? parameter)
        {
            if (parameter is null)
            {
                return new ArgumentException($"Parameter \"{nameof(parameter)}\" (object) must not be null, as the command type requires an argument of type {typeof(T)}.", nameof(parameter));
            }

            return new ArgumentException($"Parameter \"{nameof(parameter)}\" (object) cannot be of type {parameter.GetType()}, as the command type requires an argument of type {typeof(T)}.", nameof(parameter));
        }

        throw GetException(parameter);
    }
}
