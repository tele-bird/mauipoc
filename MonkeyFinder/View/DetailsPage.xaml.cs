namespace MonkeyFinder.View;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class DetailsPage : BaseContentPage
{
	public DetailsPage(MonkeyDetailsViewModel monkeyDetailsViewModel)
        : base(monkeyDetailsViewModel)
	{
        InitializeComponent();
    }
}