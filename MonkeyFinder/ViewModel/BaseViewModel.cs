using AsyncAwaitBestPractices;

namespace MonkeyFinder.ViewModel;

public abstract partial class BaseViewModel : ObservableObject
{
    private bool firstAppeared = false;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    bool isBusy;

    [ObservableProperty]
    string title;

    public bool IsNotBusy => !IsBusy;

    [RelayCommand]
    protected virtual async Task OnAppearing()
    {
        Trace.WriteLine($"{GetType().Name}.{nameof(OnAppearing)} >>");
        if(!firstAppeared)
        {
            await OnFirstAppearing();
            firstAppeared = true;
        }
    }

    protected virtual async Task OnFirstAppearing()
    {
        Trace.WriteLine($"{GetType().Name}.{nameof(OnFirstAppearing)} >>");
    }

    [RelayCommand]
    protected virtual async Task OnDisappearing()
    {
        Trace.WriteLine($"{GetType().Name}.{nameof(OnDisappearing)} >>");
    }

    [RelayCommand]
    protected virtual async Task OnNavigatingFrom()
    {
        Trace.WriteLine($"{GetType().Name}.{nameof(OnNavigatingFrom)} >>");
    }

    [RelayCommand]
    protected virtual async Task OnNavigatedFrom()
    {
        Trace.WriteLine($"{GetType().Name}.{nameof(OnNavigatedFrom)} >>");
    }

    [RelayCommand]
    protected virtual async Task OnNavigatedTo()
    {
        Trace.WriteLine($"{GetType().Name}.{nameof(OnNavigatedTo)} >>");
    }



    /// <summary>
    /// A handler for exceptions thrown by the async Task upon which SafeFireAndForget() was being invoked.
    /// Note that this handler will be invoked on a background thread, so any UI activities must be explicitly invoked on the Main thread.
    /// </summary>
    /// <param name="exc">The Exception thrown by the Task</param>
    protected void HandleFireAndForgetException(Exception exc)
    {
        Trace.WriteLine(exc);
        //MainThread.BeginInvokeOnMainThread(() =>
        //{
        //    Application.Current.MainPage.DisplayAlert(
        //        "SafeFireAndForget Exception",
        //        exc.Message,
        //        "OK").SafeFireAndForget();
        //});
    }
}
