// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPFUI.Win32;

namespace WPFUI.Tray
{
    /// <summary>
    /// Specifies a component that creates an icon in the notification area.
    /// </summary>
    [ToolboxItem(false)]
    public sealed class NotifyIcon : IDisposable
    {
        private HwndSource _hWndSource;

        private ContextMenu _contextMenu;

        /// <summary>
        /// Shell32 notify icon identifier.
        /// </summary>
        public int Uid { get; internal set; } = 0x1;

        /// <summary>
        /// Gets value inidicating whether the icon was initialized.
        /// </summary>
        public bool IsInitialized { get; internal set; } = false;

        /// <summary>
        /// Visual parent of <see cref="NotifyIcon"/> which handles HWND messages.
        /// </summary>
        public Visual Parent { get; set; }

        /// <summary>
        /// BitmapSource of tray icon.
        /// </summary>
        public ImageSource Icon { get; set; }

        /// <summary>
        /// Gets or sets the ToolTip text displayed when the mouse pointer rests on a notification area icon.
        /// </summary>
        public string Tooltip { get; set; }

        /// <summary>
        /// Menu displayed when left click.
        /// </summary>
        public ContextMenu ContextMenu
        {
            get => _contextMenu;

            set
            {
                if (value != null)
                {
                    var contextMenuStyle = Application.Current.FindResource("UiNotifyIconContextMenuStyle");

                    if (contextMenuStyle != null)
                    {
                        value.Style = (Style)contextMenuStyle;
                    }
                }

                _contextMenu = value;
            }
        }

        /// <summary>
        /// The action triggered by a left click of the mouse.
        /// </summary>
        public Action<NotifyIcon> Click { get; set; }

        /// <summary>
        /// The action triggered by a double-click with the left mouse button.
        /// </summary>
        public Action<NotifyIcon> DoubleClick { get; set; }

        /// <summary>
        /// Called on finalizing.
        /// </summary>
        ~NotifyIcon()
        {
            Dispose();
        }

        /// <summary>
        /// Loads <see cref="Icon"/> into memory, then displays it in the system tray.
        /// </summary>
        public void Show()
        {
            Parent ??= Application.Current.MainWindow;

            if (Parent == null)
            {
                return;
            }

            if (_hWndSource == null)
            {
                _hWndSource = (HwndSource)PresentationSource.FromVisual(Parent);

                if (_hWndSource != null)
                {
                    _hWndSource.AddHook(HwndSourceHook);
                }
            }

            if (_hWndSource == null)
            {
#if DEBUG
                // TODO: Change handling this exception
                //throw new NullReferenceException("Could not initialize hWnd Source for NotifyIcon.");
#endif
                return;
            }

            Shell32.NOTIFYICONDATA notifyIconData = new Shell32.NOTIFYICONDATA
            {
                uID = Uid,
                hWnd = _hWndSource.Handle,
                uCallbackMessage = (int)User32.WM.TRAYMOUSEMESSAGE,
                uFlags = UFlags.Message
            };

            if (!String.IsNullOrEmpty(Tooltip))
            {
                notifyIconData.szTip = Tooltip;
                notifyIconData.uFlags |= UFlags.ToolTip;
            }

            if (Icon == null)
            {
                IntPtr hIcon = ExtractApplicationHIcon();

                if (hIcon != IntPtr.Zero)
                {
                    notifyIconData.hIcon = GetHIcon(Icon);
                    notifyIconData.uFlags |= UFlags.Icon;
                }
            }

            if (Icon != null)
            {
                notifyIconData.hIcon = GetHIcon(Icon);
                notifyIconData.uFlags |= UFlags.Icon;
            }

            Shell32.Shell_NotifyIcon((int)User32.WM.NULL, notifyIconData);

            IsInitialized = true;
        }

        /// <summary>
        /// Removes icon.
        /// </summary>
        public void Destroy()
        {
            if (_hWndSource == null)
            {
                return;
            }

            Shell32.NOTIFYICONDATA notifyIconData = new Shell32.NOTIFYICONDATA
            {
                uID = Uid,
                hWnd = _hWndSource.Handle,
                uCallbackMessage = (int)User32.WM.TRAYMOUSEMESSAGE,
                uFlags = UFlags.Message
            };

            Shell32.Shell_NotifyIcon((int)User32.WM.DESTROY, notifyIconData);

            _hWndSource = null;

            Uid += 1;

            IsInitialized = false;
        }

        /// <summary>
        /// Called on finalizing.
        /// </summary>
        public void Dispose()
        {
            if (_hWndSource == null)
            {
                return;
            }

            User32.PostMessage(new HandleRef(_hWndSource, _hWndSource.Handle), User32.WM.CLOSE, IntPtr.Zero,
                IntPtr.Zero);

            _hWndSource.Dispose();
        }

        /// <summary>
        /// Displays the tray menu if defined.
        /// </summary>
        private void ShowContextMenu()
        {
            if (ContextMenu == null || _hWndSource == null)
            {
                return;
            }

            User32.SetForegroundWindow(new HandleRef(_hWndSource, _hWndSource.Handle));
            ContextMenuService.SetPlacement(ContextMenu, PlacementMode.MousePoint);

            // TODO: Apply Mica to ContextMenu

            //if (Background.Manager.IsSupported(BackgroundType.Mica))
            //{
            //    ContextMenu.Background = Brushes.Transparent; // Works

            //}

            ContextMenu.IsOpen = true;
        }

        /// <summary>
        /// Represents the method that handles Win32 window messages.
        /// </summary>
        /// <param name="hWnd">The window handle.</param>
        /// <param name="uMsg">The message ID.</param>
        /// <param name="wParam">The message's wParam value.</param>
        /// <param name="lParam">The message's lParam value.</param>
        /// <param name="handled">A value that indicates whether the message was handled. Set the value to <see langword="true"/> if the message was handled; otherwise, <see langword="false"/>.</param>
        /// <returns>The appropriate return value depends on the particular message. See the message documentation details for the Win32 message being handled.</returns>
        internal IntPtr HwndSourceHook(IntPtr hWnd, int uMsg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (uMsg <= (int)User32.WM.MEASUREITEM)
            {
                if (uMsg == (int)User32.WM.DESTROY)
                {
                    Destroy();

                    handled = true;

                    return IntPtr.Zero;
                }
            }
            else
            {
                if (uMsg != (int)User32.WM.TRAYMOUSEMESSAGE)
                {
                    handled = false;

                    return IntPtr.Zero;
                }

                switch ((User32.WM)lParam)
                {
                    case User32.WM.LBUTTONUP:
                        if (Click != null)
                        {
                            Click(this);
                        }

                        break;

                    case User32.WM.LBUTTONDBLCLK:
                        if (DoubleClick != null)
                        {
                            DoubleClick(this);
                        }

                        break;

                    case User32.WM.RBUTTONUP:
                        ShowContextMenu();

                        break;
                }

                handled = true;

                return IntPtr.Zero;
            }

            handled = false;

            return IntPtr.Zero;
        }

        // TODO: Usage of Bitmap here is the only reason why we need to use System.Drawing.Common. We have to somehow work around the ImageSource to HIcon conversion.

        private IntPtr ExtractApplicationHIcon()
        {
            Bitmap applicationIcon;

            try
            {
                var processName = Process.GetCurrentProcess().MainModule?.FileName;

                if (String.IsNullOrEmpty(processName))
                {
                    return IntPtr.Zero;
                }

                var appIconsExtractIcon = System.Drawing.Icon.ExtractAssociatedIcon(processName);

                if (appIconsExtractIcon == null)
                {
                    return IntPtr.Zero;
                }

                applicationIcon = appIconsExtractIcon.ToBitmap();
            }
            catch (Exception e)
            {
#if DEBUG
                Console.WriteLine(e);
                throw;
#endif
            }

            // TODO: Bitmap to HBitmap with allocation.

            return IntPtr.Zero;
        }

        /// <summary>
        /// Gets <see cref="IntPtr"/> pointer to the allocated bitmap.
        /// </summary>
        private static IntPtr GetHIcon(ImageSource source)
        {
            IntPtr hIcon = IntPtr.Zero;

            BitmapFrame bitmapFrame = source as BitmapFrame;

            if (bitmapFrame?.Decoder == null || bitmapFrame.Decoder.Frames.Count < 1)
            {
                return hIcon;
            }

            // Gets first bitmap frame.
            bitmapFrame = bitmapFrame.Decoder.Frames[0];

            int stride = bitmapFrame.PixelWidth * ((bitmapFrame.Format.BitsPerPixel + 7) / 8);
            byte[] pixels = new byte[bitmapFrame.PixelHeight * stride];

            bitmapFrame.CopyPixels(pixels, stride, 0);

            // Allocate pixels to unmanaged memory
            GCHandle gcHandle = GCHandle.Alloc(pixels, GCHandleType.Pinned);

            if (!gcHandle.IsAllocated)
            {
                return hIcon;
            }

            // Specifies that the format is 32 bits per pixel; 8 bits each are used for the alpha, red, green, and blue components.
            // The red, green, and blue components are premultiplied, according to the alpha component.
            Bitmap bitmap = new(bitmapFrame.PixelWidth, bitmapFrame.PixelHeight, stride,
                System.Drawing.Imaging.PixelFormat.Format32bppPArgb, gcHandle.AddrOfPinnedObject());

            hIcon = bitmap.GetHicon();

            // Release handle.
            gcHandle.Free();

            return hIcon;
        }
    }
}