# Visual Studio 2022 Extension for WPF UI

Visual Studio allows you to add extensions that can be installed in several ways:

- Build them locally and then install the `.vsix` package.
- Download the extension from the internet and install the `.vsix` file.
- Install the extension using the search in _Visual Studio_.

In this tutorial, we'll cover the last way, if you want to know more, check out [**Manage extensions for Visual Studio**](https://learn.microsoft.com/en-us/visualstudio/ide/finding-and-using-visual-studio-extensions?view=vs-2022).

In any case, if you want to download a plugin and install it manually, or leave your review, you can find it in the Visual Studio Marketplace:  
https://marketplace.visualstudio.com/items?itemName=lepo.wpf-ui

> [!NOTE]
> The source code for **WPF UI** _Visual Studio 2022_ extension is public and you can [check it out here](https://github.com/lepoco/wpfui/tree/development/src/Wpf.Ui.Extension/Wpf.Ui.Extension).

## How to?

1.  Install Visual Studio 2022 from [Visual Studio 2022 downloads](https://visualstudio.microsoft.com/downloads/).
2.  After installation, open Visual Studio
3.  Expand the _Extensions_ tab in the menu and then click _Manage Extensions_  
    ![Extensions tab in Visual Studio](https://user-images.githubusercontent.com/13592821/192057892-39ae96f8-ba25-4fb8-a081-0b8d530f79bf.png)
4.  In the _Online_ tab, use the search engine to enter _WPF-UI_ in it, then click _Download_  
    ![Online tab in Extension Manager for Visual Studio](https://user-images.githubusercontent.com/13592821/192058027-44929773-548d-4ae1-a6e4-e922c04e82e8.png)
5.  After downloading, restart _Visual Studio_
6.  After restarting, you will see a window asking you to confirm the installation.  
    ![Confirm Visual Studio Installation](https://user-images.githubusercontent.com/13592821/192058231-c5587473-a44d-4046-a6ad-8cd0a3cdc9df.png)
7.  Once installed, you can restart _Visual Studio_ and click _Create new project_  
    ![Create new project](https://user-images.githubusercontent.com/13592821/192058452-f1f9005c-4d40-482a-96fb-5dccbafb4102.png)
8.  In the top right corner, you can select the **WPF UI** project type  
    ![Project type filter](https://user-images.githubusercontent.com/13592821/192058531-186b0eba-14c0-4761-9781-dd8880e2763a.png)
9.  In the top right corner, you can select the project type **WPF UI**

## Done!

After creating a project, you can familiarize yourself with its structure and proceed to further steps.

- [WPF UI - Getting started](/tutorial/getting-started.html)
- [Introduction to the MVVM Toolkit](https://learn.microsoft.com/en-us/windows/communitytoolkit/mvvm/introduction)
- [.NET Generic Host in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-6.0)
