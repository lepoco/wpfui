# Accent Colors

Accent colors provide visual emphasis and brand identity in WPF UI applications. The library manages accent color resources that automatically adapt to light and dark themes.

> [!TIP]
> Use `SystemThemeWatcher.Watch(this)` in your main window constructor to automatically sync accent colors with Windows personalization settings.

## Apply System Accent

Use the system's personalization accent color:

```csharp
using Wpf.Ui.Appearance;

ApplicationAccentColorManager.ApplySystemAccent();
```

## Apply Theme with Accent

Apply theme and accent together:

```csharp
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

ApplicationThemeManager.Apply(
    ApplicationTheme.Dark,
    WindowBackdropType.Mica,
    updateAccent: true  // Automatically applies system accent
);
```

Available backdrop types: `None`, `Auto`, `Mica`, `Acrylic`, `Tabbed`.

> [!IMPORTANT]
> Always use `DynamicResource` (not `StaticResource`) for accent color bindings to receive runtime updates when accent changes.

## Apply Custom Accent

Set a custom accent color:

```csharp
ApplicationAccentColorManager.Apply(
    Color.FromArgb(0xFF, 0xEE, 0x00, 0xBB),
    ApplicationTheme.Dark
);
```

Retrieve the Windows colorization color programmatically:

```csharp
Color colorizationColor = ApplicationAccentColorManager.GetColorizationColor();
ApplicationAccentColorManager.Apply(colorizationColor, ApplicationTheme.Dark);
```

## Accent Color Resources

WPF UI provides these accent color resources:

### System Accent Colors

Base accent colors that update when you call `ApplicationAccentColorManager.Apply()`:

- `SystemAccentColor` - Primary system accent
- `SystemAccentColorPrimary` - Lighter/darker variant for light/dark themes
- `SystemAccentColorSecondary` - More prominent variant (most commonly used)
- `SystemAccentColorTertiary` - Strongest variant

```xml
<Border Background="{DynamicResource SystemAccentColorSecondaryBrush}" />
```

> [!TIP]
> `SystemAccentColorSecondary` is the most commonly used variant for interactive elements and provides optimal contrast in both light and dark themes.

### Accent Text Colors

For text and interactive elements like links:

- `AccentTextFillColorPrimaryBrush` - Primary accent text (rest/hover state)
- `AccentTextFillColorSecondaryBrush` - Secondary accent text
- `AccentTextFillColorTertiaryBrush` - Tertiary accent text (pressed state)
- `AccentTextFillColorDisabledBrush` - Disabled accent text

```xml
<ui:Anchor
    Content="WPF UI Documentation"
    NavigateUri="https://wpfui.lepo.co/"
    Foreground="{DynamicResource AccentTextFillColorPrimaryBrush}" />
```

### Accent Fill Colors

For button backgrounds and filled surfaces:

- `AccentFillColorDefaultBrush` - Default accent fill
- `AccentFillColorSecondaryBrush` - Secondary fill (90% opacity)
- `AccentFillColorTertiaryBrush` - Tertiary fill (80% opacity)
- `AccentFillColorDisabledBrush` - Disabled state fill

```xml
<Button Background="{DynamicResource AccentFillColorDefaultBrush}" />
```

### Text on Accent Colors

For text displayed on accent-colored backgrounds. These resources automatically adjust to black or white based on accent brightness:

- `TextOnAccentFillColorPrimary` - Primary text color on accent backgrounds
- `TextOnAccentFillColorSecondary` - Secondary text on accent backgrounds
- `TextOnAccentFillColorDisabled` - Disabled text on accent backgrounds

> [!NOTE]
> Text colors automatically switch between black and white when accent brightness exceeds 80% HSV to maintain readability.

## Theme-Specific Behavior

Accent variants adjust automatically based on the application theme:

**Dark Theme:**

- Primary: Base color + 17 brightness, -30% saturation
- Secondary: Base color + 17 brightness, -45% saturation
- Tertiary: Base color + 17 brightness, -65% saturation

**Light Theme:**

- Primary: Base color - 10 brightness
- Secondary: Base color - 25 brightness
- Tertiary: Base color - 40 brightness

> [!NOTE]
> Brightness adjustments are calculated in HSV color space. Negative values darken the color, positive values lighten it.

## Advanced: Custom Accent Variants

Specify all accent variants manually:

```csharp
ApplicationAccentColorManager.Apply(
    systemAccent: Color.FromArgb(0xFF, 0x00, 0x78, 0xD4),
    primaryAccent: Color.FromArgb(0xFF, 0x00, 0x67, 0xC0),
    secondaryAccent: Color.FromArgb(0xFF, 0x00, 0x3E, 0x92),
    tertiaryAccent: Color.FromArgb(0xFF, 0x00, 0x1A, 0x68)
);
```

> [!CAUTION]
> Manually specified accent variants won't automatically adjust when theme changes. Consider using automatic variant generation unless you need precise color control.

## Accessing Current Accent

Read current accent colors from `ApplicationAccentColorManager`:

```csharp
Color currentSystemAccent = ApplicationAccentColorManager.SystemAccent;
Color currentPrimaryAccent = ApplicationAccentColorManager.PrimaryAccent;
Color currentSecondaryAccent = ApplicationAccentColorManager.SecondaryAccent;
Color currentTertiaryAccent = ApplicationAccentColorManager.TertiaryAccent;

Brush accentBrush = ApplicationAccentColorManager.SystemAccentBrush;
```

## Theme Changed Event

Monitor accent changes when theme is applied:

```csharp
ApplicationThemeManager.Changed += (theme, accent) =>
{
    Debug.WriteLine($"Theme changed to {theme}, accent: {accent}");
};
```

## Automatic System Theme Tracking

Use `SystemThemeWatcher` to automatically update theme and accent when Windows settings change:

```csharp
using Wpf.Ui.Appearance;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        // Watch system theme changes with Mica backdrop and accent updates
        SystemThemeWatcher.Watch(this);
        
        InitializeComponent();
    }
}
```

With custom backdrop and accent settings:

```csharp
SystemThemeWatcher.Watch(
    this,
    WindowBackdropType.Acrylic,
    updateAccents: true
);
```

Stop watching for theme changes:

```csharp
SystemThemeWatcher.UnWatch(this);
```

> [!NOTE]
> `SystemThemeWatcher` monitors `WM_WININICHANGE` messages and automatically applies system theme and accent when Windows personalization settings change.
