// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Media.Animation;
using Wpf.Ui.Hardware;

namespace Wpf.Ui.Animations;

/// <summary>
/// Provides tools for <see cref="FrameworkElement"/> animation.
/// </summary>
/// <example>
/// <code lang="csharp">
/// TransitionAnimationProvider.ApplyTransition(MyFrameworkElement, Transition.FadeIn, 500);
/// </code>
/// </example>
public static class TransitionAnimationProvider
{
    private const double DecelerationRatio = 0.7D;

    /// <summary>
    /// Attempts to apply an animation effect while adding content to the frame.
    /// </summary>
    /// <param name="element">Currently rendered element.</param>
    /// <param name="type">Selected transition type.</param>
    /// <param name="duration">Transition duration.</param>
    /// <returns>Returns <see langword="true"/> if the transition was applied. Otherwise <see langword="false"/>.</returns>
    public static bool ApplyTransition(object? element, Transition type, int duration)
    {
        if (type == Transition.None
            || !HardwareAcceleration.IsSupported(RenderingTier.PartialAcceleration)
            || element is not UIElement uiElement
            || duration < 10)
        {
            return false;
        }

        duration = duration > 10000 ? 10000 : duration;

        var timespanDuration = new Duration(TimeSpan.FromMilliseconds(duration));

        switch (type)
        {
            case Transition.FadeIn:
                FadeInTransition(uiElement, timespanDuration);
                break;

            case Transition.FadeInWithSlide:
                FadeInWithSlideTransition(uiElement, timespanDuration);
                break;

            case Transition.SlideBottom:
                SlideBottomTransition(uiElement, timespanDuration);
                break;

            case Transition.SlideRight:
                SlideRightTransition(uiElement, timespanDuration);
                break;

            case Transition.SlideLeft:
                SlideLeftTransition(uiElement, timespanDuration);
                break;

            default:
                return false;
        }

        return true;
    }

    private static void FadeInTransition(UIElement animatedUiElement, Duration duration)
    {
        var opacityDoubleAnimation = new DoubleAnimation
        {
            Duration = duration,
            DecelerationRatio = DecelerationRatio,
            From = 0.0,
            To = 1.0,
        };

        animatedUiElement.BeginAnimation(UIElement.OpacityProperty, opacityDoubleAnimation);
    }

    private static void FadeInWithSlideTransition(UIElement animatedUiElement, Duration duration)
    {
        var translateDoubleAnimation = new DoubleAnimation
        {
            Duration = duration,
            DecelerationRatio = DecelerationRatio,
            From = 30,
            To = 0,
        };

        if (animatedUiElement.RenderTransform is not TranslateTransform)
        {
            animatedUiElement.SetCurrentValue(
                UIElement.RenderTransformProperty,
                new TranslateTransform(0, 0)
            );
        }

        if (!animatedUiElement.RenderTransformOrigin.Equals(new Point(0.5, 0.5)))
        {
            animatedUiElement.SetCurrentValue(UIElement.RenderTransformOriginProperty, new Point(0.5, 0.5));
        }

        animatedUiElement.RenderTransform.BeginAnimation(
            TranslateTransform.YProperty,
            translateDoubleAnimation
        );

        var opacityDoubleAnimation = new DoubleAnimation
        {
            Duration = duration,
            DecelerationRatio = DecelerationRatio,
            From = 0.0,
            To = 1.0,
        };

        animatedUiElement.BeginAnimation(UIElement.OpacityProperty, opacityDoubleAnimation);
    }

    private static void SlideBottomTransition(UIElement animatedUiElement, Duration duration)
    {
        var translateDoubleAnimation = new DoubleAnimation
        {
            Duration = duration,
            DecelerationRatio = DecelerationRatio,
            From = 30,
            To = 0,
        };

        if (animatedUiElement.RenderTransform is not TranslateTransform)
        {
            animatedUiElement.SetCurrentValue(
                UIElement.RenderTransformProperty,
                new TranslateTransform(0, 0)
            );
        }

        if (!animatedUiElement.RenderTransformOrigin.Equals(new Point(0.5, 0.5)))
        {
            animatedUiElement.SetCurrentValue(UIElement.RenderTransformOriginProperty, new Point(0.5, 0.5));
        }

        animatedUiElement.RenderTransform.BeginAnimation(
            TranslateTransform.YProperty,
            translateDoubleAnimation
        );
    }

    private static void SlideRightTransition(UIElement animatedUiElement, Duration duration)
    {
        var translateDoubleAnimation = new DoubleAnimation
        {
            Duration = duration,
            DecelerationRatio = DecelerationRatio,
            From = 50,
            To = 0,
        };

        if (animatedUiElement.RenderTransform is not TranslateTransform)
        {
            animatedUiElement.SetCurrentValue(
                UIElement.RenderTransformProperty,
                new TranslateTransform(0, 0)
            );
        }

        if (!animatedUiElement.RenderTransformOrigin.Equals(new Point(0.5, 0.5)))
        {
            animatedUiElement.SetCurrentValue(UIElement.RenderTransformOriginProperty, new Point(0.5, 0.5));
        }

        animatedUiElement.RenderTransform.BeginAnimation(
            TranslateTransform.XProperty,
            translateDoubleAnimation
        );
    }

    private static void SlideLeftTransition(UIElement animatedUiElement, Duration duration)
    {
        var translateDoubleAnimation = new DoubleAnimation
        {
            Duration = duration,
            DecelerationRatio = DecelerationRatio,
            From = -50,
            To = 0,
        };

        if (animatedUiElement.RenderTransform is not TranslateTransform)
        {
            animatedUiElement.SetCurrentValue(
                UIElement.RenderTransformProperty,
                new TranslateTransform(0, 0)
            );
        }

        if (!animatedUiElement.RenderTransformOrigin.Equals(new Point(0.5, 0.5)))
        {
            animatedUiElement.SetCurrentValue(UIElement.RenderTransformOriginProperty, new Point(0.5, 0.5));
        }

        animatedUiElement.RenderTransform.BeginAnimation(
            TranslateTransform.XProperty,
            translateDoubleAnimation
        );
    }
}
