![WPF UI Banner Dark](https://user-images.githubusercontent.com/13592821/174165081-9c62d188-ecb6-4200-abd8-419afbaf32c2.png#gh-dark-mode-only)
![WPF UI Banner Light](https://user-images.githubusercontent.com/13592821/174165388-921c4745-90ed-4396-9a4b-9c86478f7447.png#gh-light-mode-only)

# WPF UI

[Created with ❤ in Poland by lepo.co](https://lepo.co/) and [wonderful open-source community](https://github.com/lepoco/wpfui/graphs/contributors)  
WPF UI provides the Fluent experience in your known and loved WPF framework. Intuitive design, themes, navigation and new immersive controls. All natively and effortlessly. Library changes the base elements like `Page`, `ToggleButton` or `List`, and also includes additional controls like `Navigation`, `NumberBox`, `Dialog` or `Snackbar`.

[![Discord](https://img.shields.io/discord/1071051348348514375?label=discord)](https://discord.gg/AR9ywDUwGq) [![GitHub license](https://img.shields.io/github/license/lepoco/wpfui)](https://github.com/lepoco/wpfui/blob/master/LICENSE) [![Nuget](https://img.shields.io/nuget/v/WPF-UI)](https://www.nuget.org/packages/WPF-UI/) [![Nuget](https://img.shields.io/nuget/dt/WPF-UI?label=nuget)](https://www.nuget.org/packages/WPF-UI/)

![ua](https://user-images.githubusercontent.com/13592821/184498735-d296feb8-0f9b-45df-bc0d-b7f0b6f580ed.png)

### Deliver humanitarian aid directly to Ukraine.

https://bank.gov.ua/en/about/humanitarian-aid-to-ukraine

![ua](https://user-images.githubusercontent.com/13592821/184498735-d296feb8-0f9b-45df-bc0d-b7f0b6f580ed.png)

## 🛟 Support plans

To ensure you receive the expert guidance you need, we offer a variety of support plans designed to meet the diverse needs of our community. Whether you are looking to modernize your WPF applications or need assistance with our other libraries, our tailored support solutions are here to help. From priority email support to 24/7 dedicated assistance, we provide flexible plans to suit your project requirements.

[Take a look at the lepo.co support plans](https://lepo.co/support)

## 🤝 Help us keep working on this project

Support the development of WPF UI and other innovative projects by becoming a sponsor on GitHub! Your monthly or one-time contributions help us continue to deliver high-quality, open-source solutions that empower developers worldwide.

[Sponsor WPF UI on GitHub](https://github.com/sponsors/lepoco)

## 🚀 Getting started

For a starter guide see our [documentation](https://wpfui.lepo.co/documentation/).

**WPF UI Gallery** is a free application available in the _Microsoft Store_, with which you can test all functionalities.  
https://apps.microsoft.com/store/detail/wpf-ui/9N9LKV8R9VGM?cid=windows-lp-hero

```powershell
$ winget install 'WPF UI'
```

**WPF UI** is delivered via **NuGet** package manager. You can find the package here:  
https://www.nuget.org/packages/wpf-ui/

**Visual Studio**  
The plugin for **Visual Studio 2022** let you easily create new projects using **WPF UI**.  
https://marketplace.visualstudio.com/items?itemName=lepo.wpf-ui

## 📷 Screenshots

![Demo App Sample](https://user-images.githubusercontent.com/13592821/166259110-0fb98120-fe34-4e6d-ab92-9f72ad7113c3.png)

![Monaco Editor](https://user-images.githubusercontent.com/13592821/258610583-7d71f69d-45b3-4be6-bcb8-8cf6cd60a2ff.png)

![Store App Sample](https://user-images.githubusercontent.com/13592821/165918914-6948fb42-1ee1-4c36-870e-65bb8ffe3c8a.png)

## 🏗️ Works with Visual Studio Designer

![VS2022 Designer Preview](https://user-images.githubusercontent.com/13592821/165919228-0aa3a36c-fb37-4198-835e-53488845226c.png)

## ❤️ Custom Tray icon and menu in pure WPF

![WPF UI Tray menu in WPF](https://user-images.githubusercontent.com/13592821/166259470-2d48a88e-47ce-4f8f-8f07-c9b110de64a5.png)

## ⚓ Custom Windows 11 SnapLayout available for TitleBar.

![WPF UI Snap Layout for WPF](https://user-images.githubusercontent.com/13592821/166259869-e60d37e4-ded4-46bf-80d9-f92c47266f34.png)

## 📖 Documentation

Documentation can be found at https://wpfui.lepo.co/. We also have a [tutorial](#-getting-started) over there for newcomers.

## 🚧 Development

If you want to propose a new functionality or submit a bugfix, create a [Pull Request](https://github.com/lepoco/wpfui/compare/main...main) for the branch [main](https://github.com/lepoco/wpfui/tree/main).

## 📐 How to use?

First, your application needs to load custom styles, add in the **MyApp\App.xaml** file:

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

If your application does not have **MyApp\App.xaml** file, use `ApplicationThemeManager.Apply(frameworkElement)` to apply/update the theme resource in the `frameworkElement`.

```C#
public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        ApplicationThemeManager.Apply(this);
    }
}
```

Now you can create fantastic apps, e.g. with one button:

```xml
<ui:FluentWindow
  ...
  xmlns:ui="http://schemas.lepo.co/wpfui/2022/xaml">
  <StackPanel>
      <ui:TitleBar Title="WPF UI"/>
      <ui:Card Margin="8">
          <ui:Button Content="Hello World" Icon="{ui:SymbolIcon Fluent24}" />
      </ui:Card>
  </StackPanel>
</ui:FluentWindow>
```

## Special thanks

Crafting apps for .NET without the creators of tools like ReSharper or XAML Styler would never be such a fantastic adventure.

- [🔗 JetBrains ReSharper](https://www.jetbrains.com/resharper/)
- [🔗 XAML Styler](https://github.com/Xavalon/XamlStyler)

JetBrains was kind enough to lend a license for the open-source **dotUltimate** for WPF UI development. Learn more here:

- https://www.jetbrains.com/dotnet/
- https://www.jetbrains.com/opensource/

## Microsoft Property

Design of the interface, choice of colors and the appearance of the controls were inspired by projects made by Microsoft for Windows 11.  
The Wpf.Ui.Gallery app includes icons from _Microsoft WinUI 3 Gallery_ app. They are used here as an example of creating tools for Microsoft systems.

## Segoe Fluent Icons

**WPF UI** uses Fluent System Icons. Although this font was also created by Microsoft, it does not contain all the icons for Windows 11. If you need the missing icons, add Segoe Fluent Icons to your application.  
According to the EULA of Segoe Fluent Icons we cannot ship a copy of it with this dll. Segoe Fluent Icons is installed by default on Windows 11, but if you want these icons in an application for Windows 10 and below, you must manually add the font to your application's resources.  
[https://docs.microsoft.com/en-us/windows/apps/design/style/segoe-fluent-icons-font](https://docs.microsoft.com/en-us/windows/apps/design/style/segoe-fluent-icons-font)  
[https://docs.microsoft.com/en-us/windows/apps/design/downloads/#fonts](https://docs.microsoft.com/en-us/windows/apps/design/downloads/#fonts)

In the app dictionaries, you can add an alternate path to the font

```XML
<FontFamily x:Key="SegoeFluentIcons">pack://application:,,,/;component/Fonts/#Segoe Fluent Icons</FontFamily>
```

## Code of Conduct

This project has adopted the code of conduct defined by the Contributor Covenant to clarify expected behavior in our community.

## License

**WPF UI** is free and open source software licensed under **MIT License**. You can use it in private and commercial projects.  
Keep in mind that you must include a copy of the license in your project.
