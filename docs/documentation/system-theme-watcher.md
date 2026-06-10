# SystemThemeWatcher

`SystemThemeWatcher` automatically synchronizes the application's theme, accent color, and window backdrop with the current Windows theme settings. It listens for system-level changes and applies them to your application in real-time.

> [!TIP]
> The simplest way to enable system theme tracking is to call `SystemThemeWatcher.Watch(this);` in your main window's constructor.

## How It Works

`SystemThemeWatcher` attaches a hook to a window's message procedure (`WndProc`) and listens for the `WM_WININICHANGE` system message. When Windows broadcasts this message (e.g., when the user changes from light to dark mode), the watcher automatically calls `ApplicationThemeManager.ApplySystemTheme()` to update your application's appearance.

## Basic Usage

Enable theme watching in your window's constructor. This will use the default `Mica` backdrop and update accent colors.

```csharp
using Wpf.Ui.Appearance;

public partial class MainWindow : System.Windows.Window
{
    public MainWindow()
    {
        // This will apply the system theme, accent, and default backdrop.
        SystemThemeWatcher.Watch(this);

        InitializeComponent();
    }
}
```

## Customization

You can customize the backdrop effect and control whether the accent color is updated.

### Window Backdrop

Specify the `WindowBackdropType` to apply when the theme changes.

```csharp
SystemThemeWatcher.Watch(
    this,
    WindowBackdropType.Acrylic // Use Acrylic backdrop
);
```

Available backdrop types:

- `None`: No backdrop effect.
- `Auto`: Automatically selects the appropriate backdrop.
- `Mica`: The default Windows 11 Mica effect.
- `Acrylic`: The semi-transparent Acrylic effect.
- `Tabbed`: A blurred wallpaper effect available in recent Windows 11 versions.

### Accent Color Updates

You can prevent the watcher from changing the application's accent color.

```csharp
SystemThemeWatcher.Watch(
    this,
    updateAccents: false // Theme will change, but accent color will not
);
```

## Stop Watching

To stop a window from responding to system theme changes, use the `UnWatch` method.

```csharp
SystemThemeWatcher.UnWatch(this);
```

> [!IMPORTANT]
> Do not call `UnWatch` on a window that has not been loaded, as it will throw an `InvalidOperationException`. It is safe to call `Watch` on a window that is not yet loaded.

## Dependency Injection Usage

If you are resolving your main window from a dependency injection container, you can start the watcher after the host is built.

```csharp
var host = Host.CreateDefaultBuilder()
    .ConfigureServices(services =>
    {
        services.AddHostedService<ApplicationHostService>();
        services.AddSingleton<MainWindow>();
        // ... other services
    }).Build();

await host.StartAsync();

var mainWindow = host.Services.GetRequiredService<MainWindow>();

// Watch the window after it's been created
SystemThemeWatcher.Watch(mainWindow);
```

> [!NOTE]
> `SystemThemeWatcher` works on a static, global basis. While you can `Watch` multiple windows, the theme and accent settings applied will be the same for all of them based on the parameters of the last `Watch` call that triggered an update.
