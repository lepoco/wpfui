# Navigation View

**WPF UI** implements a variety of navigation related controls. You can use them to conveniently manage the pages of your application.

## Basics

The `Navigation` control stores information about the currently displayed pages and provides methods related to navigation.  
You can create it in your XAML files or in code.

After the navigation is rendered, it will automatically navigate to the first page added to `Items` _(if the SelectedPageIndex is set above -1)_.

You can navigate manually using the Navigate method.

```csharp
RootNavigation.Navigate("dashboard");
RootNavigation.Navigate(typeof(MyDashboardClass));
```

The Navigate method uses `PageTag` parameter of the `NavigationItem` to detect what page you want to navigate to. If you don't define it, it will be added automatically.  
If `Content` of the `NavigationItem` is **HomePage** string, the automatically generated tag will be **homepage**.

You can also navigate using an index like an array.  
`Items` and `Footer` are treated as one array. So if you have two `NavigationItem` in `Items` and another two in the `Footer` then `RootNavigation.Navigate(2)`, will navigate to the third index (counting from zero), which is first item in the `Footer`.

```csharp
// 2, which is index 2 (third element), which is the third NavigationItem added to Items and Footer
RootNavigation.Navigate(2);
```

## NavigationView

```xml
<ui:NavigationView x:Name="RootNavigation" Grid.Row="1">
  <ui:NavigationView.AutoSuggestBox>
    <ui:AutoSuggestBox x:Name="AutoSuggestBox" PlaceholderText="Search">
      <ui:AutoSuggestBox.Icon>
        <ui:IconSourceElement>
          <ui:SymbolIconSource Symbol="Search24" />
        </ui:IconSourceElement>
      </ui:AutoSuggestBox.Icon>
    </ui:AutoSuggestBox>
  </ui:NavigationView.AutoSuggestBox>
  <ui:NavigationView.Header>
    <ui:BreadcrumbBar
      Margin="42,32,0,0"
      FontSize="28"
      FontWeight="DemiBold" />
  </ui:NavigationView.Header>
  <ui:NavigationView.MenuItems>
    <ui:NavigationViewItem Content="Dashboard" TargetPageType="{x:Type pages:DashboardPage}">
      <ui:NavigationViewItem.Icon>
        <ui:SymbolIcon Symbol="Home24" />
      </ui:NavigationViewItem.Icon>
    </ui:NavigationViewItem>
    <ui:NavigationViewItem Content="Data" TargetPageType="{x:Type pages:DataPage}">
      <ui:NavigationViewItem.Icon>
        <ui:SymbolIcon Symbol="DataHistogram24" />
      </ui:NavigationViewItem.Icon>
    </ui:NavigationViewItem>
  </ui:NavigationView.MenuItems>
  <ui:NavigationView.FooterMenuItems>
    <ui:NavigationViewItem Content="Settings" TargetPageType="{x:Type pages:SettingsPage}">
      <ui:NavigationViewItem.Icon>
        <ui:SymbolIcon Symbol="Settings24" />
      </ui:NavigationViewItem.Icon>
    </ui:NavigationViewItem>
  </ui:NavigationView.FooterMenuItems>
</ui:NavigationView>
```

## Pane display mode

## Set initial page

NavigationPage.xaml

```xml
<ui:NavigationView x:Name="RootNavigation"></ui:NavigationView>
```

NavigationPage.xaml.cs

```csharp
public partial class NavigationPage : Page
{
    public NavigationPage(NavigationPageModel model)
    {
        InitializeComponent();

        DataContext = model;
        Loaded += (_, _) => RootNavigation.Navigate(type(MyDashboardClass));
    }
}
```

## Using Navigation in the MVVM

Firstly, you need to implement the `IPageService` interface

```csharp
// from src/Wpf.Ui.Demo.Mvvm/Services/PageService.cs
public class PageService : IPageService
{
    /// <summary>
    /// Service which provides the instances of pages.
    /// </summary>
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="PageService"/> class and attaches the <see cref="IServiceProvider"/>.
    /// </summary>
    public PageService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc />
    public T? GetPage<T>()
        where T : class
    {
        if (!typeof(FrameworkElement).IsAssignableFrom(typeof(T)))
        {
            throw new InvalidOperationException("The page should be a WPF control.");
        }

        return (T?)_serviceProvider.GetService(typeof(T));
    }

    /// <inheritdoc />
    public FrameworkElement? GetPage(Type pageType)
    {
        if (!typeof(FrameworkElement).IsAssignableFrom(pageType))
        {
            throw new InvalidOperationException("The page should be a WPF control.");
        }

        return _serviceProvider.GetService(pageType) as FrameworkElement;
    }
}
```

Then, inject it into the IoC container.

```csharp
var services = new ServiceCollection();

services.AddSingleton<MainWindow>();
services.AddSingleton<MainWindowViewModel>();
services.AddSingleton<IPageService, PageService>();

// inject View and ViewModel
services.AddSingleton<MainWindow>();
services.AddSingleton<MainWindowViewModel>();
services.AddSingleton<HomePage>();
services.AddSingleton<HomePageModel>();
services.AddSingleton<CounterPage>();
services.AddSingleton<CounterPageModel>();
```

Lastly, adjust the code for the navigation window.

```xml
<Window
    x:Class="NavigationDemo.MainWindow"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:local="clr-namespace:NavigationDemo"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    xmlns:ui="http://schemas.lepo.co/wpfui/2022/xaml"
    Title="Navigation Window"
    Width="800"
    Height="450"
    d:DataContext="{d:DesignInstance local:MainWindowViewModel}"
    mc:Ignorable="d">

    <ui:NavigationView x:Name="RootNavigationView" MenuItemsSource="{Binding NavigationItems}"/>
</Window>
```

```csharp
using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using Wpf.Ui;
using Wpf.Ui.Controls;

public partial class MainWindow : Window
{
    public MainWindow(IPageService pageService, MainWindowViewModel model)
    {
        DataContext = model;
        InitializeComponent();
        
        // Set the page service for the navigation control.
        RootNavigationView.SetPageService(pageService);
    }
}

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<object> _navigationItems = [];

    public MainWindowViewModel()
    {
        NavigationItems =
        [
            new NavigationViewItem()
            {
                Content = "Home",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
                TargetPageType = typeof(HomePage)
            },
            new NavigationViewItem()
            {
                Content = "Counter",
                TargetPageType = typeof(CounterPage)
            },
        ];
    }
}
```

Alternatively, you can use the **WPF UI** Visual Studio Extension that includes a project template for MVVM pattern.
