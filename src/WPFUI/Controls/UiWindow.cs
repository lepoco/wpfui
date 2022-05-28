// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using WPFUI.Appearance;
using WPFUI.Interop;

namespace WPFUI.Controls;

/// <summary>
/// Extended <see cref="System.Windows.Window"/> with WPF UI features. 
/// </summary>
public class UiWindow : System.Windows.Window
{
    protected bool _sourceInitialized = false;

    /// <summary>
    /// Contains helper for accessing this window handle.
    /// </summary>
    protected WindowInteropHelper InteropHelper { get; set; }

    /// <summary>
    /// Property for <see cref="BackdropType"/>.
    /// </summary>
    public static readonly DependencyProperty BackdropTypeProperty = DependencyProperty.Register(nameof(BackdropType),
        typeof(BackgroundType), typeof(UiWindow), new PropertyMetadata(BackgroundType.Unknown, OnBackdropTypeChanged, CoerceBackdropType));


    private static object CoerceBackdropType(DependencyObject d, object baseValue)
    {
        if (d is not UiWindow uiWindow)
            return baseValue;

        if (uiWindow._sourceInitialized)
            throw new InvalidOperationException(
                $"{nameof(BackdropType)} cannot be changed after {typeof(UiWindow)} is initialized.");

        return baseValue;
    }

    private static void OnBackdropTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        //throw new NotImplementedException();
    }


    public BackgroundType BackdropType
    {
        get => (BackgroundType)GetValue(BackdropTypeProperty);
        set => SetValue(BackdropTypeProperty, value);
    }

    public UiWindow()
    {
        InteropHelper = new WindowInteropHelper(this);

        SetResourceReference(StyleProperty, typeof(UiWindow));
    }

    static UiWindow()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(UiWindow), new FrameworkPropertyMetadata(typeof(UiWindow)));
        HeightProperty.OverrideMetadata(typeof(UiWindow), new FrameworkPropertyMetadata(600d));
        WidthProperty.OverrideMetadata(typeof(UiWindow), new FrameworkPropertyMetadata(1100d));
        MinHeightProperty.OverrideMetadata(typeof(UiWindow), new FrameworkPropertyMetadata(320d));
        MinWidthProperty.OverrideMetadata(typeof(UiWindow), new FrameworkPropertyMetadata(460d));
        //WindowStyleProperty.OverrideMetadata(typeof(UiWindow), new FrameworkPropertyMetadata(WindowStyle.SingleBorderWindow));
        //AllowsTransparencyProperty.OverrideMetadata(typeof(UiWindow), new FrameworkPropertyMetadata(false));
    }

    /// <inheritdoc />
    protected override void OnContentRendered(EventArgs e)
    {
        base.OnContentRendered(e);
    }

    /// <inheritdoc />
    protected override void OnSourceInitialized(EventArgs e)
    {
        _sourceInitialized = true;

        base.OnSourceInitialized(e);
    }


    //UnsafeNativeMethods.ApplyWindowCornerPreference(CriticalHandle, WindowCornerPreference.Round);
    //ClearRoundingRegion();
    //ExtendGlassFrame();
    //SetThemeAttributes();
    //ApplyMica();

    /// <summary>
    /// Tries to remove default Window system menu.
    /// </summary>
    protected void RemoveTitlebar()
    {
        if (_sourceInitialized)
        {
            UnsafeNativeMethods.RemoveWindowTitlebar(InteropHelper.Handle);

            return;
        }

        SourceInitialized += (sender, args) => UnsafeNativeMethods.RemoveWindowTitlebar(InteropHelper.Handle);
    }

    protected void ClearRoundingRegion()
    {
        if (InteropHelper.Handle == IntPtr.Zero)
            return;

        Interop.User32.SetWindowRgn(InteropHelper.Handle, IntPtr.Zero, Interop.User32.IsWindowVisible(InteropHelper.Handle));
    }

    protected void ExtendGlassFrame()
    {
        var hwndSource = (HwndSource)PresentationSource.FromVisual(this);

        if (InteropHelper.Handle == IntPtr.Zero || hwndSource?.CompositionTarget == null)
            return;

        //Background = Brushes.Transparent;
        //CompositionTarget.BackgroundColor = Colors.Transparent;

        var deviceGlassThickness = Common.DpiHelper.LogicalThicknessToDevice(
            new Thickness(-1, -1, -1, -1),
            Common.DpiHelper.SystemDpiXScale(),
            Common.DpiHelper.SystemDpiYScale());

        var dwmMargin = new UxTheme.MARGINS
        {
            // err on the side of pushing in glass an extra pixel.
            cxLeftWidth = (int)Math.Ceiling(deviceGlassThickness.Left),
            cxRightWidth = (int)Math.Ceiling(deviceGlassThickness.Right),
            cyTopHeight = (int)Math.Ceiling(deviceGlassThickness.Top),
            cyBottomHeight = (int)Math.Ceiling(deviceGlassThickness.Bottom),
        };

        Interop.Dwmapi.DwmExtendFrameIntoClientArea(InteropHelper.Handle, ref dwmMargin);
    }

    protected void SetThemeAttributes()
    {
        if (InteropHelper.Handle == IntPtr.Zero)
            return;

        var wtaOptions = new UxTheme.WTA_OPTIONS()
        {
            dwFlags = (UxTheme.WTNCA.NODRAWCAPTION | UxTheme.WTNCA.NODRAWICON | UxTheme.WTNCA.NOSYSMENU),
            dwMask = UxTheme.WTNCA.VALIDBITS
        };

        Interop.UxTheme.SetWindowThemeAttribute(
            InteropHelper.Handle,
            UxTheme.WINDOWTHEMEATTRIBUTETYPE.WTA_NONCLIENT,
            ref wtaOptions,
            (uint)Marshal.SizeOf(typeof(UxTheme.WTA_OPTIONS)));
    }

    protected void ApplyBackdrop(BackgroundType backgroundType)
    {
        Appearance.Background.Apply(this, backgroundType);
    }
}
