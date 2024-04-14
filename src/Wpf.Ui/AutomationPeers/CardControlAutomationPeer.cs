// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Automation;
using System.Windows.Automation.Peers;
using Wpf.Ui.Controls;

namespace Wpf.Ui.AutomationPeers;

internal class CardControlAutomationPeer : FrameworkElementAutomationPeer
{
    private readonly CardControl _owner;

    public CardControlAutomationPeer(CardControl owner)
        : base(owner)
    {
        _owner = owner;
    }

    protected override string GetClassNameCore()
    {
        return "CardControl";
    }

    protected override AutomationControlType GetAutomationControlTypeCore()
    {
        return AutomationControlType.Pane;
    }

    public override object GetPattern(PatternInterface patternInterface)
    {
        if (patternInterface == PatternInterface.ItemContainer)
        {
            return this;
        }

        return base.GetPattern(patternInterface);
    }

    protected override AutomationPeer GetLabeledByCore()
    {
        if (_owner.Header is UIElement element)
        {
            return CreatePeerForElement(element);
        }

        return base.GetLabeledByCore();
    }

    protected override string GetNameCore()
    {
        var result = base.GetNameCore() ?? string.Empty;

        if (result == string.Empty)
        {
            result = AutomationProperties.GetName(_owner);
        }

        if (result == string.Empty && _owner.Header is DependencyObject d)
        {
            result = AutomationProperties.GetName(d);
        }

        if (result == string.Empty && _owner.Header is string s)
        {
            result = s;
        }

        return result;
    }
}
