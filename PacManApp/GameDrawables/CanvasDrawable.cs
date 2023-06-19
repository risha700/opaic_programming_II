using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using PacManApp.Models;

namespace PacManApp.GameDrawables;

public partial class CanvasDrawable : ObservableObject, IDrawable
{

    public RectF CanvasDirtyRect { get; set; }
    public ICanvas GameCanvas { get; set; }


    [ObservableProperty]
    public bool firstRender = true;

    [ObservableProperty]
    public Pacman pacMan;

    //[ObservableProperty]
    public ObservableCollection<Ghost> Ghosts;

    
    public ObservableCollection<Wall> Walls;

    public CanvasDrawable()
	{
        PacMan = new(x: (float)Shell.Current.Window.Width / 3, y: (float)((Shell.Current.Window.Height * 0.9) / 2.5));
        Walls = new();
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        GameCanvas = canvas;
        CanvasDirtyRect = (RectF)dirtyRect;
        PacMan.Render(canvas, dirtyRect);

        GenerateWalls(dirtyRect);

        foreach (var w in Walls)
        {
            canvas.FillColor = w.FillColor;
            canvas.FillRectangle(w.Element);
        }
        
    }

    private void GenerateWalls(RectF dirtyRect)
    {
        // draw walls all around

        //for (var col = 30; col < desiredHeight / 2; col += 70)
        //{
        //    for (var row = 30; row < dirtyRect.Width; row += 120)
        //    {
        //        //Brick brick = new Brick(x: row, y: col, color: (Color)Color.FromRgb(random.Next(0, 255), random.Next(0, 255), (row + col) < 255 ? row + col : 100));
        //        //brick.Element = new RectF(row, col, (float)(brick.Dimension.Width * 0.85), (float)(brick.Dimension.Height * 0.85));
        //        //GameBricks.Add(brick);

        //    }
        //}
        
        Console.WriteLine($"{dirtyRect.Width} {dirtyRect.Height}");

        for (var col=0; col < dirtyRect.Height; col++)
        {
            for (var row = 0; row < dirtyRect.Width; row++)
            {
                //all borders
                if (col == 0 || row == 0 || col == dirtyRect.Height-20 || row == dirtyRect.Width-20)
                {
                    Wall wall = new Wall(x: row, y: col);
                    wall.Element = new RectF(row, col, wall.Dimension.Width, wall.Dimension.Height);
                    Walls.Add(wall);
                }

            }

        }
    }
}

