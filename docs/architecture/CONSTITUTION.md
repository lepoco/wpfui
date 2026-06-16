# WPF UI Project Constitution v4.2.0

> **Spec-Driven Development (SDD) Document**
> Single source of truth for coding conventions, boundary rules, and project-specific guidelines.
> Intended audience: AI coding agents and human contributors.

---

## Table of Contents

1. [Project Identity](#1-project-identity)
2. [Coding Conventions](#2-coding-conventions)
   - [Namespaces](#21-namespaces)
   - [Nullable Reference Types](#22-nullable-reference-types)
   - [Private Fields](#23-private-fields)
   - [File Headers](#24-file-headers)
   - [XML Documentation](#25-xml-documentation)
   - [Dependency Properties](#26-dependency-properties)
   - [Control Authoring](#27-control-authoring)
   - [XAML Styles](#28-xaml-styles)
   - [Service Pattern](#29-service-pattern)
   - [Error Handling](#210-error-handling)
   - [Win32 Interop](#211-win32-interop)
   - [Test Naming](#212-test-naming)
   - [MVVM (Gallery / Samples)](#213-mvvm-gallery--samples)
   - [Central Package Management](#214-central-package-management)
   - [Commit Convention](#215-commit-convention)
3. [Boundary System](#3-boundary-system)
   - [ALWAYS DO](#31-always-do)
   - [ASK FIRST](#32-ask-first)
   - [NEVER DO](#33-never-do)
4. [Rules Files Summary](#4-rules-files-summary)

---

## 1. Project Identity

| Field | Value |
|---|---|
| **Name** | WPF UI (wpfui) |
| **Version** | 4.2.0 |
| **Type** | Open-source WPF UI control library implementing Microsoft Fluent Design System |
| **License** | MIT |
| **Language** | C# 14 (`LangVersion: preview`) / XAML |
| **Platforms** | .NET 10/9/8 (windows TFMs) + .NET Framework 4.6.2/4.7.2/4.8.1 |
| **NuGet Package ID** | WPF-UI |
| **XAML Namespace** | `http://schemas.lepo.co/wpfui/2022/xaml` (prefix `ui`) |
| **Repository** | https://github.com/lepoco/wpfui |
| **Formatter** | CSharpier (110 char width, 4-space indent, no tabs) |
| **Analyzers** | StyleCop, AsyncFixer, IDisposableAnalyzers, WpfAnalyzers (all enforced in build) |
| **Test Frameworks** | XUnit v3, NSubstitute, AwesomeAssertions, FlaUI, Coverlet |
| **Assembly Signing** | `src/lepo.snk` for release builds |

### Project Structure

| Project | Purpose | TFMs |
|---|---|---|
| `src/Wpf.Ui/` | Core library (NuGet: WPF-UI) | net10.0-windows, net9.0-windows, net8.0-windows, net481, net472, net462 |
| `src/Wpf.Ui.Abstractions/` | Interfaces and abstractions | Includes netstandard2.0/2.1 |
| `src/Wpf.Ui.DependencyInjection/` | Microsoft.Extensions.DependencyInjection integration | Multi-target |
| `src/Wpf.Ui.Tray/` | System tray support | Multi-target |
| `src/Wpf.Ui.ToastNotifications/` | Toast notification support | Multi-target |
| `src/Wpf.Ui.Gallery/` | Demo/showcase application | net10.0-windows10.0.26100.0 |
| `src/Wpf.Ui.Extension/` | Visual Studio 2022 extension with project templates | VS SDK |
| `samples/` | Sample apps (MVVM, DI, console hosting) | Various |

### Core Library Layout (`src/Wpf.Ui/`)

| Directory | Contents |
|---|---|
| `Controls/` | Each control in its own subfolder with .cs + .xaml |
| `Appearance/` | `ApplicationThemeManager`, `ApplicationAccentColorManager` |
| `Win32/` | Raw P/Invoke declarations (native methods, structs, enums) |
| `Interop/` | Managed wrappers around Win32 APIs |
| `Converters/` | XAML value converters |
| `AutomationPeers/` | UI Automation/accessibility support |
| `Resources/Fonts/` | Embedded Fluent System Icons (Filled + Regular) |
| Root (`*.cs`) | Service interfaces (`I{Name}Service`) and implementations (`{Name}Service`) |

---

## 2. Coding Conventions

### 2.1 Namespaces

All C# files use **file-scoped namespaces**. All controls use the flat `Wpf.Ui.Controls` namespace regardless of subfolder depth, with a ReSharper suppression comment.

```csharp
// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

public class Button : System.Windows.Controls.Button, IAppearanceControl, IIconControl
{
    // ...
}
```

Services use the `Wpf.Ui` namespace:

```csharp
namespace Wpf.Ui;

public class SnackbarService : ISnackbarService
{
    // ...
}
```

Gallery app follows folder-based namespaces:

```csharp
namespace Wpf.Ui.Gallery.ViewModels.Pages.BasicInput;

public partial class AnchorViewModel : ViewModel
{
    // ...
}
```

### 2.2 Nullable Reference Types

Nullable is enabled globally via `Directory.Build.props`. Use `is not null` pattern matching over `!= null`.

```csharp
// Good
if (_presenter is null)
{
    throw new InvalidOperationException($"The SnackbarPresenter was never set");
}

_snackbar ??= new Snackbar(_presenter);

// Good - nullable property types
public IconElement? Icon
{
    get => (IconElement?)GetValue(IconProperty);
    set => SetValue(IconProperty, value);
}
```

### 2.3 Private Fields

Private fields use `_camelCase` (underscore prefix). This is enforced by `.editorconfig` naming rules.

```csharp
private bool _valueUpdating;
private SnackbarPresenter? _presenter;
private Snackbar? _snackbar;
```

In Gallery ViewModels with CommunityToolkit.Mvvm, the same convention applies:

```csharp
[ObservableProperty]
private bool _isAnchorEnabled = true;
```

### 2.4 File Headers

**C# files** -- MIT license header enforced as a build error via `IDE0073`:

```csharp
// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.
```

**XAML files** -- XML comment block with MIT license and Microsoft credit where applicable:

```xml
<!--
    This Source Code Form is subject to the terms of the MIT License.
    If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
    Copyright (C) Leszek Pomianowski and WPF UI Contributors.
    All Rights Reserved.

    Based on Microsoft XAML for Win UI
    Copyright (c) Microsoft Corporation. All Rights Reserved.
-->
```

### 2.5 XML Documentation

Public APIs require `<summary>` and `<example>` tags. Examples show XAML usage with `<code lang="xml">` and HTML-escaped angle brackets.

**Control class documentation:**

```csharp
/// <summary>
/// Creates a hyperlink to web pages, files, email addresses, locations in the same page,
/// or anything else a URL can address.
/// </summary>
/// <example>
/// <code lang="xml">
/// &lt;ui:Anchor
///     NavigateUri="https://lepo.co/" /&gt;
/// </code>
/// </example>
public class Anchor : Wpf.Ui.Controls.HyperlinkButton { }
```

**Control with multiple XAML examples:**

```csharp
/// <summary>
/// Inherited from the <see cref="System.Windows.Controls.Button"/>,
/// adding <see cref="SymbolRegular"/>.
/// </summary>
/// <example>
/// <code lang="xml">
/// &lt;ui:Button
///     Appearance="Primary"
///     Content="WPF UI button with font icon"
///     Icon="{ui:SymbolIcon Symbol=Fluent24}" /&gt;
/// </code>
/// <code lang="xml">
/// &lt;ui:Button
///     Appearance="Primary"
///     Content="WPF UI button with font icon"
///     Icon="{ui:FontIcon '&#x1F308;'}" /&gt;
/// </code>
/// </example>
```

**C# code examples (e.g., for static managers):**

```csharp
/// <example>
/// <code lang="csharp">
/// ApplicationThemeManager.Apply(
///     ApplicationTheme.Light
/// );
/// </code>
/// </example>
```

**DependencyProperty documentation** uses a single-line `<summary>`:

```csharp
/// <summary>Identifies the <see cref="Icon"/> dependency property.</summary>
public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
    nameof(Icon),
    typeof(IconElement),
    typeof(Button),
    new PropertyMetadata(null, null, IconElement.Coerce)
);
```

**Internal code**: Avoid comments unless explaining Win32 interop/marshalling. Never add comments that restate what the code does.

### 2.6 Dependency Properties

Use `DependencyProperty.Register` with `nameof()`. Always include XML doc with the `Identifies the ... dependency property` pattern.

```csharp
/// <summary>Identifies the <see cref="Icon"/> dependency property.</summary>
public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
    nameof(Icon),
    typeof(IconElement),
    typeof(Button),
    new PropertyMetadata(null, null, IconElement.Coerce)
);

/// <summary>
/// Gets or sets displayed <see cref="IconElement"/>.
/// </summary>
[Bindable(true)]
[Category("Appearance")]
public IconElement? Icon
{
    get => (IconElement?)GetValue(IconProperty);
    set => SetValue(IconProperty, value);
}
```

For two-way binding with specific update triggers:

```csharp
/// <summary>Identifies the <see cref="Value"/> dependency property.</summary>
public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
    nameof(Value),
    typeof(double?),
    typeof(NumberBox),
    new FrameworkPropertyMetadata(
        null,
        FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
        OnValueChanged,
        null,
        false,
        UpdateSourceTrigger.LostFocus
    )
);
```

### 2.7 Control Authoring

Each control resides in its own subfolder under `Controls/{Name}/` containing at minimum:
- `{Name}.cs` -- C# control class
- `{Name}.xaml` -- XAML ResourceDictionary with styles and templates

Complex controls are split into partial classes:
- `NavigationView.Base.cs` -- Static constructor, instance constructor
- `NavigationView.Properties.cs` -- DependencyProperty declarations
- `NavigationView.Events.cs` -- RoutedEvent declarations
- `NavigationView.TemplateParts.cs` -- Template part handling
- `NavigationView.Navigation.cs` -- Navigation logic

**Static constructor pattern** (required for custom-styled controls):

```csharp
static NavigationView()
{
    DefaultStyleKeyProperty.OverrideMetadata(
        typeof(NavigationView),
        new FrameworkPropertyMetadata(typeof(NavigationView))
    );
}
```

**Template parts pattern:**

```csharp
[TemplatePart(Name = PART_ClearButton, Type = typeof(Button))]
[TemplatePart(Name = PART_InlineIncrementButton, Type = typeof(RepeatButton))]
[TemplatePart(Name = PART_InlineDecrementButton, Type = typeof(RepeatButton))]
public partial class NumberBox : Wpf.Ui.Controls.TextBox
{
    private const string PART_ClearButton = nameof(PART_ClearButton);
    private const string PART_InlineIncrementButton = nameof(PART_InlineIncrementButton);
    private const string PART_InlineDecrementButton = nameof(PART_InlineDecrementButton);
}
```

**Base class selection:**

| Base Class | Use For |
|---|---|
| `System.Windows.Controls.ContentControl` | Card, Badge, InfoBar, Snackbar |
| `System.Windows.Controls.Button` | Button |
| `System.Windows.Controls.Control` | TitleBar, ProgressRing, NavigationView |
| `System.Windows.Controls.Primitives.ToggleButton` | ToggleSwitch |
| `System.Windows.Window` | FluentWindow |

### 2.8 XAML Styles

Control styles require `OverridesDefaultStyle=True` and `SnapsToDevicePixels=True`. Use `DynamicResource` for theme-dependent values and `StaticResource` for constants.

```xml
<ResourceDictionary
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:controls="clr-namespace:Wpf.Ui.Controls">

    <!-- Constants use StaticResource -->
    <Thickness x:Key="ButtonPadding">11,5,11,6</Thickness>
    <Thickness x:Key="ButtonBorderThemeThickness">1</Thickness>

    <Style x:Key="DefaultRepeatButtonStyle" TargetType="{x:Type RepeatButton}">
        <Setter Property="FocusVisualStyle" Value="{DynamicResource DefaultControlFocusVisualStyle}" />
        <Setter Property="Background" Value="{DynamicResource ButtonBackground}" />
        <Setter Property="Foreground" Value="{DynamicResource ButtonForeground}" />
        <Setter Property="BorderBrush" Value="{DynamicResource ControlElevationBorderBrush}" />
        <Setter Property="BorderThickness" Value="{StaticResource ButtonBorderThemeThickness}" />
        <Setter Property="Padding" Value="{StaticResource ButtonPadding}" />
        <Setter Property="SnapsToDevicePixels" Value="True" />
        <Setter Property="OverridesDefaultStyle" Value="True" />
    </Style>
</ResourceDictionary>
```

**Resource key naming conventions:**

| Type | Pattern | Example |
|---|---|---|
| Brushes | `{Name}Brush` | `ButtonForeground`, `TextFillColorPrimaryBrush` |
| Colors | `{Name}Color` | `AccentColor` |
| Thickness | `{ControlName}{Property}` | `ButtonPadding`, `CardBorderThemeThickness` |
| Doubles | `{ControlName}{Property}` | `ToggleButtonWidth` |

### 2.9 Service Pattern

Services follow the `I{Name}Service` / `{Name}Service` naming convention. Both interface and implementation live at the `src/Wpf.Ui/` root level (NOT in a `Services/` subfolder).

**Interface (`src/Wpf.Ui/ISnackbarService.cs`):**

```csharp
namespace Wpf.Ui;

/// <summary>
/// Represents a contract with the service that provides global <see cref="Snackbar"/>.
/// </summary>
public interface ISnackbarService
{
    TimeSpan DefaultTimeOut { get; set; }
    void SetSnackbarPresenter(SnackbarPresenter contentPresenter);
    SnackbarPresenter? GetSnackbarPresenter();
    void Show(string title, string message, ControlAppearance appearance, IconElement? icon, TimeSpan timeout);
}
```

**Implementation (`src/Wpf.Ui/SnackbarService.cs`):**

```csharp
namespace Wpf.Ui;

/// <summary>
/// A service that provides methods related to displaying the <see cref="Snackbar"/>.
/// </summary>
public class SnackbarService : ISnackbarService
{
    private SnackbarPresenter? _presenter;
    private Snackbar? _snackbar;

    /// <inheritdoc />
    public TimeSpan DefaultTimeOut { get; set; } = TimeSpan.FromSeconds(5);

    /// <inheritdoc />
    public void Show(
        string title,
        string message,
        ControlAppearance appearance,
        IconElement? icon,
        TimeSpan timeout
    )
    {
        if (_presenter is null)
        {
            throw new InvalidOperationException($"The SnackbarPresenter was never set");
        }

        _snackbar ??= new Snackbar(_presenter);
        // ...
    }
}
```

**Available services:**

| Interface | Implementation | Purpose |
|---|---|---|
| `INavigationService` | `NavigationService` | Page navigation |
| `IContentDialogService` | `ContentDialogService` | Modal content dialogs |
| `ISnackbarService` | `SnackbarService` | Toast-like snackbar notifications |
| `ITaskBarService` | `TaskBarService` | Windows taskbar integration |
| `IThemeService` | `ThemeService` | Theme management |

### 2.10 Error Handling

**Win32/Interop code** -- Bare catch blocks swallowing exceptions (intentional for cross-OS compatibility):

```csharp
catch
{
    // Ignore registry access errors on non-Windows or restricted environments
    return null;
}
```

**Service code** -- Throw `ArgumentNullException` / `InvalidOperationException` for precondition violations:

```csharp
if (_presenter is null)
{
    throw new InvalidOperationException($"The SnackbarPresenter was never set");
}
```

```csharp
protected void ThrowIfNavigationControlIsNull()
{
    if (NavigationControl is null)
    {
        throw new ArgumentNullException(nameof(NavigationControl));
    }
}
```

### 2.11 Win32 Interop

The project uses a three-layer architecture for native Windows API access.

**Layer 1: CsWin32 auto-generated P/Invoke** via `Microsoft.Windows.CsWin32` NuGet package. Native function names are listed in `src/Wpf.Ui/NativeMethods.txt`.

**Layer 2: Managed wrappers with validation** (`src/Wpf.Ui/Interop/UnsafeNativeMethods.cs`):

```csharp
internal static class UnsafeNativeMethods
{
    public static unsafe bool ApplyWindowCornerPreference(
        IntPtr handle,
        WindowCornerPreference cornerPreference
    )
    {
        // ALWAYS validate handle before calling Win32 APIs
        if (handle == IntPtr.Zero)
        {
            return false;
        }

        if (!PInvoke.IsWindow(new HWND(handle)))
        {
            return false;
        }

        DWM_WINDOW_CORNER_PREFERENCE pvAttribute = UnsafeReflection.Cast(cornerPreference);

        return PInvoke.DwmSetWindowAttribute(
                new HWND(handle),
                DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE,
                &pvAttribute,
                sizeof(int)
            ) == HRESULT.S_OK;
    }
}
```

**Layer 3: Utility helpers** (`src/Wpf.Ui/Win32/`):

```csharp
internal sealed class Utilities
{
    public static bool IsOSWindows11OrNewer => _osVersion.Build >= 22000;
}
```

**Critical rule**: Always validate handles (`IntPtr.Zero` check + `PInvoke.IsWindow`) before any native API call. Always search the existing codebase before assuming standard WPF approaches work for TitleBar, window management, system theme detection, or DWM integration.

### 2.12 Test Naming

**Unit tests** use `MethodName_ExpectedResult_WhenCondition`:

```csharp
public class TransitionAnimationProviderTests
{
    [Fact]
    public void ApplyTransition_ReturnsFalse_WhenDurationIsLessThan10()
    {
        UIElement mockedUiElement = Substitute.For<UIElement>();

        var result = TransitionAnimationProvider.ApplyTransition(mockedUiElement, Transition.FadeIn, -10);

        Assert.False(result);
    }

    [Fact]
    public void ApplyTransition_ReturnsFalse_WhenElementIsNull()
    {
        var result = TransitionAnimationProvider.ApplyTransition(null, Transition.FadeIn, 1000);

        Assert.False(result);
    }
}
```

**Integration tests** use the `UiTest` base class with FlaUI and AwesomeAssertions:

```csharp
public sealed class TitleBarTests : UiTest
{
    [Fact]
    public async Task CloseButton_ShouldCloseWindow_WhenClicked()
    {
        Button? closeButton = FindFirst("TitleBarCloseButton").AsButton();

        closeButton.Should().NotBeNull("because CloseButton should be present in the main window title bar");
        closeButton.Click(moveMouse: false);

        await Wait(2);

        Application
            ?.HasExited.Should()
            .BeTrue("because the main window should be closed after clicking the close button");
    }
}
```

### 2.13 MVVM (Gallery / Samples)

The Gallery app uses CommunityToolkit.Mvvm. ViewModels are `partial` classes inheriting from `ViewModel` (which extends `ObservableObject`).

**ViewModel (`src/Wpf.Ui.Gallery/ViewModels/Pages/BasicInput/AnchorViewModel.cs`):**

```csharp
namespace Wpf.Ui.Gallery.ViewModels.Pages.BasicInput;

public partial class AnchorViewModel : ViewModel
{
    [ObservableProperty]
    private bool _isAnchorEnabled = true;

    [RelayCommand]
    private void OnAnchorCheckboxChecked(object sender)
    {
        if (sender is not CheckBox checkbox)
        {
            return;
        }

        IsAnchorEnabled = !(checkbox?.IsChecked ?? false);
    }
}
```

**Page (`src/Wpf.Ui.Gallery/Views/Pages/BasicInput/AnchorPage.xaml.cs`):**

```csharp
namespace Wpf.Ui.Gallery.Views.Pages.BasicInput;

[GalleryPage("Button which opens a link.", SymbolRegular.CubeLink20)]
public partial class AnchorPage : INavigableView<AnchorViewModel>
{
    public AnchorViewModel ViewModel { get; init; }

    public AnchorPage(AnchorViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }
}
```

Key MVVM conventions:
- `[ObservableProperty]` for `_camelCase` backing fields (generates PascalCase properties)
- `[RelayCommand]` for `On`-prefixed private methods (generates `{Name}Command` ICommand)
- Pattern matching with `is not` for type checks in command handlers
- `DataContext = this` set in page constructor (not in XAML)
- `INavigableView<TViewModel>` interface for navigation-aware pages

### 2.14 Central Package Management

All NuGet package versions are centralized in `Directory.Packages.props`. Individual `.csproj` files reference packages without version attributes.

**`Directory.Packages.props` (versions defined here):**

```xml
<Project>
  <ItemGroup>
    <PackageVersion Include="CommunityToolkit.Mvvm" Version="8.4.0" />
    <PackageVersion Include="Microsoft.Windows.CsWin32" Version="0.3.242" />
    <PackageVersion Include="StyleCop.Analyzers" Version="1.2.0-beta.556" />
    <PackageVersion Include="WpfAnalyzers" Version="4.1.1" />
    <PackageVersion Include="xunit.v3" Version="3.2.0" />
    <!-- ... -->
  </ItemGroup>
</Project>
```

**`.csproj` (NO version attribute):**

```xml
<PackageReference Include="Microsoft.Windows.CsWin32">
  <PrivateAssets>all</PrivateAssets>
  <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
</PackageReference>
```

### 2.15 Commit Convention

**Format**: `type(scope): description (#PR_NUMBER)`

| Type | Use For |
|---|---|
| `feat` | New feature |
| `fix` | Bug fix |
| `chore` | Build/tooling changes |

| Scope | Use For |
|---|---|
| `controls` | Control-related changes |
| `app` | Gallery app changes |
| `docs` | Documentation changes |

**Examples from actual commits:**

```
fix(controls): Fix TextBlock issues (#1640)
fix(controls): Remove AnimationFactorToValueConverter from CheckBox.xaml (#1649)
fix(controls): NavigationViewItemAutomationPeer followup (#1646)
fix(controls): Update ContentDialog For TitleBar CenterContent (#1642)
feat(app): Add ability to disable automatic apply of system accent color in UiApplication (#1643)
```

---

## 3. Boundary System

### 3.1 ALWAYS DO

These practices are mandatory. Violations will be caught by analyzers, build errors, or code review.

| # | Rule | Enforcement |
|---|---|---|
| A1 | Use file-scoped namespaces in all C# files | `.editorconfig` `csharp_style_namespace_declarations = file_scoped:warning` |
| A2 | Enable nullable reference types | `Directory.Build.props` `<Nullable>enable</Nullable>` |
| A3 | Add MIT license file header to every .cs and .xaml file | `.editorconfig` `IDE0073 = error` |
| A4 | Add XML docs with `<summary>` and `<example>` tags for all public APIs | Code review |
| A5 | Use CSharpier for formatting (110 char, 4-space indent) | `.csharpierrc`, CI |
| A6 | Use Central Package Management (versions in `Directory.Packages.props` only) | `Directory.Build.props` `<ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>` |
| A7 | Validate Win32 handles before any native API call (`IntPtr.Zero` + `PInvoke.IsWindow`) | Code review |
| A8 | Use pattern matching (`is not`) over type checks with casts | `.editorconfig` `csharp_style_pattern_matching_over_is_with_cast_check = true:suggestion` |
| A9 | Use `DependencyProperty.Register` with `nameof()` | Code review, WpfAnalyzers |
| A10 | Include XAML example in XML docs for controls | Code review |
| A11 | Follow commit convention: `type(scope): description (#PR_NUMBER)` | Code review |
| A12 | Search existing codebase before assuming standard WPF approaches work for Win32 | Code review |
| A13 | Use `_camelCase` for private fields | `.editorconfig` naming rules |
| A14 | Set `OverridesDefaultStyle=True` and `SnapsToDevicePixels=True` in control styles | Code review |
| A15 | Use `DynamicResource` for theme-dependent values, `StaticResource` for constants | Code review |
| A16 | Place each control in its own subfolder under `Controls/` | Project convention |

### 3.2 ASK FIRST

These actions require discussion with the maintainer before proceeding.

| # | Action | Reason |
|---|---|---|
| Q1 | Adding a new NuGet package dependency | Impacts all consumers; increases dependency surface area |
| Q2 | Modifying Win32/Interop layer | May affect multiple OS versions; requires testing on Windows 10/11 |
| Q3 | Changing public API surface | Breaking change potential for library consumers |
| Q4 | Modifying TitleBar or window management code | Complex Win32 interaction with WndProc message handling, snap layouts, DWM |
| Q5 | Adding new multi-target framework conditional compilation | Increases maintenance burden across 6 TFMs |
| Q6 | Modifying theme/accent color management | System-wide effects; affects all controls |
| Q7 | Changing NavigationView behavior | Most complex control in the library (27+ DependencyProperties, 7+ RoutedEvents, 6 partial files) |
| Q8 | Modifying assembly signing or packaging configuration | Impacts NuGet distribution and strong-name consumers |

### 3.3 NEVER DO

These practices are forbidden. Violations will be rejected in code review.

| # | Rule | Enforcement |
|---|---|---|
| N1 | Never add package versions directly to `.csproj` files | Central Package Management enabled |
| N2 | Never use `var` for non-apparent types | `.editorconfig` `csharp_style_var_elsewhere = false:warning` |
| N3 | Never use primary constructors | `.editorconfig` `IDE0290 = none` (suppressed) |
| N4 | Never add comments that merely restate what the code does | Code review |
| N5 | Never use emoticons in code or comments | Code review |
| N6 | Never assume library/NuGet availability without checking `Directory.Packages.props` | Code review |
| N7 | Never skip handle validation in Win32 interop code | Code review |
| N8 | Never use expression-bodied members for constructors, methods, or properties | `.editorconfig` `csharp_style_expression_bodied_* = false:silent` |
| N9 | Never modify namespace to match folder structure for Controls | `.editorconfig` `IDE0130 = none` (suppressed); flat `Wpf.Ui.Controls` namespace |
| N10 | Never use `this.` qualification | `.editorconfig` `SA1101 = none`; `dotnet_style_qualification = false:silent` |

**Concrete examples of NEVER DO violations and their corrections:**

```csharp
// N1 VIOLATION: version in .csproj
<PackageReference Include="NSubstitute" Version="5.3.0" />
// CORRECT: no version attribute
<PackageReference Include="NSubstitute" />

// N2 VIOLATION: var for non-apparent type
var service = container.Resolve<ISnackbarService>();
// CORRECT: explicit type
ISnackbarService service = container.Resolve<ISnackbarService>();

// N3 VIOLATION: primary constructor
public class MyService(ILogger logger) : IMyService { }
// CORRECT: traditional constructor
public class MyService : IMyService
{
    private readonly ILogger _logger;

    public MyService(ILogger logger)
    {
        _logger = logger;
    }
}

// N7 VIOLATION: no handle validation
PInvoke.DwmSetWindowAttribute(new HWND(handle), attr, &value, sizeof(int));
// CORRECT: validate first
if (handle == IntPtr.Zero) return false;
if (!PInvoke.IsWindow(new HWND(handle))) return false;
PInvoke.DwmSetWindowAttribute(new HWND(handle), attr, &value, sizeof(int));

// N8 VIOLATION: expression-bodied property
public string Name => _name;
// CORRECT: block body
public string Name
{
    get => _name;
}

// N9 VIOLATION: folder-matching namespace
namespace Wpf.Ui.Controls.Button;
// CORRECT: flat Controls namespace
// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

// N10 VIOLATION: this. qualification
this._name = name;
// CORRECT: no this.
_name = name;
```

---

## 4. Rules Files Summary

The project enforces conventions through multiple configuration files working in concert.

### Configuration Files Overview

| File | Location | Purpose |
|---|---|---|
| `.editorconfig` | `/` | Encoding, indent rules, naming rules, diagnostic severities |
| `.csharpierrc` | `/` | CSharpier formatter configuration |
| `Directory.Build.props` | `/` | Version, lang version, nullable, unsafe blocks, CPM |
| `Directory.Build.targets` | `/` | SourceLink, assembly signing, analyzers, trimming |
| `Directory.Packages.props` | `/` | Centralized NuGet package versions |
| `.github/copilot-instructions.md` | `/.github/` | GitHub Copilot AI agent instructions |
| `CLAUDE.md` | `/` | Claude Code AI agent guidance |
| `.github/pull_request_template.md` | `/.github/` | PR template with type checkboxes |

### `.editorconfig` -- Key Rules

**Encoding and Indentation:**

| File Type | Indent | Size |
|---|---|---|
| C# (`*.cs`) | Spaces | 4 |
| XML/JSON/YAML (`*.xml`, `*.json`, `*.yml`) | Spaces | 2 |
| MSBuild (`*.csproj`, `*.props`, `*.targets`) | Spaces | 2 |
| Solutions (`*.sln`, `*.slnx`) | Tabs | 4 |
| Markdown (`*.md`) | Spaces | 2 |

**Enforced Diagnostics (error/warning level):**

| Diagnostic | Severity | Description |
|---|---|---|
| `IDE0073` | **error** | File header template (MIT license) |
| `csharp_style_namespace_declarations` | warning | File-scoped namespaces |
| `csharp_style_var_elsewhere` | warning | No `var` for non-apparent types |
| `dotnet_style_predefined_type_for_locals_parameters_members` | warning | Language keywords over BCL types |
| `dotnet_style_readonly_field` | warning | Readonly fields where possible |
| `csharp_prefer_static_local_function` | warning | Static local functions |
| `csharp_style_pattern_local_over_anonymous_function` | warning | Local functions over lambdas |
| `csharp_using_directive_placement` | warning | Usings outside namespace |

**Suppressed Diagnostics:**

| Diagnostic | Reason |
|---|---|
| `SA1101` | No `this.` qualification required |
| `SA1309` | Allow underscore-prefixed field names |
| `SA1600` | Relaxed XML documentation enforcement (not all members require docs) |
| `CS1591` | Missing XML comment (15000+ existing warnings) |
| `IDE0130` | Namespace not matching folder structure (flat `Wpf.Ui.Controls`) |
| `IDE0290` | Primary constructors not used |
| `CA1510` | `ArgumentNullException.ThrowIfNull` helper not available on older TFMs |
| `WPF0070-0073` | ValueConversion attribute warnings relaxed |

**Naming Rules:**

| Symbol | Style | Example |
|---|---|---|
| Constants | PascalCase | `MaxRetryCount` |
| Public/internal/protected fields | PascalCase | `DefaultTimeOut` |
| Public static readonly fields | PascalCase | `IconProperty` |
| Private/protected fields | `_camelCase` | `_presenter` |
| Public methods/properties/events | PascalCase | `Navigate()` |
| Parameters | camelCase | `contentPresenter` |
| Interfaces | `I` + PascalCase | `ISnackbarService` |

### `.csharpierrc` -- Formatter Configuration

```json
{
    "printWidth": 110,
    "useTabs": false,
    "tabWidth": 4,
    "preprocessorSymbolSets": [
        "",
        "DEBUG",
        "DEBUG,CODE_STYLE"
    ]
}
```

Run formatter: `dotnet csharpier .`

### `Directory.Build.props` -- Central Build Configuration

| Property | Value | Effect |
|---|---|---|
| `Version` | `4.2.0` | Assembly and package version |
| `LangVersion` | `14.0` | C# 14 language features |
| `Nullable` | `enable` | Nullable reference types globally enabled |
| `AllowUnsafeBlocks` | `true` | Required for Win32 interop |
| `ManagePackageVersionsCentrally` | `true` | Central Package Management |
| `EnforceCodeStyleInBuild` | `true` | Analyzer warnings fail the build |
| `GenerateDocumentationFile` | `true` | For core projects only |
| `PackageLicenseExpression` | `MIT` | License metadata |

### `Directory.Build.targets` -- Build Targets

Key responsibilities:
- **Analyzer packages**: AsyncFixer, IDisposableAnalyzers, StyleCop.Analyzers added to all projects (except `.dcproj`, `.sfproj`, VS template/extension projects)
- **PolySharp**: Added for netstandard2.0/2.1 and net462/472/481 targets (backports modern C# features)
- **SourceLink**: GitHub SourceLink for NuGet packages
- **Assembly signing**: Conditional on `SourceLinkEnabled=true` AND `GeneratePackageOnBuild=true`, using `src/lepo.snk`
- **Trimming/AOT**: Enabled for .NET 6+ targets (`IsTrimmable`, `EnableTrimAnalyzer`, `EnableAotAnalyzer`, `EnableSingleFileAnalyzer`)
- **Commit hash**: Embedded as `AssemblyMetadataAttribute("CommitHash", ...)` via `SourceRevisionId`

### `Directory.Packages.props` -- Centralized Package Versions

All NuGet package versions for the entire solution are defined here. Key packages:

| Package | Version | Purpose |
|---|---|---|
| `CommunityToolkit.Mvvm` | 8.4.0 | MVVM toolkit for Gallery/samples |
| `Microsoft.Windows.CsWin32` | 0.3.242 | P/Invoke source generator |
| `StyleCop.Analyzers` | 1.2.0-beta.556 | Code style analyzer |
| `WpfAnalyzers` | 4.1.1 | WPF-specific analyzers |
| `AsyncFixer` | 1.6.0 | Async/await analyzer |
| `IDisposableAnalyzers` | 4.0.8 | IDisposable pattern analyzer |
| `xunit.v3` | 3.2.0 | Unit test framework |
| `NSubstitute` | 5.3.0 | Mocking framework |
| `AwesomeAssertions` | 9.3.0 | Fluent assertions |
| `FlaUI.Core` / `FlaUI.UIA3` | 5.0.0 | UI automation testing |
| `PolySharp` | 1.15.0 | C# feature polyfills for older TFMs |
| `Microsoft.Extensions.Hosting` | 10.0.0 | .NET Generic Host |

### `.github/copilot-instructions.md` -- AI Agent Instructions

Provides GitHub Copilot with:
- Project structure and build commands
- Code conventions (never assume library availability, modern C#)
- XML documentation requirements with concrete examples
- Windows platform guidance (search before assuming WPF approaches work)
- Tone/style: no emoticons, minimize output, no preamble/postamble
- MVVM patterns for Gallery pages
- Testing patterns (unit + integration)

### `CLAUDE.md` -- Claude Code Guidance

Provides Claude Code with:
- Build commands for all solution configurations
- Architecture overview (project structure, core library layout)
- Key patterns (Win32 interop, XAML resources, MVVM, CPM)
- Testing frameworks and conventions
- Code conventions summary
- Commit convention format

### `.github/pull_request_template.md` -- PR Template

PR type checkboxes:
- Update
- Bugfix
- Feature
- Code style update (formatting, renaming)
- Refactoring (no functional changes, no API changes)
- Build related changes
- Documentation content changes

Required sections:
- What is the current behavior? (with Issue Number field)
- What is the new behavior?
- Other information (screenshots)

---

## Version Information

| Field | Value |
|---|---|
| **Library Version** | 4.2.0 |
| **Language Version** | C# 14 (preview) |
| **Target Frameworks** | .NET 10/9/8, .NET Framework 4.8.1/4.7.2/4.6.2 |
| **NuGet Package ID** | WPF-UI |
| **XAML Namespace** | `http://schemas.lepo.co/wpfui/2022/xaml` |
| **XAML Prefix** | `ui` |
| **Constitution Version** | 1.0 (2025-02-10) |
