// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Media;

namespace Wpf.Ui.Markup;

/// <summary>
/// Collection of theme resources.
/// </summary>
#pragma warning disable CS1591
public enum ThemeResource
{
    /// <summary>
    /// Unspecified theme resource.
    /// </summary>
    Unknown,

    // Accents
    SystemAccentColor,
    SystemAccentColorPrimary,
    SystemAccentColorSecondary,
    SystemAccentColorTertiary,
    SystemAccentColorPrimaryBrush,
    SystemAccentColorSecondaryBrush,
    SystemAccentColorTertiaryBrush,

    // Background
    ApplicationBackgroundColor,
    ApplicationBackgroundBrush,

    // Focus
    KeyboardFocusBorderColor,
    KeyboardFocusBorderColorBrush,

    // Text
    TextFillColorPrimary,
    TextFillColorSecondary,
    TextFillColorTertiary,
    TextFillColorDisabled,
    TextPlaceholderColor,
    TextFillColorInverse,

    AccentTextFillColorDisabled,
    TextOnAccentFillColorSelectedText,
    TextOnAccentFillColorPrimary,
    TextOnAccentFillColorSecondary,
    TextOnAccentFillColorDisabled,

    ControlFillColorDefault,
    ControlFillColorSecondary,
    ControlFillColorTertiary,
    ControlFillColorDisabled,
    ControlFillColorTransparent,
    ControlFillColorInputActive,

    ControlStrongFillColorDefault,
    ControlStrongFillColorDisabled,

    ControlSolidFillColorDefault,

    SubtleFillColorTransparent,
    SubtleFillColorSecondary,
    SubtleFillColorTertiary,
    SubtleFillColorDisabled,

    ControlAltFillColorTransparent,
    ControlAltFillColorSecondary,
    ControlAltFillColorTertiary,
    ControlAltFillColorQuarternary,
    ControlAltFillColorDisabled,

    ControlOnImageFillColorDefault,
    ControlOnImageFillColorSecondary,
    ControlOnImageFillColorTertiary,
    ControlOnImageFillColorDisabled,

    AccentFillColorDisabled,

    ControlStrokeColorDefault,
    ControlStrokeColorSecondary,
    ControlStrokeColorTertiary,
    ControlStrokeColorOnAccentDefault,
    ControlStrokeColorOnAccentSecondary,
    ControlStrokeColorOnAccentTertiary,
    ControlStrokeColorOnAccentDisabled,

    ControlStrokeColorForStrongFillWhenOnImage,

    CardStrokeColorDefault,
    CardStrokeColorDefaultSolid,

    ControlStrongStrokeColorDefault,
    ControlStrongStrokeColorDisabled,

    SurfaceStrokeColorDefault,
    SurfaceStrokeColorFlyout,
    SurfaceStrokeColorInverse,

    DividerStrokeColorDefault,

    FocusStrokeColorOuter,
    FocusStrokeColorInner,

    CardBackgroundFillColorDefault,
    CardBackgroundFillColorSecondary,

    SmokeFillColorDefault,

    LayerFillColorDefault,
    LayerFillColorAlt,
    LayerOnAcrylicFillColorDefault,
    LayerOnAccentAcrylicFillColorDefault,

    LayerOnMicaBaseAltFillColorDefault,
    LayerOnMicaBaseAltFillColorSecondary,
    LayerOnMicaBaseAltFillColorTertiary,
    LayerOnMicaBaseAltFillColorTransparent,

    SolidBackgroundFillColorBase,
    SolidBackgroundFillColorSecondary,
    SolidBackgroundFillColorTertiary,
    SolidBackgroundFillColorQuarternary,
    SolidBackgroundFillColorTransparent,
    SolidBackgroundFillColorBaseAlt,

    SystemFillColorSuccess,
    SystemFillColorCaution,
    SystemFillColorCritical,
    SystemFillColorNeutral,
    SystemFillColorSolidNeutral,
    SystemFillColorAttentionBackground,
    SystemFillColorSuccessBackground,
    SystemFillColorCautionBackground,
    SystemFillColorCriticalBackground,
    SystemFillColorNeutralBackground,
    SystemFillColorSolidAttentionBackground,
    SystemFillColorSolidNeutralBackground,

    // Brushes
    TextFillColorPrimaryBrush,
    TextFillColorSecondaryBrush,
    TextFillColorTertiaryBrush,
    TextFillColorDisabledBrush,
    TextPlaceholderColorBrush,
    TextFillColorInverseBrush,

    AccentTextFillColorDisabledBrush,

    TextOnAccentFillColorSelectedTextBrush,

    TextOnAccentFillColorPrimaryBrush,
    TextOnAccentFillColorSecondaryBrush,
    TextOnAccentFillColorDisabledBrush,

    ControlFillColorDefaultBrush,
    ControlFillColorSecondaryBrush,
    ControlFillColorTertiaryBrush,
    ControlFillColorDisabledBrush,
    ControlFillColorTransparentBrush,
    ControlFillColorInputActiveBrush,

    ControlStrongFillColorDefaultBrush,
    ControlStrongFillColorDisabledBrush,

    ControlSolidFillColorDefaultBrush,

    SubtleFillColorTransparentBrush,
    SubtleFillColorSecondaryBrush,
    SubtleFillColorTertiaryBrush,
    SubtleFillColorDisabledBrush,

    ControlAltFillColorTransparentBrush,
    ControlAltFillColorSecondaryBrush,
    ControlAltFillColorTertiaryBrush,
    ControlAltFillColorQuarternaryBrush,
    ControlAltFillColorDisabledBrush,

    ControlOnImageFillColorDefaultBrush,
    ControlOnImageFillColorSecondaryBrush,
    ControlOnImageFillColorTertiaryBrush,
    ControlOnImageFillColorDisabledBrush,

    AccentFillColorDisabledBrush,

    ControlStrokeColorDefaultBrush,
    ControlStrokeColorSecondaryBrush,
    ControlStrokeColorTertiaryBrush,
    ControlStrokeColorOnAccentDefaultBrush,
    ControlStrokeColorOnAccentSecondaryBrush,
    ControlStrokeColorOnAccentTertiaryBrush,
    ControlStrokeColorOnAccentDisabledBrush,

    ControlStrokeColorForStrongFillWhenOnImageBrush,

    CardStrokeColorDefaultBrush,
    CardStrokeColorDefaultSolidBrush,

    ControlStrongStrokeColorDefaultBrush,
    ControlStrongStrokeColorDisabledBrush,

    SurfaceStrokeColorDefaultBrush,
    SurfaceStrokeColorFlyoutBrush,
    SurfaceStrokeColorInverseBrush,

    DividerStrokeColorDefaultBrush,

    FocusStrokeColorOuterBrush,
    FocusStrokeColorInnerBrush,

    CardBackgroundFillColorDefaultBrush,
    CardBackgroundFillColorSecondaryBrush,

    SmokeFillColorDefaultBrush,

    LayerFillColorDefaultBrush,
    LayerFillColorAltBrush,
    LayerOnAcrylicFillColorDefaultBrush,
    LayerOnAccentAcrylicFillColorDefaultBrush,

    LayerOnMicaBaseAltFillColorDefaultBrush,
    LayerOnMicaBaseAltFillColorSecondaryBrush,
    LayerOnMicaBaseAltFillColorTertiaryBrush,
    LayerOnMicaBaseAltFillColorTransparentBrush,

    SolidBackgroundFillColorBaseBrush,
    SolidBackgroundFillColorSecondaryBrush,
    SolidBackgroundFillColorTertiaryBrush,
    SolidBackgroundFillColorQuarternaryBrush,
    SolidBackgroundFillColorBaseAltBrush,

    SystemFillColorSuccessBrush,
    SystemFillColorCautionBrush,
    SystemFillColorCriticalBrush,
    SystemFillColorNeutralBrush,
    SystemFillColorSolidNeutralBrush,
    SystemFillColorAttentionBackgroundBrush,
    SystemFillColorSuccessBackgroundBrush,
    SystemFillColorCautionBackgroundBrush,
    SystemFillColorCriticalBackgroundBrush,
    SystemFillColorNeutralBackgroundBrush,
    SystemFillColorSolidAttentionBackgroundBrush,
    SystemFillColorSolidNeutralBackgroundBrush,

    /// <summary>
    /// Gradient <see cref="Brush"/>.
    /// </summary>
    ControlElevationBorderBrush,

    /// <summary>
    /// Gradient <see cref="Brush"/>.
    /// </summary>
    CircleElevationBorderBrush,

    /// <summary>
    /// Gradient <see cref="Brush"/>.
    /// </summary>
    AccentControlElevationBorderBrush
}

