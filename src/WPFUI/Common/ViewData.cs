// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.ComponentModel;

namespace WPFUI.Common;

/// <summary>
/// Simple tool that makes it easy to notify a view of changes.
/// </summary>
public class ViewData : INotifyPropertyChanged
{
    /// <inheritdoc />
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Updates property by reference if value was changed.
    /// </summary>
    protected virtual void UpdateProperty<T>(ref T property, object value, string propertyName)
    {
        if (property == null || property.Equals(value))
            return;

        property = (T)value;

        OnPropertyChanged(propertyName);
    }

    protected virtual void OnPropertyChanged(string name)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

