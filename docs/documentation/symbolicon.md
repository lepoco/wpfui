# SymbolIcon

`SymbolIcon` is a control responsible for rendering icons.

### Implementation

```csharp
class Wpf.Ui.Controls.SymbolIcon
```

## Exposes

```csharp
// Gets or sets displayed symbol
SymbolIcon.Symbol = SymbolRegular.Empty;
```

```csharp
// Defines whether or not we should use the SymbolFilled
SymbolIcon.Filled = false;
```

```csharp
// Icon foreground
SymbolIcon.Foreground = Brushes.White;
```

```csharp
// Icon size
SymbolIcon.FontSize = 16;
```

### How to use

```xml
<ui:SymbolIcon
  Symbol="Fluent24"
  Filled="False"
  FontSize="16"
  Foreground="White"/>
```
