// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
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
    /// Shows alert dialog.
    /// </summary>
    /// <returns></returns>
    Task<ContentDialogResult> ShowAlertAsync(string title, string message, string closeButtonText, CancellationToken cancellationToken = default);

    /// <summary>
    /// Shows simple dialog
    /// </summary>
    /// <param name="options"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ContentDialogResult> ShowSimpleDialogAsync(SimpleContentDialogCreateOptions options, CancellationToken cancellationToken = default);
}

public class SimpleContentDialogCreateOptions
{
    public required string Title { get; set; }
    public required object Content { get; set; }
    public required string CloseButtonText { get; set; }

    public string PrimaryButtonText { get; set; } = string.Empty;
    public string SecondaryButtonText { get; set; } = string.Empty;
}
