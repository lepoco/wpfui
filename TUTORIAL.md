# Tutorial
WPF UI is a library built for [WPF](https://docs.microsoft.com/en-us/visualstudio/designers/getting-started-with-wpf) and the [C#](https://docs.microsoft.com/en-us/dotnet/csharp/) language. To be able to work with them comfortably, you will need [Visual Studio Community Edition](https://visualstudio.microsoft.com/vs/community/) *(NOT VISUAL STUDIO CODE)*.

 - [Visual Studio 2022](https://visualstudio.microsoft.com/vs/community/)
 - .NET desktop development package *(via VS2022 installer)*

## Get a package
The first thing you need to do is install the WPF UI via the package manager.  
To do so, in your new WPF project, right-click on **Dependencies** and **Manage NuGet Packages**

Type **WPF-UI** in the search and when the correct result appears - click **Install**.

![image](https://user-images.githubusercontent.com/13592821/158079885-7715b552-bbc6-4574-bac9-92ecb7b161d8.png)

## Adding dictionaries
[XAML](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/xaml/?view=netdesktop-6.0), and hence WPF, operate on resource dictionaries. These are HTML-like files that describe the appearance and various aspects of the [controls](https://wpfui.lepo.co/documentation/controls).  
**WPF UI** adds its own sets of these files to tell the application how the controls should look like.

There should be a file called `App.xaml` in your new application. Add new dictionaries to it:

```xml
<Application
    x:Class="MyNewApp"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    StartupUri="MainWindow.xaml">
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary
                  Source="pack://application:,,,/WPFUI;component/Styles/Theme/Dark.xaml" />
                <ResourceDictionary
                  Source="pack://application:,,,/WPFUI;component/Styles/WPFUI.xaml" />
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
</Application>

```

You can choose a color theme here,
`Light.xaml` or `Dark.xaml`

## The main window
At the design stage, we decided not to create ready-made [Window](https://docs.microsoft.com/en-us/dotnet/api/system.windows.window?view=windowsdesktop-6.0) templates, so you can design everything, including [TitleBar](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/TitleBar.cs), to your liking. This takes a little more work at the beginning, but allows you to have more control over application look.

First, let's modify MainWindow.xaml

```xml
<Window
  x:Class="MyNewApp.MainWindow"
  xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
  xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
  xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
  xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
  xmlns:pages="clr-namespace:MyNewApp.Pages"
  xmlns:wpfui="clr-namespace:WPFUI.Controls;assembly=WPFUI"
  Title="My New App"
  Style="{StaticResource UiWindow}"
  WindowStartupLocation="CenterScreen"
  mc:Ignorable="d">
  <Grid>
    <Grid.RowDefinitions>
      <RowDefinition Height="Auto" />
      <RowDefinition Height="*" />
    </Grid.RowDefinitions>

    <Grid Grid.Row="1" Margin="0">
      <Grid.ColumnDefinitions>
        <ColumnDefinition Width="Auto" />
        <ColumnDefinition Width="*" />
      </Grid.ColumnDefinitions>

      <wpfui:NavigationStore
        x:Name="RootNavigation"
        Grid.Column="0"
        Margin="6,0,6,0"
        Frame="{Binding ElementName=RootFrame}">
        <wpfui:NavigationStore.Items>
          <wpfui:NavigationItem
            Content="Dashboard"
            Icon="Home24"
            Tag="dashboard"
            Type="{x:Type pages:Dashboard}" />
        </wpfui:NavigationStore.Items>
      </wpfui:NavigationStore>

      <Border
        Grid.Column="1"
        Background="{DynamicResource ControlFillColorDefaultBrush}"
        CornerRadius="8,0,0,0">
        <Grid>
          <Frame x:Name="RootFrame" />
          <wpfui:Breadcrumb
            Margin="18,18,0,0"
            HorizontalAlignment="Left"
            VerticalAlignment="Top"
            FontSize="24"
            Navigation="{Binding ElementName=RootNavigation}" />
        </Grid>
      </Border>
    </Grid>

    <wpfui:TitleBar
      Title="My New App"
      Grid.Row="0"/>
  </Grid>
</Window>

```

Things have changed a bit, so let's go over what is what.

#### WPF UI Namespace
This line tells the interpreter that we will be using the WPF UI controls under the **wpfui:** abbreviation
```xml
<Window
  xmlns:wpfui="clr-namespace:WPFUI.Controls;assembly=WPFUI" />
```

#### Pages Namespace
This line informs that in the given directory there are files of our pages. They will be displayed by the navigation.
```xml
<Window
  xmlns:pages="clr-namespace:MyNewApp.Pages" />
```

#### Style
This line will make the window of our application slightly change. Necessary effects required for the correct display of the custom controls will be added.
```xml
<Window
  Style="{StaticResource UiWindow}" />
```

#### Navigation
The `wpfui:NavigationStore` control is responsible managing the displayed pages. The [Page](https://docs.microsoft.com/en-us/dotnet/api/system.windows.controls.page) is displayed inside the [Frame](https://docs.microsoft.com/en-us/dotnet/api/system.windows.controls.frame).  
As you can see in the example above, the navigation indicates which Frame will display pages.
```xml
<wpfui:NavigationStore
  Frame="{Binding ElementName=RootFrame}"/>

<Frame
  x:Name="RootFrame" />
```

### Bradcrumb
Breadcrumb is a small navigation aid, it automatically displays the title of the currently used [Page](https://docs.microsoft.com/en-us/dotnet/api/system.windows.controls.page) based on its name given in the navigation. As you can see in the example above, Breadcrumb has indicated which navigation it should use
```xml
<wpfui:NavigationStore
  x:Name="RootNavigation"/>

<wpfui:Breadcrumb
  Navigation="{Binding ElementName=RootNavigation}" />
```

### TitleBar
The [TitleBar](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/TitleBar.cs) includes minimize, maximize, and close buttons, and lets you drag the application across the desktop by grabbing at its top bar.  
TitleBar is a powerful element and allows you to control aspects such as the [Tray](https://github.com/lepoco/wpfui/blob/main/WPFUI/Tray/NotifyIcon.cs) icon/menu or [SnapLayout](https://github.com/lepoco/wpfui/blob/main/WPFUI/Common/SnapLayout.cs).

```xml
<wpfui:TitleBar
  x:Name="RootTitleBar"
  Grid.Row="0"
  ApplicationNavigation="True"
  MinimizeClicked="TitleBar_OnMinimizeClicked"
  MinimizeToTray="True"
  NotifyIconClick="RootTitleBar_OnNotifyIconClick"
  NotifyIconImage="pack://application:,,,/Assets/myicon.png"
  NotifyIconTooltip="My awesome app"
  UseNotifyIcon="True"
  UseSnapLayout="True">
  <wpfui:TitleBar.NotifyIconMenu>
    <ContextMenu>
      <MenuItem
        Header="Home" />
    </ContextMenu>
  </wpfui:TitleBar.NotifyIconMenu>
</wpfui:TitleBar>
```

# Adding pages
todo