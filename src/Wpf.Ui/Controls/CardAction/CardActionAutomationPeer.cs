// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Automation.Peers;

namespace Wpf.Ui.Controls;

internal class CardActionAutomationPeer : FrameworkElementAutomationPeer
{
    private readonly CardAction _owner;

    public CardActionAutomationPeer(CardAction owner)
        : base(owner)
    {
        _owner = owner;
    }

    protected override string GetClassNameCore()
    {
        return "Button";
    }

    protected override AutomationControlType GetAutomationControlTypeCore()
    {
        return AutomationControlType.Button;
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
        if (_owner.Content is UIElement element)
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
}
