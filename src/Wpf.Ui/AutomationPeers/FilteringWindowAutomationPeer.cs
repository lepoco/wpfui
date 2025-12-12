// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Automation.Peers;

namespace Wpf.Ui.AutomationPeers;

/// <summary>
/// Represents a <see cref="Window"/> automation peer that filters child automation peers.
/// </summary>
/// <remarks>
/// <para>
/// This peer excludes child peers whose owner elements (or their ancestors) have been explicitly
/// registered to suppress automation. The suppression mechanism is used by dialogs (for example
/// <c>ContentDialog</c>) to hide underlying window content from UI Automation while the dialog is active.
/// </para>
/// <para>
/// Note: The filtering performed by this peer is shallow — it only excludes immediate child peers
/// returned by <see cref="GetChildrenCore"/>. If a child peer is kept, its own children are not
/// additionally filtered by this class (i.e. there is no recursive pruning of descendants).
/// Developers who need different behavior can override <see cref="GetChildrenCore"/>,
/// <see cref="GetChildrenCoreInternal"/> or <see cref="ShouldIncludeChild(AutomationPeer,DependencyObject?)"/>
/// to implement custom filtering (including recursive pruning) as required.
/// </para>
/// <para>
/// Usage: to install and use this peer for a window, override the window's
/// <c>OnCreateAutomationPeer</c> to return a new <see cref="FilteringWindowAutomationPeer"/> instance.
/// Example:
/// <code>
/// protected override AutomationPeer OnCreateAutomationPeer()
/// {
///     return new FilteringWindowAutomationPeer(this);
/// }
/// </code>
/// After the peer is created you can obtain it via
/// <c>FrameworkElementAutomationPeer.FromElement(yourWindow) as FilteringWindowAutomationPeer</c>
/// and then call <see cref="RegisterSuppressed"/>, <see cref="UnregisterSuppressed"/> or
/// <see cref="ClearRegisteredSuppressed"/> as needed. When the visible structure changes,
/// invalidate the peer's children cache by calling <see cref="AutomationPeer.ResetChildrenCache()"/>
/// so UI Automation clients will observe the updated structure. Interactions with the peer
/// should be performed on the UI thread (Dispatcher).
/// </para>
/// </remarks>
public class FilteringWindowAutomationPeer : WindowAutomationPeer
{
    /// <summary>
    /// Cache of reflected "Owner" <see cref="PropertyInfo"/> per <see cref="AutomationPeer"/> type.
    /// </summary>
    private static readonly ConcurrentDictionary<Type, PropertyInfo?> OwnerPropCache = new();

    // Registered suppressed elements (used by ContentDialog to inform the window peer)
    // Use a ConditionalWeakTable so registrations don't keep elements alive.
    private ConditionalWeakTable<DependencyObject, object> _registeredSuppressed = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="FilteringWindowAutomationPeer"/> class.
    /// </summary>
    /// <param name="owner">The <see cref="Window"/> that this automation peer represents.</param>
    public FilteringWindowAutomationPeer(Window owner)
        : base(owner)
    {
    }

    /// <summary>
    /// Determines whether the specified element has any ancestor that was registered as suppressed.
    /// </summary>
    /// <param name="d">The element to start the ancestor search from.</param>
    /// <returns><see langword="true"/> if an ancestor is registered suppressed; otherwise <see langword="false"/>.</returns>
    /// <remarks>
    /// The search walks up the visual and logical tree using <c>VisualTreeHelper.GetParent</c> and
    /// <c>LogicalTreeHelper.GetParent</c>.
    /// </remarks>
    private bool IsUnderSuppressedAncestor(DependencyObject d)
    {
        DependencyObject current = d;
        while (current != null)
        {
            if (IsRegisteredSuppressed(current))
            {
                return true;
            }

            current = VisualTreeHelper.GetParent(current) ?? LogicalTreeHelper.GetParent(current);
        }

        return false;
    }

    /// <summary>
    /// Attempts to obtain the owner element for the given automation peer.
    /// </summary>
    /// <param name="peer">The automation peer to inspect. May be <see langword="null"/>.</param>
    /// <returns>The owner <see cref="DependencyObject"/> if available; otherwise <see langword="null"/>.</returns>
    /// <remarks>
    /// Uses reflection to read an "Owner" property (public or non-public) when present. Any exceptions
    /// while reading the property are ignored and treated as missing owner.
    /// </remarks>
    private static DependencyObject? GetPeerOwner(AutomationPeer? peer)
    {
        if (peer == null)
        {
            return null;
        }

        // get or add reflected Owner property info for this peer type, caching for future use
        PropertyInfo? prop = OwnerPropCache.GetOrAdd(
            peer.GetType(),
            t => t.GetProperty("Owner", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
        );

        try
        {
            return prop?.GetValue(peer) as DependencyObject;
        }
        catch
        {
            // ignore exceptions from GetValue and treat as no owner
            return null;
        }
    }

    /// <summary>
    /// Registers an element to be suppressed from automation.
    /// </summary>
    /// <param name="d">The element to register. No-op if <see langword="null"/>.</param>
    public void RegisterSuppressed(DependencyObject? d)
    {
        if (d == null)
        {
            return;
        }

        _registeredSuppressed.GetValue(d, _ => new object());
    }

    /// <summary>
    /// Unregisters a previously suppressed element.
    /// </summary>
    /// <param name="d">The element to unregister. No-op if <see langword="null"/> or not registered.</param>
    public void UnregisterSuppressed(DependencyObject? d)
    {
        if (d == null)
        {
            return;
        }

        _registeredSuppressed.Remove(d);
    }

    /// <summary>
    /// Clears all registered suppressed elements.
    /// </summary>
    public void ClearRegisteredSuppressed()
    {
        // Replace the table with a new instance to clear entries.
        _registeredSuppressed = new ConditionalWeakTable<DependencyObject, object>();
    }

    /// <summary>
    /// Determines whether the specified element was explicitly registered as suppressed.
    /// </summary>
    /// <param name="d">The element to check.</param>
    /// <returns><see langword="true"/> if the element is registered suppressed; otherwise <see langword="false"/>.</returns>
    private bool IsRegisteredSuppressed(DependencyObject d)
    {
        return _registeredSuppressed.TryGetValue(d, out _);
    }

    /// <summary>
    /// Determines whether a given child automation peer should be included in the children list.
    /// </summary>
    /// <param name="peer">The child automation peer being evaluated.</param>
    /// <param name="owner">The owner element of the peer when available; otherwise <c>null</c>.</param>
    /// <returns><c>true</c> if the peer should be included; otherwise <c>false</c>.</returns>
    protected virtual bool ShouldIncludeChild(AutomationPeer peer, DependencyObject? owner)
    {
        if (owner != null)
        {
            // default behavior: exclude if the owner itself is registered suppressed
            // or any of its ancestors is registered suppressed
            if (IsRegisteredSuppressed(owner) || IsUnderSuppressedAncestor(owner))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Returns a filtered list of immediate child automation peers. This protected virtual method
    /// contains the default filtering implementation and can be overridden by derived classes to
    /// provide a completely custom children retrieval logic.
    /// </summary>
    /// <returns>A list of child <see cref="AutomationPeer"/> objects that are not suppressed,
    /// or <see langword="null"/> if none.</returns>
    protected virtual List<AutomationPeer>? GetChildrenCoreInternal()
    {
        List<AutomationPeer>? children = base.GetChildrenCore();

        if (children == null)
        {
            return null;
        }

        var filtered = new List<AutomationPeer>();

        foreach (AutomationPeer peer in children)
        {
            DependencyObject? owner = GetPeerOwner(peer);

            if (!ShouldIncludeChild(peer, owner))
            {
                continue;
            }

            filtered.Add(peer);
        }

        return filtered;
    }

    /// <summary>
    /// Returns a filtered list of immediate child automation peers, excluding peers whose owner or ancestor
    /// elements are marked to suppress automation.
    /// </summary>
    /// <returns>A list of child <see cref="AutomationPeer"/> objects that are not suppressed,
    /// or <see langword="null"/> if none.</returns>
    protected override List<AutomationPeer>? GetChildrenCore()
    {
        return GetChildrenCoreInternal();
    }
}