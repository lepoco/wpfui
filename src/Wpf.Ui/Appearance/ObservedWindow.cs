// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Controls;

namespace Wpf.Ui.Appearance;

/// <summary>
/// Represents a window that is being observed for changes in appearance.
/// </summary>
internal class ObservedWindow
{
    private readonly HwndSource _source;

    /// <summary>
    /// Initializes a new instance of the ObservedWindow class.
    /// </summary>
    /// <param name="handle">The handle of the window.</param>
    /// <param name="backdrop">The backdrop type of the window.</param>
    /// <param name="updateAccents">Indicates whether to update accents.</param>
    public ObservedWindow(IntPtr handle, WindowBackdropType backdrop, bool updateAccents)
    {
        Handle = handle;
        Backdrop = backdrop;
        UpdateAccents = updateAccents;
        HasHook = false;

        HwndSource? windowSource = HwndSource.FromHwnd(handle);
        _source =
            windowSource ?? throw new InvalidOperationException("Unable to determine the window source.");
    }

    /// <summary>
    /// Gets the root visual of the window.
    /// </summary>
    public Window? RootVisual => (Window?)_source.RootVisual;

    /// <summary>
    /// Gets the handle of the window.
    /// </summary>
    public IntPtr Handle { get; }

    /// <summary>
    /// Gets the backdrop type of the window.
    /// </summary>
    public WindowBackdropType Backdrop { get; }

    /// <summary>
    /// Gets a value indicating whether to update accents.
    /// </summary>
    public bool UpdateAccents { get; }

    /// <summary>
    /// Gets a value indicating whether the window has a hook.
    /// </summary>
    public bool HasHook { get; private set; }

    /// <summary>
    /// Adds a hook to the window.
    /// </summary>
    /// <param name="hook">The hook to add.</param>
    public void AddHook(HwndSourceHook hook)
    {
        _source.AddHook(hook);

        HasHook = true;
    }

    /// <summary>
    /// Removes a hook from the window.
    /// </summary>
    /// <param name="hook">The hook to remove.</param>
    public void RemoveHook(HwndSourceHook hook)
    {
        _source.RemoveHook(hook);

        HasHook = false;
    }
}
