# Input
You can handle input from your users in a variety of ways. **WPF UI** modifies the default controls, but also adds its own, like `NumberBox`.

## TextBox
The default TextBox only allows you to define the `Text` parameter.
```xml
<TextBox
  Margin="0,8,0,0"
  Text="Hello World" />
```

**WPF UI** extends it and allows you to set `Placeholder` or `Icon`.
```xml
<wpfui:TextBox
  Margin="0,8,0,0"
  Icon="Fluent24"
  Placeholder="Hello World" />
```
