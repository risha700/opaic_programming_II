
using BallBreaker.ViewModels;

namespace BallBreaker.Views;

public class MainPage : ContentPage
{

	

	public MainPage(GameViewModel GameVm)
	{


		

		Content = new VerticalStackLayout
		{
			Children = {
				new Label { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Text = "Welcome to .NET MAUI!",
                
                },
                GameVm.ActiveGame.GameCanvasView,
			}
		};
	}
}
