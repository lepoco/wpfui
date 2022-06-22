# Services
**WPF UI** tries to support ***Dependency Injection*** (DI) and ***Model-View-ViewModel*** (MVVM) patterns.  
If you use [CommunityToolkit](https://github.com/CommunityToolkit/dotnet) or [Microsoft.Toolkit.Mvvm](https://docs.microsoft.com/en-us/windows/communitytoolkit/mvvm/introduction) in your application, the services listed below may be useful for you.

## Getting started
**MVVM**  
Model–view–viewmodel (MVVM) is a software architectural pattern that facilitates the separation of the development of the graphical user interface (the view) – be it via a markup language or GUI code – from the development of the business logic or back-end logic (the model) so that the view is not dependent on any specific model platform.[^1]

**DI**  
In software engineering, dependency injection is a design pattern in which an object receives other objects that it depends on. A form of inversion of control, dependency injection aims to separate the concerns of constructing objects and using them, leading to loosely coupled programs.[^2]

**Sources in WPF UI**
The classes and interfaces created for MVVM and DI are in the namespace:
```cpp
namespace Wpf.Ui.Mvvm.Services;
```

## DialogService
`DialogService` is responsible for managing the display of the `Dialog` control.

### Contract
```cpp
interface Wpf.Ui.Mvvm.Contracts.IDialogService
```

### Implementation
```cpp
class Wpf.Ui.Mvvm.Services.DialogService
```

## Exposes
```cpp
// Sets the IDialogControl
IDialogService.SetDialogControl(IDialogControl dialog);
```
```cpp
// Provides direct access to the IDialogControl
IDialogService.GetDialogControl();
```

### How to use
```cpp
// Services registration in your service management class
private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
{
    services.AddSingleton<IDialogService, DialogService>();
}

// Pointing to an existing control somewhere in your Window or Page.
public Container(IDialogService dialogService)
{
    dialogService.SetDialogControl(MyDialogControlName);
}
```



[^1]: Model–view–viewmodel https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93viewmodel

[^2]: Dependency injection https://en.wikipedia.org/wiki/Dependency_injection