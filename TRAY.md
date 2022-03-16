# Tray
The tray icon can be easily added with the help of the TitleBar control.

```xml
<wpfui:TitleBar
  ApplicationNavigation="True"
  MinimizeToTray="True"
  NotifyIconImage="pack://application:,,,/Assets/mwpf_icon.png"
  NotifyIconTooltip="My App"
  UseNotifyIcon="True"
  UseSnapLayout="True">
  <wpfui:TitleBar.NotifyIconMenu>
      <ContextMenu>
          <MenuItem Header="Home"/>
      </ContextMenu>
  </wpfui:TitleBar.NotifyIconMenu>
</wpfui:TitleBar>
```