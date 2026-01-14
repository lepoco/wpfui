// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;

namespace Wpf.Ui.Controls;

internal class NavigationViewItemAutomationPeer : FrameworkElementAutomationPeer, IExpandCollapseProvider, ISelectionItemProvider
{
    private readonly NavigationViewItem _owner;

    public NavigationViewItemAutomationPeer(NavigationViewItem owner)
        : base(owner)
    {
        _owner = owner;
    }

    protected override string GetClassNameCore()
    {
        return "NavigationItem";
    }

    protected override AutomationControlType GetAutomationControlTypeCore()
    {
        return AutomationControlType.TabItem;
    }

    public override object GetPattern(PatternInterface patternInterface)
    {
        // Only provide expand collapse pattern if we have children! https://github.com/microsoft/microsoft-ui-xaml/blob/50177b54e88e923e24440df679bdf984b0048ab4/src/controls/dev/NavigationView/NavigationViewItemAutomationPeer.cpp#L52
        if (patternInterface == PatternInterface.SelectionItem || (patternInterface == PatternInterface.ExpandCollapse && _owner is { HasMenuItems: true }))
        {
            return this;
        }

        return base.GetPattern(patternInterface);
    }

    public ExpandCollapseState ExpandCollapseState
    {
        get
        {
            if (!_owner.HasMenuItems)
            {
                return ExpandCollapseState.LeafNode;
            }

            return _owner.IsExpanded ? ExpandCollapseState.Expanded : ExpandCollapseState.Collapsed;
        }
    }

    public void Collapse()
    {
        if (!_owner.HasMenuItems)
        {
            return;
        }

        ExpandCollapseState oldState = ExpandCollapseState;

        if (oldState == ExpandCollapseState.Collapsed)
        {
            return;
        }

        _owner.SetCurrentValue(NavigationViewItem.IsExpandedProperty, false);

        RaisePropertyChangedEvent(
            ExpandCollapsePatternIdentifiers.ExpandCollapseStateProperty,
            oldState,
            ExpandCollapseState.Collapsed
        );
    }

    public void Expand()
    {
        if (!_owner.HasMenuItems)
        {
            return;
        }

        ExpandCollapseState oldState = ExpandCollapseState;

        if (oldState == ExpandCollapseState.Expanded)
        {
            return;
        }

        _owner.SetCurrentValue(NavigationViewItem.IsExpandedProperty, true);

        RaisePropertyChangedEvent(
            ExpandCollapsePatternIdentifiers.ExpandCollapseStateProperty,
            oldState,
            ExpandCollapseState.Expanded
        );
    }

    protected override AutomationPeer GetLabeledByCore()
    {
        if (_owner.Content is UIElement element)
        {
            return CreatePeerForElement(element);
        }

        return base.GetLabeledByCore();
    }

    protected override string GetNameCore()
    {
        string result = base.GetNameCore() ?? string.Empty;

        if (result == string.Empty)
        {
            result = AutomationProperties.GetName(_owner);
        }

        if (result == string.Empty && _owner.Content is DependencyObject d)
        {
            result = AutomationProperties.GetName(d);
        }

        if (result == string.Empty && _owner.Content is string s)
        {
            result = s;
        }

        return result;
    }

    void ISelectionItemProvider.AddToSelection()
    {
        // This is a single select control, so just select the item.
        ((ISelectionItemProvider)this).Select();
    }

    bool ISelectionItemProvider.IsSelected
    {
        get
        {
            if (NavigationView.GetNavigationParent(_owner) is not { } navigationView)
            {
                return false;
            }

            return navigationView.SelectedItem == _owner;
        }
    }

    IRawElementProviderSimple? ISelectionItemProvider.SelectionContainer
    {
        get
        {
            if (NavigationView.GetNavigationParent(_owner) is not { } navigationView)
            {
                return null;
            }

            if (CreatePeerForElement(navigationView) is { } peer)
            {
                return ProviderFromPeer(peer);
            }

            return null;
        }
    }

    void ISelectionItemProvider.RemoveFromSelection()
    {
        if (NavigationView.GetNavigationParent(_owner) is not { } navigationView)
        {
            return;
        }

        if (navigationView.SelectedItem != _owner)
        {
            return;
        }

        navigationView.ClearSelectedItem();
    }

    void ISelectionItemProvider.Select()
    {
        if (NavigationView.GetNavigationParent(_owner) is not { } navigationView)
        {
            return;
        }

        navigationView.OnNavigationViewItemClick(_owner);
    }
}
