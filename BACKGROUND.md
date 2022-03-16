# Backgrounds
With the help of WPF UI, you can take advantage of the new backgrounds available for Windows 11.  
All you need to do is register your `Window` in the Background class before initialization.

```c#
namespace MyApp
{
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      WPFUI.Appearance.Background.Apply(this, WPFUI.Appearance.BackgroundType.Mica);

      InitializeComponent();
    }
  }
}
```

### Available backgrounds
For the premiere edition of Windows 11, only the `Mica` background is available.  
For later editions `Auto`, `Tabbed`, and `Acrylic` are also available.