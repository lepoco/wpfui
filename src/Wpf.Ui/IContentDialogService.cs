// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace Wpf.Ui;

/// <summary>
/// Represents a contract with the service that creates <see cref="ContentDialog"/>.
/// </summary>
public interface IContentDialogService
{
    /// <summary>
    /// Sets the <see cref="ContentPresenter"/>
    /// </summary>
    /// <param name="contentPresenter"><see cref="ContentPresenter"/> inside of which the dialogue will be placed. The new <see cref="ContentDialog"/> will replace the current <see cref="ContentPresenter.Content"/>.</param>
    void SetContentPresenter(ContentPresenter contentPresenter);

    /// <summary>
    /// Provides direct access to the <see cref="ContentPresenter"/>
    /// </summary>
    /// <returns>Reference to the currently selected <see cref="ContentPresenter"/> which displays the <see cref="ContentDialog"/>'s.</returns>
    ContentPresenter GetContentPresenter();

    /// <summary>
    /// Shows alert dialog.
    /// </summary>
    /// <returns>Result of the life cycle of the <see cref="ContentDialog"/>.</returns>
    Task<ContentDialogResult> ShowAlertAsync(
        string title,
        string message,
        string closeButtonText,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Shows simple dialog
    /// </summary>
    /// <param name="options">Set of parameters of the basic dialog box.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Result of the life cycle of the <see cref="ContentDialog"/>.</returns>
    Task<ContentDialogResult> ShowSimpleDialogAsync(
        SimpleContentDialogCreateOptions options,
        CancellationToken cancellationToken = default
    );
}
