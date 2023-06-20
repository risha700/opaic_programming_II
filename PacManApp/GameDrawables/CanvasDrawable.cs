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

    public Board Board;

    public ObservableCollection<Wall> Walls;
    public ObservableCollection<Kibble> Kibbles;

    public CanvasDrawable()
    {
        PacMan = new(x: (float)Shell.Current.Window.Width / 3, y: (float)((Shell.Current.Window.Height * 0.9) / 2.5));
        Walls = new();
        Board = new Board();
        Kibbles = new();
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.StrokeLineCap = LineCap.Round;
        GameCanvas = canvas;
        CanvasDirtyRect = (RectF)dirtyRect;
        PacMan.Render(canvas, dirtyRect);

        // generate game buffer like
        try
        {
            GenerateWalls(dirtyRect);

        }catch (Exception ex)
        {
            Console.WriteLine($"Exception===> {ex.Message} {ex.InnerException}");
        }


        foreach (var w in Walls)
        {
            canvas.FillColor = w.FillColor;
            canvas.StrokeColor = Colors.DarkGray;
            canvas.StrokeSize = 4;
            canvas.SetFillPaint(w.WallPattern, w.Element);
            canvas.FillRectangle(w.Element);
            //canvas.DrawRectangle(w.Element);
        }
        canvas.ResetStroke();
        foreach (var k in Kibbles)
        {
            canvas.FillColor = k.FillColor;
            canvas.FillRectangle(k.Element);
        }
    }

    private void GenerateWalls(RectF dirtyRect)
    {
        
        var maze = Board.Matrix;
        int mazeWidth = maze.GetLength(1);
        int mazeHeight = maze.GetLength(0);
        int cellSize = (int)Math.Min(dirtyRect.Width / mazeWidth, dirtyRect.Height / mazeHeight);
        int cellWidth = (int)Math.Floor(dirtyRect.Width / mazeWidth);
        int cellHeight = (int)Math.Floor(dirtyRect.Height / mazeHeight);

        // Calculate the number of rows and columns based on the cell size and canvas dimensions
        int numRows = (int)(dirtyRect.Height / cellSize);
        int numCols = (int)(dirtyRect.Width / cellSize);

        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numCols; col++)
            {

                // Determine the cell position in the canvas
                int x = col * cellWidth;
                int y = row * cellHeight;

                var position=0;

                try{ position = maze[row, col];}catch{} // for the unbalanced maze :(

                switch (position)
                {
                    case 10:
                        Walls.Add(
                            new Wall {
                                Element=new RectF(x,y,cellWidth, cellHeight),
                                Dimension=new(cellWidth, cellHeight),
                                Position=new(x,y)
                            });
                        break;

                    case 01:

                        var newX = x + (cellWidth / 2);
                        var newY = y + (cellHeight / 2);
                        Kibble kibble = new(x: newX, y: newY);
                        kibble.Element = new RectF(newX, newY, kibble.Dimension.Width, kibble.Dimension.Height);
                        Kibbles.Add(kibble);
                        break;

                    default:
                         break;
                }

            }
        }

   
    }
}

