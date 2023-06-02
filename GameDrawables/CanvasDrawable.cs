using System.Collections.ObjectModel;
using BallBreaker.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Platform;

namespace BallBreaker.GameDrawables;

public partial class CanvasDrawable : ObservableObject, IDrawable
{

    public  RectF CanvasDirtyRect { get; set; }
    public ICanvas GameCanvas { get; set; }
    public Ball GameBall;

    [ObservableProperty]
    public bool firstRender=true;

    [ObservableProperty]
    public Bat gameBat;

    
    public ObservableCollection<Brick> GameBricks;


    public CanvasDrawable()
    {
        GameBat = new(x: (float)Shell.Current.Window.Width/2, y:(float)(Shell.Current.Window.Height*0.9)-100);
        GameBall = new(x: (float)Shell.Current.Window.Width / 3, y: (float)((Shell.Current.Window.Height*0.9)/2.5));
        GameBricks = new ObservableCollection<Brick>();

    }
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        
        GameCanvas = canvas;
        CanvasDirtyRect = (RectF)dirtyRect;

        //canvas.FillColor = GameBat.FillColor;

        // bat
        GameBat.Render(canvas, dirtyRect);
        //ball
        
        GameBall.Render(canvas, dirtyRect);

        // bricks
        // do math
        float desiredHeight = (float)dirtyRect.Height / 3;
        // how many bricks can fit in width and h
        //canvas.FillColor = Colors.Green;
   
        if(FirstRender && GameBricks.Count == 0)
        {
            GenerateBricks(dirtyRect, desiredHeight);
            FirstRender = false;
        }
        

        foreach (Brick brick in GameBricks)
        {
            if (brick.Dimension.Width + brick.Element.X < dirtyRect.Width)
            {
                canvas.FillColor = brick.FillColor;
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
        Random random = new();
        for (var col = 30; col < desiredHeight / 2; col += 70)

        {
            for (var row = 30; row < dirtyRect.Width; row += 120)
            {
                Brick brick = new Brick(x: row, y: col, color: (Color)Color.FromRgb(random.Next(0, 255), random.Next(0, 255), (row + col) < 255 ? row + col:100)) ;
                brick.Element = new RectF(row, col, (float)(brick.Dimension.Width * 0.85), (float)(brick.Dimension.Height * 0.85));
                GameBricks.Add(brick);

            }
        }
    }


}


