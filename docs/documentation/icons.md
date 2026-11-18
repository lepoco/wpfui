# Fluent System Icons

Fluent System Icons is a set of icons that is designed to be used with Microsoft's Fluent Design System. It is a collection of over 1,500 icons that are designed to be modern, consistent, and scalable, and can be used in a variety of applications and platforms, including web and mobile applications.

The Fluent System Icons set includes a range of icons, such as those for basic navigation, media playback, communication, and more. The icons are available in various sizes, from 16x16 to 512x512 pixels, and are provided in vector format, allowing for easy scaling and customization.

Fluent System Icons is available for free and can be downloaded from the official Microsoft website. It is also open source, meaning that developers can contribute to the icon set or create their own custom icons based on the existing ones.  
[Fluent UI System Icons](https://github.com/microsoft/fluentui-system-icons)

**WPF UI** uses Fluent UI System Icons in most of the graphical controls.

## Getting started

Icons are displayed by using the font that comes with the library. All glyphs are mapped to the [SymbolRegular](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Controls/SymbolRegular.cs) and [SymbolFilled](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Controls/SymbolFilled.cs) enums.

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
