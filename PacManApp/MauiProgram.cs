using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using PacManApp.ViewModels;
using PacManApp.Views;
using Plugin.Maui.Audio;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace PacManApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseSkiaSharp()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton(AudioManager.Current);
        builder.Services.AddSingleton<GameAudioViewModel>();
        builder.Services.AddTransient<GamePage>();
        builder.Services.AddTransient<GameViewModel>();

        return builder.Build();
	}
}

