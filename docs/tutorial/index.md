# Tutorial

**WPF UI** is a library built for [Windows Presentation Foundation (WPF)](https://docs.microsoft.com/en-us/visualstudio/designers/getting-started-with-wpf) and the [C#](https://docs.microsoft.com/en-us/dotnet/csharp/) language.  
To be able to work with them comfortably, you will need:

- [Visual Studio 2022 Community Edition](https://visualstudio.microsoft.com/vs/community/)
- .NET desktop development  
  _(Additional workload in Visual Studio)_

![NET development package](https://user-images.githubusercontent.com/13592821/191967842-118b8dc2-fb33-49c1-b9a9-162669b6e110.png)

> [!NOTE]
> Visual Studio 2022 and Visual Studio Code are two different programs. If you want to create WPF apps, it's possible to compile them in Visual Studio Code, however for comfortable work we recommend [Visual Studio 2022](https://visualstudio.microsoft.com/vs/community/) or [JetBrains Rider](https://www.jetbrains.com/rider/).

## Installation

You can install **WPF UI**, the library for the Windows Presentation Foundation framework, in several ways.

- Directly specify the `Wpf.Ui.dll` file in your application's project file (`.csproj`).
- Copy the library source code into your application codebase.
- Use the **NuGet** package manager.

We recommend using the **NuGet** package manager, it allows you to easily install and update your application dependencies.  
More information on how to install **WPF UI** using **NuGet** [can be found here](/tutorial/nuget.html).

## Extension for Visual Studio

Creators of **WPF UI** have prepared a special plugin that will automatically create a project based on **WPF UI**, Dependency Injection and MVVM, thanks to which you will quickly and easily start a new apps.

[Learn more about the WPF UI plug-in for Visual Studio 2022](/tutorial/extension.html)

## Getting started

Once you have chosen how to install **WPF UI**, you can move on to creating your first app, more on this in [Getting Started](/tutorial/getting-started.html).
