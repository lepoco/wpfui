// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Diagnostics;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Button that opens a URL in a web browser.
/// </summary>
public class HyperlinkButton : Wpf.Ui.Controls.Button
{
    /// <summary>Identifies the <see cref="NavigateUri"/> dependency property.</summary>
    public static readonly DependencyProperty NavigateUriProperty = DependencyProperty.Register(
        nameof(NavigateUri),
        typeof(string),
        typeof(HyperlinkButton),
        new PropertyMetadata(string.Empty)
    );

    /// <summary>
    /// Gets or sets the URL (or application shortcut) to open.
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
            Debug.WriteLine(
                $"INFO | HyperlinkButton clicked, with href: {NavigateUri}",
                "Wpf.Ui.HyperlinkButton"
            );

            ProcessStartInfo sInfo = new(new Uri(NavigateUri).AbsoluteUri) { UseShellExecute = true };

            _ = Process.Start(sInfo);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
        }
    }
}
