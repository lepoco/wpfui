// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using WPFUI.Appearance;

namespace WPFUI.Demo.Views.Pages;

public struct Pa__one
{
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public Brush Brush { get; set; }
    public string BrushKey { get; set; }
}

public class ColorsViewData : WPFUI.Common.ViewData
{
    private List<Pa__one> _paletteBrushes = new();
    public List<Pa__one> PaletteBrushes
    {
        get => _paletteBrushes;
        set => UpdateProperty(ref _paletteBrushes, value, nameof(PaletteBrushes));
    }

    private List<Pa__one> _themeBrushes = new();
    public List<Pa__one> ThemeBrushes
    {
        get => _themeBrushes;
        set => UpdateProperty(ref _themeBrushes, value, nameof(ThemeBrushes));
    }

    private int _columns = 8;
    public int Columns
    {
        get => _columns;
        set => UpdateProperty(ref _columns, value, nameof(Columns));
    }
}

/// <summary>
/// Interaction logic for Colors.xaml
/// </summary>
public partial class Colors
{
    private string[] _paletteResources =
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

    private string[] _themeResources =
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

    internal ColorsViewData _data;

    public Colors()
    {
        InitializeComponent();
        InitializeBrushes();

        WPFUI.Appearance.Theme.Changed += ThemeOnChanged;
    }

    private void InitializeBrushes()
    {
        _data = new ColorsViewData();
        DataContext = _data;

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

        _data.PaletteBrushes = pallete;
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

        _data.ThemeBrushes = theme;
    }

    private void ThemeOnChanged(ThemeType currenttheme, Color systemaccent)
    {
        FillTheme();
    }

    private void SetColorNameInClipboard(string colorName)
    {
        WPFUI.Common.Clipboard.SetText("{DynamicResource " + colorName + "}");

        if (Application.Current.MainWindow is not Container container)
            return;

        container.RootSnackbar.Icon = WPFUI.Common.SymbolRegular.Diversity24;
        container.RootSnackbar.Show("Copied!", $"The {colorName} has been copied to the clipboard!");
    }

    private void ColorPalette_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is not WPFUI.Controls.CardColor cardColor)
            return;

        var colorName = cardColor.Tag?.ToString() ?? String.Empty;

        if (String.IsNullOrEmpty(colorName))
            return;

        SetColorNameInClipboard(colorName);
    }

    private void ColorTheme_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is not WPFUI.Controls.CardColor cardColor)
            return;

        var colorName = cardColor.Tag?.ToString() ?? String.Empty;

        if (String.IsNullOrEmpty(colorName))
            return;

        SetColorNameInClipboard(colorName);
    }
}
