using Microsoft.Extensions.Logging;
using MonkeyFinder.View;
using MonkeyFinder.Services;
using CommunityToolkit.Maui;

namespace MonkeyFinder;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif
		// platform-independent services
        builder.Services.AddSingleton<IMonkeyService, MonkeyService>();
        builder.Services.AddSingleton<IGeolocation>(Geolocation.Default);
        builder.Services.AddSingleton<IMap>(Map.Default);
		builder.Services.AddSingleton<IUserPreferences, UserPreferences>();

        // views
        builder.Services.AddTransient<AppShell>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<DetailsPage>();

		// view models
		builder.Services.AddTransient<AppShellViewModel>();
        builder.Services.AddTransient<MonkeysViewModel>();
        builder.Services.AddTransient<MonkeyDetailsViewModel>();

        return builder.Build();
	}
}
