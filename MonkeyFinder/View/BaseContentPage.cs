namespace MonkeyFinder.View;

public class BaseContentPage : ContentPage
{
    public BaseContentPage(BaseViewModel baseViewModel)
    {
        BindingContext = baseViewModel;
    }
}
