// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using Wpf.Ui.Common;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace Wpf.Ui.Demo.Views.Pages;

/// <summary>
/// Interaction logic for Controls.xaml
/// </summary>
public partial class Controls
{
    private readonly ISnackbarService _snackbarService;

    private readonly IDialogControl _dialogControl;

    public Controls(ISnackbarService snackbarService, IDialogService dialogService)
    {
        InitializeComponent();

        _snackbarService = snackbarService;
        _dialogControl = dialogService.GetDialogControl();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        RootPanel.ScrollOwner = ScrollHost;

        _dialogControl.ButtonRightClick += DialogControlOnButtonRightClick;
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        _dialogControl.ButtonRightClick -= DialogControlOnButtonRightClick;
    }

    private void ButtonAction_OnClick(object sender, RoutedEventArgs e)
    {
        if (sender is not Wpf.Ui.Controls.CardAction cardAction)
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

    private async void OpenDialog()
    {
        var result = await _dialogControl.ShowAndWaitAsync(
            "WPF UI Dialog",
            "What is it like to be a scribe? Is it good? In my opinion it's not about being good or not good. If I were to say what I esteem the most in life, I would say - people. People, who gave me a helping hand when I was a mess, when I was alone. And what's interesting, the chance meetings are the ones that influence our lives. The point is that when you profess certain values, even those seemingly universal, you may not find any understanding which, let me say, which helps us to develop. I had luck, let me say, because I found it. And I'd like to thank life. I'd like to thank it - life is singing, life is dancing, life is love. Many people ask me the same question, but how do you do that? where does all your happiness come from? And i replay that it's easy, it's cherishing live, that's what makes me build machines today, and tomorrow... who knows, why not, i would dedicate myself to do some community working and i would be, wham, not least... planting .... i mean... carrots.");
    }

    private static void DialogControlOnButtonRightClick(object sender, RoutedEventArgs e)
    {
        var dialogControl = (IDialogControl)sender;
        dialogControl.Hide();
    }

    private void OpenSnackbar()
    {
        _snackbarService.Show("The cake is a lie!", "The cake is a lie...", SymbolRegular.FoodCake24, ControlAppearance.Primary);
    }

    private void OpenMessageBox()
    {
        var messageBox = new Wpf.Ui.Controls.MessageBox();

        messageBox.ButtonLeftName = "Hello World";
        messageBox.ButtonRightName = "Just close me";

        messageBox.ButtonLeftClick += MessageBox_LeftButtonClick;
        messageBox.ButtonRightClick += MessageBox_RightButtonClick;

        messageBox.Show("Something weird", "May happen");
    }

    private void MessageBox_LeftButtonClick(object sender, System.Windows.RoutedEventArgs e)
    {
        (sender as Wpf.Ui.Controls.MessageBox)?.Close();
    }

    private void MessageBox_RightButtonClick(object sender, System.Windows.RoutedEventArgs e)
    {
        (sender as Wpf.Ui.Controls.MessageBox)?.Close();
    }
}
