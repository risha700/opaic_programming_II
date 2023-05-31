using System.Timers;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Graphics.Platform;
using System.Linq;
using System.Collections.ObjectModel;
using Microsoft.Maui.Graphics;
using BallBreaker.GameDrawables;
using Microsoft.VisualBasic;


namespace BallBreaker.Models;

public partial class Game:ObservableObject
{
    private const uint GAME_DURATION_MIN=3;
    public GameLevel Level { get; set; }
    public bool IsRunning { get; set; }
    public uint Round { get; set; }
	public Player CurrentPlayer { get; set; }
	public System.Timers.Timer GameTimer { get; set; }
    public RectF CanvasDirtyRect { get; set; }
    public GraphicsView GameCanvasView { get; set; } = new();

    CanvasDrawable canvasDrawable = new CanvasDrawable();

    


    public Game(GameLevel level=GameLevel.Easy)
    {


        // setup timer
        setupTimers();

        // assign level
        Level = level;
        // create canvas view
        GameCanvasView = new GraphicsView { HeightRequest = Shell.Current.Window.Height, WidthRequest = Shell.Current.Window.Width };
        
        //GameCanvasView.HandlerChanged += new EventHandler((object sender, EventArgs e) => { Console.WriteLine($"event called {e}"); });
        SetupCanvas(); // should move to diffrent convern content page or viewmodel

        



    }

    private void setupTimers()
    {
        GameTimer = new(TimeSpan.FromMinutes(GAME_DURATION_MIN));
        GameTimer.Interval =50;
        GameTimer.Elapsed += new ElapsedEventHandler(OnGameTimerElapsed);
        GameTimer.Start();

    }


    private void DetectCollision()
    {
        
        switch (canvasDrawable.GameBall.Direction)
        {
            case (BallDirection.DownRight):
                canvasDrawable.GameBall.Position.Y += canvasDrawable.GameBall.Speed;
                canvasDrawable.GameBall.Position.X += canvasDrawable.GameBall.Speed / 2;
                DetectSideCollision();
                break;
            case (BallDirection.DownLeft):
                canvasDrawable.GameBall.Position.Y += canvasDrawable.GameBall.Speed;
                canvasDrawable.GameBall.Position.X -= canvasDrawable.GameBall.Speed / 2;
                DetectSideCollision();
                break;
            case (BallDirection.UpLeft):
                canvasDrawable.GameBall.Position.X -= canvasDrawable.GameBall.Speed / 2;
                canvasDrawable.GameBall.Position.Y -= canvasDrawable.GameBall.Speed;
                DetectSideCollision();
                break;
            case (BallDirection.UpRight):

                canvasDrawable.GameBall.Position.Y -= canvasDrawable.GameBall.Speed;
                canvasDrawable.GameBall.Position.X += canvasDrawable.GameBall.Speed / 2;
                DetectSideCollision();
                break;
            default:
                break;


        }
        void DetectSideCollision()
        {
            if (canvasDrawable.GameBall.Position.X >= GameCanvasView.WidthRequest)
            {
                canvasDrawable.GameBall.Position.X -= canvasDrawable.GameBall.Speed / 2;
                switch (canvasDrawable.GameBall.Direction)
                {
                    case (BallDirection.DownRight):
                        canvasDrawable.GameBall.SwitchDirection(BallDirection.DownLeft);
                        break;
                    case (BallDirection.UpRight):
                        canvasDrawable.GameBall.SwitchDirection(BallDirection.UpLeft);
                        break;

                }
                
            }
            else if (canvasDrawable.GameBall.Position.X <=1)
            {
                canvasDrawable.GameBall.Position.X += canvasDrawable.GameBall.Speed / 2;

                switch (canvasDrawable.GameBall.Direction)
                {
                    case (BallDirection.DownLeft):
                        canvasDrawable.GameBall.SwitchDirection(BallDirection.DownRight);
                        break;
                    case (BallDirection.UpLeft):
                        canvasDrawable.GameBall.SwitchDirection(BallDirection.UpRight);
                        break;

                }
                
            }

                if (canvasDrawable.GameBall.Element.IntersectsWith(canvasDrawable.GameBat.Element))
            {
                canvasDrawable.GameBall.SwitchDirection(BallDirection.UpLeft);
            }else if (canvasDrawable.GameBricks.Any(e => e.Element.IntersectsWith(canvasDrawable.GameBall.Element)))
            {

                //canvasDrawable.GameBricks.Remove()
                if (canvasDrawable.GameBall.Direction is BallDirection.UpLeft)
                {
                    canvasDrawable.GameBall.SwitchDirection(BallDirection.DownLeft);

                }
                else
                {
                    canvasDrawable.GameBall.SwitchDirection(BallDirection.DownRight);

                }
                //todo: pop some boxes and increment user score
                
                canvasDrawable.GameBricks.Remove(canvasDrawable.GameBricks.
                    Where(elm => elm.Element.IntersectsWith(canvasDrawable.GameBall.Element)).Single());
                // todo: handle top collision
            }
        }

        //Console.WriteLine($"{canvasDrawable.GameBall.Element.Location} canvas width {GameCanvasView.GetVisualElementWindow().Width}");
    }

    private void OnGameTimerElapsed(object sender, ElapsedEventArgs e)
    {
        // loop the game
        canvasDrawable.GameBall.Speed = 20; // todo: hook it with game level
        MainThread.BeginInvokeOnMainThread(() =>
        {

            DetectCollision();
            GameCanvasView.Invalidate();
        });

    }



    private void SetupCanvas()
    {
        GameCanvasView.BackgroundColor = Colors.Bisque;
        GameCanvasView.Drawable = canvasDrawable; // get canvas instance
        GameCanvasView.DragInteraction += OnDragAction;

    }

    private void OnDragAction(object sender, TouchEventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            if (e.Touches?[0].Y >= canvasDrawable.GameBat.Element.Y) // if in Y zone
            {
                //if (e.Touches?[0].X >= GameBat.Element.Left && e.Touches?[0].X <= GameBat.Element.Right)
                {
                    //Console.WriteLine($"moving from {(int)GameBat.Element.X} to {(int)e.Touches[0].X } diff {diff}");
                    Thread.Sleep(10); //throttle
                    canvasDrawable.GameBat.Position.X = (float)(e.Touches?[0].X - canvasDrawable.GameBat.Dimension.Width / 3);
                    GameCanvasView.Invalidate();

                }
            }
        });


    }

    public override string ToString()
    {
        return $"{(int)this.Level}\t{this.GameTimer}";
    }





    // cleanup on exit
    ~Game(){

        GameTimer?.Dispose();
    }
}



public enum GameLevel
{
    Advanced,
    Intermediate,
    Easy

}

