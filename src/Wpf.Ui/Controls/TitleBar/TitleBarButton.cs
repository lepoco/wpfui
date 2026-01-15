// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using Windows.Win32;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

public class TitleBarButton : Wpf.Ui.Controls.Button
{
    // We intentionally keep this logic local to TitleBar components to avoid changing
    // global hit-testing behavior for other controls.
    internal static bool IsMouseOverNonClient(UIElement element, IntPtr lParam, double tolerance = 1.0)
    {
        // This will be invoked very often and must be as simple as possible.
        if (lParam == IntPtr.Zero)
        {
            return false;
        }

        try
        {
            // Ensure the visual is connected to a presentation source (needed for PointFromScreen).
            if (PresentationSource.FromVisual(element) == null)
            {
                return false;
            }

            long lp = lParam.ToInt64();
            int x = (short)(lp & 0xFFFF);
            int y = (short)(lp >> 16);

            var mousePosition = new Point(x, y);

            // Add a small tolerance to reduce hover flicker at pixel boundaries (rounding/DPI edge cases).
            var hitRect = new Rect(
                -tolerance,
                -tolerance,
                element.RenderSize.Width + (2 * tolerance),
                element.RenderSize.Height + (2 * tolerance)
            );

            if (!hitRect.Contains(element.PointFromScreen(mousePosition)) || !element.IsHitTestVisible)
            {
                return false;
            }

            // If element is Panel, check if children at mousePosition is with IsHitTestVisible false.
            if (element is System.Windows.Controls.Panel panel)
            {
                foreach (UIElement child in panel.Children)
                {
                    var childHitRect = new Rect(
                        -tolerance,
                        -tolerance,
                        child.RenderSize.Width + (2 * tolerance),
                        child.RenderSize.Height + (2 * tolerance)
                    );

                    if (childHitRect.Contains(child.PointFromScreen(mousePosition)))
                    {
                        return child.IsHitTestVisible;
                    }
                }

                return false;
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>Identifies the <see cref="ButtonType"/> dependency property.</summary>
    public static readonly DependencyProperty ButtonTypeProperty = DependencyProperty.Register(
        nameof(ButtonType),
        typeof(TitleBarButtonType),
        typeof(TitleBarButton),
        new PropertyMetadata(TitleBarButtonType.Unknown, OnButtonTypeChanged)
    );

    /// <summary>Identifies the <see cref="ButtonsForeground"/> dependency property.</summary>
    public static readonly DependencyProperty ButtonsForegroundProperty = DependencyProperty.Register(
        nameof(ButtonsForeground),
        typeof(Brush),
        typeof(TitleBarButton),
        new FrameworkPropertyMetadata(
            SystemColors.ControlTextBrush,
            FrameworkPropertyMetadataOptions.Inherits
        )
    );

    /// <summary>Identifies the <see cref="MouseOverButtonsForeground"/> dependency property.</summary>
    public static readonly DependencyProperty MouseOverButtonsForegroundProperty =
        DependencyProperty.Register(
            nameof(MouseOverButtonsForeground),
            typeof(Brush),
            typeof(TitleBarButton),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits)
        );

    /// <summary>Identifies the <see cref="RenderButtonsForeground"/> dependency property.</summary>
    public static readonly DependencyProperty RenderButtonsForegroundProperty = DependencyProperty.Register(
        nameof(RenderButtonsForeground),
        typeof(Brush),
        typeof(TitleBarButton),
        new FrameworkPropertyMetadata(
            SystemColors.ControlTextBrush,
            FrameworkPropertyMetadataOptions.Inherits
        )
    );

    /// <summary>
    /// Gets or sets the type of the button.
    /// </summary>
    public TitleBarButtonType ButtonType
    {
        get => (TitleBarButtonType)GetValue(ButtonTypeProperty);
        set => SetValue(ButtonTypeProperty, value);
    }

    /// <summary>
    /// Gets or sets the foreground of the navigation buttons.
    /// </summary>
    public Brush ButtonsForeground
    {
        get => (Brush)GetValue(ButtonsForegroundProperty);
        set => SetValue(ButtonsForegroundProperty, value);
    }

    /// <summary>
    /// Gets or sets the foreground of the navigation buttons when moused over.
    /// </summary>
    public Brush? MouseOverButtonsForeground
    {
        get => (Brush?)GetValue(MouseOverButtonsForegroundProperty);
        set => SetValue(MouseOverButtonsForegroundProperty, value);
    }

    public Brush RenderButtonsForeground
    {
        get => (Brush)GetValue(RenderButtonsForegroundProperty);
        set => SetValue(RenderButtonsForegroundProperty, value);
    }

    public bool IsHovered { get; private set; }

    private readonly Brush _defaultBackgroundBrush = Brushes.Transparent; // REVIEW: Should it be transparent?
    private uint _returnValue;

    private bool _isClickedDown;

    public TitleBarButton()
    {
        Loaded += TitleBarButton_Loaded;
        Unloaded += TitleBarButton_Unloaded;
    }

    private void TitleBarButton_Unloaded(object sender, RoutedEventArgs e)
    {
        DependencyPropertyDescriptor
            .FromProperty(ButtonsForegroundProperty, typeof(Brush))
            .RemoveValueChanged(this, OnButtonsForegroundChanged);
    }

    private void TitleBarButton_Loaded(object sender, RoutedEventArgs e)
    {
        SetCurrentValue(RenderButtonsForegroundProperty, ButtonsForeground);
        DependencyPropertyDescriptor
            .FromProperty(ButtonsForegroundProperty, typeof(Brush))
            .AddValueChanged(this, OnButtonsForegroundChanged);
    }

    private void OnButtonsForegroundChanged(object? sender, EventArgs e)
    {
        SetCurrentValue(
            RenderButtonsForegroundProperty,
            IsHovered ? MouseOverButtonsForeground : ButtonsForeground
        );
    }

    /// <summary>
    /// Forces button background to change.
    /// </summary>
    public void Hover()
    {
        if (IsHovered)
        {
            return;
        }

        SetCurrentValue(BackgroundProperty, MouseOverBackground);
        if (MouseOverButtonsForeground != null)
        {
            SetCurrentValue(RenderButtonsForegroundProperty, MouseOverButtonsForeground);
        }

        IsHovered = true;
    }

    /// <summary>
    /// Forces button background to change.
    /// </summary>
    public void RemoveHover()
    {
        if (!IsHovered)
        {
            return;
        }

        SetCurrentValue(BackgroundProperty, _defaultBackgroundBrush);
        SetCurrentValue(RenderButtonsForegroundProperty, ButtonsForeground);

        IsHovered = false;
        _isClickedDown = false;
    }

    /// <summary>
    /// Invokes click on the button.
    /// </summary>
    public void InvokeClick()
    {
        if (
            new ButtonAutomationPeer(this).GetPattern(PatternInterface.Invoke)
            is IInvokeProvider invokeProvider
        )
        {
            invokeProvider.Invoke();
        }

        _isClickedDown = false;
    }

    internal bool ReactToHwndHook(uint msg, IntPtr lParam, out IntPtr returnIntPtr)
    {
        returnIntPtr = IntPtr.Zero;

        switch (msg)
        {
            case PInvoke.WM_NCHITTEST:
                if (IsMouseOverNonClient(this, lParam))
                {
                    /*Debug.WriteLine($"Hitting {ButtonType} | return code {_returnValue}");*/
                    Hover();
                    returnIntPtr = (IntPtr)_returnValue;
                    return true;
                }

                RemoveHover();
                return false;
            case PInvoke.WM_NCMOUSELEAVE: // Mouse leaves the window
                RemoveHover();
                return false;
            case PInvoke.WM_NCLBUTTONDOWN when IsMouseOverNonClient(this, lParam): // Left button clicked down
                _isClickedDown = true;
                return true;
            case PInvoke.WM_NCLBUTTONUP when _isClickedDown && IsMouseOverNonClient(this, lParam): // Left button clicked up
                InvokeClick();
                return true;
            default:
                return false;
        }
    }

    private static void OnButtonTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not TitleBarButton titleBarButton)
        {
            return;
        }

        titleBarButton.OnButtonTypeChanged(e);
    }

    protected void OnButtonTypeChanged(DependencyPropertyChangedEventArgs e)
    {
        var buttonType = (TitleBarButtonType)e.NewValue;

        _returnValue = buttonType switch
        {
            TitleBarButtonType.Unknown => PInvoke.HTNOWHERE,
            TitleBarButtonType.Help => PInvoke.HTHELP,
            TitleBarButtonType.Minimize => PInvoke.HTMINBUTTON,
            TitleBarButtonType.Close => PInvoke.HTCLOSE,
            TitleBarButtonType.Restore => PInvoke.HTMAXBUTTON,
            TitleBarButtonType.Maximize => PInvoke.HTMAXBUTTON,
            _ => throw new ArgumentOutOfRangeException(
                "e.NewValue",
                buttonType,
                $"Unsupported button type: {buttonType}."
            ),
        };
    }

    // TODO: Incorrectly calculates mouse position for high DPI displays.
    // PresentationSource presentationSource = null;
    // protected bool IsMouseOverElement(nint lParam)
    // {
    //    System.Drawing.Point winPoint;
    //    bool gotCursorPos = User32.GetCursorPos(out winPoint);

    //    if (!gotCursorPos)
    //    {
    //        int fallbackX = unchecked((short)((long)lParam & 0xFFFF));
    //        int fallbackY = unchecked((short)(((long)lParam >> 16) & 0xFFFF));
    //        winPoint = new System.Drawing.Point(fallbackX, fallbackY);
    //    }

    //    var screenPoint = new System.Windows.Point(winPoint.X, winPoint.Y);

    //    presentationSource ??= PresentationSource.FromVisual(this);

    //    if (presentationSource?.CompositionTarget != null)
    //    {
    //        screenPoint = presentationSource.CompositionTarget.TransformFromDevice.Transform(screenPoint);
    //    }

    //    var localPoint = this.PointFromScreen(screenPoint);

    //    var hitTestRect = new System.Windows.Rect(0, 0, this.ActualWidth, this.ActualHeight);

    //    return hitTestRect.Contains(localPoint);
    //}
}
