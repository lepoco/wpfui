// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Wpf.Ui.Controls;

/// <summary>
/// Button that opens a URL in a web browser.
/// </summary>
public class Hyperlink : Wpf.Ui.Controls.Button
{
    /// <summary>
    /// Property for <see cref="NavigateUri"/>.
    /// </summary>
    public static readonly DependencyProperty NavigateUriProperty = DependencyProperty.Register(nameof(NavigateUri),
        typeof(string), typeof(Hyperlink), new PropertyMetadata(string.Empty));

    /// <summary>
    /// The URL (or application shortcut) to open.
    /// </summary>
    public string NavigateUri
    {
        get => GetValue(NavigateUriProperty) as string ?? string.Empty;
        set => SetValue(NavigateUriProperty, value);
    }

    protected override void OnClick()
    {
        base.OnClick();
        if (string.IsNullOrEmpty(NavigateUri))
        {
            return;
        }

        try
        {
            Debug.WriteLine($"INFO | Hyperlink clicked, with href: {NavigateUri}", "Wpf.Ui.Hyperlink");

            ProcessStartInfo sInfo = new(new Uri(NavigateUri).AbsoluteUri)
            {
                UseShellExecute = true
            };

            Process.Start(sInfo);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
        }
    }
}
