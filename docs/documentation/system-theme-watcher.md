# SystemThemeWatcher

SystemThemeWatcher automatically updates the application background if the system theme or color changes. This component is used to adapt the application's background effect and theme according to the system theme.

## Features

  * Automatic Theme Updates: Updates the application's background and theme when the system theme changes.

  * Global Settings: Settings apply globally and cannot be changed for each System.Windows.Window.

  * Supported Backdrop Types: Provides background effects compatible with WindowBackdropType types (e.g., Egg).


## Usage

You can use SystemThemeWatcher to start watching a window's background and theme like this:

```cs
SystemThemeWatcher.Watch(this as System.Windows.Window);
SystemThemeWatcher.UnWatch(this as System.Windows.Window);
```

or
```cs
SystemThemeWatcher.Watch(
    _serviceProvider.GetRequiredService<MainWindow>()
);
```

## Example Usage

Here's an example of using SystemThemeWatcher in the MainWindow class to start watching the theme when the window is loaded:

```cs
namespace MyApp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        Loaded += (sender, args) =>
        {
            Wpf.Ui.Appearance.SystemThemeWatcher.Watch(
                this,                                    // Window class
                Wpf.Ui.Controls.WindowBackdropType.Mica, // Background type
                true                                     // Whether to change accents automatically
            );
        };
    }
}
```

## Methods

### Watch

Applies the background effect and theme according to the system theme to the observed window.

```cs
public static void Watch(
    Window? window,
    WindowBackdropType backdrop = WindowBackdropType.Mica,
    bool updateAccents = true
)
```

### UnWatch

Stops watching the window and removes the hook to receive system messages.

```cs
public static void UnWatch(Window? window)

```

### WndProc

Listens to system messages on the application windows.

```cs
private static IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)

```

> [!IMPORTANT]
> If UnWatch is called on a window that has not yet loaded, an InvalidOperationException may occur. Ensure that the window is loaded before calling UnWatch.
