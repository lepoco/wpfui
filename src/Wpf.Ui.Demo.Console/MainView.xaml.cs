// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Wpf.Ui.Demo.Console;
public partial class MainView
{
    public MainView()
    {
        InitializeComponent();

        Appearance.SystemThemeWatcher.Watch(this);

        //UiApplication.Current.MainWindow = this;
        this.ApplyTheme();

        //new Wpf.Ui.Controls.Button().Appearance = Controls.ControlAppearance.Primary;

        //SystemAccentColorPrimary

        //Wpf.Ui.Appearance.Accent.ApplySystemAccent();
        //Wpf.Ui.Appearance.Theme.Apply(this);
    }
}