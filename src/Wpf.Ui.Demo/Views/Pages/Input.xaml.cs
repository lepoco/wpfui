// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Controls;

using Wpf.Ui.Common;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Demo.ViewModels;

namespace Wpf.Ui.Demo.Views.Pages;

/// <summary>
/// Interaction logic for Input.xaml
/// </summary>
public partial class Input : INavigableView<InputViewModel>
{
    public InputViewModel ViewModel
    {
        get;
    }

    public Input(InputViewModel viewModel)
    {
        ViewModel = viewModel;
        Loaded += OnLoaded;

        InitializeComponent();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        RootPanel.ScrollOwner = ScrollHost;
    }

    #region Color picker changes event handlers
    private void ColorSpectrumComponentsRadioButtonGroupChanged(object sender, System.Windows.RoutedEventArgs args)
    {
        var radioButton = (FrameworkElement)sender;

        if (radioButton.Tag is string components)
        {
            ColorPicker.ColorSpectrumComponents = Enum.Parse<ColorSpectrumComponents>(components);
        }

    }

    private void ColorSpectrumShapeRadioButtonGroupChanged(object sender, System.Windows.RoutedEventArgs args)
    {
        var radioButton = (FrameworkElement)sender;

        if (radioButton.Tag is string shape)
        {
            ColorPicker.ColorSpectrumShape = Enum.Parse<ColorSpectrumShape>(shape);
        }

    }

    private void OrientationRadioButtonGroupChanged(object sender, System.Windows.RoutedEventArgs args)
    {
        var radioButton = (FrameworkElement)sender;

        if (radioButton.Tag is string orientation)
        {
            ColorPicker.Orientation = Enum.Parse<Orientation>(orientation);
        }
    } 
    #endregion
}
