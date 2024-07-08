// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;

namespace Wpf.Ui.ToastNotifications.Notification;

/// <summary>
/// Interaction logic for ToastNotification.xaml
/// </summary>
public partial class ToastNotification : UserControl
{
    public ToastNotification()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Identifies the <see cref="Toast"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ToastProperty = DependencyProperty.Register(
        nameof(Toast), typeof(string), typeof(ToastNotification), new PropertyMetadata(default(string)));

    /// <summary>
    /// Gets or sets the toast message displayed by the notification control.
    /// </summary>
    /// <value>
    /// The message to be displayed as a toast notification.
    /// </value>
    public string? Toast
    {
        get => (string?)GetValue(ToastProperty);
        set => SetValue(ToastProperty, value);
    }
}
