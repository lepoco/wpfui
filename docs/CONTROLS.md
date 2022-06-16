# Controls
**WPF UI** has two kinds of controls. Default WPF ones, which styles have been overridden, and custom proprietary controls like `ProgressRing`.

### Access to custom controls
In order for your `Window`, `Page`, or `UserControl` to use custom **WPF UI** controls, you need to add `wpfui` namespace.
```xml
<Window
  xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
  xmlns:wpfui="http://schemas.lepo.co/wpfui/2022/xaml"
  mc:Ignorable="d">
  <wpfui:ProgressRing IsIndeterminate="True" />
</Window>
```

## 🛠️ Custom controls
| Control | Namespace | Description |
| --- | --- | --- |
| **Anchor** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/Anchor.cs) | Creates a hyperlink to web pages, files or anything else a URL can address. |
| **Arc** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/Arc.cs) | Draws a symmetrical arc with rounded edges. |
| **Badge** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/Badge.cs) | Used to highlight an item, attract attention or flag status. |
| **Breadcrumb** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/Breadcrumb.cs) | Automatic display of the page title from the navigation in the application. |
| **Button** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/Button.cs) | Custom button with additional parameters like an icon. |
| **Card** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/Card.cs) | Simple card compatible with the theme for displaying other elements. |
| **CardAction** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/CardAction.cs) | Inherited from the Button interactive card styled according to Fluent Design. |
| **CardControl** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/CardControl.cs) | Inherited from the Button control which displays an additional control on the right side of the card. |
| **CardExpander** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/CardExpander.cs) | Inherited from the ContentControl control which can hide the collapsable content. |
| **CodeBlock** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/CodeBlock.cs) | Formats syntax and display a fragment of the source code. |
| **Dialog** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/Dialog.cs) | Displays a large card with a slightly transparent background and two action buttons. |
| **FontIcon** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/FontIcon.cs) | Represents a text element containing an icon glyph with selectable font family. |
| **Hyperlink** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/Hyperlink.cs) | Button that opens a URL in a web browser. |
| **SymbolIcon** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/SymbolIcon.cs) | Represents a text element containing an icon glyph. |
| **MessageBox** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/MessageBox.cs) | Custom window to display notifications outside the application. |
| **Navigation** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/Navigation.cs) | Navigation styled as UWP apps. |
| **NavigationItem** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/NavigationItem.cs) | Element of the navigation. |
| **NavigationStore** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/NavigationStore.cs) | Navigation styled as Windows 11 Store app |
| **NavigationFluent** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/NavigationFluent.cs) | Navigation styled as Windows 11 Settings app. |
| **NumberBox** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/NumberBox.cs) | Text field for entering numbers with the possibility of setting a mask. |
| **ProgressRing** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/ProgressRing.cs) | Rotating loading ring like in Windows 11. |
| **Rating** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/Rating.cs) | Stars to display the rating. |
| **SearchBox** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/SearchBox.cs) | Lets look for things and other stuff. |
| **Snackbar** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/Snackbar.cs) | Animated card with a notification displayed at the bottom of the application. |
| **TextBox** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/TextBox.cs) | Text field for with placeholders and icons. |
| **TitleBar** | [WPFUI.Controls](https://github.com/lepoco/wpfui/blob/main/WPFUI/Controls/TitleBar.cs) | A set of buttons that can replace the default window navigation, giving it a new, modern look with implemented [NotifyIcon](https://github.com/lepoco/wpfui/blob/main/WPFUI/Tray/NotifyIcon.cs). |