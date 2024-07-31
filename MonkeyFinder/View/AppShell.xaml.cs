namespace MonkeyFinder.View;

public partial class AppShell : Shell
{
	public AppShell(AppShellViewModel appShellViewModel)
	{
		InitializeComponent();
		BindingContext = appShellViewModel;
		Routing.RegisterRoute(nameof(DetailsPage), typeof(DetailsPage));
	}
}