// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Controls.Primitives;

// https://docs.microsoft.com/en-us/fluent-ui/web-components/components/anchor

namespace Wpf.Ui.Controls;

/// <summary>
/// Creates a hyperlink to web pages, files, email addresses, locations in the same page, or anything else a URL can address.
/// </summary>
[ToolboxItem(true)]
[ToolboxBitmap(typeof(Anchor), "Anchor.bmp")]
public class Anchor : Wpf.Ui.Controls.Button
{
    /// <summary>
    /// Property for <see cref="NavigateUri"/>.
    /// </summary>
    public static readonly DependencyProperty NavigateUriProperty =
        DependencyProperty.Register(nameof(NavigateUri), typeof(string), typeof(Anchor),
            new PropertyMetadata(String.Empty));

    /// <summary>
    /// Gets or sets the URL that the hyperlink points to.
    /// </summary>
    public string NavigateUri
    {
        get => (string)GetValue(NavigateUriProperty);
        set => SetValue(NavigateUriProperty, value);
    }

    /// <summary>
    /// This virtual method is called when button is clicked and it raises the Click event
    /// </summary>
    protected override void OnClick()
    {
        RoutedEventArgs newEvent = new RoutedEventArgs(ButtonBase.ClickEvent, this);
        RaiseEvent(newEvent);

#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO | Anchor clicked, with href: {NavigateUri}", "Wpf.Ui.Anchor");
#endif
        if (String.IsNullOrEmpty(NavigateUri))
            return;
        System.Diagnostics.ProcessStartInfo sInfo = new(new Uri(NavigateUri).AbsoluteUri)
        {
            UseShellExecute = true
        };

        System.Diagnostics.Process.Start(sInfo);
    }
}
