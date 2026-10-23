// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Drawing;
using FlaUI.Core.Input;

namespace Wpf.Ui.Gallery.IntegrationTests;

public sealed class NavigationViewPaneInputTests : UiTest
{
    [Fact]
    public async Task EmptySpaceClicks_RespectControlsAndDisplayMode()
    {
        await Wait(2, TestContext.Current.CancellationToken);
        FindFirst(c => c.ByText("Navigation"))!.Click();
        await Wait(1, TestContext.Current.CancellationToken);
        FindFirst(c => c.ByText("NavigationView"))!.Click();
        await Wait(1, TestContext.Current.CancellationToken);
        await ScrollSampleIntoView();

        await AssertPaneOpen(true);
        ClickMiddleSpace();
        await AssertPaneOpen(true); // Opt-in defaults to off.
        FindFirst("EmptySpaceToggleEnabled")!.AsCheckBox().IsChecked = true;

        // A: beside the hamburger, derived from current item/header bounds.
        Rectangle item = FindFirst("EmptySpaceDashboard")!.BoundingRectangle;
        Rectangle header = FindFirst("EmptySpaceContent")!.BoundingRectangle;
        Mouse.Click(new Point(item.Right - 20, header.Top + (header.Height / 2)));
        await AssertPaneOpen(false);
        ClickMiddleSpace(); // C: compact icon column.
        await AssertPaneOpen(true);
        ClickMiddleSpace(); // B: expanded menu/footer gap.
        await AssertPaneOpen(false);
        ClickMiddleSpace();
        await AssertPaneOpen(true);

        foreach (
            string id in new[]
            {
                "EmptySpaceDashboard",
                "EmptySpaceSettings",
                "EmptySpaceFooter",
                "EmptySpaceContent",
            }
        )
        {
            FindFirst(id)!.Click();
            await AssertPaneOpen(true);
        }

        Point middle = MiddleSpace();
        Mouse.MoveTo(middle);
        Mouse.Scroll(1);
        await AssertPaneOpen(true);
        Mouse.Drag(middle, new Point(middle.X, middle.Y + 30));
        await AssertPaneOpen(true);

        ComboBox mode = FindFirst("EmptySpacePaneMode")!.AsComboBox();
        mode.Select(1); // LeftMinimal
        mode.Collapse();
        await Wait(1, TestContext.Current.CancellationToken);
        mode.SelectedItem!.Text.Should().Be("LeftMinimal");
        ClickMiddleSpace();
        await AssertPaneOpen(false);
        mode.Select(3); // Top
        mode.Collapse();
        await Wait(1, TestContext.Current.CancellationToken);
        FindFirst("EmptySpaceContent")!.Click();
        await AssertPaneOpen(false);
        mode.Select(0); // Left, template reapplied
        mode.Collapse();
        await Wait(1, TestContext.Current.CancellationToken);
        ClickMiddleSpace();
        await AssertPaneOpen(true);
        FindFirst("EmptySpaceToggleEnabled")!.AsCheckBox().IsChecked = false;
        ClickMiddleSpace();
        await AssertPaneOpen(true);
    }

    private Point MiddleSpace()
    {
        Rectangle item = FindFirst("EmptySpaceItems")!.BoundingRectangle;
        Rectangle settings = FindFirst("EmptySpaceSettings")!.BoundingRectangle;
        // Use the icon column: LeftMinimal can give its item content a wider layout slot
        // than the currently visible compact pane.
        return new Point(item.Left + 20, item.Bottom + ((settings.Top - item.Bottom) / 2));
    }

    private void ClickMiddleSpace() => Mouse.Click(MiddleSpace());

    private async Task ScrollSampleIntoView()
    {
        Rectangle window = MainWindow!.BoundingRectangle;
        Mouse.MoveTo(new Point(window.Right - 100, window.Bottom - 100));
        for (
            int i = 0;
            i < 16
                && (
                    FindFirst("EmptySpaceState")!.IsOffscreen
                    || FindFirst("EmptySpaceState")!.BoundingRectangle.Bottom > window.Bottom - 30
                );
            i++
        )
        {
            Mouse.Scroll(-1);
            await Task.Delay(100, TestContext.Current.CancellationToken);
        }
        FindFirst("EmptySpaceToggleEnabled")!.IsOffscreen.Should().BeFalse();
        FindFirst("EmptySpaceState")!.IsOffscreen.Should().BeFalse();
    }

    private async Task AssertPaneOpen(
        bool expected,
        [System.Runtime.CompilerServices.CallerLineNumber] int line = 0
    )
    {
        await Wait(1, TestContext.Current.CancellationToken);
        FindFirst("EmptySpaceState")!
            .Name.Should()
            .Be(
                $"IsPaneOpen: {expected}",
                $"the action at line {line} should preserve or toggle the pane as specified"
            );
    }
}
