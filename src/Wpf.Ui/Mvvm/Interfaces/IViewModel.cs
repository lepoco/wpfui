// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using Wpf.Ui.Controls.Interfaces;

namespace Wpf.Ui.Mvvm.Interfaces;

/// <summary>
/// Represents a model used in views.
/// </summary>
public interface IViewModel
{
    /// <summary>
    /// Triggered when the model is mounted to the view using the <see cref="INavigation"/>.
    /// </summary>
    /// <param name="parentElement"></param>
    public void OnMounted(FrameworkElement parentElement);
}
