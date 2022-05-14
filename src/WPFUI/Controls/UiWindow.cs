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
    private bool _sourceInitialized = false;

    private HwndSource _sourceWindow;

    protected IntPtr CriticalHandle
    {
        get => _sourceWindow?.Handle ?? IntPtr.Zero;
    }

    protected HwndTarget CompositionTarget
    {
        get => _sourceWindow?.CompositionTarget;
    }

    public UiWindow()
    {
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
        _sourceWindow = (HwndSource)PresentationSource.FromVisual(this);
#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO | Handle of UiWindow is {CriticalHandle}", "WPFUI.UiWindow");
#endif
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
            UnsafeNativeMethods.RemoveWindowTitlebar(CriticalHandle);

            return;
        }

        SourceInitialized += (sender, args) => UnsafeNativeMethods.RemoveWindowTitlebar(CriticalHandle);
    }

    protected void ClearRoundingRegion()
    {
        if (CriticalHandle == IntPtr.Zero)
            return;

        Interop.User32.SetWindowRgn(CriticalHandle, IntPtr.Zero, Interop.User32.IsWindowVisible(CriticalHandle));
    }

    protected void ExtendGlassFrame()
    {
        if (CriticalHandle == IntPtr.Zero || CompositionTarget == null)
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

        Interop.Dwmapi.DwmExtendFrameIntoClientArea(CriticalHandle, ref dwmMargin);
    }

    protected void SetThemeAttributes()
    {
        if (CriticalHandle == IntPtr.Zero)
            return;

        var wtaOptions = new UxTheme.WTA_OPTIONS()
        {
            dwFlags = (UxTheme.WTNCA.NODRAWCAPTION | UxTheme.WTNCA.NODRAWICON | UxTheme.WTNCA.NOSYSMENU),
            dwMask = UxTheme.WTNCA.VALIDBITS
        };

        Interop.UxTheme.SetWindowThemeAttribute(
            CriticalHandle,
            UxTheme.WINDOWTHEMEATTRIBUTETYPE.WTA_NONCLIENT,
            ref wtaOptions,
            (uint)Marshal.SizeOf(typeof(UxTheme.WTA_OPTIONS)));
    }

    protected void ApplyBackdrop(BackgroundType backgroundType)
    {
        Appearance.Background.Apply(this, backgroundType);
    }
}
