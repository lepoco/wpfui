# Navigation

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

## NavigationStore

It is similar to the navigation from the Windows Store.

```xml
<ui:NavigationStore
  x:Name="RootNavigation"
  Frame="{Binding ElementName=RootFrame, Mode=OneWay}">
  <ui:NavigationStore.Items>
    <ui:NavigationItem
      Content="Home"
      Icon="Home24"
      PageType="{x:Type pages:Dashboard}"
      PageTag="dashboard" />
  </ui:NavigationStore.Items>
  <ui:NavigationStore.Footer>
    <ui:NavigationItem
      Content="Settings"
      Icon="Diversity24"
      PageType="{x:Type pages:Settings}" />
    <!--  A navigation element that does not point to the page can be used as a button.  -->
    <ui:NavigationItem
      Click="NavigationButtonTheme_OnClick"
      Content="Theme"
      Icon="DarkTheme24" />
  </ui:NavigationStore.Footer>
</ui:NavigationStore>
```

## NavigationCompact

It is similar to the navigation from the Windows 11 Task Manager.

```xml
<ui:NavigationCompact
  x:Name="RootNavigation"
  Frame="{Binding ElementName=RootFrame}">
  <ui:NavigationCompact.Items>
    <ui:NavigationItem
      Content="Home"
      Icon="Home24"
      PageType="{x:Type pages:Dashboard}"
      PageTag="dashboard" />
  </ui:NavigationCompact.Items>
  <ui:NavigationCompact.Footer>
    <ui:NavigationItem
      Content="Settings"
      Icon="Diversity24"
      PageType="{x:Type pages:Settings}" />
    <!--  A navigation element that does not point to the page can be used as a button.  -->
    <ui:NavigationItem
      Click="NavigationButtonTheme_OnClick"
      Content="Theme"
      Icon="DarkTheme24" />
  </ui:NavigationCompact.Footer>
</ui:NavigationCompact>
```

## NavigationFluent

It is similar to the navigation from the Windows 11 Settings app.

```xml
<ui:NavigationFluent
  x:Name="RootNavigation"
  Frame="{Binding ElementName=RootFrame}">
  <ui:NavigationFluent.Items>
    <ui:NavigationItem
      Content="Home"
      Icon="Home24"
      Page="{x:Type pages:Dashboard}"
      PageTag="dashboard" />
  </ui:NavigationFluent.Items>
  <ui:NavigationFluent.Footer>
    <ui:NavigationItem
      Content="Settings"
      Icon="Diversity24"
      Page="{x:Type pages:Settings}" />
    <!--  A navigation element that does not point to the page can be used as a button.  -->
    <ui:NavigationItem
      Click="NavigationButtonTheme_OnClick"
      Content="Theme"
      Icon="DarkTheme24" />
  </ui:NavigationFluent.Footer>
</ui:NavigationFluent>
```
