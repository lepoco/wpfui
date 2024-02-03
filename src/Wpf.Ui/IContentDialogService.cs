// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace Wpf.Ui;

/// <summary>
/// Represents a contract with the service that creates <see cref="ContentDialog"/>.
/// </summary>
/// <example>
/// <code lang="xml">
/// &lt;ContentPresenter x:Name="RootContentDialogPresenter" Grid.Row="0" /&gt;
/// </code>
/// <code lang="csharp">
/// IContentDialogService contentDialogService = new ContentDialogService();
/// contentDialogService.SetContentPresenter(RootContentDialogPresenter);
///
/// await _contentDialogService.ShowAsync(
///     new ContentDialog(_contentDialogService.GetContentPresenter()){
///         Title = "The cake?",
///         Content = "IS A LIE!",
///         PrimaryButtonText = "Save",
///         SecondaryButtonText = "Don't Save",
///         CloseButtonText = "Cancel"
///     }
/// );
/// </code>
/// </example>
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
    /// Asynchronously shows the specified dialog.
    /// </summary>
    /// <param name="dialog">The dialog to be displayed.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the dialog result.</returns>
    Task<ContentDialogResult> ShowAsync(ContentDialog dialog, CancellationToken cancellationToken);
}
