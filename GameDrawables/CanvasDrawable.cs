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

    public  RectF CanvasDirtyRect { get; set; }
    public ICanvas GameCanvas { get; set; }
    public Ball GameBall;

    [ObservableProperty]
    public Bat gameBat;

    
    public ObservableCollection<Brick> GameBricks;


    public CanvasDrawable()
    {
        GameBat = new(x: (float)Shell.Current.Window.Width/2, y:(float)Shell.Current.Window.Height-100);
        GameBall = new(x: (float)Shell.Current.Window.Width / 3, y: (float)(Shell.Current.Window.Height/2.5));
        GameBricks = new ObservableCollection<Brick>();

    }
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        GameCanvas = canvas;
        CanvasDirtyRect = (RectF)dirtyRect;

        //canvas.FillColor = GameBat.FillColor;

        float X = dirtyRect.Width / 2;
        float Y = dirtyRect.Height - RectHeight * 2;

        // bat
        GameBat.Render(canvas, dirtyRect);
        //ball

        GameBall.Render(canvas, dirtyRect);

        // bricks
        // do math
        float desiredHeight = (float)dirtyRect.Height / 3;
        // how many bricks can fit in width and h
        canvas.FillColor = Colors.Green;

        if(GameBricks.Count == 0)
        {
            GenerateBricks(dirtyRect, desiredHeight);
        }
        

        foreach (Brick brick in GameBricks)
        {
            if (brick.Dimension.Width + brick.Element.X < dirtyRect.Width)
            {
                canvas.FillRectangle(brick.Element);
            } // dont draw incomplete rect
            else
            {
                GameBricks.Remove(brick);

            }
        }

        

    }

    private void GenerateBricks(RectF dirtyRect, float desiredHeight)
    {
        for (var col = 30; col < desiredHeight / 2; col += 70)

        {
            for (var row = 30; row < dirtyRect.Width; row += 120)
            {
                Brick brick = new Brick(x: row, y: col);

                brick.Element = new RectF(row, col, (float)(brick.Dimension.Width * 0.85), (float)(brick.Dimension.Height * 0.85));
                GameBricks.Add(brick);

            }
        }
    }


}


