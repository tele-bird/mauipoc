namespace MonkeyFinder.ViewModel;

[QueryProperty(nameof(Monkey), "Monkey")]
public partial class MonkeyDetailsViewModel : BaseViewModel
{
    [ObservableProperty]
    private Monkey monkey;
    private readonly IMap map;

    public MonkeyDetailsViewModel(IMap map)
    {
        this.map = map;
    }

    [RelayCommand]
    async Task OpenMap()
    {
        try
        {
            await map.OpenAsync(Monkey.Latitude, Monkey.Longitude, new MapLaunchOptions
            {
                Name = Monkey.Name,
                NavigationMode = NavigationMode.None
            });
        }
        catch(Exception exc)
        {
            Debug.WriteLine($"Unable to launch maps: {exc.Message}");
            await Application.Current.MainPage.DisplayAlert("Error, no maps app!", exc.Message, "OK");
        }
    }
}
