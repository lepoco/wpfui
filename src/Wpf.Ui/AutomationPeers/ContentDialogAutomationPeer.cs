// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Threading;
using Wpf.Ui.Controls;

namespace Wpf.Ui.AutomationPeers;

/// <summary>
/// Automation peer that exposes a <see cref="ContentDialog"/> as a standard modal window
/// for UI Automation clients.
/// </summary>
/// <remarks>
/// This peer maps dialog-specific behavior to the <see cref="IWindowProvider"/> pattern so
/// assistive technologies (screen readers, automation tools) perceive the <see cref="ContentDialog"/>
/// as a modal, non-resizable dialog window.
/// </remarks>
internal sealed class ContentDialogAutomationPeer : UIElementAutomationPeer, IWindowProvider
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ContentDialogAutomationPeer"/> class.
    /// </summary>
    /// <param name="owner">The associated <see cref="ContentDialog"/>.</param>
    public ContentDialogAutomationPeer(ContentDialog owner)
        : base(owner) { }

    /// <summary>
    /// Gets a value indicating whether the window is modal.
    /// Always <see langword="true"/> for <see cref="ContentDialog"/>.
    /// </summary>
    bool IWindowProvider.IsModal => true;

    /// <summary>
    /// Gets a value indicating whether the window is topmost.
    /// <see cref="ContentDialog"/> are treated as topmost for automation.
    /// </summary>
    bool IWindowProvider.IsTopmost => true;

    /// <summary>
    /// Gets the current interaction state of the dialog window for UI Automation.
    /// </summary>
    public WindowInteractionState InteractionState
    {
        get
        {
            if (Owner is ContentDialog dialog)
            {
                if (
                    !dialog.IsLoaded
                    || dialog.Dispatcher is { HasShutdownFinished: true } or { HasShutdownStarted: true }
                )
                {
                    return WindowInteractionState.Closing;
                }
            }

            return WindowInteractionState.Running;
        }
    }

    /// <summary>
    /// Gets a value indicating whether the window can be maximized.
    /// Always <see langword="false"/> for <see cref="ContentDialog"/>.
    /// </summary>
    public bool Maximizable => false;

    /// <summary>
    /// Gets a value indicating whether the window can be minimized.
    /// Always <see langword="false"/> for <see cref="ContentDialog"/>.
    /// </summary>
    public bool Minimizable => false;

    /// <summary>
    /// Gets the visual state of the window.
    /// <see cref="ContentDialog"/> report <see cref="WindowVisualState.Normal"/>.
    /// </summary>
    public WindowVisualState VisualState => WindowVisualState.Normal;

    /// <inheritdoc/>
    protected override string GetClassNameCore()
    {
        // "Emulating WinUI3's ContentDialog ClassName"
        return "Popup";
    }

    /// <inheritdoc/>
    protected override string? GetNameCore()
    {
        if (Owner is ContentDialog dialog)
        {
            return dialog.Title as string ?? dialog.Title?.ToString();
        }

        return base.GetNameCore();
    }

    /// <inheritdoc/>
    protected override AutomationControlType GetAutomationControlTypeCore()
    {
        return AutomationControlType.Window;
    }

#if NET48_OR_GREATER || NET5_0_OR_GREATER
    /// <inheritdoc/>
    protected override bool IsDialogCore()
    {
        return true;
    }
#endif

    /// <inheritdoc/>
    protected override bool IsControlElementCore()
    {
        return true;
    }

    /// <inheritdoc/>
    protected override bool IsContentElementCore()
    {
        return true;
    }

    /// <inheritdoc/>
    protected override bool IsKeyboardFocusableCore()
    {
        return false;
    }

    /// <summary>
    /// Returns whether the dialog is currently offscreen. A dialog is considered offscreen when not loaded or not visible.
    /// </summary>
    protected override bool IsOffscreenCore()
    {
        return Owner is ContentDialog { IsLoaded: false } or { IsVisible: false };
    }

    /// <summary>
    /// Returns automation pattern implementations supported by this peer. Provides <see cref="IWindowProvider"/>.
    /// </summary>
    /// <param name="pattern">The requested automation pattern.</param>
    /// <returns>An object implementing the requested pattern or <see langword="null"/> when not supported.</returns>
    public override object? GetPattern(PatternInterface pattern)
    {
        // Include PatternInterface.ScrollItem to align with WinUI3 behavior: WinUI3 exposes this pattern
        // for dialog-like popups, and exposing it here helps automation clients that rely on that behavior.
        if (pattern is PatternInterface.Window or PatternInterface.ScrollItem)
        {
            return this;
        }

        return null;
    }

    /// <summary>
    /// Closes the associated <see cref="ContentDialog"/>.
    /// This is invoked by UI Automation clients through the <see cref="IWindowProvider"/> pattern.
    /// </summary>
    void IWindowProvider.Close()
    {
        if (Owner is ContentDialog dialog)
        {
            Dispatcher? dispatcher = dialog.Dispatcher;
            if (dispatcher is { HasShutdownStarted: false, HasShutdownFinished: false })
            {
                dispatcher.BeginInvoke(
                    () =>
                    {
                        dialog.Hide();
                    },
                    DispatcherPriority.Normal
                );
            }
            else
            {
                dialog.Hide();
            }
        }
    }

    /// <summary>
    /// Sets the visual state of the window. Not supported for <see cref="ContentDialog"/>.
    /// </summary>
    void IWindowProvider.SetVisualState(WindowVisualState state)
    {
        // Not supported for this.
    }

    /// <summary>
    /// Waits for the dialog to become idle.
    /// Always returns <see langword="true"/> for <see cref="ContentDialog"/>.
    /// </summary>
    /// <param name="milliseconds">Maximum time to wait in milliseconds (ignored).</param>
    /// <returns>
    /// <see langword="true"/> if the dialog is idle or the operation completed;
    /// otherwise <see langword="false"/>.
    /// </returns>
    public bool WaitForInputIdle(int milliseconds)
    {
        return true;
    }
}
