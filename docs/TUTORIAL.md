# Tutorial
**WPF UI** is a library built for [WPF](https://docs.microsoft.com/en-us/visualstudio/designers/getting-started-with-wpf) and the [C#](https://docs.microsoft.com/en-us/dotnet/csharp/) language. To be able to work with them comfortably, you will need [Visual Studio Community Edition](https://visualstudio.microsoft.com/vs/community/) *(NOT VISUAL STUDIO CODE)*.

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

There should be a file called `App.xaml` in your new application. Add new dictionaries to it using **WPF UI** `Resources` class:

```xml
<Application
  xmlns:wpfui="http://schemas.lepo.co/wpfui/2022/xaml">
  <Application.Resources>
    <ResourceDictionary>
      <ResourceDictionary.MergedDictionaries>
        <wpfui:Resources Theme="Dark" />
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
<Window
  xmlns:pages="clr-namespace:MyNewApp.Pages"
  xmlns:wpfui="http://schemas.lepo.co/wpfui/2022/xaml"
  Style="{StaticResource UiWindow}"
  WindowStartupLocation="CenterScreen"
  mc:Ignorable="d">
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
        <wpfui:NavigationStore
            x:Name="RootNavigation"
            Grid.Column="0"
            Margin="6,0,6,0"
            Frame="{Binding ElementName=RootFrame}"
            Navigated="RootNavigation_OnNavigated"
            SelectedPageIndex="0">
            <wpfui:NavigationStore.Items>
                <wpfui:NavigationItem
                    Content="Home"
                    Icon="Home24"
                    Page="{x:Type pages:Dashboard}"
                    PageTag="dashboard" />
            </wpfui:NavigationStore.Items>
            <wpfui:NavigationStore.Footer>
              <wpfui:NavigationItem
                    Content="Settings"
                    Icon="Diversity24"
                    Page="{x:Type pages:Settings}" />
                <!--  A navigation element that does not point to the page can be used as a button.  -->
                <wpfui:NavigationItem
                    Click="NavigationButtonTheme_OnClick"
                    Content="Theme"
                    Icon="DarkTheme24" />
            </wpfui:NavigationStore.Footer>
        </wpfui:NavigationStore>

        <!--  We display our pages inside this element.  -->
        <Border
            Grid.Column="1"
            Background="{DynamicResource ControlFillColorDefaultBrush}"
            CornerRadius="8,0,0,0">
            <Grid>
                <Grid.RowDefinitions>
                    <RowDefinition Height="Auto" />
                    <RowDefinition Height="*" />
                </Grid.RowDefinitions>
                <Frame x:Name="RootFrame" Grid.Row="1" />
                <wpfui:Breadcrumb
                    Grid.Row="0"
                    Margin="18"
                    HorizontalAlignment="Left"
                    VerticalAlignment="Top"
                    FontSize="24"
                    Navigation="{Binding ElementName=RootNavigation}" />
            </Grid>
        </Border>
    </Grid>

    <!--  The title bar contains window navigation elements and some Tray related extras.  -->
    <!--  You can put additional controls in the header, such as a search bar.  -->
    <!--  <wpfui:TitleBar.Header />  -->
    <wpfui:TitleBar
      Title="WPF UI - Fluent design system"
      Grid.Row="0">
      <wpfui:TitleBar.Tray>
        <wpfui:NotifyIcon
          FocusOnLeftClick="True"
          MenuOnRightClick="True"
          TooltipText="WPF UI">
          <wpfui:NotifyIcon.Menu>
             <ContextMenu>
               <wpfui:MenuItem
                 Header="Home"
                 SymbolIcon="Library28"
                 Tag="home" />
            </ContextMenu>
          </wpfui:NotifyIcon.Menu>
        </wpfui:NotifyIcon>
      </wpfui:TitleBar.Tray>
    </wpfui:TitleBar>
  </Grid>
</Window>

```

Things have changed a bit, so let's go over what is what.

#### WPF UI Namespace
This line tells the interpreter that we will be using the **WPF UI** controls under the **wpfui:** abbreviation
```xml
<Window
  xmlns:wpfui="http://schemas.lepo.co/wpfui/2022/xaml" />
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
`TitleBar` is a powerful control and allows you to control aspects such as the [Tray](https://github.com/lepoco/wpfui/blob/main/WPFUI/Tray/NotifyIcon.cs) icon/menu or [SnapLayout](https://github.com/lepoco/wpfui/blob/main/WPFUI/Common/SnapLayout.cs).

```xml
<wpfui:TitleBar
  Title="WPF UI - Fluent design system"
  Grid.Row="0">
  <wpfui:TitleBar.Tray>
    <wpfui:NotifyIcon
      FocusOnLeftClick="True"
      MenuOnRightClick="True"
      TooltipText="WPF UI">
      <wpfui:NotifyIcon.Menu>
          <ContextMenu>
            <wpfui:MenuItem
              Header="Home"
              SymbolIcon="Library28"
              Tag="home" />
        </ContextMenu>
      </wpfui:NotifyIcon.Menu>
    </wpfui:NotifyIcon>
  </wpfui:TitleBar.Tray>
</wpfui:TitleBar>
```
