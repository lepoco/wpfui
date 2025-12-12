// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

#pragma warning disable IDE0008 // Use explicit type instead of 'var'

/// <summary>
/// Provides attached behavior helpers for dialog hosts implemented with <see cref="ContentPresenter"/>.
///
/// Exposes attached properties to enable the behavior on any <see cref="ContentPresenter"/>,
/// which will manage a <see cref="ContentDialogHostController"/> for runtime dialog lifecycle handling:
/// - monitor <see cref="ContentControl.ContentProperty"/> changes
/// - invoke controller callbacks when a dialog is added/removed
/// - propagate the <see cref="IsDisableSiblingsEnabledProperty"/> setting to the controller
///
/// Typical usage: attach `ContentDialogHostBehavior.IsEnabled="True"` to a <see cref="ContentPresenter"/>
/// placed in the window to enable dialog isolation behavior for dialogs injected into that presenter.
/// </summary>
public static class ContentDialogHostBehavior
{
    /// <summary>
    /// Descriptor for listening to content changes on <see cref="ContentControl"/> instances.
    /// </summary>
    private static readonly DependencyPropertyDescriptor ContentPropertyDescriptor =
        DependencyPropertyDescriptor.FromProperty(ContentControl.ContentProperty, typeof(ContentControl))!;

    /// <summary>
    /// Attached property that enables the behavior on a <see cref="ContentPresenter"/> when set to <see langword="true"/>.
    /// When enabled the behavior will create and manage an internal controller that reacts to Content changes.
    /// </summary>
    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
        "IsEnabled",
        typeof(bool),
        typeof(ContentDialogHostBehavior),
        new PropertyMetadata(false, OnIsEnabledChanged)
    );

    /// <summary>
    /// Attached property which controls whether sibling elements should be disabled while a dialog is active.
    /// The value is forwarded to the internal controller managed by the behavior.
    /// </summary>
    public static readonly DependencyProperty IsDisableSiblingsEnabledProperty = DependencyProperty.RegisterAttached(
        "IsDisableSiblingsEnabled",
        typeof(bool),
        typeof(ContentDialogHostBehavior),
        new PropertyMetadata(false, OnIsDisableSiblingsEnabledChanged)
    );

    private static readonly DependencyProperty StateProperty = DependencyProperty.RegisterAttached(
        "State",
        typeof(BehaviorState),
        typeof(ContentDialogHostBehavior),
        new PropertyMetadata(null)
    );

    /// <summary>
    /// Helper for setting <see cref="IsEnabledProperty"/> on <paramref name="element"/>.
    /// </summary>
    /// <param name="element">
    /// <see cref="ContentPresenter"/> to set <see cref="IsEnabledProperty"/> on.
    /// </param>
    /// <param name="value">
    /// IsEnabled property value.
    /// </param>
    public static void SetIsEnabled(ContentPresenter element, bool value)
    {
        element.SetValue(IsEnabledProperty, value);
    }

    /// <summary>
    /// Helper for getting <see cref="IsEnabledProperty"/> from <paramref name="element"/>.
    /// </summary>
    /// <param name="element">
    /// <see cref="ContentPresenter"/> to read <see cref="IsEnabledProperty"/> from.
    /// </param>
    /// <returns>
    /// IsEnabled property value.
    /// </returns>
    [AttachedPropertyBrowsableForType(typeof(ContentPresenter))]
    public static bool GetIsEnabled(ContentPresenter element)
    {
        return (bool)element.GetValue(IsEnabledProperty);
    }

    /// <summary>
    /// Helper for setting <see cref="IsDisableSiblingsEnabledProperty"/> on <paramref name="element"/>.
    /// </summary>
    /// <param name="element"><see cref="ContentPresenter"/> to set <see cref="IsDisableSiblingsEnabledProperty"/> on.</param>
    /// <param name="value">IsDisableSiblingsEnabled property value.</param>
    public static void SetIsDisableSiblingsEnabled(ContentPresenter element, bool value)
    {
        element.SetValue(IsDisableSiblingsEnabledProperty, value);
    }

    /// <summary>
    /// Helper for getting <see cref="IsDisableSiblingsEnabledProperty"/> from <paramref name="element"/>.
    /// </summary>
    /// <param name="element"><see cref="ContentPresenter"/> to read <see cref="IsDisableSiblingsEnabledProperty"/> from.</param>
    /// <returns>IsDisableSiblingsEnabled property value.</returns>
    [AttachedPropertyBrowsableForType(typeof(ContentPresenter))]
    public static bool GetIsDisableSiblingsEnabled(ContentPresenter element)
    {
        return (bool)element.GetValue(IsDisableSiblingsEnabledProperty);
    }

    private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not ContentPresenter presenter)
        {
            return;
        }

        if ((bool)e.NewValue)
        {
            var state = (BehaviorState?)presenter.GetValue(StateProperty);
            if (state == null)
            {
                state = new BehaviorState(presenter);
                presenter.SetValue(StateProperty, state);
            }

            state.Start();
            return;
        }

        if (presenter.GetValue(StateProperty) is BehaviorState existing)
        {
            existing.Stop();
            presenter.ClearValue(StateProperty);
        }
    }

    private static void OnIsDisableSiblingsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ContentPresenter presenter && presenter.GetValue(StateProperty) is BehaviorState state)
        {
            state.IsDisableSiblingsEnabled = (bool)e.NewValue;
        }
    }

    /// <summary>
    /// Internal state object created per-enabled presenter. Manages the lifecycle of the underlying controller
    /// and subscribes to content changes to trigger controller callbacks.
    /// </summary>
    private sealed class BehaviorState
    {
        private readonly ContentPresenter _host;
        private readonly ContentDialogHostController _controller;
        private object? _currentContent;
        private bool _isStarted;

        public BehaviorState(ContentPresenter host)
        {
            _host = host;
            _controller = new ContentDialogHostController(host);
        }

        /// <summary>
        /// Gets or sets a value indicating whether sibling elements should be disabled while a dialog is active.
        /// </summary>
        /// <remarks>
        /// This setting is forwarded to dialogs displayed in this host.
        /// </remarks>
        public bool IsDisableSiblingsEnabled
        {
            get => _controller.IsDisableSiblingsEnabled;
            set => _controller.IsDisableSiblingsEnabled = value;
        }

        /// <summary>
        /// Initializes monitoring of the host's content and prepares the controller to handle dialog-related events.
        /// </summary>
        public void Start()
        {
            if (_isStarted)
            {
                return;
            }

            _currentContent = _host.Content;
            IsDisableSiblingsEnabled = GetIsDisableSiblingsEnabled(_host);
            ContentPropertyDescriptor.AddValueChanged(_host, OnContentChanged);

            if (_currentContent != null)
            {
                _controller.HandleDialogAdded();
            }

            _isStarted = true;
        }

        /// <summary>
        /// Stops monitoring content changes and deactivates any active dialog associated with the host.
        /// </summary>
        public void Stop()
        {
            if (!_isStarted)
            {
                return;
            }

            ContentPropertyDescriptor.RemoveValueChanged(_host, OnContentChanged);

            if (_controller.IsDialogActive)
            {
                _controller.HandleDialogRemoved();
            }

            _isStarted = false;
        }

        private void OnContentChanged(object? sender, EventArgs e)
        {
            var newContent = _host.Content;
            var oldContent = _currentContent;

            if (oldContent == null && newContent != null)
            {
                _controller.HandleDialogAdded();
            }
            else if (oldContent != null && newContent == null)
            {
                _controller.HandleDialogRemoved();
            }

            _currentContent = newContent;
        }
    }
}

#pragma warning restore IDE0008 // Use explicit type instead of 'var'
