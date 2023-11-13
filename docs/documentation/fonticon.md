# FontIcon

`FontIcon` is a control responsible for rendering icons based on the provided font.

### Implementation

```csharp
class Wpf.Ui.Controls.FontIcon
```

## Exposes

```csharp
// Gets or sets displayed glyph
FontIcon.Glyph = '\uE00B';
```

```csharp
// Gets or sets used font family
FontIcon.FontFamily = "Segoe Fluent Icons";
```

```csharp
// Icon foreground
FontIcon.Foreground = Brushes.White;
```

```csharp
// Icon size
FontIcon.FontSize = 16;
```

### How to use

```xml
<ui:FontIcon
  Glyph="&#xe00b;"
  FontFamily="{DynamicResource SegoeFluentIcons}"
  FontSize="16"
  Foreground="White"/>
```
