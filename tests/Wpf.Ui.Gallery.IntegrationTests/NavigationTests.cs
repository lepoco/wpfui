// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using FlaUI.Core.Definitions;
using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;

namespace Wpf.Ui.Gallery.IntegrationTests;

public sealed class NavigationTests : UiTest
{
    [Theory]
    [InlineData(120)]
    [InlineData(-80)]
    public async Task ResizedPane_ShouldRestoreSearchWidth_AfterToggling(int widthChange)
    {
        await Wait(1);
        MainWindow!.FocusNative();
        await Wait(1);
        AutomationElement search = FindFirst("NavigationAutoSuggestBox")!;
        AutomationElement toggle = FindFirst("NavigationToggleButton")!;
        AutomationElement splitter = FindFirst(c => c.ByClassName("GridSplitter"))!;
        search.Should().NotBeNull();
        toggle.Should().NotBeNull();
        splitter.Should().NotBeNull();
        splitter.IsEnabled.Should().BeTrue();

        // Activate the window and focus the splitter before starting a drag.
        splitter.Click();
        await Task.Delay(100, TestContext.Current.CancellationToken);

        double originalWidth = search.BoundingRectangle.Width;
        var originalSplitterBounds = splitter.BoundingRectangle;
        var startingPoint = splitter.GetClickablePoint();
        Mouse.MoveTo(startingPoint);
        Mouse.Down();
        try
        {
            await Task.Delay(100, TestContext.Current.CancellationToken);
            Mouse.MoveTo(startingPoint.X + widthChange, startingPoint.Y);
        }
        finally
        {
            Mouse.Up();
        }
        await Wait(1);
        double resizedWidth = search.BoundingRectangle.Width;
        resizedWidth
            .Should()
            .BeApproximately(
                originalWidth + widthChange,
                2,
                "the splitter was dragged from {0} by {1}px (bounds before: {2}, after: {3})",
                startingPoint,
                widthChange,
                originalSplitterBounds,
                splitter.BoundingRectangle
            );

        toggle.AsButton().Invoke();
        await Wait(1);
        FindFirst("NavigationAutoSuggestBoxButton")!.IsOffscreen.Should().BeFalse();
        toggle.AsButton().Invoke();
        await Wait(1);
        ((double)search.BoundingRectangle.Width).Should().BeApproximately(resizedWidth, 2);
        search.IsOffscreen.Should().BeFalse();
    }

    [Fact]
    public async Task Settings_ShouldBeAvailable_ThroughAutoSuggestBox()
    {
        AutomationElement? autoSuggestBox = FindFirst("NavigationAutoSuggestBox");

        autoSuggestBox
            .Should()
            .NotBeNull("because NavigationAutoSuggestBox should be present in the main window");

        autoSuggestBox.As<AutoSuggestBox>().Enter("Settings");

        await Wait(1);

        FindFirst(c => c.ByText("About"))
            .Should()
            .NotBeNull("because Settings page should be displayed after clicking the Settings button");
    }

    [Fact]
    public async Task Settings_ShouldBeAvailable_ThroughNavigation()
    {
        AutomationElement? settingsButton = FindFirst("NavigationFooterItems")
            ?.FindFirstDescendant(c => c.ByText("Settings"));

        settingsButton.Should().NotBeNull("because NavigationView should be present in the main window");
        settingsButton.Click();

        await Wait(1);

        FindFirst(c => c.ByText("About"))
            .Should()
            .NotBeNull("because Settings page should be displayed after clicking the Settings button");
    }
}
