# Controls
WPF UI has two kinds of controls. Default WPF ones, which styles have been overridden, and custom proprietary controls like `ProgressRing`.

### Access to custom controls
In order for your `Window`, `Page`, or `UserControl` to use custom WPF UI controls, you need to add `wpfui` namespace.
```xml
<Window
  xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
  xmlns:wpfui="clr-namespace:WPFUI.Controls;assembly=WPFUI"
  mc:Ignorable="d">
  <wpfui:ProgressRing IsIndeterminate="True" />
</Window>
```
