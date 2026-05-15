// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Drawing;
using Wpf.Ui.Tray;
using Wpf.Ui.Tray.Controls;

namespace Wpf.Ui.Gallery.ViewModels.Pages.OpSystem;

public partial class NotifyIconViewModel : ViewModel
{
    private readonly NotifyIcon _notifyIcon;

    public NotifyIconViewModel(INotifyIconService notifyIconService)
    {
        _notifyIcon = notifyIconService.GetNotifyIcon() ?? throw new InvalidOperationException("NotifyIcon was never set.");

        TooltipText = _notifyIcon.TooltipText;

        _notifyIcon.BalloonTipShown += NotifyIcon_BalloonTipShown;
        _notifyIcon.BalloonTipClose += NotifyIcon_BalloonTipClose;
        _notifyIcon.BalloonTipClick += NotifyIcon_BalloonTipClick;
    }

    private void NotifyIcon_BalloonTipShown([System.Diagnostics.CodeAnalysis.NotNull] NotifyIcon sender, RoutedEventArgs e)
    {
        BalloonTipStatus = "Shown";
    }

    private void NotifyIcon_BalloonTipClose([System.Diagnostics.CodeAnalysis.NotNull] NotifyIcon sender, RoutedEventArgs e)
    {
        BalloonTipStatus = "Closed";
    }

    private void NotifyIcon_BalloonTipClick([System.Diagnostics.CodeAnalysis.NotNull] NotifyIcon sender, RoutedEventArgs e)
    {
        BalloonTipStatus = "Clicked";
    }

    [ObservableProperty]
    private string _balloonTipTitle = "Hello!";

    [ObservableProperty]
    private string _balloonTipMessage = "This is a balloon tip notification.";

    [ObservableProperty]
    private string _tooltipText = string.Empty;

    [ObservableProperty]
    private string _balloonTipStatus = "No notification sent yet.";

    [ObservableProperty]
    private ToolTipIcon _selectedBalloonTipIcon = ToolTipIcon.Info;

    [ObservableProperty]
    private IEnumerable<ToolTipIcon> _balloonTipIcons = Enum.GetValues<ToolTipIcon>();

    [RelayCommand]
    private void OnShowBalloonTip()
    {
        // _notifyIcon.SetCurrentValue(NotifyIcon.BalloonTipTitleProperty, BalloonTipTitle);
        // _notifyIcon.SetCurrentValue(NotifyIcon.BalloonTipTextProperty, BalloonTipMessage);
        // _notifyIcon.SetCurrentValue(NotifyIcon.BalloonTipIconProperty, SelectedBalloonTipIcon);
        // _notifyIcon.ShowBalloonTip(TimeSpan.FromSeconds(3));
        _notifyIcon.ShowBalloonTip(
            TimeSpan.FromSeconds(3),
            BalloonTipTitle,
            BalloonTipMessage,
            SelectedBalloonTipIcon
        );
    }

    [RelayCommand]
    private void OnUpdateTooltip()
    {
        _notifyIcon.SetCurrentValue(NotifyIcon.TooltipTextProperty, TooltipText);
    }
}