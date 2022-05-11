// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;

namespace WPFUI.Controls;

public class UiWindow : System.Windows.Window
{
    public UiWindow()
    {
        SetResourceReference(StyleProperty, typeof(UiWindow));
    }

    static UiWindow()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(UiWindow), new FrameworkPropertyMetadata(typeof(UiWindow)));
    }
}

