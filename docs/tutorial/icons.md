# Icons

**WPF UI** uses [Fluent UI System Icons](https://github.com/microsoft/fluentui-system-icons), a collection of familiar, friendly and modern icons from Microsoft.

## Getting started

Icons are displayed by using the font that comes with the library. All glyphs are mapped to the [SymbolRegular](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Common/SymbolRegular.cs) and [SymbolFilled](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Common/SymbolFilled.cs) enums.

Icon controls and fonts will be automatically added to your application if you add `ControlsDictionary` in the **App.xaml** file:

```xml
<Application
    ...
    xmlns:ui="http://schemas.lepo.co/wpfui/2022/xaml">
    <Application.Resources>
        <ui:ThemesDictionary Theme="Dark" />
        <ui:ControlsDictionary />
    </Application.Resources>
</Application>
```

> [!NOTE]
> You can find out how the Control Dictionary works here

# SymbolIcon

`SymbolIcon` is a control responsible for rendering icons.

### Implementation

```csharp
class Wpf.Ui.Controls.SymbolIcon
```

## Exposes

```csharp
// Gets or sets displayed symbol
SymbolIcon.Symbol = SymbolRegular.Empty;
```

```csharp
// Defines whether or not we should use the SymbolFilled
SymbolIcon.Filled = false;
```

```csharp
// Icon foreground
SymbolIcon.Foreground = Brushes.White;
```

```csharp
// Icon size
SymbolIcon.FontSize = 16;
```

### How to use

```xml
<ui:SymbolIcon
  Symbol="Fluent24"
  Filled="False"
  FontSize="16"
  Foreground="White"/>
```

# FontIcon

`FontIcon` is a control responsible for rendering icons based on the provided font.

### Implementation

```csharp
class Wpf.Ui.Controls.FontIcon
```

## Exposes

```csharp
// Gets or sets displayed glyph
FontIcon.Glyph = '\uE00B';
```

```csharp
// Gets or sets used font family
FontIcon.FontFamily = "Segoe Fluent Icons";
```

```csharp
// Icon foreground
FontIcon.Foreground = Brushes.White;
```

```csharp
// Icon size
FontIcon.FontSize = 16;
```

### How to use

```xml
<ui:FontIcon
  Glyph="&#xe00b;"
  FontFamily="{DynamicResource SegoeFluentIcons}"
  FontSize="16"
  Foreground="White"/>
```

## Segoe Fluent Icons

Not all icons available in WinUi 3 are in **Fluent UI System Icons**. Some of them require the **Segoe Fluent Icons** font.  
According to the EULA of Segoe Fluent Icons we cannot ship a copy of it with this dll. Segoe Fluent Icons is installed by default on Windows 11, but if you want these icons in an application for Windows 10 and below, you must manually add the font to your application's resources.  
[https://docs.microsoft.com/en-us/windows/apps/design/style/segoe-fluent-icons-font](https://docs.microsoft.com/en-us/windows/apps/design/style/segoe-fluent-icons-font)  
[https://docs.microsoft.com/en-us/windows/apps/design/downloads/#fonts](https://docs.microsoft.com/en-us/windows/apps/design/downloads/#fonts)

In the `App.xaml` dictionaries, you can add an alternate path to the font

```xml
<Application
    ...
    xmlns:ui="http://schemas.lepo.co/wpfui/2022/xaml">
    <Application.Resources>
        <ui:ThemesDictionary Theme="Dark" />
        <ui:ControlsDictionary />

        <FontFamily x:Key="SegoeFluentIcons">pack://application:,,,/;component/Fonts/#Segoe Fluent Icons</FontFamily>
    </Application.Resources>
</Application>
```
