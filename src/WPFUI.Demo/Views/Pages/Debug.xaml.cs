// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Windows;
using WPFUI.Common;

namespace WPFUI.Demo.Views.Pages;

public class Hardware
{
    public string Name { get; set; } = String.Empty;
    public double Value { get; set; } = 0d;
    public double Min { get; set; } = 0d;
    public double Max { get; set; } = 0d;

    public IEnumerable<Hardware> SubItems { get; set; } = new Hardware[] { };
}

public class DebugPageData : ViewData
{
    private IEnumerable<Hardware> _hardwareCollection = new Hardware[] { };

    public IEnumerable<Hardware> HardwareCollection
    {
        get => _hardwareCollection;
        set => UpdateProperty(ref _hardwareCollection, value, nameof(HardwareCollection));
    }
}

/// <summary>
/// Interaction logic for Debug.xaml
/// </summary>
public partial class Debug
{
    protected DebugPageData _data;

    public Debug()
    {
        InitializeComponent();
        InitializeData();
    }

    private void InitializeData()
    {
        _data = new DebugPageData();
        DataContext = _data;

        var hardwareCollection = new List<Hardware>();

        hardwareCollection.Add(new Hardware()
        {
            Name = "CPU",
            Value = 89,
            Min = 29,
            Max = 92
        });


        hardwareCollection.Add(new Hardware()
        {
            Name = "GPU",
            Value = 59,
            Min = 22,
            Max = 78
        });

        _data.HardwareCollection = hardwareCollection;
    }

    private void FocusSwitch_Checked(object sender, RoutedEventArgs e)
    {
        if (Window.GetWindow(this) is Container window)
        {
            window.DebuggingLayer.IsFocusIndicatorEnabled = FocusSwitch.IsChecked is true;
        }
    }
}
