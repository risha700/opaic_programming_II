using System.Timers;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Graphics.Platform;
using System.Linq;
using System.Collections.ObjectModel;
using Microsoft.Maui.Graphics;
using BallBreaker.GameDrawables;
using Microsoft.VisualBasic;
using BallBreaker.ViewModels;
using Plugin.Maui.Audio;

namespace BallBreaker.Models;

public partial class Game:ObservableObject
{
    private const uint GAME_DURATION_MIN=3;
    private readonly uint TIMER_ITERVALS=40;

    public GameLevel Level { get; set; }
    public bool IsRunning { get; set; }
    public uint Round { get; set; }
    public Player GamePlayer { get; set; } = new();

    [ObservableProperty]
    public TimeSpan remainingTime;

    [ObservableProperty]
    public System.Timers.Timer gameTimer;

    [ObservableProperty]
    public GameAudioViewModel audioModel;

    public RectF CanvasDirtyRect { get; set; }
    public GraphicsView GameCanvasView { get; set; } = new();

    CanvasDrawable canvasDrawable = new CanvasDrawable();

    //GameViewModel GameVm;


    public Game(GameLevel level=GameLevel.Easy)
    {
        setupTimers();
        Level = level;
        GameCanvasView = new GraphicsView { HeightRequest = Shell.Current.Window.Height*0.9, WidthRequest = Shell.Current.Window.Width };
        SetupCanvas();
        //GameVm = gameVM;
        //this.audioManager = audioManager;
        AudioModel = new GameAudioViewModel(AudioManager.Current);
    }


    private void setupTimers()
    {
        RemainingTime = TimeSpan.FromMinutes(GAME_DURATION_MIN);
        GameTimer = new(RemainingTime);
        GameTimer.Interval = TIMER_ITERVALS;
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
                break;
            case (BallDirection.DownLeft):
                canvasDrawable.GameBall.Position.Y += canvasDrawable.GameBall.Speed;
                canvasDrawable.GameBall.Position.X -= canvasDrawable.GameBall.Speed / 2;
                break;
            case (BallDirection.UpLeft):
                canvasDrawable.GameBall.Position.X -= canvasDrawable.GameBall.Speed / 2;
                canvasDrawable.GameBall.Position.Y -= canvasDrawable.GameBall.Speed;
                break;
            case (BallDirection.UpRight):
                canvasDrawable.GameBall.Position.Y -= canvasDrawable.GameBall.Speed;
                canvasDrawable.GameBall.Position.X += canvasDrawable.GameBall.Speed / 2;
                
                break;
            default:
                break;


        }
        DetectBounds();
    }


    private void DetectBounds()
    {

        if (canvasDrawable.GameBall.Position.X >= GameCanvasView.WidthRequest - canvasDrawable.GameBall.Dimension.Width) // max right
        {
            canvasDrawable.GameBall.Position.X -= canvasDrawable.GameBall.Speed / 2;
            DetectRightCollision();

        }
        else if (canvasDrawable.GameBall.Position.X <= canvasDrawable.GameBall.Dimension.Width - 1) //max left
        {
            canvasDrawable.GameBall.Position.X += canvasDrawable.GameBall.Speed / 2;
            DetectMaxLeft();
        }

        else if (canvasDrawable.GameBall.Position.Y <= (canvasDrawable.GameBall.Dimension.Height*2 + GameCanvasView.HeightRequest*0.1)) // max top 
        {
            DetectMaxTop();
            canvasDrawable.GameBall.Position.Y += canvasDrawable.GameBall.Speed / 2;
        }
        else if (canvasDrawable.GameBall.Element.IntersectsWith(canvasDrawable.GameBat.Element)) // bat
        {
            //canvasDrawable.GameBall.SwitchDirection(BallDirection.UpLeft);
            DetectBatCollision();

        }
        else if (canvasDrawable.GameBricks.Any(e => e.Element.IntersectsWith(canvasDrawable.GameBall.Element))) // bricks
        {
 
            canvasDrawable.GameBricks.Remove(canvasDrawable.GameBricks.Where(elm => elm.Element.IntersectsWith(canvasDrawable.GameBall.Element)).Single());
            DetectBrickHit();
            MainThread.BeginInvokeOnMainThread(async() =>
            {
                await AudioModel.PlayAudio("fireball");

            });
            GamePlayer.Score += 10;

        }
        else if (canvasDrawable.GameBall.Position.Y >= (canvasDrawable.GameBall.Dimension.Height*2 + GameCanvasView.HeightRequest )) // out of bounds
        {
            // lost
            AnnounceResult("Lost");

        }else if (canvasDrawable.GameBricks.Count() ==0)
        {
            AnnounceResult("Won");
        }


    }

    private void AnnounceResult(string game_state)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            GameTimer.Stop();
            if(game_state.ToLower()=="lost") await AudioModel.PlayAudio("gameover");
            if (game_state.ToLower() == "won") await AudioModel.PlayAudio("won");

            var result = await Shell.Current.DisplayAlert($"You {game_state}!", $"Your score {GamePlayer.Score}", "Replay", "Quit");



            if (result)
            {

                canvasDrawable = new();
                setupTimers();
                GamePlayer.Score = 0;
                SetupCanvas();
                GameTimer.Start();
                GameCanvasView.Invalidate();
            }
            else
            {
                App.Current.Quit();
            }
        });
    }

    private void DetectBrickHit()
    {
        switch (canvasDrawable.GameBall.Direction)
        {
            case (BallDirection.UpLeft):
                canvasDrawable.GameBall.SwitchDirection(BallDirection.DownLeft);
                break;
            case (BallDirection.UpRight):
                canvasDrawable.GameBall.SwitchDirection(BallDirection.DownRight);
                break;
            default:
                canvasDrawable.GameBall.SwitchDirection(BallDirection.DownRight);
                break;
        }
    }

    private void DetectBatCollision()
    {
        switch (canvasDrawable.GameBall.Direction)
        {
            case (BallDirection.DownRight):
                canvasDrawable.GameBall.SwitchDirection(BallDirection.UpRight);
                break;
            case (BallDirection.DownLeft):
                canvasDrawable.GameBall.SwitchDirection(BallDirection.UpLeft);
                break;

        }
    }

    private void DetectMaxTop()
    {
        switch (canvasDrawable.GameBall.Direction)
        {
            case (BallDirection.UpRight):
                canvasDrawable.GameBall.SwitchDirection(BallDirection.DownRight);
                break;
            case (BallDirection.UpLeft):
                canvasDrawable.GameBall.SwitchDirection(BallDirection.DownLeft);
                break;

        }
    }

    private void DetectMaxLeft()
    {
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

    private void DetectRightCollision()
    {
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

    private void OnGameTimerElapsed(object sender, ElapsedEventArgs e)
    {
        // loop the game
        canvasDrawable.GameBall.Speed = 20; // todo: hook it with game level
        MainThread.BeginInvokeOnMainThread(() =>
        {
            DetectCollision();
            GameCanvasView.Invalidate();
        });
        RemainingTime -= TimeSpan.FromMilliseconds(TIMER_ITERVALS);
        
    }



    private void SetupCanvas()
    {
        GameCanvasView.BackgroundColor = Colors.Bisque;
        GameCanvasView.Drawable = canvasDrawable; // set canvas instance
        GameCanvasView.DragInteraction += OnDragAction;

    }

    private void OnDragAction(object sender, TouchEventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            if (e.Touches?[0].Y >= canvasDrawable.GameBat.Element.Y) // if in Y zone
            {
                //if (e.Touches?[0].X >= GameBat.Element.Left && e.Touches?[0].X <= GameBat.Element.Right) // too hard
                {
                    //Console.WriteLine($"moving from {(int)GameBat.Element.X} to {(int)e.Touches[0].X } diff {diff}");
                    Thread.Sleep(5); //throttle
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

