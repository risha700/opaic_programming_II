using CommunityToolkit.Mvvm.Input;
using PacManApp.ViewModels;

namespace PacManApp.Views;

public class GamePage : ContentPage
{
    Grid gridContainer = new Grid
    {

        VerticalOptions = LayoutOptions.Start,

        HeightRequest = Shell.Current.Window.Height,
        WidthRequest = Shell.Current.Window.Width,
        ColumnSpacing = 0,
        RowSpacing = 0,
        RowDefinitions = {
                new RowDefinition {Height = new GridLength(0.85, GridUnitType.Star)}, // canvas
                new RowDefinition {Height = new GridLength(0.15, GridUnitType.Star)}, // result box
            },
        ColumnDefinitions = {
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                },
    };
    Frame resultBox = new Frame
    {
        Content = new FlexLayout
        {
            BackgroundColor = Colors.Bisque,
            JustifyContent = Microsoft.Maui.Layouts.FlexJustify.SpaceAround,
            AlignContent = Microsoft.Maui.Layouts.FlexAlignContent.Center,
        },
    };


    public GamePage(GameViewModel GameVm)
	{
		
        gridContainer.Add(resultBox, 0, 1);
        gridContainer.SetRowSpan(resultBox, 1);
        gridContainer.Add(GameVm.ActiveGame.GameCanvasView, 0, 0);
        gridContainer.SetRowSpan(GameVm.ActiveGame.GameCanvasView, 1);
        Content = gridContainer;
    }
}
