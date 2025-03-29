// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Animations;

internal static class AnimationProperties
{
    public static readonly DependencyProperty AnimationTagValueProperty = DependencyProperty.RegisterAttached(
        "AnimationTagValue",
        typeof(double),
        typeof(AnimationProperties),
        new FrameworkPropertyMetadata(
            0.0,
            FrameworkPropertyMetadataOptions.Inherits));

    public static double GetAnimationTagValue(DependencyObject dp)
    {
        return (double)dp.GetValue(AnimationTagValueProperty);
    }

    public static void SetAnimationTagValue(DependencyObject dp, double value)
    {
        dp.SetValue(AnimationTagValueProperty, value);
    }
}