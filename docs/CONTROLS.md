# Controls
**WPF UI** has two kinds of controls. Default WPF ones, which styles have been overridden, and custom proprietary controls like `ProgressRing`.

### Access to custom controls
In order for your `Window`, `Page`, or `UserControl` to use custom **WPF UI** controls, you need to add `ui` namespace.
```xml
<Window
  ...
  xmlns:ui="http://schemas.lepo.co/wpfui/2022/xaml">
  <ui:ProgressRing IsIndeterminate="True" />
</Window>
```

## 🛠️ Custom controls
| Control | Namespace | Description |
| --- | --- | --- |
| **UiWindow** | [Wpf.Ui.Controls](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Controls/UiWindow.cs) | WPF window with additional features. |
| **UiPage** | [Wpf.Ui.Controls](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Controls/UiPage.cs) | WPF page with additional features. |
| **Anchor** | [Wpf.Ui.Controls](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Controls/Anchor.cs) | Creates a hyperlink to web pages, files or anything else a URL can address. |
| **Arc** | [Wpf.Ui.Controls](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Controls/Arc.cs) | Draws a symmetrical arc with rounded edges. |
| **AutoSuggestBox** | [Wpf.Ui.Controls](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Controls/AutoSuggestBox.cs) | Represents a text control that makes suggestions to users as they enter text using a keyboard. |
| **Badge** | [Wpf.Ui.Controls](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Controls/Badge.cs) | Used to highlight an item, attract attention or flag status. |
| **Breadcrumb** | [Wpf.Ui.Controls](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Controls/Breadcrumb.cs) | Automatic display of the page title from the navigation in the application. |
| **Button** | [Wpf.Ui.Controls](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Controls/Button.cs) | Custom button with additional parameters like an icon. |
| **Card** | [Wpf.Ui.Controls](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Controls/Card.cs) | Simple card compatible with the theme for displaying other elements. |
| **CardAction** | [Wpf.Ui.Controls](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Controls/CardAction.cs) | Inherited from the Button interactive card styled according to Fluent Design. |
| **CardControl** | [Wpf.Ui.Controls](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Controls/CardControl.cs) | Inherited from the Button control which displays an additional control on the right side of the card. |
| **CardExpander** | [Wpf.Ui.Controls](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Controls/CardExpander.cs) | Inherited from the ContentControl control which can hide the collapsable content. |
| **CodeBlock** | [Wpf.Ui.Controls](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Controls/CodeBlock.cs) | Formats syntax and display a fragment of the source code. |
| **Dialog** | [Wpf.Ui.Controls](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Controls/Dialog.cs) | Displays a large card with a slightly transparent background and two action buttons. |
| **FontIcon** | [Wpf.Ui.Controls](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Controls/FontIcon.cs) | Represents a text element containing an icon glyph with selectable font family. |
| **Hyperlink** | [Wpf.Ui.Controls](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Controls/Hyperlink.cs) | Button that opens a URL in a web browser. |
| **SymbolIcon** | [Wpf.Ui.Controls](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Controls/SymbolIcon.cs) | Represents a text element containing an icon glyph. |
| **MessageBox** | [Wpf.Ui.Controls](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Controls/MessageBox.cs) | Custom window to display notifications outside the application. |
| **Navigation** | [Wpf.Ui.Controls](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Controls/Navigation.cs) | Navigation styled as UWP apps. |
| **NavigationHeader** | [Wpf.Ui.Controls](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Controls/NavigationHeader.cs) | Header for the navigation. |
| **NavigationSeparator** | [Wpf.Ui.Controls](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Controls/NavigationSeparator.cs) | Separator for the navigation. |
| **NavigationItem** | [Wpf.Ui.Controls](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Controls/NavigationItem.cs) | Element of the navigation. |
| **NavigationStore** | [Wpf.Ui.Controls](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Controls/NavigationStore.cs) | Navigation styled as Windows 11 Store app |
| **NavigationFluent** | [Wpf.Ui.Controls](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Controls/NavigationFluent.cs) | Navigation styled as Windows 11 Settings app. |
| **NavigationCompact** | [Wpf.Ui.Controls](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Controls/NavigationCompact.cs) | Compact navigation styled as Windows 11 Task Manager app. |
| **NotifyIcon** | [Wpf.Ui.Controls](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Controls/NotifyIcon.cs) | Icon with menu in the tray. |
| **NumberBox** | [Wpf.Ui.Controls](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Controls/NumberBox.cs) | Text field for entering numbers with the possibility of setting a mask. |
| **ProgressRing** | [Wpf.Ui.Controls](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Controls/ProgressRing.cs) | Rotating loading ring like in Windows 11. |
| **ThumbRate** | [Wpf.Ui.Controls](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Controls/ThumbRate.cs) | Buttons to leave positive or negative ratings. |
| **Rating** | [Wpf.Ui.Controls](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Controls/Rating.cs) | Stars to display the rating. |
| **Snackbar** | [Wpf.Ui.Controls](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Controls/Snackbar.cs) | Animated card with a notification displayed at the bottom of the application. |
| **ToggleSwitch** | [Wpf.Ui.Controls](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Controls/ToggleSwitch.cs) | Use ToggleSwitch to present users with two mutally exclusive options (like on/off). |
| **TextBox** | [Wpf.Ui.Controls](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Controls/TextBox.cs) | Text field for with placeholders and icons. |
| **TitleBar** | [Wpf.Ui.Controls](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Controls/TitleBar.cs) | A set of buttons that can replace the default window navigation, giving it a new, modern look with implemented [NotifyIcon](https://github.com/lepoco/wpfui/blob/main/src/Wpf.Ui/Tray/NotifyIcon.cs). |