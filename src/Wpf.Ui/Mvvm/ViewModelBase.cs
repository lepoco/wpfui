// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

#nullable enable

using System.ComponentModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using Wpf.Ui.Common;
using Wpf.Ui.Mvvm.Interfaces;

namespace Wpf.Ui.Mvvm;

/// <summary>
/// Contains a base implementation of methods that facilitate the creation of properties for views.
/// </summary>
public abstract class ViewModelBase : ObservableObject, IViewModel, INotifyPropertyChanged
{
    /// <summary>
    /// Command which raises the <see cref="OnViewCommand"/>.
    /// </summary>
    public IRelayCommand ViewCommand { get; set; }

    /// <summary>
    /// Creates new instance of the class and defines the property container.
    /// </summary>
    public ViewModelBase()
    {
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
}
