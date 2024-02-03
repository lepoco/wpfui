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
public class ContentDialogService : IContentDialogService
{
    private ContentPresenter? _contentPresenter;

    /// <inheritdoc/>
    public void SetContentPresenter(ContentPresenter contentPresenter)
    {
        _contentPresenter = contentPresenter;
    }

    /// <inheritdoc/>
    public ContentPresenter GetContentPresenter()
    {
        if (_contentPresenter is null)
        {
            throw new ArgumentNullException($"The ContentPresenter didn't set previously.");
        }

        return _contentPresenter;
    }

    /// <inheritdoc/>
    public Task<ContentDialogResult> ShowAsync(ContentDialog dialog, CancellationToken cancellationToken)
    {
        if (_contentPresenter is null)
        {
            throw new ArgumentNullException($"The ContentPresenter didn't set previously.");
        }

        return dialog.ShowAsync(cancellationToken);
    }
}
