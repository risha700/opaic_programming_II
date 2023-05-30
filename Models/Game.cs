using System;
using System.Timers;
using BallBreaker.GameDrawables;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Graphics.Platform;

namespace BallBreaker.Models;




public class Game
{
    private const uint GAME_DURATION_MIN=3;

    public GameLevel Level { get; set; }
    public bool IsRunning { get; set; }
    public uint Round { get; set; }
	public Player CurrentPlayer { get; set; }
	
	public System.Timers.Timer GameTimer { get; set; }

    public RectF CanvasDirtyRect { get; set; }

    public GraphicsView GameCanvasView { get; set; }

    public PlatformCanvas GameCanvas { get; set; }
    
	public Game(GameLevel level=GameLevel.Easy)
	{
        // setup timer
		GameTimer = new(TimeSpan.FromMinutes(GAME_DURATION_MIN));
		GameTimer.Elapsed += new ElapsedEventHandler(OnGameTimerElapsed);
        // assign level
        Level = level;
        // create canvas view
        GameCanvasView = new GraphicsView { HeightRequest = Shell.Current.Window.Height, WidthRequest = Shell.Current.Window.Width };
        //Shell.Current.Window.Page.on
        //GameCanvasView.Handler += new IViewHandler( (object sender, EventArgs e) => { });
        //.HandlerChanged += new EventHandler((object sender, EventArgs e) => { Console.WriteLine($"event called {e}"); });
        SetupCanvas();

        

    }

    private void OnDragAction(object sender, TouchEventArgs e)
    {
        Console.WriteLine($"Drag happended, {e.Touches?[0].X} {e.Touches?[0].Y}");
        //throw new NotImplementedException();
    }

    private void DetectCollision()
    {
        //todo
    }

    private void OnGameTimerElapsed(object sender, ElapsedEventArgs e)
    {
        // loop the game
        //GameTimer.Start();
        throw new NotImplementedException($"{e.SignalTime}");
    }

    private void SetupCanvas()
    {
        GameCanvasView.BackgroundColor = Colors.Bisque;
        var canvasDrawable = new CanvasDrawable();
        GameCanvasView.Drawable = canvasDrawable;
        // hook event
        //Console.WriteLine($"GameCanvasView.Bounds : {GameCanvasView.Bounds}");
        GameCanvasView.DragInteraction += OnDragAction;
        
        //Canvas.Invalidate();
        GameCanvas = canvasDrawable.GetGameCanvas();
        
        

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

