// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Automation;
using WindowVisualState = FlaUI.Core.Definitions.WindowVisualState;

namespace Wpf.Ui.Gallery.IntegrationTests;

public sealed class TitleBarTests : UiTest
{
    [Fact]
    public async Task CloseButton_ShouldCloseWindow_WhenClicked()
    {
        Button? closeButton = FindFirst("TitleBarCloseButton").AsButton();

        closeButton.Should().NotBeNull("because CloseButton should be present in the main window title bar");
        closeButton.Click(moveMouse: false);

        await Wait(2);

        Application
            ?.HasExited.Should()
            .BeTrue("because the main window should be closed after clicking the close button");
    }

    [Fact]
    public async Task MinimizeButton_ShouldHideWindow_WhenClicked()
    {
        Button? minimizeButton = FindFirst("TitleBarMinimizeButton").AsButton();

        minimizeButton
            .Should()
            .NotBeNull("because MinimizeButton should be present in the main window title bar");
        minimizeButton.Click(moveMouse: false);

        await Wait(2);

        MainWindow
            .Patterns.Window.Pattern.WindowVisualState.ValueOrDefault.Should()
            .Be(
                WindowVisualState.Minimized,
                "because the main window should be minimized after clicking the minimize button"
            );
    }

    [Fact]
    public async Task MaximizeButton_ShouldExpandWindow_WhenClicked()
    {
        Button? maximizeButton = FindFirst("TitleBarMaximizeButton").AsButton();

        maximizeButton
            .Should()
            .NotBeNull("because MaximizeButton should be present in the main window title bar");
        maximizeButton.Click(moveMouse: false);

        await Wait(2);

        MainWindow
            .Patterns.Window.Pattern.WindowVisualState.ValueOrDefault.Should()
            .Be(
                WindowVisualState.Maximized,
                "because the main window should be maximized after clicking the maximize button"
            );
    }
}
