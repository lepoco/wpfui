// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

// TODO: This class is the only reason for using System.Drawing.Common.
// It is worth looking for a way to get hIcon without using it.

using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Wpf.Ui.Tray;

/// <summary>
/// Facilitates the creation of a hIcon.
/// </summary>
internal static class Hicon
{
    /// <summary>
    /// Tries to take the icon pointer assigned to the application.
    /// </summary>
    public static IntPtr FromApp()
    {
        try
        {
            var processName = Process.GetCurrentProcess().MainModule?.FileName;

            if (string.IsNullOrEmpty(processName))
            {
                return IntPtr.Zero;
            }

            var appIconsExtractIcon = System.Drawing.Icon.ExtractAssociatedIcon(processName);

            if (appIconsExtractIcon == null)
            {
                return IntPtr.Zero;
            }

            /*appIconsExtractIcon.ToBitmap();*/

            return appIconsExtractIcon.Handle;
        }
        catch (Exception e)
        {
            System.Diagnostics.Debug.WriteLine(
                $"ERROR | Unable to get application hIcon - {e}",
                "Wpf.Ui.Hicon"
            );
#if DEBUG
            throw;
#else
            return IntPtr.Zero;
#endif
        }
    }

    /// <summary>
    /// Tries to allocate an icon to memory and fetch a pointer to it.
    /// </summary>
    /// <param name="source">Image source.</param>
    public static IntPtr FromSource(ImageSource source)
    {
        IntPtr hIcon = IntPtr.Zero;
        var bitmapFrame = source as BitmapFrame;

        if (source is not BitmapSource bitmapSource)
        {
            System.Diagnostics.Debug.WriteLine(
                $"ERROR | Unable to allocate hIcon, ImageSource is not a BitmapSource",
                "Wpf.Ui.Hicon"
            );
            return hIcon;
        }

        if ((bitmapFrame?.Decoder?.Frames?.Count ?? 0) > 1)
        {
            // Gets first bitmap frame.
            bitmapSource = bitmapFrame!.Decoder!.Frames![0];
        }

        var stride = bitmapSource!.PixelWidth * ((bitmapSource.Format.BitsPerPixel + 7) / 8);
        var pixels = new byte[bitmapSource.PixelHeight * stride];

        bitmapSource.CopyPixels(pixels, stride, 0);

        // Allocate pixels to unmanaged memory
        var gcHandle = GCHandle.Alloc(pixels, GCHandleType.Pinned);

        if (!gcHandle.IsAllocated)
        {
            System.Diagnostics.Debug.WriteLine(
                $"ERROR | Unable to allocate hIcon, allocation failed.",
                "Wpf.Ui.Hicon"
            );
            return hIcon;
        }

        // Specifies that the format is 32 bits per pixel; 8 bits each are used for the alpha, red, green, and blue components.
        // The red, green, and blue components are premultiplied, according to the alpha component.
        var bitmap = new Bitmap(
            bitmapSource.PixelWidth,
            bitmapSource.PixelHeight,
            stride,
            System.Drawing.Imaging.PixelFormat.Format32bppPArgb,
            gcHandle.AddrOfPinnedObject()
        );

        hIcon = bitmap.GetHicon();

        // Release handle.
        gcHandle.Free();

        return hIcon;
    }
}
