// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Threading;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

#pragma warning disable IDE0008 // Use explicit type instead of 'var'

/// <summary>
/// Provides a host control for displaying modal content dialogs within a WPF window.
/// Ensures that only one dialog host is registered per window and manages dialog
/// presentation and interaction blocking as needed.
/// </summary>
/// <example>
/// <para>XAML (place near the root of the Window):</para>
/// <code lang="xml">
/// &lt;Window x:Class="MyApp.MainWindow" xmlns:ui="clr-namespace:Wpf.Ui.Controls;assembly=Wpf.Ui"&gt;
///   &lt;Grid&gt;
///     &lt;ui:ContentDialogHost x:Name="RootDialogHost" /&gt;
///   &lt;/Grid&gt;
/// &lt;/Window&gt;
/// </code>
/// <para>C# (showing a simple dialog via the host):</para>
/// <code lang="csharp">
/// var dialog = new ContentDialog(RootDialogHost)
/// {
///     Title = "Confirm",
///     Content = "Are you sure?",
///     PrimaryButtonText = "Yes",
///     CloseButtonText = "No",
/// };
///
/// var result = await dialog.ShowAsync();
/// </code>
/// </example>
/// <remarks>
/// <para>
/// Use this control to present modal dialogs that overlay application content
/// and optionally disable interaction with sibling elements.
/// </para>
/// <para>
/// <strong>Placement Requirements:</strong>
/// 1. Place near the root of the window's visual tree to ensure broad coverage.
/// 2. Position as the last sibling among its peers to guarantee the highest Z-order.
/// </para>
/// <para>
/// Only one instance of <see cref="ContentDialogHost"/> can be registered per <see cref="Window"/>; attempting to
/// register multiple instances will result in an exception. To retrieve the dialog host associated
/// with a specific window, use <see cref="GetForWindow"/>.
/// </para>
/// </remarks>
public class ContentDialogHost : ContentControl
{
    /// <summary>Identifies the <see cref="IsDisableSiblingsEnabled"/> dependency property.</summary>
    public static readonly DependencyProperty IsDisableSiblingsEnabledProperty = DependencyProperty.Register(
        nameof(IsDisableSiblingsEnabled),
        typeof(bool),
        typeof(ContentDialogHost),
        new PropertyMetadata(false, OnIsDisableSiblingsEnabledChanged)
    );

    // Enforce single host per Window
    private static readonly ConditionalWeakTable<Window, ContentDialogHost> WindowHosts = new();

#if NET9_0_OR_GREATER
    private static readonly Lock WindowHostsLock = new();
#else
    private static readonly object WindowHostsLock = new();
#endif

    private readonly ContentDialogHostController _controller;

    static ContentDialogHost()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(ContentDialogHost),
            new FrameworkPropertyMetadata(typeof(ContentDialogHost))
        );
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ContentDialogHost"/> class.
    /// </summary>
    public ContentDialogHost()
    {
        _controller = new ContentDialogHostController(this);

        Loaded += ContentDialogHost_Loaded;
        Unloaded += ContentDialogHost_Unloaded;
    }

    /// <summary>
    /// Gets or sets a value indicating whether sibling elements of the dialog host should be
    /// disabled while the dialog is displayed. The default value is <see langword="false"/>.
    /// </summary>
    /// <value>
    /// <see langword="true"/> to disable sibling elements; <see langword="false"/> to leave
    /// sibling elements unaffected.
    /// </value>
    /// <remarks>
    /// When enabled, sibling elements in the host window may appear disabled while the
    /// <see cref="ContentDialog"/> is displayed. This option is disabled by default and
    /// is intended for scenarios where background interaction must be prevented.
    /// </remarks>
    public bool IsDisableSiblingsEnabled
    {
        get => (bool)GetValue(IsDisableSiblingsEnabledProperty);
        set => SetValue(IsDisableSiblingsEnabledProperty, value);
    }

    /// <summary>
    /// Returns the <see cref="ContentDialogHost"/> instance registered for the specified <see cref="Window"/>, if any.
    /// </summary>
    /// <param name="window">Window to query for a registered <see cref="ContentDialogHost"/>.</param>
    /// <returns>The registered <see cref="ContentDialogHost"/> for the given window, or <see langword="null"/> if none is registered.</returns>
    /// <example>
    /// <code lang="csharp">
    /// var host = ContentDialogHost.GetForWindow(Window.GetWindow(someElement));
    /// </code>
    /// </example>
    public static ContentDialogHost? GetForWindow(Window? window)
    {
        if (window == null)
        {
            return null;
        }

        lock (WindowHostsLock)
        {
            return WindowHosts.TryGetValue(window, out var existing) ? existing : null;
        }
    }

    protected override void OnContentChanged(object? oldContent, object? newContent)
    {
        // Transition: no dialog -> dialog (first open)
        if (oldContent == null && newContent != null)
        {
            _controller.HandleDialogAdded();
            base.OnContentChanged(oldContent, newContent);
            return;
        }

        // Transition: dialog -> no dialog (last close)
        if (oldContent != null && newContent == null)
        {
            base.OnContentChanged(oldContent, newContent);
            _controller.HandleDialogRemoved();
            return;
        }

        // Replacement: oldContent != null && newContent != null
        base.OnContentChanged(oldContent, newContent);
    }

    private static void OnIsDisableSiblingsEnabledChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e
    )
    {
        if (d is ContentDialogHost host)
        {
            host._controller.IsDisableSiblingsEnabled = (bool)e.NewValue;
        }
    }

    private void ContentDialogHost_Loaded(object? sender, RoutedEventArgs e)
    {
        var window = Window.GetWindow(this);
        if (window == null)
        {
            // Try again later if window not available yet
            _ = Dispatcher.BeginInvoke(new Action(RegisterHostForWindow), DispatcherPriority.Loaded);
            return;
        }

        RegisterHost(window);
    }

    private void ContentDialogHost_Unloaded(object? sender, RoutedEventArgs e)
    {
        var window = Window.GetWindow(this);
        if (window == null)
        {
            return;
        }

        lock (WindowHostsLock)
        {
            if (WindowHosts.TryGetValue(window, out var existing) && ReferenceEquals(existing, this))
            {
                WindowHosts.Remove(window);
            }
        }
    }

    private void RegisterHostForWindow()
    {
        var window = Window.GetWindow(this);
        if (window != null)
        {
            RegisterHost(window);
        }
    }

    private void RegisterHost(Window window)
    {
        lock (WindowHostsLock)
        {
            if (WindowHosts.TryGetValue(window, out var existing))
            {
                if (!ReferenceEquals(existing, this))
                {
                    throw new InvalidOperationException(
                        "Only one ContentDialogHost instance is allowed per Window."
                    );
                }

                // already registered for this window and it's this instance
                return;
            }

            WindowHosts.Add(window, this);
        }
    }
}

#pragma warning restore IDE0008 // Use explicit type instead of 'var'
