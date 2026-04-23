# ADR-001: Multi-Target Framework Support

## Status
Accepted

## Context
The WPF UI library needs to support a wide range of .NET implementations to maximize compatibility with existing projects while leveraging modern .NET features where available.

### Supported Frameworks
- **.NET 10, 9, 8** - Modern .NET with Windows-specific APIs
- **.NET Framework 4.8.1, 4.7.2, 4.6.2** - Legacy enterprise applications
- **.NET Standard 2.0, 2.1** - Abstractions library only (maximizes compatibility)

## Decision

### Core Library (Wpf.Ui)
Target frameworks: `net10.0-windows;net9.0-windows;net8.0-windows;net481;net472;net462`

**Rationale:**
- Windows-specific project requires `-windows` TFM suffix for .NET 5+
- .NET Framework support ensures compatibility with legacy WPF applications
- Multi-version .NET support provides upgrade path for consumers

### Abstractions Library (Wpf.Ui.Abstractions)
Target frameworks: `net10.0;net9.0;net8.0;net462;netstandard2.1;netstandard2.0`

**Rationale:**
- No WPF dependencies allows broader compatibility
- .NET Standard 2.0 enables use in class libraries shared between .NET Framework and .NET Core/5+
- AOT-compatible (aot_compatible: true) for modern deployment scenarios

### DI Integration (Wpf.Ui.DependencyInjection)
Target frameworks: Same as Abstractions

**Rationale:**
- Depends only on Microsoft.Extensions.DependencyInjection.Abstractions (version 3.1.0 for broad compatibility)
- No WPF dependencies
- Enables DI integration in non-WPF contexts (e.g., background services)

## Central Package Management

All package versions are managed in `Directory.Packages.props`:

```xml
<Project>
  <PropertyGroup>
    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
  </PropertyGroup>

  <ItemGroup>
    <!-- Package versions defined here -->
    <PackageVersion Include="Microsoft.Windows.CsWin32" Version="0.3.242" />
    <PackageVersion Include="System.Memory" Version="4.6.3" />
  </ItemGroup>
</Project>
```

Individual projects reference packages without version attributes:
```xml
<PackageReference Include="Microsoft.Windows.CsWin32" />
```

**Benefits:**
- Single source of truth for package versions
- Prevents version conflicts across projects
- Simplifies dependency updates

## Conditional Compilation

### Framework Detection
```csharp
#if NET5_0_OR_GREATER
    // Modern .NET APIs (Environment.OSVersion)
#else
    // .NET Framework fallback (registry)
#endif

#if NET6_0_OR_GREATER
    // DisposeAsync for CancellationTokenRegistration
#endif

#if NET8_0_OR_GREATER
    // Latest .NET 8+ features
#endif
```

### Framework-Specific Dependencies
```xml
<!-- Only for .NET Framework 4.6.2 -->
<PackageReference Include="System.ValueTuple" Condition="'$(TargetFramework)' == 'net462'" />

<!-- Only for .NET Core/5+ (not .NET Framework 4.6.2) -->
<PackageReference Include="System.Drawing.Common" Condition="'$(TargetFramework)' != 'net462'" />
```

## PolySharp Integration

**Package:** PolySharp (build-time source generator)

Provides polyfills for newer C# features on older target frameworks:
- C# 11+ features on .NET 6/7/8
- C# 12+ features on older .NET versions

**Configuration:**
```xml
<ItemGroup>
  <CompilerVisibleProperty Include="PolySharpExcludeGeneratedTypes" />
</ItemGroup>

<PropertyGroup>
  <!-- Exclude specific polyfills if needed -->
  <PolySharpExcludeGeneratedTypes>
    System.Runtime.CompilerServices.OverloadResolutionPriorityAttribute;
    System.Diagnostics.CodeAnalysis.UnscopedRefAttribute
  </PolySharpExcludeGeneratedTypes>
</PropertyGroup>
```

**Benefits:**
- Use modern C# syntax across all target frameworks
- Init-only properties on .NET Framework
- Required members support
- CallerArgumentExpression on older frameworks

## Language Version

Set to C# 14.0 across all projects:
```xml
<LangVersion>14.0</LangVersion>
```

> **Note:** `Wpf.Ui.csproj` overrides this to `<LangVersion>preview</LangVersion>` to enable C# preview features.

**Combined with PolySharp**, this enables:
- Latest C# language features
- Polyfills generated at compile-time for older frameworks
- No runtime dependencies

## Enforcement

### MUST Follow

1. **All package versions in `Directory.Packages.props` only** — Central Package Management is the single source of truth for NuGet versions
2. **Use `-windows` TFM suffix** for projects with WPF dependencies (e.g., `net10.0-windows`, not `net10.0`)
3. **Use `netstandard2.0`/`netstandard2.1`** for abstraction-only packages that have no WPF or Windows dependency
4. **Guard framework-specific code with `#if NET{X}_0_OR_GREATER` directives** — never use runtime version checks for compile-time API differences
5. **Use PolySharp for C# polyfills** — never hand-write polyfill classes for language features (e.g., `IsExternalInit`, `CallerArgumentExpression`)
6. **Set `LangVersion` in `Directory.Build.props`** — override in individual `.csproj` only when justified (currently only `Wpf.Ui.csproj` overrides to `preview`)

### MUST NOT Do

1. **Never add `Version` attribute to `PackageReference`** in `.csproj` files — all versions must be in `Directory.Packages.props`
2. **Never add a new TFM without updating all projects** that share the same TFM set — all projects in a TFM group must stay in sync
3. **Never use `#if` with specific patch versions** (e.g., `NET8_0_10`) — only use `_OR_GREATER` suffixed symbols
4. **Never remove a TFM from a shipping package** without a major version bump — removing a TFM is a breaking change for consumers on that framework

### Verification

- **CPM enforcement:** `<ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>` in `Directory.Build.props` causes build errors if `Version` is specified in `.csproj`
- **TFM validation:** Build compiles all target frameworks on every `dotnet build` — missing APIs surface as compile errors immediately
- **PolySharp coverage:** PolySharp source generator runs at build time and provides polyfills automatically; manual polyfills would cause duplicate symbol errors

## Consequences

### Positive
- **Broad Compatibility:** Supports applications from .NET Framework 4.6.2 through .NET 10
- **Modern Development:** C# 14 preview features via PolySharp
- **Simplified Maintenance:** Central package management
- **Clear Upgrade Path:** Consumers can upgrade .NET version without changing library
- **AOT Ready:** Abstractions library supports AOT scenarios

### Negative
- **Increased Build Complexity:** Each commit builds 6+ framework variants
- **Larger Package Size:** Multi-target packages include assemblies for all frameworks
- **Conditional Compilation:** Requires `#if` directives for framework-specific code
- **Testing Burden:** Features should be tested on multiple framework versions

## Implementation Details

### Directory.Build.props
Central build properties defined once:
```xml
<PropertyGroup>
  <Version>4.2.0</Version>
  <LangVersion>14.0</LangVersion>
  <Nullable>enable</Nullable>
  <AllowUnsafeBlocks>true</AllowUnsafeBlocks>
  <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
  <GenerateDocumentationFile>true</GenerateDocumentationFile>
</PropertyGroup>
```

### Conditional Property Groups
```xml
<PropertyGroup Condition="$([MSBuild]::IsTargetFrameworkCompatible('$(TargetFramework)', 'net8.0'))">
  <DefineConstants>$(DefineConstants);NET8_0_OR_GREATER</DefineConstants>
</PropertyGroup>

<PropertyGroup Condition="!$([MSBuild]::IsTargetFrameworkCompatible('$(TargetFramework)', 'net8.0'))">
  <DefineConstants>$(DefineConstants);BELOW_NET8</DefineConstants>
</PropertyGroup>
```

## References
- [.NET Multi-Targeting Documentation](https://docs.microsoft.com/dotnet/standard/frameworks)
- [Central Package Management](https://docs.microsoft.com/nuget/consume-packages/central-package-management)
- [PolySharp GitHub](https://github.com/Sergio0694/PolySharp)
- [TFM Compatibility](https://docs.microsoft.com/dotnet/standard/frameworks#net-5-os-specific-tfms)
