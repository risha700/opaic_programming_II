using Microsoft.Maui.Controls;
using BallBreaker.ViewModels;
using Microsoft.Maui.Handlers;

namespace BallBreaker.Views;

public class MainPage : ContentPage
{

	public ContentView GameContentView=new();

    

    //private void OnKeyPressEvt(object sender, Android.Views.View.KeyEventArgs e)
    //{
    //    throw new NotImplementedException();
    //}

    public MainPage(GameViewModel GameVm)
	{
        //EntryHandler.Mapper.AppendToMapping("MyCustomization", (handler, view) => {

        //    });

        //v.ContainerView.key
        


        //if (nativeView != null)
        //{
        //    nativeView.KeyDown += this.PlatformView_KeyDown;
        //    nativeView.KeyUp += this.PlatformView_KeyUp;
        //    nativeView.PreviewKeyDown += this.PlatformView_PreviewKeyDown;
        //}



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
