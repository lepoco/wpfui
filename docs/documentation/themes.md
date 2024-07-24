# Themes

**WPF UI** supports themes. You set the default theme in `App.xaml`, with the help of an automatic resources importer.

```xml
<Application
  ...
  xmlns:ui="http://schemas.lepo.co/wpfui/2022/xaml">
  <Application.Resources>
    <ResourceDictionary>
      <ResourceDictionary.MergedDictionaries>
        <ui:ThemesDictionary Theme="Dark" />
        <ui:ControlsDictionary />
      </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
  </Application.Resources>
</Application>
```

Or, you can add **WPF UI** resources manually.

```xml
<Application>
  <Application.Resources>
    <ResourceDictionary>
      <ResourceDictionary.MergedDictionaries>
        <ResourceDictionary Source="pack://application:,,,/Wpf.Ui;component/Resources/Theme/Dark.xaml" />
        <ResourceDictionary Source="pack://application:,,,/Wpf.Ui;component/Resources/Wpf.Ui.xaml" />
      </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
  </Application.Resources>
</Application>
```

### Change on the fly

If you want to change the theme while the application is running, you can call the static `Apply` method of the `Theme` class.

```csharp
Wpf.Ui.Appearance.ApplicationThemeManager.Apply(
  Wpf.Ui.Appearance.ApplicationTheme.Light, // Theme type
  Wpf.Ui.Controls.WindowBackdropType.Mica,  // Background type
  true                                      // Whether to change accents automatically
);
```

### Automatic change

The theme can be changed automatically when the operating system changes its background or accent using the [SystemThemeWatcher](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Appearance/SystemThemeWatcher.cs) class.

```csharp
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
