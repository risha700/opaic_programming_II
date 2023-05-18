using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using SlotGame.Models;
using SlotGame.ViewModels;
using SlotGame;

namespace SlotGame;

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
        builder.Services.AddSingleton<Spinner>();
        builder.Services.AddSingleton<SpinnerViewModel>();
        return builder.Build();
	}
}

