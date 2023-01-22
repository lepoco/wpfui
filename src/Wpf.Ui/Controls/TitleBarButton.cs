using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Interop;
using System.Windows.Media;
using Wpf.Ui.Appearance;
using Wpf.Ui.Interop;
using Wpf.Ui.TitleBar;

namespace Wpf.Ui.Controls;

public class TitleBarButton : Wpf.Ui.Controls.Button
{
    public static readonly DependencyProperty ButtonTypeProperty = DependencyProperty.Register(nameof(ButtonType),
        typeof(TitleBarButtonType), typeof(TitleBarButton), new PropertyMetadata(TitleBarButtonType.Unknown));

    /// <summary>
    /// Property for <see cref="ButtonsForeground"/>.
    /// </summary>
    public static readonly DependencyProperty ButtonsForegroundProperty = DependencyProperty.Register(
        nameof(ButtonsForeground),
        typeof(Brush), typeof(TitleBarButton), new FrameworkPropertyMetadata(SystemColors.ControlTextBrush,
            FrameworkPropertyMetadataOptions.Inherits));

    public TitleBarButtonType ButtonType
    {
        get => (TitleBarButtonType)GetValue(ButtonTypeProperty);
        set => SetValue(ButtonTypeProperty, value);
    }
    
    /// <summary>
    /// Foreground of the navigation buttons.
    /// </summary>
    public Brush ButtonsForeground
    {
        get => (Brush)GetValue(ButtonsForegroundProperty);
        set => SetValue(ButtonsForegroundProperty, value);
    }

    public bool IsHovered { get; private set; }

    /*private static SolidColorBrush _hoverColorLight = new SolidColorBrush(Color.FromArgb(
        (byte)0x1A,
        (byte)0x00,
        (byte)0x00,
        (byte)0x00));

    private static SolidColorBrush _hoverColorDark = new SolidColorBrush(Color.FromArgb(
        (byte)0x17,
        (byte)0xFF,
        (byte)0xFF,
        (byte)0xFF));*/

    private User32.WM_NCHITTEST _returnValue;
    private Brush _defaultBackgroundBrush = null!;

    private bool _isClickedDown;

    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);

        _defaultBackgroundBrush = Background;

        if (ButtonType == TitleBarButtonType.Unknown)
            return;

        _returnValue = ButtonType switch
        {
            TitleBarButtonType.Help => User32.WM_NCHITTEST.HTHELP,
            TitleBarButtonType.Minimize => User32.WM_NCHITTEST.HTMINBUTTON,
            TitleBarButtonType.Close => User32.WM_NCHITTEST.HTCLOSE,
            TitleBarButtonType.Restore => User32.WM_NCHITTEST.HTMAXBUTTON,
            TitleBarButtonType.Maximize => User32.WM_NCHITTEST.HTMAXBUTTON,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    internal bool ReactToHwndHook(User32.WM msg, IntPtr lParam, out IntPtr returnIntPtr)
    {
        returnIntPtr = IntPtr.Zero;

        switch (msg)
        {
            case User32.WM.MOVE:
                // Adjust [Size] of the buttons if the DPI is changed
                break;

            // Hit test, for determining whether the mouse cursor is over one of the buttons
            case User32.WM.NCHITTEST:
                if (IsMouseOverElement(lParam))
                {
                    Hover();

                    returnIntPtr = (IntPtr)_returnValue;
                    return true;
                }

                RemoveHover();
                break;

            // Mouse leaves the window
            case User32.WM.NCMOUSELEAVE:
                RemoveHover();
                break;

            // Left button clicked down
            case User32.WM.NCLBUTTONDOWN:
                if (IsMouseOverElement(lParam))
                {
                    _isClickedDown = true;
                    return true;
                }
                break;

            // Left button clicked up
            case User32.WM.NCLBUTTONUP:
                if (_isClickedDown && IsMouseOverElement(lParam))
                {
                    InvokeClick();
                    return true;
                }
                break;
        }

        return false;
    }

    /// <summary>
    /// Do not call it outside of NCHITTEST message!
    /// </summary>
    private bool IsMouseOverElement(IntPtr lParam)
    {
        // This method will be invoked very often and must be as simple as possible.

        if (lParam == IntPtr.Zero)
            return false;

        var mousePosScreen = new Point(Get_X_LParam(lParam), Get_Y_LParam(lParam));
        var bounds = new Rect(new Point(), RenderSize);
        var mousePosRelative = PointFromScreen(mousePosScreen);
        return bounds.Contains(mousePosRelative);
    }

    /// <summary>
    /// Invokes click on the button.
    /// </summary>
    private void InvokeClick()
    {
        if (new ButtonAutomationPeer(this).GetPattern(PatternInterface.Invoke) is IInvokeProvider invokeProvider)
            invokeProvider.Invoke();

        _isClickedDown = false;
    }

    /// <summary>
    /// Forces button background to change.
    /// </summary>
    private void Hover()
    {
        if (IsHovered)
            return;

        Background = MouseOverBackground;
        IsHovered = true;
    }

    /// <summary>
    /// Forces button background to change.
    /// </summary>
    public void RemoveHover()
    {
        if (!IsHovered)
            return;

        Background = _defaultBackgroundBrush;

        IsHovered = false;
        _isClickedDown = false;
    }

    private static int Get_X_LParam(IntPtr lParam)
    {
        return (short)(lParam.ToInt32() & 0xFFFF);
    }

    private static int Get_Y_LParam(IntPtr lParam)
    {
        return (short)(lParam.ToInt32() >> 16);
    }
}
