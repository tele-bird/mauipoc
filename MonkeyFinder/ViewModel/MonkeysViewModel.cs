using MonkeyFinder.Services;
using CommunityToolkit.Mvvm.Input;
using MonkeyFinder.View;
using AsyncAwaitBestPractices;

namespace MonkeyFinder.ViewModel;

public partial class MonkeysViewModel : BaseViewModel
{
    private readonly IMonkeyService monkeyService;
    private readonly IGeolocation geolocation;
    private readonly IUserPreferences userPreferences;

    public ObservableCollection<Monkey> Monkeys { get; } = new();
    //public ObservableCollection<int> PossibleItemsPerRow { get; } = new() { 1, 2, 3, 4, 5 };

    [ObservableProperty]
    int gridItemsLayoutSpan;

    [ObservableProperty]
    bool isRefreshing;

    public MonkeysViewModel(
        IMonkeyService monkeyService,
        IGeolocation geolocation,
        IUserPreferences userPreferences)
    {
        Title = "Monkey Finder";
        this.monkeyService = monkeyService;
        this.geolocation = geolocation;
        this.userPreferences = userPreferences;
        GridItemsLayoutSpan = this.userPreferences.GridItemsLayoutSpan;
    }

    protected override async Task OnFirstAppearing()
    {
        IsBusy = true;
        await base.OnFirstAppearing().ConfigureAwait(ConfigureAwaitOptions.ContinueOnCapturedContext);
        RefreshMonkeys(CancellationToken.None).SafeFireAndForget(HandleFireAndForgetException);
    }

    [RelayCommand]
    async Task RefreshMonkeys(CancellationToken cancellationToken)
    {

        // enforce a minimum duration so that the user can tell that the refresh actually happened,
        // even in the case where code execution is extremely fast
        Task minimumRefreshTimeTask = Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);

        List<Monkey> monkeys = null;

        try
        {
            // use ContinueOnCapturedContext (a .NET 8 addition which is the same as passing false)
            // to return execution of the remainder of this method to the calling thread after Task
            // completion - this will ensure that the modification of the UI-bound properties
            // (Monkeys, IsBusy, and IsRefreshing) will update properly
            monkeys = await monkeyService.GetMonkeys(cancellationToken)
                .ConfigureAwait(ConfigureAwaitOptions.ContinueOnCapturedContext);
        }
        finally
        {
            await minimumRefreshTimeTask.ConfigureAwait(ConfigureAwaitOptions.ContinueOnCapturedContext);
            if (monkeys != null)
            {
                // update UI with results
                Monkeys.Clear();
                foreach (var monkey in monkeys) Monkeys.Add(monkey);
            }
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
            }).ConfigureAwait(ConfigureAwaitOptions.ContinueOnCapturedContext);

            // Find closest monkey to us
            var closestMonkey = Monkeys.OrderBy(m => location.CalculateDistance(new Location(m.Latitude, m.Longitude), DistanceUnits.Miles)).FirstOrDefault();

            if(closestMonkey == null)
            {
                await Shell.Current.DisplayAlert(string.Empty, "No Monkeys found.", "OK");
            }
            else
            {
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
