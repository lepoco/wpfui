# Themes
WPF UI supports themes. You set the default theme in `App.xaml`, where you enter the direct path to the dictionary.
```xml
<Application
    x:Class="MyNewApp"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    StartupUri="MainWindow.xaml">
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="pack://application:,,,/WPFUI;component/Styles/Theme/Dark.xaml" />
                <ResourceDictionary Source="pack://application:,,,/WPFUI;component/Styles/WPFUI.xaml" />
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
</Application>

```

### Change on the fly
If you want to change the theme while the application is running, you can call the static `Set` method of the `Theme` class.
```cpp
WPFUI.Appearance.Theme.Set(
  WPFUI.Appearance.ThemeType.Light,     // Theme type
  WPFUI.Appearance.BackgroundType.Mica, // Background type
  true                                  // Whether to change accents automatically
);
```

### Automatic change
The theme can be changed automatically when changing the colors or the theme of the operating system using the [Watcher](https://github.com/lepoco/wpfui/blob/main/WPFUI/Appearance/Watcher.cs) class.
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
          Appearance.BackgroundType.Mica, // Background type
          true                            // Whether to change accents automatically
        );
      };
    }
  }
}
```