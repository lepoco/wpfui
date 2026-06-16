# ADR-005: Feature Folder Organization for Controls

## Status
Accepted

## Context
With 77+ controls in the library, code organization becomes critical for maintainability. The structure must support:
- Clear isolation between controls
- Easy location of control-related files
- Logical grouping of complex control components
- Scalability as new controls are added

## Decision

### Feature Folder per Control

Each control resides in `Controls/{ControlName}/` directory:

```
Controls/
├── Button/
│   ├── Button.cs
│   └── Button.xaml
├── Card/
│   ├── Card.cs
│   └── Card.xaml
├── NavigationView/
│   ├── NavigationView.Base.cs
│   ├── NavigationView.Properties.cs
│   ├── NavigationView.Events.cs
│   ├── NavigationView.Navigation.cs
│   ├── NavigationView.TemplateParts.cs
│   ├── NavigationView.AttachedProperties.cs
│   ├── NavigationView.xaml
│   ├── NavigationViewItem.cs
│   ├── NavigationViewItemHeader.cs
│   └── NavigationViewItemSeparator.cs
```

**One control = one folder** with all related files.

## Structure Patterns

### Simple Controls
**Pattern:** Single .cs + .xaml pair

```
Badge/
├── Badge.cs        # Control implementation
└── Badge.xaml      # Implicit style
```

**Applies to:** Button, Badge, Card, InfoBar, TextBox, etc. (50+ controls)

### Complex Controls with Partial Classes
**Pattern:** Multiple partial class files organized by concern

```
NavigationView/
├── NavigationView.Base.cs              # Core control logic
├── NavigationView.Properties.cs         # 27 dependency properties
├── NavigationView.Events.cs            # 7 routed events
├── NavigationView.Navigation.cs         # Page navigation logic
├── NavigationView.TemplateParts.cs     # Template part fields/binding
├── NavigationView.AttachedProperties.cs # Attached property definitions
└── NavigationView.xaml                 # Implicit style + template
```

**Partial class naming:** `{ControlName}.{Concern}.cs`

**Applies to:** NavigationView, ContentDialog, TitleBar

### Controls with Related Types
**Pattern:** Additional related controls in same folder

```
NavigationView/
├── NavigationView*.cs                   # Main control (6 files)
├── NavigationView.xaml
├── NavigationViewItem.cs               # Item container
├── NavigationViewItemHeader.cs         # Header item
├── NavigationViewItemSeparator.cs      # Separator
├── NavigationViewContentPresenter.cs   # Content host
├── INavigationView.cs                  # Interface
└── INavigationViewItem.cs              # Item interface
```

**Rationale:** Tightly coupled types that are only used together.

### Controls with Supporting Subdirectories
**Pattern:** Subfolder for supporting types

```
ContentDialog/
├── ContentDialog.cs
├── ContentDialog.FocusBehavior.cs
├── ContentDialog.xaml
├── ContentDialogHost.cs
├── ContentDialogHostBehavior.cs
└── EventArgs/
    ├── ContentDialogButtonClickEventArgs.cs
    ├── ContentDialogClosingEventArgs.cs
    └── ContentDialogClosedEventArgs.cs
```

**When to use subfolder:**
- 3+ related types (EventArgs, Converters, Enums)
- Clear sub-component (e.g., EventArgs)

## Flat Namespace Strategy

**All controls use:** `Wpf.Ui.Controls` namespace (regardless of folder depth)

```csharp
// File: Controls/NavigationView/NavigationView.cs
namespace Wpf.Ui.Controls;  // Flat namespace, not Wpf.Ui.Controls.NavigationView
// ReSharper disable once CheckNamespace
```

**Trade-off:** Folder structure does not match namespace.

**IDE Configuration Required:**
```ini
# .editorconfig
dotnet_diagnostic.IDE0130.severity = none  # Suppress namespace/folder mismatch
```

## Partial Class Decomposition Strategies

### By Concern
NavigationView splits by logical concern:
- **Base.cs** - Control infrastructure (template application, initialization)
- **Properties.cs** - Dependency property definitions
- **Events.cs** - Routed event definitions
- **Navigation.cs** - Page navigation logic
- **TemplateParts.cs** - Template part fields and OnApplyTemplate logic
- **AttachedProperties.cs** - Attached property definitions

### By Feature
ContentDialog splits by feature:
- **ContentDialog.cs** - Main implementation (async ShowAsync, dialog lifecycle)
- **ContentDialog.FocusBehavior.cs** - Keyboard focus management (isolated behavior)

### Guidelines for Splitting

**When to split:**
- File exceeds 500 lines
- Clear separation of concerns exists (properties vs. logic)
- Feature is self-contained and isolatable

**How to name:**
- **Base.cs** - Core control logic (template, initialization)
- **Properties.cs** - All dependency properties
- **Events.cs** - All routed events
- **{Feature}.cs** - Specific feature (FocusBehavior, Animation, etc.)

**Don't split:**
- Controls under 300 lines
- No clear separation of concerns
- When references would create circular dependencies

## File Naming Conventions

### Control Classes
```
{ControlName}.cs              # Simple control
{ControlName}.{Concern}.cs    # Partial class with concern
```

### XAML Styles
```
{ControlName}.xaml            # Implicit style ResourceDictionary
```

### Supporting Types
```
{TypePurpose}{ControlName}.cs # e.g., NavigationViewItem
{ControlName}{Purpose}.cs     # e.g., ContentDialogHost
I{ControlName}.cs             # Interface
```

### Event Arguments
```
{ControlName}{EventName}EventArgs.cs
```

Examples:
- `ContentDialogButtonClickEventArgs.cs`
- `NavigatedEventArgs.cs`

## Enforcement

### MUST Follow

1. **One control = one folder** under `Controls/`
2. **Paired .cs + .xaml** with matching names
3. **Flat namespace** `Wpf.Ui.Controls` for all controls
4. **Partial class naming** `{ControlName}.{Concern}.cs`
5. **ReSharper suppress comment** when namespace doesn't match folder:
   ```csharp
   namespace Wpf.Ui.Controls;
   // ReSharper disable once CheckNamespace
   ```

6. **Keep related types together** in same folder when tightly coupled

### MUST NOT Do

1. **Never nest control folders** (keep flat under Controls/)
   ```
   ❌ Controls/Buttons/Button/
   ✅ Controls/Button/
   ```

2. **Never use category-specific namespace**
   ```csharp
   ❌ namespace Wpf.Ui.Controls.Buttons;
   ✅ namespace Wpf.Ui.Controls;
   ```

3. **Never split files arbitrarily** (must have clear separation of concerns)

4. **Never create more than 2 directory levels** under Controls/
   ```
   ✅ Controls/ContentDialog/EventArgs/
   ❌ Controls/ContentDialog/EventArgs/Closing/
   ```

### Verification

1. **IDE0130 suppressed** in .editorconfig for namespace/folder mismatch
2. **Code review** checks folder organization
3. **Naming conventions** enforced during PR review

## Benefits

### Developer Experience

**Easy to Find:**
```bash
# Looking for Button control?
Controls/Button/Button.cs         # Immediately obvious location
```

**Clear Boundaries:**
- All Button-related code in `Controls/Button/`
- No cross-control dependencies (enforced by folder isolation)

**Scalable:**
- Adding new control = create new folder
- Doesn't affect existing controls

### Maintainability

**Isolation:**
- Changes to Button don't affect Card
- Merge conflicts localized to single control

**Discoverability:**
- New developers find controls by folder name
- Related types co-located (NavigationViewItem with NavigationView)

**Refactoring:**
- Easy to split large files (add `.Properties.cs`)
- Clear when to split (file size, concern separation)

### Build Performance

**Partial Classes:**
- Compiler can parallelize partial class compilation
- Changes to Properties.cs don't trigger recompilation of Navigation.cs

## Trade-offs

### Advantages
- ✅ Clear physical organization
- ✅ Easy to locate control files
- ✅ Enforces encapsulation (hard to reference across control folders)
- ✅ Scales to 100+ controls
- ✅ Supports complex controls with many files

### Disadvantages
- ❌ Folder/namespace mismatch requires suppression
- ❌ 77+ folders in single directory (large directory)
- ❌ No physical categorization (all controls appear equal)
- ❌ Related controls may be far apart alphabetically (Button vs. ToggleButton)

## Alternatives Considered

### Category-Based Folders
```
Controls/
├── Buttons/
│   ├── Button/
│   └── ToggleButton/
└── Navigation/
    └── NavigationView/
```

**Rejected:**
- Requires category-specific namespaces or deeper mismatch
- Category classification is subjective (is ToggleSwitch a Button or Input?)
- Doesn't align with flat namespace strategy

### Single File Per Control
```
Controls/
├── Button.cs
├── Button.xaml
├── NavigationView.cs
└── NavigationView.xaml
```

**Rejected:**
- Complex controls (NavigationView 1000+ lines) unmanageable
- Supporting types have unclear location
- 150+ files in single directory

### Hybrid Categorization
```
Controls/
├── Button/
├── Input/
│   ├── TextBox/
│   └── NumberBox/
└── Navigation/
    └── NavigationView/
```

**Rejected:**
- Inconsistent structure (some categories, some not)
- Unclear where new controls go
- Complicates namespace strategy

## Migration Path

### Adding New Simple Control

1. Create folder `Controls/{NewControl}/`
2. Add `{NewControl}.cs` with control class
3. Add `{NewControl}.xaml` with implicit style
4. Add ReSharper suppress comment to .cs file

### Splitting Existing Control

Example: Card becomes too large

**Before:**
```
Card/
├── Card.cs (500 lines)
└── Card.xaml
```

**After:**
```
Card/
├── Card.Base.cs (200 lines - core logic)
├── Card.Properties.cs (100 lines - dependency properties)
├── Card.Animation.cs (100 lines - animation logic)
└── Card.xaml
```

**Refactoring steps:**
1. Extract dependency properties to `Card.Properties.cs`
2. Extract animation logic to `Card.Animation.cs`
3. Keep core logic in `Card.Base.cs`
4. All files use `partial class Card`

## Documentation

### Control Folder README
Each complex control folder includes README.md:

```markdown
# NavigationView

Complex navigation container with 6 partial class files:

- **Base.cs** - Core control logic, template application
- **Properties.cs** - 27 dependency properties
- **Events.cs** - 7 routed events
- **Navigation.cs** - Page navigation, journal, back/forward
- **TemplateParts.cs** - Template part bindings
- **AttachedProperties.cs** - HeaderContent attached property

Related types:
- NavigationViewItem - Selectable item container
- NavigationViewItemHeader - Non-selectable header
- INavigationView - Public control interface
```

## References
- [Feature Folders in ASP.NET](https://docs.microsoft.com/archive/msdn-magazine/2016/september/asp-net-core-feature-slices-for-asp-net-core-mvc) (similar pattern)
- [Vertical Slice Architecture](https://jimmybogard.com/vertical-slice-architecture/)
