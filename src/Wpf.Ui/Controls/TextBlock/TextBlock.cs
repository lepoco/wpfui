// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Extended <see cref="System.Windows.Controls.TextBlock"/> with additional parameters like <see cref="FontTypography"/>.
/// </summary>
public class TextBlock : System.Windows.Controls.TextBlock
{
    private FontTypographyPreset? _cachedTypographyPreset;

    static TextBlock()
    {
        // Coerce FontSize and FontWeight based on FontTypography when set.
        FontSizeProperty.OverrideMetadata(
            typeof(TextBlock),
            new FrameworkPropertyMetadata(
                SystemFonts.MessageFontSize, // placeholder default, baseValue will be used when not coerced
                null,
                static (d, baseValue) => ((TextBlock)d)._cachedTypographyPreset?.FontSize ?? baseValue
            )
        );

        FontWeightProperty.OverrideMetadata(
            typeof(TextBlock),
            new FrameworkPropertyMetadata(
                FontWeights.Normal,
                null,
                static (d, baseValue) => ((TextBlock)d)._cachedTypographyPreset?.FontWeight ?? baseValue)
        );
    }

    /// <summary>Identifies the <see cref="FontTypography"/> dependency property.</summary>
    public static readonly DependencyProperty FontTypographyProperty = DependencyProperty.Register(
        nameof(FontTypography),
        typeof(FontTypography?),
        typeof(TextBlock),
        new PropertyMetadata(
            null,
            static (o, args) =>
            {
                ((TextBlock)o).OnFontTypographyChanged((FontTypography?)args.NewValue);
            }
        )
    );

    /// <summary>Identifies the <see cref="Appearance"/> dependency property.</summary>
    public static readonly DependencyProperty AppearanceProperty = DependencyProperty.Register(
        nameof(Appearance),
        typeof(TextColor),
        typeof(TextBlock),
        new PropertyMetadata(
            TextColor.Primary,
            static (o, args) =>
            {
                ((TextBlock)o).OnAppearanceChanged((TextColor)args.NewValue);
            }
        )
    );

    /// <summary>
    /// Private puppet dependency property used to hold a ResourceReference to a FontTypographyPreset.
    /// </summary>
    private static readonly DependencyProperty FontTypographyPresetResourceProperty = DependencyProperty.Register(
        "FontTypographyPresetResource",
        typeof(FontTypographyPreset),
        typeof(TextBlock),
        new PropertyMetadata(null)
    );

    /// <summary>
    /// Gets or sets the <see cref="FontTypography"/> of the text.
    /// </summary>
    public FontTypography? FontTypography
    {
        get => (FontTypography?)GetValue(FontTypographyProperty);
        set => SetValue(FontTypographyProperty, value);
    }

    /// <summary>
    /// Gets or sets the color of the text.
    /// </summary>
    public TextColor Appearance
    {
        get => (TextColor)GetValue(AppearanceProperty);
        set => SetValue(AppearanceProperty, value);
    }

    private void OnFontTypographyChanged(FontTypography? newTypography)
    {
        if (newTypography.HasValue)
        {
            var resourceKey = newTypography.Value.ToResourceKey();

            // Use WPF resource reference mechanism to resolve and cache the preset.
            // This avoids manual TryFindResource tree traversal.
            SetResourceReference(FontTypographyPresetResourceProperty, resourceKey);

            _cachedTypographyPreset = GetValue(FontTypographyPresetResourceProperty) as FontTypographyPreset;
        }
        else
        {
            // Clear any puppet resource reference and cached preset
            ClearValue(FontTypographyPresetResourceProperty);
            _cachedTypographyPreset = null;
        }

        // Re-evaluate coerced values so when FontTypography is set, the
        // CoerceValueCallbacks installed on FontSize/FontWeight will take effect
        // and prevent local changes from overriding typography-defined values.
        CoerceValue(FontSizeProperty);
        CoerceValue(FontWeightProperty);
    }

    private void OnAppearanceChanged(TextColor textColor)
    {
        SetResourceReference(ForegroundProperty, textColor.ToResourceKey());
    }
}