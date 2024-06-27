// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;
using Wpf.Ui.Appearance;
using Wpf.Ui.Designer;
using Wpf.Ui.Input;
#pragma warning disable WPF0041

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

[TemplatePart(Name = "PART_Icon", Type = typeof(ContentPresenter))]
public class TitleBar : Control, IThemeControl
{
    private Window _currentWindow = null!;
    private DependencyObject? _parentWindow;
    private ContentPresenter? _iconPresenter;
    private static DpiScale? _dpiScale;
    private readonly Point _relativePoint = new(-15, 48);

    /// <summary>Identifies the <see cref="ApplicationTheme"/> dependency property.</summary>
    public static readonly DependencyProperty ApplicationThemeProperty =
        DependencyProperty.Register(
            nameof(ApplicationTheme),
            typeof(ApplicationTheme),
            typeof(TitleBar),
            new PropertyMetadata(ApplicationTheme.Unknown)
            );

    /// <summary>Identifies the <see cref="Title"/> dependency property.</summary>
    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(
            nameof(Title),
            typeof(string),
            typeof(TitleBar),
            new PropertyMetadata(null)
            );

    /// <summary>Identifies the <see cref="Header"/> dependency property.</summary>
    public static readonly DependencyProperty HeaderProperty =
        DependencyProperty.Register(
            nameof(Header),
            typeof(object),
            typeof(TitleBar),
            new PropertyMetadata(null)
            );

    /// <summary>Identifies the <see cref="AdditionalControls"/> dependency property.</summary>
    public static readonly DependencyProperty AdditionalControlsProperty =
        DependencyProperty.Register(
            nameof(AdditionalControls),
            typeof(object),
            typeof(TitleBar),
            new PropertyMetadata(null)
            );

    /// <summary>Identifies the <see cref="Icon"/> dependency property.</summary>
    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register(
            nameof(Icon),
            typeof(IconElement),
            typeof(TitleBar),
            new PropertyMetadata(null)
            );

    /// <summary>Identifies the <see cref="CloseWindowByDoubleClickOnIcon"/> dependency property.</summary>
    public static readonly DependencyProperty CloseWindowByDoubleClickOnIconProperty =
        DependencyProperty.Register(
            nameof(CloseWindowByDoubleClickOnIcon),
            typeof(bool),
            typeof(TitleBar),
            new PropertyMetadata(false)
        );

    /// <summary>Identifies the <see cref="ShowMaximize"/> dependency property.</summary>
    public static readonly DependencyProperty ShowMaximizeProperty =
        DependencyProperty.Register(
            nameof(ShowMaximize),
            typeof(bool),
            typeof(TitleBar),
            new PropertyMetadata(true)
            );

    /// <summary>Identifies the <see cref="ShowMinimize"/> dependency property.</summary>
    public static readonly DependencyProperty ShowMinimizeProperty =
        DependencyProperty.Register(
            nameof(ShowMinimize),
            typeof(bool),
            typeof(TitleBar),
            new PropertyMetadata(true)
            );

    /// <summary>Identifies the <see cref="ShowHelp"/> dependency property.</summary>
    public static readonly DependencyProperty ShowHelpProperty =
        DependencyProperty.Register(
            nameof(ShowHelp),
            typeof(bool),
            typeof(TitleBar),
            new PropertyMetadata(false)
            );

    /// <summary>Identifies the <see cref="ShowClose"/> dependency property.</summary>
    public static readonly DependencyProperty ShowCloseProperty =
        DependencyProperty.Register(
            nameof(ShowClose),
            typeof(bool),
            typeof(TitleBar),
            new PropertyMetadata(true)
            );

    /// <summary>Identifies the <see cref="CanMaximize"/> dependency property.</summary>
    public static readonly DependencyProperty CanMaximizeProperty =
        DependencyProperty.Register(
            nameof(CanMaximize),
            typeof(bool),
            typeof(TitleBar),
            new PropertyMetadata(true)
        );

    /// <summary>Identifies the <see cref="IsMaximized"/> dependency property.</summary>
    public static readonly DependencyProperty IsMaximizedProperty =
        DependencyProperty.Register(
            nameof(IsMaximized),
            typeof(bool),
            typeof(TitleBar),
            new PropertyMetadata(false)
        );

    /// <summary>Identifies the <see cref="ForceShutdown"/> dependency property.</summary>
    public static readonly DependencyProperty ForceShutdownProperty =
        DependencyProperty.Register(
            nameof(ForceShutdown),
            typeof(bool),
            typeof(TitleBar),
            new PropertyMetadata(false)
            );

    /// <summary>Identifies the <see cref="UseTrayMenu"/> dependency property.</summary>
    public static readonly DependencyProperty UseTrayMenuProperty =
        DependencyProperty.Register(
            nameof(UseTrayMenu),
            typeof(bool),
            typeof(TitleBar),
            new PropertyMetadata(false)
            );

    /// <inheritdoc />
    public ApplicationTheme ApplicationTheme
    {
        get => (ApplicationTheme)GetValue(ApplicationThemeProperty);
        set => SetValue(ApplicationThemeProperty, value);
    }

    /// <summary>
    /// Gets or sets title displayed on the left.
    /// </summary>
    public string? Title
    {
        get => (string?)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    /// <summary>
    /// Gets or sets the content displayed in the <see cref="Header"/> section.
    /// </summary>
    public object? Header
    {
        get => (object?)GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    /// <summary>
    /// Gets or sets the content displayed in the <see cref="AdditionalControls"/> section.
    /// </summary>
    public object? AdditionalControls
    {
        get => (object?)GetValue(AdditionalControlsProperty);
        set => SetValue(AdditionalControlsProperty, value);
    }

    /// <summary>
    /// Gets or sets the titlebar icon.
    /// </summary>
    public IconElement? Icon
    {
        get => (IconElement?)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the window can be closed by double-clicking on the icon
    /// </summary>
    public bool CloseWindowByDoubleClickOnIcon
    {
        get => (bool)GetValue(CloseWindowByDoubleClickOnIconProperty);
        set => SetValue(CloseWindowByDoubleClickOnIconProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether to show the minimize button.
    /// </summary>
    public bool ShowMaximize
    {
        get => (bool)GetValue(ShowMaximizeProperty);
        set => SetValue(ShowMaximizeProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether to show the maximize button.
    /// </summary>
    public bool ShowMinimize
    {
        get => (bool)GetValue(ShowMinimizeProperty);
        set => SetValue(ShowMinimizeProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether to show the help button
    /// </summary>
    public bool ShowHelp
    {
        get => (bool)GetValue(ShowHelpProperty);
        set => SetValue(ShowHelpProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether to show the close button.
    /// </summary>
    public bool ShowClose
    {
        get => (bool)GetValue(ShowCloseProperty);
        set => SetValue(ShowCloseProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the maximize functionality is enabled. If disabled the MaximizeActionOverride action won't be called
    /// </summary>
    public bool CanMaximize
    {
        get => (bool)GetValue(CanMaximizeProperty);
        set => SetValue(CanMaximizeProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the current window is maximized.
    /// </summary>
    public bool IsMaximized
    {
        get => (bool)GetValue(IsMaximizedProperty);
        set => SetValue(IsMaximizedProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the controls affect main application window.
    /// </summary>
    public bool ForceShutdown
    {
        get => (bool)GetValue(ForceShutdownProperty);
        set => SetValue(ForceShutdownProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="Application"/> closes or hides if using a tray menu.
    /// </summary>
    public bool UseTrayMenu
    {
        get => (bool)GetValue(UseTrayMenuProperty);
        set => SetValue(UseTrayMenuProperty, value);
    }

    /// <summary>
    /// Gets or sets what <see cref="Action"/> should be executed when the Help button is clicked.
    /// </summary>
    public Action<TitleBar, Window>? HelpActionOverride { get; set; }

    /// <summary>
    /// Gets or sets what <see cref="Action"/> should be executed when the Minimize button is clicked.
    /// </summary>
    public Action<TitleBar, Window>? MinimizeActionOverride { get; set; }

    /// <summary>
    /// Gets or sets what <see cref="Action"/> should be executed when the Maximize button is clicked.
    /// </summary>
    public Action<TitleBar, Window>? MaximizeActionOverride { get; set; }

    /// <summary>
    /// Gets or sets what <see cref="Action"/> should be executed when the Close button is clicked.
    /// </summary>
    public Action<TitleBar, Window>? CloseActionOverride { get; set; }

    /// <summary>
    /// Gets the command triggered when clicking the titlebar button.
    /// </summary>
    public RelayCommand<string> ButtonCommand { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TitleBar"/> class and sets the default <see cref="FrameworkElement.Loaded"/> event.
    /// </summary>
    public TitleBar()
    {
        _dpiScale ??= VisualTreeHelper.GetDpi(this);

        Loaded += OnLoaded;
        Unloaded += (_, _) =>
        {
            ApplicationThemeManager.Changed -= OnThemeChanged;
        };
        MouseDoubleClick += (_, ie) =>
        {
            if (IsMouseOverButton(this))
            {
                return;
            }

            MaximizeWindow();
            ie.Handled = true;
        };
        ButtonCommand = new RelayCommand<string>(
            execute: command =>
        {
            if (command is null)
            {
                return;
            }

            switch (command)
            {
                case "Help":
                    HelpClicked();
                    break;
                case "Minimize":
                    MinimizeWindow();
                    break;
                case "Maximize":
                    MaximizeWindow();
                    break;
                case "Close":
                    CloseWindow();
                    break;
            }
        },
            canExecute: _ => true
            );
    }

    /// <summary>
    /// Gets or sets a value indicating if the mouse is over any <see cref="Button"/>.
    /// </summary>
    /// <returns><see cref="bool"/></returns>
    private static bool IsMouseOverButton(FrameworkElement? titleBar) =>
        FindVisualChildren<Button>(titleBar).Any(button => button.IsMouseOver);

    private static IEnumerable<T> FindVisualChildren<T>(DependencyObject? depObj)
        where T : DependencyObject
    {
        if (depObj == null)
        {
            yield break;
        }

        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
        {
            DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
            if (child is T t)
            {
                yield return t;
            }

            foreach (T childOfChild in FindVisualChildren<T>(child))
            {
                yield return childOfChild;
            }
        }
    }

    private void HelpClicked()
    {
        HelpActionOverride?.Invoke(this, _currentWindow);
    }

    private void MinimizeWindow()
    {
        if (MinimizeActionOverride is not null)
        {
            MinimizeActionOverride(this, _currentWindow);
            return;
        }

        _currentWindow.SetCurrentValue(Window.WindowStateProperty, WindowState.Minimized);
    }

    private void MaximizeWindow()
    {
        if (!CanMaximize)
        {
            return;
        }

        if (MaximizeActionOverride is not null)
        {
            MaximizeActionOverride(this, _currentWindow);
            return;
        }

        switch (_currentWindow.WindowState)
        {
            case WindowState.Normal:
                _currentWindow.SetCurrentValue(Window.WindowStateProperty, WindowState.Maximized);
                break;
            case WindowState.Maximized:
                _currentWindow.SetCurrentValue(Window.WindowStateProperty, WindowState.Normal);
                break;
            case WindowState.Minimized:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void CloseWindow()
    {
        if (CloseActionOverride is not null)
        {
            CloseActionOverride(this, _currentWindow);
            return;
        }

        if (ForceShutdown)
        {
            UiApplication.Current.Shutdown();
            return;
        }

        if (UseTrayMenu)
        {
            _currentWindow.Hide();
            return;
        }

        _currentWindow.Close();
    }

    /// <summary>
    /// Show 'SystemMenu' on mouse right button up.
    /// </summary>
    private void TitleBar_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
    {
        Point point = PointToScreen(e.GetPosition(this));

        if (_dpiScale is null)
        {
            throw new InvalidOperationException("dpiScale is not initialized.");
        }

        SystemCommands.ShowSystemMenu(
            _parentWindow as Window,
            new Point(point.X / _dpiScale.Value.DpiScaleX, point.Y / _dpiScale.Value.DpiScaleY)
        );
    }

    /// <summary>
    /// Invoked whenever application code or an internal process,
    /// such as a rebuilding layout pass, calls the ApplyTemplate method.
    /// </summary>
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _parentWindow = VisualTreeHelper.GetParent(this);

        while (_parentWindow is not null and not Window)
        {
            _parentWindow = VisualTreeHelper.GetParent(_parentWindow);
        }

        _iconPresenter = GetTemplateChild("PART_Icon") as ContentPresenter;
        MouseRightButtonUp += TitleBar_MouseRightButtonUp;
    }

    protected virtual void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DesignerHelper.IsInDesignMode)
        {
            return;
        }

        _currentWindow = Window.GetWindow(this) ?? throw new InvalidOperationException("Window is null");
        _currentWindow.StateChanged += (_, _) =>
        {
            if (IsMaximized != (_currentWindow.WindowState == WindowState.Maximized))
            {
                SetCurrentValue(IsMaximizedProperty, _currentWindow.WindowState == WindowState.Maximized);
            }
        };

        MouseMove += (_, te) =>
        {
            if (te.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }

            if (_currentWindow.WindowState == WindowState.Maximized)
            {
                // Do not use DependencyProperty to set the window state and postion here.
                Point mouseRelativeToScreen = te.GetPosition(null);
                var screenWidth = SystemParameters.PrimaryScreenWidth;
                var screenHeight = SystemParameters.PrimaryScreenHeight;
                _currentWindow.WindowState = WindowState.Normal;
                var newLeft = mouseRelativeToScreen.X - (_currentWindow.ActualWidth / 2);
                var newTop = mouseRelativeToScreen.Y - 10;
                newLeft = Math.Max(0, Math.Min(newLeft, screenWidth - _currentWindow.ActualWidth));
                newTop = Math.Max(0, Math.Min(newTop, screenHeight - _currentWindow.ActualHeight));
                _currentWindow.Left = newLeft;
                _currentWindow.Top = newTop;
            }

            _currentWindow.DragMove();
        };

        if (_iconPresenter == null)
        {
            return;
        }

        _iconPresenter.MouseLeftButtonDown += (_, ie) =>
        {
            switch (CloseWindowByDoubleClickOnIcon)
            {
                case true when ie.ClickCount == 2:
                    _currentWindow.Close();
                    break;
                case false when ie.ClickCount > 0:
                    Point point = _iconPresenter.PointToScreen(_relativePoint);
                    SystemCommands.ShowSystemMenu(_parentWindow as Window, point);
                    Debug.WriteLine($"SYSMENU | Point.X = {point.X} Point.Y = {point.Y}");
                    break;
            }
        };
    }

    /// <inheritdoc />
    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);
        SetCurrentValue(ApplicationThemeProperty, ApplicationThemeManager.GetAppTheme());
        ApplicationThemeManager.Changed += OnThemeChanged;
    }

    /// <summary>
    /// This virtual method is triggered when the app's theme changes.
    /// </summary>
    protected virtual void OnThemeChanged(ApplicationTheme currentApplicationTheme, Color systemAccent)
    {
        Debug.WriteLine(
            $"INFO | {typeof(TitleBar)} received theme -  {currentApplicationTheme}",
            "Wpf.Ui.TitleBar"
        );
        SetCurrentValue(ApplicationThemeProperty, currentApplicationTheme);
    }
}