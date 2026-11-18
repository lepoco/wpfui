// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Input;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <content>
/// Handles optional pointer input on unused pane backgrounds.
/// </content>
public partial class NavigationView
{
    private DependencyObject? _paneToggleSpace;
    private DependencyObject? _paneMenuSpace;
    private DependencyObject? _pressedPaneSpace;
    private Point _panePressPosition;

    private bool CanTogglePaneFromEmptySpace =>
        IsPaneToggleOnEmptySpaceEnabled
        && PaneDisplayMode is NavigationViewPaneDisplayMode.Left or NavigationViewPaneDisplayMode.LeftMinimal;

    private bool IsPaneEmptySpace(object source) =>
        source is DependencyObject element
        && (ReferenceEquals(element, _paneToggleSpace) || ReferenceEquals(element, _paneMenuSpace));

    /// <inheritdoc />
    protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
    {
        _pressedPaneSpace = null;
        base.OnPreviewMouseDown(e);

        if (
            !e.Handled
            && e.ChangedButton == MouseButton.Left
            && e.ClickCount == 1
            && CanTogglePaneFromEmptySpace
            && IsPaneEmptySpace(e.OriginalSource)
        )
        {
            _pressedPaneSpace = (DependencyObject)e.OriginalSource;
            _panePressPosition = e.GetPosition(this);
        }
    }

    /// <inheritdoc />
    protected override void OnPreviewMouseMove(MouseEventArgs e)
    {
        base.OnPreviewMouseMove(e);

        if (_pressedPaneSpace is null)
        {
            return;
        }

        Point position = e.GetPosition(this);
        if (
            e.LeftButton != MouseButtonState.Pressed
            || Math.Abs(position.X - _panePressPosition.X) >= SystemParameters.MinimumHorizontalDragDistance
            || Math.Abs(position.Y - _panePressPosition.Y) >= SystemParameters.MinimumVerticalDragDistance
        )
        {
            _pressedPaneSpace = null;
        }
    }

    /// <inheritdoc />
    protected override void OnMouseUp(MouseButtonEventArgs e)
    {
        base.OnMouseUp(e);

        DependencyObject? pressedSpace = _pressedPaneSpace;
        _pressedPaneSpace = null;
        Point position = e.GetPosition(this);

        // Only a press and release on the same template background is a pane click.
        // Child controls, scroll bars and application content never qualify.
        if (
            !e.Handled
            && e.ChangedButton == MouseButton.Left
            && pressedSpace is not null
            && ReferenceEquals(pressedSpace, e.OriginalSource)
            && IsPaneEmptySpace(e.OriginalSource)
            && CanTogglePaneFromEmptySpace
            && Math.Abs(position.X - _panePressPosition.X) < SystemParameters.MinimumHorizontalDragDistance
            && Math.Abs(position.Y - _panePressPosition.Y) < SystemParameters.MinimumVerticalDragDistance
        )
        {
            OnToggleButtonClick(this, e);
            e.Handled = true;
        }
    }

    /// <inheritdoc />
    protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
    {
        _pressedPaneSpace = null;
        base.OnPreviewMouseWheel(e);
    }

    /// <inheritdoc />
    protected override void OnMouseLeave(MouseEventArgs e)
    {
        _pressedPaneSpace = null;
        base.OnMouseLeave(e);
    }
}
