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