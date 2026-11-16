# WPF UI Implementation Guide

Step-by-step guides for common implementation tasks in the WPF UI project.

## Table of Contents
1. [How to Add a New Control](#1-how-to-add-a-new-control)
2. [How to Add a New Service](#2-how-to-add-a-new-service)
3. [How to Add a Gallery Page](#3-how-to-add-a-gallery-page)
4. [How to Use Win32 Interop](#4-how-to-use-win32-interop)
5. [How to Add Theme Support](#5-how-to-add-theme-support)
6. [Testing a Feature](#6-testing-a-feature)

---

## 1. How to Add a New Control

This guide walks through creating a new control following the WPF UI conventions.

### Step 1: Create Control Folder Structure

```
src/Wpf.Ui/Controls/
└── MyControl/
    ├── MyControl.cs       # Code-behind class
    └── MyControl.xaml     # Style and ControlTemplate
```

### Step 2: Create the Control Class (MyControl.cs)

```csharp
// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// A custom control that demonstrates the WPF UI control pattern.
/// </summary>
/// <example>
/// <code lang="xml">
/// &lt;ui:MyControl
///     Content="Hello World"
///     Appearance="Primary"
///     Icon="{ui:SymbolIcon Symbol=Heart24}" /&gt;
/// </code>
/// </example>
public class MyControl : ContentControl, IAppearanceControl, IIconControl
{
    /// <summary>Identifies the <see cref="Appearance"/> dependency property.</summary>
    public static readonly DependencyProperty AppearanceProperty = DependencyProperty.Register(
        nameof(Appearance),
        typeof(ControlAppearance),
        typeof(MyControl),
        new PropertyMetadata(ControlAppearance.Primary)
    );

    /// <summary>Identifies the <see cref="Icon"/> dependency property.</summary>
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        nameof(Icon),
        typeof(IconElement),
        typeof(MyControl),
        new PropertyMetadata(null, null, IconElement.Coerce)
    );

    /// <summary>Identifies the <see cref="IsCustom"/> dependency property.</summary>
    public static readonly DependencyProperty IsCustomProperty = DependencyProperty.Register(
        nameof(IsCustom),
        typeof(bool),
        typeof(MyControl),
        new PropertyMetadata(false, OnIsCustomChanged)
    );

    static MyControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(MyControl),
            new FrameworkPropertyMetadata(typeof(MyControl))
        );
    }

    /// <summary>
    /// Gets or sets the appearance of the control.
    /// </summary>
    [Bindable(true)]
    [Category("Appearance")]
    public ControlAppearance Appearance
    {
        get => (ControlAppearance)GetValue(AppearanceProperty);
        set => SetValue(AppearanceProperty, value);
    }

    /// <summary>
    /// Gets or sets the icon displayed in the control.
    /// </summary>
    [Bindable(true)]
    [Category("Appearance")]
    public IconElement? Icon
    {
        get => (IconElement?)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the control uses custom behavior.
    /// </summary>
    [Bindable(true)]
    [Category("Behavior")]
    public bool IsCustom
    {
        get => (bool)GetValue(IsCustomProperty);
        set => SetValue(IsCustomProperty, value);
    }

    private static void OnIsCustomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not MyControl control)
        {
            return;
        }

        control.OnIsCustomChanged((bool)e.NewValue);
    }

    protected virtual void OnIsCustomChanged(bool isCustom)
    {
        // Handle property change
    }
}
```

### Step 3: Create the XAML Style (MyControl.xaml)

```xml
<!--
    This Source Code Form is subject to the terms of the MIT License.
    If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
    Copyright (C) Leszek Pomianowski and WPF UI Contributors.
    All Rights Reserved.

    Based on Microsoft XAML for Win UI
    Copyright (c) Microsoft Corporation. All Rights Reserved.
-->
<ResourceDictionary
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:controls="clr-namespace:Wpf.Ui.Controls"
    xmlns:system="clr-namespace:System;assembly=mscorlib">

    <!-- Local Resources -->
    <Thickness x:Key="MyControlPadding">11,5,11,6</Thickness>
    <Thickness x:Key="MyControlBorderThickness">1</Thickness>
    <Thickness x:Key="MyControlIconMargin">0,0,8,0</Thickness>
    <system:Double x:Key="MyControlMinHeight">32</system:Double>

    <!-- Default Style -->
    <Style TargetType="{x:Type controls:MyControl}">
        <Setter Property="OverridesDefaultStyle" Value="True" />
        <Setter Property="SnapsToDevicePixels" Value="True" />
        <Setter Property="Background" Value="{DynamicResource ControlFillColorDefaultBrush}" />
        <Setter Property="Foreground" Value="{DynamicResource TextFillColorPrimaryBrush}" />
        <Setter Property="BorderBrush" Value="{DynamicResource ControlElevationBorderBrush}" />
        <Setter Property="BorderThickness" Value="{StaticResource MyControlBorderThickness}" />
        <Setter Property="Padding" Value="{StaticResource MyControlPadding}" />
        <Setter Property="MinHeight" Value="{StaticResource MyControlMinHeight}" />
        <Setter Property="HorizontalAlignment" Value="Stretch" />
        <Setter Property="VerticalAlignment" Value="Center" />
        <Setter Property="HorizontalContentAlignment" Value="Center" />
        <Setter Property="VerticalContentAlignment" Value="Center" />
        <Setter Property="Template">
            <Setter.Value>
                <ControlTemplate TargetType="{x:Type controls:MyControl}">
                    <Border
                        x:Name="Border"
                        Background="{TemplateBinding Background}"
                        BorderBrush="{TemplateBinding BorderBrush}"
                        BorderThickness="{TemplateBinding BorderThickness}"
                        CornerRadius="{DynamicResource ControlCornerRadius}">
                        <Grid>
                            <Grid.ColumnDefinitions>
                                <ColumnDefinition Width="Auto" />
                                <ColumnDefinition Width="*" />
                            </Grid.ColumnDefinitions>

                            <!-- Icon -->
                            <ContentPresenter
                                x:Name="PART_Icon"
                                Grid.Column="0"
                                Margin="{StaticResource MyControlIconMargin}"
                                VerticalAlignment="Center"
                                Content="{TemplateBinding Icon}"
                                TextElement.FontSize="16"
                                TextElement.Foreground="{TemplateBinding Foreground}" />

                            <!-- Content -->
                            <ContentPresenter
                                Grid.Column="1"
                                HorizontalAlignment="{TemplateBinding HorizontalContentAlignment}"
                                VerticalAlignment="{TemplateBinding VerticalContentAlignment}"
                                Content="{TemplateBinding Content}"
                                ContentTemplate="{TemplateBinding ContentTemplate}"
                                RecognizesAccessKey="True"
                                TextElement.Foreground="{TemplateBinding Foreground}" />
                        </Grid>
                    </Border>

                    <ControlTemplate.Triggers>
                        <!-- Hide icon if not set -->
                        <Trigger Property="Icon" Value="{x:Null}">
                            <Setter TargetName="PART_Icon" Property="Visibility" Value="Collapsed" />
                            <Setter TargetName="PART_Icon" Property="Margin" Value="0" />
                        </Trigger>

                        <!-- Mouse Over -->
                        <Trigger Property="IsMouseOver" Value="True">
                            <Setter TargetName="Border" Property="Background" Value="{DynamicResource ControlFillColorSecondaryBrush}" />
                        </Trigger>

                        <!-- Disabled -->
                        <Trigger Property="IsEnabled" Value="False">
                            <Setter Property="Foreground" Value="{DynamicResource TextFillColorDisabledBrush}" />
                            <Setter TargetName="Border" Property="Background" Value="{DynamicResource ControlFillColorDisabledBrush}" />
                            <Setter TargetName="Border" Property="BorderBrush" Value="{DynamicResource ControlStrokeColorDefaultBrush}" />
                        </Trigger>

                        <!-- Appearance Variants -->
                        <Trigger Property="Appearance" Value="Primary">
                            <Setter TargetName="Border" Property="Background" Value="{DynamicResource AccentFillColorDefaultBrush}" />
                            <Setter Property="Foreground" Value="{DynamicResource TextOnAccentFillColorPrimaryBrush}" />
                        </Trigger>

                        <Trigger Property="Appearance" Value="Secondary">
                            <Setter TargetName="Border" Property="Background" Value="{DynamicResource ControlFillColorSecondaryBrush}" />
                        </Trigger>
                    </ControlTemplate.Triggers>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
    </Style>
</ResourceDictionary>
```

### Step 4: Register in Wpf.Ui.xaml

Add reference to `src/Wpf.Ui/Resources/Wpf.Ui.xaml`:

```xml
<ResourceDictionary.MergedDictionaries>
    <!-- Existing entries... -->
    <ResourceDictionary Source="pack://application:,,,/Wpf.Ui;component/Controls/MyControl/MyControl.xaml" />
</ResourceDictionary.MergedDictionaries>
```

### Step 5: Add Designer Support (Optional)

Add toolbox icon bitmap to `src/Wpf.Ui/Assets/Toolbox/MyControl.bmp` (16x16 pixels).

Register in `src/Wpf.Ui/VisualStudioToolsManifest.xml`:

```xml
<ToolboxItems UIFramework="WPF" VSCategory="WPF UI" BlendCategory="WPF UI">
    <Item Type="Wpf.Ui.Controls.MyControl" />
</ToolboxItems>
```

### Step 6: Add to Category Documentation

Document the control category in architecture notes for future reference.

---

## 2. How to Add a New Service

This guide shows how to create a new service following the WPF UI service pattern.

### Step 1: Create the Service Interface (IMyService.cs)

```csharp
// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui;

/// <summary>
/// Provides functionality for managing custom operations.
/// </summary>
public interface IMyService
{
    /// <summary>
    /// Gets the current state of the service.
    /// </summary>
    bool IsActive { get; }

    /// <summary>
    /// Initializes the service with the specified parameter.
    /// </summary>
    /// <param name="parameter">The initialization parameter.</param>
    void Initialize(string parameter);

    /// <summary>
    /// Performs an asynchronous operation.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<bool> PerformOperationAsync(CancellationToken cancellationToken = default);
}
```

### Step 2: Create the Service Implementation (MyService.cs)

```csharp
// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Wpf.Ui;

/// <summary>
/// Implementation of <see cref="IMyService"/>.
/// </summary>
public partial class MyService : IMyService
{
    private string? _parameter;
    private bool _isActive;

    /// <inheritdoc />
    public bool IsActive => _isActive;

    /// <inheritdoc />
    public void Initialize(string parameter)
    {
        ArgumentNullException.ThrowIfNull(parameter);

        _parameter = parameter;
        _isActive = true;
    }

    /// <inheritdoc />
    public async Task<bool> PerformOperationAsync(CancellationToken cancellationToken = default)
    {
        ThrowIfNotInitialized();

        try
        {
            // Perform async operation
            await Task.Delay(100, cancellationToken);
            return true;
        }
        catch (OperationCanceledException)
        {
            return false;
        }
    }

    private void ThrowIfNotInitialized()
    {
        if (!_isActive || _parameter is null)
        {
            throw new InvalidOperationException("Service has not been initialized. Call Initialize() first.");
        }
    }
}
```

### Step 3: Add DI Extension Method

Create `src/Wpf.Ui.DependencyInjection/ServiceCollectionExtensions.MyService.cs`:

```csharp
// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Microsoft.Extensions.DependencyInjection;
using Wpf.Ui;

namespace Wpf.Ui.DependencyInjection;

/// <summary>
/// Extension methods for registering WPF UI services.
/// </summary>
public static partial class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="IMyService"/> as a singleton.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddMyService(this IServiceCollection services)
    {
        _ = services.AddSingleton<IMyService, MyService>();
        return services;
    }
}
```

### Step 4: Register in Application

```csharp
// In your application startup (e.g., App.xaml.cs or Program.cs)
services.AddMyService();
```

### Step 5: Use the Service

```csharp
public partial class MyViewModel : ViewModel
{
    private readonly IMyService _myService;

    public MyViewModel(IMyService myService)
    {
        _myService = myService;
        _myService.Initialize("parameter-value");
    }

    [RelayCommand]
    private async Task OnPerformAction()
    {
        bool result = await _myService.PerformOperationAsync();
        // Handle result
    }
}
```

---

## 3. How to Add a Gallery Page

This guide demonstrates adding a new demo page to the WPF UI Gallery application.

### Step 1: Create ViewModel

Create `src/Wpf.Ui.Gallery/ViewModels/Pages/MyCategory/MyControlViewModel.cs`:

```csharp
// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Controls;

namespace Wpf.Ui.Gallery.ViewModels.Pages.MyCategory;

public partial class MyControlViewModel : ViewModel
{
    [ObservableProperty]
    private string _textValue = "Hello World";

    [ObservableProperty]
    private bool _isEnabled = true;

    [ObservableProperty]
    private ControlAppearance _selectedAppearance = ControlAppearance.Primary;

    [ObservableProperty]
    private ObservableCollection<ControlAppearance> _appearances = new()
    {
        ControlAppearance.Primary,
        ControlAppearance.Secondary,
        ControlAppearance.Success,
        ControlAppearance.Danger
    };

    [RelayCommand]
    private void OnToggleEnabled()
    {
        IsEnabled = !IsEnabled;
    }

    [RelayCommand]
    private async Task OnPerformAction()
    {
        await Task.Delay(500);
        TextValue = "Action performed!";
    }

    public override Task OnNavigatedToAsync()
    {
        // Called when page is navigated to
        return base.OnNavigatedToAsync();
    }

    public override Task OnNavigatedFromAsync()
    {
        // Called when navigating away from page
        return base.OnNavigatedFromAsync();
    }
}
```

### Step 2: Create Page (XAML)

Create `src/Wpf.Ui.Gallery/Views/Pages/MyCategory/MyControlPage.xaml`:

```xml
<Page
    x:Class="Wpf.Ui.Gallery.Views.Pages.MyCategory.MyControlPage"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:local="clr-namespace:Wpf.Ui.Gallery.Views.Pages.MyCategory"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    xmlns:ui="http://schemas.lepo.co/wpfui/2022/xaml"
    Title="MyControl"
    d:DataContext="{d:DesignInstance local:MyControlPage, IsDesignTimeCreatable=False}"
    d:DesignHeight="450"
    d:DesignWidth="800"
    ui:Design.Background="{DynamicResource ApplicationBackgroundBrush}"
    ui:Design.Foreground="{DynamicResource TextFillColorPrimaryBrush}"
    Foreground="{DynamicResource TextFillColorPrimaryBrush}"
    mc:Ignorable="d">

    <StackPanel Margin="56,0">
        <!-- Header -->
        <TextBlock
            Margin="0,0,0,24"
            FontSize="32"
            FontWeight="SemiBold"
            Text="MyControl" />

        <!-- Description -->
        <ui:Card Margin="0,0,0,24" Padding="24">
            <TextBlock
                FontSize="14"
                Foreground="{DynamicResource TextFillColorSecondaryBrush}"
                TextWrapping="Wrap">
                Demonstrates the MyControl with various appearance modes and interactions.
            </TextBlock>
        </ui:Card>

        <!-- Example 1: Basic Usage -->
        <ui:Card Margin="0,0,0,24" Padding="24">
            <Grid>
                <Grid.RowDefinitions>
                    <RowDefinition Height="Auto" />
                    <RowDefinition Height="Auto" />
                </Grid.RowDefinitions>

                <TextBlock
                    Grid.Row="0"
                    Margin="0,0,0,12"
                    FontSize="16"
                    FontWeight="SemiBold"
                    Text="Basic Example" />

                <StackPanel Grid.Row="1" Spacing="8">
                    <ui:MyControl
                        Appearance="{Binding ViewModel.SelectedAppearance, Mode=TwoWay}"
                        Content="{Binding ViewModel.TextValue}"
                        Icon="{ui:SymbolIcon Symbol=Heart24}"
                        IsEnabled="{Binding ViewModel.IsEnabled}" />

                    <ComboBox
                        Header="Appearance"
                        ItemsSource="{Binding ViewModel.Appearances}"
                        SelectedItem="{Binding ViewModel.SelectedAppearance, Mode=TwoWay}" />

                    <ui:ToggleSwitch
                        Content="Is Enabled"
                        IsChecked="{Binding ViewModel.IsEnabled, Mode=TwoWay}" />

                    <ui:Button
                        Appearance="Primary"
                        Command="{Binding ViewModel.PerformActionCommand}"
                        Content="Perform Action"
                        Icon="{ui:SymbolIcon Symbol=Play24}" />
                </StackPanel>
            </Grid>
        </ui:Card>

        <!-- Example 2: XAML Code -->
        <ui:Card Margin="0,0,0,24" Padding="24">
            <Grid>
                <Grid.RowDefinitions>
                    <RowDefinition Height="Auto" />
                    <RowDefinition Height="Auto" />
                </Grid.RowDefinitions>

                <TextBlock
                    Grid.Row="0"
                    Margin="0,0,0,12"
                    FontSize="16"
                    FontWeight="SemiBold"
                    Text="XAML Usage" />

                <syntax:CodeBlock Grid.Row="1">
                    <![CDATA[
<ui:MyControl
    Content="Hello World"
    Appearance="Primary"
    Icon="{ui:SymbolIcon Symbol=Heart24}" />
                    ]]>
                </syntax:CodeBlock>
            </Grid>
        </ui:Card>
    </StackPanel>
</Page>
```

### Step 3: Create Page Code-Behind

Create `src/Wpf.Ui.Gallery/Views/Pages/MyCategory/MyControlPage.xaml.cs`:

```csharp
// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Controls;
using Wpf.Ui.Gallery.Attributes;
using Wpf.Ui.Gallery.ViewModels.Pages.MyCategory;

namespace Wpf.Ui.Gallery.Views.Pages.MyCategory;

[GalleryPage("Custom control demonstration.", SymbolRegular.Heart24)]
public partial class MyControlPage : INavigableView<MyControlViewModel>
{
    public MyControlViewModel ViewModel { get; }

    public MyControlPage(MyControlViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;
        InitializeComponent();
    }
}
```

### Step 4: Register in DI Container

Add to `src/Wpf.Ui.Gallery/App.xaml.cs` or DI configuration:

```csharp
// Register ViewModel
services.AddSingleton<MyControlViewModel>();

// Register Page
services.AddSingleton<MyControlPage>();
```

### Step 5: Add Navigation Entry

If you need manual navigation registration (not using automatic discovery), add to navigation setup:

```csharp
navigationService.SetNavigationControl(navigationView);

// Register page type
menuItems.Add(new NavigationViewItem
{
    Content = "MyControl",
    Icon = new SymbolIcon { Symbol = SymbolRegular.Heart24 },
    TargetPageType = typeof(MyControlPage)
});
```

---

## 4. How to Use Win32 Interop

This guide demonstrates the three-layer Win32 interop architecture used in WPF UI.

### Step 1: Add Function to NativeMethods.txt

Edit `src/Wpf.Ui/NativeMethods.txt`:

```text
// Existing entries...
DwmSetWindowAttribute
IsWindow

// Add your new function
GetSystemMetrics
```

This triggers CsWin32 to auto-generate P/Invoke declarations in the `Windows.Win32` namespace.

### Step 2: Create Managed Wrapper (if needed)

If you need a safe wrapper with handle validation, add to `src/Wpf.Ui/Interop/UnsafeNativeMethods.cs`:

```csharp
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;

namespace Wpf.Ui.Interop;

internal static partial class UnsafeNativeMethods
{
    /// <summary>
    /// Safely retrieves a system metric value with handle validation.
    /// </summary>
    /// <param name="handle">Window handle for context (optional).</param>
    /// <param name="metric">The system metric to retrieve.</param>
    /// <returns>The metric value, or 0 if the operation fails.</returns>
    public static int GetSystemMetricSafe(IntPtr handle, SYSTEM_METRICS_INDEX metric)
    {
        // Validate handle if provided
        if (handle != IntPtr.Zero && !PInvoke.IsWindow(new HWND(handle)))
        {
            return 0;
        }

        try
        {
            return PInvoke.GetSystemMetrics(metric);
        }
        catch
        {
            // Swallow exception for OS compatibility (intentional)
            return 0;
        }
    }
}
```

### Step 3: Create Utility Helper (if needed)

If you need higher-level utility functions, add to `src/Wpf.Ui/Win32/Utilities.cs`:

```csharp
using Wpf.Ui.Interop;
using Windows.Win32.UI.WindowsAndMessaging;

namespace Wpf.Ui.Win32;

internal sealed class Utilities
{
    /// <summary>
    /// Gets the width of the window border.
    /// </summary>
    public static int BorderWidth => UnsafeNativeMethods.GetSystemMetricSafe(
        IntPtr.Zero,
        SYSTEM_METRICS_INDEX.SM_CXBORDER
    );

    /// <summary>
    /// Gets the height of the window border.
    /// </summary>
    public static int BorderHeight => UnsafeNativeMethods.GetSystemMetricSafe(
        IntPtr.Zero,
        SYSTEM_METRICS_INDEX.SM_CYBORDER
    );
}
```

### Step 4: Use in Your Code

```csharp
using Wpf.Ui.Interop;
using Wpf.Ui.Win32;
using Windows.Win32;
using Windows.Win32.Foundation;

// Option 1: Direct P/Invoke (for simple cases)
int screenWidth = PInvoke.GetSystemMetrics(SYSTEM_METRICS_INDEX.SM_CXSCREEN);

// Option 2: Safe wrapper (validates handle, catches exceptions)
int borderWidth = UnsafeNativeMethods.GetSystemMetricSafe(
    windowHandle,
    SYSTEM_METRICS_INDEX.SM_CXBORDER
);

// Option 3: High-level utility
int borderHeight = Utilities.BorderHeight;
```

### Step 5: Handle WndProc Messages (if needed)

To intercept Windows messages:

```csharp
using System.Windows.Interop;
using Windows.Win32.UI.WindowsAndMessaging;

public class MyWindow : Window
{
    private HwndSource? _hwndSource;

    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);

        _hwndSource = (HwndSource)PresentationSource.FromVisual(this);
        _hwndSource?.AddHook(WndProc);
    }

    private IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        switch ((uint)msg)
        {
            case PInvoke.WM_DWMCOLORIZATIONCOLORCHANGED:
                // Handle theme change
                handled = true;
                break;

            case PInvoke.WM_THEMECHANGED:
                // Handle system theme change
                handled = true;
                break;
        }

        return IntPtr.Zero;
    }

    protected override void OnClosed(EventArgs e)
    {
        _hwndSource?.RemoveHook(WndProc);
        base.OnClosed(e);
    }
}
```

### Important Notes

1. **Always validate handles** before calling Win32 APIs:
   ```csharp
   if (handle == IntPtr.Zero || !PInvoke.IsWindow(new HWND(handle)))
       return false;
   ```

2. **Catch exceptions** in interop code for OS compatibility (some APIs may not exist on older Windows versions)

3. **Use `unsafe` keyword** for pointer operations

4. **Check OS version** before using new APIs:
   ```csharp
   if (Wpf.Ui.Win32.Utilities.IsOSWindows11OrNewer)
   {
       // Use Windows 11-specific API
   }
   ```

---

## 5. How to Add Theme Support

This guide shows how to make your control theme-aware.

### Step 1: Use DynamicResource in XAML

```xml
<Style TargetType="{x:Type controls:MyControl}">
    <!-- Use DynamicResource for theme-dependent brushes -->
    <Setter Property="Background" Value="{DynamicResource ControlFillColorDefaultBrush}" />
    <Setter Property="Foreground" Value="{DynamicResource TextFillColorPrimaryBrush}" />
    <Setter Property="BorderBrush" Value="{DynamicResource ControlElevationBorderBrush}" />

    <!-- Use StaticResource for constants -->
    <Setter Property="Padding" Value="{StaticResource MyControlPadding}" />
</Style>
```

### Step 2: Subscribe to Theme Changes (if needed)

If your control needs to react to theme changes programmatically:

```csharp
using Wpf.Ui.Appearance;

public class MyControl : ContentControl
{
    public MyControl()
    {
        ApplicationThemeManager.Changed += OnThemeChanged;
    }

    private void OnThemeChanged(ApplicationTheme currentTheme, Color systemAccent)
    {
        // React to theme change
        UpdateVisuals(currentTheme);
    }

    private void UpdateVisuals(ApplicationTheme theme)
    {
        // Update control state based on theme
        if (theme == ApplicationTheme.Dark)
        {
            // Dark theme-specific logic
        }
        else
        {
            // Light theme-specific logic
        }
    }
}
```

### Step 3: Use Theme-Aware Brushes

Available dynamic brush resources:

**Background Brushes**:
- `ApplicationBackgroundBrush`
- `ControlFillColorDefaultBrush`
- `ControlFillColorSecondaryBrush`
- `ControlFillColorTertiaryBrush`
- `ControlFillColorDisabledBrush`

**Foreground Brushes**:
- `TextFillColorPrimaryBrush`
- `TextFillColorSecondaryBrush`
- `TextFillColorTertiaryBrush`
- `TextFillColorDisabledBrush`

**Accent Brushes**:
- `AccentFillColorDefaultBrush`
- `AccentFillColorSecondaryBrush`
- `AccentFillColorTertiaryBrush`
- `SystemAccentColor` (Color, not Brush)

**Border Brushes**:
- `ControlElevationBorderBrush`
- `ControlStrokeColorDefaultBrush`
- `ControlStrokeColorSecondaryBrush`

### Step 4: Implement IThemeControl (if applicable)

```csharp
public class MyControl : ContentControl, IThemeControl
{
    /// <summary>Identifies the <see cref="Theme"/> dependency property.</summary>
    public static readonly DependencyProperty ThemeProperty = DependencyProperty.Register(
        nameof(Theme),
        typeof(ApplicationTheme),
        typeof(MyControl),
        new PropertyMetadata(ApplicationTheme.Light, OnThemeChanged)
    );

    /// <summary>
    /// Gets or sets the theme of the control.
    /// </summary>
    public ApplicationTheme Theme
    {
        get => (ApplicationTheme)GetValue(ThemeProperty);
        set => SetValue(ThemeProperty, value);
    }

    private static void OnThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is MyControl control)
        {
            control.UpdateTheme((ApplicationTheme)e.NewValue);
        }
    }

    private void UpdateTheme(ApplicationTheme theme)
    {
        // Update control based on theme
    }
}
```

### Step 5: Test Theme Switching

```csharp
// Apply theme programmatically
ApplicationThemeManager.Apply(ApplicationTheme.Dark);

// Apply system accent color
ApplicationAccentColorManager.ApplySystemAccent();

// Watch for system theme changes
SystemThemeWatcher.Watch(myWindow);
```

---

## 6. Testing a Feature

This guide covers both unit testing and integration testing for WPF UI.

### Unit Test Example

Create `tests/Wpf.Ui.UnitTests/Controls/MyControlTests.cs`:

```csharp
// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using NSubstitute;
using Wpf.Ui.Controls;
using Xunit;

namespace Wpf.Ui.UnitTests.Controls;

public class MyControlTests
{
    [Fact]
    public void Constructor_ShouldSetDefaultValues()
    {
        // Arrange & Act
        var control = new MyControl();

        // Assert
        Assert.Equal(ControlAppearance.Primary, control.Appearance);
        Assert.Null(control.Icon);
        Assert.False(control.IsCustom);
    }

    [Fact]
    public void Appearance_ShouldUpdateWhenSet()
    {
        // Arrange
        var control = new MyControl();

        // Act
        control.Appearance = ControlAppearance.Secondary;

        // Assert
        Assert.Equal(ControlAppearance.Secondary, control.Appearance);
    }

    [Fact]
    public void IsCustomChanged_ShouldInvokeCallback_WhenValueChanges()
    {
        // Arrange
        var control = new MyControl();
        bool callbackInvoked = false;

        // Subscribe to property change if there's a routed event
        // Or test via reflection if the method is protected

        // Act
        control.IsCustom = true;

        // Assert
        Assert.True(control.IsCustom);
    }

    [Fact]
    public void Icon_ShouldAcceptSymbolIcon()
    {
        // Arrange
        var control = new MyControl();
        var icon = new SymbolIcon { Symbol = SymbolRegular.Heart24 };

        // Act
        control.Icon = icon;

        // Assert
        Assert.Equal(icon, control.Icon);
    }
}
```

### Integration Test Example

Create `tests/Wpf.Ui.Gallery.IntegrationTests/MyControlTests.cs`:

```csharp
// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using AwesomeAssertions.Autofac;
using FlaUI.Core.AutomationElements;
using Wpf.Ui.Gallery.IntegrationTests.Fixtures;
using Xunit;

namespace Wpf.Ui.Gallery.IntegrationTests;

public sealed class MyControlTests : UiTest
{
    [Fact]
    public async Task MyControl_ShouldBeVisible_WhenPageLoads()
    {
        // Arrange
        await NavigateToPage("MyControl");

        // Act
        var control = Window.FindFirstDescendant(cf =>
            cf.ByAutomationId("MyControlExample"));

        // Assert
        control.Should().NotBeNull();
        control.IsOffscreen.Should().BeFalse();
    }

    [Fact]
    public async Task MyControl_ShouldChangeAppearance_WhenComboBoxChanged()
    {
        // Arrange
        await NavigateToPage("MyControl");

        var comboBox = Window.FindFirstDescendant(cf =>
            cf.ByAutomationId("AppearanceComboBox")).AsComboBox();

        var control = Window.FindFirstDescendant(cf =>
            cf.ByAutomationId("MyControlExample"));

        // Act
        comboBox.Select(1); // Select "Secondary"
        await Task.Delay(100);

        // Assert
        control.Should().NotBeNull();
        // Add appearance-specific assertions
    }

    [Fact]
    public async Task MyControl_ShouldDisable_WhenToggleSwitchUnchecked()
    {
        // Arrange
        await NavigateToPage("MyControl");

        var toggleSwitch = Window.FindFirstDescendant(cf =>
            cf.ByAutomationId("EnabledToggle")).AsToggleButton();

        var control = Window.FindFirstDescendant(cf =>
            cf.ByAutomationId("MyControlExample"));

        // Act
        toggleSwitch.Toggle();
        await Task.Delay(100);

        // Assert
        control.IsEnabled.Should().BeFalse();
    }

    [Fact]
    public async Task MyControl_ShouldExecuteCommand_WhenButtonClicked()
    {
        // Arrange
        await NavigateToPage("MyControl");

        var button = Window.FindFirstDescendant(cf =>
            cf.ByAutomationId("ActionButton")).AsButton();

        var textBlock = Window.FindFirstDescendant(cf =>
            cf.ByAutomationId("ResultText")).AsLabel();

        // Act
        button.Click();
        await Task.Delay(600); // Wait for async operation

        // Assert
        textBlock.Text.Should().Be("Action performed!");
    }

    private async Task NavigateToPage(string pageName)
    {
        var navigationView = Window.FindFirstDescendant(cf =>
            cf.ByAutomationId("NavigationView"));

        var menuItem = navigationView.FindFirstDescendant(cf =>
            cf.ByName(pageName));

        menuItem.Click();
        await Task.Delay(500); // Wait for navigation
    }
}
```

### Test Infrastructure

**Base Test Class** (`tests/Wpf.Ui.Gallery.IntegrationTests/Fixtures/UiTest.cs`):

```csharp
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using Xunit;

namespace Wpf.Ui.Gallery.IntegrationTests.Fixtures;

public abstract class UiTest : IAsyncLifetime
{
    protected Application? Application { get; private set; }
    protected Window Window { get; private set; } = null!;
    private Process? _process;

    public async Task InitializeAsync()
    {
        // Launch Gallery application
        var appPath = GetApplicationPath();
        _process = Process.Start(appPath);

        await Task.Delay(2000); // Wait for app to start

        // Attach to application
        Application = FlaUI.Core.Application.Attach(_process);

        using var automation = new UIA3Automation();
        Window = Application.GetMainWindow(automation);
    }

    public async Task DisposeAsync()
    {
        Application?.Close();
        Application?.Dispose();

        if (_process != null && !_process.HasExited)
        {
            _process.Kill();
            _process.Dispose();
        }

        await Task.CompletedTask;
    }

    private static string GetApplicationPath()
    {
        // Path to Gallery executable
        return Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "..\\..\\..\\..\\..\\src\\Wpf.Ui.Gallery\\bin\\Debug\\net10.0-windows\\Wpf.Ui.Gallery.exe"
        );
    }
}
```

### Running Tests

```bash
# Run all tests
dotnet test

# Run only unit tests
dotnet test tests/Wpf.Ui.UnitTests/Wpf.Ui.UnitTests.csproj

# Run only integration tests
dotnet test tests/Wpf.Ui.Gallery.IntegrationTests/Wpf.Ui.Gallery.IntegrationTests.csproj

# Run with code coverage
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

---

## Common Pitfalls to Avoid

1. **Don't add package versions to .csproj** - use `Directory.Packages.props` only

2. **Don't forget handle validation** in Win32 interop:
   ```csharp
   if (handle == IntPtr.Zero || !PInvoke.IsWindow(new HWND(handle)))
       return false;
   ```

3. **Don't use `var` for non-apparent types** (causes warning)

4. **Don't forget MIT license header** (causes error via IDE0073)

5. **Don't modify control namespace** to match folder structure - use flat `Wpf.Ui.Controls`

6. **Don't use StaticResource for theme-dependent values** - use DynamicResource

7. **Don't forget to register pages and ViewModels** in DI container

8. **Don't use `this.` qualification** (SA1101 suppressed)

9. **Don't add XML docs that restate code** - explain WHY, not WHAT

10. **Don't skip the `[GalleryPage]` attribute** on Gallery pages

---

## Quick Reference Checklist

### Adding a Control
- [ ] Create folder under `Controls/`
- [ ] Create `.cs` file with control class
- [ ] Create `.xaml` file with style
- [ ] Implement interfaces (IAppearanceControl, IIconControl)
- [ ] Register DependencyProperties with XML docs
- [ ] Override DefaultStyleKeyProperty
- [ ] Add to `Wpf.Ui.xaml` MergedDictionaries
- [ ] Add toolbox icon (optional)
- [ ] Add unit tests

### Adding a Service
- [ ] Create `I{Name}Service.cs` interface
- [ ] Create `{Name}Service.cs` implementation
- [ ] Add DI extension method in `Wpf.Ui.DependencyInjection`
- [ ] Add XML docs with examples
- [ ] Add unit tests
- [ ] Document in CLAUDE.md

### Adding a Gallery Page
- [ ] Create ViewModel in `ViewModels/Pages/{Category}/`
- [ ] Create Page.xaml in `Views/Pages/{Category}/`
- [ ] Create Page.xaml.cs code-behind
- [ ] Add `[GalleryPage]` attribute
- [ ] Register ViewModel and Page in DI
- [ ] Add navigation entry (if manual)
- [ ] Add code examples with syntax:CodeBlock
