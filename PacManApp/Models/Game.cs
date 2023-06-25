
using System.Timers;
using CommunityToolkit.Mvvm.ComponentModel;

using PacManApp.GameDrawables;
using PacManApp.ViewModels;



namespace PacManApp.Models;

public partial class Game:ObservableObject
{
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
            if (CanMoveTo(canvasDrawable.PacMan, SwipeDirection))
            {
                if (CanTurn(canvasDrawable.PacMan, SwipeDirection))
                    MovePacMan();

            }
            GameCanvasView.Invalidate();
        });
        

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

        Direction OldSwipe = SwipeDirection;

        double pressed_angle = LimitTapGesture(pac_x, pac_y, touch_x, touch_y);

        if (touch_y < pac_y && pressed_angle > 80 && pressed_angle < 120) // && can go up
        {
            SwipeDirection = Direction.Up;            
        }
        else if (touch_y > pac_y && pressed_angle > 235 && pressed_angle < 280)
        {
            SwipeDirection = Direction.Down;
        }
        else if (touch_x > pac_x && pressed_angle > 160 && pressed_angle < 199 )
        {
            SwipeDirection = Direction.Right;
        }
        else if (touch_x < pac_x && pressed_angle > 300 && pressed_angle < 360 )
        {
            SwipeDirection = Direction.Left;   
        }
        if (CanMoveTo(canvasDrawable.PacMan, SwipeDirection))
        {
            if (CanTurn(canvasDrawable.PacMan, SwipeDirection))
            MovePacMan();
        }

        if (OldSwipe != SwipeDirection)
        {
            Console.WriteLine($"CAN TURN FROM {OldSwipe} TO {SwipeDirection} - {CanTurn(canvasDrawable.PacMan, SwipeDirection)} packman width = {canvasDrawable.PacMan.Element.Width}");
            OldSwipe = SwipeDirection;
        }


        Thread.Sleep(5); //throttle


        GameCanvasView.Invalidate();

    }

    public PointF ConvertToCanvasCords(int x, int y)
    {

        float X = x * canvasDrawable.WallBrickDimensions.X;
        float Y = y * canvasDrawable.WallBrickDimensions.Y;

        return new PointF(X, Y);
  


    }

    public Point ConvertToMatrixCords(int x, int y)
    {
        int X = (int)(x / canvasDrawable.WallBrickDimensions.X);
        int Y = (int)(y / canvasDrawable.WallBrickDimensions.Y);

        return new Point((int)X, (int)Y);

    }
    public PointF ConvertToMatrixCordsF(float x, float y)
    {
        float X = x / canvasDrawable.WallBrickDimensions.X;
        float Y = y / canvasDrawable.WallBrickDimensions.Y;

        return new PointF(X, Y);

    }

    public int GetMatrixPointContent(int x, int y)
    {
        try {return canvasDrawable.Board.Matrix[x, y];}
        catch {}
        return 0;
    }



    private static double LimitTapGesture(float pac_x, float pac_y, float touch_x, float touch_y)
    {
        double angleRadians = Math.Atan2(pac_y - touch_y, pac_x - touch_x);// get the angle between touch point and pacman rect
        double angleDegrees = angleRadians * (180 / Math.PI); // hollow circle to help decide which way to go!
        var pressed_angle = (angleDegrees + 360) % 360; // normalize
        return pressed_angle;
    }

    bool CanMoveTo(dynamic obj, Direction direction, int step=1)
    {
        var maze = Board.FlipArray(canvasDrawable.Board.Matrix); // board is flipped
        //PointF matrix_cords_f = ConvertToMatrixCordsF(canvasDrawable.PacMan.Position.X, canvasDrawable.PacMan.Position.Y);
        Point matrix_cords = ConvertToMatrixCords((int)obj.Position.X, (int)obj.Position.Y);
        int nextX = (int)matrix_cords.X;
        int nextY = (int)matrix_cords.Y;
        bool canFit = false;
        PointF oldCanvasCords = ConvertToCanvasCords(nextX, nextY);
        
        
        switch (direction)
        {
            case Direction.Right:
                canFit = obj.Element.Center.X - oldCanvasCords.X >= (canvasDrawable.WallBrickDimensions.X / 2) + (obj.Element.Width / 2) - (obj.Element.Width / 3);
                nextX += step;
                break;
            case Direction.Left:
                
                canFit = obj.Element.Center.X - oldCanvasCords.X >= (canvasDrawable.WallBrickDimensions.X / 2) + (obj.Element.Width / 2) - (obj.Element.Width / 3);

                nextX -= step;
                break;
            case Direction.Up:
                canFit = obj.Element.Center.Y - oldCanvasCords.Y >= (canvasDrawable.WallBrickDimensions.Y / 2) + (obj.Element.Height / 2) - (obj.Element.Height / 3);
                nextY -= step;
                break;
            case Direction.Down:
                canFit = obj.Element.Center.Y - oldCanvasCords.Y >= (canvasDrawable.WallBrickDimensions.Y / 2) + (obj.Element.Height / 2) - (obj.Element.Height / 3);
                nextY += step;
                break;
        }
        



        if (nextX<0|| nextX>maze.GetLength(1) || nextY<0||nextY >= maze.GetLength(0))
        {
            //Console.WriteLine($"Caught in boundary lenght");
            return false;
        }


        if (maze[nextX, nextY] == 10)
        {
            //PointF newCanvasCords = ConvertToCanvasCords(nextX, nextY);
            //int content = GetMatrixPointContent(nextX, nextY);
            //var pac_elm = canvasDrawable.PacMan.Element;
            //var pac_colelm = canvasDrawable.PacMan.CollissionElement;

            //Console.WriteLine($"CANFIt {canFit}\n"); 
         
            return canFit;

        }


        return true;
    }

    bool CanEat() => canvasDrawable.Kibbles.Any(k => k.Element.IntersectsWith(canvasDrawable.PacMan.Element));


    // only allow middle turn
    // casting numbers int and back float makes 4 or 5 steps diffence depending on size of wall block
    // and where is the element placed on the window in relation to the matrix
    bool CanTurn( dynamic obj,  Direction direction)
    {

        Point matrixCords = ConvertToMatrixCords((int)obj.Position.X, (int)obj.Position.Y);
        int nextX = (int)matrixCords.X;
        int nextY = (int)matrixCords.Y;

        PointF matrixCordsF = ConvertToMatrixCordsF(obj.Position.X, obj.Position.Y);

        float realNextX = matrixCordsF.X;
        float realNextY = matrixCordsF.Y;

        //Console.WriteLine($"realNextX {realNextX} nextX {nextX} diff={realNextX-nextX:f2} GOING... {SwipeDirection} ");
        //Console.WriteLine($"realNextY {realNextY} nextY {nextY} diff={realNextY - nextY:f2} GOING... {SwipeDirection} ");
        bool canTurn = false;

        // x and y flipped because we changing direction
        switch (direction)
        {
            case Direction.Right or Direction.Left:
                canTurn = realNextY - nextY <= 0.21;
                break;
            case Direction.Up or Direction.Down:
                canTurn = realNextX - nextX <= 0.21;
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
                canvasDrawable.PacMan.Position.X += canvasDrawable.PacMan.Speed;
                canvasDrawable.PacMan.Position.Y = canvasDrawable.PacMan.Position.Y;
                break;
            case Direction.Left:
                canvasDrawable.PacMan.Position.X -= canvasDrawable.PacMan.Speed;
                canvasDrawable.PacMan.Position.Y = canvasDrawable.PacMan.Position.Y;
                break;
            case Direction.Up:
                canvasDrawable.PacMan.Position.X = canvasDrawable.PacMan.Position.X;
                canvasDrawable.PacMan.Position.Y -= canvasDrawable.PacMan.Speed;
                break;
            case Direction.Down:
                canvasDrawable.PacMan.Position.X = canvasDrawable.PacMan.Position.X;
                canvasDrawable.PacMan.Position.Y += canvasDrawable.PacMan.Speed;
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
            GamePlayer.Score += 10;
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