# Menus
**WPF UI** mainly modifies the styles of already added navigation related controls in WPF. The exceptions are, for example, the `MenuItem`, which has a `SymbolIcon` parameter added to it.

## Menu
An interesting menu, you can create, for example, this way.
```xml
<Menu Margin="0,8,0,0">
  <MenuItem Header="File">
      <ui:MenuItem
          Header="New"
          InputGestureText="Ctrl+N"
          SymbolIcon="Accessibility24" />
      <MenuItem Header="Open" InputGestureText="Ctrl+O" />
      <MenuItem Header="Disabled" IsEnabled="False" />
      <ui:MenuItem
          Header="Save"
          InputGestureText="Ctrl+S"
          SymbolIcon="Save24" />
      <Separator />
      <MenuItem Header="Exit" InputGestureText="Ctrl+Q" />
  </MenuItem>
  <MenuItem Header="With icon">
      <MenuItem.Icon>
          <ui:SymbolIcon Filled="True" Symbol="StoreMicrosoft24" />
      </MenuItem.Icon>
  </MenuItem>
  <ui:MenuItem Header="Save as" SymbolIcon="Save24">
      <ui:MenuItem Header="Word Document" SymbolIcon="AlertUrgent24" />
      <MenuItem Header="PDF" InputGestureText="Ctrl+Alt+P" />
      <MenuItem Header="Text File" />
      <Separator />
      <MenuItem Header="Print" InputGestureText="Ctrl+P" />
  </ui:MenuItem>
  <MenuItem Header="With submenu">
      <MenuItem
          Header="Show status bar"
          IsCheckable="True"
          IsChecked="True" />
      <MenuItem Header="Word wrap" IsCheckable="True" />
      <ui:MenuItem
          Header="Checkable custom"
          IsCheckable="True"
          SymbolIcon="AlignTop24" />
      <MenuItem Header="NormalItem" />
      <ui:MenuItem Header="SubMenu" SymbolIcon="Balloon24">
          <ui:MenuItem Header="SubMenu 2" SymbolIcon="Diversity24">
              <ui:MenuItem Header="SubItem 2.1" SymbolIcon="Guardian24" />
          </ui:MenuItem>
          <MenuItem Header="SubItem 1" />
      </ui:MenuItem>
  </MenuItem>
  <MenuItem Header="Disabled" IsEnabled="False" />
  <Separator />
  <MenuItem Header="Close" />
</Menu>
```
As you can see, in some places we use the native `MenuItem` control, and in some places we use `wpfui: MenuItem`. You can mix them with each other freely.