# Navigation
**WPF UI** implements a variety of navigation related controls. You can use them to conveniently manage the pages of your application.

## Basics
The `Navigation` control stores information about the currently displayed pages and provides methods related to navigation.  
You can create it in your XAML files or in code.

After the navigation is rendered, it will automatically navigate to the first page added to `Items`.

You can navigate manually using the Navigate method.
```cpp
RootNavigation.Navigate("dashboard");
```
The Navigate method uses `PageTag` parameter of the `NavigationItem` to detect what page you want to navigate to. If you don't define it, it will be added automatically.  
If `Content` of the `NavigationItem` is **HomePage**, the automatically generated tag will be **homepage**.

Additionally, you can choose whether the instance you are navigating to should be recreated and pass the view context.
```cpp
RootNavigation.Navigate(
  "dashboard", // Tag of the Navigationitem
  false,       // Do you want to reset the page instance?
  _myData      // Some object, preferably an ObservableCollection that contains your page data
);
```

You can also navigate using an index like an array.
```cpp
// 1, which is index 1, which is the second NavigationItem added to Items
RootNavigation.Navigate(1);
```

## NavigationStore
It is similar to the navigation from the Windows Store.

```xml
<wpfui:NavigationStore
  x:Name="RootNavigation"
  Frame="{Binding ElementName=RootFrame}">
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
```

## NavigationCompact
It is similar to the navigation from the Windows 11 Task Manager.

```xml
<wpfui:NavigationCompact
  x:Name="RootNavigation"
  Frame="{Binding ElementName=RootFrame}">
  <wpfui:NavigationCompact.Items>
    <wpfui:NavigationItem
      Content="Home"
      Icon="Home24"
      Page="{x:Type pages:Dashboard}"
      PageTag="dashboard" />
  </wpfui:NavigationCompact.Items>
  <wpfui:NavigationCompact.Footer>
    <wpfui:NavigationItem
      Content="Settings"
      Icon="Diversity24"
      Page="{x:Type pages:Settings}" />
    <!--  A navigation element that does not point to the page can be used as a button.  -->
    <wpfui:NavigationItem
      Click="NavigationButtonTheme_OnClick"
      Content="Theme"
      Icon="DarkTheme24" />
  </wpfui:NavigationCompact.Footer>
</wpfui:NavigationCompact>
```

## NavigationFluent
It is similar to the navigation from the Windows 11 Settings app.

```xml
<wpfui:NavigationFluent
  x:Name="RootNavigation"
  Frame="{Binding ElementName=RootFrame}">
  <wpfui:NavigationFluent.Items>
    <wpfui:NavigationItem
      Content="Home"
      Icon="Home24"
      Page="{x:Type pages:Dashboard}"
      PageTag="dashboard" />
  </wpfui:NavigationFluent.Items>
  <wpfui:NavigationFluent.Footer>
    <wpfui:NavigationItem
      Content="Settings"
      Icon="Diversity24"
      Page="{x:Type pages:Settings}" />
    <!--  A navigation element that does not point to the page can be used as a button.  -->
    <wpfui:NavigationItem
      Click="NavigationButtonTheme_OnClick"
      Content="Theme"
      Icon="DarkTheme24" />
  </wpfui:NavigationFluent.Footer>
</wpfui:NavigationFluent>
```