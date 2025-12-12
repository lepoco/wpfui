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
public class ContentDialogService : IContentDialogService
{
    private ContentPresenter? _dialogHost;
    private ContentDialogHost? _dialogHostEx;

    [Obsolete("Use SetDialogHost instead.")]
    public void SetContentPresenter(ContentPresenter contentPresenter)
    {
        SetDialogHost(contentPresenter);
    }

    [Obsolete("Use GetDialogHost instead.")]
    public ContentPresenter? GetContentPresenter()
    {
        return GetDialogHost();
    }

    /// <inheritdoc/>
    [Obsolete("Use SetDialogHost(ContentDialogHost) instead.")]
    public void SetDialogHost(ContentPresenter contentPresenter)
    {
        if (contentPresenter == null)
        {
            throw new ArgumentNullException(nameof(contentPresenter));
        }

        if (_dialogHostEx != null)
        {
            throw new InvalidOperationException(
                "Cannot set ContentPresenter: a ContentDialogHost host has already been set. " +
                "Only one host type is allowed per instance for compatibility."
            );
        }

        _dialogHost = contentPresenter;
    }

    /// <inheritdoc/>
    [Obsolete("Use GetDialogHostEx() instead.")]
    public ContentPresenter? GetDialogHost()
    {
        return _dialogHost;
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="dialogHost"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when a legacy dialog host (ContentPresenter) has already been set via
    /// <see cref="SetDialogHost(ContentPresenter)"/>. Only one host type can be set per instance.
    /// </exception>
    /// <remarks>
    /// <para>
    /// This method sets the enhanced <see cref="ContentDialogHost"/> to contain and manage dialogs.
    /// For compatibility reasons, an instance can have either a legacy host (set via
    /// <see cref="SetDialogHost(ContentPresenter)"/>) or an enhanced host (set via this method),
    /// but not both.
    /// </para>
    /// </remarks>
    public void SetDialogHost(ContentDialogHost dialogHost)
    {
        if (dialogHost == null)
        {
            throw new ArgumentNullException(nameof(dialogHost));
        }

        // Defense mechanism: prevent mixed host types for compatibility
        if (_dialogHost != null)
        {
            throw new InvalidOperationException(
                "Cannot set ContentDialogHost: a legacy ContentPresenter host has already been set. " +
                "Only one host type is allowed per instance for compatibility."
            );
        }

        _dialogHostEx = dialogHost;
    }

    /// <inheritdoc/>
    public ContentDialogHost? GetDialogHostEx()
    {
        return _dialogHostEx;
    }

    /// <inheritdoc/>
    public Task<ContentDialogResult> ShowAsync(ContentDialog dialog, CancellationToken cancellationToken)
    {
#pragma warning disable CS0618 // (Warning: Obsolete) To maintain compatibility

        if (dialog == null)
        {
            throw new ArgumentNullException(nameof(dialog));
        }

        if (_dialogHostEx == null && _dialogHost == null)
        {
            throw new InvalidOperationException("The DialogHost was never set.");
        }

        object? svcHost = _dialogHostEx is not null ? _dialogHostEx : _dialogHost;

        object? dlgHost = dialog.DialogHostEx is not null ? dialog.DialogHostEx : dialog.DialogHost;

        if (dlgHost != null && !ReferenceEquals(dlgHost, svcHost))
        {
            throw new InvalidOperationException(
                "The DialogHost is not the same as the one that was previously set."
            );
        }

        if (_dialogHostEx != null)
        {
            dialog.DialogHostEx = _dialogHostEx;
        }
        else
        {
            dialog.DialogHost = _dialogHost;
        }

        return dialog.ShowAsync(cancellationToken);

#pragma warning restore CS0618 // (Warning: Obsolete)
    }
}
