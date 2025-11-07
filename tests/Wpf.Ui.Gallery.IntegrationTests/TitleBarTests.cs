// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Gallery.IntegrationTests;

public sealed class TitleBarTests : UiTest
{
    [Fact]
    public async Task CloseButton_ShouldCloseWindow_WhenClicked()
    {
        Button? closeButton = FindFirst("TitleBarCloseButton").AsButton();

        closeButton.Should().NotBeNull("because CloseButton should be present in the main window title bar.");
        closeButton.Click(moveMouse: false);

        await Wait(5);

        Application
            ?.HasExited.Should()
            .BeTrue("because the main window should be closed after clicking the close button.");
    }
}
