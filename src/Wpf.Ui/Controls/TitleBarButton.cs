using System;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Media;
using Wpf.Ui.Extensions;
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
            // Hit test, for determining whether the mouse cursor is over one of the buttons
            case User32.WM.NCHITTEST:
                if (this.IsMouseOverElement(lParam))
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
                if (this.IsMouseOverElement(lParam))
                {
                    _isClickedDown = true;
                    return true;
                }
                break;

            // Left button clicked up
            case User32.WM.NCLBUTTONUP:
                if (_isClickedDown && this.IsMouseOverElement(lParam))
                {
                    InvokeClick();
                    return true;
                }
                break;
        }

        return false;
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
}
