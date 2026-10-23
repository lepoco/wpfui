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

        await SelectMode(1, "LeftMinimal");
        ClickMiddleSpace();
        await AssertPaneOpen(false);
        await SelectMode(3, "Top");
        FindFirst("EmptySpaceContent")!.Click();
        await AssertPaneOpen(false);
        await SelectMode(0, "Left");
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

    [Theory]
    [InlineData(3, "Top", true)]
    [InlineData(3, "Top", false)]
    [InlineData(4, "Bottom", true)]
    [InlineData(4, "Bottom", false)]
    public async Task DisplayModeSelection_WithMouse_DoesNotCrash(int index, string name, bool isPaneOpen)
    {
        await Wait(2, TestContext.Current.CancellationToken);
        FindFirst(c => c.ByText("Navigation"))!.Click();
        await Wait(1, TestContext.Current.CancellationToken);
        FindFirst(c => c.ByText("NavigationView"))!.Click();
        await Wait(1, TestContext.Current.CancellationToken);
        await ScrollSampleIntoView();
        FindFirst("EmptySpaceToggleEnabled")!.AsCheckBox().IsChecked = true;
        await SelectMode(1, "LeftMinimal");
        if (!isPaneOpen)
        {
            ClickMiddleSpace();
        }

        await AssertPaneOpen(isPaneOpen);
        await SelectMode(index, name);
        await AssertPaneOpen(isPaneOpen);
        await SelectMode(0, "Left");
        await AssertPaneOpen(isPaneOpen);
        ClickMiddleSpace();
        await AssertPaneOpen(!isPaneOpen);
    }

    [Theory]
    [InlineData(0, "Left")]
    [InlineData(1, "LeftMinimal")]
    public async Task LeavingLeftFluent_RestoresWorkingHamburger(int index, string name)
    {
        await Wait(2, TestContext.Current.CancellationToken);
        FindFirst(c => c.ByText("Navigation"))!.Click();
        await Wait(1, TestContext.Current.CancellationToken);
        FindFirst(c => c.ByText("NavigationView"))!.Click();
        await Wait(1, TestContext.Current.CancellationToken);
        await ScrollSampleIntoView();
        await SelectMode(2, "LeftFluent");
        await SelectMode(index, name);

        // Keep the empty-space option off: only a real, visible hamburger can toggle here.
        Rectangle mode = FindFirst("EmptySpacePaneMode")!.BoundingRectangle;
        Rectangle item = FindFirst("EmptySpaceDashboard")!.BoundingRectangle;
        AutomationElement[] buttons = MainWindow!
            .FindAllDescendants(c => c.ByAutomationId("NavigationToggleButton"))
            .Where(button =>
                !button.IsOffscreen
                && button.BoundingRectangle.Top > mode.Bottom
                && button.BoundingRectangle.Bottom < item.Top
            )
            .ToArray();
        buttons.Should().ContainSingle();
        buttons[0].Click();
        await AssertPaneOpen(false);
        buttons[0].Click();
        await AssertPaneOpen(true);
    }

    private async Task SelectMode(int index, string name)
    {
        ComboBox mode = FindFirst("EmptySpacePaneMode")!.AsComboBox();
        mode.Expand();
        await Task.Delay(200, TestContext.Current.CancellationToken);
        mode.Items[index].Click();
        await Wait(1, TestContext.Current.CancellationToken);
        FindFirst("EmptySpaceModeState")!.Name.Should().Be($"PaneDisplayMode: {name}");
    }

    private async Task ScrollSampleIntoView()
    {
        Rectangle window = MainWindow!.BoundingRectangle;
        Mouse.MoveTo(new Point(window.Right - 100, window.Bottom - 100));
        // The optional example follows all four built-in examples at the end of the page.
        for (
            int i = 0;
            i < 64
                && (
                    FindFirst("EmptySpaceModeState")!.IsOffscreen
                    || FindFirst("EmptySpaceModeState")!.BoundingRectangle.Bottom > window.Bottom - 30
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
