using System;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Media;
using Wpf.Ui.Extensions;
using Wpf.Ui.Interop;

namespace Wpf.Ui.Controls.TitleBarControl;

internal class TitleBarButton : Wpf.Ui.Controls.Button
{

    /// <summary>
    /// Property for <see cref="ButtonType"/>.
    /// </summary>
    public static readonly DependencyProperty ButtonTypeProperty = DependencyProperty.Register(nameof(ButtonType),
        typeof(TitleBarButtonType), typeof(TitleBarButton), new PropertyMetadata(TitleBarButtonType.Unknown, ButtonTypePropertyCallback));

    /// <summary>
    /// Property for <see cref="ButtonsForeground"/>.
    /// </summary>
    public static readonly DependencyProperty ButtonsForegroundProperty = DependencyProperty.Register(
        nameof(ButtonsForeground),
        typeof(Brush), typeof(TitleBarButton), new FrameworkPropertyMetadata(SystemColors.ControlTextBrush,
            FrameworkPropertyMetadataOptions.Inherits));

    /// <summary>
    /// Sets or gets the 
    /// </summary>
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
    private Brush _defaultBackgroundBrush = Brushes.Transparent; //Should it be transparent?

    private bool _isClickedDown;

    /// <summary>
    /// Forces button background to change.
    /// </summary>
    public void Hover()
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

    /// <summary>
    /// Invokes click on the button.
    /// </summary>
    public void InvokeClick()
    {
        if (new ButtonAutomationPeer(this).GetPattern(PatternInterface.Invoke) is IInvokeProvider invokeProvider)
            invokeProvider.Invoke();

        _isClickedDown = false;
    }

    internal bool ReactToHwndHook(User32.WM msg, IntPtr lParam, out IntPtr returnIntPtr)
    {
        returnIntPtr = IntPtr.Zero;

        switch (msg)
        {
            case User32.WM.NCHITTEST:
                if (this.IsMouseOverElement(lParam))
                {
                    //Debug.WriteLine($"Hitting {ButtonType} | return code {_returnValue}");

                    Hover();
                    returnIntPtr = (IntPtr)_returnValue;
                    return true;
                }

                RemoveHover();
                return false;

            case User32.WM.NCMOUSELEAVE: // Mouse leaves the window
                RemoveHover();
                return false;
            case User32.WM.NCLBUTTONDOWN when this.IsMouseOverElement(lParam): // Left button clicked down
                _isClickedDown = true;
                return true;
            case User32.WM.NCLBUTTONUP when _isClickedDown && this.IsMouseOverElement(lParam): // Left button clicked up
                InvokeClick();
                return true;
            default:
                return false;
        }
    }

    private void UpdateReturnValue(TitleBarButtonType buttonType) =>
        _returnValue = buttonType switch
        {
            TitleBarButtonType.Unknown => User32.WM_NCHITTEST.HTNOWHERE,
            TitleBarButtonType.Help => User32.WM_NCHITTEST.HTHELP,
            TitleBarButtonType.Minimize => User32.WM_NCHITTEST.HTMINBUTTON,
            TitleBarButtonType.Close => User32.WM_NCHITTEST.HTCLOSE,
            TitleBarButtonType.Restore => User32.WM_NCHITTEST.HTMAXBUTTON,
            TitleBarButtonType.Maximize => User32.WM_NCHITTEST.HTMAXBUTTON,
            _ => throw new ArgumentOutOfRangeException(nameof(buttonType), buttonType, null)
        };

    private static void ButtonTypePropertyCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var titleBarButton = (TitleBarButton)d;
        titleBarButton.UpdateReturnValue((TitleBarButtonType)e.NewValue);
    }
}
