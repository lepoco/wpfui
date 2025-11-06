// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Gallery.IntegrationTests;

public sealed class WindowTests() : UiTest
{
    [Fact]
    public void WindowTitle_ShouldMatchPredefinedOne()
    {
        string? title = MainWindow?.Title;

        title.Should().Be("WPF UI Gallery", "because the main window title should match the predefined one.");
    }
}
