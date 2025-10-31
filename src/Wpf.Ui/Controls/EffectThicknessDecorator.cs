// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Wpf.Ui.Controls;

public class EffectThicknessDecorator : Decorator
{
    public static readonly DependencyProperty ThicknessProperty =
        DependencyProperty.Register(nameof(Thickness), typeof(Thickness), typeof(EffectThicknessDecorator), new PropertyMetadata(new Thickness(35), OnThicknessChanged));

    public static readonly DependencyProperty AnimationDelayProperty =
        DependencyProperty.Register(nameof(AnimationDelay), typeof(TimeSpan), typeof(EffectThicknessDecorator), new PropertyMetadata(TimeSpan.Zero));

    public static readonly DependencyProperty AnimationElementProperty =
        DependencyProperty.Register(nameof(AnimationElement), typeof(UIElement), typeof(EffectThicknessDecorator), new PropertyMetadata(default(UIElement)));

    private PopupContainer? _popupContainer;

    public EffectThicknessDecorator()
    {
        SizeChanged += (_, _) => UpdateLayout();
    }

    public TimeSpan AnimationDelay
    {
        get { return (TimeSpan)GetValue(AnimationDelayProperty); }
        set { SetValue(AnimationDelayProperty, value); }
    }

    public UIElement? AnimationElement
    {
        get { return (UIElement?)GetValue(AnimationElementProperty); }
        set { SetValue(AnimationElementProperty, value); }
    }

    /// <summary>
    /// Gets or sets the thickness of the effect around the containing element.
    /// </summary>
    public Thickness Thickness
    {
        get { return (Thickness)GetValue(ThicknessProperty); }
        set { SetValue(ThicknessProperty, value); }
    }

    /// <inheritdoc />
    protected override int VisualChildrenCount => 1;

    /// <inheritdoc />
    protected override void OnVisualParentChanged(DependencyObject oldParent)
    {
        base.OnVisualParentChanged(oldParent);

        if (IsInitialized)
        {
            SetPopupContainer();
        }
    }

    /// <inheritdoc />
    protected override Visual GetVisualChild(int index)
    {
        // Only 1 child...
        return Child;
    }

    /// <inheritdoc />
    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);

        SetPopupContainer();
    }

    private void SetPopupContainer()
    {
        PopupContainer? popupContainer = null;

        switch (VisualParent)
        {
            case ContextMenu contextMenu:
                popupContainer = new PopupContainer(contextMenu);
                break;
            case ToolTip toolTip:
                popupContainer = new PopupContainer(toolTip);
                break;
            default:
                if (GetParentPopup(this) is { } parentPopup)
                {
                    popupContainer = new PopupContainer(parentPopup);
                }

                break;
        }

        if (popupContainer == null || _popupContainer?.FrameworkElement == popupContainer.FrameworkElement)
        {
            return;
        }

        popupContainer.Opened += (_, _) =>
        {
            if (AnimationElement is { Effect: { } effect } animationElement && AnimationDelay.Ticks > 0)
            {
                animationElement.Effect = null;

                Task.Delay(AnimationDelay).ContinueWith(_ => Dispatcher.Invoke(() => animationElement.Effect = effect));
            }
        };

        _popupContainer = popupContainer;
        ApplyMargin();
    }

    private static Popup? GetParentPopup(FrameworkElement element)
    {
        while (true)
        {
            switch (element.Parent)
            {
                case Popup popup:
                    return popup;
                case FrameworkElement frameworkElement:
                    element = frameworkElement;
                    continue;
            }

            if (VisualTreeHelper.GetParent(element) is FrameworkElement parent)
            {
                element = parent;
                continue;
            }

            return null;
        }
    }

    private static void OnThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is EffectThicknessDecorator decorator)
        {
            decorator.ApplyMargin();
        }
    }

    private void ApplyMargin()
    {
        _popupContainer?.SetMargin(Thickness);
    }

    private class PopupContainer
    {
        private readonly ContextMenu? _contextMenu;
        private readonly Popup? _popup;
        private readonly ToolTip? _toolTip;

        public PopupContainer(ContextMenu contextMenu)
        {
            _contextMenu = contextMenu;
            contextMenu.Opened += (sender, args) => Opened?.Invoke(sender, args);
        }

        public PopupContainer(ToolTip toolTip)
        {
            _toolTip = toolTip;
            toolTip.Opened += (sender, args) => Opened?.Invoke(sender, args);
        }

        public PopupContainer(Popup popup)
        {
            _popup = popup;
            popup.Opened += (sender, args) => Opened?.Invoke(sender, args);
        }

        public event EventHandler Opened;

        public FrameworkElement? FrameworkElement => _contextMenu ?? _toolTip ?? _popup?.Child as FrameworkElement;

        public void SetMargin(Thickness margin)
        {
            if (FrameworkElement is { } frameworkElement)
            {
                frameworkElement.Margin = margin;
            }
        }
    }
}
