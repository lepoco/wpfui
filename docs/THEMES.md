# Themes
**WPF UI** supports themes. You set the default theme in `App.xaml`, with the help of an automatic resources importer.
```xml
<Application
  xmlns:wpfui="http://schemas.lepo.co/wpfui/2022/xaml">
  <Application.Resources>
    <ResourceDictionary>
      <ResourceDictionary.MergedDictionaries>
        <wpfui:Resources Theme="Dark" />
      </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
  </Application.Resources>
</Application>
```

Or, you can add WPF UI resources manually.
```xml
<Application>
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
If you want to change the theme while the application is running, you can call the static `Apply` method of the `Theme` class.
```cpp
WPFUI.Appearance.Theme.Apply(
  WPFUI.Appearance.ThemeType.Light,     // Theme type
  WPFUI.Appearance.BackgroundType.Mica, // Background type
  true                                  // Whether to change accents automatically
);
```

### Automatic change
The theme can be changed automatically when the operating system changes its background or accent using the [Watcher](https://github.com/lepoco/wpfui/blob/main/WPFUI/Appearance/Watcher.cs) class.
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
          WPFUI.Appearance.BackgroundType.Mica, // Background type
          true                            // Whether to change accents automatically
        );
      };
    }
  }
}
```
