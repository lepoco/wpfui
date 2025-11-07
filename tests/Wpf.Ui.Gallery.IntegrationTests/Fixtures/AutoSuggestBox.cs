// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.Generic;
using System.Linq;
using FlaUI.Core;
using FlaUI.Core.Exceptions;
using FlaUI.Core.Input;
using FlaUI.Core.Patterns;
using FlaUI.Core.WindowsAPI;

namespace Wpf.Ui.Gallery.IntegrationTests.Fixtures;

/// <summary>Class to interact with a <see cref="Wpf.Ui.Controls.AutoSuggestBox"/> element.</summary>
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
