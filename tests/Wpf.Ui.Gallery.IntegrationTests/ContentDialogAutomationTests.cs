// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using FlaUI.Core.Definitions;

#pragma warning disable IDE0008 // Use explicit type instead of 'var'
#pragma warning disable SA1512  // Single-line comments should not be followed by blank line

namespace Wpf.Ui.Gallery.IntegrationTests;

public sealed class ContentDialogAutomationTests : UiTest
{
    [Fact]
    public async Task ContentDialog_Should_Suppress_Background_Automation_WhenShown()
    {
        // Give the test app a moment to display the window before starting UI automation interactions
        await Wait(2, TestContext.Current.CancellationToken);

        // Navigate to the ContentDialog page explicitly: click parent then child nav items
        var parentNav = FindFirst(c => c.ByText("Dialogs & flyouts"));
        parentNav.Should().NotBeNull("because the Dialogs & flyouts navigation item should be present");
        parentNav.Click();

        await Wait(1, TestContext.Current.CancellationToken);

        var childNav = FindFirst(c => c.ByText("ContentDialog"));
        if (childNav == null)
        {
            // If the child item is not immediately visible, try toggling the parent to expand it and retry
            parentNav.Click();
            await Wait(1, TestContext.Current.CancellationToken);
            childNav = FindFirst(c => c.ByText("ContentDialog"));
        }

        childNav.Should().NotBeNull("because the ContentDialog navigation item should be present as a child");
        childNav.Click();

        await Wait(1, TestContext.Current.CancellationToken);

        var showButton = FindFirst(c => c.ByText("Show"));
        showButton.Should().NotBeNull("because the ContentDialog page must contain a Show button to open the dialog");
        showButton.AsButton().Click();

        await Wait(1, TestContext.Current.CancellationToken);

        // After opening dialog, the primary (Save) button should have keyboard focus before suppression checks
        var saveButton = FindFirst(c => c.ByText("Save"));
        saveButton.Should().NotBeNull("because dialog must contain a Save button");

        bool saveHasFocus;
        try
        {
            saveHasFocus = saveButton.Properties.HasKeyboardFocus.TryGetValue(out var v) && v;
        }
        catch
        {
            // If property access fails, treat as not focused (test will assert accordingly)
            saveHasFocus = false;
        }

        saveHasFocus.Should().BeTrue("because the Save button should receive focus when the dialog is shown");

        // Background elements should be suppressed from automation while the ContentDialog is shown.
        var bgParent = FindFirst(c => c.ByText("Dialogs & flyouts"));
        bgParent.Should().BeNull("because background UI should be suppressed when dialog is shown");

        var bgChild = FindFirst(c => c.ByText("ContentDialog"));
        bgChild.Should().BeNull("because background UI should be suppressed when dialog is shown");

        var bgShow = FindFirst(c => c.ByText("Show"));
        bgShow.Should().BeNull("because background UI should be suppressed when dialog is shown");

        var navAutoSuggest = FindFirst("NavigationAutoSuggestBox");
        navAutoSuggest.Should().BeNull("because background UI automation should be suppressed while dialog is open");

        // Now exercise each dialog button (Primary, Secondary, Close) and verify
        // the page TextBlock updates according to ContentDialogViewModel.

        // Primary -> "Save" -> expect "User saved their work"
        await OpenDialog();
        await ClickButtonMatching(["Save"]);
        await WaitForText("User saved their work");

        // Secondary -> "Don't Save" -> expect "User did not save their work"
        await OpenDialog();
        await ClickButtonMatching(["Don't Save", "Do not Save", "Dont Save"]);
        await WaitForText("User did not save their work");

        // Close/Cancel -> "Cancel" -> expect "User cancelled the dialog"
        await OpenDialog();
        await ClickButtonMatching(["Cancel", "Close"]);
        await WaitForText("User cancelled the dialog");

        // After dialog is closed, background element should be available in the automation tree again
        var navAutoSuggestRestored = FindFirst("NavigationAutoSuggestBox");
        navAutoSuggestRestored.Should().NotBeNull("because background UI automation should be restored after dialog is closed");
    }

    private Task OpenDialog()
    {
        // ensure dialog opened by clicking Show
        var showButton = FindFirst(c => c.ByText("Show"));
        if (showButton == null)
        {
            // If Show button is not found, assume dialog is already open; give UI a moment to stabilize.
            return Wait(1, TestContext.Current.CancellationToken);
        }

        showButton.AsButton().Click();
        return Wait(1, TestContext.Current.CancellationToken);
    }

    /// <summary>
    /// Polls the UI until the specified text appears or a retry limit is reached.
    /// Useful for waiting for view-model-driven UI updates after dialog interactions.
    /// </summary>
    /// <param name="text">The text to wait for.</param>
    /// <param name="retries">Number of polling attempts.</param>
    /// <param name="delaySeconds">Delay in seconds between attempts (uses test cancellation token).</param>
    /// <returns>A task that completes when the text is found or throws an assertion if not found.</returns>
    private async Task WaitForText(string text, int retries = 10, int delaySeconds = 1)
    {
        for (var i = 0; i < retries; i++)
        {
            if (FindFirst(c => c.ByText(text)) != null)
            {
                return;
            }

            await Wait(delaySeconds, TestContext.Current.CancellationToken);
        }

        // Final assertion to fail test with clear message if text never appeared
        FindFirst(c => c.ByText(text)).Should().NotBeNull($"Expected text '{text}' to appear within timeout");
    }

    /// <summary>
    /// Finds and clicks a button matching one of the provided candidate texts.
    /// If no direct match is found, falls back to clicking the last available button in the window.
    /// </summary>
    /// <param name="candidates">Array of acceptable button texts (first match is used).</param>
    /// <returns>A task that waits a short time after clicking to allow UI to settle.</returns>
    private Task ClickButtonMatching(string[] candidates)
    {
        AutomationElement? btn = null;

        foreach (var txt in candidates)
        {
            btn = FindFirst(c => c.ByText(txt));
            if (btn != null)
            {
                break;
            }
        }

        if (btn == null)
        {
            var buttons = MainWindow?.FindAllDescendants(cf => cf.ByControlType(ControlType.Button));
            if (buttons is { Length: > 0 })
            {
                btn = buttons.Last();
            }
        }

        btn.Should().NotBeNull($"expected one of: {string.Join(',', candidates)}");
        btn.AsButton().Click();

        return Wait(1, TestContext.Current.CancellationToken);
    }
}

#pragma warning restore IDE0008 // Use explicit type instead of 'var'
#pragma warning restore SA1512  // Single-line comments should not be followed by blank line
