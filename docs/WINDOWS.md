# Windows
Before we cover non-standard `Window` behavior, we need to understand how the window works, well... in Windows.  
In hidden code layers WPF uses WINAPI to create a native system window. Then, inside it, we can create the content of our application. Each such native window has a number of additional properties that can be modified, like border, minimize button or icon. The window also receives WM (Windows Messages) such as keystrokes or mouse movements. These can also be manually processed to achieve specific effects.

## Default `Window` with transparency
The classic Window control in WPF allows us to set parameters such as size, transparency or name. You can [read more about it here](https://docs.microsoft.com/en-us/dotnet/api/system.windows.window). While transparency matters in the context of Acrylic effect, Mica does not really need it for the entire window, but only for its content.  
Therefore, if we want to get the cool effect of a fully custom window, we should remove the default titlebar and make the background of our content presenter transparent.

One way to do this is to use `WindowChrome`. And then setting the background of our `Window` control to transparent.
```xml
<Window
  Title="WPF UI"
  Background="Transparent"
  mc:Ignorable="d">
  <WindowChrome
    CaptionHeight="1"
    CornerRadius="0"
    GlassFrameThickness="-1"
    NonClientFrameEdges="None"
    ResizeBorderThickness="4"
    UseAeroCaptionButtons="False" />
</Window>
```
Additionally, a few parameters related to margins need to be modified, so if you want an easy window effect without the default titlebar, you can use the preset style.
```xml
<Window
  Title="WPF UI"
  Style="{DynamicResource UiWindow}"
  mc:Ignorable="d">
</Window>
```
As you can see, the above `Window` does not set a transparent background. The reason for this is that if the application is run on Windows 10 and not 11, the background will be just black. Therefore, it is worth using a ready-made function in the **WPF UI** which is Background.Apply.
```cpp
public partial class MyWindow : Window
{
  public MyWindow()
  {
    InitializeComponent();

    WPFUI.Appearance.Background.Apply(this, WPFUI.Appearance.BackgroundType.Mica);
  }
}
```
The above function will check if the used OS is version 11 or higher and will remove the `Window` background automatically.

## Custom `UiWindow` control
In **WPF UI**, you'll find a `UiWindow` control. It has custom styles immediately, but by default it does not use `WindowChrome`. Many of its functions can be forced manually.

For example, you can remove the default menu
```cpp
public partial class MyWindow : WPFUI.Controls.UiWindow
{
  public MyWindow()
  {
    InitializeComponent();

    RemoveTitlebar();
  }
}
```

Or choose a background for Windows 11 (this method is just a shortcut to Background.Apply).
```cpp
public partial class MyWindow : WPFUI.Controls.UiWindow
{
  public MyWindow()
  {
    InitializeComponent();

    ApplyBackdrop(WPFUI.Appearance.BackgroundType.Mica);
  }
}
```

~~~
**NOTE** 
The `UiWindow` control is under development and experimental, please check branch development to stay up to date.
~~~

## Known limitations
You must remember that **WPF UI** is designed for single-window, non-fullscreen applications. If your application has special needs and behaviors, don't use the `Style="{DynamicResource UiWindow}"` and create your own. You can also play with the `UiWindow` control to achieve the desired effect.

Known issues include:
 - Strange window behavior on Windows 7.
 - Problems with maximization.
 - Problems with multi-window applications.