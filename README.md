# WPF UI
[Created with ❤ in Poland by lepo.co](https://dev.lepo.co/)  
A simple way to make your application written in WPF keep up with modern design trends. Library changes the base elements like Page, ToggleButton or List, and also includes additional controls like Navigation, NumberBox, Dialog or Snackbar.

[![GitHub license](https://img.shields.io/github/license/lepoco/wpfui)](https://github.com/lepoco/wpfui/blob/master/LICENSE) [![Nuget](https://img.shields.io/nuget/v/WPF-UI)](https://www.nuget.org/packages/WPF-UI/) [![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/WPF-UI?label=nuget-pre)](https://www.nuget.org/packages/WPF-UI/) [![Nuget](https://img.shields.io/nuget/dt/WPF-UI?label=nuget-downloads)](https://www.nuget.org/packages/WPF-UI/) [![Size](https://img.shields.io/github/repo-size/lepoco/wpfui)](https://github.com/lepoco/wpfui) [![Sponsors](https://img.shields.io/github/sponsors/lepoco)](https://github.com/sponsors/lepoco)

## 📁 What's included?
| Name| Framework | Build Status |
| --- | --- | --- | 
| **WPFUI**<br />Library that allows you to use all features in your own application | [![NET6](https://img.shields.io/badge/.NET-6.0-red)](https://github.com/lepoco/wpfui/blob/main/WPFUI/WPFUI.csproj) [![NET5](https://img.shields.io/badge/.NET-5.0-blue)](https://github.com/lepoco/wpfui/blob/main/WPFUI/WPFUI.csproj)<br/>[![NETCore3](https://img.shields.io/badge/.NET%20Core-3.1-brightgreen)](https://github.com/lepoco/wpfui/blob/main/WPFUI/WPFUI.csproj)<br/>[![NETFramework48](https://img.shields.io/badge/.NET%20Framework-4.8-orange)](https://github.com/lepoco/wpfui/blob/main/WPFUI/WPFUI.csproj)<br/>[![NETFramework47](https://img.shields.io/badge/.NET%20Framework-4.7-orange)](https://github.com/lepoco/wpfui/blob/main/WPFUI/WPFUI.csproj)<br/>[![NETFramework46](https://img.shields.io/badge/.NET%20Framework-4.6-orange)](https://github.com/lepoco/wpfui/blob/main/WPFUI/WPFUI.csproj) | [![Build status](https://github.com/lepoco/wpfui/workflows/CI/badge.svg)](https://github.com/lepoco/wpfui/actions) | 
| **WPFUI.Demo**<br />An application written in WPF .NET 6 where you can test the features. | [![NET6win](https://img.shields.io/badge/.NET-6.0--windows-red)](https://github.com/lepoco/wpfui/blob/main/WPFUI.Demo/WPFUI.Demo.csproj) | [![Build status](https://github.com/lepoco/wpfui/workflows/CI/badge.svg)](https://github.com/lepoco/wpfui/actions) |

## 📷 Screenshots
![Screen-4](https://github.com/lepoco/wpfui/blob/main/.github/assets/screen-4.png?raw=true)

![Screen-1](https://github.com/lepoco/wpfui/blob/main/.github/assets/screen-1.png?raw=true)

![Screen-5](https://github.com/lepoco/wpfui/blob/main/.github/assets/screen-5.png?raw=true)

## 🕹️ Radiograph
Radiograph is an application written by me that uses WPF UI.

![Screen-6](https://github.com/lepoco/wpfui/blob/main/.github/assets/screen-6.png?raw=true)

[<img src="https://github.com/lepoco/wpfui/blob/main/.github/assets/microsoft-badge.png?raw=true" width="160">](https://www.microsoft.com/en-us/p/radiograph/9nh1p86h06cg?activetab=pivot:overviewtab)

## 🛠️ Custom controls
| Control | Namespace | Description |
| --- | --- | --- |
| **NumberBox** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/NumberBox.cs) | Text field for entering numbers with the possibility of setting a mask. |
| **Button** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/Button.cs) | Custom button with additional parameters like an icon. |
| **Card** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/Card.cs) | Simple card compatible with the theme for displaying other elements. |
| **CardAction** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/CardAction.cs) | Inherited from the Button interactive card styled according to Fluent Design. |
| **CardExpander** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/CardExpander.cs) | Inherited from the ContentControl control which can hide the collapsable content. |
| **CardControl** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/CardControl.cs) | Inherited from the Button control which displays an additional control on the right side of the card. |
| **CardProfile** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/CardProfile.cs) | Simple element that displays an image in a circular frame like in default applications for Windows 11. |
| **CodeBlock** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/CodeBlock.cs) | Formats syntax and display a fragment of the source code. |
| **Dialog** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/Dialog.cs) | Displays a large card with a slightly transparent background and two action buttons. |
| **Snackbar** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/Snackbar.cs) | Animated card with a notification displayed at the bottom of the application. |
| **FontIcon** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/FontIcon.cs) | Represents a text element containing an icon glyph with selectable font family. |
| **Icon** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/Icon.cs) | Represents a text element containing an icon glyph. |
| **Hyperlink** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/Hyperlink.cs) | Button that opens a URL in a web browser. |
| **Navigation** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/Navigation.cs) | Navigation styled as UWP apps. |
| **NavigationStore** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/NavigationStore.cs) | Navigation styled as Windows 11 Store app |
| **NavigationFluent** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/NavigationFluent.cs) | Navigation styled as Windows 11 Settings app. |
| **Breadcrumb** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/Breadcrumb.cs) | Automatic display of the page title from the navigation in the application. |
| **Rating** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/Rating.cs) | Stars to display the rating. |
| **MessageBox** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/MessageBox.cs) | Custom window to display notifications outside the application. |
| **TitleBar** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/TitleBar.cs) | A set of buttons that can replace the default window navigation, giving it a new, modern look with implemented [NotifyIcon](https://github.com/lepoco/wpfui/blob/main/WPFUI/Tray/NotifyIcon.cs). |

## 🧩 Custom classes and tools
| Class | Namespace | Description |
| --- | --- | --- |
| **Manager** | [WPFUI.Background](https://github.com/lepoco/wpfui/blob/main/WPFUI/Background/Manager.cs) | Allows to add background effects like Mica or Acrylic. |
| **Manager** | [WPFUI.Theme](https://github.com/lepoco/wpfui/blob/main/WPFUI/Theme/Manager.cs) | Allows to manage available color themes from the library. |
| **Watcher** | [WPFUI.Theme](https://github.com/lepoco/wpfui/blob/main/WPFUI/Theme/Watcher.cs) | Listens for **SystemParameters** changes while waiting for **StaticPropertyChanged** to change, then switches theme with **Manager.Switch**. |
| **Progress** | [WPFUI.Taskbar](https://github.com/lepoco/wpfui/blob/main/WPFUI/Taskbar/Progress.cs) | Allows to change the status of the displayed notification in the application icon on the TaskBar. |
| **NotifyIcon** | [WPFUI.Tray](https://github.com/lepoco/wpfui/blob/main/WPFUI/Tray/NotifyIcon.cs) | It allows you to create an icon and a menu in the tray. |

## 🖌️ XAML styles for use in the application.
| Resource usage | Description |
| --- | --- |
| `<Window Style="{StaticResource UiWindow}"/>` | Add a custom appearance to the window and removes the navigation buttons. |
| `<Page Style="{StaticResource UiPage}"/>` | Add a custom appearance to the page. |
| `<Page Style="{StaticResource UiPageScrollable}"/>` | Add a custom appearance to the page and automatic display of scrolling if the content is too long. |

## 📐 How to use?
First, your application needs to load custom styles, add in the **MyApp\App.xaml** file:
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

Now, you can customize your views, for example by adding a non-standard look to the main window and navigation buttons
```xml
<Window
  xmlns:wpfui="clr-namespace:WPFUI.Controls;assembly=WPFUI"
  Style="{StaticResource UiWindow}">
  <Grid>
    <Grid.RowDefinitions>
      <RowDefinition Height="Auto" />
      <RowDefinition Height="*" />
    </Grid.RowDefinitions>

    <wpfui:TitleBar Grid.Row="0" ApplicationNavigation="True" />

    <Grid Grid.Row="1" Margin="12,6,12,12">
      <Grid.ColumnDefinitions>
        <ColumnDefinition Width="Auto" />
        <ColumnDefinition Width="*" />
      </Grid.ColumnDefinitions>

      <wpfui:NavigationFluent Grid.Column="0" Margin="0,0,12,0" />

      <Frame Grid.Column="1" />
    </Grid>
  </Grid>
</Window>
```

## Special thanks
Crafting apps for .NET without the creators of tools like ReSharper or XAML Styler would never be such a fantastic adventure.
 - [🔗 JetBrains ReSharper](https://www.jetbrains.com/resharper/)
 - [🔗 XAML Styler](https://github.com/Xavalon/XamlStyler)

## Microsoft Property
Design of the interface, choice of colors and the appearance of the controls were inspired by projects made by Microsoft for Windows 11.  
The WPFUI.Demo app includes icons from Shell32 for Windows 11. These icons are the legal property of Microsoft and you may not use them in your own app without permission. They are used here as an example of creating tools for Microsoft systems.

## Segoe Fluent Icons
According to the EULA of Segoe Fluent Icons we cannot ship a copy of it with this dll. Segoe Fluent Icons is installed by default on Windows 11, but if you want these icons in an application for Windows 10 and below, you must manually add the font to your application's resources.  
[https://docs.microsoft.com/en-us/windows/apps/design/style/segoe-fluent-icons-font](https://docs.microsoft.com/en-us/windows/apps/design/style/segoe-fluent-icons-font)  
[https://docs.microsoft.com/en-us/windows/apps/design/downloads/#fonts](https://docs.microsoft.com/en-us/windows/apps/design/downloads/#fonts)

In the app dictionaries, you can add an alternate path to the font
```XML
<FontFamily x:Key="SegoeFluentIcons">pack://application:,,,/;component/Fonts/#Segoe Fluent Icons</FontFamily>
```

## Compilation
Use Visual Studio 2022 and invoke the .sln.

Visual Studio  
**WPF UI** is an Open Source project. You are entitled to download and use the freely available Visual Studio Community Edition to build, run or develop for WPF UI. As per the Visual Studio Community Edition license, this applies regardless of whether you are an individual or a corporate user.

## License
WPF UI is free and open source software licensed under **MIT License**. You can use it in private and commercial projects.  
Keep in mind that you must include a copy of the license in your project.
