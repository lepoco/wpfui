// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;

namespace WPFUI.Demo.Views.Pages;

/// <summary>
/// Interaction logic for Controls.xaml
/// </summary>
public partial class Controls
{
    public Controls()
    {
        InitializeComponent();

        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        RootPanel.ScrollOwner = ScrollHost;
    }

    private void ButtonAction_OnClick(object sender, RoutedEventArgs e)
    {
        if (sender is not WPFUI.Controls.CardAction cardAction)
            return;

        var tag = cardAction.Tag as string;

        if (String.IsNullOrWhiteSpace(tag))
            return;

        switch (tag)
        {
            case "dialog":
                OpenDialog();
                break;

            case "snackbar":
                OpenSnackbar();
                break;

            case "messagebox":
                OpenMessageBox();
                break;
        }
    }

    private void OpenDialog()
    {
        (Application.Current.MainWindow as Container)?.RootDialog.Show();
    }

    private void OpenSnackbar()
    {
        (Application.Current.MainWindow as Container)?.RootSnackbar.Show("The cake is a lie!", "The cake is a lie...");
    }

    private void OpenMessageBox()
    {
        var messageBox = new WPFUI.Controls.MessageBox();

        messageBox.ButtonLeftName = "Hello World";
        messageBox.ButtonRightName = "Just close me";

        messageBox.ButtonLeftClick += MessageBox_LeftButtonClick;
        messageBox.ButtonRightClick += MessageBox_RightButtonClick;

        messageBox.Show("Something weird", "May happen");
    }

    private void MessageBox_LeftButtonClick(object sender, System.Windows.RoutedEventArgs e)
    {
        (sender as WPFUI.Controls.MessageBox)?.Close();
    }

    private void MessageBox_RightButtonClick(object sender, System.Windows.RoutedEventArgs e)
    {
        (sender as WPFUI.Controls.MessageBox)?.Close();
    }
}
