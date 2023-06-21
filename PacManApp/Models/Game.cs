using CommunityToolkit.Mvvm.ComponentModel;
using PacManApp.GameDrawables;
using PacManApp.ViewModels;

namespace PacManApp.Models;

public partial class Game:ObservableObject
{

    [ObservableProperty]
    public GameAudioViewModel audioModel;

    public GraphicsView GameCanvasView { get; set; } = new();

    CanvasDrawable canvasDrawable = new CanvasDrawable();


    public Game(GameAudioViewModel audioModel)
	{
        GameCanvasView = new GraphicsView {
            HeightRequest = Shell.Current.Window.Height * 0.85,
            WidthRequest = Shell.Current.Window.Width
        };
        

        SetupCanvas();

        AudioModel = audioModel;
    }
    private void SetupCanvas()
    {
        //GameCanvasView.BackgroundColor = Colors.DarkSlateBlue;
        GameCanvasView.Drawable = (IDrawable)canvasDrawable; // set canvas instance
        GameCanvasView.DragInteraction += OnDragAction;

    }

    private void OnDragAction(object sender, TouchEventArgs e)
    {
        //e.Touches

        canvasDrawable.PacMan.Element.X = e.Touches[0].X+canvasDrawable.PacMan.Dimension.Width;
        GameCanvasView.Invalidate();

        //canvasDrawable.PacMan.Position.Y
        //e.Touches?[0].X
        //throw new NotImplementedException();
    }
}


public enum GameLevel
{
    Advanced,
    Intermediate,
    Easy

}