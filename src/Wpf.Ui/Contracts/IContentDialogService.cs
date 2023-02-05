// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.ContentDialogControl;

namespace Wpf.Ui.Contracts;

/// <summary>
/// Represents a contract with the service that creates <see cref="ContentDialog"/>.
/// </summary>
public interface IContentDialogService
{
    /// <summary>
    /// Sets the <see cref="ContentPresenter"/>
    /// </summary>
    /// <param name="contentPresenter"></param>
    void SetContentPresenter(ContentPresenter  contentPresenter);

    /// <summary>
    /// Provides direct access to the <see cref="ContentPresenter"/>
    /// </summary>
    /// <returns></returns>
    ContentPresenter GetContentPresenter();

    /// <summary>
    /// Creates a new <see cref="ContentDialog"/>
    /// </summary>
    /// <returns></returns>
    ContentDialog CreateDialog();
}
