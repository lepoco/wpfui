// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using UiButton = Wpf.Ui.Controls.Button;
using WinButton = System.Windows.Controls.Button;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

#pragma warning disable IDE0008 // Use explicit type instead of 'var'

/// <summary>
/// Partial class <c>ContentDialog.FocusBehavior</c>
///
/// This partial class implements the initial focus behavior for ContentDialog,
/// ensuring compliance with the Windows App SDK's official guidelines.
///
/// Reference:
/// https://learn.microsoft.com/en-us/windows/apps/develop/ui/controls/dialogs-and-flyouts/dialogs
/// </summary>
/// <remarks>
/// Implementation notes:
/// - Focuses only on initial display behavior, not ongoing focus management
/// - Works with both XAML-defined and dynamically added content
/// - Does not handle UI Automation or accessibility providers
/// </remarks>
public partial class ContentDialog
{
    // Temporarily suppress focus event listener
    private bool _suppressFocusRestore;

    /// <summary>
    /// Returns <see langword="true"/> when the keyboard focus is currently within the dialog's visual/logical tree.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the method is called from a non-UI thread.
    /// </exception>
    public bool IsFocusInsideDialog()
    {
        if (Dispatcher is not { HasShutdownStarted: false, HasShutdownFinished: false })
        {
            return false;
        }

        if (Dispatcher.CheckAccess())
        {
            return IsFocusInsideDialogCore(Keyboard.FocusedElement);
        }

        throw new InvalidOperationException(
            "IsFocusInsideDialog can only be called from the UI thread."
        );
    }

    /// <summary>
    /// Completely prevents focus from escaping the ContentDialog. When a focus escape is detected,
    /// the focus is forcibly pulled back into the dialog.
    /// </summary>
    protected override void OnPreviewLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
    {
        if (_suppressFocusRestore)
        {
            return;
        }

        var window = Window.GetWindow(this);
        if (e.NewFocus == null || window is not { IsActive: true })
        {
            return;
        }

        if (!IsFocusInsideDialogCore(e.NewFocus) && IsLoaded)
        {
            e.Handled = true;

            _suppressFocusRestore = true;

            Dispatcher.BeginInvoke(
                () =>
                {
                    if (e.OldFocus is { } old && IsFocusInsideDialogCore(old))
                    {
                        e.OldFocus.Focus();
                    }
                    else
                    {
                        Focus();
                    }

                    _suppressFocusRestore = false;
                },
                DispatcherPriority.Input
            );
        }
    }

    private bool IsFocusInsideDialogCore(IInputElement? element)
    {
        // ReSharper disable once SuspiciousTypeConversion.Global
        if (element is not DependencyObject focused)
        {
            return false;
        }

        var current = focused;

        while (current != null)
        {
            if (ReferenceEquals(current, this))
            {
                return true;
            }

            current = GetParent(current);
        }

        return false;
    }

    /// <summary>
    /// Sets the initial keyboard focus when the dialog is first displayed.
    /// </summary>
    /// <remarks>
    /// Priority strategy:
    /// 1. Content-first: focus the first focusable element within the user-provided
    ///    `Content` (the first focusable `<see cref="Control"/>`).
    /// 2. Built-in default button: if a built-in template button (Primary, Close, or
    ///    Secondary) is marked as default and is safely focusable, focus it (see
    ///    <see cref="FocusBuiltInButton"/>).
    /// 3. Template fallback: find any `System.Windows.Controls.Button` in the template
    ///    with `IsDefault == true` and focus it.
    /// 4. Fallback: if none of the above are available, make the
    ///    `ContentDialog` itself focusable and set focus to it.
    /// </remarks>
    protected virtual void SetInitialFocus()
    {
        // 1) Primary (content-first): focus first focusable element within user-provided content.
        var content = Content as DependencyObject;

        // Prefer `Control` (elements deriving from System.Windows.Controls.Control) because
        // they reliably provide UI Automation peers and are recognized by screen readers.
        var firstFocusable = FindDescendant<Control>(content, IsSafelyFocusable);
        if (firstFocusable is not null)
        {
            firstFocusable.Focus();
            return;
        }

        // 2) Secondary: focus built-in default button placed in template footer
        if (FocusBuiltInButton())
        {
            return;
        }

        // 3) Template fallback: try to find any custom button marked as default (IsDefault == true)
        var templateDefault = FindDescendant<WinButton>(
            this,
            b => b is { IsDefault: true } && IsSafelyFocusable(b)
        );

        if (templateDefault is not null)
        {
            templateDefault.Focus();
            return;
        }

        /*
            At this point, there are no safely focusable controls available. The final attempt is to set focus to the
            ContentDialog itself. Since ContentDialog contains a full-window overlay mask layer, UI automation tools
            will recognize the ContentDialog's size as the mask layer's dimensions. Therefore, if the focus indicator
            appears inconsistent with the "dialog" size, do not be surprised.
        */

        // 4) Fallback: make ContentDialog focusable and focus it
        SetCurrentValue(FocusableProperty, true);
        Focus();
    }

    private bool FocusBuiltInButton()
    {
        var safelyButtons = new List<WinButton>();

        if (GetTemplateChild("PrimaryButton") is WinButton primaryBtn && IsSafelyFocusable(primaryBtn))
        {
            safelyButtons.Add(primaryBtn);
        }

        if (GetTemplateChild("CloseButton") is WinButton closeBtn && IsSafelyFocusable(closeBtn))
        {
            safelyButtons.Add(closeBtn);
        }

        if (GetTemplateChild("SecondaryButton") is WinButton secondaryBtn && IsSafelyFocusable(secondaryBtn))
        {
            safelyButtons.Add(secondaryBtn);
        }

        // Priority: find the first IsDefault button and select it, then return.
        foreach (var btn in safelyButtons)
        {
            if (btn.IsDefault)
            {
                btn.Focus();
                return true;
            }
        }

        // Fallback: Find the first button and focus it, then return.
        if (safelyButtons.Count > 0)
        {
            safelyButtons[0].Focus();
            return true;
        }

        return false;
    }

    private static T? FindDescendant<T>(DependencyObject? root, Predicate<T> predicate)
        where T : DependencyObject
    {
        if (root == null)
        {
            return null;
        }

        try
        {
            // If the root is a Visual or Visual3D, traverse the visual tree
            if (root is Visual or Visual3D)
            {
                var childrenCount = VisualTreeHelper.GetChildrenCount(root);

                for (var i = 0; i < childrenCount; i++)
                {
                    var child = VisualTreeHelper.GetChild(root, i);

                    if (child is T t && predicate(t))
                    {
                        return t;
                    }

                    T? found = FindDescendant(child, predicate);
                    if (found is not null)
                    {
                        return found;
                    }
                }

                return null;
            }

            // For non-visual elements, fall back to logical tree traversal
            foreach (var logicalChild in LogicalTreeHelper.GetChildren(root))
            {
                if (logicalChild is not DependencyObject child)
                {
                    continue;
                }

                if (child is T t && predicate(t))
                {
                    return t;
                }

                T? found = FindDescendant(child, predicate);
                if (found is not null)
                {
                    return found;
                }
            }
        }
        catch
        {
            // defensive: ignore traversal errors and return null
        }

        return null;
    }

    private static bool IsSafelyFocusable(DependencyObject? dp)
    {
        switch (dp)
        {
            case not (UIElement or ContentElement or UIElement3D):
            case UIElement ue when !ue.Focusable || !ue.IsVisible || !ue.IsEnabled:
            case Control { IsTabStop: false }:
            case ContentElement { Focusable: false }:
            case UIElement3D { Focusable: false }:
            case UiButton { Appearance: ControlAppearance.Danger or ControlAppearance.Caution }:
                return false;
        }

        return true;
    }

    private static DependencyObject? GetParent(DependencyObject d)
    {
        try
        {
            var p = VisualTreeHelper.GetParent(d);
            if (p != null)
            {
                return p;
            }

            var lp = LogicalTreeHelper.GetParent(d);
            if (lp != null)
            {
                return lp;
            }
        }
        catch
        {
            // ignored
        }

        if (d is FrameworkElement fe)
        {
            return fe.Parent ?? fe.TemplatedParent;
        }

        return null;
    }
}

#pragma warning restore IDE0008 // Use explicit type instead of 'var'