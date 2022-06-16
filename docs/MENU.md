# Menus
**WPF UI** mainly modifies the styles of already added navigation related controls in WPF. The exceptions are, for example, the `MenuItem`, which has a `SymbolIcon` parameter added to it.

## Menu
An interesting menu, you can create, for example, this way.
```xml
<Menu Margin="0,8,0,0">
  <MenuItem Header="File">
      <wpfui:MenuItem
          Header="New"
          InputGestureText="Ctrl+N"
          SymbolIcon="Accessibility24" />
      <MenuItem Header="Open" InputGestureText="Ctrl+O" />
      <MenuItem Header="Disabled" IsEnabled="False" />
      <wpfui:MenuItem
          Header="Save"
          InputGestureText="Ctrl+S"
          SymbolIcon="Save24" />
      <Separator />
      <MenuItem Header="Exit" InputGestureText="Ctrl+Q" />
  </MenuItem>
  <MenuItem Header="With icon">
      <MenuItem.Icon>
          <wpfui:SymbolIcon Filled="True" Symbol="StoreMicrosoft24" />
      </MenuItem.Icon>
  </MenuItem>
  <wpfui:MenuItem Header="Save as" SymbolIcon="Save24">
      <wpfui:MenuItem Header="Word Document" SymbolIcon="AlertUrgent24" />
      <MenuItem Header="PDF" InputGestureText="Ctrl+Alt+P" />
      <MenuItem Header="Text File" />
      <Separator />
      <MenuItem Header="Print" InputGestureText="Ctrl+P" />
  </wpfui:MenuItem>
  <MenuItem Header="With submenu">
      <MenuItem
          Header="Show status bar"
          IsCheckable="True"
          IsChecked="True" />
      <MenuItem Header="Word wrap" IsCheckable="True" />
      <wpfui:MenuItem
          Header="Checkable custom"
          IsCheckable="True"
          SymbolIcon="AlignTop24" />
      <MenuItem Header="NormalItem" />
      <wpfui:MenuItem Header="SubMenu" SymbolIcon="Balloon24">
          <wpfui:MenuItem Header="SubMenu 2" SymbolIcon="Diversity24">
              <wpfui:MenuItem Header="SubItem 2.1" SymbolIcon="Guardian24" />
          </wpfui:MenuItem>
          <MenuItem Header="SubItem 1" />
      </wpfui:MenuItem>
  </MenuItem>
  <MenuItem Header="Disabled" IsEnabled="False" />
  <Separator />
  <MenuItem Header="Close" />
</Menu>
```
As you can see, in some places we use the native `MenuItem` control, and in some places we use `wpfui: MenuItem`. You can mix them with each other freely.