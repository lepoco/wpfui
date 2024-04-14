// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Animations;

namespace Wpf.Ui.UnitTests.Animations;

public class TransitionAnimationProviderTests
{
    [Fact]
    public void ApplyTransition_ReturnsFalse_WhenDurationIsLessThan10()
    {
        UIElement mockedUiElement = Substitute.For<UIElement>();

        var result = TransitionAnimationProvider.ApplyTransition(mockedUiElement, Transition.FadeIn, -10);

        Assert.False(result);
    }

    [Fact]
    public void ApplyTransition_ReturnsFalse_WhenElementIsNull()
    {
        var result = TransitionAnimationProvider.ApplyTransition(null, Transition.FadeIn, 1000);

        Assert.False(result);
    }
}
