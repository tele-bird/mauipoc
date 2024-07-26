namespace MonkeyFinder.View;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class MainPage : ContentPage
{
	public MainPage(MonkeysViewModel monkeysViewModel)
	{
		InitializeComponent();
		BindingContext = monkeysViewModel;
	}
}

