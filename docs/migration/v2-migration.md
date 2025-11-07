# Migration plan

This page outlines key changes and important details to consider when migrating. It highlights what’s new, what’s changed, and any steps you need to take to ensure a smooth transition. This isn’t a full step-by-step guide but a quick reference to help you navigate the most critical parts of the migration process.

## Key changes in v2

### Global namespace change

In version 2.0 the namespace has been changed from `WPF-UI` to the more .NET compatible `Wpf.Ui`. The package name has not been changed to maintain branding and consistency.

### Navigation

The navigation control has been rewritten yet again, making it almost completely incompatible.

All navigation controls inherit from `Wpf.Ui.Controls.Navigation.NavigationBase` base class.

```xml
<ui:NavigationStore
  Frame="{Binding ElementName=RootFrame}"
  Precache="False"
  SelectedPageIndex="-1"
  TransitionDuration="200"
  TransitionType="FadeInWithSlide">
  <ui:NavigationStore.Items>
    <ui:NavigationItem
      Cache="True"
      Content="Home"
      Icon="Home24"
      PageTag="dashboard"
      PageType="{x:Type pages:Dashboard}" />
    <ui:NavigationSeparator />
  </ui:NavigationStore.Items>
  <ui:NavigationStore.Footer>
    <ui:NavigationItem
      Click="NavigationButtonTheme_OnClick"
      Content="Theme"
      Icon="DarkTheme24" />
  </ui:NavigationStore.Footer>
</ui:NavigationStore>
```

```xml
<ui:NavigationFluent
  Frame="{Binding ElementName=RootFrame}"
  Precache="False"
  SelectedPageIndex="-1"
  TransitionDuration="200"
  TransitionType="FadeInWithSlide">
  <ui:NavigationFluent.Items>
    <ui:NavigationItem
      Cache="True"
      Content="Home"
      Icon="Home24"
      PageTag="dashboard"
      PageType="{x:Type pages:Dashboard}" />
    <ui:NavigationSeparator />
  </ui:NavigationFluent.Items>
  <ui:NavigationFluent.Footer>
    <ui:NavigationItem
      Click="NavigationButtonTheme_OnClick"
      Content="Theme"
      Icon="DarkTheme24" />
  </ui:NavigationFluent.Footer>
</ui:NavigationFluent>
```

```xml
<ui:NavigationCompact
  Frame="{Binding ElementName=RootFrame}"
  Precache="False"
  SelectedPageIndex="-1"
  TransitionDuration="200"
  TransitionType="FadeInWithSlide">
  <ui:NavigationCompact.Items>
    <ui:NavigationItem
      Cache="True"
      Content="Home"
      Icon="Home24"
      PageTag="dashboard"
      PageType="{x:Type pages:Dashboard}" />
    <ui:NavigationSeparator />
  </ui:NavigationCompact.Items>
  <ui:NavigationCompact.Footer>
    <ui:NavigationItem
      Click="NavigationButtonTheme_OnClick"
      Content="Theme"
      Icon="DarkTheme24" />
  </ui:NavigationCompact.Footer>
</ui:NavigationCompact>
```

### Titlebar

The titlebar control has been rewritten yet again, making it almost completely incompatible.
