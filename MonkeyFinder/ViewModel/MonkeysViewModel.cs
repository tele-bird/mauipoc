using MonkeyFinder.Services;
using CommunityToolkit.Mvvm.Input;
using MonkeyFinder.View;

namespace MonkeyFinder.ViewModel;

public partial class MonkeysViewModel : BaseViewModel
{
    private readonly MonkeyService monkeyService;
    private readonly IGeolocation geolocation;
    private readonly IUserPreferences userPreferences;

    public ObservableCollection<Monkey> Monkeys { get; } = new();
    //public ObservableCollection<int> PossibleItemsPerRow { get; } = new() { 1, 2, 3, 4, 5 };

    [ObservableProperty]
    int gridItemsLayoutSpan;

    [ObservableProperty]
    bool isRefreshing;

    public MonkeysViewModel(
        MonkeyService monkeyService,
        IGeolocation geolocation,
        IUserPreferences userPreferences)
    {
        Title = "Monkey Finder";
        this.monkeyService = monkeyService;
        this.geolocation = geolocation;
        this.userPreferences = userPreferences;
        GridItemsLayoutSpan = this.userPreferences.GridItemsLayoutSpan;
    }

    [RelayCommand]
    async Task RefreshMonkeys()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            var monkeys = await monkeyService.GetMonkeys();
            if (Monkeys.Count != 0) Monkeys.Clear();
            foreach (var monkey in monkeys) Monkeys.Add(monkey);
        }
        catch(Exception exc)
        {
            Debug.WriteLine($"Unable to get monkeys: {exc.Message}");
            await Application.Current.MainPage.DisplayAlert("Error", exc.Message, "OK");
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    async Task GetClosestMonkey()
    {
        if (IsBusy || Monkeys.Count == 0) return;
        try
        {
            IsBusy = true;
            var location = await geolocation.GetLocationAsync(new GeolocationRequest
            {
                DesiredAccuracy = GeolocationAccuracy.Medium,
                Timeout = TimeSpan.FromSeconds(30)
            });

            // Find closest monkey to us
            var closestMonkey = Monkeys.OrderBy(m => location.CalculateDistance(new Location(m.Latitude, m.Longitude), DistanceUnits.Miles)).FirstOrDefault();

            // show results:
            var navigateToMonkey = await Shell.Current.DisplayAlert("Found!", $"{closestMonkey.Name} in {closestMonkey.Location}", "View Monkey", "Cancel");

            // navigate to monkey if desired
            if (navigateToMonkey)
            {
                await Shell.Current.GoToAsync(nameof(DetailsPage), true, new Dictionary<string, object>
                {
                    { "Monkey", closestMonkey }
                });
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to query location: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    async Task ClearMonkeys()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            if (Monkeys.Count != 0) Monkeys.Clear();
        }
        catch (Exception exc)
        {
            Debug.WriteLine($"Unable to clear monkeys: {exc.Message}");
            await Application.Current.MainPage.DisplayAlert("Error", exc.Message, "OK");
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    async Task GoToMonkeyDetail(Monkey monkey)
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            await Shell.Current.GoToAsync(nameof(DetailsPage), true, new Dictionary<string, object>
            {
                { "Monkey", monkey }
            });
        }
        catch (Exception exc)
        {
            Debug.WriteLine($"Unable to navigate to monkey: {exc.Message}");
            await Shell.Current.DisplayAlert("Error!", exc.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
