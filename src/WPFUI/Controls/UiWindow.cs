// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using WPFUI.Appearance;
using WPFUI.Interop;

namespace WPFUI.Controls;

public class UiWindow : System.Windows.Window
{
    private HwndSource _sourceWindow;

    internal IntPtr CriticalHandle
    {
        get
        {
            if (_sourceWindow != null)
            {
                return _sourceWindow.Handle;
            }
            else
            {
                return IntPtr.Zero;
            }
        }
    }

    public UiWindow()
    {
        SetResourceReference(StyleProperty, typeof(UiWindow));
    }

    static UiWindow()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(UiWindow), new FrameworkPropertyMetadata(typeof(UiWindow)));
        //WindowStyleProperty.OverrideMetadata(typeof(UiWindow), new FrameworkPropertyMetadata(WindowStyle.SingleBorderWindow));
        //AllowsTransparencyProperty.OverrideMetadata(typeof(UiWindow), new FrameworkPropertyMetadata(false));
    }

    /// <summary>
    /// Tries to remove default Window titlebar.
    /// </summary>
    protected void RemoveTitlebar()
    {
        // new HandleRef(this,CriticalHandle),
        //case WindowStyle.None:
        //_Style &= (~NativeMethods.WS_CAPTION);
        //break;

        //WindowStyle = WindowStyle.None;
        //AllowsTransparency = true;

        //const Interop.User32.MF mfEnabled = Interop.User32.MF.ENABLED | Interop.User32.MF.BYCOMMAND;
        //const Interop.User32.MF mfDisabled = Interop.User32.MF.GRAYED | Interop.User32.MF.DISABLED | Interop.User32.MF.BYCOMMAND;

        //var hmenu = Interop.User32.GetSystemMenu(CriticalHandle, false);

        //Interop.User32.EnableMenuItem(hmenu, Interop.User32.SC.MINIMIZE, mfDisabled);
        //Interop.User32.EnableMenuItem(hmenu, Interop.User32.SC.MAXIMIZE, mfDisabled);

        Interop.Dwmapi.DwmIsCompositionEnabled(out var isComposition);
#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO | Composition is {isComposition}", "WPFUI.UiWindow");
#endif

        // CHROME STUFF YAY

        //var currentStyle = Interop.User32.GetWindowLong(CriticalHandle, Interop.User32.GWL.GWL_STYLE);
        //var currentExStyle = Interop.User32.GetWindowLong(CriticalHandle, Interop.User32.GWL.GWL_EXSTYLE);

        //currentStyle |= (int)Interop.User32.WS.CAPTION;
        //currentExStyle |= (int)Interop.User32.WS_EX.CLIENTEDGE;

        //Interop.User32.SetWindowLong(CriticalHandle, Interop.User32.GWL.GWL_STYLE, currentStyle);
        //Interop.User32.SetWindowLong(CriticalHandle, Interop.User32.GWL.GWL_EXSTYLE, currentExStyle);
    }

    protected virtual void OnContentRendered(EventArgs e)
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
        base.OnSourceInitialized(e);

        //UnsafeNativeMethods.ApplyWindowCornerPreference(CriticalHandle, WindowCornerPreference.Round);

        ClearRoundingRegion();
        ExtendGlassFrame();
        SetThemeAttributes();
        ApplyMica();
    }

    private void ClearRoundingRegion()
    {
        Interop.User32.SetWindowRgn(CriticalHandle, IntPtr.Zero, Interop.User32.IsWindowVisible(CriticalHandle));
    }

    //private const Interop.Dwmapi.SWP _SwpFlags = SWP.FRAMECHANGED | SWP.NOSIZE | SWP.NOMOVE | SWP.NOZORDER | SWP.NOOWNERZORDER | SWP.NOACTIVATE;

    private void ExtendGlassFrame()
    {
        Background = Brushes.Transparent;
        _sourceWindow.CompositionTarget.BackgroundColor = Colors.Transparent;

        Thickness deviceGlassThickness = Common.DpiHelper.LogicalThicknessToDevice(new Thickness(-1, -1, -1, -1), Common.DpiHelper.SystemDpiXScale(), Common.DpiHelper.SystemDpiYScale());

        var dwmMargin = new Interop.UxTheme.MARGINS
        {
            // err on the side of pushing in glass an extra pixel.
            cxLeftWidth = (int)Math.Ceiling(deviceGlassThickness.Left),
            cxRightWidth = (int)Math.Ceiling(deviceGlassThickness.Right),
            cyTopHeight = (int)Math.Ceiling(deviceGlassThickness.Top),
            cyBottomHeight = (int)Math.Ceiling(deviceGlassThickness.Bottom),
        };

        Interop.Dwmapi.DwmExtendFrameIntoClientArea(CriticalHandle, ref dwmMargin);
    }

    private void SetThemeAttributes()
    {
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

    private void ApplyMica()
    {
        //if (!UnsafeNativeMethods.RemoveWindowTitlebar(CriticalHandle))
        //    return;

        if (Theme.GetAppTheme() == ThemeType.Dark)
            UnsafeNativeMethods.ApplyWindowDarkMode(CriticalHandle);
        else
            UnsafeNativeMethods.RemoveWindowDarkMode(CriticalHandle);

        UnsafeNativeMethods.ApplyWindowLegacyMicaEffect(CriticalHandle);
    }
}
