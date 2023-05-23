using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using RaceGame.Models;
using RaceGame.ViewModels;
using RaceGame.Views;

namespace RaceGame;

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
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif
		builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<Coordinates>();
        builder.Services.AddTransient<MainViewModel>();
        return builder.Build();
	}
}

