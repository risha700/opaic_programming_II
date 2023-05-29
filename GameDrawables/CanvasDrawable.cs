using System.Collections.ObjectModel;
using BallBreaker.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Platform;

namespace BallBreaker.GameDrawables;

public partial class CanvasDrawable : ObservableObject, IDrawable
{
    private readonly uint RectHeight = 50;
    private readonly uint RectWidth = 200;
    private readonly uint BallRadius = 50;

    public PlatformCanvas GameCanvas { get; set; }
    public Ball GameBall;
    public Bat GameBat;
    public ObservableCollection<Brick> GameBricks;


    public CanvasDrawable()
    {
        GameBat = new();
        GameBall = new();
        GameBricks = new ObservableCollection<Brick>();

    }
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        GameCanvas = (PlatformCanvas)canvas;

        canvas.FillColor =(Color) GameBat.FillColor;

        float X = dirtyRect.Width / 2;
        float Y = dirtyRect.Height - RectHeight * 2;
       
        // bat
        GameBat.Element = new RectF(X, Y, RectWidth, RectHeight);
        canvas.FillRectangle(GameBat.Element);

        //ball
        canvas.StrokeColor = Colors.Violet;
        canvas.StrokeSize = 6;
        canvas.FillColor = GameBall.FillColor;
        GameBall.Element = new RectF(X, Y - 200, GameBall.Dimension.Height, GameBall.Dimension.Width);// todo: set ball size dynamic
        //var intersected = GameBall.Element.IntersectsWith(GameBat.Element);
        canvas.FillRoundedRectangle(GameBall.Element, BallRadius);



        // bricks
        // do math
        float desiredHeight =(float) dirtyRect.Height / 3;
        // how many bricks can fit in width and h
        canvas.FillColor = Colors.Green;

        for (var col = 30; col < desiredHeight/2; col += 70)
            
        {
            for (var row = 30; row < dirtyRect.Width; row += 120)
            {
                Brick brick = new Brick(x: row, y: col);
                brick.Element = new RectF(row, col, (float)(brick.Dimension.Width*0.85), (float)(brick.Dimension.Height*0.85));
                GameBricks.Add(brick);
                
                

            }
        }


        foreach (Brick brick in GameBricks)
        {
            canvas.ResetState();
            if (brick.Dimension.Width + brick.Element.X < dirtyRect.Width) // dont draw incomplete rect
            canvas.FillRectangle(brick.Element);
        }
    }

    public PlatformCanvas GetGameCanvas()
    {
        return GameCanvas;
    }
}


