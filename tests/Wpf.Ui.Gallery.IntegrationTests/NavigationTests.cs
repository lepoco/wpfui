// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using FlaUI.Core.Definitions;
using FlaUI.Core.WindowsAPI;

namespace Wpf.Ui.Gallery.IntegrationTests;

public sealed class NavigationTests : UiTest
{
    [Fact]
    public async Task Settings_ShouldBeAvailable_ThroughAutoSuggestBox()
    {
        AutomationElement? autoSuggestBox = FindFirst("NavigationAutoSuggestBox");

        autoSuggestBox
            .Should()
            .NotBeNull("because NavigationAutoSuggestBox should be present in the main window.");

        autoSuggestBox.As<AutoSuggestBox>().Enter("Settings");

        await Wait(5);

        AutomationElement? aboutHeader = FindFirst(c => c.ByText("About"));

        aboutHeader
            .Should()
            .NotBeNull("because Settings page should be displayed after clicking the Settings button.");
    }

    [Fact]
    public async Task Settings_ShouldBeAvailable_ThroughNavigation()
    {
        AutomationElement? settingsButton = FindFirst("NavigationFooterItems")
            ?.FindFirstDescendant(c => c.ByText("Settings"));

        settingsButton.Should().NotBeNull("because NavigationView should be present in the main window.");
        settingsButton.Click();

        await Wait(5);

        AutomationElement? aboutHeader = FindFirst(c => c.ByText("About"));

        aboutHeader
            .Should()
            .NotBeNull("because Settings page should be displayed after clicking the Settings button.");
    }
}
