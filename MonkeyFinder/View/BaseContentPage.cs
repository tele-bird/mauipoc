using CommunityToolkit.Maui.Behaviors;

namespace MonkeyFinder.View;

public partial class BaseContentPage : ContentPage
{
    protected BaseViewModel BaseViewModel => (BaseViewModel)BindingContext;

    public BaseContentPage(BaseViewModel baseViewModel)
    {
        BindingContext = baseViewModel;
        Behaviors.Add(new EventToCommandBehavior
        {
            EventName = nameof(Appearing),
            Command = BaseViewModel.AppearingCommand
        });
        Behaviors.Add(new EventToCommandBehavior
        {
            EventName = nameof(Disappearing),
            Command = BaseViewModel.DisappearingCommand
        });
        Behaviors.Add(new EventToCommandBehavior
        {
            EventName = nameof(NavigatingFrom),
            Command = BaseViewModel.NavigatingFromCommand
        });
        Behaviors.Add(new EventToCommandBehavior
        {
            EventName = nameof(NavigatedFrom),
            Command = BaseViewModel.NavigatedFromCommand
        });
        Behaviors.Add(new EventToCommandBehavior
        {
            EventName = nameof(NavigatedTo),
            Command = BaseViewModel.NavigatedToCommand
        });
    }
}
