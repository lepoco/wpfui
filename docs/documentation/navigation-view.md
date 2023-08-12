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
