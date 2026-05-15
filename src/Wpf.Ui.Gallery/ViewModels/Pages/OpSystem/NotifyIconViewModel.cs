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
    private readonly INotifyIconService _notifyIconService;

    public NotifyIconViewModel(INotifyIconService notifyIconService)
    {
        _notifyIconService = notifyIconService;
        TooltipText = _notifyIconService.TooltipText;

        NotifyIcon notifyIcon = _notifyIconService.GetNotifyIcon()
            ?? throw new InvalidOperationException("NotifyIcon was never set.");

        notifyIcon.BalloonTipShown += NotifyIcon_BalloonTipShown;
        notifyIcon.BalloonTipClose += NotifyIcon_BalloonTipClose;
        notifyIcon.BalloonTipClick += NotifyIcon_BalloonTipClick;
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
        NotifyIcon icon = _notifyIconService.GetNotifyIcon()
            ?? throw new InvalidOperationException("NotifyIcon was never set.");

        icon.ShowBalloonTip(
            TimeSpan.FromSeconds(3),
            BalloonTipTitle,
            BalloonTipMessage,
            SelectedBalloonTipIcon
        );
    }

    [RelayCommand]
    private void OnUpdateTooltip()
    {
        NotifyIcon icon = _notifyIconService.GetNotifyIcon()
            ?? throw new InvalidOperationException("NotifyIcon was never set.");

        icon.TooltipText = TooltipText;
    }
}