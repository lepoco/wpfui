# NavigationView

`NavigationView` is a top-level navigation control that provides a collapsible navigation pane (the "hamburger menu") and a content area. It is the primary way to implement top-level navigation in your app.

> [!TIP]
> For a complete implementation example, see the [WPF UI Gallery](https://github.com/lepoco/wpfui/tree/main/src/Wpf.Ui.Gallery) application.

## Anatomy

The `NavigationView` control has several key areas:

- **Pane**: The area on the left or top that contains navigation items.
- **Header**: An area at the top of the content area, often used for a page title or a `BreadcrumbBar`.
- **Content Area**: The main area of the control where page content is displayed.
- **AutoSuggestBox**: An optional search box integrated into the navigation pane.
- **MenuItems**: The primary list of navigation items.
- **FooterMenuItems**: A secondary list of navigation items, typically for settings or about pages.

## Basic Usage

Define `NavigationView` in your XAML and add `NavigationViewItem` objects to the `MenuItems` and `FooterMenuItems` collections.

```xml
<ui:NavigationView
    xmlns:pages="clr-namespace:YourApp.Views.Pages"
    xmlns:ui="http://schemas.lepo.co/wpfui/2022/xaml">
    <ui:NavigationView.MenuItems>
        <ui:NavigationViewItem
            Content="Home"
            Icon="{ui:SymbolIcon Home24}"
            TargetPageType="{x:Type pages:DashboardPage}" />
        <ui:NavigationViewItem
            Content="Data"
            Icon="{ui:SymbolIcon DataHistogram24}"
            TargetPageType="{x:Type pages:DataPage}" />
    </ui:NavigationView.MenuItems>
    <ui:NavigationView.FooterMenuItems>
        <ui:NavigationViewItem
            Content="Settings"
            Icon="{ui:SymbolIcon Settings24}"
            TargetPageType="{x:Type pages:SettingsPage}" />
    </ui:NavigationView.FooterMenuItems>
</ui:NavigationView>
```

> [!NOTE]
> `TargetPageType` is a required property on `NavigationViewItem` that specifies the page to navigate to when the item is selected. The value must be a `System.Type`.

## Programmatic Navigation

You can navigate programmatically by calling the `Navigate` method with either the `Type` of the page or its `PageTag`.

```csharp
// Navigate by Type
MyNavigationView.Navigate(typeof(SettingsPage));

// Navigate by Tag
MyNavigationView.Navigate("settings");
```

To use tags, you must define a `PageTag` on the `NavigationViewItem`. If not defined, a tag is automatically generated from the `Content` property (e.g., "Settings Page" becomes "settingspage").

```xml
<ui:NavigationViewItem
    Content="Settings"
    PageTag="settings"
    TargetPageType="{x:Type pages:SettingsPage}" />
```

### Back Navigation

`NavigationView` automatically handles back navigation. The back button is shown when `CanGoBack` is `true`. You can also call `GoBack()` programmatically.

```csharp
if (MyNavigationView.CanGoBack)
{
    MyNavigationView.GoBack();
}
```

## Pane Display Mode

Control the visibility and behavior of the navigation pane with the `PaneDisplayMode` property.

- `Left`: The pane is always open on the left.
- `Top`: The pane is shown as a horizontal bar at the top.
- `LeftCompact`: The pane is collapsed to show only icons, and expands on hover or when the hamburger button is clicked.
- `LeftMinimal`: The pane is hidden and can be opened as an overlay.

```xml
<ui:NavigationView PaneDisplayMode="Top" />
```

You can also control the pane's open state with the `IsPaneOpen` property.

> [!TIP]
> To create a responsive layout that changes `PaneDisplayMode` based on window width, bind `PaneDisplayMode` to a property in your ViewModel and update it in the `Window.SizeChanged` event.

## Header

The `Header` property provides a content area above the navigation frame. It is commonly used with a `BreadcrumbBar` to show the user's location.

```xml
<ui:NavigationView>
    <ui:NavigationView.Header>
        <ui:BreadcrumbBar />
    </ui:NavigationView.Header>
</ui:NavigationView>
```

The `BreadcrumbBar` will automatically sync with the `NavigationView`'s navigation history.

## MVVM Integration

For MVVM applications, it is recommended to use `INavigationService` and `IPageService` for navigation and page resolution.

### 1. Service Configuration

First, register the required services and your pages/ViewModels with your dependency injection container.

```csharp
// Using Microsoft.Extensions.DependencyInjection
Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        // Main window
        services.AddScoped<IWindow, MainWindow>();
        services.AddScoped<MainWindowViewModel>();

        // Services
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IPageService, PageService>();

        // Pages and ViewModels
        services.AddScoped<DashboardPage>();
        services.AddScoped<DashboardViewModel>();
        services.AddScoped<SettingsPage>();
        services.AddScoped<SettingsViewModel>();
    }).Build();
```

### 2. ViewModel Setup

In your `MainWindowViewModel`, define collections for your navigation items and bind them to the `NavigationView`.

```csharp
public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private ICollection<object> _menuItems = new ObservableCollection<object>();

    [ObservableProperty]
    private ICollection<object> _footerMenuItems = new ObservableCollection<object>();

    public MainWindowViewModel()
    {
        MenuItems = new ObservableCollection<object>
        {
            new NavigationViewItem("Home", SymbolRegular.Home24, typeof(DashboardPage)),
            new NavigationViewItem("Data", SymbolRegular.DataHistogram24, typeof(DataPage))
        };

        FooterMenuItems = new ObservableCollection<object>
        {
            new NavigationViewItem("Settings", SymbolRegular.Settings24, typeof(SettingsPage))
        };
    }
}
```

### 3. View Setup

In your `MainWindow.xaml`, bind the `MenuItemsSource` and `FooterMenuItemsSource` properties to the collections in your ViewModel. Then, attach the `INavigationService`.

```xml
<ui:NavigationView
    x:Name="RootNavigationView"
    MenuItemsSource="{Binding MenuItems}"
    FooterMenuItemsSource="{Binding FooterMenuItems}" />
```

```csharp
public partial class MainWindow : IWindow
{
    public MainWindow(
        MainWindowViewModel viewModel,
        INavigationService navigationService,
        IPageService pageService
    )
    {
        ViewModel = viewModel;
        DataContext = this;
        InitializeComponent();

        // Attach the service to the NavigationView
        navigationService.SetNavigationControl(RootNavigationView);
        
        // You can also set the page service, which is required for some functionalities
        RootNavigationView.SetPageService(pageService);
    }

    public MainWindowViewModel ViewModel { get; }
}
```

### 4. Navigating from a ViewModel

Inject `INavigationService` into any ViewModel and use it to navigate.

```csharp
public partial class DashboardViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;

    public DashboardViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    [RelayCommand]
    private void OnGoToSettings()
    {
        _navigationService.Navigate(typeof(SettingsPage));
    }
}
```

## Navigation Events

`NavigationView` provides several events to hook into the navigation lifecycle:

- `Navigating`: Occurs before navigation starts. Can be cancelled.
- `Navigated`: Occurs after navigation is complete.
- `SelectionChanged`: Occurs when a `NavigationViewItem` is selected.

```csharp
private void OnNavigating(NavigationView sender, NavigatingCancelEventArgs args)
{
    // Don't navigate to settings if the user is not an admin
    if (args.PageType == typeof(SettingsPage) && !_isAdmin)
    {
        args.Cancel = true;
    }
}
```

## Navigation-Aware Pages

Implement `INavigationAware` on your page's code-behind or `INavigableView<T>` on your ViewModel to receive navigation events directly.

### INavigationAware

This interface is ideal for code-behind scenarios.

```csharp
public partial class MyPage : INavigationAware
{
    public void OnNavigatedTo()
    {
        // Page was navigated to
    }

    public void OnNavigatedFrom()
    {
        // Page was navigated away from
    }
}
```

### `INavigableView<T>`

This interface is designed for MVVM. Your page must inherit from `INavigableView<T>` where `T` is its ViewModel. The ViewModel will then receive the navigation calls.

**Page:**

```csharp
[GalleryPage("My Page", SymbolRegular.Page24)]
public partial class MyPage : INavigableView<MyViewModel>
{
    public MyViewModel ViewModel { get; }

    public MyPage(MyViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;
        InitializeComponent();
    }
}
```

**ViewModel:**

```csharp
public partial class MyViewModel : ObservableObject, INavigationAware
{
    public void OnNavigatedTo()
    {
        // Page was navigated to
    }

    public void OnNavigatedFrom()
    {
        // Page was navigated away from
    }
}
```

> [!IMPORTANT]
> For `INavigableView<T>` to work, your page must have a public `ViewModel` property that returns an instance of the ViewModel.

## History and Caching

`NavigationView` maintains a navigation history.

- `History`: A collection of `Page` instances that have been visited.
- `CacheHistory`: The number of pages to keep in memory. The default is `0`. Set to a value greater than 0 to cache pages. When a cached page is navigated to, its previous state is preserved.

```xml
<ui:NavigationView CacheHistory="5" />
```

> [!CAUTION]
> Caching pages increases memory consumption. Use it only for pages that are expensive to create or where preserving state is critical. Avoid caching pages that display frequently changing data.
