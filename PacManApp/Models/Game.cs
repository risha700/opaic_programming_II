using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Timers;
using CommunityToolkit.Mvvm.ComponentModel;

using PacManApp.GameDrawables;
using PacManApp.ViewModels;



namespace PacManApp.Models;

public partial class Game:ObservableObject
{
    const int PACMAN_SPEEE= 20;
    private readonly uint TIMER_ITERVALS = 200;


    [ObservableProperty]
    public GameAudioViewModel audioModel;

    [ObservableProperty]
    public Player gamePlayer = new();

    [ObservableProperty]
    public System.Timers.Timer gameTimer;

    public GraphicsView GameCanvasView { get; set; }

    CanvasDrawable canvasDrawable = new CanvasDrawable();

    [ObservableProperty]
    public Direction swipeDirection=Direction.Right;

    public List<PointF> InteractionTouches = new List<PointF>() { new PointF { } };
    
    public Game(GameAudioViewModel audioModel)
	{
        GameCanvasView = new GraphicsView {
            HeightRequest = Shell.Current.Window.Height * 0.85,
            WidthRequest = Shell.Current.Window.Width
        };

        setupTimers();
        SetupCanvas();

        AudioModel = audioModel;
    }
    private void SetupCanvas()
    {
        //GameCanvasView.BackgroundColor = Colors.DarkSlateBlue;
        GameCanvasView.Drawable = (IDrawable)canvasDrawable; // set canvas instance
        GameCanvasView.StartInteraction += OnDragAction;

    }
    private void setupTimers()
    {
        //RemainingTime = TimeSpan.FromMinutes(100);
        GameTimer = new (TimeSpan.FromMinutes(10));
        GameTimer.Interval = TIMER_ITERVALS;
        GameTimer.Elapsed += new ElapsedEventHandler(OnGameTimerElapsed);
        //GameTimer.Start();
        

    }

    private void OnGameTimerElapsed(object sender, ElapsedEventArgs e)
    {
        // loop the game
        //canvasDrawable.GameBall.Speed = 20; // todo: hook it with game level
        MainThread.BeginInvokeOnMainThread(() =>
        {
            //DetectCollision();
            
            MovePacMan();
            //Console.WriteLine($"timer is running");
            GameCanvasView.Invalidate();
        });
        //RemainingTime -= TimeSpan.FromMilliseconds(TIMER_ITERVALS);

    }

    private bool DetectCollision()
    {
        var maze = canvasDrawable.Board.Matrix;

        var currenScreenX = canvasDrawable.PacMan.Position.X;
        var currentScreenY = canvasDrawable.PacMan.Position.Y;

        int nextX = (int)(currenScreenX / canvasDrawable.WallBrickDimensions.X);
        int nextY = (int)(currentScreenY / canvasDrawable.WallBrickDimensions.Y);

        double realNextX = currenScreenX / canvasDrawable.WallBrickDimensions.X;
        double realNextY = (currentScreenY / canvasDrawable.WallBrickDimensions.Y);

        bool canFit = false;

        

        switch (SwipeDirection)
        {
            case Direction.Right:
                canFit = Math.Abs(realNextX - nextX) > 0.4;
                nextX += 1;
                break;
            case Direction.Left:
                canFit = Math.Abs(realNextX - nextX) > 0.4;
                nextX -= 1;
                break;
            case Direction.Up:
                //Console.WriteLine($"CanFit In UP {canFit} => {maze[nextX, nextY]} - ({nextX},{nextY}) - X-diff = {realNextX - nextX} Y-diff= {realNextY - nextY:c2}");
                canFit = Math.Abs(realNextY - nextY) > 0.4;
                nextY -= 1;
                break;
            case Direction.Down:
                canFit = Math.Abs(realNextY - nextY) > 0.4;
                nextY += 1;
                break;
        }

        Console.WriteLine($"NEXT MOVE {canFit} => {maze[nextX, nextY]} - ({nextX},{nextY}) - X-diff = {realNextX - nextX} Y-diff= {realNextY - nextY:c2}");

        
        if (nextX < 0 || nextX > maze.GetLength(1) || nextY < 0 || nextY >= maze.GetLength(0))
        {
            return false;
        }


        if (maze[nextX, nextY] == 10)
        {
            // now we need real x and y on screen

            //Console.WriteLine($"Caught 10 {canFit} => {maze[nextX, nextY]} - ({nextX},{nextY}) - X-diff = {realNextX - nextX} Y-diff= {realNextY - nextY:c2}");



            float nextSxreenX = nextX * canvasDrawable.WallBrickDimensions.X;
            float nextScreenY = nextY * canvasDrawable.WallBrickDimensions.Y;
            Console.WriteLine($"Caught 10 nextScreen {nextSxreenX} {nextScreenY} currenScreen {currenScreenX} {currentScreenY}");

            return canFit;

        }


        return true;

    }

    private void OnDragAction(object sender, TouchEventArgs e)
    {

        var pac_x = canvasDrawable.PacMan.Position.X;
        var pac_y = canvasDrawable.PacMan.Position.Y;
        var touch_x = e.Touches[0].X;
        var touch_y = e.Touches[0].Y;

        InteractionTouches.Add(e.Touches[0]);

        var OldSwipe = SwipeDirection;

        double pressed_angle = LimitTapGesture(pac_x, pac_y, touch_x, touch_y);

        if (touch_y < pac_y && pressed_angle > 80 && pressed_angle < 120) // && can go up
        {
            SwipeDirection = Direction.Up;            
            //Console.WriteLine($"debug===> should go up");
        }
        else if (touch_y > pac_y && pressed_angle > 235 && pressed_angle < 280)
        {
            SwipeDirection = Direction.Down;
            //Console.WriteLine($"debug===> should go down ");
        }
        else if (touch_x > pac_x && pressed_angle > 160 && pressed_angle < 199 )
        {
            SwipeDirection = Direction.Right;
            //Console.WriteLine($"debug===> should go right ");
        }
        else if (touch_x < pac_x && pressed_angle > 300 && pressed_angle < 360 )
        {
            SwipeDirection = Direction.Left;   
            //Console.WriteLine($"debug===> should go left ");
        }
        if (CanMoveTo(SwipeDirection))
        {
            MovePacMan();

        }

        if (OldSwipe != SwipeDirection)
        {
            Console.WriteLine($"CAN TURN {SwipeDirection} - {CanTurn(SwipeDirection)}");
            OldSwipe = SwipeDirection;
        }


        Thread.Sleep(5); //throttle


        GameCanvasView.Invalidate();

    }

    private static double LimitTapGesture(float pac_x, float pac_y, float touch_x, float touch_y)
    {
        double angleRadians = Math.Atan2(pac_y - touch_y, pac_x - touch_x);// get the angle between touch point and pacman rect
        double angleDegrees = angleRadians * (180 / Math.PI); // hollow circle to help decide which way to go!
        var pressed_angle = (angleDegrees + 360) % 360; // normalize
        return pressed_angle;
    }

    bool CanMoveTo(Direction direction, int step=1)
    {
        var maze = canvasDrawable.Board.Matrix;

        var currenScreenX = canvasDrawable.PacMan.Position.X;
        var currentScreenY = canvasDrawable.PacMan.Position.Y;

        int nextX = (int)(currenScreenX / canvasDrawable.WallBrickDimensions.X);
        int nextY = (int)(currentScreenY / canvasDrawable.WallBrickDimensions.Y);

        float realNextX = currenScreenX / canvasDrawable.WallBrickDimensions.X;
        float realNextY = currentScreenY / canvasDrawable.WallBrickDimensions.Y;

        Rect nextElm = new Rect { Location = new (realNextX, realNextY) , Size = canvasDrawable.PacMan.Element.Size };

        //Console.WriteLine($"element is touching {canvasDrawable.Walls.Any(w => w.Element.IntersectsWith(canvasDrawable.PacMan.CollissionElement))}");
        bool canFit = false;


        //return !canvasDrawable.Walls.Any(w => w.Element.IntersectsWith(nextElm));
        Console.WriteLine($"\nCURRENT MOVE => {maze[nextX, nextY]} - ({nextX},{nextY}) \n" +
            $"double value {realNextX}, {realNextY}\n X-diff = {realNextX - nextX:f2} Y-diff= {realNextY - nextY:f2}");
        //Console.WriteLine($"CURRENT MOVE => {maze[nextX, nextY]} - ({nextX},{nextY}) double value {realNextX} X-diff = {realNextX - nextX:f2} ");
        switch (direction)
        {
            case Direction.Right:
                canFit = realNextX - nextX > 0.25;
                //if(canFit)
                    nextX += 1;
                break;
            case Direction.Left:
                canFit = realNextX - nextX > 0.25;
                //if (canFit)
                    nextX -= 1;
                break;
            case Direction.Up:
                canFit = realNextY - nextY > 0.25;
                //Console.WriteLine($"CanFit In UP {canFit} => {maze[nextX, nextY]} - ({nextX},{nextY}) - X-diff = {realNextX - nextX:f2} Y-diff= {realNextY - nextY:f2}");
                //if (canFit)
                    nextY -= 1;
                break;
            case Direction.Down:
                canFit = realNextY - nextY > 0.25;
                //if (canFit)
                    nextY += 1;
                break;
        }

        Console.WriteLine($"NEXT MOVE can fit {canFit} on {direction} => {maze[nextX, nextY]} - ({nextX},{nextY}) - X-diff = {realNextX-nextX:f2} Y-diff= {realNextY-nextY :f2}");

        
        if (nextX<0|| nextX>maze.GetLength(1) || nextY<0||nextY >= maze.GetLength(0))
        {
            return false;
        }


        if (maze[nextX, nextY] == 10)
        {
            // now we need real x and y on screen

            //Console.WriteLine($"Caught 10 {canFit} => {maze[nextX, nextY]} - ({nextX},{nextY}) - X-diff = {realNextX - nextX} Y-diff= {realNextY - nextY:c2}");



            float nextSxreenX = nextX * canvasDrawable.WallBrickDimensions.X;
            float nextScreenY = nextY * canvasDrawable.WallBrickDimensions.Y;
            Console.WriteLine($"Caught 10 nextScreen {nextSxreenX} {nextScreenY} currenScreen {currenScreenX} {currentScreenY}");

            return canFit;

        }


        return true;
    }

    bool CanEat() => canvasDrawable.Kibbles.Any(k => k.Element.IntersectsWith(canvasDrawable.PacMan.Element));


    // only allow middle turn
    // casting numbers int and back float makes 4 or 5 steps diffence depending on size of wall block
    // and where is the element placed on the window in relation to the matrix
    bool CanTurn( Direction direction)
    {
        

        var currenScreenX = canvasDrawable.PacMan.Position.X;
        var currentScreenY = canvasDrawable.PacMan.Position.Y;

        int nextX = (int)(currenScreenX / canvasDrawable.WallBrickDimensions.X);
        int nextY = (int)(currentScreenY / canvasDrawable.WallBrickDimensions.Y);

        double realNextX = currenScreenX / canvasDrawable.WallBrickDimensions.X;
        double realNextY = currentScreenY / canvasDrawable.WallBrickDimensions.Y;

        bool canTurn = false;

        switch (direction)
        {
            case Direction.Right:
                canTurn = realNextX - nextX < 0.2;
                break;
            case Direction.Left:
                canTurn = realNextX - nextX < 0.2;
                break;
            case Direction.Up:
                canTurn = realNextY - nextY < 0.2;
                break;
            case Direction.Down:
                canTurn = realNextY - nextY < 0.2;
                break;
        }

        return canTurn;
    }





    void MovePacMan()
    {
        // dont go if the next move intersects with walls
        canvasDrawable.PacMan.Direction = SwipeDirection;

        switch (SwipeDirection)
        {
            case Direction.Right:
                canvasDrawable.PacMan.Position.X += PACMAN_SPEEE;
                canvasDrawable.PacMan.Position.Y = canvasDrawable.PacMan.Position.Y;
                break;
            case Direction.Left:
                canvasDrawable.PacMan.Position.X -= PACMAN_SPEEE;
                canvasDrawable.PacMan.Position.Y = canvasDrawable.PacMan.Position.Y;
                break;
            case Direction.Up:
                canvasDrawable.PacMan.Position.X = canvasDrawable.PacMan.Position.X;
                canvasDrawable.PacMan.Position.Y -= PACMAN_SPEEE;
                break;
            case Direction.Down:
                canvasDrawable.PacMan.Position.X = canvasDrawable.PacMan.Position.X;
                canvasDrawable.PacMan.Position.Y += PACMAN_SPEEE;
                break;
        }

        // eat
        if (canvasDrawable.Kibbles.Any(k=>canvasDrawable.PacMan.Element.IntersectsWith(k.Element)))
        {
            canvasDrawable.Kibbles.Remove(canvasDrawable.Kibbles.Where(x=>x.Element.IntersectsWith(canvasDrawable.PacMan.Element)).Single());

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                canvasDrawable.PacMan.IsEating = true;
                GameCanvasView.Invalidate();
                await AudioModel.PlayAudio("fireball");
                await Task.Delay(50);
                canvasDrawable.PacMan.IsEating = false;
            });

            // increment score
            GamePlayer.Score += 1;
            // play sound
        }
    }

    ~Game()
    {

        GameTimer?.Dispose();
    }
}


public enum GameLevel
{
    Advanced,
    Intermediate,
    Easy

}