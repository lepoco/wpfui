// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Wpf.Ui.ToastNotifications.Notification;

namespace Wpf.Ui.ToastNotifications.Services;

/// <summary>
/// Toast notification service
/// </summary>
internal class ToastNotificationService : INotificationService
{
    public void Show(string message, ContentControl host)
    {
        ClearCurrentToast(host);
        ToastNotification toast = CreateToast(message);
        Storyboard animation = CreateToastAnimation(toast);

        SubscribeToAnimationCompletion(animation, toast, host);

        StartToastAnimation(host, toast, animation);
    }

    /// <summary>
    /// Subscribes to the animation completion event to release resources
    /// </summary>
    /// <param name="animation">The animation</param>
    /// <param name="control">The control</param>
    /// <param name="host">The host control</param>
    private static void SubscribeToAnimationCompletion(Storyboard animation, FrameworkElement control, ContentControl host)
    {
        animation.Completed += (s2, e2) =>
        {
            if (host.Content == control && host.IsLoaded)
            {
                host.SetCurrentValue(ContentControl.ContentProperty, null);
            }
        };
    }

    /// <summary>
    /// Starts the notification animation
    /// </summary>
    /// <param name="host">The host control</param>
    /// <param name="control">The control</param>
    /// <param name="animation">The animation</param>
    private static void StartToastAnimation(ContentControl host, FrameworkElement control, Storyboard animation)
    {
        if (host.IsLoaded)
        {
            host.SetCurrentValue(ContentControl.ContentProperty, control);
            control.BeginStoryboard(animation);
        }
    }

    /// <summary>
    /// Clears the current Toast notification
    /// </summary>
    /// <param name="host">The control host</param>
    private static void ClearCurrentToast(ContentControl host)
    {
        host.Dispatcher.Invoke(() =>
        {
            if (host.Content is ToastNotification currentToast)
            {
                if (currentToast.Resources.Contains("ToastAnimation"))
                {
                    if (currentToast.FindResource("ToastAnimation") is Storyboard currentAnimation)
                    {
                        currentAnimation.Stop();
                    }
                }

                host.SetCurrentValue(ContentControl.ContentProperty, null);
            }
        });
    }

    /// <summary>
    /// Creates a new notification control and sets its message
    /// </summary>
    /// <param name="message">The message content</param>
    /// <returns>The configured notification control</returns>
    private static ToastNotification CreateToast(string message)
    {
        return new ToastNotification
        {
            Toast = message
        };
    }

    /// <summary>
    /// Creates and configures the Toast notification animation
    /// </summary>
    /// <param name="toast">The Toast notification</param>
    /// <returns>The configured animation object</returns>
    private static Storyboard CreateToastAnimation(ToastNotification toast)
    {
        Storyboard animation = new();
        DoubleAnimationUsingKeyFrames fadein = new()
        {
            Duration = TimeSpan.FromSeconds(3)
        };
        _ = fadein.KeyFrames.Add(new SplineDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0))));
        _ = fadein.KeyFrames.Add(new SplineDoubleKeyFrame(1, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.5))));
        _ = fadein.KeyFrames.Add(new SplineDoubleKeyFrame(1, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(2.5))));
        _ = fadein.KeyFrames.Add(new SplineDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(3))));

        Storyboard.SetTarget(fadein, toast);
        Storyboard.SetTargetProperty(fadein, new PropertyPath("Opacity"));
        animation.Children.Add(fadein);

        return animation;
    }
}
