#nullable enable
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Wpf.Ui.Animations;
using Wpf.Ui.Common.Interfaces;

namespace Wpf.Ui.Services.Internal;

internal sealed class FrameManager
{
    private readonly Frame _frame;
    private readonly int _transitionDuration;
    private readonly TransitionType _transitionType;

    public FrameManager(Frame frame, int transitionDuration, TransitionType transitionType)
    {
        _frame = frame;
        _transitionDuration = transitionDuration;
        _transitionType = transitionType;

        _frame.NavigationUIVisibility = NavigationUIVisibility.Hidden;

        _frame.Navigating += OnFrameNavigating;
        _frame.Navigated += OnFrameNavigated;
    }

    
    private void OnFrameNavigating(object sender, NavigatingCancelEventArgs e)
    {
        NotifyFrameContentAboutLeave();
    }

    private void OnFrameNavigated(object sender, NavigationEventArgs e)
    {
        _frame.NavigationService.RemoveBackEntry();

        if (_transitionDuration > 0 && e.Content != null)
            Transitions.ApplyTransition(e.Content, _transitionType, _transitionDuration);

        //TODO
        /*// Finally, the navigation took place internally,
        // the context was set from extra data, the cache has to be saved,
        // so we save it, notify it and this is the end of the method
        _navigationServiceItems[extraData.PageId].Instance = _frame.Content;*/

        NotifyFrameContentAboutEnter();
    }

    private void NotifyFrameContentAboutEnter()
    {
        var navigationAware = GetINavigationAwareOrDefault();
        navigationAware?.OnNavigatedTo();
    }

    private void NotifyFrameContentAboutLeave()
    {
        var navigationAware = GetINavigationAwareOrDefault();
        navigationAware?.OnNavigatedFrom();
    }

    private INavigationAware? GetINavigationAwareOrDefault()
    {
        INavigationAware? navigationAware = _frame.Content switch
        {
            INavigationAware aware => aware,
            INavigableView<object> {ViewModel: INavigationAware viewModelNavigationAware} => viewModelNavigationAware,
            FrameworkElement  {DataContext: INavigationAware dataContextNavigationAware} => dataContextNavigationAware,
            _ => null
        };

        return navigationAware;
    }
}
