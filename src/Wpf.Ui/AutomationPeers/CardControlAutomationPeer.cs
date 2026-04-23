// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Automation.Peers;
using Wpf.Ui.Controls;

namespace Wpf.Ui.AutomationPeers;

/// <summary>
/// Provides UI Automation peer for the CardControl.
/// </summary>
internal class CardControlAutomationPeer : FrameworkElementAutomationPeer
{
    public CardControlAutomationPeer(CardControl owner)
        : base(owner) { }

    protected override string GetClassNameCore()
    {
        return "CardControl";
    }

    protected override AutomationControlType GetAutomationControlTypeCore()
    {
        return AutomationControlType.Pane;
    }
}
