// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Runtime.InteropServices;

namespace Wpf.Ui.Appearance;

/// <summary>
/// Contains internal RCWs for invoking the UISettings
/// </summary>
internal static class UISettingsRCW
{
    public enum UIColorType
    {
        Background = 0,
        Foreground = 1,
        AccentDark3 = 2,
        AccentDark2 = 3,
        AccentDark1 = 4,
        Accent = 5,
        AccentLight1 = 6,
        AccentLight2 = 7,
        AccentLight3 = 8,
        Complement = 9
    }

    public static object GetUISettingsInstance()
    {
        const string typeName = "Windows.UI.ViewManagement.UISettings";

        int hr = NativeMethods.WindowsCreateString(typeName, typeName.Length, out IntPtr hstring);
        Marshal.ThrowExceptionForHR(hr);

        try
        {
            hr = NativeMethods.RoActivateInstance(hstring, out object instance);
            Marshal.ThrowExceptionForHR(hr);
            return instance;
        }
        finally
        {
            hr = NativeMethods.WindowsDeleteString(hstring);
            Marshal.ThrowExceptionForHR(hr);
        }
    }

    /// <summary>
    /// Contains internal RCWs for invoking the InputPane (tiptsf touch keyboard)
    /// </summary>
    internal static class NativeMethods
    {
        [DllImport("api-ms-win-core-winrt-string-l1-1-0.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern int WindowsCreateString(
            [MarshalAs(UnmanagedType.LPWStr)] string sourceString,
            int length,
            out IntPtr hstring);

        [DllImport("api-ms-win-core-winrt-string-l1-1-0.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern int WindowsDeleteString(IntPtr hstring);

        [DllImport("api-ms-win-core-winrt-l1-1-0.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern int RoActivateInstance(IntPtr runtimeClassId, [MarshalAs(UnmanagedType.Interface)] out object instance);
    }

    [Guid("03021BE4-5254-4781-8194-5168F7D06D7B")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    internal interface IUISettings3
    {
        void GetIids(out uint iidCount, out IntPtr iids);

        void GetRuntimeClassName(out string className);

        void GetTrustLevel(out TrustLevel TrustLevel);

        UIColor GetColorValue(UIColorType desiredColor);
    }

    internal enum TrustLevel
    {
        BaseTrust,
        PartialTrust,
        FullTrust
    }

    internal readonly record struct UIColor(byte A, byte R, byte G, byte B);
}
