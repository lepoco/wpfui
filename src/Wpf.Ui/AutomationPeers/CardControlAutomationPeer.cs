using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using Wpf.Ui.Controls;

namespace Wpf.Ui.AutomationPeers;

internal class CardControlAutomationPeer : FrameworkElementAutomationPeer
{
    private readonly CardControl _owner;

    public CardControlAutomationPeer(CardControl owner) : base(owner)
    {
        this._owner = owner;
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
        if (this._owner.Header is UIElement element)
        {
            return CreatePeerForElement(element);
        }

        return base.GetLabeledByCore();
    }

    protected override string GetNameCore()
    {
        string result = base.GetNameCore() ?? String.Empty;

        if (result == String.Empty)
        {
            result = AutomationProperties.GetName(this._owner);
        }

        if (result == String.Empty && this._owner.Header is DependencyObject d)
        {
            result = AutomationProperties.GetName(d);
        } 

        if (result == String.Empty && this._owner.Header is string s)
        {
            result = s;
        }

        return result;
    }
}