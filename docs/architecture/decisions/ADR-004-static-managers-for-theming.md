# ADR-004: Static Managers for Theming

## Status
Accepted

## Context
The theming system requires global coordination across:
- Application resource dictionary management
- System theme synchronization
- Accent color application
- Window appearance updates

Multiple approaches exist:
1. **Static classes** (global singleton)
2. **Instance-based services** (registered in DI container)
3. **Ambient context pattern** (ThreadStatic/AsyncLocal)

## Decision

Use **static class singleton pattern** for core theme managers:
- `ApplicationThemeManager`
- `ApplicationAccentColorManager`
- `SystemThemeWatcher`
- `WindowBackgroundManager`
- `ResourceDictionaryManager` (internal)

### Implementation Pattern

```csharp
public static class ApplicationThemeManager
{
    // Global state
    private static ApplicationTheme _currentTheme = ApplicationTheme.Unknown;

    // Global event
    public static event ThemeChangedEvent? Changed;

    // Static methods
    public static void Apply(ApplicationTheme theme)
    {
        if (_currentTheme == theme)
            return;

        ResourceDictionaryManager manager = new(LibraryNamespace);
        manager.UpdateDictionary("theme", GetThemeUri(theme));

        _currentTheme = theme;
        Changed?.Invoke(theme, GetSystemAccent());
    }

    public static ApplicationTheme GetAppTheme()
    {
        return _currentTheme;
    }
}
```

### No Constructor, No Instances
```csharp
public static class ApplicationThemeManager
{
    // All members are static
    // Cannot be instantiated
    // Cannot be inherited
    // Cannot be mocked/substituted
}
```

## Rationale

### Simple API Surface
```csharp
// Immediate clarity of global scope
ApplicationThemeManager.Apply(ApplicationTheme.Dark);
var current = ApplicationThemeManager.GetAppTheme();
```

Compared to instance-based:
```csharp
// Requires context about where themeManager comes from
_themeManager.Apply(ApplicationTheme.Dark);
var current = _themeManager.GetAppTheme();
```

### Single Source of Truth
Application theme is fundamentally global state:
- Only one theme can be active
- All windows share the same theme
- Resource dictionaries are process-wide

Static class enforces singleton semantics at compile-time.

### No DI Configuration Required
```csharp
// Works immediately without setup
public MainWindow()
{
    InitializeComponent();
    ApplicationThemeManager.Apply(ApplicationTheme.Dark);
}
```

Instance-based would require:
```csharp
// App.xaml.cs
services.AddSingleton<IThemeManager, ThemeManager>();

// MainWindow.xaml.cs
public MainWindow(IThemeManager themeManager)
{
    _themeManager = themeManager;
    InitializeComponent();
    _themeManager.Apply(ApplicationTheme.Dark);
}
```

### WPF Application Model Alignment
WPF itself uses static patterns extensively:
- `Application.Current` (static property)
- `Application.Current.Resources` (global resource dictionary)
- `SystemColors` (static class)
- `SystemParameters` (static class)

## Trade-offs

### Advantages

**1. Simplicity**
- No DI configuration required
- No interface abstraction needed
- Clear that state is global
- Immediate usability

**2. Performance**
- Zero overhead (no interface dispatch)
- No allocation for manager instances
- Direct static method calls

**3. Discoverability**
- Easy to find with IntelliSense
- Self-documenting via naming (`ApplicationThemeManager` clearly global)
- No need to understand DI container

**4. Compatibility**
- Works in non-DI scenarios (simple WPF apps)
- Compatible with .NET Framework patterns
- No breaking changes if DI added later

### Disadvantages

**1. Limited Testability**
- Cannot mock static classes
- Difficult to isolate in unit tests
- Tests may affect each other through shared state

**Mitigation:**
- Extract `IThemeService` for consumers who need testability
- Test through integration tests instead of unit tests
- Use `[Collection]` attribute in XUnit to isolate test state

**2. Hidden Dependencies**
- Static call hides dependency
- Harder to track theme manager usage
- Violates dependency injection principle

**Mitigation:**
- Theme management is intentionally global
- Not a "hidden" dependency if explicitly documented as global

**3. Global State**
- Mutable global state
- Concurrent access concerns
- No lifetime management

**Mitigation:**
- Theme changes are inherently single-threaded (UI thread)
- `Application.Current.Resources` is already global mutable state
- No need for lifetime management (lives entire process lifetime)

**4. No Polymorphism**
- Cannot substitute alternative implementations
- Cannot extend behavior through inheritance

**Mitigation:**
- Theme system is not extensible by design
- Alternative implementations not a use case

## Service Interface for DI

For consumers requiring testability, `IThemeService` wraps static managers:

```csharp
public interface IThemeService
{
    ApplicationTheme GetTheme();
    SystemTheme GetNativeSystemTheme();
    ApplicationTheme GetSystemTheme();
    bool SetTheme(ApplicationTheme applicationTheme);
    bool SetSystemAccent();
    bool SetAccent(Color accentColor);
    bool SetAccent(SolidColorBrush accentSolidBrush);
}

public partial class ThemeService : IThemeService
{
    public ApplicationTheme GetTheme()
        => ApplicationThemeManager.GetAppTheme();

    public SystemTheme GetNativeSystemTheme()
        => ApplicationThemeManager.GetSystemTheme();

    public ApplicationTheme GetSystemTheme()
        => ApplicationThemeManager.GetSystemTheme() switch { ... };

    public bool SetTheme(ApplicationTheme applicationTheme)
    {
        ApplicationThemeManager.Apply(applicationTheme);
        return true;
    }

    public bool SetSystemAccent()
    {
        ApplicationAccentColorManager.ApplySystemAccent();
        return true;
    }

    public bool SetAccent(Color accentColor)
    {
        ApplicationAccentColorManager.Apply(accentColor);
        return true;
    }

    public bool SetAccent(SolidColorBrush accentSolidBrush)
    {
        ApplicationAccentColorManager.Apply(accentSolidBrush.Color);
        return true;
    }
}
```

**Registration:**
```csharp
services.AddSingleton<IThemeService, ThemeService>();
```

**Usage in testable code:**
```csharp
public class SettingsViewModel
{
    private readonly IThemeService _themeService;

    public SettingsViewModel(IThemeService themeService)
    {
        _themeService = themeService;
    }

    public void ApplyDarkMode()
    {
        _themeService.SetTheme(ApplicationTheme.Dark);
    }
}
```

**Testing with mock:**
```csharp
[Fact]
public void ApplyDarkMode_CallsThemeService()
{
    // Arrange
    var mockThemeService = Substitute.For<IThemeService>();
    var viewModel = new SettingsViewModel(mockThemeService);

    // Act
    viewModel.ApplyDarkMode();

    // Assert
    mockThemeService.Received(1).SetTheme(ApplicationTheme.Dark);
}
```

## UiApplication Pattern

`UiApplication` uses `[ThreadStatic]` for thread-local singleton:

```csharp
public class UiApplication
{
    [ThreadStatic]
    private static UiApplication? _uiApplication;

    public static UiApplication? Current => _uiApplication;

    public UiApplication(Application application)
    {
        // Stores the application reference and sets _uiApplication
    }

    // Instance methods operate on the wrapped Application
    public ResourceDictionary Resources => Application.Current.Resources;
}
```

**Rationale:**
- Non-static class instantiated with an `Application` parameter
- Uses `[ThreadStatic]` backing field `_uiApplication` for thread-local singleton
- Each UI thread gets its own `UiApplication` instance
- Safely wraps `Application.Current` (which is also thread-local)
- Allows future expansion with per-thread state

## Enforcement

### MUST Follow

1. **Use static managers directly** for simple scenarios:
   ```csharp
   ApplicationThemeManager.Apply(ApplicationTheme.Dark);
   ```

2. **Use IThemeService** in testable/DI-dependent code:
   ```csharp
   public ViewModel(IThemeService themeService) { }
   ```

3. **Document global nature** in XML docs:
   ```csharp
   /// <summary>
   /// Global theme manager. Applies themes application-wide.
   /// </summary>
   ```

4. **Never create wrapper instances** of static managers:
   ```csharp
   // BAD: Don't do this
   public class ThemeManagerWrapper
   {
       private ApplicationTheme _cachedTheme;

       public void Apply(ApplicationTheme theme)
       {
           _cachedTheme = theme;
           ApplicationThemeManager.Apply(theme);
       }
   }
   ```

### MUST NOT Do

1. **Never attempt to mock static classes** in unit tests
   - Use `IThemeService` instead if testing is needed

2. **Never cache theme state** in instance fields
   - Always query `ApplicationThemeManager.GetAppTheme()` for current state

3. **Never create parallel theme systems**
   - Static managers are the single source of truth

### Verification

- Code review enforces proper usage
- Documentation clearly marks classes as static global managers
- `IThemeService` provides tested alternative where needed

## Testing Strategy

### Integration Tests
Theme system is tested through integration tests that exercise full stack:

```csharp
[Fact]
public async Task ThemeChange_UpdatesWindowAppearance()
{
    // Arrange
    var window = new FluentWindow();
    window.Show();

    // Act
    ApplicationThemeManager.Apply(ApplicationTheme.Dark);
    await Task.Delay(500); // Allow visual update

    // Assert
    var theme = ApplicationThemeManager.GetAppTheme();
    Assert.Equal(ApplicationTheme.Dark, theme);

    // Cleanup
    window.Close();
}
```

### Unit Tests for Consumer Code
Consumer code uses `IThemeService` for testability:

```csharp
[Fact]
public void ApplyDarkTheme_UpdatesCurrentTheme()
{
    var themeService = Substitute.For<IThemeService>();
    var viewModel = new SettingsViewModel(themeService);

    viewModel.ApplyDarkTheme();

    themeService.Received().SetTheme(ApplicationTheme.Dark);
}
```

## Documentation Requirements

All static manager classes include XML doc warning:

```csharp
/// <summary>
/// Global static manager for application theming.
/// </summary>
/// <remarks>
/// <para>
/// This is a static class managing process-wide theme state.
/// Theme changes affect all windows in the application.
/// </para>
/// <para>
/// For testable code, use <see cref="IThemeService"/> instead.
/// </para>
/// </remarks>
public static class ApplicationThemeManager
{
    // ...
}
```

## Future Considerations

### Potential Migration to Instances
If future requirements demand it, can migrate while maintaining compatibility:

```csharp
// New instance-based implementation
public sealed class ThemeManager : IThemeManager
{
    // Instance implementation
}

// Static facade maintains compatibility
public static class ApplicationThemeManager
{
    private static readonly IThemeManager _instance = new ThemeManager();

    public static void Apply(ApplicationTheme theme)
        => _instance.Apply(theme);
}
```

This preserves existing code while enabling DI-based usage.

## References
- WPF Static Classes: `Application.Current`, `SystemColors`, `SystemParameters`
- Gang of Four Singleton Pattern
- [Dependency Injection Anti-Pattern: Service Locator](https://blog.ploeh.dk/2010/02/03/ServiceLocatorisanAnti-Pattern/)
