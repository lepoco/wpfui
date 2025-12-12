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
/// &lt;ContentDialogHost x:Name="RootContentDialogPresenter" Grid.Row="0" /&gt;
/// </code>
/// <code lang="csharp">
/// IContentDialogService contentDialogService = new ContentDialogService();
/// contentDialogService.SetContentPresenter(RootContentDialogPresenter);
///
/// await _contentDialogService.ShowAsync(
///     new ContentDialog(){
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
    [Obsolete("Use SetDialogHost instead.")]
    void SetContentPresenter(ContentPresenter contentPresenter);

    [Obsolete("Use GetDialogHost instead.")]
    ContentPresenter? GetContentPresenter();

    /// <summary>
    /// Sets the <see cref="ContentPresenter"/>
    /// </summary>
    /// <param name="dialogHost"><see cref="ContentPresenter"/> inside of which the dialogue will be placed. The new <see cref="ContentDialog"/> will replace the current <see cref="ContentPresenter.Content"/>.</param>
    /// <remarks>
    /// DEPRECATED: This method is obsolete.
    /// Use <see cref="SetDialogHost(ContentDialogHost)"/> instead.
    /// </remarks>
    [Obsolete("SetDialogHost(ContentPresenter) is deprecated. Use SetDialogHost(ContentDialogHost) instead for better modal features.")]
    void SetDialogHost(ContentPresenter dialogHost);

    /// <summary>
    /// Sets the <see cref="ContentDialogHost"/> that will host and present content dialogs.
    /// </summary>
    /// <param name="dialogHost">
    /// The <see cref="ContentDialogHost"/> instance to use for dialog presentation.
    /// </param>
    void SetDialogHost(ContentDialogHost dialogHost);

    /// <summary>
    /// Provides direct access to the <see cref="ContentPresenter"/>
    /// </summary>
    /// <returns>Reference to the currently selected <see cref="ContentPresenter"/> which displays the <see cref="ContentDialog"/>'s.</returns>
    /// <remarks>
    /// DEPRECATED: This method is obsolete.
    /// Use <see cref="GetDialogHostEx"/> instead.
    /// </remarks>
    [Obsolete("Use GetDialogHostEx() instead to access enhanced modal features.", false)]
    ContentPresenter? GetDialogHost();

    /// <summary>
    /// Gets the <see cref="ContentDialogHost"/> currently associated with the content.
    /// </summary>
    /// <returns>
    /// The associated <see cref="ContentDialogHost"/> instance, or <see langword="null"/>
    /// if no dialog host is currently assigned.
    /// </returns>
    ContentDialogHost? GetDialogHostEx();

    /// <summary>
    /// Asynchronously shows the specified dialog.
    /// </summary>
    /// <param name="dialog">The dialog to be displayed.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the dialog result.</returns>
    Task<ContentDialogResult> ShowAsync(ContentDialog dialog, CancellationToken cancellationToken);
}
