# ADR-002: Control Library Architecture

## Status
Accepted

## Context
The WPF UI library provides 77+ Fluent Design System controls. The architecture must support:
- Clear code organization
- XAML implicit styling
- Type-safe intellisense
- Easy discovery for developers
- Maintainable codebase at scale

## Decision

### Folder-Per-Control Structure

Each control resides in its own subfolder under `src/Wpf.Ui/Controls/{ControlName}/`:

```
Controls/
├── Button/
│   ├── Button.cs              # Control class
│   └── Button.xaml            # Implicit style ResourceDictionary
├── NavigationView/
│   ├── NavigationView.Base.cs         # Core logic
│   ├── NavigationView.Properties.cs   # Dependency properties
│   ├── NavigationView.Events.cs       # Routed events
│   ├── NavigationView.Navigation.cs   # Navigation logic
│   ├── NavigationView.TemplateParts.cs # Template part bindings
│   ├── NavigationView.AttachedProperties.cs
│   └── NavigationView.xaml            # Implicit style
└── ContentDialog/
    ├── ContentDialog.cs
    ├── ContentDialog.FocusBehavior.cs  # Focused concern
    ├── ContentDialogHost.cs            # Host control
    ├── ContentDialogHostBehavior.cs
    ├── EventArgs/                      # Supporting types
    └── ContentDialog.xaml
```

**Benefits:**
- Physical isolation prevents coupling between controls
- Easy to locate all files related to a control
- Supports partial class decomposition for complex controls
- Clear ownership boundaries

### Paired .cs + .xaml Files

Each control consists of:
1. **{ControlName}.cs** - Control class with code-behind
2. **{ControlName}.xaml** - ResourceDictionary with implicit style

**Control Class Pattern:**
```csharp
// Controls/Button/Button.cs
namespace Wpf.Ui.Controls; // Flat namespace
// ReSharper disable once CheckNamespace

public class Button : System.Windows.Controls.Button, IAppearanceControl, IIconControl
{
    static Button()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(Button),
            new FrameworkPropertyMetadata(typeof(Button))
        );
    }

    // Dependency properties
    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register(nameof(Icon), ...);
}
```

**XAML Style Pattern:**
```xaml
<!-- Controls/Button/Button.xaml -->
<ResourceDictionary
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:controls="clr-namespace:Wpf.Ui.Controls">

    <Thickness x:Key="ButtonPadding">11,5,11,6</Thickness>

    <Style TargetType="{x:Type controls:Button}">
        <Setter Property="OverridesDefaultStyle" Value="True" />
        <Setter Property="SnapsToDevicePixels" Value="True" />
        <Setter Property="Template">
            <Setter.Value>
                <ControlTemplate TargetType="{x:Type controls:Button}">
                    <!-- Control template here -->
                </ControlTemplate>
            </Setter.Value>
        </Setter>
    </Style>
</ResourceDictionary>
```

### Flat Namespace Strategy

**All controls use a single namespace:** `Wpf.Ui.Controls`

```csharp
// Physical path: Controls/Button/Button.cs
namespace Wpf.Ui.Controls;  // NOT Wpf.Ui.Controls.Button
// ReSharper disable once CheckNamespace  // Suppress warning
```

**Rationale:**
- Simpler XAML namespace mapping (`xmlns:ui="http://schemas.lepo.co/wpfui/2022/xaml"`)
- No need for consumers to know physical folder structure
- Consistent with WPF framework controls (all in System.Windows.Controls)
- Better intellisense experience (all controls in one namespace dropdown)

**Trade-off:** IDE warning suppression required (`IDE0130: CheckNamespace`)

### Partial Class Decomposition

Complex controls split across multiple files:

**NavigationView example:**
- **NavigationView.Base.cs** - Core logic, template application
- **NavigationView.Properties.cs** - 27 dependency properties
- **NavigationView.Events.cs** - 7 routed events
- **NavigationView.Navigation.cs** - Navigation journal logic
- **NavigationView.TemplateParts.cs** - Template part fields and bindings
- **NavigationView.AttachedProperties.cs** - Attached properties

**ContentDialog example:**
- **ContentDialog.cs** - Main implementation (714 lines)
- **ContentDialog.FocusBehavior.cs** - Keyboard focus management

**Benefits:**
- Files stay under 300-500 lines (typically)
- Clear separation of concerns
- Easy to navigate specific aspects
- Reduces merge conflicts

**Naming Convention:** `{ControlName}.{Concern}.cs`

### DependencyProperty Pattern

**Registration:**
```csharp
/// <summary>Identifies the <see cref="Icon"/> dependency property.</summary>
public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
    nameof(Icon),
    typeof(IconElement),
    typeof(Button),
    new PropertyMetadata(null, OnIconChanged, IconElement.Coerce)
);
```

**CLR Wrapper:**
```csharp
[Bindable(true)]
[Category("Appearance")]
public IconElement? Icon
{
    get => (IconElement?)GetValue(IconProperty);
    set => SetValue(IconProperty, value);
}
```

**Callback Pattern:**
```csharp
private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
{
    if (d is Button button)
    {
        button.UpdateIconVisibility();
    }
}
```

### Capability Interfaces

Controls implement interfaces for cross-cutting capabilities:

#### IAppearanceControl
```csharp
public interface IAppearanceControl
{
    ControlAppearance Appearance { get; set; }
}

public enum ControlAppearance
{
    Primary, Secondary, Info, Dark, Light,
    Danger, Success, Caution, Transparent
}
```

**Used by:** Button, Badge, Snackbar, HyperlinkButton

#### IIconControl
```csharp
public interface IIconControl
{
    IconElement? Icon { get; set; }
}
```

**Used by:** Button, NavigationViewItem, AutoSuggestBox

#### IThemeControl
```csharp
public interface IThemeControl
{
    Appearance.ApplicationTheme ApplicationTheme { get; }
}
```

**Used by:** TitleBar, controls that need direct theme awareness

**Benefits:**
- Type-safe capability detection
- Shared behavior implementation
- Consistent property naming across controls

## Control Categories

### Window Chrome
FluentWindow, TitleBar, ClientAreaBorder, Window

### Navigation
NavigationView, NavigationViewItem, BreadcrumbBar, TabControl, TabView, Menu

### Buttons
Button, HyperlinkButton, DropDownButton, SplitButton, ToggleButton, ToggleSwitch

### Text Input
TextBox, PasswordBox, RichTextBox, AutoSuggestBox, NumberBox

### Dialogs & Overlays
ContentDialog, ContentDialogHost, MessageBox, Flyout, Snackbar, SnackbarPresenter

### Data Display
Card, InfoBar, InfoBadge, Badge, ListView, DataGrid, TreeView

### Pickers
CalendarDatePicker, DatePicker, TimePicker, ColorPicker

### Progress & Feedback
ProgressBar, ProgressRing, RatingControl, ThumbRate

### Icons
IconElement, FontIcon, SymbolIcon, ImageIcon, IconSourceElement

### Layout
Anchor, Page, Frame, Expander, Separator, Slider

## Enforcement

### MUST Follow

1. **Each control in its own subfolder** under `Controls/{ControlName}/`
2. **Paired .cs + .xaml files** with matching names
3. **Flat namespace** `Wpf.Ui.Controls` for all controls
4. **Static constructor** with `DefaultStyleKeyProperty.OverrideMetadata`
5. **Implicit style** in XAML with `TargetType` (no `x:Key`)
6. **`OverridesDefaultStyle=True`** in all control styles
7. **`SnapsToDevicePixels=True`** in all control styles
8. **XML documentation** with `<summary>` and `<example>` for public API

### MUST NOT Do

1. **Never nest controls** in subdirectories (keep flat under Controls/)
2. **Never use different namespace** from `Wpf.Ui.Controls`
3. **Never create keyed styles** as primary style (use implicit TargetType)
4. **Never reference controls** by folder structure in documentation

### Verification

- IDE0130 (CheckNamespace) suppressed in .editorconfig
- WpfAnalyzers enforces DependencyProperty correctness
- StyleCop rules enforce XML documentation (SA1600 suppressed for internal members)

## Consequences

### Positive
- **Highly Discoverable:** Single namespace for all 77+ controls
- **Scalable:** Adding new controls doesn't affect existing structure
- **Maintainable:** Clear boundaries and partial class decomposition
- **Type-Safe:** Interface-based capabilities enable polymorphism
- **Consistent:** Uniform naming and organization patterns

### Negative
- **IDE Warnings:** Namespace/folder mismatch requires suppression
- **Large Directory:** 77+ control folders in single directory
- **No Categorization:** Physical structure doesn't reflect logical categories
- **Partial Class Complexity:** Large controls split across many files

## Alternatives Considered

### Nested Category Folders
```
Controls/
├── Buttons/
│   ├── Button/
│   └── ToggleSwitch/
└── Navigation/
    └── NavigationView/
```

**Rejected:** Would require category-specific namespaces or deeper folder/namespace mismatch.

### Category-Based Namespaces
```csharp
namespace Wpf.Ui.Controls.Buttons;
namespace Wpf.Ui.Controls.Navigation;
```

**Rejected:** Requires consumers to know category classification. Inconsistent with WPF framework patterns.

### Single File Per Control
**Rejected:** Controls like NavigationView have 1000+ lines. Unmanageable in single file.

## References
- WPF Framework Control Architecture: `System.Windows.Controls` namespace
- WinUI 3 Control Architecture: Flat namespace strategy
- [WPF Control Authoring](https://docs.microsoft.com/dotnet/desktop/wpf/controls/control-authoring-overview)
