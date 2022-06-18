# Pages
Pages are controls that you display inside a `Frame` control using one of the available navigation methods.  
[You can read more here](https://docs.microsoft.com/en-us/dotnet/api/system.windows.controls.page)

**WPF UI** provides you with several modifiers for the `Page` control that can help you manage your application.

## Add a custom style
The custom `UiPage` style changes the background, text color and some basic parameters according to your chosen theme.

```xml
<Page
  x:Class="WPFUI.Demo.Views.Pages.Dashboard"
  xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
  xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
  xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
  xmlns:local="clr-namespace:WPFUI.Demo.Views.Pages"
  xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
  xmlns:wpfui="http://schemas.lepo.co/wpfui/2022/xaml"
  Title="Dashboard"
  d:DesignHeight="550"
  d:DesignWidth="800"
  Style="{StaticResource UiPage}"
  mc:Ignorable="d">
</Page>
```

You can also use an other style, `UiPageScrollable`, which automatically adds a scrollbar if the page content is too long.
```xml
<Page
  Style="{StaticResource UiPageScrollable}"
  mc:Ignorable="d">
</Page>
```