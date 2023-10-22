// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Controls;

namespace Wpf.Ui.Appearance;

internal class ObservedWindow
{
    private readonly HwndSource _source;

    public ObservedWindow(IntPtr handle, WindowBackdropType backdrop, bool forceBackgroundReplace, bool updateAccents)
    {
        Handle = handle;
        Backdrop = backdrop;
        ForceBackgroundReplace = forceBackgroundReplace;
        UpdateAccents = updateAccents;
        HasHook = false;

        var windowSource = HwndSource.FromHwnd(handle);
        _source = windowSource ?? throw new InvalidOperationException("Unable to determine the window source.");
    }

    public Window? RootVisual => (Window?)_source.RootVisual;

    public IntPtr Handle { get; }

    public WindowBackdropType Backdrop { get; }

    public bool ForceBackgroundReplace { get; }

    public bool UpdateAccents { get; }

    public bool HasHook { get; private set; }

    public void AddHook(HwndSourceHook hook)
    {
        _source.AddHook(hook);

        HasHook = true;
    }

    public void RemoveHook(HwndSourceHook hook)
    {
        _source.RemoveHook(hook);

        HasHook = false;
    }
}
