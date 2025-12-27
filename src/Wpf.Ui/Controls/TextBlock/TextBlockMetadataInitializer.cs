// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Initializes metadata for <see cref="System.Windows.Controls.TextBlock"/> and <see cref="System.Windows.Documents.TextElement"/> to align
/// native WPF text rendering with WPFUI's Fluent design system.
/// </summary>
/// <remarks>
/// <para>
/// This initializer applies a limited set of global adjustments to native WPF text elements
/// to preserve visual consistency across mixed control trees (native and WPFUI controls),
/// particularly for <see cref="System.Windows.Controls.TextBlock"/>, which is a fundamental text-rendering primitive
/// and participates in property inheritance.
/// </para>
/// <para>
/// The adjustments are intentionally constrained:
/// <list type="bullet">
/// <item>
/// <description>
/// Explicit values (local values, style setters, and inherited values) always take precedence.
/// The design-system defaults are applied only when the corresponding property is not explicitly set.
/// </description>
/// </item>
/// <item>
/// <description>
/// No dependency property precedence rules are altered; only default resolution behavior is affected.
/// </description>
/// </item>
/// </list>
/// </para>
/// <para>
/// New or extended design-system behavior is implemented in
/// <see cref="Wpf.Ui.Controls.TextBlock"/>. This initializer exists solely to provide
/// baseline compatibility for native WPF text elements.
/// </para>
/// </remarks>
internal static class TextBlockMetadataInitializer
{
    /*
    Module initializers can be used to perform initialization,
    but for certain reasons, this method is not being used for now.
    See https://learn.microsoft.com/zh-cn/dotnet/fundamentals/code-analysis/quality-rules/ca2255

#if NET5_0_OR_GREATER
    // Module initializer for runtimes that support it (net5+/net6+/net7+/net8+/net9+/net10).
    [ModuleInitializer]
    public static void InitModule()
    {
        EnsureInitialized();
    }
#endif

    */

    internal static void EnsureInitialized()
    {
        // Idempotent: safe to call multiple times.
        if (_initialized)
        {
            return;
        }

        _initialized = true;

        // WPFUI implements its own Fluent typography scale independently of WPF's experimental Fluent styles.
        // To ensure visual consistency, the default font size of native text elements must be aligned
        // with the design-system baseline.
        System.Windows.Documents.TextElement.FontSizeProperty.OverrideMetadata(
            typeof(System.Windows.Documents.TextElement),
            new FrameworkPropertyMetadata(14d)
        );

        // Override the default FontSize metadata for framework TextBlock.
        //
        // Rationale:
        // WPFUI enforces a Fluent-based typography system whose baseline font sizes
        // differ from WPF defaults. TextBlock is a fundamental text primitive, and
        // leaving its default FontSize unchanged would lead to inconsistent typography
        // when native WPF TextBlock instances appear alongside WPFUI controls.
        //
        // Design considerations:
        // - The default value is aligned with the Fluent "Body" typography size.
        // - An explicitly specified typography preset (via FontTypography) takes precedence
        //   and is resolved through a proxy dependency property to avoid manual resource lookup.
        // - Explicit local values, styles, and inherited values continue to participate
        //   in standard WPF precedence; this logic only participates during value coercion.
        //
        // Note:
        // The default FontSize value is intentionally set to match TextElement defaults
        // to avoid design-time inconsistencies caused by TextBlock switching to complex
        // content (Run/Inline) paths during property inspection.
        System.Windows.Controls.TextBlock.FontSizeProperty.OverrideMetadata(
            typeof(System.Windows.Controls.TextBlock),
            new FrameworkPropertyMetadata(
                14d,
                null,
                static (d, baseValue) =>
                {
                    // Typography property takes precedence:
                    var preset = d.GetValue(TextBlockTheming.FontTypographyProxyProperty) as FontTypographyPreset;
                    if (preset?.FontSize is { } size)
                    {
                        return size;
                    }

                    return baseValue;
                }
            )
        );

        // Override the default FontWeight metadata for framework TextBlock.
        //
        // Rationale:
        // Fluent typography defines a consistent default font weight for body text.
        // Aligning the framework TextBlock default ensures visual consistency across
        // mixed control trees containing both native WPF and WPFUI elements.
        //
        // Design considerations:
        // - The default value matches the Fluent typography baseline.
        // - When a FontTypography preset is specified, its FontWeight takes precedence
        //   and is resolved via a proxy dependency property.
        // - No dependency property precedence rules are altered; explicit values
        //   provided by consumers remain authoritative.
        System.Windows.Controls.TextBlock.FontWeightProperty.OverrideMetadata(
            typeof(System.Windows.Controls.TextBlock),
            new FrameworkPropertyMetadata(
                FontWeights.Regular,
                null,
                static (d, baseValue) =>
                {
                    // Typography property takes precedence:
                    var preset = d.GetValue(TextBlockTheming.FontTypographyProxyProperty) as FontTypographyPreset;
                    if (preset?.FontWeight is { } weight)
                    {
                        return weight;
                    }

                    return baseValue;
                }
            )
        );

        // Ensure that when Foreground is not explicitly set, native TextBlock instances
        // fall back to the library's themed text brush (e.g., TextFillColorPrimaryBrush),
        // and optionally resolve a higher-priority appearance-defined foreground when present.
        //
        // Rationale:
        // TextBlock is a fundamental text-rendering primitive and participates in Foreground
        // property inheritance. In mixed visual trees (native WPF controls combined with WPFUI
        // controls), leaving native TextBlock unthemed can lead to visible color discontinuities,
        // especially across inheritance boundaries.
        //
        // Design considerations:
        // - An explicitly specified Appearance (when present and resolvable) has the highest precedence.
        // - Explicit Foreground values (local values, style setters, and inherited values) remain authoritative.
        // - The themed fallback brush is applied only when no explicit Foreground is available.
        // - Dependency property precedence rules are not modified.
        //
        // This behavior participates only in foreground value resolution and does not prevent
        // consumers from fully controlling Foreground via standard WPF mechanisms.
        System.Windows.Controls.TextBlock.ForegroundProperty.OverrideMetadata(
            typeof(System.Windows.Controls.TextBlock),
            new FrameworkPropertyMetadata(
                null,
                null,
                static (d, baseValue) =>
                {
                    // Appearance property takes precedence:
                    // If the Appearance property is set and resolves to a Brush, use it.
                    if (d.GetValue(TextBlockTheming.AppearanceForegroundProxyProperty) is Brush appearance)
                    {
                        return appearance;
                    }

                    // Otherwise, if Foreground already has a valid value
                    // (set via styles, inheritance, or other mechanisms), keep it.
                    if (baseValue is Brush)
                    {
                        return baseValue;
                    }

                    // Otherwise, try to resolve the dynamic theme resource referenced by
                    // the implicit default foreground resource property.
                    if (d.GetValue(TextBlockTheming.FallbackForegroundProperty) is Brush defaultForeground)
                    {
                        return defaultForeground;
                    }

                    // Finally, fall back to WPF's default primary text foreground brush.
                    return Brushes.Black;
                }
            )
        );
    }

    private static bool _initialized;
}