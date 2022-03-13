# Tutorial
Okay, you've decided to make a great fun journey with WPF UI.  
You'll need:
 - [Visual Studio 2022](https://visualstudio.microsoft.com/vs/community/)
 - .NET desktop development package (via VS 2022 installer)

![image](https://user-images.githubusercontent.com/13592821/158079915-f3682261-e5ee-499a-97e1-f0f14cbe7253.png)

## Get a package
The first thing you need to do is install the WPF UI via the package manager.  
To do this, in your new WPF project, click **Dependencies**, then **Manage NuGet Packages**

![image](https://user-images.githubusercontent.com/13592821/158079836-3bb42fa1-9b83-47b2-b887-277d19db09df.png)

Type **WPF-UI** in the search, then click install.

![image](https://user-images.githubusercontent.com/13592821/158079885-7715b552-bbc6-4574-bac9-92ecb7b161d8.png)

## Adding dictionaries
XAML, and hence WPF, operate on resource dictionaries. These are HTML-like files that describe the appearance and various aspects of the controls.  
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
                <ResourceDictionary Source="pack://application:,,,/WPFUI;component/Styles/Theme/Dark.xaml" />
                <ResourceDictionary Source="pack://application:,,,/WPFUI;component/Styles/WPFUI.xaml" />
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
</Application>

```

You can choose a color theme here,
`Light.xaml` or `Dark.xaml`

## The main window
This part is gonna be the weirdest. At the design stage, we decided not to create ready-made window templates, so you can design everything, including TitleBar, to your liking. This takes a little more work at the beginning, but allows you to have more control over application look.

First, let's modify MainWindow.xaml

```xml
<Window
  x:Class="MyNewApp"
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
```xml
<Window xmlns:wpfui="clr-namespace:WPFUI.Controls;assembly=WPFUI" />
```
This line tells the interpreter that we will be using the WPF UI controls under the **wpfui:** abbreviation

#### Pages Namespace
```xml
<Window xmlns:pages="clr-namespace:MyNewApp.Pages" />
```
This line informs that in the given directory there are files of our pages. They will be displayed by the navigation.

#### Default style
```xml
<Window xmlns:pages="clr-namespace:MyNewApp.Pages" />
```
This line informs that in the given directory there are files of our pages. They will be displayed by the navigation.