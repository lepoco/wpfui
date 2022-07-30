# Tutorial

**WPF UI** is a library built for [WPF](https://docs.microsoft.com/en-us/visualstudio/designers/getting-started-with-wpf) and the [C#](https://docs.microsoft.com/en-us/dotnet/csharp/) language. To be able to work with them comfortably, you will need [Visual Studio Community Edition](https://visualstudio.microsoft.com/vs/community/) _(NOT VISUAL STUDIO CODE)_.

- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/community/)
- .NET desktop development package _(via VS2022 installer)_

## Extension
One of the easiest ways to create a new project using *WPF UI* is to use the plug-in for _Visual Studio 2022_.  
https://marketplace.visualstudio.com/items?itemName=lepo.wpf-ui

![wpfui-template](https://user-images.githubusercontent.com/13592821/181920257-1e7bca97-e20c-4324-bf55-d3433a6684a8.png)

## Get a package

The first thing you need to do is install the WPF UI via the package manager.  
To do so, in your new WPF project, right-click on **Dependencies** and **Manage NuGet Packages**

Type **WPF-UI** in the search and when the correct result appears - click **Install**.

![image](https://user-images.githubusercontent.com/13592821/181920201-892f0e88-39b7-4028-8519-0191532c774d.png)


## Adding dictionaries

[XAML](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/xaml/?view=netdesktop-6.0), and hence WPF, operate on resource dictionaries. These are HTML-like files that describe the appearance and various aspects of the [controls](https://wpfui.lepo.co/documentation/controls).  
**WPF UI** adds its own sets of these files to tell the application how the controls should look like.

There should be a file called `App.xaml` in your new application. Add new dictionaries to it using **WPF UI** `Resources` class:

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

You can choose a color theme here,  
`Light` or `Dark`.

## The main window

At the design stage, we decided not to create ready-made [Window](https://docs.microsoft.com/en-us/dotnet/api/system.windows.window?view=windowsdesktop-6.0) templates, so you can design everything, including [TitleBar](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/TitleBar.cs), to your liking. This takes a little more work at the beginning, but allows you to have more control over application look.

First, let's modify MainWindow.xaml

```xml
<ui:UiWindow
  ...
  xmlns:pages="clr-namespace:MyNewApp.Pages"
  xmlns:ui="http://schemas.lepo.co/wpfui/2022/xaml"
  Background="{ui:ThemeResource ApplicationBackgroundBrush}"
  ExtendsContentIntoTitleBar="True"
  WindowBackdropType="Mica"
  WindowCornerPreference="Round"
  WindowStartupLocation="CenterScreen">
  <Grid>
    <Grid.RowDefinitions>
      <RowDefinition Height="Auto" />
      <RowDefinition Height="*" />
    </Grid.RowDefinitions>

    <Grid
        x:Name="RootMainGrid"
        Grid.Row="1"
        Margin="0">
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="Auto" />
            <ColumnDefinition Width="*" />
        </Grid.ColumnDefinitions>

        <!--  This is the main navigation of the application.  -->
        <ui:NavigationStore
            x:Name="RootNavigation"
            Grid.Column="0"
            Margin="6,0,6,0"
            Frame="{Binding ElementName=RootFrame, Mode=OneWay}"
            Navigated="RootNavigation_OnNavigated"
            SelectedPageIndex="0">
            <ui:NavigationStore.Items>
                <ui:NavigationItem
                    Content="Home"
                    Icon="Home24"
                    PageType="{x:Type pages:Dashboard}"
                    PageTag="dashboard" />
            </ui:NavigationStore.Items>
            <ui:NavigationStore.Footer>
              <ui:NavigationItem
                    Content="Settings"
                    Icon="Diversity24"
                    PageType="{x:Type pages:Settings}" />
                <!--  A navigation element that does not point to the page can be used as a button.  -->
                <ui:NavigationItem
                    Click="NavigationButtonTheme_OnClick"
                    Content="Theme"
                    Icon="DarkTheme24" />
            </ui:NavigationStore.Footer>
        </ui:NavigationStore>

        <!--  We display our pages inside this element.  -->
        <Border
            Grid.Column="1"
            Background="{ui:ThemeResource ControlFillColorDefaultBrush}"
            CornerRadius="8,0,0,0">
            <Grid>
                <Grid.RowDefinitions>
                    <RowDefinition Height="Auto" />
                    <RowDefinition Height="*" />
                </Grid.RowDefinitions>
                <Frame x:Name="RootFrame" Grid.Row="1" />
                <ui:Breadcrumb
                    Grid.Row="0"
                    Margin="18"
                    HorizontalAlignment="Left"
                    VerticalAlignment="Top"
                    FontSize="24"
                    Navigation="{Binding ElementName=RootNavigation, Mode=OneWay}" />
            </Grid>
        </Border>
    </Grid>

    <!--  The title bar contains window navigation elements and some Tray related extras.  -->
    <!--  You can put additional controls in the header, such as a search bar.  -->
    <!--  <ui:TitleBar.Header />  -->
    <ui:TitleBar
      Title="WPF UI - Fluent design system"
      Grid.Row="0">
      <ui:TitleBar.Tray>
        <ui:NotifyIcon
          FocusOnLeftClick="True"
          MenuOnRightClick="True"
          TooltipText="WPF UI">
          <ui:NotifyIcon.Menu>
             <ContextMenu>
               <ui:MenuItem
                 Header="Home"
                 SymbolIcon="Library28"
                 Tag="home" />
            </ContextMenu>
          </ui:NotifyIcon.Menu>
        </ui:NotifyIcon>
      </ui:TitleBar.Tray>
    </ui:TitleBar>
  </Grid>
</Window>

```

Things have changed a bit, so let's go over what is what.

#### WPF UI Namespace

This line tells the interpreter that we will be using the **WPF UI** controls under the **ui:** abbreviation.  
Additionally, we use a modified `UiWindow` instead of the default window control.

```xml
<ui:UiWindow
  xmlns:ui="http://schemas.lepo.co/wpfui/2022/xaml" />
```

#### Pages Namespace

This line informs that in the given directory there are files of our pages. They will be displayed by the navigation.

```xml
<ui:UiWindow
  xmlns:pages="clr-namespace:MyNewApp.Pages" />
```

#### Mica Background

Using the modified attributes of the `UiWindow` class, we extend the content of the window to the entire workspace, and then apply the Mica effect for Windows 11 and above.

```xml
<ui:UiWindow
  ExtendsContentIntoTitleBar="True"
  WindowBackdropType="Mica" />
```

#### Navigation

The `ui:NavigationStore` control is responsible managing the displayed pages. The [Page](https://docs.microsoft.com/en-us/dotnet/api/system.windows.controls.page) is displayed inside the [Frame](https://docs.microsoft.com/en-us/dotnet/api/system.windows.controls.frame).  
As you can see in the example below, the navigation indicates which `Frame` will display pages.

```xml
<ui:NavigationStore
  Frame="{Binding ElementName=RootFrame, Mode=OneWay}"/>

<Frame
  x:Name="RootFrame" />
```

### Breadcrumb

Breadcrumb is a small navigation aid, it automatically displays the title of the currently used [Page](https://docs.microsoft.com/en-us/dotnet/api/system.windows.controls.page) based on its name given in the navigation. As you can see in the example above, Breadcrumb has indicated which navigation control it should use

```xml
<ui:NavigationStore
  x:Name="RootNavigation"/>

<ui:Breadcrumb
  Navigation="{Binding ElementName=RootNavigation, Mode=OneWay}" />
```

### TitleBar

The [TitleBar](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Controls/TitleBar.cs) includes minimize, maximize, and close buttons, and lets you drag the application across the desktop by grabbing at its top bar.  
`TitleBar` is a powerful control and allows you to control aspects such as the [Tray](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Tray/NotifyIcon.cs) icon/menu or [SnapLayout](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Controls/SnapLayout.cs).

```xml
<ui:TitleBar
  Title="WPF UI - Fluent design system"
  Grid.Row="0">
  <ui:TitleBar.Tray>
    <ui:NotifyIcon
      FocusOnLeftClick="True"
      MenuOnRightClick="True"
      TooltipText="WPF UI">
      <ui:NotifyIcon.Menu>
          <ContextMenu>
            <ui:MenuItem
              Header="Home"
              SymbolIcon="Library28"
              Tag="home" />
        </ContextMenu>
      </ui:NotifyIcon.Menu>
    </ui:NotifyIcon>
  </ui:TitleBar.Tray>
</ui:TitleBar>
```
