namespace MonkeyFinder.View;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class DetailsPage : ContentPage
{
	public DetailsPage(MonkeyDetailsViewModel monkeyDetailsViewModel)
	{
		InitializeComponent();
		BindingContext = monkeyDetailsViewModel;
	}
}