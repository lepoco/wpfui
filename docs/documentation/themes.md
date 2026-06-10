# Application Themes

WPF UI provides a robust theming system that allows you to control your application's appearance, including support for light, dark, and high contrast modes. The `ApplicationThemeManager` class is the primary tool for managing themes at runtime.

> [!IMPORTANT]
> For theme changes to apply correctly, your colors and brushes should be referenced as `DynamicResource`.

## Setting the Initial Theme

The easiest way to set the initial theme is by using the `ThemesDictionary` in your `App.xaml`. This ensures that the correct theme resources are loaded at startup.

```xml
<Application
    xmlns:ui="http://schemas.lepo.co/wpfui/2022/xaml">
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ui:ThemesDictionary Theme="Light" />
                <ui:ControlsDictionary />
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
</Application>
```

The `Theme` property on `ThemesDictionary` can be set to `Light` or `Dark`.

## Changing the Theme at Runtime

Use the `ApplicationThemeManager.Apply()` method to change the theme while the application is running.

```csharp
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

// Apply the Light theme with a Mica backdrop
ApplicationThemeManager.Apply(
    ApplicationTheme.Light,
    WindowBackdropType.Mica
);
```

### `ApplicationTheme` Enum

This enum specifies the theme to apply:

- `Light`: The standard light theme.
- `Dark`: The standard dark theme.
- `HighContrast`: Automatically selects the appropriate Windows High Contrast theme.

## System Theme Integration

You can synchronize your application's theme with the current Windows theme settings.

### One-Time Sync

To apply the current system theme once, use `ApplySystemTheme()`.

```csharp
// Apply the current Windows theme (light or dark)
ApplicationThemeManager.ApplySystemTheme();
```

### Automatic Tracking

For continuous synchronization, use the `SystemThemeWatcher`. It automatically updates your app's theme and accent color when the user changes their Windows settings. See the [SystemThemeWatcher documentation](./system-theme-watcher.md) for more details.

```csharp
using Wpf.Ui.Appearance;

public partial class MainWindow : System.Windows.Window
{
    public MainWindow()
    {
        // Watch for system theme changes
        SystemThemeWatcher.Watch(this);

        InitializeComponent();
    }
}
```

## Reading the Current Theme

You can get the current application and system themes at any time.

- `ApplicationThemeManager.GetAppTheme()`: Returns the current `ApplicationTheme` (Light, Dark, or HighContrast).
- `ApplicationThemeManager.GetSystemTheme()`: Returns the current `SystemTheme`.

```csharp
ApplicationTheme currentAppTheme = ApplicationThemeManager.GetAppTheme();
SystemTheme currentSystemTheme = ApplicationThemeManager.GetSystemTheme();

if (currentAppTheme == ApplicationTheme.Dark)
{
    // ...
}
```

### `SystemTheme` Enum

This enum represents the actual theme reported by Windows, including decorative themes like `Glow`, `CapturedMotion`, and `Sunrise`. `ApplicationThemeManager` maps these to either `Light` or `Dark`.

## High Contrast Themes

WPF UI automatically handles Windows High Contrast themes. When a high contrast mode is detected, `ApplicationThemeManager` loads the appropriate high contrast resource dictionary (`HC1`, `HC2`, `HCBlack`, or `HCWhite`).

- `ApplicationThemeManager.IsHighContrast()`: Checks if the application is currently in a high contrast theme.
- `ApplicationThemeManager.IsSystemHighContrast()`: Checks if Windows is currently in a high contrast mode.

## Theme Changed Event

The `ApplicationThemeManager.Changed` event is triggered whenever the application's theme is successfully changed.

```csharp
ApplicationThemeManager.Changed += (currentTheme, currentAccent) =>
{
    Debug.WriteLine($"Theme changed to {currentTheme} with accent {currentAccent}");
};
```

This event is useful for applying custom logic after a theme change, such as updating graphics or non-WPF UI elements.

> [!TIP]
> The `Changed` event is fired by both manual `Apply()` calls and automatic updates from `SystemThemeWatcher`, providing a single place to react to any theme change.
