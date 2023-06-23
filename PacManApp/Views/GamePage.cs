using CommunityToolkit.Mvvm.Input;
using PacManApp.ViewModels;

namespace PacManApp.Views;

public class GamePage : ContentPage
{
    Button startButton = new Button { Text = "Play" };
    Label gameScore = new Label { Text = "Score" };
    Label playerLives = new Label { Text = "Lives" };

    public bool IsTimerRunning {get;set;}
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

    };

    FlexLayout resultContainer = new FlexLayout
        {
            //BackgroundColor = Colors.Bisque,
            JustifyContent = Microsoft.Maui.Layouts.FlexJustify.SpaceEvenly,
            AlignContent = Microsoft.Maui.Layouts.FlexAlignContent.Start,
            AlignItems = Microsoft.Maui.Layouts.FlexAlignItems.Start,
        };
    public GameViewModel CurrentGame { get; set; }


    public GamePage(GameViewModel GameVm)
	{

        CurrentGame = GameVm;
        // create the result box
        resultBox.Content = resultContainer;
        resultContainer.Children.Add(gameScore);
        resultContainer.Children.Add(playerLives);
        resultContainer.Children.Add(startButton);
        gameScore.SetBinding(Label.TextProperty, new Binding("Score", source: CurrentGame.ActiveGame.GamePlayer, stringFormat:"Score \n {0}"));
        playerLives.SetBinding(Label.TextProperty, new Binding("Lives", source: CurrentGame.ActiveGame.GamePlayer, stringFormat: "Lives \n {0}"));

        startButton.Clicked += (s,o) =>
        {
            if (IsTimerRunning)
            {
                //shoud start timer
                CurrentGame.ActiveGame.GameTimer.Stop();
                startButton.Text = "Start";
                IsTimerRunning = false;

            }
            else
            {
                //shoud start timer
                CurrentGame.ActiveGame.GameTimer.Start();
                startButton.Text = "Pause";
                IsTimerRunning = true;
            }

        };


        gridContainer.Add(resultBox, 0, 1);
        gridContainer.SetRowSpan(resultBox, 1);
        gridContainer.Add(GameVm.ActiveGame.GameCanvasView, 0, 0);
        gridContainer.SetRowSpan(GameVm.ActiveGame.GameCanvasView, 1);
        Content = gridContainer;
    }
}
