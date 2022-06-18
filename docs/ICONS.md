# Icons
**WPF UI** uses [Fluent UI System Icons](https://github.com/microsoft/fluentui-system-icons), a collection of familiar, friendly and modern icons from Microsoft.  

### Source
Icons are displayed by using the font that comes with the library. All glyphs are mapped to the [Icon](https://github.com/lepoco/wpfui/blob/main/WPFUI/Common/Icon.cs) and [IconFilled](https://github.com/lepoco/wpfui/blob/main/WPFUI/Common/IconFilled.cs) enums.

### Usage
Many interface elements use icons via simple parameters, for example you can add an icon to a button with the Icon parameter.
```xml
<wpfui:Button
  Icon="FoodCake24"
  Content="The cake is a lie!"/>
```

### SymbolIcon Control
You can simply display the icon with `SymbolIcon` control:
```xml
<wpfui:SymbolIcon
  Symbol="FoodCake24"
  Filled="True"/>
```

### FontIcon Control
If you add a custom font like **Segoe Fluent Icons** to your application, you can use it with `FontIcon`:
```xml
<wpfui:FontIcon
  Glyph="&#xE700;"
  FontFamily="{DynamicResource SegoeFluentIcons}"/>
```

### Segoe Fluent Icons
Not all icons available in WinUi 3 are in **Fluent UI System Icons**. Some of them require the **Segoe Fluent Icons** font.  
According to the EULA of Segoe Fluent Icons we cannot ship a copy of it with this dll. Segoe Fluent Icons is installed by default on Windows 11, but if you want these icons in an application for Windows 10 and below, you must manually add the font to your application's resources.  
[https://docs.microsoft.com/en-us/windows/apps/design/style/segoe-fluent-icons-font](https://docs.microsoft.com/en-us/windows/apps/design/style/segoe-fluent-icons-font)  
[https://docs.microsoft.com/en-us/windows/apps/design/downloads/#fonts](https://docs.microsoft.com/en-us/windows/apps/design/downloads/#fonts)

In the `App.xaml` dictionaries, you can add an alternate path to the font
```XML
<FontFamily x:Key="SegoeFluentIcons">pack://application:,,,/;component/Fonts/#Segoe Fluent Icons</FontFamily>
```