# Backgrounds
With the help of WPF UI, you can take advantage of the new backgrounds available for Windows 11.  
All you need to do is register your `Window` in the [Background](https://github.com/lepoco/wpfui/blob/main/WPFUI/Appearance/Background.cs) class before initialization.

```cpp
namespace MyApp
{
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      WPFUI.Appearance.Background.Apply(
        this,                                // Window class
        WPFUI.Appearance.BackgroundType.Mica // Background type
      );

      InitializeComponent();
    }
  }
}
```

### Available backgrounds
For the premiere edition of Windows 11, only the `Mica` background is available.  
For later editions `Auto`, `Tabbed`, and `Acrylic` are also available.

### Automatic change
The background can be changed automatically when changing the colors or the theme of the operating system using the [Watcher](https://github.com/lepoco/wpfui/blob/main/WPFUI/Appearance/Watcher.cs) class.
```cpp
namespace MyApp
{
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();

      Loaded += (sender, args) =>
      {
        WPFUI.Appearance.Watcher.Watch(
          this,                           // Window class
          BackgroundType.Mica, // Background type
          true                            // Whether to change accents automatically
        );
      };
    }
  }
}
```
