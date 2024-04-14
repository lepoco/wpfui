// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Shell;
using Wpf.Ui.Interop;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// A custom WinUI Window with more convenience methods.
/// </summary>
public class FluentWindow : System.Windows.Window
{
    private WindowInteropHelper? _interopHelper = null;

    /// <summary>
    /// Gets contains helper for accessing this window handle.
    /// </summary>
    protected WindowInteropHelper InteropHelper
    {
        get => _interopHelper ??= new WindowInteropHelper(this);
    }

    /// <summary>Identifies the <see cref="WindowCornerPreference"/> dependency property.</summary>
    public static readonly DependencyProperty WindowCornerPreferenceProperty = DependencyProperty.Register(
        nameof(WindowCornerPreference),
        typeof(WindowCornerPreference),
        typeof(FluentWindow),
        new PropertyMetadata(WindowCornerPreference.Round, OnWindowCornerPreferenceChanged)
    );

    /// <summary>Identifies the <see cref="WindowBackdropType"/> dependency property.</summary>
    public static readonly DependencyProperty WindowBackdropTypeProperty = DependencyProperty.Register(
        nameof(WindowBackdropType),
        typeof(WindowBackdropType),
        typeof(FluentWindow),
        new PropertyMetadata(WindowBackdropType.None, OnWindowBackdropTypeChanged)
    );

    /// <summary>Identifies the <see cref="ExtendsContentIntoTitleBar"/> dependency property.</summary>
    public static readonly DependencyProperty ExtendsContentIntoTitleBarProperty =
        DependencyProperty.Register(
            nameof(ExtendsContentIntoTitleBar),
            typeof(bool),
            typeof(FluentWindow),
            new PropertyMetadata(false, OnExtendsContentIntoTitleBarChanged)
        );

    /// <summary>
    /// Gets or sets a value determining corner preference for current <see cref="Window"/>.
    /// </summary>
    public WindowCornerPreference WindowCornerPreference
    {
        get => (WindowCornerPreference)GetValue(WindowCornerPreferenceProperty);
        set => SetValue(WindowCornerPreferenceProperty, value);
    }

    /// <summary>
    /// Gets or sets a value determining preferred backdrop type for current <see cref="Window"/>.
    /// </summary>
    public WindowBackdropType WindowBackdropType
    {
        get => (WindowBackdropType)GetValue(WindowBackdropTypeProperty);
        set => SetValue(WindowBackdropTypeProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the default title bar of the window should be hidden to create space for app content.
    /// </summary>
    public bool ExtendsContentIntoTitleBar
    {
        get => (bool)GetValue(ExtendsContentIntoTitleBarProperty);
        set => SetValue(ExtendsContentIntoTitleBarProperty, value);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentWindow"/> class.
    /// </summary>
    public FluentWindow()
    {
        SetResourceReference(StyleProperty, typeof(FluentWindow));
    }

    /// <summary>
    /// Initializes static members of the <see cref="FluentWindow"/> class.
    /// Overrides default properties.
    /// </summary>
    /// <remarks>
    /// Overrides default properties.
    /// </remarks>
    static FluentWindow()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(FluentWindow),
            new FrameworkPropertyMetadata(typeof(FluentWindow))
        );
    }

    /// <inheritdoc />
    protected override void OnSourceInitialized(EventArgs e)
    {
        OnCornerPreferenceChanged(default, WindowCornerPreference);
        OnExtendsContentIntoTitleBarChanged(default, ExtendsContentIntoTitleBar);
        OnBackdropTypeChanged(default, WindowBackdropType);

        base.OnSourceInitialized(e);
    }

    /// <summary>
    /// Private <see cref="WindowCornerPreference"/> property callback.
    /// </summary>
    private static void OnWindowCornerPreferenceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not FluentWindow window)
        {
            return;
        }

        if (e.OldValue == e.NewValue)
        {
            return;
        }

        window.OnCornerPreferenceChanged(
            (WindowCornerPreference)e.OldValue,
            (WindowCornerPreference)e.NewValue
        );
    }

    /// <summary>
    /// This virtual method is called when <see cref="WindowCornerPreference"/> is changed.
    /// </summary>
    protected virtual void OnCornerPreferenceChanged(
        WindowCornerPreference oldValue,
        WindowCornerPreference newValue
    )
    {
        if (InteropHelper.Handle == IntPtr.Zero)
        {
            return;
        }

        _ = UnsafeNativeMethods.ApplyWindowCornerPreference(InteropHelper.Handle, newValue);
    }

    /// <summary>
    /// Private <see cref="WindowBackdropType"/> property callback.
    /// </summary>
    private static void OnWindowBackdropTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not FluentWindow window)
        {
            return;
        }

        if (e.OldValue == e.NewValue)
        {
            return;
        }

        window.OnBackdropTypeChanged((WindowBackdropType)e.OldValue, (WindowBackdropType)e.NewValue);
    }

    /// <summary>
    /// This virtual method is called when <see cref="WindowBackdropType"/> is changed.
    /// </summary>
    protected virtual void OnBackdropTypeChanged(WindowBackdropType oldValue, WindowBackdropType newValue)
    {
        if (Appearance.ApplicationThemeManager.GetAppTheme() == Appearance.ApplicationTheme.HighContrast)
        {
            newValue = WindowBackdropType.None;
        }

        if (InteropHelper.Handle == IntPtr.Zero)
        {
            return;
        }

        if (newValue == WindowBackdropType.None)
        {
            _ = WindowBackdrop.RemoveBackdrop(this);

            return;
        }

        if (!ExtendsContentIntoTitleBar)
        {
            throw new InvalidOperationException(
                $"Cannot apply backdrop effect if {nameof(ExtendsContentIntoTitleBar)} is false."
            );
        }

        if (WindowBackdrop.IsSupported(newValue) && WindowBackdrop.RemoveBackground(this))
        {
            _ = WindowBackdrop.ApplyBackdrop(this, newValue);
        }
    }

    /// <summary>
    /// Private <see cref="ExtendsContentIntoTitleBar"/> property callback.
    /// </summary>
    private static void OnExtendsContentIntoTitleBarChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e
    )
    {
        if (d is not FluentWindow window)
        {
            return;
        }

        if (e.OldValue == e.NewValue)
        {
            return;
        }

        window.OnExtendsContentIntoTitleBarChanged((bool)e.OldValue, (bool)e.NewValue);
    }

    /// <summary>
    /// This virtual method is called when <see cref="ExtendsContentIntoTitleBar"/> is changed.
    /// </summary>
    protected virtual void OnExtendsContentIntoTitleBarChanged(bool oldValue, bool newValue)
    {
        // AllowsTransparency = true;
        SetCurrentValue(WindowStyleProperty, WindowStyle.SingleBorderWindow);

        WindowChrome.SetWindowChrome(
            this,
            new WindowChrome
            {
                CaptionHeight = 0,
                CornerRadius = default,
                GlassFrameThickness = new Thickness(-1),
                ResizeBorderThickness = ResizeMode == ResizeMode.NoResize ? default : new Thickness(4),
                UseAeroCaptionButtons = false
            }
        );

        // WindowStyleProperty.OverrideMetadata(typeof(FluentWindow), new FrameworkPropertyMetadata(WindowStyle.SingleBorderWindow));
        // AllowsTransparencyProperty.OverrideMetadata(typeof(FluentWindow), new FrameworkPropertyMetadata(false));
        _ = UnsafeNativeMethods.RemoveWindowTitlebarContents(this);
    }
}
