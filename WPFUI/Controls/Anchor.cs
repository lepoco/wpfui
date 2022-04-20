// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Controls.Primitives;

// https://docs.microsoft.com/en-us/fluent-ui/web-components/components/anchor

namespace WPFUI.Controls;

/// <summary>
/// Creates a hyperlink to web pages, files, email addresses, locations in the same page, or anything else a URL can address.
/// </summary>
public class Anchor : WPFUI.Controls.Button
{
    /// <summary>
    /// Property for <see cref="Href"/>.
    /// </summary>
    public static readonly DependencyProperty HrefProperty =
        DependencyProperty.Register(nameof(Href), typeof(string), typeof(Anchor),
            new PropertyMetadata(String.Empty));

    /// <summary>
    /// Gets or sets the URL that the hyperlink points to.
    /// </summary>
    public string Href
    {
        get => (string)GetValue(HrefProperty);
        set => SetValue(HrefProperty, value);
    }

    /// <summary>
    /// This virtual method is called when button is clicked and it raises the Click event
    /// </summary>
    protected override void OnClick()
    {
        RoutedEventArgs newEvent = new RoutedEventArgs(ButtonBase.ClickEvent, this);
        RaiseEvent(newEvent);

#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO | Anchor clicked, with href: {Href}", "WPFUI.Anchor");
#endif
        if (String.IsNullOrEmpty(Href))
            return;
        System.Diagnostics.ProcessStartInfo sInfo = new(new Uri(Href).AbsoluteUri)
        {
            UseShellExecute = true
        };

        System.Diagnostics.Process.Start(sInfo);
    }
}
