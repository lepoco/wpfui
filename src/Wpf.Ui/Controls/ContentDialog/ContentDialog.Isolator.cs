// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Diagnostics;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using Wpf.Ui.AutomationPeers;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Partial class for <see cref="ContentDialog"/> that isolates the dialog host from
/// the rest of the application's UI while the dialog is shown.
/// </summary>
/// <remarks>
/// While a dialog is visible this part temporarily disables or suppresses sibling
/// elements of the dialog host to prevent user interaction and to keep automation
/// tools focused on the dialog. Restoration preserves bindings by using
/// <see cref="DependencyObject.SetCurrentValue"/> semantics.
///
/// Failure/exception policy: methods in this class are fail-safe and will not
/// throw when automation peers cannot be created or when individual element
/// operations fail. Exceptions are caught and written to the debug output so
/// that runtime callers are not affected by automation or visual tree issues.
/// </remarks>
public partial class ContentDialog
{
    // Tracks elements that were disabled so they can be restored later.
    private readonly HashSet<DependencyObject> _disabledElements = [];

    // Tracks elements that were registered as suppressed with automation peers.
    // Records objects that were passed to the window automation peer via
    // 'RegisterSuppressed' (current strategy: register the parent sibling).
    private readonly HashSet<DependencyObject> _suppressedUIAElements = [];

    /// <summary>Identifies the <see cref="DisableSiblings"/> dependency property.</summary>
    public static readonly DependencyProperty DisableSiblingsProperty = DependencyProperty.Register(
        nameof(DisableSiblings),
        typeof(bool),
        typeof(ContentDialog),
        new PropertyMetadata(false)
    );

    /// <summary>
    /// Gets or sets a value indicating whether sibling elements of the dialog host should be
    /// disabled while the dialog is displayed. The default value is <see langword="false"/>.
    /// </summary>
    /// <value>
    /// <see langword="true"/> to disable sibling elements; <see langword="false"/> to leave
    /// sibling elements unaffected.
    /// </value>
    /// <remarks>
    /// When enabled, sibling elements in the host window may appear disabled while the
    /// <see cref="ContentDialog"/> is displayed. This option is disabled by default and
    /// is intended for scenarios where background interaction must be prevented.
    ///
    /// Note: the class is fail-safe — automation failures or visual tree issues are
    /// logged to debug output and do not throw to callers.
    /// </remarks>
    public bool DisableSiblings
    {
        get => (bool)GetValue(DisableSiblingsProperty);
        set => SetValue(DisableSiblingsProperty, value);
    }

    /// <summary>
    /// Isolates sibling elements of the dialog host by suppressing their automation peers
    /// and disabling user interaction. This method ensures that the dialog is the only
    /// focusable element while it is displayed.
    /// </summary>
    private void IsolateDialogHostSiblings()
    {
        _disabledElements.Clear();
        _suppressedUIAElements.Clear();

        if (DialogHost == null)
        {
            return;
        }

        // Find parent in visual or logical tree.
        DependencyObject? parent = GetParent(DialogHost);
        if (parent == null)
        {
            return;
        }

        // Attempt to obtain a FilteringWindowAutomationPeer for the hosting window.
        FilteringWindowAutomationPeer? peer = GetFilteringWindowPeer(DialogHost);

        // Enumerate children depending on parent type.
        if (parent is Visual or Visual3D)
        {
            var count = VisualTreeHelper.GetChildrenCount(parent);
            for (var i = 0; i < count; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                SuppressAndDisableInternal(child, peer);
            }
        }
        else if (parent is Panel panel)
        {
            foreach (UIElement child in panel.Children.OfType<UIElement>())
            {
                SuppressAndDisableInternal(child, peer);
            }
        }
        else if (parent is ContentControl contentControl)
        {
            if (contentControl.Content is DependencyObject content)
            {
                SuppressAndDisableInternal(content, peer);
            }
        }
        else if (parent is Decorator decorator)
        {
            if (decorator.Child is DependencyObject child)
            {
                SuppressAndDisableInternal(child, peer);
            }
        }
        else if (parent is ItemsControl itemsControl)
        {
            foreach (var item in itemsControl.Items)
            {
                DependencyObject? container = itemsControl.ItemContainerGenerator.ContainerFromItem(item);
                if (container != null)
                {
                    SuppressAndDisableInternal(container, peer);
                }
            }
        }
        else
        {
            // Fallback to logical children.
            foreach (var obj in LogicalTreeHelper.GetChildren(parent).OfType<object>())
            {
                if (obj is DependencyObject d)
                {
                    SuppressAndDisableInternal(d, peer);
                }
            }
        }

        try
        {
            peer?.ResetChildrenCache();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"ContentDialog:IsolateDialogHostSiblings.NotifyStructureChanged failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Restores sibling elements of the dialog host that were previously suppressed
    /// and disabled. This method re-enables user interaction and restores automation
    /// peers for the affected elements.
    /// </summary>
    private void RestoreDialogHostSiblings()
    {
        // Ensure UI thread
        if (!Dispatcher.CheckAccess())
        {
            Dispatcher.Invoke(RestoreDialogHostSiblings);
            return;
        }

        // Re-enable elements we disabled. Use SetCurrentValue to preserve bindings.
        foreach (DependencyObject obj in _disabledElements)
        {
            try
            {
                switch (obj)
                {
                    case UIElement ui:
                        ui.SetCurrentValue(IsEnabledProperty, true);
                        break;

                    case FrameworkContentElement fce:
                        fce.SetCurrentValue(ContentElement.IsEnabledProperty, true);
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ContentDialog:RestoreDialogHostSiblings failed to re-enable element: {ex.Message}");
            }
        }

        if (GetFilteringWindowPeer(DialogHost) is FilteringWindowAutomationPeer peer)
        {
            // Unregister automation suppression for tracked elements.
            foreach (DependencyObject obj in _suppressedUIAElements)
            {
                try
                {
                    peer.UnregisterSuppressed(obj);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"ContentDialog:RestoreDialogHostSiblings failed to unregister suppression: {ex.Message}");
                }
            }

            peer.ResetChildrenCache();
        }

        _disabledElements.Clear();
        _suppressedUIAElements.Clear();
    }

    // Helper to obtain or create a FilteringWindowAutomationPeer for the window that contains the provided element.
    // Returns null if the peer cannot be created or the element is not hosted in a Window.
    private static FilteringWindowAutomationPeer? GetFilteringWindowPeer(DependencyObject? element)
    {
        if (element is not FrameworkElement fe)
        {
            return null;
        }

        var hostWindow = Window.GetWindow(fe);
        if (hostWindow == null)
        {
            return null;
        }

        var peer = UIElementAutomationPeer.FromElement(hostWindow) as FilteringWindowAutomationPeer;
        if (peer == null)
        {
            try
            {
                UIElementAutomationPeer.CreatePeerForElement(hostWindow);
                peer = UIElementAutomationPeer.FromElement(hostWindow) as FilteringWindowAutomationPeer;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ContentDialog:GetFilteringWindowPeer failed to create peer: {ex.Message}");
            }
        }

        return peer;
    }

    // Suppresses automation and disables the provided element as needed.
    // - Skips registration if an ancestor is already registered as suppressed (avoids duplicate peer calls).
    // - Always attempts to register suppression for the provided element unless an ancestor is already registered.
    // - Disables the target element (or TitleBar.TrailingContent) when requested.
    private void SuppressAndDisableInternal(DependencyObject? d, FilteringWindowAutomationPeer? peer)
    {
        if (d == null)
        {
            return;
        }

        if (IsPartOfThisDialog(d) || ReferenceEquals(d, DialogHost))
        {
            return;
        }

        try
        {
            if (!IsAncestorAlreadySuppressed(d) && peer != null)
            {
                peer.RegisterSuppressed(d);
                _suppressedUIAElements.Add(d);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"ContentDialog:RegisterSuppressed failed: {ex.Message}");
        }

        if (!DisableSiblings)
        {
            return;
        }

        DependencyObject? dp = d;

        // Special handling: only disable the TrailingContent in the title bar
        if (d is TitleBar titleBar)
        {
            dp = titleBar.TrailingContent as DependencyObject;
        }

        switch (dp)
        {
            case UIElement { IsEnabled: true } ui:
                _disabledElements.Add(dp);
                ui.SetCurrentValue(IsEnabledProperty, false);
                break;

            case FrameworkContentElement { IsEnabled: true } fce:
                _disabledElements.Add(dp);
                fce.SetCurrentValue(ContentElement.IsEnabledProperty, false);
                break;
        }
    }

    // Walks up the visual/logical parent chain to determine if an ancestor has already been suppressed.
    private bool IsAncestorAlreadySuppressed(DependencyObject d)
    {
        try
        {
            DependencyObject? parent = GetParent(d);
            while (parent != null)
            {
                if (_suppressedUIAElements.Contains(parent))
                {
                    return true;
                }

                parent = GetParent(parent);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"ContentDialog:IsAncestorAlreadySuppressed failed: {ex.Message}");
        }

        return false;
    }

    // Safe parent retrieval that tries visual then logical then framework fallbacks.
    private static DependencyObject? GetParent(DependencyObject d)
    {
        try
        {
            DependencyObject? p = VisualTreeHelper.GetParent(d);
            if (p != null)
            {
                return p;
            }

            DependencyObject? lp = LogicalTreeHelper.GetParent(d);
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

