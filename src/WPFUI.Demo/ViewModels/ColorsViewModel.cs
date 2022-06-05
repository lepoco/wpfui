// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using WPFUI.Appearance;
using WPFUI.Common.Interfaces;
using WPFUI.Demo.Models.Colors;

namespace WPFUI.Demo.ViewModels;

public class ColorsViewModel : WPFUI.Mvvm.ViewModelBase, INavigationAware
{
    private bool _dataInitialized = false;

    private readonly string[] _paletteResources =
   {
        "PalettePrimaryBrush",
        "PaletteRedBrush",
        "PalettePinkBrush",
        "PalettePurpleBrush",
        "PaletteDeepPurpleBrush",
        "PaletteIndigoBrush",
        "PaletteBlueBrush",
        "PaletteLightBlueBrush",
        "PaletteCyanBrush",
        "PaletteTealBrush",
        "PaletteGreenBrush",
        "PaletteLightGreenBrush",
        "PaletteLimeBrush",
        "PaletteYellowBrush",
        "PaletteAmberBrush",
        "PaletteOrangeBrush",
        "PaletteDeepOrangeBrush",
        "PaletteBrownBrush",
        "PaletteGreyBrush",
        "PaletteBlueGreyBrush"
    };

    private readonly string[] _themeResources =
    {
        "SystemAccentColorLight1Brush",
        "SystemAccentColorLight2Brush",
        "SystemAccentColorLight3Brush",

        "ControlElevationBorderBrush",
        "CircleElevationBorderBrush",

        "AccentControlElevationBorderBrush",

        "TextFillColorPrimaryBrush",
        "TextFillColorSecondaryBrush",
        "TextFillColorTertiaryBrush",
        "TextFillColorDisabledBrush",
        "TextFillColorInverseBrush",

        "AccentTextFillColorDisabledBrush",
        "TextOnAccentFillColorSelectedTextBrush",

        "ControlFillColorDefaultBrush",
        "ControlFillColorSecondaryBrush",
        "ControlFillColorTertiaryBrush",
        "ControlFillColorDisabledBrush",
        "ControlSolidFillColorDefaultBrush",
        "AccentFillColorDisabledBrush",
        "MenuBorderColorDefaultBrush",

        "SystemFillColorSuccessBrush",
        "SystemFillColorCautionBrush",
        "SystemFillColorCriticalBrush",
        "SystemFillColorNeutralBrush",
        "SystemFillColorSolidNeutralBrush",
        "SystemFillColorAttentionBackgroundBrush",
        "SystemFillColorSuccessBackgroundBrush",
        "SystemFillColorCautionBackgroundBrush",
        "SystemFillColorCriticalBackgroundBrush",
        "SystemFillColorNeutralBackgroundBrush",
        "SystemFillColorSolidAttentionBackgroundBrush",
        "SystemFillColorSolidNeutralBackgroundBrush"
    };

    public IEnumerable<Pa__one> PaletteBrushes
    {
        get => GetValue<IEnumerable<Pa__one>>();
        set => SetValue(value);
    }

    public IEnumerable<Pa__one> ThemeBrushes
    {
        get => GetValue<IEnumerable<Pa__one>>();
        set => SetValue(value);
    }

    public int Columns
    {
        get => GetStructOrDefault(8);
        set => SetValue(value);
    }

    public ColorsViewModel()
    {
        WPFUI.Appearance.Theme.Changed += ThemeOnChanged;
    }

    /// <inheritdoc />
    protected override void OnViewCommand(object? parameter = null)
    {
    }

    public void OnNavigatedTo()
    {
        if (!_dataInitialized)
            InitializeData();
    }

    public void OnNavigatedFrom()
    {
    }

    private void ThemeOnChanged(ThemeType currentTheme, Color systemAccent)
    {
        FillTheme();
    }

    private void InitializeData()
    {
        _dataInitialized = true;

        FillPalette();
        FillTheme();
    }

    private void FillPalette()
    {
        var pallete = new List<Pa__one> { };

        foreach (var singleBrushKey in _paletteResources)
        {
            var singleBrush = Application.Current.Resources[singleBrushKey] as Brush;

            if (singleBrush == null)
                continue;

            string description;

            if (singleBrush is SolidColorBrush solidColorBrush)
                description =
                    $"R: {solidColorBrush.Color.R}, G: {solidColorBrush.Color.G}, B: {solidColorBrush.Color.B}";
            else
                description = "Gradient";

            pallete.Add(new Pa__one
            {
                Title = "PALETTE",
                Subtitle = description + "\n" + singleBrushKey,
                Brush = singleBrush,
                BrushKey = singleBrushKey
            });
        }

        PaletteBrushes = pallete;
    }

    private void FillTheme()
    {
        var theme = new List<Pa__one> { };

        foreach (var singleBrushKey in _themeResources)
        {
            var singleBrush = Application.Current.Resources[singleBrushKey] as Brush;

            if (singleBrush == null)
                continue;

            string description;

            if (singleBrush is SolidColorBrush solidColorBrush)
                description =
                    $"R: {solidColorBrush.Color.R}, G: {solidColorBrush.Color.G}, B: {solidColorBrush.Color.B}";
            else
                description = "Gradient";

            theme.Add(new Pa__one
            {
                Title = "THEME",
                Subtitle = description + "\n" + singleBrushKey,
                Brush = singleBrush,
                BrushKey = singleBrushKey
            });
        }

        ThemeBrushes = theme;
    }
}
