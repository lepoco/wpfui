// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Extended <see cref="System.Windows.Controls.TextBlock"/> that integrates with the library's
/// typography and appearance resources.
/// </summary>
/// <remarks>
/// This control supports two mechanisms for applying design-system typography:
/// <para>
/// - The <see cref="FontTypography"/> enum which is mapped to a resource key and resolved
///   at runtime to a <see cref="FontTypographyPreset"/>.
/// </para>
/// <para>
/// - An internal resource-backed preset is resolved via a private puppet dependency property
///   so the framework's resource system (DynamicResource) performs resolution and updates.
/// </para>
/// <para>
/// The resolved preset is used to coerce <see cref="System.Windows.Controls.TextBlock.FontSizeProperty"/>
/// and <see cref="System.Windows.Controls.TextBlock.FontWeight"/>.
/// </para>
/// <para>
/// The <see cref="Appearance"/> property maps to brush resources and is applied via a resource reference.
/// </para>
/// </remarks>
/// <example>
/// <code lang="xml">
/// &lt;ui:TextBlock FontTypography="Body" Appearance="Primary" Text="Hello, world!" /&gt;
/// </code>
/// </example>
public class TextBlock : System.Windows.Controls.TextBlock
{
    static TextBlock()
    {
        // To ensure the initialization at the entry point is invoked, retry the call here.
        TextBlockMetadataInitializer.EnsureInitialized();
    }

    /// <summary>Identifies the <see cref="FontTypography"/> dependency property.</summary>
    /// <remarks>
    /// <para>
    /// <see cref="FontTypography"/> defines a logical typography token (for example
    /// <c>Body</c>, <c>Caption</c>, etc.) which is resolved to a <see cref="FontTypographyPreset"/>
    /// resource by the WPFUI theming system. The preset determines font-related properties
    /// such as <see cref="System.Windows.Controls.TextBlock.FontSize"/> and
    /// <see cref="System.Windows.Controls.TextBlock.FontWeight"/>.
    /// </para>
    ///
    /// <para>
    /// Although declared on <see cref="TextBlock"/>, this property is an
    /// <b>attached property</b> and can also be applied to the native WPF
    /// <see cref="System.Windows.Controls.TextBlock"/>, allowing standard
    /// TextBlock controls to participate in WPFUI typography.
    /// </para>
    ///
    /// <para>
    /// When <see cref="FontTypography"/> is set, a resource reference to the
    /// corresponding preset is attached to an internal proxy property so that
    /// the WPF resource system performs resolution and updates automatically.
    /// The resolved preset is used to coerce font-size and font-weight values
    /// so that typography-defined values take precedence over local changes.
    /// </para>
    ///
    /// Example:
    /// <example>
    /// <code lang="xml">
    /// &lt;TextBlock Text="Hello, world!" ui:TextBlock.FontTypography="Body" /&gt;
    /// &lt;ui:TextBlock Text="Hello, world!" FontTypography="Body" /&gt;
    /// </code>
    /// </example>
    /// </remarks>
    public static readonly DependencyProperty FontTypographyProperty =
        DependencyProperty.RegisterAttached(
            nameof(FontTypography),
            typeof(FontTypography?),
            typeof(TextBlock),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.AffectsMeasure |
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.Inherits,
                OnFontTypographyChanged
            )
        );

    /// <summary>Identifies the <see cref="Appearance"/> dependency property.</summary>
    /// <remarks>
    /// <para>
    /// <see cref="Appearance"/> defines a logical text appearance (for example
    /// <c>Primary</c>, <c>Secondary</c>, etc.) which is resolved to a themed
    /// foreground brush by the WPFUI theming system.
    /// </para>
    ///
    /// <para>
    /// Although declared on <see cref="TextBlock"/>, this property is an
    /// <b>attached property</b> and can also be applied to the native WPF
    /// <see cref="System.Windows.Controls.TextBlock"/>, allowing standard
    /// TextBlock controls to participate in WPFUI text theming.
    /// </para>
    ///
    /// <para>
    /// When <see cref="Appearance"/> is explicitly set (non-null) and resolves
    /// to a valid brush, it takes <b>highest precedence</b> during foreground
    /// resolution and overrides values provided by styles, inheritance, or
    /// local <see cref="System.Windows.Controls.TextBlock.Foreground"/>.
    /// </para>
    ///
    /// <para>
    /// When <see cref="Appearance"/> is <c>null</c>, it does not participate in
    /// foreground selection and the normal WPF precedence rules apply.
    /// </para>
    ///
    /// Example:
    /// <example>
    /// <code>
    /// &lt;TextBlock Text="Hello, world!" ui:TextBlock.Appearance="Primary" /&gt;
    /// &lt;ui:TextBlock Text="Hello, world!" Appearance="Primary" /&gt;
    /// </code>
    /// </example>
    /// </remarks>
    public static readonly DependencyProperty AppearanceProperty =
        DependencyProperty.RegisterAttached(
            nameof(Appearance),
            typeof(TextColor?),
            typeof(TextBlock),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender |
                FrameworkPropertyMetadataOptions.Inherits,
                OnAppearanceChanged
            )
        );

    public TextBlock()
    {
        SetResourceReference(TextBlockTheming.FallbackForegroundProperty, TextColor.Primary.ToResourceKey());
    }

    /// <summary>
    /// Helper for getting <see cref="FontTypographyProperty"/> from <paramref name="obj"/>.
    /// </summary>
    /// <param name="obj">
    /// <see cref="DependencyObject"/> to read <see cref="FontTypographyProperty"/> from.
    /// </param>
    /// <returns>
    /// FontTypography property value.
    /// </returns>
    [AttachedPropertyBrowsableForType(typeof(DependencyObject))]
    public static FontTypography? GetFontTypography(DependencyObject obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        return (FontTypography?)obj.GetValue(FontTypographyProperty);
    }

    /// <summary>
    /// Helper for setting <see cref="FontTypographyProperty"/> on <paramref name="obj"/>.
    /// </summary>
    /// <param name="obj">
    /// <see cref="DependencyObject"/> to set <see cref="FontTypographyProperty"/> on.
    /// </param>
    /// <param name="value">
    /// FontTypography property value.
    /// </param>
    public static void SetFontTypography(DependencyObject obj, FontTypography? value)
    {
        if (obj == null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        obj.SetValue(FontTypographyProperty, value);
    }

    /// <summary>
    /// Helper for getting <see cref="AppearanceProperty"/> from <paramref name="obj"/>.
    /// </summary>
    /// <param name="obj">
    /// <see cref="DependencyObject"/> to read <see cref="AppearanceProperty"/> from.
    /// </param>
    /// <returns>
    /// Appearance property value.
    /// </returns>
    [AttachedPropertyBrowsableForType(typeof(DependencyObject))]
    public static TextColor? GetAppearance(DependencyObject obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        return (TextColor?)obj.GetValue(AppearanceProperty);
    }

    /// <summary>
    /// Helper for setting <see cref="AppearanceProperty"/> on <paramref name="obj"/>.
    /// </summary>
    /// <param name="obj">
    /// <see cref="DependencyObject"/> to set <see cref="AppearanceProperty"/> on.
    /// </param>
    /// <param name="value">
    /// Appearance property value.
    /// </param>
    public static void SetAppearance(DependencyObject obj, TextColor? value)
    {
        if (obj == null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        obj.SetValue(AppearanceProperty, value);
    }

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
    public TextColor? Appearance
    {
        get => (TextColor?)GetValue(AppearanceProperty);
        set => SetValue(AppearanceProperty, value);
    }

    private static void OnFontTypographyChanged(DependencyObject o, DependencyPropertyChangedEventArgs args)
    {
        if (o is System.Windows.Controls.TextBlock tb)
        {
            if (args.NewValue is FontTypography newTypography)
            {
                var resourceKey = newTypography.ToResourceKey();

                // Use WPF resource reference mechanism to resolve and cache the preset.
                // This avoids manual TryFindResource tree traversal.
                tb.SetResourceReference(TextBlockTheming.FontTypographyProxyProperty, resourceKey);
            }
            else
            {
                // Clear any puppet resource reference
                tb.ClearValue(TextBlockTheming.FontTypographyProxyProperty);
            }

            // Re-evaluate coerced values so when FontTypography is set, the
            // CoerceValueCallbacks installed on FontSize/FontWeight will take effect
            // and prevent local changes from overriding typography-defined values.
            tb.CoerceValue(FontSizeProperty);
            tb.CoerceValue(FontWeightProperty);
        }
    }

    private static void OnAppearanceChanged(DependencyObject o, DependencyPropertyChangedEventArgs args)
    {
        if (o is System.Windows.Controls.TextBlock tb)
        {
            if (args.NewValue is TextColor textColor)
            {
                var resourceKey = textColor.ToResourceKey();

                // Similar to OnFontTypographyChanged, attach themed color resource reference to a proxy property,
                // allowing the WPF resource system to handle updates automatically.
                tb.SetResourceReference(TextBlockTheming.AppearanceForegroundProxyProperty, resourceKey);
            }
            else
            {
                tb.ClearValue(TextBlockTheming.AppearanceForegroundProxyProperty);
            }

            tb.CoerceValue(ForegroundProperty);
        }
    }
}
