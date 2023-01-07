// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Shell;
using Wpf.Ui.Interop;

namespace Wpf.Ui.Controls.Window;

/// <summary>
/// A custom WinUI Window with more convenience methods.
/// </summary>
[ToolboxItem(true)]
[ToolboxBitmap(typeof(FluentWindow), "FluentWindow.bmp")]
public class FluentWindow : System.Windows.Window
{
    private WindowInteropHelper? _interopHelper = null;

    /// <summary>
    /// Contains helper for accessing this window handle.
    /// </summary>
    protected WindowInteropHelper InteropHelper
    {
        get => _interopHelper ??= new WindowInteropHelper(this);
    }

    /// <summary>
    /// Property for <see cref="WindowCornerPreference"/>.
    /// </summary>
    public static readonly DependencyProperty WindowCornerPreferenceProperty = DependencyProperty.Register(
        nameof(WindowCornerPreference),
        typeof(WindowCornerPreference), typeof(FluentWindow),
        new PropertyMetadata(WindowCornerPreference.Round, OnCornerPreferenceChanged));

    /// <summary>
    /// Property for <see cref="WindowBackdropType"/>.
    /// </summary>
    public static readonly DependencyProperty WindowBackdropTypeProperty = DependencyProperty.Register(
        nameof(WindowBackdropType),
        typeof(WindowBackdropType), typeof(FluentWindow),
        new PropertyMetadata(WindowBackdropType.None, OnBackdropTypeChanged));

    /// <summary>
    /// Property for <see cref="ExtendsContentIntoTitleBar"/>.
    /// </summary>
    public static readonly DependencyProperty ExtendsContentIntoTitleBarProperty = DependencyProperty.Register(
        nameof(ExtendsContentIntoTitleBar),
        typeof(bool), typeof(FluentWindow), new PropertyMetadata(false, OnExtendsContentIntoTitleBarChanged));

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
    /// Gets or sets a value that specifies whether the default title bar of the window should be hidden to create space for app content.
    /// </summary>
    public bool ExtendsContentIntoTitleBar
    {
        get => (bool)GetValue(ExtendsContentIntoTitleBarProperty);
        set => SetValue(ExtendsContentIntoTitleBarProperty, value);
    }


    /// <summary>
    /// Creates new instance and sets default style.
    /// </summary>
    public FluentWindow()
    {
        SetResourceReference(StyleProperty, typeof(FluentWindow));
    }

    /// <summary>
    /// Overrides default properties.
    /// </summary>
    static FluentWindow()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(FluentWindow), new FrameworkPropertyMetadata(typeof(FluentWindow)));
        HeightProperty.OverrideMetadata(typeof(FluentWindow), new FrameworkPropertyMetadata(600d));
        WidthProperty.OverrideMetadata(typeof(FluentWindow), new FrameworkPropertyMetadata(1100d));
        MinHeightProperty.OverrideMetadata(typeof(FluentWindow), new FrameworkPropertyMetadata(320d));
        MinWidthProperty.OverrideMetadata(typeof(FluentWindow), new FrameworkPropertyMetadata(460d));
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
    private static void OnCornerPreferenceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not FluentWindow window)
            return;

        if (e.OldValue == e.NewValue)
            return;

        window.OnCornerPreferenceChanged((WindowCornerPreference)e.OldValue, (WindowCornerPreference)e.NewValue);
    }

    /// <summary>
    /// This virtual method is called when <see cref="WindowCornerPreference"/> is changed.
    /// </summary>
    protected virtual void OnCornerPreferenceChanged(WindowCornerPreference oldValue, WindowCornerPreference newValue)
    {
        if (InteropHelper.Handle == IntPtr.Zero)
            return;

        UnsafeNativeMethods.ApplyWindowCornerPreference(InteropHelper.Handle, newValue);
    }

    /// <summary>
    /// Private <see cref="WindowBackdropType"/> property callback.
    /// </summary>
    private static void OnBackdropTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not FluentWindow window)
            return;

        if (e.OldValue == e.NewValue)
            return;

        window.OnBackdropTypeChanged((WindowBackdropType)e.OldValue, (WindowBackdropType)e.NewValue);
    }

    /// <summary>
    /// This virtual method is called when <see cref="WindowBackdropType"/> is changed.
    /// </summary>
    protected virtual void OnBackdropTypeChanged(WindowBackdropType oldValue, WindowBackdropType newValue)
    {
        if (InteropHelper.Handle == IntPtr.Zero)
            return;

        if (newValue == WindowBackdropType.None)
        {
            WindowBackdrop.RemoveBackdrop(this);
            return;
        }

        if (!ExtendsContentIntoTitleBar)
            throw new InvalidOperationException($"Cannot apply backdrop effect if {nameof(ExtendsContentIntoTitleBar)} is false.");

        if (WindowBackdrop.IsSupported(newValue) && WindowBackdrop.RemoveBackground(this))
            WindowBackdrop.ApplyBackdrop(this, newValue);
    }

    /// <summary>
    /// Private <see cref="ExtendsContentIntoTitleBar"/> property callback.
    /// </summary>
    private static void OnExtendsContentIntoTitleBarChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not FluentWindow window)
            return;

        if (e.OldValue == e.NewValue)
            return;

        window.OnExtendsContentIntoTitleBarChanged((bool)e.OldValue, (bool)e.NewValue);
    }

    /// <summary>
    /// This virtual method is called when <see cref="ExtendsContentIntoTitleBar"/> is changed.
    /// </summary>
    protected virtual void OnExtendsContentIntoTitleBarChanged(bool oldValue, bool newValue)
    {
        WindowStyle = WindowStyle.SingleBorderWindow;
        //AllowsTransparency = true;

        WindowChrome.SetWindowChrome(this,
            new WindowChrome
            {
                CaptionHeight = 0,
                CornerRadius = default,
                GlassFrameThickness = new Thickness(-1),
                ResizeBorderThickness = ResizeMode == ResizeMode.NoResize ? default : new Thickness(4),
                UseAeroCaptionButtons = false
            });

        UnsafeNativeMethods.RemoveWindowTitlebarContents(this);
        ////WindowStyleProperty.OverrideMetadata(typeof(FluentWindow), new FrameworkPropertyMetadata(WindowStyle.SingleBorderWindow));
        ////AllowsTransparencyProperty.OverrideMetadata(typeof(FluentWindow), new FrameworkPropertyMetadata(false));
    }
}
