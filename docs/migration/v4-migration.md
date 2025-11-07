# Migration plan

This page outlines key changes and important details to consider when migrating. It highlights what’s new, what’s changed, and any steps you need to take to ensure a smooth transition. This isn’t a full step-by-step guide but a quick reference to help you navigate the most critical parts of the migration process.

## Abstractions package

Some WPF UI interfaces have been moved to a standalone package **WPF-UI.Abstractions**. You don't need to reference it, it will always be added automatically with **WPF-UI** NuGet package.

## Navigation interfaces

Navigation interfaces have been moved to a standalone **WPF-UI.Abstractions** package. This way, if you have models, views or other business services in another project, not related to WPF, you can develop them together for multiple applications.

### New namespaces

`INavigationAware` and `INavigableView` have beed moved to `Wpf.Ui.Abstractions.Controls` namespace.

### Dependency injection based page creation

`IPageService` have been renamed to `INavigationViewPageProvider`.

Its default implementation is in the new **Wpf.Ui.DependencyInjection** package. You just need to use the `services.AddNavigationViewPageProvider()` extension and then indicate in the navigation that you want to use this interface. Then `NavigationView` will use DI container for pages creation.

**Program.cs**
```csharp
var builder = Host.CreateDefaultBuilder();
builder.Services.AddNavigationViewPageProvider();
```

**MyWindow.xaml.cs**
```csharp
var pageProvider = serviceProvider.GetRequiredService<INavigationViewPageProvider>();

// NavigationControl is x:Name of our NavigationView defined in XAML.
NavigationControl.SetPageProviderService(pageProvider)
```

### Navigation service

The `INavigationService` defined in the main package (**WPF-UI**) makes navigation management easy. You can use it for convenient injection between view models. We **HIGHLY** recommend it to be Singleton.

```csharp
var builder = Host.CreateDefaultBuilder();
builder.Services.AddNavigationViewPageProvider();
builder.Services.AddSingleton<INavigationService, NavigationService>();
```
