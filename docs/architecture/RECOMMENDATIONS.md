# WPF UI - Architecture Recommendations

> Generated: 2026-02-10. These recommendations are based on architectural analysis and should be re-evaluated as the codebase evolves.

## Recommendations

### Testing

| Priority | Recommendation | Rationale | Affected |
|----------|---------------|-----------|----------|
| **Critical** | Add unit tests for core controls, targeting at least NavigationView, TitleBar, FluentWindow, ContentDialog, NumberBox, and ToggleSwitch. | Only 6 unit tests exist for a 77+ control library. Regressions ship undetected. | `src/Wpf.Ui/Controls/`, `tests/Wpf.Ui.UnitTests/` |
| **Critical** | Enable test execution in the PR validator CI workflow. | The current workflow builds the Gallery app but never runs tests. Merging broken code is possible. | `.github/workflows/` |
| **High** | Add integration tests for NavigationView page caching and lifecycle transitions. | Caching behavior is undocumented and untested. Users report inconsistent state across navigation. | `src/Wpf.Ui/Controls/NavigationView/` |
| **Medium** | Add contract tests for `INavigationService`, `IContentDialogService`, and `ISnackbarService`. | Service interfaces are the primary public API surface for DI consumers but lack test coverage. | `src/Wpf.Ui.Abstractions/`, `src/Wpf.Ui/Services/` |

### Architecture

| Priority | Recommendation | Rationale | Affected |
|----------|---------------|-----------|----------|
| **High** | Wrap static theme managers (`ApplicationThemeManager`, `ApplicationAccentColorManager`) with injectable service interfaces. | Static managers cannot be mocked or unit tested. `IThemeService` exists but the underlying managers remain untestable. | `src/Wpf.Ui/Appearance/` |
| **High** | Document NavigationView page caching strategy: when pages are created, cached, and disposed. | Caching behavior is implicit in the implementation with no documentation or configuration surface. | `src/Wpf.Ui/Controls/NavigationView/` |
| **Medium** | Add structured logging (e.g., `ILogger`) to Win32 interop error-handling paths. | Catch blocks intentionally swallow Win32 exceptions but produce no diagnostics. Silent failures make debugging difficult for consumers. | `src/Wpf.Ui/Interop/`, `src/Wpf.Ui/Win32/` |

### Technical Debt

| Priority | Recommendation | Rationale | Affected |
|----------|---------------|-----------|----------|
| **High** | Either implement `Wpf.Ui.ToastNotifications` or remove it from the solution. | The project is a stub with no implementation. It ships as a package that does nothing. | `src/Wpf.Ui.ToastNotifications/` |
| **Medium** | Audit and reduce public API surface of Win32/Interop namespaces. | Many P/Invoke declarations are public but intended for internal use only. Exposing raw Win32 types couples consumers to implementation details. | `src/Wpf.Ui/Win32/`, `src/Wpf.Ui/Interop/` |
| **Low** | Consolidate resource dictionary loading paths. | `ThemesDictionary` and `ControlsDictionary` have overlapping responsibilities that could confuse consumers. | `src/Wpf.Ui/Resources/` |

### CI/CD

| Priority | Recommendation | Rationale | Affected |
|----------|---------------|-----------|----------|
| **Critical** | Add a CI job that runs `dotnet test tests/Wpf.Ui.UnitTests/` on every PR. | No tests run in CI. The existing test suite provides zero protection against regressions. | `.github/workflows/` |
| **High** | Add code coverage reporting (Coverlet is already configured) and set a minimum threshold. | Coverage tooling is present but never executed. Without a baseline, coverage can only decrease. | `tests/Wpf.Ui.UnitTests/`, `.github/workflows/` |
| **Medium** | Add a CI step to run `dotnet csharpier --check .` to enforce formatting. | CSharpier is configured but not enforced in CI. Formatting inconsistencies can slip through review. | `.github/workflows/` |

### Documentation

| Priority | Recommendation | Rationale | Affected |
|----------|---------------|-----------|----------|
| **Medium** | Add XML doc `<example>` tags with XAML usage to all public control APIs. | Project conventions require examples, but coverage is inconsistent across the 77+ controls. | `src/Wpf.Ui/Controls/` |
| **Low** | Create a process for keeping architecture docs in sync with code changes. | Architecture documentation (generated 2026-02-10) will drift from the implementation without a manual update process or automation. | `docs/architecture/` |

---

## Task Backlog

Structured work items derived from the recommendations above. Each item includes acceptance criteria (AC).

### TB-001: Add unit tests for core controls [Critical]

**Source:** Testing #1
**AC:**
- [ ] NavigationView has ≥5 unit tests covering navigation, caching, back stack
- [ ] TitleBar has ≥3 tests covering minimize/maximize/close commands
- [ ] FluentWindow has ≥2 tests covering backdrop type selection
- [ ] ContentDialog has ≥3 tests covering show/hide/result lifecycle
- [ ] NumberBox has ≥3 tests covering min/max/step validation
- [ ] ToggleSwitch has ≥2 tests covering checked/unchecked state
- [ ] All tests follow `MethodName_ExpectedResult_WhenCondition` naming convention
- [ ] All tests use XUnit v3, NSubstitute, AwesomeAssertions

### TB-002: Enable test execution in CI [Critical]

**Source:** CI/CD #1, Testing #2
**AC:**
- [ ] `.github/workflows/` contains a job that runs `dotnet test tests/Wpf.Ui.UnitTests/` on every PR
- [ ] CI job fails the PR if any test fails
- [ ] Test results are visible in the GitHub Actions summary
- [ ] Job runs on `ubuntu-latest` or `windows-latest` as appropriate for WPF tests

### TB-003: Add code coverage reporting with threshold [High]

**Source:** CI/CD #2
**AC:**
- [ ] Coverlet generates coverage report during CI test run
- [ ] Coverage report is uploaded as a CI artifact or displayed in PR summary
- [ ] Minimum coverage threshold is set (≥50% for initial baseline)
- [ ] Build fails if coverage drops below threshold

### TB-004: Enforce CSharpier formatting in CI [Medium]

**Source:** CI/CD #3
**AC:**
- [ ] CI job runs `dotnet csharpier --check .` on every PR
- [ ] PR fails if any file is not formatted
- [ ] Contributing guide documents the `dotnet csharpier .` command

### TB-005: Wrap static theme managers with injectable interfaces [High]

**Source:** Architecture #1
**AC:**
- [ ] `IApplicationThemeManager` interface exists with `Apply()`, `GetAppTheme()`, and `Changed` event
- [ ] `IApplicationAccentColorManager` interface exists with `Apply()` and `GetColorizationColor()`
- [ ] Default implementations delegate to existing static classes
- [ ] Interfaces are registered in DI via `ServiceCollectionExtensions`
- [ ] Existing static API remains functional (non-breaking change)

### TB-006: Document NavigationView page caching strategy [High]

**Source:** Architecture #2
**AC:**
- [ ] XML doc comments on `NavigationCacheMode` enum explain each mode
- [ ] `docs/architecture/cross-cutting/navigation.md` describes caching lifecycle
- [ ] Gallery demo includes a page demonstrating cache mode differences
- [ ] At least one unit test verifies cache behavior per mode

### TB-007: Implement or remove Wpf.Ui.ToastNotifications [High]

**Source:** Technical Debt #1
**AC:**
- [ ] Decision documented: implement with Windows App SDK toast APIs OR remove from solution
- [ ] If implemented: at least one functional toast notification scenario works end-to-end
- [ ] If removed: NuGet package is delisted, project removed from solution, references cleaned up

### TB-008: Audit and reduce Win32/Interop public API surface [Medium]

**Source:** Technical Debt #2
**AC:**
- [ ] All types in `Wpf.Ui.Interop` and `Wpf.Ui.Win32` are marked `internal` or documented as public API
- [ ] `[EditorBrowsable(EditorBrowsableState.Never)]` added to types that must remain public for binary compat
- [ ] No consumer-facing documentation references internal interop types

### TB-009: Add integration tests for NavigationView caching [High]

**Source:** Testing #3
**AC:**
- [ ] FlaUI integration test navigates forward/backward and verifies page instance identity (cached vs new)
- [ ] Test covers `NavigationCacheMode.Enabled`, `Disabled`, and `Required`
- [ ] Test verifies `INavigationAware.OnNavigatedTo`/`OnNavigatedFrom` callback order

### TB-010: Add contract tests for service interfaces [Medium]

**Source:** Testing #4
**AC:**
- [ ] `INavigationService` has ≥3 contract tests (Navigate, GoBack, SetService)
- [ ] `IContentDialogService` has ≥2 contract tests (ShowAsync, SetDialogHost)
- [ ] `ISnackbarService` has ≥2 contract tests (Show, SetPresenter)
- [ ] Tests verify interface contracts, not implementation details
- [ ] Tests use NSubstitute for mock dependencies
