// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Threading;

namespace Wpf.Ui.Controls;

internal class CardActionAutomationPeer : FrameworkElementAutomationPeer, IInvokeProvider
{
    public CardActionAutomationPeer(CardAction owner)
        : base(owner) { }

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
        if (patternInterface == PatternInterface.Invoke)
        {
            return this;
        }

        return base.GetPattern(patternInterface);
    }

    void IInvokeProvider.Invoke()
    {
        if (!IsEnabled())
        {
            throw new ElementNotEnabledException();
        }

        // Async call of click event
        // In ClickHandler opens a dialog and suspend the execution we don't want to block this thread
        Dispatcher.BeginInvoke(
            DispatcherPriority.Input,
            new DispatcherOperationCallback(_ =>
            {
                ((CardAction)Owner).AutomationClick();
                return null;
            }),
            null
        );
    }
}
