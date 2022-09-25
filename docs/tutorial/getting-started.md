# Getting started

## Adding dictionaries

[XAML](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/xaml/?view=netdesktop-6.0), and hence WPF, operate on resource dictionaries. These are HTML-like files that describe the appearance and various aspects of the [controls](https://wpfui.lepo.co/documentation/controls).  
**WPF UI** adds its own sets of these files to tell the application how the controls should look like.

There should be a file called `App.xaml` in your new application. Add new dictionaries to it using **WPF UI** `ControlsDictionary` and `ThemesDictionary` classes:

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

There should be a `MainWindow.xaml` file in your newly created application.  
It contains the arrangement of the controls used and their parameters.

```xml
<Window x:Class="WpfApp1.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:WpfApp1"
        mc:Ignorable="d"
        Title="MainWindow" Height="450" Width="800">
    <Grid>

    </Grid>
</Window>
```

You can add a new namespace to this window to tell the interpreter that you will be using controls from somewhere, like the **WPF UI** library.

```xml
<Window
  ...
  xmlns:ui="http://schemas.lepo.co/wpfui/2022/xaml" />
```

## Adding controls

To add a new control from the **WPF UI** library, you just need to enter its class name, prefixing it with the `ui:` prefix:

```xml
<Window x:Class="WpfApp1.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:ui="http://schemas.lepo.co/wpfui/2022/xaml"
        xmlns:local="clr-namespace:WpfApp1"
        mc:Ignorable="d"
        Title="MainWindow" Height="450" Width="800">
    <Grid>
      <ui:SymbolIcon Symbol="Fluent24"/>
    </Grid>
</Window>
```

# Well...

That's it when it comes to the basics, information about individual controls can be found in [documentation](https://wpfui.lepo.co/documentation/), rules for building a WPF application can be found in the [official Microsoft documentation](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/controls/styles-templates-overview?view=netdesktop-6.0). You can check out [**how to build MVVM applications** here](https://learn.microsoft.com/en-us/windows/communitytoolkit/mvvm/puttingthingstogether).  
If you think this documentation sucks, [help improve it here](https://github.com/lepoco/wpfui/tree/development/docs/tutorial).
