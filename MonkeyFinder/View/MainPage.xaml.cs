namespace MonkeyFinder.View;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class MainPage : BaseContentPage
{
	public MainPage(MonkeysViewModel monkeysViewModel)
		: base(monkeysViewModel)
	{
        InitializeComponent();
    }
}

