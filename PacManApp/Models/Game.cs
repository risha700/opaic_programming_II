﻿using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

using PacManApp.GameDrawables;
using PacManApp.ViewModels;


namespace PacManApp.Models;

public partial class Game:ObservableObject
{
    const int PACMAN_SPEEE= 10;

    [ObservableProperty]
    public GameAudioViewModel audioModel;

    public GraphicsView GameCanvasView { get; set; }

    CanvasDrawable canvasDrawable = new CanvasDrawable();

    public Direction SwipeDirection { get; set; } = Direction.Right;

    public List<PointF> InteractionTouches = new List<PointF>() { new PointF { } };
    
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
        GameCanvasView.StartInteraction += OnDragAction;

    }

    private void OnDragAction(object sender, TouchEventArgs e)
    {
        
        var pac_x = canvasDrawable.PacMan.Position.X;
        var pac_y = canvasDrawable.PacMan.Position.Y;
        var touch_x = e.Touches[0].X;
        var touch_y = e.Touches[0].Y;

        InteractionTouches.Add(e.Touches[0]);

        var maze = canvasDrawable.Board.Matrix;

        int maze_x = (int)(pac_x / canvasDrawable.WallBrickDimensions.X);
        int maze_y = (int)(pac_y / canvasDrawable.WallBrickDimensions.Y);
        var position = 0;


        try { position = maze[maze_x, maze_y]; } catch { }

        //Console.WriteLine($"debug===>  maze point {position} x:{maze_x} y:{maze_y}");

        double angleRadians = Math.Atan2(pac_y - touch_y, pac_x - touch_x);// get the angle between touch point and pacman rect
        double angleDegrees = angleRadians * (180 / Math.PI); // hollow circle to help decide which way to go!
        var pressed_angle = (angleDegrees + 360) % 360; // normalize

        
        //Console.WriteLine($"debug===> {pressed_angle}");

        if (touch_y < pac_y && pressed_angle > 80 && pressed_angle < 120 && CanMoveTo(Direction.Up)) // && can go up
        {
            // if can go
            // get next maze block
            
            //Console.WriteLine($"debug===>  maze next point {maze[maze_x, maze_y-1]} x:{maze_x} y:{maze_y-1}");

            SwipeDirection = Direction.Up;
            MovePacMan();
            Console.WriteLine($"debug===> should go up");


        }
        else if (touch_y > pac_y && pressed_angle > 235 && pressed_angle < 280 && CanMoveTo(Direction.Down))
        {
            SwipeDirection = Direction.Down;
            MovePacMan();
            Console.WriteLine($"debug===> should go down ");
        }
        else if (touch_x > pac_x && pressed_angle > 160 && pressed_angle < 199 && CanMoveTo(Direction.Right))
        {
            SwipeDirection = Direction.Right;
            MovePacMan();
            Console.WriteLine($"debug===> should go right ");
        }
        else if (touch_x < pac_x && pressed_angle > 300 && pressed_angle < 360 && CanMoveTo(Direction.Left))
        {

            SwipeDirection = Direction.Left;
            MovePacMan();
            Console.WriteLine($"debug===> should go left ");
        }

        Thread.Sleep(5); //throttle
        GameCanvasView.Invalidate();
    }
    bool CanMoveTo( Direction direction)
    {
        var maze = canvasDrawable.Board.Matrix;
        var pac_x = (int)canvasDrawable.PacMan.Position.X;
        var pac_y = (int)canvasDrawable.PacMan.Position.Y;
        int maze_x = (int)(pac_x / canvasDrawable.WallBrickDimensions.X);
        int maze_y = (int)(pac_y / canvasDrawable.WallBrickDimensions.Y);
        //int maze_x = (int)(pac_x / canvasDrawable.PacMan.Dimension.Width);
        //int maze_y = (int)(pac_y / canvasDrawable.PacMan.Dimension.Height);
        //var temp_rect = canvasDrawable.PacMan.Element;

        //var position = 0;

        int nextX = (int)((int)canvasDrawable.PacMan.Position.X/ (canvasDrawable.PacMan.Dimension.Height * 2));
        int nextY = (int)((int)canvasDrawable.PacMan.Position.Y / (canvasDrawable.PacMan.Dimension.Width * 2));
        //int nextX = (int)((int)canvasDrawable.PacMan.Position.X / (canvasDrawable.PacMan.Dimension.Width));
        //int nextY = (int)((int)canvasDrawable.PacMan.Position.Y / (canvasDrawable.PacMan.Dimension.Height));

        switch (direction)
        {
            case Direction.Right:
                nextX += 1;
                break;
            case Direction.Left:
                nextX -= 1;
                break;
            case Direction.Up:
                nextY -= 1;
                break;
            case Direction.Down:
                nextY += 1;
                break;
        }

        Console.WriteLine($"CAN MOVE ===>nextY {nextY} nextX {nextX} pac_x {pac_x} pac_y {pac_y}");

        //// Check if the next position is within the game boundaries
        //if (nextX < 0 || nextX >= canvasDrawable.Board.Matrix.GetLength(0) || nextY < 0 || nextY >= canvasDrawable.Board.Matrix.GetLength(1))
        //{
        //    return false; // Movement outside the game boundaries
        //}

        //// Check if the next position collides with a wall
        //if (maze[nextX, nextY] == 10)
        //{
        //    return false; // Movement collides with a wall
        //}
        return true;
        //Console.WriteLine($"devug===> next position is {position} - current position is {maze[maze_x, maze_y]} at x: {pac_x} ,y: {pac_y} - mazex:{maze_x}, mazey{maze_y}");
        //return position!=10;

        //return canvasDrawable.Walls.Any(w=> !w.Element.IntersectsWith(temp_rect));
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
            // increment score
            // play sound
        }
    }
}


public enum GameLevel
{
    Advanced,
    Intermediate,
    Easy

}