// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using FlaUI.Core;
using FlaUI.Core.Conditions;
using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;

namespace Wpf.Ui.Gallery.IntegrationTests.Fixtures;

/// <summary>
/// Base class for UI tests implementing <see cref="Xunit.IAsyncLifetime"/> to manage <see cref="FlaUI.Core.Application"/> lifecycle.
/// </summary>
public abstract class UiTest : IAsyncLifetime
{
    private readonly TestedApplication app = new();

    /// <summary>
    /// Gets the wrapper for an application which should be automated.
    /// </summary>
    internal Application? Application => app.Application;

    /// <summary>
    /// Gets the main window of the applications process.
    /// </summary>
    internal Window? MainWindow => app.MainWindow;

    /// <inheritdoc />
    public ValueTask InitializeAsync() => app.InitializeAsync();

    /// <inheritdoc />
    public ValueTask DisposeAsync() => app.DisposeAsync();

    /// <summary>
    /// Finds the first descendant with the given automation id.
    /// </summary>
    /// <param name="automationId">The automation id.</param>
    /// <returns>The found element or null if no element was found.</returns>
    protected AutomationElement? FindFirst(string automationId) => app.MainWindow?.FindFirstDescendant(automationId);

    /// <summary>Finds the first descendant with the condition.</summary>
    /// <param name="conditionFunc">The condition method.</param>
    /// <returns>The found element or null if no element was found.</returns>
    protected AutomationElement? FindFirst(Func<ConditionFactory, ConditionBase> conditionFunc) =>
        app.MainWindow?.FindFirstDescendant(conditionFunc);

    /// <summary>
    /// Creates a Task that will complete after a time delay.
    /// </summary>
    /// <param name="seconds">The time delay in seconds.</param>
    /// <returns>A Task that represents the time delay</returns>
    /// <remarks>
    /// After the specified time delay, the Task is completed in RanToCompletion state.
    /// </remarks>
    protected Task Wait(int seconds) => Task.Delay(TimeSpan.FromSeconds(seconds));

    /// <summary>
    /// Simulate typing in text. This is slower than setting <see cref="P:FlaUI.Core.AutomationElements.TextBox.Text" /> but raises more events.
    /// </summary>
    protected void Enter(string value)
    {
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

        FlaUI.Core.Input.Wait.UntilInputIsProcessed();
    }

    /// <summary>
    /// Type the given key.
    /// </summary>
    protected void Press(VirtualKeyShort virtualKey)
    {
        Keyboard.Type(virtualKey);
        
        FlaUI.Core.Input.Wait.UntilInputIsProcessed();
    }
}
