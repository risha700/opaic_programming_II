using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using PacManApp.Models;



namespace PacManApp.GameDrawables;

public partial class CanvasDrawable : ObservableObject, IDrawable
{
    public PointF WallBrickDimensions = new();

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
        Board = new();
        Kibbles = new();
        Ghosts = new();
        PacMan = new();
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.StrokeLineCap = LineCap.Round;

        // assign canvas to reuse it
        GameCanvas = canvas; 
        CanvasDirtyRect = (RectF)dirtyRect;

        if (FirstRender)
        {
            GenerateWalls(dirtyRect);

            PacMan.Position.X = ((float)(WallBrickDimensions.X *1.1));
            PacMan.Position.Y = (float)(dirtyRect.Height - ((WallBrickDimensions.Y)+ PacMan.Dimension.Height*2));
            PacMan.Dimension.Height = (float)(WallBrickDimensions.Y * 0.80);
            PacMan.Dimension.Width = (float)(WallBrickDimensions.X * 0.80);

            FirstRender = false;
            

        }

        // draw maze walls
        foreach (var w in Walls)
        {
            canvas.FillColor = w.FillColor;
            canvas.StrokeColor = Colors.Gray;
            canvas.StrokeSize = 2;
            //canvas.SetFillPaint(w.WallPattern, w.Element);
            canvas.FillRectangle(w.Element);
            canvas.DrawRectangle(w.Element);

            //// debug only
            //canvas.FontColor = Colors.Blue;
            //canvas.FontSize = 12;

            //canvas.DrawString($"({w.Position.X},{ w.Position.Y})", w.Element.Location.X, w.Element.Location.Y, w.Dimension.Width, w.Dimension.Height, HorizontalAlignment.Center, VerticalAlignment.Top);
            //canvas.DrawString($"({w.MatrixPosition.X},{w.MatrixPosition.Y})", w.Element.Location.X, w.Element.Location.Y, w.Dimension.Width, w.Dimension.Height, HorizontalAlignment.Center, VerticalAlignment.Bottom);
        }
        canvas.ResetStroke();

        // draw kibbles
        foreach (var k in Kibbles)
        {
            //k.Element.Width = WallBrickDimensions.X;
            //k.Element.Height = WallBrickDimensions.Y;



            //canvas.FillColor = Colors.Aqua;
            //canvas.FillRectangle(k.CollissionElement);

            canvas.FillColor = k.FillColor;
            canvas.FillEllipse(k.Element);
            // super debug
            //k.Dimension.Width = WallBrickDimensions.X;
            //k.Dimension.Height = WallBrickDimensions.Y;
            //canvas.FontColor = Colors.Black;
            //canvas.DrawString($"({k.Position.X},{k.Position.Y})", k.CollissionElement.X, k.CollissionElement.Y, k.CollissionElement.Width, k.CollissionElement.Height, HorizontalAlignment.Left, VerticalAlignment.Top);
            //canvas.DrawString($"({k.MatrixPosition.X},{k.MatrixPosition.Y})", k.CollissionElement.X, k.CollissionElement.Y, k.CollissionElement.Width, k.CollissionElement.Height, HorizontalAlignment.Left, VerticalAlignment.Bottom);
        }
        canvas.ResetStroke();

        // draw ghost
        foreach (var g in Ghosts)
        {
            canvas.FillColor = g.FillColor;
            g.Render(canvas, dirtyRect);
            //canvas.FillRoundedRectangle(g.Element, 23);
        }
        // draw pacman


        PacMan.Render(canvas, dirtyRect, WallBrickDimensions);

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
                                Position=new(x,y),
                                MatrixPosition = new(row, col)
                            });
                        
                        break;

                    case 01:
                        var centerX = x + (cellWidth / 2);
                        var centerY = y + (cellHeight / 2);
                        Kibble kibble = new(x: x, y: y, matrixPos: new(row, col));
                        kibble.Element = new RectF(centerX, centerY, kibble.Dimension.Height, kibble.Dimension.Height);
                        
                        kibble.CollissionElement = new RectF(x, y, cellWidth, cellHeight);
                        Kibbles.Add(kibble);
                        break;
                    case 99:
                        var xpos = x+10;
                        var ypos = y+10;
                        Ghost ghost = new(x: xpos, y: ypos);
                        ghost.Element = new RectF(xpos, ypos, ghost.Dimension.Height, ghost.Dimension.Height);
                        Ghosts.Add(ghost);
                        break;

                    default:
                         break;
                }

            }
        }

   
    }
}

