using Microsoft.Maui.Controls;
using BallBreaker.ViewModels;
using Microsoft.Maui.Handlers;


namespace BallBreaker.Views;

public class MainPage : ContentPage
{

	

    public MainPage(GameViewModel GameVm)
	{
		var scores = new Label { TextType=TextType.Text, FontSize=20, FontAttributes=FontAttributes.Bold};
        var timer = new Label { TextType = TextType.Text, FontSize = 20, FontAttributes = FontAttributes.Bold };
        scores.SetBinding(Label.TextProperty, new Binding(path: "Score", source: GameVm.ActiveGame.GamePlayer, mode: BindingMode.Default, stringFormat: "Your Score: {0}"));
		timer.SetBinding(Label.TextProperty, new Binding(path: "RemainingTime", source: GameVm.ActiveGame, mode: BindingMode.Default, stringFormat: "Remaining time: {0:mm\\:ss\\:ff}"));
		var gridContainer = new Grid
		{

			VerticalOptions = LayoutOptions.Start,
			
			HeightRequest = Shell.Current.Window.Height,
			ColumnSpacing = 0,
			RowSpacing = 0,
			RowDefinitions = {
				new RowDefinition {Height = new GridLength(0.1, GridUnitType.Star)}, // result box
                new RowDefinition {Height = new GridLength(0.9, GridUnitType.Star)} // canvas
            },
			ColumnDefinitions = {
				new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
				},
		};

		var resultBox = new Frame
		{
			//HeightRequest = 100,
			//VerticalOptions = LayoutOptions.End,
			Content = new FlexLayout
			{
				JustifyContent = Microsoft.Maui.Layouts.FlexJustify.SpaceAround,
				AlignContent = Microsoft.Maui.Layouts.FlexAlignContent.Center,
				Children =
						{
                            scores,
							timer
						}
			},
		};
		gridContainer.Add(resultBox, 0, 0);
		gridContainer.SetRowSpan(resultBox, 1);
		gridContainer.Add(GameVm.ActiveGame.GameCanvasView, 0,1);
        gridContainer.SetRowSpan(GameVm.ActiveGame.GameCanvasView, 1);
        Content = gridContainer;

	}

}
