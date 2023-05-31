using Microsoft.Maui.Controls;
using BallBreaker.ViewModels;
using Microsoft.Maui.Handlers;


namespace BallBreaker.Views;

public class MainPage : ContentPage
{

	

    public MainPage(GameViewModel GameVm)
	{
		//var hook = new SimpleGlobalHook();
		//hook.KeyPressed += OnKeyDown;
  //      MainThread.BeginInvokeOnMainThread(() =>
  //      {
  //          hook.Run();

  //      });
        Content = new VerticalStackLayout
		{
			Children = {
				new Label { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Text = "Welcome to .NET MAUI!",
                
                },
                GameVm.ActiveGame.GameCanvasView,
			}
		};
	}

  //  private void OnKeyDown(object sender, KeyboardHookEventArgs e)
  //  {
		//Console.WriteLine($"heeey key down!!!!{sender} {e.Data.KeyCode}");
  //  }
}
