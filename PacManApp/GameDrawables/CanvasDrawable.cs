using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using PacManApp.Models;



namespace PacManApp.GameDrawables;

public partial class CanvasDrawable : ObservableObject, IDrawable
{
    PointF WallBrickDimensions = new();

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
        Walls = new();
        Board = new Board();
        Kibbles = new();
        Ghosts = new();
        PacMan = new();
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.StrokeLineCap = LineCap.Round;
        GameCanvas = canvas;
        CanvasDirtyRect = (RectF)dirtyRect;

        GenerateWalls(dirtyRect);

        foreach (var w in Walls)
        {
            canvas.FillColor = w.FillColor;
            canvas.StrokeColor = Colors.Gray;
            canvas.StrokeSize = 2;
            canvas.SetFillPaint(w.WallPattern, w.Element);
            canvas.FillRectangle(w.Element);
            canvas.DrawRectangle(w.Element);
        }
        canvas.ResetStroke();

        foreach (var k in Kibbles)
        {
            canvas.FillColor = k.FillColor;
            canvas.FillEllipse(k.Element);
        }

        foreach (var g in Ghosts)
        {
            canvas.FillColor = g.FillColor;
            canvas.FillRoundedRectangle(g.Element, 23);
        }

        PacMan.Position.X = WallBrickDimensions.X + PacMan.Dimension.Width/2;
        PacMan.Position.Y = dirtyRect.Height - WallBrickDimensions.X-PacMan.Dimension.Height-5;

        PacMan.Render(canvas, dirtyRect);

    }

    private void GenerateWalls(RectF dirtyRect)
    {
        
        var maze = Board.Matrix;
        int mazeWidth = maze.GetLength(1);
        int mazeHeight = maze.GetLength(0);
        int cellSize = (int)Math.Min(dirtyRect.Width / mazeWidth, dirtyRect.Height / mazeHeight);
        int cellWidth = (int)Math.Floor(dirtyRect.Width / mazeWidth);
        int cellHeight = (int)Math.Floor(dirtyRect.Height / mazeHeight);
        int numRows = (int)(dirtyRect.Height / cellSize);
        int numCols = (int)(dirtyRect.Width / cellSize);

        WallBrickDimensions.X = cellWidth;
        WallBrickDimensions.Y = cellHeight;

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
                    case 99:
                        var xpos = x+10;
                        var ypos = y+10;
                        Ghost ghost = new(x: xpos, y: ypos);
                        ghost.Element = new RectF(xpos, ypos, ghost.Dimension.Width, ghost.Dimension.Height);
                        Ghosts.Add(ghost);
                        break;

                    default:
                         break;
                }

            }
        }

   
    }
}

