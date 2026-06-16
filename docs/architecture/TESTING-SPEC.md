# Testing Specification for AI Agents

## Overview

This document provides testing guidelines for AI agents working on the WPF UI project. The project uses separate unit and integration test suites with distinct frameworks and patterns.

## Test Project Structure

### Unit Tests
**Project:** `tests/Wpf.Ui.UnitTests/`
**Target Framework:** `net10.0-windows`
**Project Reference:** `Wpf.Ui`

### Integration Tests
**Project:** `tests/Wpf.Ui.Gallery.IntegrationTests/`
**Target Framework:** `net10.0-windows10.0.26100.0`
**Project References:** `Wpf.Ui.FlaUI`, `Wpf.Ui.Gallery`

## Testing Frameworks

### Unit Testing Stack
- **XUnit v2-style** (PackageReference: `xunit`, `xunit.runner.visualstudio`)
- **NSubstitute 5.3.0** - Mocking framework
- **Standard XUnit Assert** - Assertion library
- **Coverlet 6.0.4** - Code coverage collector

### Integration Testing Stack
- **XUnit v3** (PackageReference: `xunit.v3`, `xunit.runner.visualstudio`)
- **FlaUI.UIA3 5.0.0** - UI automation framework
- **AwesomeAssertions 9.3.0** - Fluent assertion library (FluentAssertions successor)
- **Custom Wpf.Ui.FlaUI** - Custom automation element wrappers

## Unit Test Template

### Naming Convention
```
MethodName_ExpectedResult_WhenCondition
```

**Alternative format:**
```
GivenCondition_MethodName_ExpectedResult
```

### Basic Structure
```csharp
using Xunit;
using NSubstitute;
using Wpf.Ui.Animations; // Namespace under test

namespace Wpf.Ui.UnitTests.Animations;

public class TransitionAnimationProviderTests
{
    [Fact]
    public void ApplyTransition_ReturnsFalse_WhenDurationIsLessThan10()
    {
        // Arrange
        UIElement mockedUiElement = Substitute.For<UIElement>();

        // Act
        var result = TransitionAnimationProvider.ApplyTransition(
            mockedUiElement,
            Transition.FadeIn,
            -10
        );

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ApplyTransition_ReturnsFalse_WhenElementIsNull()
    {
        // Arrange
        UIElement? nullElement = null;

        // Act
        var result = TransitionAnimationProvider.ApplyTransition(
            nullElement,
            Transition.FadeIn,
            100
        );

        // Assert
        Assert.False(result);
    }
}
```

### Mocking with NSubstitute
```csharp
// Create mock
UIElement element = Substitute.For<UIElement>();

// Setup return value
INavigationService service = Substitute.For<INavigationService>();
service.Navigate(typeof(DashboardPage)).Returns(true);

// Verify call
service.Received(1).Navigate(Arg.Any<Type>());
```

### Global Usings
Located in `tests/Wpf.Ui.UnitTests/Usings.cs`:
```csharp
global using System;
global using System.Windows;
global using NSubstitute;
global using Xunit;
```

## Integration Test Template

### Naming Convention
```
Subject_ShouldExpectedBehavior_WhenCondition
```

### Base Class Pattern
All integration tests inherit from `UiTest`:

```csharp
using AwesomeAssertions;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3.Patterns;

namespace Wpf.Ui.Gallery.IntegrationTests;

public sealed class NavigationTests : UiTest
{
    [Fact]
    public async Task Settings_ShouldBeAvailable_ThroughAutoSuggestBox()
    {
        // Arrange
        Wpf.Ui.FlaUI.AutoSuggestBox? autoSuggestBox =
            FindFirst("NavigationAutoSuggestBox")?.As<AutoSuggestBox>();

        autoSuggestBox.Should().NotBeNull(
            "because AutoSuggestBox should be present in the navigation bar"
        );

        // Act
        autoSuggestBox!.Enter("Settings");
        await Wait(1);

        // Assert
        TextBox? pageTitle = FindFirst("PageTitle")?.AsTextBox();
        pageTitle.Should().NotBeNull();
        pageTitle!.Text.Should().Be("Settings");
    }
}
```

### UiTest Base Class Features

**Location:** `tests/Wpf.Ui.Gallery.IntegrationTests/Fixtures/UiTest.cs`

**Provided Methods:**
```csharp
// Find element by automation ID
protected AutomationElement? FindFirst(string automationId)

// Find element by condition
protected AutomationElement? FindFirst(Func<ConditionFactory, ConditionBase> buildCondition)

// Wait for specified seconds
protected async Task Wait(int seconds, CancellationToken cancellationToken = default)

// Type text
protected void Enter(string text)

// Press key
protected void Press(VirtualKeyShort key)
```

**Lifecycle:**
- `IAsyncLifetime` implementation
- Each test gets its own application instance
- Application launched via `TestedApplication` fixture
- Automatic cleanup after test completion

### AwesomeAssertions Syntax
```csharp
// Null checks
element.Should().NotBeNull("because element must exist");
element.Should().BeNull();

// String assertions
text.Should().Be("Expected");
text.Should().Contain("substring");
text.Should().StartWith("prefix");

// Boolean assertions
condition.Should().BeTrue("because condition must be met");
Application?.HasExited.Should().BeTrue();

// Collection assertions
items.Should().HaveCount(5);
items.Should().Contain(item);
```

### FlaUI Element Access
```csharp
// Find and cast to specific control
Button? button = FindFirst("ButtonId").AsButton();
TextBox? textBox = FindFirst("TextBoxId")?.AsTextBox();

// Custom automation elements
var autoSuggestBox = FindFirst("AutoSuggestBoxId")?.AsAutoSuggestBox();

// Interact with controls
button.Click(moveMouse: false);
textBox.Text = "value";

// Pattern-based interaction
var invokePattern = element.Patterns.Invoke.Pattern;
invokePattern.Invoke();
```

## Running Tests

### Unit Tests
```bash
# Run all unit tests
dotnet test tests/Wpf.Ui.UnitTests/

# Run specific test class
dotnet test tests/Wpf.Ui.UnitTests/ --filter "FullyQualifiedName~TransitionAnimationProviderTests"

# Run with coverage
dotnet test tests/Wpf.Ui.UnitTests/ --collect:"XPlat Code Coverage"
```

### Integration Tests
```bash
# Run all integration tests
dotnet test tests/Wpf.Ui.Gallery.IntegrationTests/

# Run specific test
dotnet test tests/Wpf.Ui.Gallery.IntegrationTests/ --filter "FullyQualifiedName~TitleBarTests"

# Run with diagnostics
dotnet test tests/Wpf.Ui.Gallery.IntegrationTests/ --logger "console;verbosity=detailed"
```

### XUnit Runner Configuration
**Location:** `tests/Wpf.Ui.Gallery.IntegrationTests/xunit.runner.json`

```json
{
  "$schema": "https://xunit.net/schema/current/xunit.runner.schema.json",
  "parallelizeTestCollections": false,
  "diagnosticMessages": true,
  "culture": "invariant"
}
```

**Important:** Integration tests do NOT run in parallel due to single Gallery app instance.

## Test Organization

### Namespace Mirroring
Test namespaces mirror source namespaces:

```
src/Wpf.Ui/Animations/TransitionAnimationProvider.cs
↓
tests/Wpf.Ui.UnitTests/Animations/TransitionAnimationProviderTests.cs
```

### File Naming
```
{ClassName}Tests.cs
```

### Test Coverage Areas

**Current Unit Test Coverage:**
- Animations: `TransitionAnimationProvider`
- Extensions: `SymbolExtensions.Swap()`, `SymbolExtensions.GetString()`

**Current Integration Test Coverage:**
- Window title verification
- TitleBar button interaction (Close, Minimize, Maximize)
- Navigation via AutoSuggestBox
- Navigation via NavigationView
- ContentDialog result verification
- ContentDialog keyboard focus isolation

## Guidelines for AI Agents

### When Writing Unit Tests

1. **Mock dependencies** using NSubstitute
2. **Test public API surface** only
3. **Use XUnit Assert methods** for assertions
4. **Follow naming convention** strictly
5. **One assertion per test** (where logical)
6. **Test both success and failure paths**

### When Writing Integration Tests

1. **Inherit from UiTest** base class
2. **Use automation IDs** for element discovery
3. **Add delays with Wait()** for UI updates
4. **Use AwesomeAssertions fluent syntax**
5. **Provide clear "because" messages** in assertions
6. **Clean up state** (handled automatically by fixture)

### Test File Template Locations

**Unit Test Template:**
```
tests/Wpf.Ui.UnitTests/Animations/TransitionAnimationProviderTests.cs
tests/Wpf.Ui.UnitTests/Extensions/SymbolExtensionsTests.cs
```

**Integration Test Template:**
```
tests/Wpf.Ui.Gallery.IntegrationTests/TitleBarTests.cs
tests/Wpf.Ui.Gallery.IntegrationTests/NavigationTests.cs
```

## Common Patterns

### Testing Dependency Properties
```csharp
[Fact]
public void PropertyName_DefaultValue_IsExpected()
{
    var control = new MyControl();
    Assert.Equal(expectedDefault, control.PropertyName);
}

[Fact]
public void PropertyName_CanBeSet_AndRetrieved()
{
    var control = new MyControl();
    var expectedValue = new SomeType();

    control.PropertyName = expectedValue;

    Assert.Equal(expectedValue, control.PropertyName);
}
```

### Testing Services
```csharp
[Fact]
public void Navigate_ReturnsTrue_WhenNavigationSucceeds()
{
    // Arrange
    var pageProvider = Substitute.For<INavigationViewPageProvider>();
    pageProvider.GetPage(Arg.Any<Type>()).Returns(new DashboardPage());

    var service = new NavigationService(pageProvider);
    var navigationView = Substitute.For<INavigationView>();
    service.SetNavigationControl(navigationView);

    // Act
    bool result = service.Navigate(typeof(DashboardPage));

    // Assert
    Assert.True(result);
}
```

## Continuous Integration

Tests are NOT currently run in CI (PR validator only builds Gallery app).

**To add test execution to CI**, modify `.github/workflows/wpf-ui-pr-validator.yaml`:
```yaml
- name: Run Unit Tests
  run: dotnet test tests/Wpf.Ui.UnitTests/ --no-restore --verbosity normal

- name: Run Integration Tests
  run: dotnet test tests/Wpf.Ui.Gallery.IntegrationTests/ --no-restore --verbosity normal
```
