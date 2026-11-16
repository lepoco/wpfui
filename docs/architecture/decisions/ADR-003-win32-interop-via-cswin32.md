# ADR-003: Win32 Interop via CsWin32

## Status
Accepted

## Context
WPF UI requires extensive Win32 API access for features unavailable in standard WPF:
- Desktop Window Manager (DWM) effects (Mica, Acrylic backdrops)
- Window corner rounding (Windows 11)
- Dark mode title bars
- System tray icon management
- System theme detection
- Taskbar progress indicators

Traditional P/Invoke requires:
- Manual function signature declarations
- COM interface definitions
- Struct layout definitions
- Constant value definitions
- Maintaining cross-architecture compatibility (x86/x64/ARM64)

## Decision

### Use CsWin32 Source Generator

**Package:** Microsoft.Windows.CsWin32 (build-time only, PrivateAssets="all")

**Declaration File:** `src/Wpf.Ui/NativeMethods.txt`

CsWin32 generates P/Invoke bindings at compile-time from Win32 metadata.

### NativeMethods.txt Format

```
# DWM Functions
DwmIsCompositionEnabled
DwmSetWindowAttribute
DwmExtendFrameIntoClientArea
S_OK
SetWindowThemeAttribute
DWM_SYSTEMBACKDROP_TYPE
DWM_WINDOW_CORNER_PREFERENCE
DWMWA_COLOR_NONE
WTA_OPTIONS

# Window Management
GetDpiForWindow
GetForegroundWindow
IsWindowVisible
SetWindowRgn
GetWindowRect
GetSystemMetrics
WINDOW_STYLE

# COM Interfaces
ITaskbarList4
TaskbarList

# Wildcard Patterns
WM_*
HT*
```

**CsWin32 automatically generates:**
- Function P/Invoke declarations
- Struct definitions with correct layout
- Enum types
- COM interface wrappers
- Foundation types (HWND, HRESULT, BOOL, etc.)

### Generated Code Location
**Namespace:** `Windows.Win32` and `Windows.Win32.Foundation`
**Physical Location:** `obj/` directory (not committed to source control)

**Usage:**
```csharp
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Dwm;

// Generated type-safe P/Invoke
HRESULT result = PInvoke.DwmSetWindowAttribute(
    new HWND(windowHandle),
    DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE,
    &darkMode,
    sizeof(BOOL)
);
```

## Three-Layer Architecture

### Layer 1: CsWin32 Generated Code
**Purpose:** Auto-generated P/Invoke declarations

**Characteristics:**
- Compile-time generated
- Type-safe API surface
- Cross-architecture compatible
- Not committed to source control

### Layer 2: Managed Wrappers
**Purpose:** Safe, validated native API access

**Location:** `src/Wpf.Ui/Interop/`

#### UnsafeNativeMethods.cs
```csharp
internal static class UnsafeNativeMethods
{
    public static unsafe bool ApplyWindowCornerPreference(
        IntPtr handle,
        WindowCornerPreference cornerPreference)
    {
        // Validation layer
        if (handle == IntPtr.Zero)
            return false;

        if (!PInvoke.IsWindow(new HWND(handle)))
            return false;

        // Type conversion
        DWM_WINDOW_CORNER_PREFERENCE pvAttribute =
            UnsafeReflection.Cast(cornerPreference);

        // Native call with exception handling
        try
        {
            HRESULT hr = PInvoke.DwmSetWindowAttribute(
                new HWND(handle),
                DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE,
                &pvAttribute,
                (uint)sizeof(DWM_WINDOW_CORNER_PREFERENCE)
            );

            return hr == HRESULT.S_OK;
        }
        catch
        {
            // Graceful degradation for unsupported OS versions
            return false;
        }
    }
}
```

**Responsibilities:**
- Handle validation (IntPtr.Zero, IsWindow checks)
- Exception suppression for cross-version compatibility
- HRESULT → bool conversion
- Type-safe enum conversions

#### Custom PInvoke.cs
**Purpose:** Supplement CsWin32 for missing/incorrect signatures

```csharp
namespace Windows.Win32;

internal static partial class PInvoke
{
    // CsWin32 doesn't generate correct SetWindowLongPtr for 32/64-bit
    [DllImport("USER32.dll",
        ExactSpelling = true,
        EntryPoint = "SetWindowLongPtrW",
        SetLastError = true)]
    internal static extern nint SetWindowLongPtr(
        HWND hWnd,
        WINDOW_LONG_PTR_INDEX nIndex,
        nint dwNewLong
    );
}
```

### Layer 3: High-Level Utilities
**Purpose:** Business logic and feature implementation

**Locations:**
- `src/Wpf.Ui/Win32/Utilities.cs` - OS version detection
- `src/Wpf.Ui/Appearance/` - Theme managers
- `src/Wpf.Ui/Controls/FluentWindow/` - Window chrome
- `src/Wpf.Ui/Tray/` - System tray management

**Characteristics:**
- Consumes Layer 2 safe wrappers
- OS version feature gating
- Business logic and state management

## Handle Validation Pattern

**Critical Requirement:** All native calls MUST validate handles.

```csharp
public static bool NativeOperation(IntPtr handle)
{
    // Step 1: Null check
    if (handle == IntPtr.Zero)
    {
        return false;
    }

    // Step 2: Verify window exists
    if (!PInvoke.IsWindow(new HWND(handle)))
    {
        return false;
    }

    // Step 3: Perform operation
    HRESULT hr = PInvoke.SomeWin32Function(new HWND(handle), ...);
    return hr == HRESULT.S_OK;
}
```

**Rationale:**
- Handles can become invalid between retrieval and use
- Window may be destroyed on background thread
- Invalid handles cause native crashes
- IsWindow is inexpensive (single User32 call)

## Exception Handling Strategy

**Philosophy:** Native APIs fail silently across Windows versions. Prefer graceful degradation over exceptions.

```csharp
try
{
    HRESULT hr = PInvoke.DwmSetWindowAttribute(...);
    return hr == HRESULT.S_OK;
}
catch (COMException)
{
    // API not available on this Windows version
    return false;
}
catch
{
    // Unexpected failure, degrade gracefully
    return false;
}
```

**Suppressed Exceptions:**
- `COMException` - COM API failures
- `Win32Exception` - Native API errors
- `EntryPointNotFoundException` - API not available on OS version
- `DllNotFoundException` - DLL not present

## Conditional Compilation

### Framework-Specific Code
```csharp
#if NET5_0_OR_GREATER
    // Modern API available
    var version = Environment.OSVersion;
#else
    // Fallback for .NET Framework
    var version = GetVersionFromRegistry();
#endif
```

### OS Version Feature Gating
```csharp
// Windows 11+ only features
if (Win32.Utilities.IsOSWindows11OrNewer)
{
    UnsafeNativeMethods.ApplyWindowCornerPreference(
        handle,
        WindowCornerPreference.Round
    );
}

// DWM composition required
if (Win32.Utilities.IsCompositionEnabled)
{
    UnsafeNativeMethods.ApplyWindowBackdrop(
        handle,
        WindowBackdropType.Acrylic
    );
}
```

## Enforcement

### MUST Follow

1. **Add new Win32 APIs to NativeMethods.txt** (never manual P/Invoke unless CsWin32 fails)
2. **Validate handles** before all native calls (IntPtr.Zero + IsWindow)
3. **Return bool** from wrapper methods indicating success
4. **Suppress exceptions** in interop layer for compatibility
5. **Use unsafe keyword** explicitly for pointer operations
6. **Feature-gate by OS version** for version-specific APIs
7. **Use HRESULT == S_OK** pattern for success checking
8. **Keep generated code private** (internal/private visibility)

### MUST NOT Do

1. **Never call PInvoke directly** from high-level code (use UnsafeNativeMethods wrappers)
2. **Never skip handle validation** (even if "guaranteed" valid)
3. **Never throw exceptions** from interop wrappers (return false instead)
4. **Never assume API availability** across Windows versions
5. **Never use var** for native types (HRESULT, HWND, etc. - explicit types required)
6. **Never commit obj/ directory** (contains generated code)

### Verification

- WpfAnalyzers enforces correct patterns
- Code review checks handle validation
- Multi-version testing (Windows 7, 8.1, 10, 11)

## Consequences

### Positive
- **Type Safety:** CsWin32 generates correct signatures from metadata
- **Maintenance:** Win32 metadata updates automatically benefit project
- **Cross-Platform:** ARM64, x64, x86 handled automatically
- **Correctness:** Struct layouts, calling conventions verified by Microsoft
- **Discoverability:** IntelliSense for all Windows APIs
- **Build-Time Only:** Zero runtime dependencies

### Negative
- **Build-Time Dependency:** Requires CsWin32 NuGet package
- **Opaque Generation:** Generated code in obj/, harder to debug
- **NativeMethods.txt Maintenance:** Must manually add new APIs (currently 34 lines, including wildcard patterns like `WM_*` and `HT*`)
- **Supplements Needed:** Some APIs require manual P/Invoke (SetWindowLongPtr)
- **Learning Curve:** Developers must understand NativeMethods.txt format

## Performance Considerations

### CsWin32 Performance
- **Zero overhead:** Generated code identical to hand-written P/Invoke
- **Inlined by JIT:** Same JIT optimization as manual declarations
- **No reflection:** Compile-time code generation

### Handle Validation Cost
- **IsWindow:** Single User32 API call (~1-2 μs)
- **Negligible:** Compared to DWM/window operations (hundreds of μs)
- **Essential:** Prevents native crashes worth the cost

## Alternatives Considered

### Manual P/Invoke
**Rejected:**
- High maintenance burden (200+ Win32 functions across project)
- Error-prone struct layout definitions
- Cross-architecture compatibility issues
- No automatic updates from Windows SDK

### ComWrappers
**Rejected:**
- Only solves COM interop, not general P/Invoke
- More complex than CsWin32
- Limited to .NET 5+

### PInvoke.net Snippets
**Rejected:**
- Community-maintained, not authoritative
- Copy-paste errors common
- No compile-time verification
- Inconsistent signature styles

## Migration Guide

### Adding New Win32 API

1. **Add to NativeMethods.txt:**
   ```
   DwmGetColorizationColor
   ```

2. **Rebuild project** (CsWin32 generates code)

3. **Create managed wrapper in UnsafeNativeMethods.cs:**
   ```csharp
   public static bool GetColorizationColor(out Color color)
   {
       try
       {
           HRESULT hr = PInvoke.DwmGetColorizationColor(out uint colorValue, out BOOL opaque);
           color = Color.FromArgb(...);
           return hr == HRESULT.S_OK;
       }
       catch
       {
           color = default;
           return false;
       }
   }
   ```

4. **Consume from high-level code:**
   ```csharp
   if (UnsafeNativeMethods.GetColorizationColor(out Color color))
   {
       // Use color
   }
   ```

## References
- [CsWin32 GitHub](https://github.com/microsoft/CsWin32)
- [Windows API Documentation](https://docs.microsoft.com/windows/win32/api/)
- [P/Invoke Best Practices](https://docs.microsoft.com/dotnet/standard/native-interop/best-practices)
