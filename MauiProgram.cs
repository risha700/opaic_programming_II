using BallBreaker.Models;
using BallBreaker.ViewModels;
using BallBreaker.Views;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace BallBreaker;

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
        builder.Services.AddTransient<Game>();
        builder.Services.AddTransient<GameViewModel>();
        builder.Services.AddTransient<Ball>();
        builder.Services.AddTransient<Brick>();
        builder.Services.AddTransient<Bat>();
        return builder.Build();
	}
}
