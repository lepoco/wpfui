# Tray
**WPF UI** implements Tray icon its own way using native Windows features.  
You can create such an icon in several ways, but we particularly recommend pinning it to the `Titlebar` control inside your main `Window`. Thanks to this, you will have access to all functionalities.

```xml
<wpfui:TitleBar Title="WPF UI - Fluent design system">
  <wpfui:TitleBar.Tray>
    <wpfui:NotifyIcon
      FocusOnLeftClick="True"
      Icon="pack://application:,,,/Assets/wpfui.png"
      MenuOnRightClick="True"
      TooltipText="WPF UI">
      <wpfui:NotifyIcon.Menu>
        <ContextMenu>
          <wpfui:MenuItem
            Header="Home"
            SymbolIcon="Library28"/>
        </ContextMenu>
      </wpfui:NotifyIcon.Menu>
    </wpfui:NotifyIcon>
  </wpfui:TitleBar.Tray>
</wpfui:TitleBar>
```

## NotifyIcon
The `NotifyIcon` control can be placed inside one of your XAML files, or simply created in code.  
It has several properties.

`TooltipText` is the text that will be displayed when you hover your mouse over the icon.
```cpp
NotifyIcon.TooltipText = "Hello World";
```

`FocusOnLeftClick` automatically restores your `Window` if it is minimized, and drags it over all other applications.
```cpp
NotifyIcon.FocusOnLeftClick = true;
```

`MenuOnRightClick` automatically opens the menu you set on right-click.
```cpp
NotifyIcon.MenuOnRightClick = true;
```

`Icon` is an `ImageSource`, e.g. in the form of a PNG that will be used as an icon. It should be added as the `Resource` of your application.
```cpp
NotifyIcon.Icon = "pack://application:,,,/Assets/wpfui.png";
```

`Menu` is the `ContextMenu` that will appear in the tray after right-click.
```cpp
NotifyIcon.Menu = new ContextMenu();
```

`MenuFontSize` is a `Double` number that defines the size of the text in the menu.
```cpp
NotifyIcon.MenuFontSize = 14d;
```