# Architecture Uncertainties

This document collects areas where the architecture documentation is incomplete or uncertain based on the analysis. These items require further investigation or are subject to change.

## Core Library (Group 1)

### Test Coverage
**Area:** `tests/` directory (outside analysis scope)

Cannot determine exact test coverage. Given 77+ controls with complex state management (NavigationView journal, ContentDialog async lifecycle), the testability of static managers (ApplicationThemeManager, SystemThemeWatcher) is limited. Service interfaces (INavigationService, IContentDialogService, etc.) are testable through mock implementations.

Current unit test coverage is minimal (6 tests covering Animations and Extensions only).

### WindowBackdrop Implementation
**Area:** `src/Wpf.Ui/Controls/Window/WindowBackdrop.cs`

WindowBackdrop class is referenced in FluentWindow and WindowBackgroundManager (ApplyBackdrop, RemoveBackdrop, RemoveBackground, RemoveTitlebarBackground) but was not directly examined. Likely located in Controls/Window/ directory and provides Mica/Acrylic/Tabbed backdrop effects via DWM APIs.

### NavigationView Page Caching
**Area:** `src/Wpf.Ui/Controls/NavigationView/NavigationCache.cs` and `NavigationCacheMode.cs`

These files exist but were not deeply examined. The caching strategy for navigation pages includes Required, Enabled, and Disabled modes but the exact implementation details need further investigation.

### PolySharp Configuration
**Area:** Central Package Management in `Directory.Packages.props`

PolySharp is referenced in core library csproj via `PolySharpExcludeGeneratedTypes` but the actual package reference appears to come from Directory.Packages.props (central package management) which was not fully examined. The exact polyfill configuration and version require verification.

## Satellite Libraries (Group 2)

### Toast Notifications Implementation Status
**Module:** `Wpf.Ui.ToastNotifications`

It is unclear whether this library has any concrete implementation plans or if it is purely a placeholder for future work. The Toast.Show() method throws NotImplementedException. The library has no dependency on Wpf.Ui core despite being a WPF-targeting project.

### FlaUI Library Scope
**Module:** `Wpf.Ui.FlaUI`

The FlaUI library has no project reference to Wpf.Ui, yet it exists under the Wpf.Ui namespace. It only depends on FlaUI.Core. Currently contains only an AutoSuggestBox automation element wrapper. It is unclear whether additional automation elements for other WPF UI controls will be added.

### FontMapper Integration Workflow
**Module:** `Wpf.Ui.FontMapper`

The FontMapper generates enum files for the core library but has no project reference to it. The generated output path uses a relative 'generated/' directory. The exact integration workflow (how generated files reach the core library) is not evident from the project analysis alone.

**Additional Issue:** FetchVersion() returns a hardcoded version '1.1.316' instead of actually fetching from GitHub API (the API call code is commented out).

### SyntaxHighlight Font Dependency
**Module:** `Wpf.Ui.SyntaxHighlight`

The SyntaxHighlight.xaml references a FontFamily with key 'FiraCode' using pack URI pointing to Wpf.Ui (not Wpf.Ui.SyntaxHighlight), suggesting the font may need to also exist in the core library.

**Additional Issue:** The CodeBlock.cs is also listed as an EmbeddedResource in the csproj, which is unusual for a source file.

**WIP Status:** Highlighter class is marked as work in progress. The autodetect language feature defaults to XAML, and C# and XAML use identical regex patterns.

## Gallery and Tests (Group 3)

### Gallery MSIX Packaging
**Module:** `Wpf.Ui.Gallery.Package`

A .wapproj (Windows Application Packaging) project is referenced in Wpf.Ui.Gallery.slnf but not directly analyzed. This likely creates an MSIX package for the Gallery app. The packaging configuration and deployment process need clarification.

### GalleryAssembly Reference
**Area:** `src/Wpf.Ui.Gallery/ControlsLookup/`

Referenced as `GalleryAssembly.Asssembly` (note triple 's') in DependencyModel and ControlsLookup. The actual GalleryAssembly class was not directly read. It likely provides the Gallery assembly reference for reflection-based page discovery.

### ReflectionEventing Usage
**Package:** ReflectionEventing and ReflectionEventing.DependencyInjection

These packages are referenced in the Gallery csproj but their usage was not traced to specific event handling code within the analyzed files. The purpose and integration point of these packages require investigation.

### Visual Studio Extension
**Module:** `src/Wpf.Ui.Extension`

The VS2022 extension project (.vsix) is built for x64 and arm64 platforms but was not analyzed. The extension capabilities and integration points with Visual Studio are unknown.

## Build and Infrastructure

### .NET SDK Version Mismatch
**Area:** `build.ps1`

The build script references .NET SDK 8 for installation (`winget install Microsoft.DotNet.SDK.8`) but the project now targets .NET 10. The build script may be outdated.

### Dependabot Target Branch
**Area:** `.github/dependabot.yml`

The Dependabot configuration targets the 'development' branch, not 'main'. The branch strategy and merge workflow from development to main are not documented.

### CI Test Execution
**Area:** `.github/workflows/wpf-ui-pr-validator.yaml`

The PR validator workflow only builds the Gallery app and does not run any tests (unit or integration). The rationale for not including test execution in CI is unclear.

### Strong-Name Signing
**Area:** GitHub Actions secrets and certificate management

The CD pipeline fetches a strong-name certificate from GitHub secrets (`${{ secrets.WPF_UI_CERTIFICATE_BASE64 }}`). The certificate generation, storage, and rotation procedures are not documented.

## Documentation and Versioning

### Version Synchronization
**Area:** `Directory.Build.props` vs actual package versions

The Version property is set to 4.2.0, but the synchronization mechanism between this central version, individual package versions, and changelog updates is not documented.

### API Compatibility
**Area:** Deprecated APIs and migration paths

Several APIs are marked `[Obsolete]` (ContentDialog legacy constructors, WindowBackgroundManager.UpdateBackground with forceBackground parameter). The deprecation timeline and migration strategy for consumers are not documented.

## Automation Peer Coverage
**Area:** `src/Wpf.Ui/AutomationPeers/`

Only 4 automation peers exist for 77+ controls:
- CardControlAutomationPeer
- ContentDialogAutomationPeer
- CardActionAutomationPeer
- NavigationViewItemAutomationPeer

Many interactive controls (Button, NavigationView base, TextBox, etc.) may lack custom automation peer implementations. The strategy for expanding accessibility support is unclear. This potentially affects screen reader compatibility and UI automation testing.

## Theme Resource Keys
**Area:** Dynamic resource naming and contracts

20+ accent-related dynamic resources are updated programmatically by ApplicationAccentColorManager:
- SystemAccentColor
- AccentFillColorDefault
- TextOnAccentFillColorPrimary
- (and others)

The complete list of dynamic resources and their contracts (expected types, update triggers) is not centrally documented. Third-party theme authors may struggle to determine which resources must be provided.

## Multi-Target Conditional Compilation
**Area:** Cross-framework API compatibility

Various conditional compilation symbols are used:
- `NET5_0_OR_GREATER`
- `NET6_0_OR_GREATER`
- `NET8_0_OR_GREATER`
- `NET48_OR_GREATER_or_NETCOREAPP3_0_OR_GREATER`

The decision matrix for when to use each symbol and the feature availability across frameworks is not documented in a single location.

## System Requirements
**Area:** Minimum Windows version and feature matrix

The library supports Windows 7 through Windows 11, but feature availability varies:
- Mica/Acrylic backdrops require Windows 11
- Some DWM features require Windows 10
- Certain APIs have Fall Creators Update requirements

A comprehensive feature compatibility matrix by Windows version is not documented.
