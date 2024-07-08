// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;

namespace Wpf.Ui.ToastNotifications;

/// <summary>
/// Interface for the notification service.
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// Displays a notification message.
    /// </summary>
    /// <param name="message">The message content.</param>
    /// <param name="host">The name of the control host.</param>
    void Show(string message, ContentControl host);
}
