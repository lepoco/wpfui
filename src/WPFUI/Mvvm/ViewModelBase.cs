// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

#nullable enable

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using WPFUI.Common;
using WPFUI.Mvvm.Interfaces;

namespace WPFUI.Mvvm;

/// <summary>
/// Contains a base implementation of methods that facilitate the creation of properties for views.
/// </summary>
public abstract class ViewModelBase : IViewModel, INotifyPropertyChanged
{
    /// <summary>
    /// Contains view data.
    /// </summary>
    private volatile IDictionary<string, object?> _propertiesCollectionObjects;

    /// <summary>
    /// Gets or sets a value indicating whether the properties of the <see cref="ViewModelBase"/> should be thread independent.
    /// </summary>
    public bool Concurrent { get; set; } = false;

    /// <summary>
    /// Command which raises the <see cref="OnViewCommand"/>.
    /// </summary>
    public IRelayCommand ViewCommand { get; set; }

    /// <summary>
    /// Raised when a property is changed.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Creates new instance of the class and defines the property container.
    /// </summary>
    public ViewModelBase()
    {
        if (Concurrent)
            _propertiesCollectionObjects = new ConcurrentDictionary<string, object?>();
        else
            _propertiesCollectionObjects = new Dictionary<string, object?>();

        ViewCommand = new RelayCommand(OnViewCommand);
    }

    /// <inheritdoc />
    public virtual void OnMounted(FrameworkElement parentElement)
    {
    }

    /// <summary>
    /// Triggered when a command is triggered by a <see cref="ViewCommand"/>.
    /// </summary>
    /// <param name="parameter">Passed parameter.</param>
    protected virtual void OnViewCommand(object? parameter = null)
    {
    }

    /// <summary>
    /// Sets a value of the calling property based on it's name.
    /// <para>Does not send a change notification if the new value is the same as the current one.</para>
    /// </summary>
    /// <param name="value">Value to set.</param>
    /// <param name="callingMemberPropertyName">Calling property name.</param>
    protected void SetValue(object value, [CallerMemberName] string? callingMemberPropertyName = null)
    {
        if (String.IsNullOrWhiteSpace(callingMemberPropertyName))
            return;

        if (_propertiesCollectionObjects.TryGetValue(callingMemberPropertyName, out var currentValue))
        {
            if (currentValue == value)
                return;

            _propertiesCollectionObjects[callingMemberPropertyName] = value;

            OnPropertyChanged(callingMemberPropertyName);

            return;
        }

        _propertiesCollectionObjects.Add(callingMemberPropertyName, value);

        OnPropertyChanged(callingMemberPropertyName);
    }

    /// <summary>
    /// Gets a value of the calling property based on it's name.
    /// </summary>
    /// <param name="callingMemberPropertyName">Calling property name.</param>
    /// <returns>Property value or <see langword="null"/></returns>
    protected object? GetValue([CallerMemberName] string? callingMemberPropertyName = null)
    {
        if (String.IsNullOrWhiteSpace(callingMemberPropertyName))
            return null;

        if (!_propertiesCollectionObjects.TryGetValue(callingMemberPropertyName, out var currentValue))
            return null;

        return currentValue ?? null;
    }

    /// <summary>
    /// Gets a value with the specified class of the calling property based on it's name.
    /// </summary>
    /// <param name="callingMemberPropertyName">Calling property name.</param>
    /// <returns>Property value or <see langword="null"/></returns>
    protected T? GetValue<T>([CallerMemberName] string? callingMemberPropertyName = null) where T : class
    {
        if (String.IsNullOrWhiteSpace(callingMemberPropertyName))
            return null;

        if (!_propertiesCollectionObjects.TryGetValue(callingMemberPropertyName, out var currentValue))
            return null;

        return (T?)currentValue ?? null;
    }

    /// <summary>
    /// Gets a value with the specified class of the calling property based on it's name.
    /// </summary>
    /// <param name="defaultValue">Value returned if property is <see langword="null"/>.</param>
    /// <param name="callingMemberPropertyName">Calling property name.</param>
    /// <returns>Property value or provided <c>defaultValue</c></returns>
    protected T GetValueOrDefault<T>(T defaultValue, [CallerMemberName] string callingMemberPropertyName = null) where T : class
    {
        if (String.IsNullOrWhiteSpace(callingMemberPropertyName))
            return defaultValue;

        if (!_propertiesCollectionObjects.TryGetValue(callingMemberPropertyName, out var currentValue))
            return defaultValue;

        return (T)(currentValue ?? defaultValue);
    }

    /// <summary>
    /// Gets a value with the specified convertible class of the calling property based on it's name.
    /// </summary>
    /// <param name="defaultValue">Value returned if property is <see langword="null"/>.</param>
    /// <param name="callingMemberPropertyName">Calling property name.</param>
    /// <returns>Property value or provided <c>defaultValue</c></returns>
    protected T GetStructOrDefault<T>(T defaultValue, [CallerMemberName] string? callingMemberPropertyName = null) where T : struct, IComparable, IConvertible
    {
        if (String.IsNullOrWhiteSpace(callingMemberPropertyName))
            return defaultValue;

        if (!_propertiesCollectionObjects.TryGetValue(callingMemberPropertyName, out var currentValue))
            return defaultValue;

        return (T)(currentValue ?? defaultValue);
    }

    /// <summary>
    /// Raised notify event for selected property.
    /// </summary>
    /// <param name="propertyName">The property about which the notification is to be sent.</param>
    protected virtual void OnPropertyChanged(string propertyName)
    {
        if (String.IsNullOrWhiteSpace(propertyName))
            return;

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
