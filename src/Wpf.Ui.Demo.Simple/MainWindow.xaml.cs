// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.ComponentModel;

using Wpf.Ui.Demo.Simple.Views.Pages;

namespace Wpf.Ui.Demo.Simple;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : INotifyPropertyChanged
{
    private double _value;

    public double Value
    {
        get => _value;

        set
        {
            _value = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
        }
    }

    public MainWindow()
    {
        DataContext = this;

        Appearance.SystemThemeWatcher.Watch(this);

        InitializeComponent();

        Loaded += (_, _) => RootNavigation.Navigate(typeof(DashboardPage));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}
