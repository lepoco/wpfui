// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Controls;

namespace Wpf.Ui.Gallery.ViewModels.Pages.DialogsAndFlyouts;

public partial class SnackbarViewModel : ObservableObject
{
    public SnackbarViewModel(ISnackbarService snackbarService)
    {
        _snackbarService = snackbarService;
    }

    private readonly ISnackbarService _snackbarService;
    private ControlAppearance _snackbarAppearance = ControlAppearance.Secondary;

    [ObservableProperty]
    private int _snackbarTimeout = 2;

    private int _snackbarAppearanceComboBoxSelectedIndex = 1;

    public int SnackbarAppearanceComboBoxSelectedIndex
    {
        get => _snackbarAppearanceComboBoxSelectedIndex;
        set
        {
            SetProperty<int>(ref _snackbarAppearanceComboBoxSelectedIndex, value);
            UpdateSnackbarAppearance(value);
        }
    }

    [RelayCommand]
    private void OnOpenSnackbar(object sender)
    {
        _snackbarService.Show("Don't Blame Yourself.", "No Witcher's Ever Died In His Bed.", _snackbarAppearance,
            new SymbolIcon(SymbolRegular.Fluent24), TimeSpan.FromSeconds(SnackbarTimeout));
    }

    private void UpdateSnackbarAppearance(int appearanceIndex)
    {
        _snackbarAppearance = appearanceIndex switch
        {
            1 => ControlAppearance.Secondary,
            2 => ControlAppearance.Info,
            3 => ControlAppearance.Success,
            4 => ControlAppearance.Caution,
            5 => ControlAppearance.Danger,
            6 => ControlAppearance.Light,
            7 => ControlAppearance.Dark,
            8 => ControlAppearance.Transparent,
            _ => ControlAppearance.Primary
        };
    }
}
