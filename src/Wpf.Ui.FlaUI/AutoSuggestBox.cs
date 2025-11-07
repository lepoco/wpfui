// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.FlaUI;

/// <summary>
/// Class to interact with a WPF UI AutoSuggestBox element.
/// </summary>
public class AutoSuggestBox(FrameworkAutomationElementBase frameworkAutomationElement)
    : AutomationElement(frameworkAutomationElement)
{
    /// <summary>
    /// Simulate typing in text.
    /// </summary>
    public void Enter(string value)
    {
        this.Click();

        this.Patterns.Value.PatternOrDefault?.SetValue(string.Empty);
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        string[] source = value.Replace("\r\n", "\n").Split('\n');

        Keyboard.Type(source[0]);

        foreach (string text in ((IEnumerable<string>)source).Skip<string>(1))
        {
            Keyboard.Type(VirtualKeyShort.RETURN);
            Keyboard.Type(text);
        }

        Keyboard.Type(VirtualKeyShort.ENTER);

        Wait.UntilInputIsProcessed();
    }
}
